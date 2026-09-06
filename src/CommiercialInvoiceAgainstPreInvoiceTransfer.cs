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
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
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
namespace Architecture.WinApp.Export
{

public class CommiercialInvoiceAgainstPreInvoiceTransfer : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private int GdBreakUpRecId;

	private int MultiInvoiceRecId;

	private int UpdateDetailIndexForMultiInvoiceToGd = -1;

	private int contractidInupdateCase;

	private bool UpdateMode;

	private int updateDetailIndex;

	private int updateDetailIndexOther = -1;

	private int updateDetailIndexGdBreakUp;

	private int updateDetailIndexAdvanceUtilize;

	private int VoucherHeadId;

	private int InvoiceUpdateFlag;

	private int CommissionDebitAcId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private string AdvanceUtilizeIds = "";

	private DataTable dtDetailForMultiInvoiceForm = new DataTable();

	private DataTable dtdetail4update = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtGdBreakUp = new DataTable();

	private DataTable dtAdvanceUtilize = new DataTable();

	private DataTable dtFiList = new DataTable();

	private DataTable dtIN = new DataTable();

	private DataTable dtpcktype = new DataTable();

	private LoadForwardingForCommercialInvoice LoadInvoice;

	private List<ExImInvoicePackingDetail> lstRemoveRecordDetail = new List<ExImInvoicePackingDetail>();

	private List<ExImInvoiceBankGDBreakUp> lstRemoveRecordGdBreakUpDetail = new List<ExImInvoiceBankGDBreakUp>();

	private List<GDBreakUpDetail> lstRemoveRecordOfInvoiceFromGd = new List<GDBreakUpDetail>();

	private IContainer components;

	private Panel panel4;

	private Panel panel5;

	private Panel panel1;

	private Panel panel7;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel8;

	private GroupBox HeaderGropBox;

	internal UltraCombo cmbSupCust;

	private Label label21;

	private Label label3;

	private DateTimePicker txtDocDate;

	private TextBox txtdocno;

	private Label label5;

	private Label label1;

	private Label label43;

	internal UltraCombo cmbproject;

	private Label label18;

	internal UltraCombo cmbbranches;

	private Label label13;

	internal GridEX grdDetail;

	private Panel panel10;

	private Label label23;

	private Panel panel12;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private Panel panel13;

	internal GridEX DataGridHistory;

	private Panel panel15;

	private Label label69;

	private Panel panel16;

	private ToolStrip toolStrip3;

	private ToolStripButton btnSlipHistory;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo cmbFCurrencr;

	internal UltraCombo cmbPaymentTerm;

	internal UltraCombo combranches;

	internal UltraCombo comproject;

	internal UltraCombo ultraCombo3;

	internal UltraCombo CmbLCContractor;

	internal UltraCombo ultraCombo2;

	internal UltraCombo ultraCombo1;

	internal UltraCombo cmbCarierType;

	internal UltraCombo cmbExporterBank;

	internal UltraCombo cmbImporterBank;

	internal UltraCombo cmbEFormNo;

	internal UltraCombo cmbdeliveryterm;

	private ToolStrip miniToolStrip;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton toolStripButton4;

	private ToolStripButton toolStripButton5;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip2;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label4;

	internal UltraCombo cmbNotifyParty2;

	private Label label10;

	internal UltraCombo cmbNotifyParty1;

	private Label label8;

	private Label label7;

	private TextBox txtLotRef;

	private Label label9;

	private Label label11;

	private Label label12;

	internal UltraCombo cmbexporterBankNew;

	internal UltraCombo cmbimporterBankNew;

	internal UltraCombo cmbPaymentTermsNew;

	internal UltraCombo cmbdeliverytermnew;

	private Label label24;

	private Label label19;

	private Label label26;

	private Label label56;

	internal UltraCombo cmbDestinationPort;

	internal UltraCombo cmbLoadingPort;

	private Label label16;

	private Label label41;

	private Label label47;

	internal UltraCombo cmbcareiertype;

	private Label label22;

	private TextBox txtfcyAmount;

	private Label label25;

	private Label label27;

	internal UltraCombo cmbfcycode;

	private TextBox txtLocalAmt;

	private Label label42;

	private TextBox txtExRate;

	private Label label44;

	private Label label45;

	private DateTimePicker txtIformDate;

	private TextBox txtGdNo;

	private Label label49;

	private Label label46;

	private TextBox txtGrossWeight;

	private Label label54;

	private TextBox txtNetWeight;

	private Label label55;

	private TextBox txtNoOfContainer;

	private Label label57;

	private Label label53;

	private Label label50;

	private Label label31;

	private Label label60;

	private Label label62;

	private TextBox txtcertificateOfOrigion;

	private Label label65;

	private TextBox txtTotalNetAmount;

	private Label label66;

	private TextBox txtaddlessamount;

	private Label label64;

	private Label label40;

	private TextBox txtaddlesscommnets;

	private Label label67;

	private Label label68;

	private CheckBox ChkPrintPreview;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel22;

	private Panel panel24;

	private Panel panel23;

	private Panel panel17;

	private Panel panel11;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private GroupBox GroupDetail;

	private Label label72;

	internal UltraCombo CmbCropYear;

	private TextBox txtCostMTon;

	private Label label29;

	private TextBox txtNoOfBags;

	private Label label30;

	internal UltraCombo combitem;

	private Label label32;

	internal UltraCombo combpcktype;

	private Label label33;

	private TextBox txtQtyMTon;

	private Button btnCancel;

	private Label label35;

	private Button btnGridUpdate;

	private Button btnAddinGrid;

	internal UltraCombo CmbPackUom;

	private Label label36;

	private TextBox txtAmount;

	private Label label37;

	private Label label38;

	internal UltraCombo CmbRateUom;

	private Panel panel9;

	private TabPage tabPage4;

	private Panel panel19;

	internal GridEX grdotheritems;

	private Panel panel18;

	private Button btnoCancel;

	private Button btnoUpdate;

	private Button btnoadd;

	private Label label14;

	private TextBox txtoRemarks;

	private TextBox txtoQty;

	private Label label15;

	private TextBox txtoRate;

	private Label label63;

	private Label label70;

	private TextBox txtoAmount;

	internal UltraCombo CmbOtherItem;

	private Label label71;

	private TabPage tabPage5;

	private Panel panel21;

	internal GridEX grdpaymentdetail;

	private Panel panel20;

	private Label label82;

	private Label label81;

	private Label label80;

	private Label label79;

	private Button btndcancel;

	private Button btndupdate;

	private Button btndadd;

	private TextBox dtxtdueday;

	private Label label78;

	private TextBox dtxtfcyamount;

	private Label label77;

	internal UltraCombo dcmbpaymentterm;

	private Label label76;

	private TextBox txtBalanceFI;

	private Label label83;

	private TextBox dtxtpercentoftotal;

	private Label label74;

	internal UltraCombo CmbFinInstruments;

	private Label label75;

	private Label label85;

	private Label label84;

	private Label label51;

	private Label label2;

	private Panel panel2;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	private TextBox txtgrossWeightdetail;

	private Label label86;

	private TextBox txtPackingWeight;

	private Label label59;

	private TextBox txtNetWeightdetail;

	private Label label87;

	private TextBox txttotalpackingweight;

	private Label label88;

	internal GridEX gridDetailHistory;

	private Panel panel3;

	private Label label89;

	private TextBox txtCommAmount;

	private TextBox txtCommPercent;

	internal UltraCombo cmbCommissionAgent;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnLoaderForm;

	private ToolStripButton btnrefersh;

	private TextBox txtPackingExpiryDate;

	private Label label6;

	private Label label20;

	private TextBox txtremarksheader;

	private TextBox txtweightcheck;

	private TextBox txtqtycheck;

	private TextBox txtNoOfBagsPerCntnr;

	private Label label58;

	private TextBox txtNoOfContainersDetail;

	private Label label48;

	private TextBox txtcertificate;

	private Label label61;

	internal UltraCombo CmbPreInvoice;

	private Label label17;

	internal UltraCombo CmbCommissionDebitAccount;

	private Label label28;

	private ToolStripButton btnVoucherPrint;

	private GroupBox groupBox2;

	private Label label52;

	internal UltraCombo cmbOtherDestinationPort;

	private Label label73;

	internal UltraCombo cmbOtherCustomer;

	private GroupBox groupBox1;

	private Label label91;

	private Label label94;

	private Label label95;

	private TextBox txtOtherAmountDetil;

	private Label label34;

	private TextBox txtOtherHSCodeDetail;

	private Label label39;

	private TextBox txtOtherRateDetail;

	private Label label90;

	private TextBox txtItemHsCodeDetail;

	private Label label92;

	private ToolStripButton btnPrint548;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton btnGenerateInvoiceNo;

	internal UltraCombo Cmbinvoiceno;

	private Label label93;

	private TextBox txtItemCommodityDetail;

	private TextBox txtHealthPermitnodetail;

	private Label label96;

	private TextBox txtProductionNo;

	private Label label97;

	private CheckBox chk548Print;

	private TabPage tabPage7;

	private Panel panel26;

	private Panel panel28;

	private Panel panel27;

	private ToolStrip toolStrip1;

	private ToolStripButton toolStripButton6;

	private ToolStripButton toolStripButton8;

	private ToolStripButton toolStripButton9;

	private ToolStripButton toolStripButton10;

	internal GridEX grdGdBreakUp;

	private Panel panel14;

	private Label label101;

	private Label label99;

	private DateTimePicker datInvoiceDateGdBreakUp;

	private DateTimePicker datGDDateGDBreakUp;

	private Label label100;

	private TextBox txtInvoiceNoGDBreakUp;

	private Label label102;

	private TextBox txtGDNoBreakUp;

	private Button btnCancelGdBreakUp;

	private Button btnUpdateGdBreakUp;

	private Button btnAddGDBreakUp;

	private Label label107;

	private TextBox txtGDValue;

	private Panel panel25;

	private CtrlGrdBar ctrlGrdBar5;

	internal UltraCombo CmbInvoiceGdBreakUp;

	private Label label98;

	private Panel panel29;

	internal GridEX grdAdvanceUtlize;

	private Panel panel32;

	internal UltraCombo CmbFIAdvanceUtlize;

	private Label label108;

	private TextBox txtGdValueAdvanceUtlize;

	private Label label109;

	private Button btnAddAdvanceUtlize;

	private Button btnUpdateAdvanceUtilize;

	private Label label111;

	private Button btnCancelAdvanceUtilize;

	private TextBox txtFIBalanceAdvanceUtlize;

	private Label label112;

	private TextBox txtAdvanceUtlize;

	internal UltraCombo CmbGdNoAdvanceUtilize;

	private Label label115;

	private Panel panel33;

	private CtrlGrdBar ctrlGrdBar6;

	private ToolStrip toolStrip5;

	private ToolStripButton toolStripButton11;

	private ToolStripButton toolStripButton12;

	private ToolStripButton toolStripButton13;

	private ToolStripButton toolStripButton14;

	private Panel panel31;

	private Panel panel30;

	private Label label113;

	private Label label114;

	private Label lblInvoiceBalanceGdBreakUp;

	internal UltraCombo CmbInvoiceAdvanceUtilize;

	private Label label103;

	private TabPage GDHistory;

	internal GridEX grdGDhistory;

	private Panel panel34;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label104;

	internal UltraCombo CmbBankName;

	private Label label105;

	private TextBox txtFreight;

	private Label label110;

	private TextBox txtDueDays;

	private Label label106;

	private TextBox txtFobValue;

	private Label label116;

	private TextBox txtExchangeRate;

	private Label label119;

	private TextBox txtNetAmount;

	private Label label118;

	private TextBox txtFTTPrcnt;

	private Label label117;

	private TextBox txtFttAmount;

	private Label label120;

	private TextBox txtRemarks;

	private Label label121;

	private TabPage InvoicesAllocationToGd;

	private ToolStrip toolStrip6;

	private ToolStripButton btnResetMultiInvoiceGd;

	private ToolStripButton btnRefreshMultiInvoiceGd;

	private ToolStripButton btnSaveMultiInvoiceGd;

	private ToolStripButton btnUpdateMultiInvoiceGd;

	private Panel panel35;

	private Panel panel36;

	private Label label122;

	private CtrlGrdBar ctrlGrdBar8;

	private GroupBox groupBox3;

	private TextBox txtExchangeRateMultiInvoiceForm;

	private Label label123;

	private TextBox txtDueDaysMultiInvoiceForm;

	private Label label124;

	internal UltraCombo CmbBankMultiInvoiceForm;

	private Label label125;

	private TextBox GdNoMultiInvoiceForm;

	private Label label126;

	private TextBox BankInvoiceNoMultiInvoiceForm;

	private Label label127;

	private DateTimePicker GdDateMultiInvoiceForm;

	private Label label128;

	private DateTimePicker BankInvoiceDateMultiInvoiceForm;

	private Label label129;

	private GroupBox groupBox4;

	private TextBox txtRemarksMultiInvoiceForm;

	private Label label130;

	private TextBox FttAmountMultiInvoiceForm;

	private Label label131;

	private TextBox txtNetAmountMultiInvoiceForm;

	private Label label132;

	private TextBox FttPrcntMultiInvoiceForm;

	private Label label133;

	private TextBox txtFobValueMultiInvoiceForm;

	private Label label134;

	private TextBox txtFreightMultiInvoiceForm;

	private Label label135;

	private TextBox txtGdValueMultiInvoiceForm;

	private Label label136;

	private Button btnAddMultiInvoiceForm;

	private Button btnUpdateDetailMultiInvoiceForm;

	private Button btnCancelDetailMultiInvoiceForm;

	private TextBox txtInvoiceAmountMultiInvoiceForm;

	private Label label138;

	internal UltraCombo CmbInvoiceNoMultiInvoiceForm;

	private Label label137;

	private Panel panel37;

	private TextBox txtDetailRemarksMultiInvoiceToGdForm;

	private Label label139;

	private Panel panel38;

	private Label label140;

	private CtrlGrdBar ctrlGrdBar9;

	private Panel panel39;

	private Panel panel40;

	private Label label141;

	internal GridEX grdMultiInvoiceAgainstGd;

	private Panel panel41;

	private GroupBox groupBox5;

	private Button btnShowMultiInvoicesToGdForm;

	internal UltraCombo CmbBankMultiInvoiceAgainstGdHistory;

	private Label label142;

	private DateTimePicker ToDateMultiInvoiceAgainstGd;

	private Label label145;

	private DateTimePicker FromDateMultiInvoiceAgainstGd;

	private Label label146;

	internal GridEX DataGridHistoryMultiInvoiceToGdForm;

	private Button btnRefreshMultiInvoiceToGdHistory;

	private Button btnResetMultiInvoiceToGdHistory;

	private ToolStrip toolStrip7;

	private ToolStripButton btnResetGdBreakupHistory;

	private ToolStripButton btnRefreshGdBreakupHistory;

	private Panel panel42;

	private GroupBox groupBox6;

	private Button btnShowGdBreakUpHistory;

	internal UltraCombo CmbBankGdBankHistory;

	private Label label143;

	private DateTimePicker ToDateGdBreakupHistory;

	private Label label144;

	private DateTimePicker FromDateGdBreakupHistory;

	private Label label147;

	private Panel panel43;

	private Panel panel44;

	private GroupBox groupBox7;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label148;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label149;

	private Label label150;

	private Label label151;

	private Label label152;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label153;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnGdBreakupManual;

	private ToolStripButton BtnPaymentDetail;

	private TabControl tabControl3;

	private TabPage tabPage6;

	private TabPage tabPage8;

	internal GridEX grdAdvanceHistory;

	private TextBox txtRateWithoutAddLess;

	private Label label155;

	private TextBox txtAddLessRate;

	private Label label154;

	internal UltraCombo cmbPrefixType;

	private Label label156;

	private ToolStripButton btnBankSlipHistory;

	public CommiercialInvoiceAgainstPreInvoiceTransfer(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void projectGetAll()
	{
		try
		{
			DataTable dtproject = CommonServices.ProjectServiceBind();
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, cmbproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)cmbproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void branchesGetAll()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, cmbbranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)cmbbranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void generateCode()
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
			int Code = ExImInvoice.GenerateCode(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 211,
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

	private void CustomerGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable Supplierlst = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Supplierlst.Rows.Count > 0)
			{
				DDL.BindDDLNew(Supplierlst, cmbSupCust, "Id", "CompanyName", "Supplier Name", false);
				DDL.BindDDLNew(Supplierlst, cmbNotifyParty1, "Id", "CompanyName", "Notify Party", false);
				DDL.BindDDLNew(Supplierlst, cmbOtherCustomer, "Id", "CompanyName", "Bank Customer/Importer", false);
				DDL.BindDDLNew(Supplierlst, cmbNotifyParty2, "Id", "CompanyName", "Notify Party", false);
				DDL.BindDDLNew(Supplierlst, cmbCommissionAgent, "Id", "CompanyName", "Commission Agent", false);
				((UltraGridBase)cmbSupCust).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbNotifyParty1).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbCommissionAgent).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbOtherCustomer).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			DataTable dtDeliveryTerms = ExImDeliveryTerm.Getall();
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, cmbdeliverytermnew, "Id", "Code", "DeliveryTerms", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPreInvoice()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			DataTable dtPreIN = ExImInvoice.GetPreInvoicenumberForCommercialInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 209,
				SupplierCustomerId = Conversion.ToInt(cmbSupCust.Value)
			});
			if (dtPreIN.Rows.Count > 0)
			{
				DDL.BindDDL(dtPreIN, CmbPreInvoice, "Id", "InvoiceNo", "Pre-Invoice_Number", ZeroIndex: false);
				((UltraGridBase)CmbPreInvoice).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((UltraGridBase)CmbPreInvoice).DataSource = null;
				((Control)(object)CmbPreInvoice).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPrefixType()
	{
		try
		{
			DataTable dt = ExportPrefixType.FormHistory();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbPrefixType, "Id", "PrefixDescription", "Prefix Type", ZeroIndex: false);
				((UltraGridBase)cmbPrefixType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)cmbPrefixType).Text = string.Empty;
				((UltraGridBase)cmbPrefixType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCommercialInvoiceNofromlookup(string InvoiceNo = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = GenerateExportInvoiceNos.GetFinalInvoiceNosForCommercialInvoice(new GenerateExportInvoiceNos
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinalInvoiceNo = InvoiceNo
			});
			if (dt.Rows.Count > 0)
			{
				dtIN.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtIN.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["MainId"], dt.Rows[i]["PrefixTypeId"], dt.Rows[i]["FinalInvoiceNo"]);
				}
				DDL.BindDDL(dtIN, Cmbinvoiceno, "Id", "InvoiceNo", "Commercial-Invoice_Number", ZeroIndex: false);
				((UltraGridBase)Cmbinvoiceno).DisplayLayout.Bands[0].Columns[1].Hidden = true;
				((UltraGridBase)Cmbinvoiceno).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)Cmbinvoiceno).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDeliveryTerms = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, cmbPaymentTermsNew, "Id", "LcOrderTerm", "PaymentTerms", false);
				DDL.BindDDLNew(dtDeliveryTerms, dcmbpaymentterm, "Id", "LcOrderTerm", "Payment Term ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPortFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbLoadingPort, "Id", "PortName", "Loading Port", false);
				DDL.BindDDLNew(dt, cmbDestinationPort, "Id", "PortName", "Destination Port", false);
				DDL.BindDDLNew(dt, cmbOtherDestinationPort, "Id", "PortName", "Bank Destination Port", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
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
				DDL.BindDDLNew(dt, cmbfcycode, "Id", "CurrencyName", "CurrencyName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImporterandExportBankFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<Bank> lst = Bank.GetAll(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("BranchName");
			DataTable dtimporter = new DataTable();
			dtimporter.Columns.Add("Id");
			dtimporter.Columns.Add("BranchName");
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].IsHomeland == "Foreign Country")
				{
					dtimporter.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
				if (lst[i].IsHomeland == "Home Country")
				{
					dt.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbexporterBankNew, "Id", "BranchName", "ExporterBank", ZeroIndex: false);
				DDL.BindDDL(dt, CmbBankName, "Id", "BranchName", "Bank Name", ZeroIndex: false);
				DDL.BindDDL(dt, CmbBankMultiInvoiceForm, "Id", "BranchName", "Bank Name", ZeroIndex: false);
			}
			if (dtimporter.Rows.Count > 0)
			{
				DDL.BindDDL(dtimporter, cmbimporterBankNew, "Id", "BranchName", "ImporterBank", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CarierType()
	{
		try
		{
			DataTable dtcariertype = new DataTable();
			dtcariertype.Columns.Add("Id");
			dtcariertype.Columns.Add("Type");
			dtcariertype.Rows.Add(1, "By Sea");
			dtcariertype.Rows.Add(2, "By Air");
			dtcariertype.Rows.Add(3, "By Road");
			DDL.BindDDL(dtcariertype, cmbcareiertype, "Id", "Type", "Carier type", ZeroIndex: false);
			((UltraGridBase)cmbcareiertype).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderStatus()
	{
		try
		{
			DataTable dtstatus = new DataTable();
			dtstatus.Columns.Add("Id");
			dtstatus.Columns.Add("Status");
			dtstatus.Rows.Add(1, "Open");
			dtstatus.Rows.Add(2, "Complete");
			dtstatus.Rows.Add(3, "Cancel");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			DataTable dtUOM = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(combitem.Value));
			if (dtUOM.Rows.Count > 0)
			{
				DDL.BindDDL(dtUOM, CmbPackUom, "Id", "UOMCode", "Pack Uom", ZeroIndex: false);
				DDL.BindDDL(dtUOM, CmbRateUom, "Id", "UOMCode", "Rate Uom", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemPackingTypeFill()
	{
		try
		{
			DataTable lst = CommonServices.InvPackingTypeGetAllService();
			if (lst.Rows.Count > 0)
			{
				dtpcktype.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtpcktype.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["PackTypeDesc"], 0);
				}
				if (dtpcktype.Rows.Count > 0)
				{
					DDL.BindDDL(dtpcktype, combpcktype, "Id", "Description", "Packing Type", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		try
		{
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCropYear, "Id", "CropYear", "CropYear", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCommissionDebitAccount()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,12,13,14,20,21");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCommissionDebitAccount, "Id", "AccountTitle", "Commission DebitAccount", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindOtherItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImLcOrder.GetOtherItemByContractId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, CmbOtherItem, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindFinancialInstrument()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtFiList = ExImEFormRegistration.GetFINoForInvoice(obj);
			if (dtFiList.Rows.Count > 0)
			{
				DDL.BindDDL(dtFiList, CmbFinInstruments, "Id", "EFormNo", "FI No", ZeroIndex: false);
				DDL.BindDDL(dtFiList, CmbFIAdvanceUtlize, "Id", "EFormNo", "FI No", ZeroIndex: false);
				((UltraGridBase)CmbFinInstruments).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbFinInstruments).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbFinInstruments).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbFIAdvanceUtlize).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbFIAdvanceUtlize).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbFIAdvanceUtlize).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHeaderInformPreInvoiceByPreInvoiceId(int ExImInvoiceId)
	{
		try
		{
			DataTable dtInvoice = ExImInvoice.GetPreInvoiceDataByPreInvoiceId(ExImInvoiceId);
			if (dtInvoice.Rows.Count > 0)
			{
				cmbSupCust.Value = dtInvoice.Rows[0]["SupplierCustomerId"];
				cmbfcycode.Value = dtInvoice.Rows[0]["FCurrencyId"];
				txtExRate.Text = dtInvoice.Rows[0]["ConversionRate"].ToString();
				cmbLoadingPort.Value = dtInvoice.Rows[0]["LoadingPortId"];
				cmbPaymentTermsNew.Value = dtInvoice.Rows[0]["PaymentTermId"];
				cmbDestinationPort.Value = dtInvoice.Rows[0]["DestinationPortId"];
				cmbdeliverytermnew.Value = dtInvoice.Rows[0]["DeliveryTermId"];
				cmbOtherCustomer.Value = dtInvoice.Rows[0]["OtherCustomerId"];
				cmbOtherDestinationPort.Value = dtInvoice.Rows[0]["OtherDestinationPortId"];
				if (Conversion.ToInt(dtInvoice.Rows[0]["NotifyParty1"]) > 0)
				{
					cmbNotifyParty1.Value = dtInvoice.Rows[0]["NotifyParty1"];
				}
				if (Conversion.ToInt(dtInvoice.Rows[0]["NotifyParty2"]) > 0)
				{
					cmbNotifyParty2.Value = dtInvoice.Rows[0]["NotifyParty2"];
				}
				txtNoOfContainer.Text = dtInvoice.Rows[0]["NoOfContainers"].ToString();
				txtLotRef.Text = dtInvoice.Rows[0]["LotReference"].ToString();
				cmbCommissionAgent.Value = dtInvoice.Rows[0]["CommissionAgentId"];
				txtCommPercent.Text = dtInvoice.Rows[0]["CommRate"].ToString();
				txtCommAmount.Text = dtInvoice.Rows[0]["CommAmount"].ToString();
				((Control)(object)cmbSupCust).Enabled = false;
				((Control)(object)cmbfcycode).Enabled = false;
				txtoRemarks.Text = dtInvoice.Rows[0]["RemarksHeader"].ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dcmbfino_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbFinInstruments).ActiveRow != null && Conversion.ToInt(CmbFinInstruments.Value) > 0)
			{
				dcmbpaymentterm.Value = Conversion.ToInt(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["PaymenttermId"].Value);
				GetFinancialInstrumentsBalance();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImProformaInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			dtpcktype.Columns.Add("id");
			dtpcktype.Columns.Add("Description");
			dtpcktype.Columns.Add("PackingWeight");
			dtIN.Columns.Add("Id", typeof(int));
			dtIN.Columns.Add("MainId", typeof(int));
			dtIN.Columns.Add("PrefixTypeId", typeof(int));
			dtIN.Columns.Add("InvoiceNo");
			CmbPreInvoice.Focus();
			generateCode();
			BindPreInvoice();
			BindPrefixType();
			CustomerGetAll();
			DeliveryTermFill();
			PaymentTermsFill();
			BindCommercialInvoiceNofromlookup();
			LoadingPortFill();
			MultiCurrencyfill();
			ImporterandExportBankFill();
			CarierType();
			ItemPackingTypeFill();
			CropYearFill();
			BindCommissionDebitAccount();
			CommissionDebitAcId = ExImInvoice.GetLastCommissionDebitAccountId(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (CommissionDebitAcId > 0)
			{
				CmbCommissionDebitAccount.Value = CommissionDebitAcId;
			}
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("PerformaId", typeof(int));
			dtdetail.Columns.Add("PerformaDetailId", typeof(int));
			dtdetail.Columns.Add("PerformaDate", typeof(DateTime));
			dtdetail.Columns.Add("PRI-RefNo", typeof(string));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("HSCode");
			dtdetail.Columns.Add("PackTypeId", typeof(int));
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("PackingWeight", typeof(double));
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("NoOfContainers", typeof(double));
			dtdetail.Columns.Add("NoOfBags/Cntnr", typeof(double));
			dtdetail.Columns.Add("PackSizeId", typeof(int));
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("NoOfBags", typeof(double));
			dtdetail.Columns.Add("ValidateBags", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("ValidateNetWeight", typeof(double));
			dtdetail.Columns.Add("TotalPackingWeight", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("RateWithoutAddLess", typeof(double));
			dtdetail.Columns.Add("AddLessRate", typeof(double));
			dtdetail.Columns.Add("Cost/M.Ton", typeof(double));
			dtdetail.Columns.Add("ContractRate", typeof(double));
			dtdetail.Columns.Add("RateUOMId", typeof(int));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("OtherRate", typeof(double));
			dtdetail.Columns.Add("OtherAmount", typeof(double));
			dtdetail.Columns.Add("OtherHSCode");
			dtdetail.Columns.Add("PackingExpiryDate");
			dtdetail.Columns.Add("ProductionNo");
			dtdetail.Columns.Add("ItemCommodityDetail");
			dtdetail.Columns.Add("HealthPermitNoDetail");
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			dtOtherItem.Columns.Add("ItemId");
			dtOtherItem.Columns.Add("ItemName");
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("Rate", typeof(double));
			dtOtherItem.Columns.Add("Amount", typeof(double));
			dtOtherItem.Columns.Add("Remarks");
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			dtPaymentTerm.Columns.Add("Id");
			dtPaymentTerm.Columns.Add("DocumentTypeId");
			dtPaymentTerm.Columns.Add("ExImEFormRegistrationId");
			dtPaymentTerm.Columns.Add("FinancialInstrumentNo");
			dtPaymentTerm.Columns.Add("PaymentTermId");
			dtPaymentTerm.Columns.Add("PaymentTerm");
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(double));
			dtPaymentTerm.Columns.Add("FcyAmount", typeof(double));
			dtPaymentTerm.Columns.Add("DueDays");
			grdpaymentdetail.DataSource = dtPaymentTerm;
			grdpaymentdetail.RetrieveStructure();
			grdSettingsPaymentTerm();
			GetBankInvoicesForGdBreakUp();
			GetInvoicesagainstAdvances();
			dtGdBreakUp.Columns.Add("Id");
			dtGdBreakUp.Columns.Add("InvoiceId");
			dtGdBreakUp.Columns.Add("PartyInvoiceNo");
			dtGdBreakUp.Columns.Add("GDNo");
			dtGdBreakUp.Columns.Add("GDDate", typeof(DateTime));
			dtGdBreakUp.Columns.Add("InvoiceNo", typeof(string));
			dtGdBreakUp.Columns.Add("InvoiceDate", typeof(DateTime));
			dtGdBreakUp.Columns.Add("OtherAmount", typeof(double));
			dtGdBreakUp.Columns.Add("BankId", typeof(int));
			dtGdBreakUp.Columns.Add("BankName", typeof(string));
			dtGdBreakUp.Columns.Add("DueDays", typeof(int));
			dtGdBreakUp.Columns.Add("ExchangeRate", typeof(double));
			dtGdBreakUp.Columns.Add("GDValue", typeof(double));
			dtGdBreakUp.Columns.Add("Freight", typeof(double));
			dtGdBreakUp.Columns.Add("FOBValue", typeof(double));
			dtGdBreakUp.Columns.Add("FTT%", typeof(double));
			dtGdBreakUp.Columns.Add("FttAmount", typeof(double));
			dtGdBreakUp.Columns.Add("NetAmount", typeof(double));
			dtGdBreakUp.Columns.Add("Remarks", typeof(string));
			grdGdBreakUp.DataSource = dtGdBreakUp;
			grdGdBreakUp.RetrieveStructure();
			grdSettingsGdBreakUp();
			dtAdvanceUtilize.Columns.Add("Id");
			dtAdvanceUtilize.Columns.Add("InvoiceId");
			dtAdvanceUtilize.Columns.Add("InvoiceNo");
			dtAdvanceUtilize.Columns.Add("GDId");
			dtAdvanceUtilize.Columns.Add("GDNo");
			dtAdvanceUtilize.Columns.Add("DocumentTypeId");
			dtAdvanceUtilize.Columns.Add("FIId");
			dtAdvanceUtilize.Columns.Add("FINo");
			dtAdvanceUtilize.Columns.Add("UtilizeAmount", typeof(double));
			grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
			grdAdvanceUtlize.RetrieveStructure();
			grdSettingsAdvanceUtilize();
			FromDateGdBreakupHistory.Value = DateTime.Now;
			ToDateGdBreakupHistory.Value = DateTime.Now;
			HistoryComboFillForGdBreakupHistory();
			dtDetailForMultiInvoiceForm.Columns.Add("Id");
			dtDetailForMultiInvoiceForm.Columns.Add("InvoiceId");
			dtDetailForMultiInvoiceForm.Columns.Add("InvoiceNo");
			dtDetailForMultiInvoiceForm.Columns.Add("InvoiceAmount", typeof(double));
			dtDetailForMultiInvoiceForm.Columns.Add("Remarks");
			grdMultiInvoiceAgainstGd.DataSource = dtDetailForMultiInvoiceForm;
			grdMultiInvoiceAgainstGd.RetrieveStructure();
			grdSettingsForMultiInvoiceForm();
			FromDateMultiInvoiceAgainstGd.Value = DateTime.Now;
			ToDateMultiInvoiceAgainstGd.Value = DateTime.Now;
			GetBankInvoicesForMultiInvoiceToGdForm();
			HistoryComboFillForMultiInvoicesToGdForm();
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			ChkPrintPreview.Checked = true;
			tabControl2.SelectedIndex = 0;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailFormReset()
	{
		try
		{
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combpcktype).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtRateWithoutAddLess.Text = string.Empty;
			txtAddLessRate.Text = string.Empty;
			txtCostMTon.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtOtherHSCodeDetail.Text = string.Empty;
			txtOtherRateDetail.Text = string.Empty;
			txtOtherAmountDetil.Text = string.Empty;
			txtPackingExpiryDate.Text = string.Empty;
			txtItemHsCodeDetail.Text = string.Empty;
			txtHealthPermitnodetail.Text = string.Empty;
			txtItemCommodityDetail.Text = string.Empty;
			txtNoOfContainersDetail.Text = string.Empty;
			txtNoOfBagsPerCntnr.Text = string.Empty;
			txtweightcheck.Text = string.Empty;
			txtqtycheck.Text = string.Empty;
			txttotalpackingweight.Text = string.Empty;
			txtPackingWeight.Text = string.Empty;
			txtNetWeightdetail.Text = string.Empty;
			txtgrossWeightdetail.Text = string.Empty;
			txtProductionNo.Text = string.Empty;
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			combitem.ReadOnly = false;
			tabControl2.SelectedIndex = 0;
			if (CommissionDebitAcId > 0)
			{
				CmbCommissionDebitAccount.Value = CommissionDebitAcId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormReset()
	{
		try
		{
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)Cmbinvoiceno).Enabled = true;
			BindCommercialInvoiceNofromlookup();
			((Control)(object)cmbPrefixType).Text = string.Empty;
			UpdateMode = false;
			RecId = 0;
			contractidInupdateCase = 0;
			generateCode();
			((UltraGridBase)cmbcareiertype).Rows[0].Activate();
			txtDocDate.Value = DateTime.Now;
			((Control)(object)CmbPreInvoice).Text = string.Empty;
			((Control)(object)Cmbinvoiceno).Text = string.Empty;
			((Control)(object)cmbNotifyParty1).Text = string.Empty;
			((Control)(object)cmbNotifyParty2).Text = string.Empty;
			((Control)(object)cmbSupCust).Text = string.Empty;
			((Control)(object)cmbPaymentTerm).Text = string.Empty;
			((Control)(object)cmbimporterBankNew).Text = string.Empty;
			((Control)(object)cmbdeliverytermnew).Text = string.Empty;
			((Control)(object)cmbCommissionAgent).Text = string.Empty;
			((Control)(object)cmbLoadingPort).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			txtcertificate.Text = string.Empty;
			txtCommPercent.Text = string.Empty;
			txtCommAmount.Text = string.Empty;
			txtcertificateOfOrigion.Text = string.Empty;
			txtLotRef.Text = string.Empty;
			((Control)(object)cmbexporterBankNew).Text = string.Empty;
			((Control)(object)cmbfcycode).Text = string.Empty;
			txtfcyAmount.Text = string.Empty;
			txtExRate.Text = string.Empty;
			txtLocalAmt.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			txtGdNo.Text = string.Empty;
			txtaddlesscommnets.Text = string.Empty;
			txtaddlessamount.Text = string.Empty;
			txtTotalNetAmount.Text = string.Empty;
			dtdetail4update.Rows.Clear();
			dtdetail.Rows.Clear();
			grdDetail.ClearStructure();
			grdpaymentdetail.ClearStructure();
			updateDetailIndexOther = -1;
			((Control)(object)combitem).Text = string.Empty;
			((UltraGridBase)combitem).DataSource = null;
			cmbSupCust.Focus();
			DetailFormReset();
			ResePaymentDetails();
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecordDetail.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetOtherItems()
	{
		try
		{
			((Control)(object)CmbOtherItem).Text = string.Empty;
			txtoQty.Text = string.Empty;
			txtoRate.Text = string.Empty;
			txtoAmount.Text = string.Empty;
			txtoRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResePaymentDetails()
	{
		try
		{
			updateDetailIndexOther = -1;
			((Control)(object)CmbFinInstruments).Text = string.Empty;
			((Control)(object)dcmbpaymentterm).Text = string.Empty;
			dtxtpercentoftotal.Text = Conversion.ToInt((object)0).ToString();
			dtxtfcyamount.Text = Conversion.ToInt((object)0).ToString();
			dtxtdueday.Text = Conversion.ToInt((object)0).ToString();
			CmbFinInstruments.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetGdBreakUpDetails()
	{
		try
		{
			GdBreakUpRecId = 0;
			((Control)(object)CmbBankName).Text = string.Empty;
			txtGDNoBreakUp.Text = string.Empty;
			txtInvoiceNoGDBreakUp.Text = string.Empty;
			txtFreight.Clear();
			txtGDValue.Text = string.Empty;
			txtDueDays.Clear();
			txtFobValue.Clear();
			txtFTTPrcnt.Text = "10";
			txtFttAmount.Clear();
			txtNetAmount.Clear();
			txtRemarks.Clear();
			txtGDNoBreakUp.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetGdAdvanceUtilizeDetails()
	{
		try
		{
			txtAdvanceUtlize.Text = string.Empty;
			CmbGdNoAdvanceUtilize.Focus();
			BindFinancialInstrument();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Expected O, but got Unknown
		//IL_0e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6f: Expected O, but got Unknown
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5d: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() <= 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			double NetWeightkg = 0.0;
			NetWeightkg = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2)) * 1000.0;
			txtNetWeight.Text = NetWeightkg.ToString();
			ExImInvoice PIH = new ExImInvoice();
			if (!formvalidation())
			{
				return;
			}
			if (RecId > 0)
			{
				PIH.Id = RecId;
			}
			PIH.OrganizationId = UserAccount.OrganizationId;
			PIH.CompanyId = UserAccount.CompanyId;
			PIH.BranchesId = UserAccount.BranchesId;
			PIH.IsApproved = false;
			PIH.EntryDate = DateTime.Now;
			PIH.EntryUser = UserAccount.ID;
			PIH.ModifyDate = DateTime.Now;
			PIH.ModifyUser = UserAccount.ID;
			PIH.ApprovedDate = DateTime.Now;
			PIH.DocumentTypeId = 211;
			PIH.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PIH.BranchesId = Conversion.ToInt(cmbbranches.Value);
			PIH.ProjectsId = Conversion.ToInt(cmbproject.Value);
			PIH.DocCode = Conversion.ToInt((object)txtdocno.Text);
			PIH.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			PIH.PrefixTypeId = Conversion.ToInt(cmbPrefixType.Value);
			PIH.InvoiceNo = Conversion.ToString((object)((Control)(object)Cmbinvoiceno).Text.Trim());
			PIH.PreInvoiceId = Conversion.ToInt(CmbPreInvoice.Value);
			PIH.SupplierCustomerId = Conversion.ToInt(cmbSupCust.Value);
			PIH.NotifyParty1 = Conversion.ToInt(cmbNotifyParty1.Value);
			PIH.NotifyParty2 = Conversion.ToInt(cmbNotifyParty2.Value);
			PIH.LotNoRef = Conversion.ToString((object)txtLotRef.Text);
			PIH.PaymentTermId = Conversion.ToInt(cmbPaymentTermsNew.Value);
			PIH.ImporterBankId = Conversion.ToInt(cmbimporterBankNew.Value);
			PIH.ExporteBankId = Conversion.ToInt(cmbexporterBankNew.Value);
			PIH.DeliveryTermId = Conversion.ToInt(cmbdeliverytermnew.Value);
			PIH.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			PIH.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			PIH.OtherDestinationPortId = Conversion.ToInt(cmbOtherDestinationPort.Value);
			PIH.OtherCustomerId = Conversion.ToInt(cmbOtherCustomer.Value);
			PIH.CarierType = Conversion.ToString((object)((Control)(object)cmbcareiertype).Text);
			PIH.CommissionAmount = Conversion.ToDouble((object)txtCommAmount.Text.Trim());
			PIH.CommissionAgentId = Conversion.ToInt(cmbCommissionAgent.Value);
			PIH.CommissionPercentage = Conversion.ToDouble((object)txtCommPercent.Text.Trim());
			if (PIH.CommissionAmount > 0.0)
			{
				PIH.CommissionDebitAcId = Conversion.ToInt(CmbCommissionDebitAccount.Value);
			}
			PIH.FcurrencyId = Conversion.ToInt(cmbfcycode.Value);
			PIH.FCurrencyAmount = Conversion.ToDouble((object)txtfcyAmount.Text.Trim());
			PIH.ConversionRate = Conversion.ToDouble((object)txtExRate.Text.Trim());
			PIH.EquivalentAmount = Conversion.ToDouble((object)txtLocalAmt.Text.Trim());
			PIH.GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			PIH.NetWeight = Conversion.ToDouble((object)txtNetWeight.Text.Trim());
			PIH.NoOfContainers = Conversion.ToInt((object)txtNoOfContainer.Text.Trim());
			PIH.EFormNo = Conversion.ToString((object)txtGdNo.Text.Trim());
			PIH.EFormDate = Conversion.ToDateTime((object)txtIformDate.Value);
			PIH.Certificate2 = Conversion.ToString((object)txtcertificateOfOrigion.Text.Trim());
			PIH.Certificate1 = Conversion.ToString((object)txtcertificate.Text.Trim());
			PIH.AddLessComments = Conversion.ToString((object)txtaddlesscommnets.Text);
			PIH.AddLessAmount = Conversion.ToDouble((object)txtaddlessamount.Text.Trim());
			PIH.TotalAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
			PIH.RemarksHeader = Conversion.ToString((object)txtoRemarks.Text.Trim());
			PIH.ExImInvoiceDetail = new List<ExImInvoicePackingDetail>();
			PIH.ExImInvoiceOtherItemslist = new List<ExImInvoiceOtherItems>();
			PIH.ExImInvoicePaymentTermsDetail = new List<ExImInvoicePaymentTermsDetail>();
			double TotalFcAmount = 0.0;
			if (RecId > 0 && lstRemoveRecordDetail.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecordDetail.Count; i++)
				{
					PIH.ExImInvoiceDetail.Add(lstRemoveRecordDetail[i]);
				}
			}
			string ContractIds = "";
			string ContractDetailIds = "";
			string ContractNos = "";
			string ContractDates = "";
			double TotalBankAmount = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				ExImInvoicePackingDetail vd2 = new ExImInvoicePackingDetail();
				vd2.Id = Conversion.ToInt(r3.Cells["Id"].Value);
				vd2.ContractId = Conversion.ToInt(r3.Cells["PerformaId"].Value);
				ContractIds = ContractIds + "," + Conversion.ToString(r3.Cells["PerformaId"].Value);
				ContractDetailIds = ContractDetailIds + "," + Conversion.ToString(r3.Cells["PerformaDetailId"].Value);
				ContractNos = ContractNos + "," + Conversion.ToString(r3.Cells["PRI-RefNo"].Value);
				ContractDates = ContractDates + "," + Conversion.ToDateTime(r3.Cells["PerformaDate"].Value).ToShortDateString();
				vd2.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				vd2.HsCode = Conversion.ToString(r3.Cells["HSCode"].Value);
				vd2.PackingMaterialTypeId = Conversion.ToInt(r3.Cells["PackTypeId"].Value);
				vd2.PackingWeight = Conversion.ToDouble(r3.Cells["PackingWeight"].Value);
				vd2.CropYearId = Conversion.ToInt(r3.Cells["CropYearId"].Value);
				vd2.NoofContainers = Conversion.ToDouble(r3.Cells["NoOfContainers"].Value);
				vd2.NoofBagsPerContainer = Conversion.ToDouble(r3.Cells["NoOfBags/Cntnr"].Value);
				vd2.OuterQtyUomId = Conversion.ToInt(r3.Cells["PackSizeId"].Value);
				vd2.InnerQtyUomId = Conversion.ToInt(r3.Cells["PackSizeId"].Value);
				vd2.MTon = Conversion.ToDouble(r3.Cells["Qty/M.Ton"].Value);
				vd2.OuterQty = Conversion.ToDouble(r3.Cells["NoOfBags"].Value);
				vd2.InnerQty = Conversion.ToInt(r3.Cells["NoOfBags"].Value);
				vd2.NetWeight = Conversion.ToDouble(r3.Cells["NetWeight"].Value);
				vd2.TotalPackingWeight = Conversion.ToDouble(r3.Cells["TotalPackingWeight"].Value);
				vd2.GrossWeight = Conversion.ToDouble(r3.Cells["GrossWeight"].Value);
				vd2.RateWithoutAddLess = Conversion.ToDouble(r3.Cells["RateWithoutAddLess"].Value);
				vd2.RateAddLess = Conversion.ToDouble(r3.Cells["AddLessRate"].Value);
				vd2.RatePrice = Conversion.ToDouble(r3.Cells["Cost/M.Ton"].Value);
				vd2.RateUomId = Conversion.ToInt(r3.Cells["RateUOMId"].Value);
				vd2.FcAmount = Conversion.ToDouble(r3.Cells["Amount"].Value);
				vd2.OtherRate = Conversion.ToDouble(r3.Cells["OtherRate"].Value);
				vd2.OtherAmount = Conversion.ToDouble(r3.Cells["OtherAmount"].Value);
				TotalBankAmount += vd2.OtherAmount;
				if (vd2.OtherRate > 0.0 && vd2.OtherAmount == 0.0)
				{
					throw new Exception("Bank Amount Can't Be Zero in Row#" + (r3.RowIndex + 1));
				}
				TotalFcAmount += vd2.FcAmount;
				vd2.OtherHsCode = Conversion.ToString((object)r3.Cells["OtherHSCode"].Text);
				vd2.PackingExpiryDate = Conversion.ToString((object)r3.Cells["PackingExpiryDate"].Text);
				vd2.ProductionNo = Conversion.ToString((object)r3.Cells["ProductionNo"].Text);
				if (vd2.PackingExpiryDate == null || vd2.PackingExpiryDate == string.Empty)
				{
					throw new Exception("Packing ExpiryDate field is required in Row#" + (r3.RowIndex + 1));
				}
				if (vd2.ProductionNo == null || vd2.PackingExpiryDate == string.Empty)
				{
					throw new Exception("Production No field is required in Row#" + (r3.RowIndex + 1));
				}
				vd2.ItemCommodityDetail = Conversion.ToString((object)r3.Cells["ItemCommodityDetail"].Text);
				vd2.HealthPermitNoDetail = Conversion.ToString((object)r3.Cells["HealthPermitNoDetail"].Text);
				vd2.ActionTypeId = ((vd2.Id <= 0) ? 1 : 2);
				PIH.ExImInvoiceDetail.Add(vd2);
			}
			if (!string.IsNullOrEmpty(ContractIds))
			{
				string[] distinctContractIds = ContractIds.Split(',').Distinct().ToArray();
				if (distinctContractIds.Length == 1)
				{
					PIH.ContractIds = distinctContractIds[0];
				}
				else
				{
					PIH.ContractIds = string.Join(",", distinctContractIds).TrimStart(',');
				}
			}
			if (!string.IsNullOrEmpty(ContractDetailIds))
			{
				string[] distinctContractDetailIds = ContractDetailIds.Split(',').Distinct().ToArray();
				if (distinctContractDetailIds.Length == 1)
				{
					PIH.ContractDetailIds = distinctContractDetailIds[0];
				}
				else
				{
					PIH.ContractDetailIds = string.Join(",", distinctContractDetailIds).TrimStart(',');
				}
			}
			if (!string.IsNullOrEmpty(ContractNos))
			{
				string[] distinctContractNos = ContractNos.Split(',').Distinct().ToArray();
				if (distinctContractNos.Length == 1)
				{
					PIH.ContractNos = distinctContractNos[0];
				}
				else
				{
					PIH.ContractNos = string.Join(",", distinctContractNos).TrimStart(',');
				}
			}
			if (!string.IsNullOrEmpty(ContractDates))
			{
				string[] distinctContractDates = ContractDates.Split(',').Distinct().ToArray();
				if (distinctContractDates.Length == 1)
				{
					PIH.ContractDates = distinctContractDates[0];
				}
				else
				{
					PIH.ContractDates = string.Join(",", distinctContractDates).TrimStart(',');
				}
			}
			double OtherAmount = 0.0;
			double UtilizeAmount = 0.0;
			if (grdotheritems.RowCount > 0)
			{
				rows = grdotheritems.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToDecimal(r2.Cells["Qty"].Value) > 0m)
					{
						ExImInvoiceOtherItems vd = new ExImInvoiceOtherItems();
						vd.otherItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
						vd.oItemQty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
						vd.oItemRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						vd.oItemAmount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
						OtherAmount += Conversion.ToDouble((object)vd.oItemAmount);
						vd.OtherItemRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						PIH.ExImInvoiceOtherItemslist.Add(vd);
					}
				}
			}
			if (grdpaymentdetail.RowCount > 0)
			{
				rows = grdpaymentdetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImInvoicePaymentTermsDetail vdp = new ExImInvoicePaymentTermsDetail();
					vdp.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vdp.DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					vdp.ExImEFormRegistrationId = Conversion.ToInt(r.Cells["ExImEFormRegistrationId"].Value);
					vdp.FinancialInstrumentNo = Conversion.ToString((object)r.Cells["FinancialInstrumentNo"].Text);
					vdp.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
					vdp.PrcntOfTotal = Conversion.ToDouble(r.Cells["%OfTotal"].Value);
					vdp.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
					vdp.FcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					UtilizeAmount += vdp.FcyAmount;
					vdp.SortNo = r.RowIndex + 1;
					PIH.ExImInvoicePaymentTermsDetail.Add(vdp);
				}
			}
			PIH.TotalAmount = TotalFcAmount + PIH.AddLessAmount + OtherAmount;
			if (grdpaymentdetail.GetRows().Length != 0 && UtilizeAmount > TotalBankAmount)
			{
				throw new Exception("UtilizeAmount cannot be greater than TotalBankAmount");
			}
			PIH.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			PIH.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImInvoice>(PIH, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = ExImInvoice.Save(PIH);
			if (RecId == 0)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (RecId > 0)
			{
				MessageBox.Show("Update SuccessFully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, PIH.DocumentTypeId, base.Name);
			}
			if (ChkPrintPreview.Checked)
			{
				InvoiceSlip549(success);
			}
			if (chk548Print.Checked)
			{
				CommonServices.ExportPreCommercialInvoiceSlip548(success);
			}
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
		VoucherHeadId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value);
	}

	public void ReadById(int Id)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("Name");
			btnsave.Visible = false;
			btnupdate.Visible = true;
			lstRemoveRecordDetail.Clear();
			ExImInvoice PIH = new ExImInvoice();
			RecId = Id;
			PIH = ExImInvoice.GetByID(RecId);
			if (PIH != null)
			{
				tabControl1.SelectedIndex = 0;
				cmbbranches.Value = PIH.BranchesId;
				cmbproject.Value = PIH.ProjectsId;
				txtdocno.Text = PIH.DocCode.ToString();
				txtDocDate.Value = Conversion.ToDateTime((object)PIH.DocDate);
				BindCommercialInvoiceNofromlookup(PIH.InvoiceNo);
				cmbPrefixType.Value = PIH.PrefixTypeId;
				((Control)(object)Cmbinvoiceno).Text = PIH.InvoiceNo.ToString();
				CmbPreInvoice.Value = PIH.PreInvoiceId;
				cmbSupCust.Value = PIH.SupplierCustomerId;
				cmbCommissionAgent.Value = PIH.CommissionAgentId;
				CmbCommissionDebitAccount.Value = PIH.CommissionDebitAcId;
				txtCommPercent.Text = PIH.CommissionPercentage.ToString();
				txtCommAmount.Text = PIH.CommissionAmount.ToString();
				cmbDestinationPort.Value = PIH.DestinationPortId;
				cmbOtherCustomer.Value = PIH.OtherCustomerId;
				cmbOtherDestinationPort.Value = PIH.OtherDestinationPortId;
				contractidInupdateCase = PIH.LcOrderNoId;
				if (PIH.NotifyParty1 > 0)
				{
					cmbNotifyParty1.Value = PIH.NotifyParty1;
				}
				if (PIH.NotifyParty2 > 0)
				{
					cmbNotifyParty2.Value = PIH.NotifyParty2;
				}
				if (PIH.ImporterBankId > 0)
				{
					cmbimporterBankNew.Value = PIH.ImporterBankId;
				}
				if (PIH.ExporteBankId > 0)
				{
					cmbexporterBankNew.Value = PIH.ExporteBankId;
				}
				txtLotRef.Text = PIH.LotNoRef;
				cmbPaymentTermsNew.Value = PIH.PaymentTermId;
				cmbdeliverytermnew.Value = PIH.DeliveryTermId;
				cmbLoadingPort.Value = PIH.LoadingPortId;
				cmbDestinationPort.Value = PIH.DestinationPortId;
				((Control)(object)cmbcareiertype).Text = PIH.CarierType;
				cmbfcycode.Value = PIH.FcurrencyId;
				txtfcyAmount.Text = PIH.FCurrencyAmount.ToString();
				txtExRate.Text = PIH.ConversionRate.ToString();
				txtLocalAmt.Text = PIH.EquivalentAmount.ToString();
				txtGrossWeight.Text = PIH.GrossWeight.ToString();
				txtNetWeight.Text = PIH.NetWeight.ToString();
				txtNoOfContainer.Text = PIH.NoOfContainers.ToString();
				txtGdNo.Text = PIH.EFormNo;
				txtIformDate.Value = PIH.EFormDate;
				txtcertificate.Text = PIH.Certificate1;
				txtcertificateOfOrigion.Text = PIH.Certificate2;
				txtaddlesscommnets.Text = PIH.AddLessComments;
				txtaddlessamount.Text = PIH.AddLessAmount.ToString();
				txtTotalNetAmount.Text = PIH.TotalAmount.ToString();
				BindOtherItems();
				dtdetail.Rows.Clear();
				for (int k = 0; k < PIH.ExImInvoiceDetail.Count; k++)
				{
					dtItem.Rows.Add(PIH.ExImInvoiceDetail[k].ItemId, PIH.ExImInvoiceDetail[k].ItemName);
					dtdetail.Rows.Add(PIH.ExImInvoiceDetail[k].Id, PIH.ExImInvoiceDetail[k].ContractId, PIH.ExImInvoiceDetail[k].ContractDetailId, PIH.ExImInvoiceDetail[k].ProformaDocDate, PIH.ExImInvoiceDetail[k].PriRefNo, PIH.ExImInvoiceDetail[k].ItemId, PIH.ExImInvoiceDetail[k].ItemName, PIH.ExImInvoiceDetail[k].OtherHsCode, PIH.ExImInvoiceDetail[k].PackingMaterialTypeId, PIH.ExImInvoiceDetail[k].PackMaterilaType, PIH.ExImInvoiceDetail[k].PackingWeight, PIH.ExImInvoiceDetail[k].CropYearId, PIH.ExImInvoiceDetail[k].CropYear, PIH.ExImInvoiceDetail[k].NoofContainers, PIH.ExImInvoiceDetail[k].NoofBagsPerContainer, PIH.ExImInvoiceDetail[k].OuterQtyUomId, PIH.ExImInvoiceDetail[k].OuterUOM, PIH.ExImInvoiceDetail[k].MTon, PIH.ExImInvoiceDetail[k].OuterQty, PIH.ExImInvoiceDetail[k].OuterQty, PIH.ExImInvoiceDetail[k].NetWeight, PIH.ExImInvoiceDetail[k].NetWeight, PIH.ExImInvoiceDetail[k].TotalPackingWeight, PIH.ExImInvoiceDetail[k].GrossWeight, PIH.ExImInvoiceDetail[k].RateWithoutAddLess, PIH.ExImInvoiceDetail[k].RateAddLess, PIH.ExImInvoiceDetail[k].RatePrice, PIH.ExImInvoiceDetail[k].ContractRate, PIH.ExImInvoiceDetail[k].RateUomId, PIH.ExImInvoiceDetail[k].RateUOM, PIH.ExImInvoiceDetail[k].FcAmount, PIH.ExImInvoiceDetail[k].OtherRate, PIH.ExImInvoiceDetail[k].OtherAmount, PIH.ExImInvoiceDetail[k].OtherHsCode, PIH.ExImInvoiceDetail[k].PackingExpiryDate, PIH.ExImInvoiceDetail[k].ProductionNo, PIH.ExImInvoiceDetail[k].ItemCommodityDetail, PIH.ExImInvoiceDetail[k].HealthPermitNoDetail);
				}
				InvoiceUpdateFlag = PIH.ExImInvoiceDetail[0].ContractType;
				if (dtItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtItem, combitem, "Id", "Name", "Item", ZeroIndex: false);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				dtOtherItem.Rows.Clear();
				for (int j = 0; j < PIH.ExImInvoiceOtherItemslist.Count; j++)
				{
					dtOtherItem.Rows.Add(PIH.ExImInvoiceOtherItemslist[j].otherItemId, PIH.ExImInvoiceOtherItemslist[j].ItemName, PIH.ExImInvoiceOtherItemslist[j].oItemQty, PIH.ExImInvoiceOtherItemslist[j].oItemRate, PIH.ExImInvoiceOtherItemslist[j].oItemAmount, PIH.ExImInvoiceOtherItemslist[j].OtherItemRemarks);
				}
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				dtPaymentTerm.Rows.Clear();
				for (int i = 0; i < PIH.ExImInvoicePaymentTermsDetail.Count; i++)
				{
					dtPaymentTerm.Rows.Add(PIH.ExImInvoicePaymentTermsDetail[i].Id, PIH.ExImInvoicePaymentTermsDetail[i].DocumentTypeId, PIH.ExImInvoicePaymentTermsDetail[i].ExImEFormRegistrationId, PIH.ExImInvoicePaymentTermsDetail[i].FinancialInstrumentNo, PIH.ExImInvoicePaymentTermsDetail[i].PaymentTermId, PIH.ExImInvoicePaymentTermsDetail[i].PaymentTerm, PIH.ExImInvoicePaymentTermsDetail[i].PrcntOfTotal, PIH.ExImInvoicePaymentTermsDetail[i].FcyAmount, PIH.ExImInvoicePaymentTermsDetail[i].DueDays);
				}
				grdpaymentdetail.DataSource = dtPaymentTerm;
				grdpaymentdetail.RetrieveStructure();
				grdSettingsPaymentTerm();
				DetailFormReset();
				UpdateMode = true;
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BindFinancialInstrument();
	}

	private bool formvalidation()
	{
		if (txtdocno.Text == "" || Conversion.ToInt((object)txtdocno.Text) == 0)
		{
			MessageBox.Show("Doc No Is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPreInvoice).ActiveRow == null)
		{
			MessageBox.Show("PreInvoice Is Required");
			CmbPreInvoice.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPrefixType).ActiveRow == null || Conversion.ToInt(cmbPrefixType.Value) <= 0)
		{
			MessageBox.Show("Prefix Type Is Required");
			cmbPrefixType.Focus();
			return false;
		}
		if (((Control)(object)Cmbinvoiceno).Text == null || ((Control)(object)Cmbinvoiceno).Text == string.Empty || ((Control)(object)Cmbinvoiceno).Text.Trim() == "0")
		{
			MessageBox.Show("Invoice No Is Required");
			Cmbinvoiceno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSupCust).ActiveRow == null)
		{
			MessageBox.Show("Customer Is Required");
			cmbSupCust.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPaymentTermsNew).ActiveRow == null)
		{
			MessageBox.Show("Payment Term Is Required");
			cmbPaymentTermsNew.Focus();
			return false;
		}
		if (((UltraGridBase)cmbdeliverytermnew).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Is Required");
			cmbdeliverytermnew.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLoadingPort).ActiveRow == null)
		{
			MessageBox.Show("Loading Port Is Required");
			cmbLoadingPort.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDestinationPort).ActiveRow == null)
		{
			MessageBox.Show("Destination Port  Is Required");
			cmbDestinationPort.Focus();
			return false;
		}
		if (((Control)(object)cmbDestinationPort).Text.Trim() == ((Control)(object)cmbLoadingPort).Text.Trim())
		{
			MessageBox.Show("Destination Port and Loaing Port cannot be same...");
			cmbDestinationPort.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcareiertype).ActiveRow == null)
		{
			MessageBox.Show("Carier Type  Is Required");
			cmbcareiertype.Focus();
			return false;
		}
		if (txtGdNo.Text == "" || txtGdNo.Text == "0")
		{
			MessageBox.Show("GD_number Is Required");
			txtGdNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbfcycode).ActiveRow == null)
		{
			MessageBox.Show("Fcy Code Is Required");
			cmbfcycode.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfcyAmount.Text) == 0.0)
		{
			MessageBox.Show("Fcy Amount  Is Required");
			txtfcyAmount.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtNetWeight.Text) > 0.0))
		{
			MessageBox.Show("Net Weight  Is Required");
			txtNetWeight.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtGrossWeight.Text) >= Conversion.ToDouble((object)txtNetWeight.Text)))
		{
			MessageBox.Show("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
			txtGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommPercent.Text) > 0.0 && ((UltraGridBase)cmbCommissionAgent).ActiveRow == null)
		{
			MessageBox.Show("As you have entered 'Commission %', the 'Commission Agent' field is required. ");
			cmbCommissionAgent.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommPercent.Text) == 0.0 && ((UltraGridBase)cmbCommissionAgent).ActiveRow != null)
		{
			MessageBox.Show("As you have selected 'Commission agent', please enter the commission percentage.");
			txtCommPercent.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommAmount.Text) > 0.0 && ((UltraGridBase)cmbCommissionAgent).ActiveRow != null && ((UltraGridBase)CmbCommissionDebitAccount).ActiveRow == null)
		{
			MessageBox.Show("Commission DebitAccount Is Required");
			CmbCommissionDebitAccount.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtExRate.Text) > 0.0))
		{
			MessageBox.Show("ExchangeRate Field is Required");
			txtExRate.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtTotalNetAmount.Text) > 0.0))
		{
			MessageBox.Show("Total Amount  Is Required");
			txtTotalNetAmount.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (((UltraGridBase)combitem).ActiveRow == null)
		{
			MessageBox.Show("Item field required");
			combitem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("Pack Type field required");
			CmbPackUom.Focus();
			return false;
		}
		if (txtPackingWeight.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtPackingWeight.Text) > 0.0))
		{
			MessageBox.Show("PackingWeight field required");
			txtPackingWeight.Focus();
			return false;
		}
		if (txtNoOfContainersDetail.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtNoOfContainersDetail.Text) > 0.0))
		{
			MessageBox.Show("NoOfContainer field required");
			txtNoOfContainersDetail.Focus();
			return false;
		}
		if (txtNoOfBagsPerCntnr.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtNoOfBagsPerCntnr.Text) > 0.0))
		{
			MessageBox.Show("NoOfBagsPerCntnr field required");
			txtNoOfBagsPerCntnr.Focus();
			return false;
		}
		if (txtQtyMTon.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtQtyMTon.Text) > 0.0))
		{
			MessageBox.Show("Qty/M.Ton field required");
			txtQtyMTon.Focus();
			return false;
		}
		if (txtNoOfBags.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtNoOfBags.Text.Trim()) > 0.0))
		{
			MessageBox.Show("No of Bags field required");
			txtNoOfBags.Focus();
			return false;
		}
		if (txtNetWeightdetail.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtNetWeightdetail.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Net Weight field required");
			txtNetWeightdetail.Focus();
			return false;
		}
		if (txttotalpackingweight.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txttotalpackingweight.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Total Packing Weight field required");
			txttotalpackingweight.Focus();
			return false;
		}
		if (txtgrossWeightdetail.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtgrossWeightdetail.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Gross Weight field required");
			txtgrossWeightdetail.Focus();
			return false;
		}
		if (txtCostMTon.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtCostMTon.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Cost/M.Ton field required");
			txtCostMTon.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null)
		{
			MessageBox.Show("Rate UOM field required");
			CmbRateUom.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtAmount.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Amount field required");
			txtAmount.Focus();
			return false;
		}
		if (txtTotalNetAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Total Net Amount  field required");
			txtTotalNetAmount.Focus();
			return false;
		}
		return true;
	}

	private bool OtherItemFormValidation()
	{
		if (((UltraGridBase)CmbOtherItem).ActiveRow == null)
		{
			MessageBox.Show("Item field required");
			CmbOtherItem.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtoRate.Focus();
			return false;
		}
		return true;
	}

	private bool PaymentDetailFormValidation()
	{
		if (((UltraGridBase)CmbFinInstruments).ActiveRow == null)
		{
			MessageBox.Show("Financial Instrument No field required");
			CmbFinInstruments.Focus();
			return false;
		}
		if (((UltraGridBase)dcmbpaymentterm).ActiveRow == null)
		{
			MessageBox.Show("Payment Term field required");
			dcmbpaymentterm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim()) == 0.0 || dtxtpercentoftotal.Text == "")
		{
			MessageBox.Show("Percent Field Required");
			dtxtpercentoftotal.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)dtxtfcyamount.Text.Trim()) == 0.0 || dtxtfcyamount.Text == "")
		{
			MessageBox.Show("Fcy Amount  Field Required");
			dtxtfcyamount.Focus();
			return false;
		}
		return true;
	}

	private bool GdBreakUpFormValidation()
	{
		if (((UltraGridBase)CmbInvoiceGdBreakUp).ActiveRow == null)
		{
			MessageBox.Show("InvoiceNo field is required");
			CmbInvoiceGdBreakUp.Focus();
			return false;
		}
		if (txtGDNoBreakUp.Text == null || txtGDNoBreakUp.Text == string.Empty)
		{
			MessageBox.Show("GdNo field is required");
			txtGDNoBreakUp.Focus();
			return false;
		}
		if (txtInvoiceNoGDBreakUp.Text == null || txtInvoiceNoGDBreakUp.Text == string.Empty)
		{
			MessageBox.Show("BankInvoiceNo field is required");
			txtInvoiceNoGDBreakUp.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtDueDays.Text.Trim()) <= 0)
		{
			MessageBox.Show("Due Days field is Required");
			txtDueDays.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGDValue.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("GD Value field is Required");
			txtGDValue.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFobValue.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("FOb Value field is Required");
			txtFobValue.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNetAmount.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Net Amount field is Required");
			txtNetAmount.Focus();
			return false;
		}
		return true;
	}

	private bool AdvanceUtilizeFormValidation()
	{
		if (((UltraGridBase)CmbGdNoAdvanceUtilize).ActiveRow == null)
		{
			MessageBox.Show("GdNo field is required");
			CmbGdNoAdvanceUtilize.Focus();
			return false;
		}
		if (((UltraGridBase)CmbFIAdvanceUtlize).ActiveRow == null)
		{
			MessageBox.Show("FINO field is required");
			CmbFIAdvanceUtlize.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAdvanceUtlize.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("UtilizeAmount field is Required");
			txtAdvanceUtlize.Focus();
			return false;
		}
		return true;
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				((Control)(object)combitem).Text = string.Empty;
				((UltraGridBase)combitem).DataSource = null;
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbRateUom).DataSource = null;
				updateDetailIndex = item.RowIndex;
				DataTable dtitem = new DataTable();
				dtitem.Columns.Add("ItemId");
				dtitem.Columns.Add("ItemName");
				dtitem.Rows.Add(item.Cells["ItemId"].Value.ToString(), item.Cells["ItemName"].Text.ToString());
				DDL.BindDDL(dtitem, combitem, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
				combitem.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)combitem).Text = item.Cells["ItemName"].Text.ToString();
				bindRateUomAndItemPackUom();
				txtItemHsCodeDetail.Text = item.Cells["HSCode"].Text;
				combpcktype.Value = item.Cells["PackTypeId"].Value;
				txtPackingWeight.Text = Conversion.ToDouble(item.Cells["PackingWeight"].Value).ToString("#,##0.###");
				CmbCropYear.Value = item.Cells["CropYearId"].Value;
				((Control)(object)CmbCropYear).Text = item.Cells["CropYear"].Text;
				txtqtycheck.Text = item.Cells["ValidateBags"].Value.ToString();
				txtNoOfContainersDetail.Text = item.Cells["NoOfContainers"].Value.ToString();
				txtNoOfBagsPerCntnr.Text = item.Cells["NoOfBags/Cntnr"].Value.ToString();
				CmbPackUom.Value = item.Cells["PackSizeId"].Value;
				txtQtyMTon.Text = Conversion.ToDouble(item.Cells["Qty/M.Ton"].Value).ToString("#,##0.####");
				txtNoOfBags.Text = Conversion.ToDouble(item.Cells["NoOfBags"].Value).ToString("#,##0.###");
				txtNetWeightdetail.Text = Conversion.ToDouble(item.Cells["NetWeight"].Value).ToString("#,##0.###");
				txtweightcheck.Text = item.Cells["ValidateNetWeight"].Value.ToString();
				txttotalpackingweight.Text = Conversion.ToDouble(item.Cells["TotalPackingWeight"].Value).ToString("#,##0.###");
				txtgrossWeightdetail.Text = Conversion.ToDouble(item.Cells["GrossWeight"].Value).ToString("#,##0.###");
				CmbRateUom.Value = item.Cells["RateUOMId"].Value;
				txtRateWithoutAddLess.Text = Conversion.ToDouble(item.Cells["RateWithoutAddLess"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtAddLessRate.Text = Conversion.ToDouble(item.Cells["AddLessRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtCostMTon.Text = Conversion.ToDouble(item.Cells["Cost/M.Ton"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtOtherRateDetail.Text = item.Cells["OtherRate"].Value.ToString();
				txtOtherAmountDetil.Text = Conversion.ToDouble(item.Cells["OtherAmount"].Value).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtOtherHSCodeDetail.Text = item.Cells["OtherHSCode"].Value.ToString();
				txtPackingExpiryDate.Text = item.Cells["PackingExpiryDate"].Value.ToString();
				txtProductionNo.Text = item.Cells["ProductionNo"].Value.ToString();
				txtItemCommodityDetail.Text = item.Cells["ItemCommodityDetail"].Value.ToString();
				txtHealthPermitnodetail.Text = item.Cells["HealthPermitNoDetail"].Value.ToString();
				btnAddinGrid.Visible = false;
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				if (InvoiceUpdateFlag == 0)
				{
					txtCostMTon.ReadOnly = true;
				}
				else
				{
					txtCostMTon.ReadOnly = false;
				}
				CalculatAmount();
				CalculateLocalAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndadd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!PaymentDetailFormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdpaymentdetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["ExImEFormRegistrationId"].Value) == Conversion.ToInt(CmbFinInstruments.Value) && Conversion.ToInt(item.Cells["DocumentTypeId"].Value) == Conversion.ToInt(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["DocumentTypeId"].Value))
				{
					throw new Exception("Financial Instrument No already add in Grid Please Check");
				}
			}
			dtPaymentTerm.Rows.Add(0, ((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["DocumentTypeId"].Value, CmbFinInstruments.Value, ((Control)(object)CmbFinInstruments).Text, dcmbpaymentterm.Value, ((Control)(object)dcmbpaymentterm).Text, Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim()), Conversion.ToDouble((object)dtxtfcyamount.Text.Trim()), Conversion.ToDouble((object)dtxtdueday.Text.Trim()));
			grdpaymentdetail.DataSource = dtPaymentTerm;
			grdpaymentdetail.RetrieveStructure();
			grdSettingsPaymentTerm();
			ResePaymentDetails();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void btndupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (!PaymentDetailFormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdpaymentdetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (updateDetailIndexOther != Conversion.ToInt((object)item.RowIndex) && Conversion.ToInt(item.Cells["ExImEFormRegistrationId"].Value) == Conversion.ToInt(CmbFinInstruments.Value) && Conversion.ToInt(item.Cells["DocumentTypeId"].Value) == Conversion.ToInt(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["DocumentTypeId"].Value))
				{
					throw new Exception("Financial Instrument No already add in Grid Please Check");
				}
			}
			dtPaymentTerm.Rows[updateDetailIndexOther]["DocumentTypeId"] = ((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["DocumentTypeId"].Value;
			dtPaymentTerm.Rows[updateDetailIndexOther]["ExImEFormRegistrationId"] = CmbFinInstruments.Value;
			dtPaymentTerm.Rows[updateDetailIndexOther]["FinancialInstrumentNo"] = ((Control)(object)CmbFinInstruments).Text;
			dtPaymentTerm.Rows[updateDetailIndexOther]["PaymentTermId"] = dcmbpaymentterm.Value;
			dtPaymentTerm.Rows[updateDetailIndexOther]["PaymentTerm"] = ((Control)(object)dcmbpaymentterm).Text;
			dtPaymentTerm.Rows[updateDetailIndexOther]["%OfTotal"] = dtxtpercentoftotal.Text.Trim();
			dtPaymentTerm.Rows[updateDetailIndexOther]["FcyAmount"] = dtxtfcyamount.Text.Trim();
			dtPaymentTerm.Rows[updateDetailIndexOther]["DueDays"] = dtxtdueday.Text;
			grdpaymentdetail.DataSource = dtPaymentTerm;
			btndadd.Visible = true;
			btndupdate.Visible = false;
			btndcancel.Visible = false;
			ResePaymentDetails();
			CmbFinInstruments.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndcancel_Click(object sender, EventArgs e)
	{
		btndadd.Visible = true;
		btndupdate.Visible = false;
		btndcancel.Visible = false;
		ResePaymentDetails();
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ExImInvoicePackingDetail vd = new ExImInvoicePackingDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ContractId = Conversion.ToInt(r.Cells["PerformaId"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.HsCode = Conversion.ToString(r.Cells["HSCode"].Value);
					vd.PackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					vd.PackingWeight = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.NoofContainers = Conversion.ToDouble(r.Cells["NoOfContainers"].Value);
					vd.NoofBagsPerContainer = Conversion.ToDouble(r.Cells["NoOfBags/Cntnr"].Value);
					vd.OuterQtyUomId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
					vd.InnerQtyUomId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
					vd.MTon = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
					vd.OuterQty = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
					vd.InnerQty = Conversion.ToInt(r.Cells["NoOfBags"].Value);
					vd.NetWeight = Conversion.ToInt(r.Cells["NetWeight"].Value);
					vd.TotalPackingWeight = Conversion.ToDouble(r.Cells["TotalPackingWeight"].Value);
					vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
					vd.RateWithoutAddLess = Conversion.ToDouble(r.Cells["RateWithoutAddLess"].Value);
					vd.RateAddLess = Conversion.ToDouble(r.Cells["AddLessRate"].Value);
					vd.RatePrice = Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value);
					vd.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					vd.FcAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.OtherRate = Conversion.ToDouble(r.Cells["OtherRate"].Value);
					vd.OtherAmount = Conversion.ToDouble(r.Cells["OtherAmount"].Value);
					vd.OtherHsCode = Conversion.ToString((object)r.Cells["OtherHSCode"].Text);
					vd.PackingExpiryDate = Conversion.ToString((object)r.Cells["PackingExpiryDate"].Text);
					vd.ProductionNo = Conversion.ToString((object)r.Cells["ProductionNo"].Text);
					vd.ItemCommodityDetail = Conversion.ToString((object)r.Cells["ItemCommodityDetail"].Text);
					vd.HealthPermitNoDetail = Conversion.ToString((object)r.Cells["HealthPermitNoDetail"].Text);
					vd.ActionTypeId = 3;
					lstRemoveRecordDetail.Add(vd);
					dtdetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			getTotalnetWeightFromGrid();
			grdDetail.UpdateData();
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
			MessageBox.Show("Sorry Cannot Add New Record Only Update Detail Record : Thank You", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getTotalnetWeightFromGrid()
	{
		try
		{
			double NetWeightkg = 0.0;
			double GrossWeightkg = 0.0;
			if (grdDetail.CurrentRow != null)
			{
				grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
				object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["NetWeight"], (AggregateFunction)2);
				object GWeightTotal = grdDetail.GetTotal(grdDetail.RootTable.Columns["GrossWeight"], (AggregateFunction)2);
				double Amount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
				double TotalContainers = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["NoOfContainers"], (AggregateFunction)2));
				NetWeightkg = Conversion.ToDouble(total);
				txtNetWeight.Text = NetWeightkg.ToString("#,#.##");
				GrossWeightkg = Conversion.ToDouble(GWeightTotal);
				txtGrossWeight.Text = GrossWeightkg.ToString("#,#.##");
				txtfcyAmount.Text = Amount.ToString("#,#.##");
				txtNoOfContainer.Text = TotalContainers.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGridUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				if (btnsave.Visible && btnsave.Enabled && Conversion.ToDouble((object)txtNetWeightdetail.Text) > Conversion.ToDouble((object)txtweightcheck.Text))
				{
					MessageBox.Show("Net Weight Can not be greater than Bal Weight...\n Balance Net Weight Is " + txtweightcheck.Text);
					txtNoOfContainersDetail.Focus();
					return;
				}
				dtdetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(combitem.Value);
				dtdetail.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString((object)((Control)(object)combitem).Text);
				dtdetail.Rows[updateDetailIndex]["HSCode"] = Conversion.ToString((object)txtItemHsCodeDetail.Text);
				dtdetail.Rows[updateDetailIndex]["PackTypeId"] = Conversion.ToInt(combpcktype.Value);
				dtdetail.Rows[updateDetailIndex]["PackType"] = Conversion.ToString((object)((Control)(object)combpcktype).Text);
				dtdetail.Rows[updateDetailIndex]["PackingWeight"] = Conversion.ToDouble((object)txtPackingWeight.Text);
				dtdetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
				dtdetail.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
				dtdetail.Rows[updateDetailIndex]["NoOfContainers"] = Conversion.ToDouble((object)txtNoOfContainersDetail.Text);
				dtdetail.Rows[updateDetailIndex]["NoOfBags/Cntnr"] = Conversion.ToDouble((object)txtNoOfBagsPerCntnr.Text);
				dtdetail.Rows[updateDetailIndex]["PackSizeId"] = Conversion.ToInt(CmbPackUom.Value);
				dtdetail.Rows[updateDetailIndex]["PackSize"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
				dtdetail.Rows[updateDetailIndex]["Qty/M.Ton"] = Conversion.ToDouble((object)txtQtyMTon.Text);
				dtdetail.Rows[updateDetailIndex]["NoOfBags"] = Conversion.ToDouble((object)txtNoOfBags.Text);
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtNetWeightdetail.Text);
				dtdetail.Rows[updateDetailIndex]["TotalPackingWeight"] = Conversion.ToDouble((object)txttotalpackingweight.Text);
				dtdetail.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtgrossWeightdetail.Text);
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = Conversion.ToDouble((object)((Control)(object)CmbRateUom).Text);
				dtdetail.Rows[updateDetailIndex]["RateWithoutAddLess"] = Conversion.ToDouble((object)txtRateWithoutAddLess.Text);
				dtdetail.Rows[updateDetailIndex]["AddLessRate"] = Conversion.ToDouble((object)txtAddLessRate.Text);
				dtdetail.Rows[updateDetailIndex]["Cost/M.Ton"] = Conversion.ToDouble((object)txtCostMTon.Text);
				dtdetail.Rows[updateDetailIndex]["RateUOMId"] = Conversion.ToInt(CmbRateUom.Value);
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = Conversion.ToString((object)((Control)(object)CmbRateUom).Text);
				dtdetail.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				dtdetail.Rows[updateDetailIndex]["OtherRate"] = Conversion.ToDouble((object)txtOtherRateDetail.Text);
				dtdetail.Rows[updateDetailIndex]["OtherAmount"] = Conversion.ToDouble((object)txtOtherAmountDetil.Text);
				dtdetail.Rows[updateDetailIndex]["OtherHSCode"] = Conversion.ToString((object)txtOtherHSCodeDetail.Text);
				dtdetail.Rows[updateDetailIndex]["PackingExpiryDate"] = Conversion.ToString((object)txtPackingExpiryDate.Text);
				dtdetail.Rows[updateDetailIndex]["ProductionNo"] = Conversion.ToString((object)txtProductionNo.Text);
				dtdetail.Rows[updateDetailIndex]["ItemCommodityDetail"] = Conversion.ToString((object)txtItemCommodityDetail.Text);
				dtdetail.Rows[updateDetailIndex]["HealthPermitNoDetail"] = Conversion.ToString((object)txtHealthPermitnodetail.Text);
				getTotalnetWeightFromGrid();
				btnAddinGrid.Visible = true;
				btnGridUpdate.Visible = false;
				btnCancel.Visible = false;
				DetailFormReset();
				getTotalnetWeightFromGrid();
				CalculateLocalAmount();
				txtCommPercent_TextChanged(null, null);
				combitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			DetailFormReset();
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
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["PerformaId"].Visible = false;
			grdDetail.RootTable.Columns["PerformaDetailId"].Visible = false;
			grdDetail.RootTable.Columns["PerformaDate"].Visible = false;
			grdDetail.RootTable.Columns["PRI-RefNo"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetail.RootTable.Columns["PackSizeId"].Visible = false;
			grdDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdDetail.RootTable.Columns["ValidateNetWeight"].Visible = false;
			grdDetail.RootTable.Columns["ValidateBags"].Visible = false;
			grdDetail.RootTable.Columns["ContractRate"].Visible = false;
			grdDetail.RootTable.Columns["PackType"].Caption = "Packing Type";
			grdDetail.RootTable.Columns["Qty/M.Ton"].Caption = "M.Ton";
			grdDetail.RootTable.Columns["PackSize"].Caption = "Pack Uom";
			grdDetail.RootTable.Columns["Cost/M.Ton"].Caption = "Rate/M.Ton";
			grdDetail.RootTable.Columns["ContractRate"].Caption = "Proforma Rate";
			grdDetail.RootTable.Columns["OtherHSCode"].Caption = "Bank HS Code";
			grdDetail.RootTable.Columns["OtherRate"].Caption = "Bank Rate";
			grdDetail.RootTable.Columns["OtherAmount"].Caption = "Bank Amount";
			grdDetail.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PackingWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Qty/M.Ton"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty/M.Ton"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty/M.Ton"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Qty/M.Ton"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TotalPackingWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TotalPackingWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TotalPackingWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TotalPackingWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["RateWithoutAddLess"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateWithoutAddLess"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdDetail.RootTable.Columns["AddLessRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLessRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdDetail.RootTable.Columns["Cost/M.Ton"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Cost/M.Ton"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["OtherRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["OtherRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdDetail.RootTable.Columns["OtherAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["OtherAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["OtherAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["OtherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.FrozenColumns = 1;
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
			FromDateHistory.Focus();
		}
		else
		{
			txtDocDate.Focus();
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
			gridDetailHistory.DataSource = null;
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = ExImLcOrder.GetDataForDropDownFromExportInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
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
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.Ids = "211";
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			Obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			Obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			Obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			Obj.ApprovedFilter = "All";
			DataTable dtgrd = ExImInvoice.FormHistory(Obj);
			if (dtgrd.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("VoucherHeadId");
				dt.Columns.Add("InvoiceNo");
				dt.Columns.Add("DocCode", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("NoOfContainers", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("Add/Less", typeof(double));
				dt.Columns.Add("TotalAmount", typeof(double));
				dt.Columns.Add("LoadingPort");
				dt.Columns.Add("DestinationPort");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedStatus");
				dt.Columns.Add("ApprovedUser");
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dtgrd.Rows.Count; i++)
				{
					dt.Rows.Add(dtgrd.Rows[i]["Id"], dtgrd.Rows[i]["VoucherHeadId"], dtgrd.Rows[i]["InvoiceNo"], dtgrd.Rows[i]["DocCode"], Conversion.ToDateTime(dtgrd.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtgrd.Rows[i]["Customer"], dtgrd.Rows[i]["NoOfContainers"], dtgrd.Rows[i]["GrossWeight"], dtgrd.Rows[i]["NetWeight"], dtgrd.Rows[i]["FCurrencyAmount"], dtgrd.Rows[i]["AddLessAmount"], dtgrd.Rows[i]["TotalAmount"], dtgrd.Rows[i]["LoadingPort"], dtgrd.Rows[i]["DestinationPort"], dtgrd.Rows[i]["EntryUserName"], Conversion.ToDateTime(dtgrd.Rows[i]["EntryDate"]).ToString("dd-MMM-yyyy"), dtgrd.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dtgrd.Rows[i]["ModifyDate"]).ToString("dd-MMM-yyyy"), dtgrd.Rows[i]["ApprovedStatus"], dtgrd.Rows[i]["ApprovedUserName"], Conversion.ToDateTime(dtgrd.Rows[i]["ApprovedDate"]).ToString("dd-MMM-yyyy"), dtgrd.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
				gridDetailHistory.ClearStructure();
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
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["Add/Less"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Add/Less"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["NoOfContainers"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["TotalAmount"].TotalFormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["Add/Less"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["NoOfContainers"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["NetWeight"].FormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["TotalAmount"].FormatString = "#,#.###";
			DataGridHistory.RootTable.Columns.Add("Slip");
			DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Slip"].ButtonText = "549-Slip";
			DataGridHistory.RootTable.Columns["Slip"].Width = 70;
			DataGridHistory.RootTable.Columns["Slip"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("BankSlip");
			DataGridHistory.RootTable.Columns["BankSlip"].Caption = "Bank Slip";
			DataGridHistory.RootTable.Columns["BankSlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["BankSlip"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["BankSlip"].ButtonText = "Bank Slip";
			DataGridHistory.RootTable.Columns["BankSlip"].Width = 70;
			DataGridHistory.RootTable.Columns["BankSlip"].Position = 1;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 70;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBankInvoicesForGdBreakUp()
	{
		try
		{
			DataTable dtInvoice = ExImInvoice.GetBankInvoicesForGdBreakUp(UserAccount.OrganizationId, UserAccount.CompanyId, GdBreakUpRecId);
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoice, CmbInvoiceGdBreakUp, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbInvoiceGdBreakUp).Text = string.Empty;
			((UltraGridBase)CmbInvoiceGdBreakUp).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetInvoicesagainstAdvances()
	{
		try
		{
			DataTable dtInvoice = ExImInvoice.GetInvoicesForAdvanceUtilize(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoice, CmbInvoiceAdvanceUtilize, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
				((UltraGridBase)CmbInvoiceAdvanceUtilize).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			else
			{
				((Control)(object)CmbInvoiceAdvanceUtilize).Text = string.Empty;
				((UltraGridBase)CmbInvoiceAdvanceUtilize).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGDsAgainstAdvancePaymentUtilizeInInvoice()
	{
		try
		{
			DataTable dtInvoice = ExImInvoice.GetGDsAgainstAdvancePaymentUtilizeInInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value), 0, 0);
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoice, CmbGdNoAdvanceUtilize, "Id", "GDNO", "GDNO", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbGdNoAdvanceUtilize).Text = string.Empty;
			((UltraGridBase)CmbGdNoAdvanceUtilize).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPendingFINosForAdvanceUtilize()
	{
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 211);
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
			if (e.Column.Key == "Slip")
			{
				InvoiceSlip549(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value), 211);
			}
			if (e.Column.Key == "BankSlip")
			{
				CommonServices.ExportPreCommercialInvoiceSlip548(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.CurrentRow == null || (int)DataGridHistory.CurrentRow.RowType != 0 || DataGridHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemName");
			dt.Columns.Add("HSCode");
			dt.Columns.Add("PackingType");
			dt.Columns.Add("PackingWeight", typeof(double));
			dt.Columns.Add("CropYear");
			dt.Columns.Add("NoOfContainers", typeof(double));
			dt.Columns.Add("NoOfBags/Cntnr", typeof(double));
			dt.Columns.Add("PackUom");
			dt.Columns.Add("M.Ton", typeof(double));
			dt.Columns.Add("NoOfBags", typeof(double));
			dt.Columns.Add("NetWeight", typeof(double));
			dt.Columns.Add("TotalPackingWeight", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("Rate/M.Ton", typeof(double));
			dt.Columns.Add("RateUOM");
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("BankRate", typeof(double));
			dt.Columns.Add("BankAmount", typeof(double));
			dt.Columns.Add("BankHSCode");
			dt.Columns.Add("PackingExpiryDate");
			dt.Columns.Add("ItemCommodityDetail");
			dt.Columns.Add("HealthPermitNoDetail");
			ExImInvoice PIH = new ExImInvoice();
			PIH = ExImInvoice.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			if (PIH.ExImInvoiceDetail.Count > 0)
			{
				for (int i = 0; i < PIH.ExImInvoiceDetail.Count; i++)
				{
					dt.Rows.Add(PIH.ExImInvoiceDetail[i].ItemId, PIH.ExImInvoiceDetail[i].ItemName, PIH.ExImInvoiceDetail[i].HsCode, PIH.ExImInvoiceDetail[i].PackMaterilaType, PIH.ExImInvoiceDetail[i].PackingWeight, PIH.ExImInvoiceDetail[i].CropYear, PIH.ExImInvoiceDetail[i].NoofContainers, PIH.ExImInvoiceDetail[i].NoofBagsPerContainer, PIH.ExImInvoiceDetail[i].OuterUOM, PIH.ExImInvoiceDetail[i].MTon, PIH.ExImInvoiceDetail[i].OuterQty, PIH.ExImInvoiceDetail[i].NetWeight, PIH.ExImInvoiceDetail[i].TotalPackingWeight, PIH.ExImInvoiceDetail[i].GrossWeight, PIH.ExImInvoiceDetail[i].RatePrice, PIH.ExImInvoiceDetail[i].RateUOM, PIH.ExImInvoiceDetail[i].FcAmount, PIH.ExImInvoiceDetail[i].OtherRate, PIH.ExImInvoiceDetail[i].OtherAmount, PIH.ExImInvoiceDetail[i].OtherHsCode, PIH.ExImInvoiceDetail[i].PackingExpiryDate, PIH.ExImInvoiceDetail[i].ItemCommodityDetail, PIH.ExImInvoiceDetail[i].HealthPermitNoDetail);
				}
				gridDetailHistory.DataSource = dt;
				gridDetailHistory.RetrieveStructure();
				gridDetailHistory.RootTable.Columns["ItemId"].Visible = false;
				gridDetailHistory.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["PackingWeight"].FormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["NoOfBags/Cntnr"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["NoOfBags/Cntnr"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["NoOfBags/Cntnr"].FormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["NoOfBags/Cntnr"].TotalFormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["M.Ton"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["M.Ton"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["M.Ton"].FormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["M.Ton"].TotalFormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["TotalPackingWeight"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["TotalPackingWeight"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["TotalPackingWeight"].FormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["TotalPackingWeight"].TotalFormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
				gridDetailHistory.RootTable.Columns["Rate/M.Ton"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["Rate/M.Ton"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
				gridDetailHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
				gridDetailHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
				gridDetailHistory.RootTable.Columns["BankRate"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["BankRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
				gridDetailHistory.RootTable.Columns["BankAmount"].AggregateFunction = (AggregateFunction)2;
				gridDetailHistory.RootTable.Columns["BankAmount"].TextAlignment = (TextAlignment)3;
				gridDetailHistory.RootTable.Columns["BankAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
				gridDetailHistory.RootTable.Columns["BankAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
				CommonServices.GridAutoAdjustmentNew(gridDetailHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoadd_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows.Add(CmbOtherItem.Value, ((Control)(object)CmbOtherItem).Text, Conversion.ToDouble((object)txtoQty.Text.Trim()), Conversion.ToString((object)txtoRate.Text), Conversion.ToDouble((object)txtoAmount.Text.Trim()), txtoRemarks.Text.Trim());
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				ResetOtherItems();
				TotalAmountCalculate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOther = item.RowIndex;
				CmbOtherItem.Value = item.Cells["ItemId"].Value.ToString();
				txtoQty.Text = item.Cells["Qty"].Value.ToString();
				txtoRate.Text = item.Cells["Rate"].Value.ToString();
				txtoAmount.Text = item.Cells["Amount"].Value.ToString();
				txtoRemarks.Text = item.Cells["Remarks"].Value.ToString();
				btnoadd.Visible = false;
				btnoUpdate.Visible = true;
				btnoCancel.Visible = true;
				txtoQty.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows[updateDetailIndexOther]["ItemId"] = CmbOtherItem.Value;
				dtOtherItem.Rows[updateDetailIndexOther]["ItemName"] = ((Control)(object)CmbOtherItem).Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Qty"] = txtoQty.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Rate"] = txtoRate.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Amount"] = txtoAmount.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Remarks"] = txtoRemarks.Text;
				grdotheritems.DataSource = dtOtherItem;
				btnoadd.Visible = true;
				btnoUpdate.Visible = false;
				btnoCancel.Visible = false;
				ResetOtherItems();
				txtoQty.Focus();
				TotalAmountCalculate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		btnoadd.Visible = true;
		btnoUpdate.Visible = false;
		btnoCancel.Visible = false;
		ResetOtherItems();
	}

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Qty"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Amount"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Rate"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["ItemName"].Width = 350;
			grdotheritems.RootTable.Columns["Remarks"].Width = 350;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsGdBreakUp()
	{
		try
		{
			grdGdBreakUp.RootTable.Columns["Id"].Visible = false;
			grdGdBreakUp.RootTable.Columns["InvoiceId"].Visible = false;
			grdGdBreakUp.RootTable.Columns["OtherAmount"].Visible = false;
			grdGdBreakUp.RootTable.Columns["BankId"].Visible = false;
			grdGdBreakUp.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["GDValue"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["GDValue"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["GDValue"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["GDValue"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["Freight"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FOBValue"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["FOBValue"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["FOBValue"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FOBValue"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FTT%"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["FTT%"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FttAmount"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["FttAmount"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["FttAmount"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FttAmount"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["NetAmount"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["NetAmount"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns.Add("Delete");
			grdGdBreakUp.RootTable.Columns["Delete"].Caption = "X";
			grdGdBreakUp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGdBreakUp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGdBreakUp.RootTable.Columns["Delete"].ButtonText = "X";
			grdGdBreakUp.RootTable.Columns["Delete"].Width = 20;
			grdGdBreakUp.RootTable.Columns["Delete"].Position = 0;
			grdGdBreakUp.FrozenColumns = 1;
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsAdvanceUtilize()
	{
		try
		{
			grdAdvanceUtlize.RootTable.Columns["Id"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["GDId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["InvoiceId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["FIId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["Id"].EditType = (EditType)0;
			grdAdvanceUtlize.RootTable.Columns["GDId"].EditType = (EditType)0;
			grdAdvanceUtlize.RootTable.Columns["InvoiceId"].EditType = (EditType)0;
			grdAdvanceUtlize.RootTable.Columns["FIId"].EditType = (EditType)0;
			grdAdvanceUtlize.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			grdAdvanceUtlize.RootTable.Columns["GDNo"].EditType = (EditType)0;
			grdAdvanceUtlize.RootTable.Columns["FINo"].EditType = (EditType)0;
			grdAdvanceUtlize.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grdAdvanceUtlize.RootTable.Columns["UtilizeAmount"].AggregateFunction = (AggregateFunction)2;
			grdAdvanceUtlize.RootTable.Columns["UtilizeAmount"].TextAlignment = (TextAlignment)3;
			grdAdvanceUtlize.RootTable.Columns["UtilizeAmount"].FormatString = "#,##0.##";
			grdAdvanceUtlize.RootTable.Columns["UtilizeAmount"].TotalFormatString = "#,##0.##";
			grdAdvanceUtlize.RootTable.Columns.Add("Delete");
			grdAdvanceUtlize.RootTable.Columns["Delete"].Caption = "X";
			grdAdvanceUtlize.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdAdvanceUtlize.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdAdvanceUtlize.RootTable.Columns["Delete"].ButtonText = "X";
			grdAdvanceUtlize.RootTable.Columns["Delete"].Width = 20;
			grdAdvanceUtlize.RootTable.Columns["Delete"].Position = 0;
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtxtpercentoftotal_TextChanged(object sender, EventArgs e)
	{
		PaymentDetailPercentCalculate();
	}

	private void dtxtfcyamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double TotalAmount = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				TotalAmount += Conversion.ToDouble(item.Cells["OtherAmount"].Value);
			}
			if (TotalAmount > 0.0 && Conversion.ToDouble((object)dtxtfcyamount.Text) > 0.0)
			{
				double Percent = Conversion.ToDouble((object)dtxtfcyamount.Text.Trim()) / TotalAmount * 100.0;
				dtxtpercentoftotal.Text = Percent.ToString("#,##0.###");
				if (Conversion.ToDouble((object)dtxtpercentoftotal.Text) > 100.0)
				{
					dtxtpercentoftotal.Text = "0";
					throw new Exception("Percent cannot be greater than 100 Please check");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtoQty_TextChanged(object sender, EventArgs e)
	{
		OtherItemsCalculations();
	}

	private void CmbOtherItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (btnupdate.Visible && btnupdate.Enabled && ((UltraGridBase)CmbOtherItem).ActiveRow != null)
			{
				txtoRate.Text = Conversion.ToString(((UltraDropDownBase)CmbOtherItem).SelectedRow.Cells[2].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtaddlessamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfcyAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtaddlessamount_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentDetailPercentCalculate()
	{
		try
		{
			if (grdDetail.GetRows().Length != 0 && Conversion.ToDouble((object)dtxtpercentoftotal.Text) > 0.0)
			{
				if (Conversion.ToDouble((object)dtxtpercentoftotal.Text) > 100.0)
				{
					dtxtpercentoftotal.Text = "0";
					throw new Exception("Percent cannot be greater than 100 Please check");
				}
				double TotalAmount = 0.0;
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					TotalAmount += Conversion.ToDouble(item.Cells["OtherAmount"].Value);
				}
				double Percent = Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim());
				if (TotalAmount > 0.0 && Percent > 0.0)
				{
					double FcAmount = TotalAmount * Percent / 100.0;
					dtxtfcyamount.Text = FcAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetFinancialInstrumentsBalance()
	{
		try
		{
			if (((UltraGridBase)CmbFinInstruments).ActiveRow != null)
			{
				txtBalanceFI.Text = Conversion.ToString(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["BalFIAmount"].Value);
			}
			else
			{
				txtBalanceFI.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsCalculations()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtoQty.Text.Trim());
			double Rate = Conversion.ToDouble((object)txtoRate.Text.Trim());
			if (Qty > 0.0 && Rate > 0.0)
			{
				txtoAmount.Text = (Qty * Rate).ToString();
			}
			else
			{
				txtoAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmountCalculate()
	{
		try
		{
			object total = grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2);
			double AddlessAmount = Conversion.ToDouble((object)txtaddlessamount.Text);
			double FcyAmount = Conversion.ToDouble((object)txtfcyAmount.Text);
			double OtherAmount = Conversion.ToDouble(total);
			if (FcyAmount > 0.0)
			{
				txtTotalNetAmount.Text = (FcyAmount + AddlessAmount + OtherAmount).ToString("#,#.###");
			}
			if (Conversion.ToDouble((object)txtTotalNetAmount.Text) > 0.0 && grdpaymentdetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdpaymentdetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double TotalAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text);
					double Amount = Conversion.ToDouble(obj.Cells["%OfTotal"].Value) * TotalAmount / 100.0;
					obj.Cells["FcyAmount"].Value = Amount;
					obj.EndEdit();
				}
			}
			CalculateLocalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateLocalAmount()
	{
		try
		{
			double fcyAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text);
			double ExchangeRate = Conversion.ToDouble((object)txtExRate.Text);
			if (fcyAmount > 0.0 && ExchangeRate > 0.0)
			{
				txtLocalAmt.Text = (fcyAmount * ExchangeRate).ToString("0,0");
			}
			else
			{
				txtLocalAmt.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateLocalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtaddlesscommnets_TextChanged(object sender, EventArgs e)
	{
	}

	private void CalculateWeight()
	{
		try
		{
			if (Conversion.ToDouble((object)txtNoOfContainersDetail.Text) > 0.0 && Conversion.ToDouble((object)txtNoOfBagsPerCntnr.Text) > 0.0)
			{
				double OuterUom = 0.0;
				double TotalNoOfBags = Conversion.ToDouble((object)txtNoOfContainersDetail.Text) * Conversion.ToDouble((object)txtNoOfBagsPerCntnr.Text);
				if (TotalNoOfBags > Conversion.ToDouble((object)txtqtycheck.Text))
				{
					txtNoOfBagsPerCntnr.Text = "0";
					txtNoOfBags.Text = Conversion.ToDouble((object)txtqtycheck.Text).ToString("#,##0.###");
					throw new Exception("Total No Of Bags Can't Be Greater Than Balance No. Of Bags Which Is " + Conversion.ToDouble((object)txtqtycheck.Text));
				}
				txtNoOfBags.Text = TotalNoOfBags.ToString();
				double PackingWeight = Conversion.ToDouble((object)txtPackingWeight.Text);
				txttotalpackingweight.Text = (TotalNoOfBags * PackingWeight).ToString("#,##0.###");
				if (((UltraGridBase)CmbPackUom).ActiveRow != null)
				{
					OuterUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
					txtQtyMTon.Text = (TotalNoOfBags * OuterUom / 1000.0).ToString("#,##0.####");
					txtNetWeightdetail.Text = (TotalNoOfBags * OuterUom).ToString("#,##0.###");
					txtgrossWeightdetail.Text = (TotalNoOfBags * OuterUom + TotalNoOfBags * PackingWeight).ToString("#,##0.###");
				}
				else
				{
					txtNoOfBags.Text = 0.ToString();
					txtQtyMTon.Text = 0.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatAmount()
	{
		try
		{
			double ItemRate = 0.0;
			double OtherItemRate = 0.0;
			double ItemRateUom = 0.0;
			double Qty = 0.0;
			txtAmount.Text = 0.ToString();
			if (((UltraGridBase)CmbRateUom).ActiveRow != null)
			{
				ItemRate = Conversion.ToDouble((object)txtCostMTon.Text);
				OtherItemRate = Conversion.ToDouble((object)txtOtherRateDetail.Text);
				ItemRateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				Qty = Conversion.ToDouble((object)txtQtyMTon.Text);
				if (ItemRate > 0.0 && ItemRateUom > 0.0 && Qty > 0.0)
				{
					double weightkgs = Qty * 1000.0;
					double amount = ItemRate / ItemRateUom * weightkgs;
					double Otheramount = OtherItemRate / ItemRateUom * weightkgs;
					txtAmount.Text = amount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
					txtOtherAmountDetil.Text = Otheramount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
			}
			else
			{
				txtAmount.Text = "0";
				txtOtherAmountDetil.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_TextChanged(object sender, EventArgs e)
	{
	}

	private void combitempck_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCostMTon_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbSupCust_Leave_1(object sender, EventArgs e)
	{
		try
		{
			BindPreInvoice();
			BindFinancialInstrument();
			BindHeaderInformPreInvoiceByPreInvoiceId(Conversion.ToInt(CmbPreInvoice.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpaymentdetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdpaymentdetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOther = item.RowIndex;
				CmbFinInstruments.Value = item.Cells["ExImEFormRegistrationId"].Value.ToString();
				dcmbpaymentterm.Value = item.Cells["PaymentTermId"].Value.ToString();
				dtxtpercentoftotal.Text = Conversion.ToDouble(item.Cells["%OfTotal"].Value).ToString("#,##0.###");
				dtxtfcyamount.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				dtxtdueday.Text = item.Cells["DueDays"].Value.ToString();
				btndadd.Visible = false;
				btndupdate.Visible = true;
				btndcancel.Visible = true;
				txtoQty.Focus();
			}
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
			grdpaymentdetail.RootTable.Columns["%OfTotal"].AggregateFunction = (AggregateFunction)2;
			grdpaymentdetail.RootTable.Columns["%OfTotal"].TextAlignment = (TextAlignment)3;
			grdpaymentdetail.RootTable.Columns["%OfTotal"].FormatString = "#,#.##";
			grdpaymentdetail.RootTable.Columns["%OfTotal"].TotalFormatString = "#,#.##";
			grdpaymentdetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].FormatString = "#,#.##";
			grdpaymentdetail.RootTable.Columns["FcyAmount"].TotalFormatString = "#,#.##";
			grdpaymentdetail.RootTable.Columns["ExImEFormRegistrationId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["PaymentTermId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["Id"].Visible = false;
			grdpaymentdetail.RootTable.Columns["FinancialInstrumentNo"].Width = 350;
			grdpaymentdetail.RootTable.Columns["PaymentTerm"].Width = 350;
			grdpaymentdetail.RootTable.Columns.Add("Delete");
			grdpaymentdetail.RootTable.Columns["Delete"].Caption = "X";
			grdpaymentdetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdpaymentdetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdpaymentdetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdpaymentdetail.RootTable.Columns["Delete"].Width = 20;
			grdpaymentdetail.RootTable.Columns["Delete"].Position = 0;
			grdpaymentdetail.FrozenColumns = 1;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceSlip549(int PrintId)
	{
		try
		{
			CommonServices.ExportPreCommercialInvoiceSlip549(PrintId);
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
			CommonServices.ExportPreCommercialInvoiceSlip549(RecId);
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			DetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCostMTon_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommPercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackingWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txttotalpackingweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdotheritems).Name);
		if (fs != null)
		{
			grdotheritems.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdpaymentdetail).Name);
		if (fs != null)
		{
			grdpaymentdetail.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void ImProformaInvoice_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 3)
				{
					btnResetGdBreakupHistory_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 4)
				{
					btnResetMultiInvoiceGd_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnrefersh_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 3)
				{
					btnRefreshGdBreakupHistory_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 4)
				{
					btnRefreshMultiInvoiceGd_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (!UpdateMode)
					{
						btnsave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 3)
				{
					btnShowGdBreakUpHistory_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 4 && btnsave.Visible)
				{
					btnSaveMultiInvoiceGd_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.U)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (UpdateMode)
					{
						btnsave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 4 && btnUpdateMultiInvoiceGd.Visible)
				{
					btnUpdateMultiInvoiceGd_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 2;
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					tabControl1.SelectedIndex = 3;
				}
				else if (tabControl1.SelectedIndex == 3)
				{
					tabControl1.SelectedIndex = 4;
					GdNoMultiInvoiceForm.Focus();
				}
				else if (tabControl1.SelectedIndex == 4)
				{
					tabControl1.SelectedIndex = 0;
				}
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex != 0)
			{
				if (tabControl1.SelectedIndex == 3)
				{
					((Control)(object)grdGDhistory).Focus();
				}
				else if (tabControl1.SelectedIndex == 4)
				{
					if (((Control)(object)grdMultiInvoiceAgainstGd).Focused)
					{
						((Control)(object)DataGridHistoryMultiInvoiceToGdForm).Focus();
					}
					else if (((Control)(object)DataGridHistoryMultiInvoiceToGdForm).Focused)
					{
						((Control)(object)grdMultiInvoiceAgainstGd).Focus();
					}
					else
					{
						((Control)(object)grdMultiInvoiceAgainstGd).Focus();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex != 0)
			{
				if (tabControl1.SelectedIndex == 3)
				{
					FromDateGdBreakupHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 4)
				{
					if (((Control)(object)CmbInvoiceNoMultiInvoiceForm).Focused)
					{
						FromDateMultiInvoiceAgainstGd.Focus();
					}
					else if (FromDateMultiInvoiceAgainstGd.Focused)
					{
						CmbInvoiceNoMultiInvoiceForm.Focus();
					}
					else
					{
						CmbInvoiceNoMultiInvoiceForm.Focus();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.F5 && tabControl1.SelectedIndex == 4)
			{
				GdNoMultiInvoiceForm.Focus();
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
			if (e.Control && e.Alt && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 4)
			{
				btnShowMultiInvoicesToGdForm_Click(null, null);
			}
			if (e.Control && e.Alt && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 4)
			{
				btnResetMultiInvoiceToGdHistory_Click(null, null);
			}
			if (e.Control && e.Alt && e.KeyCode == Keys.R && tabControl1.SelectedIndex == 4)
			{
				btnRefreshMultiInvoiceToGdHistory_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailByHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeightdetail_TextChanged(object sender, EventArgs e)
	{
	}

	private void txttotalpackingweight_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtCommPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double commRate = Conversion.ToDouble((object)txtCommPercent.Text.Trim());
			if (commRate > 100.0)
			{
				MessageBox.Show("Commission % Can not Greater than 100...");
				txtCommPercent.Text = "100";
			}
			else if (grdDetail.CurrentRow != null)
			{
				double total = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
				if (!double.IsNaN(total))
				{
					double commAmt = Conversion.ToDouble((object)total.ToString()) * commRate / 100.0;
					txtCommAmount.Text = Math.Round(commAmt).ToString();
				}
				else
				{
					txtCommAmount.Text = "0";
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoaderForm_Click(object sender, EventArgs e)
	{
		try
		{
			LoadInvoice = new LoadForwardingForCommercialInvoice(UserAccount);
			LoadInvoice.ShowDialog();
			LoadInvoice.BringToFront();
			LoadInGridDetail();
			cmbSupCust_Leave_1(null, null);
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
			DataTable dt = LoadInvoice.dtSupply;
			if (dt.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbSupCust).ActiveRow != null && grdDetail.GetRows().Count() > 0 && Conversion.ToInt(cmbSupCust.Value) != Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]))
				{
					throw new Exception("You Can't Load Data Of Different Customer");
				}
				if (((UltraGridBase)cmbfcycode).ActiveRow != null && grdDetail.GetRows().Count() > 0 && Conversion.ToInt(cmbfcycode.Value) != Conversion.ToInt(dt.Rows[0]["FCurrencyId"]))
				{
					throw new Exception("You Can't Load Data Of Different Currency");
				}
				((Control)(object)cmbSupCust).Enabled = false;
				((Control)(object)cmbfcycode).Enabled = false;
				cmbSupCust.Value = Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]);
				cmbfcycode.Value = Conversion.ToInt(dt.Rows[0]["FCurrencyId"]);
				cmbCommissionAgent.Value = Conversion.ToInt(dt.Rows[0]["CommissionAgentId"]);
				txtCommPercent.Text = Conversion.ToDouble(dt.Rows[0]["CommRate"]).ToString(clsGlobalVariables.DecimalFCYRateFormate);
				bool flag = false;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (grdDetail.GetRows().Length != 0)
					{
						GridEXRow[] rows = grdDetail.GetRows();
						foreach (GridEXRow item in rows)
						{
							if (Conversion.ToInt(item.Cells["ItemId"].Value) == Conversion.ToInt(dt.Rows[i]["ItemId"]) && Conversion.ToInt(item.Cells["PackTypeId"].Value) == Conversion.ToInt(dt.Rows[i]["PackingMaterialId"]) && Conversion.ToInt(item.Cells["CropYearId"].Value) == Conversion.ToInt(dt.Rows[i]["CropYearId"]) && Conversion.ToInt(item.Cells["PackSizeId"].Value) == Conversion.ToInt(dt.Rows[i]["PackUomId"]) && Conversion.ToInt(item.Cells["RateUOMId"].Value) == Conversion.ToInt(dt.Rows[i]["RateUomId"]) && Conversion.ToInt(item.Cells["Cost/M.Ton"].Value) == Conversion.ToInt(dt.Rows[i]["RatePrice"]) && Conversion.ToInt(item.Cells["OtherRate"].Value) == Conversion.ToInt(dt.Rows[i]["OtherRate"]) && Conversion.ToString(item.Cells["PackingExpiryDate"].Value) == Conversion.ToString(dt.Rows[i]["PackingExpiryDate"]) && Conversion.ToString(item.Cells["ProductionNo"].Value) == Conversion.ToString(dt.Rows[i]["ProductionNo"]))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtdetail.Rows.Add(0, Conversion.ToInt(dt.Rows[i]["ContractId"]), Conversion.ToInt(dt.Rows[i]["ContractDetailId"]), Conversion.ToDateTime(dt.Rows[i]["ProformaDate"]), Conversion.ToString(dt.Rows[i]["ProformaNo"]), Conversion.ToInt(dt.Rows[i]["ItemId"]), Conversion.ToString(dt.Rows[i]["ItemName"]), "", Conversion.ToInt(dt.Rows[i]["PackingMaterialId"]), Conversion.ToString(dt.Rows[i]["PackingType"]), Conversion.ToDouble(dt.Rows[i]["EbUnit"]), Conversion.ToInt(dt.Rows[i]["CropYearId"]), Conversion.ToString(dt.Rows[i]["CropYear"]), 0, 0, Conversion.ToInt(dt.Rows[i]["PackUomId"]), Conversion.ToString(dt.Rows[i]["PackUom"]), Conversion.ToDouble(dt.Rows[i]["BalMTon"]), Conversion.ToDouble(dt.Rows[i]["BalNoofBags"]), Conversion.ToDouble(dt.Rows[i]["BalNoofBags"]), Conversion.ToDouble(dt.Rows[i]["BalWeight"]), Conversion.ToDouble(dt.Rows[i]["BalWeight"]), Conversion.ToDouble(dt.Rows[i]["EbTotal"]), Conversion.ToDouble(dt.Rows[i]["GrossWeight"]), Conversion.ToDouble(dt.Rows[i]["RatePrice"]), 0, Conversion.ToDouble(dt.Rows[i]["RatePrice"]), Conversion.ToDouble(dt.Rows[i]["RatePrice"]), Conversion.ToInt(dt.Rows[i]["RateUomId"]), Conversion.ToString(dt.Rows[i]["RateUom"]), Conversion.ToDouble(dt.Rows[i]["BalShipAmount"]), Conversion.ToDouble(dt.Rows[i]["OtherRate"]), 0, 0, Conversion.ToString(dt.Rows[i]["PackingExpiryDate"]), Conversion.ToString(dt.Rows[i]["ProductionNo"]), "", "");
					}
					flag = false;
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
			}
			dtOtherItem.Clear();
			CalculatAmount();
			CalculateLocalAmount();
			CalculateWeight();
			TotalAmountCalculate();
			getTotalnetWeightFromGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void LoadOtherItemsData(string ID)
	{
		try
		{
			DataTable dtOthItm = ExImLcOrder.ReadOtherItemsByProformaHeaderIds(ID);
			if (dtOthItm.Rows.Count > 0)
			{
				dtOtherItem.Rows.Clear();
				for (int i = 0; i < dtOthItm.Rows.Count; i++)
				{
					dtOtherItem.Rows.Add(dtOthItm.Rows[i]["otherItemId"], dtOthItm.Rows[i]["ItemName"], dtOthItm.Rows[i]["oItemQty"], dtOthItm.Rows[i]["oItemRate"], dtOthItm.Rows[i]["oItemAmount"], dtOthItm.Rows[i]["OtherItemRemarks"]);
				}
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnrefersh_Click(object sender, EventArgs e)
	{
		try
		{
			branchesGetAll();
			BindCommercialInvoiceNofromlookup();
			BindPrefixType();
			projectGetAll();
			CustomerGetAll();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			MultiCurrencyfill();
			ImporterandExportBankFill();
			CarierType();
			ItemPackingTypeFill();
			CropYearFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtaddlessamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNoOfContainer_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtLocalAmt_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtTotalNetAmount_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtAmount_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtNoOfBags_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtCommAmount_TextChanged(object sender, EventArgs e)
	{
	}

	private void combpcktype_Leave(object sender, EventArgs e)
	{
	}

	private void txtNoOfContainersDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfBagsPerCntnr_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 211);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherRateDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOtherRateDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
			txtOtherAmountDetil.Text = decimal.Parse(txtOtherAmountDetil.Text.Replace(",", "")).ToString("#,##0.00");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint548_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExportPreCommercialInvoiceSlip548(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateInvoiceNo_Click(object sender, EventArgs e)
	{
		try
		{
			new GenerateExportInvoiceNos(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommiercialInvoiceAgainstPreInvoiceTransfer_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdAdvanceUtlize).Name);
		if (fs != null)
		{
			grdAdvanceUtlize.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void txtGDValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGdBreakUp).Name);
		if (fs != null)
		{
			grdGdBreakUp.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		try
		{
			if (!GdBreakUpFormValidation())
			{
				return;
			}
			if (dtGdBreakUp.Rows.Count > 0)
			{
				for (int i = 0; i < dtGdBreakUp.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtGdBreakUp.Rows[i]["InvoiceId"]) != Conversion.ToInt(CmbInvoiceGdBreakUp.Value))
					{
						throw new Exception("You can only add data against one invoice in the grid");
					}
				}
			}
			dtGdBreakUp.Rows.Add(0, CmbInvoiceGdBreakUp.Value, ((Control)(object)CmbInvoiceGdBreakUp).Text.Trim(), txtGDNoBreakUp.Text.Trim(), Conversion.ToDateTime((object)datGDDateGDBreakUp.Value), txtInvoiceNoGDBreakUp.Text.Trim(), Conversion.ToDateTime((object)datInvoiceDateGdBreakUp.Value), lblInvoiceBalanceGdBreakUp.Text.Trim(), Conversion.ToInt(CmbBankName.Value), ((Control)(object)CmbBankName).Text.Trim(), Conversion.ToInt((object)txtDueDays.Text), Conversion.ToDouble((object)txtExchangeRate.Text), Conversion.ToDouble((object)txtGDValue.Text.Trim()), Conversion.ToDouble((object)txtFreight.Text), Conversion.ToDouble((object)txtFobValue.Text), Conversion.ToDouble((object)txtFTTPrcnt.Text), Conversion.ToDouble((object)txtFttAmount.Text), Conversion.ToDouble((object)txtNetAmount.Text), Conversion.ToString((object)txtRemarks.Text));
			grdGdBreakUp.DataSource = dtGdBreakUp;
			grdGdBreakUp.RetrieveStructure();
			grdSettingsGdBreakUp();
			ResetGdBreakUpDetails();
			CmbInvoiceGdBreakUp.ReadOnly = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateGdBreakUp_Click(object sender, EventArgs e)
	{
		try
		{
			if (GdBreakUpFormValidation())
			{
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["BankId"] = CmbBankName.Value;
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["BankName"] = ((Control)(object)CmbBankName).Text;
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["InvoiceId"] = CmbInvoiceGdBreakUp.Value;
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["PartyInvoiceNo"] = ((Control)(object)CmbInvoiceGdBreakUp).Text;
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["GdNo"] = txtGDNoBreakUp.Text;
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["GdDate"] = datGDDateGDBreakUp.Value;
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["InvoiceNo"] = txtInvoiceNoGDBreakUp.Text;
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["InvoiceDate"] = datInvoiceDateGdBreakUp.Value;
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["DueDays"] = Conversion.ToInt((object)txtDueDays.Text.Trim());
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["ExchangeRate"] = Conversion.ToDouble((object)txtExchangeRate.Text.Trim());
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["GDValue"] = Conversion.ToDouble((object)txtGDValue.Text.Trim());
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["Freight"] = Conversion.ToDouble((object)txtFreight.Text.Trim());
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["FOBValue"] = Conversion.ToDouble((object)txtFobValue.Text.Trim());
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["FTT%"] = Conversion.ToDouble((object)txtFTTPrcnt.Text.Trim());
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["FttAmount"] = Conversion.ToDouble((object)txtFttAmount.Text.Trim());
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["NetAmount"] = Conversion.ToDouble((object)txtNetAmount.Text.Trim());
				dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["Remarks"] = Conversion.ToString((object)txtRemarks.Text.Trim());
				grdGdBreakUp.DataSource = dtGdBreakUp;
				btnAddGDBreakUp.Visible = true;
				btnUpdateGdBreakUp.Visible = false;
				btnCancelGdBreakUp.Visible = false;
				ResetGdBreakUpDetails();
				CmbInvoiceGdBreakUp.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGdBreakUp_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdGdBreakUp.CurrentRow;
			if (item != null)
			{
				updateDetailIndexGdBreakUp = item.RowIndex;
				if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
				{
					GdBreakUpRecId = Conversion.ToInt(item.Cells["Id"].Value);
				}
				CmbBankName.Value = Conversion.ToInt(item.Cells["BankId"].Value);
				CmbInvoiceGdBreakUp.Value = Conversion.ToInt(item.Cells["InvoiceId"].Value);
				((Control)(object)CmbInvoiceGdBreakUp).Text = Conversion.ToString(item.Cells["PartyInvoiceNo"].Value);
				txtGDNoBreakUp.Text = Conversion.ToString(item.Cells["GdNo"].Value);
				datGDDateGDBreakUp.Value = Conversion.ToDateTime(item.Cells["GdDate"].Value);
				txtInvoiceNoGDBreakUp.Text = Conversion.ToString(item.Cells["InvoiceNo"].Value);
				datInvoiceDateGdBreakUp.Value = Conversion.ToDateTime(item.Cells["InvoiceDate"].Value);
				txtDueDays.Text = Conversion.ToString(item.Cells["DueDays"].Value);
				txtExchangeRate.Text = Conversion.ToString(item.Cells["ExchangeRate"].Value);
				txtGDValue.Text = Conversion.ToDouble(item.Cells["GDValue"].Value).ToString("#,##0.###");
				txtFreight.Text = Conversion.ToDouble(item.Cells["Freight"].Value).ToString("#,##0.###");
				txtFobValue.Text = Conversion.ToDouble(item.Cells["FOBValue"].Value).ToString("#,##0.###");
				txtFTTPrcnt.Text = ((Conversion.ToDouble(item.Cells["FTT%"].Value) > 0.0) ? Conversion.ToDouble(item.Cells["FTT%"].Value).ToString("#,##0.###") : "10");
				txtFttAmount.Text = Conversion.ToDouble(item.Cells["FttAmount"].Value).ToString("#,##0.###");
				txtNetAmount.Text = Conversion.ToDouble(item.Cells["NetAmount"].Value).ToString("#,##0.###");
				txtRemarks.Text = Conversion.ToString(item.Cells["Remarks"].Value);
				btnAddGDBreakUp.Visible = false;
				btnUpdateGdBreakUp.Visible = true;
				btnCancelGdBreakUp.Visible = true;
				CmbInvoiceGdBreakUp.Focus();
				GetBankInvoicesForGdBreakUp();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGdBreakUp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGdBreakUp.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ExImInvoiceBankGDBreakUp vd = new ExImInvoiceBankGDBreakUp();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					vd.BankId = Conversion.ToInt(r.Cells["BankId"].Value);
					vd.GDNO = Conversion.ToString(r.Cells["GdNo"].Value);
					vd.GDDate = Conversion.ToDateTime(r.Cells["GdDate"].Value);
					vd.BankInvoiceNo = Conversion.ToString(r.Cells["InvoiceNo"].Value);
					vd.BankInvoiceDate = Conversion.ToDateTime(r.Cells["InvoiceDate"].Value);
					vd.GDValue = Conversion.ToDecimal(r.Cells["GDValue"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecordGdBreakUpDetail.Add(vd);
					dtGdBreakUp.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			grdGdBreakUp.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadyByIdGdBreak(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImInvoiceBankGDBreakUp obj = new ExImInvoiceBankGDBreakUp();
			obj = ExImInvoice.GetGdBreakUpByInvoiceId(Id);
			dtGdBreakUp.Rows.Clear();
			for (int i = 0; i < obj.ExImInvoiceBankGDBreakUpslist.Count; i++)
			{
				dtGdBreakUp.Rows.Add(obj.ExImInvoiceBankGDBreakUpslist[i].Id, obj.ExImInvoiceBankGDBreakUpslist[i].ExImInvoiceId, obj.ExImInvoiceBankGDBreakUpslist[i].InvoiceNo, obj.ExImInvoiceBankGDBreakUpslist[i].GDNO, obj.ExImInvoiceBankGDBreakUpslist[i].GDDate, obj.ExImInvoiceBankGDBreakUpslist[i].BankInvoiceNo, obj.ExImInvoiceBankGDBreakUpslist[i].BankInvoiceDate, obj.ExImInvoiceBankGDBreakUpslist[i].OtherAmount, obj.ExImInvoiceBankGDBreakUpslist[i].BankId, obj.ExImInvoiceBankGDBreakUpslist[i].BankName, obj.ExImInvoiceBankGDBreakUpslist[i].DueDays, obj.ExImInvoiceBankGDBreakUpslist[i].ExchangeRate, obj.ExImInvoiceBankGDBreakUpslist[i].GDValue, obj.ExImInvoiceBankGDBreakUpslist[i].Freight, obj.ExImInvoiceBankGDBreakUpslist[i].FobValue, obj.ExImInvoiceBankGDBreakUpslist[i].CommPercent, obj.ExImInvoiceBankGDBreakUpslist[i].CommAmount, obj.ExImInvoiceBankGDBreakUpslist[i].NetToBeRealized, obj.ExImInvoiceBankGDBreakUpslist[i].Remarks);
			}
			grdGdBreakUp.DataSource = dtGdBreakUp;
			grdGdBreakUp.RetrieveStructure();
			grdSettingsGdBreakUp();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceGdBreakUp_TextChanged(object sender, EventArgs e)
	{
		try
		{
			lblInvoiceBalanceGdBreakUp.Visible = true;
			if (((UltraGridBase)CmbInvoiceGdBreakUp).ActiveRow != null)
			{
				lblInvoiceBalanceGdBreakUp.Text = Conversion.ToString(((UltraDropDownBase)CmbInvoiceGdBreakUp).SelectedRow.Cells[2].Value);
				return;
			}
			lblInvoiceBalanceGdBreakUp.Visible = false;
			lblInvoiceBalanceGdBreakUp.Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton6_Click(object sender, EventArgs e)
	{
		try
		{
			ResetGdBreakUpDetails();
			dtGdBreakUp.Rows.Clear();
			grdGdBreakUp.ClearStructure();
			CmbInvoiceGdBreakUp.ReadOnly = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton8_Click(object sender, EventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		try
		{
			if (grdGdBreakUp.GetRows().Count() <= 0)
			{
				throw new Exception("GDBreakUp Grid Record not found");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			decimal GDValue = default(decimal);
			if (grdGdBreakUp.RowCount <= 0)
			{
				return;
			}
			ExImInvoiceBankGDBreakUp obj = new ExImInvoiceBankGDBreakUp();
			obj.ExImInvoiceBankGDBreakUpslist = new List<ExImInvoiceBankGDBreakUp>();
			if (lstRemoveRecordGdBreakUpDetail.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecordGdBreakUpDetail.Count; i++)
				{
					obj.ExImInvoiceBankGDBreakUpslist.Add(lstRemoveRecordGdBreakUpDetail[i]);
				}
			}
			decimal BankInvoiceAmount = default(decimal);
			GridEXRow[] rows = grdGdBreakUp.GetRows();
			foreach (GridEXRow r in rows)
			{
				ExImInvoiceBankGDBreakUp vd = new ExImInvoiceBankGDBreakUp();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.DocumentTypeId = 1;
				vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				vd.BankId = Conversion.ToInt(r.Cells["BankId"].Value);
				vd.GDNO = Conversion.ToString(r.Cells["GdNo"].Value);
				vd.GDDate = Conversion.ToDateTime(r.Cells["GdDate"].Value);
				vd.BankInvoiceNo = Conversion.ToString(r.Cells["InvoiceNo"].Value);
				vd.BankInvoiceDate = Conversion.ToDateTime(r.Cells["InvoiceDate"].Value);
				vd.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
				vd.ExchangeRate = Conversion.ToDecimal(r.Cells["ExchangeRate"].Value);
				vd.GDValue = Conversion.ToDecimal(r.Cells["GDValue"].Value);
				vd.Freight = Conversion.ToDecimal(r.Cells["Freight"].Value);
				vd.FobValue = Conversion.ToDecimal(r.Cells["FOBValue"].Value);
				vd.CommPercent = Conversion.ToDecimal(r.Cells["FTT%"].Value);
				vd.CommAmount = Conversion.ToDecimal(r.Cells["FttAmount"].Value);
				vd.NetToBeRealized = Conversion.ToDecimal(r.Cells["NetAmount"].Value);
				vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				obj.ExImInvoiceBankGDBreakUpslist.Add(vd);
				GDValue += Conversion.ToDecimal((object)vd.GDValue);
				BankInvoiceAmount = Conversion.ToDecimal(r.Cells["OtherAmount"].Value);
			}
			if (BankInvoiceAmount != GDValue)
			{
				throw new Exception("Bank InvoiceAmount must be equal to Total GDValue Please Check");
			}
			ExImInvoice.SaveGdBreakUp(obj);
			MessageBox.Show("Record Save Successfully");
			ResetGdBreakUpDetails();
			dtGdBreakUp.Rows.Clear();
			grdGdBreakUp.ClearStructure();
			((Control)(object)CmbInvoiceGdBreakUp).Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelGdBreakUp_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddGDBreakUp.Visible = true;
			btnUpdateGdBreakUp.Visible = false;
			btnCancelGdBreakUp.Visible = false;
			ResetGdBreakUpDetails();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton10_Click(object sender, EventArgs e)
	{
		try
		{
			GetBankInvoicesForGdBreakUp();
			GetInvoicesagainstAdvances();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceAdvanceUtilize_Leave(object sender, EventArgs e)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			GetGDsAgainstAdvancePaymentUtilizeInInvoice();
			if (((UltraGridBase)CmbInvoiceAdvanceUtilize).ActiveRow != null && grdAdvanceUtlize.GetRows().Length == 0)
			{
				int RefDocumentTypeId = 0;
				if (((UltraGridBase)CmbInvoiceAdvanceUtilize).ActiveRow != null && Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value) > 0)
				{
					RefDocumentTypeId = Conversion.ToInt(((UltraDropDownBase)CmbInvoiceAdvanceUtilize).SelectedRow.Cells["DocumentTypeId"].Value);
				}
				AdvanceUtilizeagainstGDs obj = new AdvanceUtilizeagainstGDs();
				obj = ExImInvoice.GetAdvanceUtilizeByInvoiceId(Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value), RefDocumentTypeId);
				dtAdvanceUtilize.Rows.Clear();
				for (int i = 0; i < obj.AdvanceUtilizeagainstGDslist.Count; i++)
				{
					dtAdvanceUtilize.Rows.Add(obj.AdvanceUtilizeagainstGDslist[i].Id, obj.AdvanceUtilizeagainstGDslist[i].EximInvoiceId, obj.AdvanceUtilizeagainstGDslist[i].InvoiceNo, obj.AdvanceUtilizeagainstGDslist[i].GDId, obj.AdvanceUtilizeagainstGDslist[i].GDNO, obj.AdvanceUtilizeagainstGDslist[i].RefDocumentTypeId, obj.AdvanceUtilizeagainstGDslist[i].FIId, obj.AdvanceUtilizeagainstGDslist[i].FINo, obj.AdvanceUtilizeagainstGDslist[i].UtilizeAmount);
				}
				grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
				grdAdvanceUtlize.RetrieveStructure();
				grdSettingsAdvanceUtilize();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbGdNoAdvanceUtilize_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbGdNoAdvanceUtilize).ActiveRow != null)
			{
				txtGdValueAdvanceUtlize.Text = Conversion.ToString(((UltraDropDownBase)CmbGdNoAdvanceUtilize).SelectedRow.Cells["GDBalance"].Value);
			}
			else
			{
				txtGdValueAdvanceUtlize.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbFIAdvanceUtlize_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbFIAdvanceUtlize).ActiveRow != null)
			{
				txtFIBalanceAdvanceUtlize.Text = Conversion.ToString(((UltraDropDownBase)CmbFIAdvanceUtlize).SelectedRow.Cells["BalFIAmount"].Value);
			}
			else
			{
				txtFIBalanceAdvanceUtlize.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddAdvanceUtlize_Click(object sender, EventArgs e)
	{
		try
		{
			if (AdvanceUtilizeFormValidation())
			{
				dtAdvanceUtilize.Rows.Add(0, Conversion.ToInt(((UltraDropDownBase)CmbGdNoAdvanceUtilize).SelectedRow.Cells["ExImInvoiceId"].Value), Conversion.ToString((object)((UltraDropDownBase)CmbGdNoAdvanceUtilize).SelectedRow.Cells["BankInvoiceNo"].Text), CmbGdNoAdvanceUtilize.Value, ((Control)(object)CmbGdNoAdvanceUtilize).Text.Trim(), Conversion.ToInt(((UltraDropDownBase)CmbGdNoAdvanceUtilize).SelectedRow.Cells["DocumentTypeId"].Value), CmbFIAdvanceUtlize.Value, ((Control)(object)CmbFIAdvanceUtlize).Text.Trim(), Conversion.ToDouble((object)txtAdvanceUtlize.Text.Trim()));
				grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
				grdAdvanceUtlize.RetrieveStructure();
				grdSettingsAdvanceUtilize();
				ResetGdAdvanceUtilizeDetails();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAdvanceUtlize_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdAdvanceUtlize.CurrentRow;
			if (item != null)
			{
				updateDetailIndexAdvanceUtilize = item.RowIndex;
				CmbInvoiceAdvanceUtilize.Value = Conversion.ToInt(item.Cells["InvoiceId"].Value);
				CmbGdNoAdvanceUtilize.Value = Conversion.ToInt(item.Cells["GDId"].Value);
				CmbFIAdvanceUtlize.Value = Conversion.ToInt(item.Cells["FIId"].Value);
				txtAdvanceUtlize.Text = Conversion.ToString(item.Cells["UtilizeAmount"].Value);
				btnAddAdvanceUtlize.Visible = false;
				btnUpdateAdvanceUtilize.Visible = true;
				btnCancelAdvanceUtilize.Visible = true;
				CmbInvoiceAdvanceUtilize.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateAdvanceUtilize_Click(object sender, EventArgs e)
	{
		try
		{
			if (AdvanceUtilizeFormValidation())
			{
				dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["InvoiceId"] = CmbInvoiceAdvanceUtilize.Value;
				dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["InvoiceNo"] = ((Control)(object)CmbInvoiceAdvanceUtilize).Text;
				dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["GDId"] = CmbGdNoAdvanceUtilize.Value;
				dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["GDNo"] = ((Control)(object)CmbGdNoAdvanceUtilize).Text;
				dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["FIId"] = CmbFIAdvanceUtlize.Value;
				dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["FINo"] = ((Control)(object)CmbFIAdvanceUtlize).Text;
				dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["UtilizeAmount"] = txtAdvanceUtlize.Text.Trim();
				grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
				btnAddAdvanceUtlize.Visible = true;
				btnUpdateAdvanceUtilize.Visible = false;
				btnCancelAdvanceUtilize.Visible = false;
				ResetGdAdvanceUtilizeDetails();
				CmbInvoiceAdvanceUtilize.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton11_Click(object sender, EventArgs e)
	{
		try
		{
			AdvanceUtilizeIds = "";
			((Control)(object)CmbInvoiceAdvanceUtilize).Text = string.Empty;
			((Control)(object)CmbFIAdvanceUtlize).Text = string.Empty;
			txtGdValueAdvanceUtlize.Text = string.Empty;
			txtFIBalanceAdvanceUtlize.Text = string.Empty;
			((Control)(object)CmbGdNoAdvanceUtilize).Text = string.Empty;
			((UltraGridBase)CmbGdNoAdvanceUtilize).DataSource = null;
			btnAddAdvanceUtlize.Visible = true;
			btnUpdateAdvanceUtilize.Visible = false;
			btnCancelAdvanceUtilize.Visible = false;
			ResetGdAdvanceUtilizeDetails();
			dtAdvanceUtilize.Rows.Clear();
			grdAdvanceUtlize.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton12_Click(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			if (grdAdvanceUtlize.GetRows().Count() <= 0)
			{
				throw new Exception("Advance Utilize Grid Record not found");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No && grdAdvanceUtlize.RowCount > 0)
			{
				AdvanceUtilizeagainstGDs obj = new AdvanceUtilizeagainstGDs();
				obj.AdvanceUtilizeagainstGDslist = new List<AdvanceUtilizeagainstGDs>();
				obj.Ids = AdvanceUtilizeIds;
				GridEXRow[] rows = grdAdvanceUtlize.GetRows();
				foreach (GridEXRow r in rows)
				{
					AdvanceUtilizeagainstGDs vd = new AdvanceUtilizeagainstGDs();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.EximInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					vd.GDId = Conversion.ToInt(r.Cells["GDId"].Value);
					vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					vd.FIId = Conversion.ToInt(r.Cells["FIId"].Value);
					vd.FINo = Conversion.ToString(r.Cells["FINo"].Value);
					vd.UtilizeAmount = Conversion.ToDecimal(r.Cells["UtilizeAmount"].Value);
					obj.AdvanceUtilizeagainstGDslist.Add(vd);
				}
				ExImInvoice.SaveAdvanceUtilize(obj);
				MessageBox.Show("Record Save Successfully");
				dtAdvanceUtilize.Rows.Clear();
				grdAdvanceUtlize.ClearStructure();
				AdvanceUtilizeIds = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton14_Click(object sender, EventArgs e)
	{
		try
		{
			GetBankInvoicesForGdBreakUp();
			GetInvoicesagainstAdvances();
			BindFinancialInstrument();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAdvanceUtlize_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdAdvanceUtlize.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					AdvanceUtilizeIds = AdvanceUtilizeIds + "," + Conversion.ToString(r.Cells["Id"].Value);
					dtAdvanceUtilize.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			grdAdvanceUtlize.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFillForGdBreakupHistory()
	{
		try
		{
			DataTable dtBank = new DataTable();
			dtBank.Columns.Add("Id", typeof(int));
			dtBank.Columns.Add("Bank");
			DataTable dt = ExImInvoice.GetDataForDropDownFromExportGdBreakup(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Bank")
					{
						dtBank.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtBank, CmbBankGdBankHistory, "Id", "Bank", "Bank Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillGDFormHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.ExImInvoiceBankGDBreakUp_History(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDateGdBreakupHistory.Value,
				ToDate = ToDateGdBreakupHistory.Value,
				AccountId = Conversion.ToInt(CmbBankGdBankHistory.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ExImInvoiceId", typeof(int));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("GDNO", typeof(string));
				dtcol.Columns.Add("GDDate", typeof(DateTime));
				dtcol.Columns.Add("BankInvoiceNo", typeof(string));
				dtcol.Columns.Add("BankInvoiceDate", typeof(DateTime));
				dtcol.Columns.Add("OtherAmount", typeof(decimal));
				dtcol.Columns.Add("BankName", typeof(string));
				dtcol.Columns.Add("DueDays", typeof(int));
				dtcol.Columns.Add("ExchangeRate", typeof(decimal));
				dtcol.Columns.Add("GDValue", typeof(decimal));
				dtcol.Columns.Add("Freight", typeof(decimal));
				dtcol.Columns.Add("FobValue", typeof(decimal));
				dtcol.Columns.Add("FTT%", typeof(decimal));
				dtcol.Columns.Add("FTTAmount", typeof(decimal));
				dtcol.Columns.Add("NetAmount", typeof(decimal));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Rows.Clear();
				foreach (DataRow row in dt.Rows)
				{
					dtcol.Rows.Add(row["DocumentTypeId"], row["Id"], row["ExImInvoiceId"], row["InvoiceNo"], row["GDNO"], Conversion.ToDateTime(row["GDDate"]).ToShortDateString(), row["BankInvoiceNo"], row["BankInvoiceDate"], row["OtherAmount"], row["BranchName"], row["DueDays"], row["ExchangeRate"], row["GDValue"], row["Freight"], row["FobValue"], row["CommPercent"], row["CommAmount"], row["NetToBeRealized"], row["Remarks"]);
				}
				grdGDhistory.DataSource = dtcol;
				grdGDhistory.RetrieveStructure();
				grdGdHistorySetting();
			}
			else
			{
				grdGDhistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGdHistorySetting()
	{
		try
		{
			grdGDhistory.RootTable.Columns["Id"].Visible = false;
			grdGDhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdGDhistory.RootTable.Columns["OtherAmount"].Visible = false;
			grdGDhistory.RootTable.Columns["ExImInvoiceId"].Visible = false;
			grdGDhistory.RootTable.Columns["GDValue"].AggregateFunction = (AggregateFunction)2;
			grdGDhistory.RootTable.Columns["GDValue"].TextAlignment = (TextAlignment)3;
			grdGDhistory.RootTable.Columns["GDValue"].FormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["GDValue"].TotalFormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdGDhistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdGDhistory.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["Freight"].TotalFormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["FobValue"].AggregateFunction = (AggregateFunction)2;
			grdGDhistory.RootTable.Columns["FobValue"].TextAlignment = (TextAlignment)3;
			grdGDhistory.RootTable.Columns["FobValue"].FormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["FobValue"].TotalFormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdGDhistory.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["FTT%"].TextAlignment = (TextAlignment)3;
			grdGDhistory.RootTable.Columns["FTT%"].FormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["FTTAmount"].AggregateFunction = (AggregateFunction)2;
			grdGDhistory.RootTable.Columns["FTTAmount"].TextAlignment = (TextAlignment)3;
			grdGDhistory.RootTable.Columns["FTTAmount"].FormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["FTTAmount"].TotalFormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdGDhistory.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdGDhistory.RootTable.Columns["NetAmount"].FormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["NetAmount"].TotalFormatString = "#,##0.###";
			grdGDhistory.RootTable.Columns["OtherAmount"].AggregateFunction = (AggregateFunction)2;
			grdGDhistory.RootTable.Columns["OtherAmount"].TextAlignment = (TextAlignment)3;
			grdGDhistory.RootTable.Columns["OtherAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdGDhistory.RootTable.Columns["OtherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdGDhistory);
			grdGDhistory.RootTable.Columns.Add("Edit");
			grdGDhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdGDhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdGDhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGDhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdGDhistory.RootTable.Columns["Edit"].Width = 40;
			grdGDhistory.RootTable.Columns["Edit"].Position = 0;
			grdGDhistory.FrozenColumns = 1;
			ctrlGrdBar5_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGDhistory).Name);
			if (fs != null)
			{
				grdGDhistory.LoadLayoutFile((Stream)fs);
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

	private void txtGDValue_KeyPress_1(object sender, KeyPressEventArgs e)
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

	private void txtFobValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFTTPrcnt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFttAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNetAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGDValue_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValue();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValue();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFobValue_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFTTAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFTTPrcnt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFTTAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFttAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetAmount();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateFobValue()
	{
		try
		{
			if (Conversion.ToDouble((object)txtFreight.Text) > Conversion.ToDouble((object)txtGDValue.Text))
			{
				throw new Exception("Freight Cant be Greater Than GD Value");
			}
			double Fob = Conversion.ToDouble((object)txtGDValue.Text) - Conversion.ToDouble((object)txtFreight.Text);
			txtFobValue.Text = Fob.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateFTTAmount()
	{
		try
		{
			if (Conversion.ToDouble((object)txtFTTPrcnt.Text) > 100.0)
			{
				txtFTTPrcnt.Text = "10";
				MessageBox.Show("Percent Cant be Greater Than 100", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			double FTTAmount = Conversion.ToDouble((object)txtFobValue.Text) * Conversion.ToDouble((object)txtFTTPrcnt.Text) / 100.0;
			txtFttAmount.Text = FTTAmount.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateNetAmount()
	{
		try
		{
			if (Conversion.ToDouble((object)txtGDValue.Text) > 0.0 && Conversion.ToDouble((object)txtFttAmount.Text) > 0.0)
			{
				double NetAmount = Conversion.ToDouble((object)txtGDValue.Text) - Conversion.ToDouble((object)txtFttAmount.Text);
				txtNetAmount.Text = NetAmount.ToString("#,##0.###");
			}
			else
			{
				txtNetAmount.Text = 0.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnResetGdBreakupHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateGdBreakupHistory.Value = DateTime.Now;
			ToDateGdBreakupHistory.Value = DateTime.Now;
			((Control)(object)CmbBankGdBankHistory).Text = string.Empty;
			grdGDhistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshGdBreakupHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFillForGdBreakupHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowGdBreakUpHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FillGDFormHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGDhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGDhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				int GdTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				int ExImInvoiceId = Conversion.ToInt(item.Cells["ExImInvoiceId"].Value);
				switch (GdTypeId)
				{
				case 1:
					tabControl1.SelectedIndex = 2;
					GdBreakUpRecId = id;
					GetBankInvoicesForGdBreakUp();
					ReadyByIdGdBreak(ExImInvoiceId);
					break;
				case 2:
					tabControl1.SelectedIndex = 4;
					ReadByIdMultiInvoiceToGdForm(id);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGDhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGDhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int id = Conversion.ToInt(item.Cells["Id"].Value);
			int GdTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			int ExImInvoiceId = Conversion.ToInt(item.Cells["ExImInvoiceId"].Value);
			if (e.Column.Key == "Edit")
			{
				switch (GdTypeId)
				{
				case 1:
					tabControl1.SelectedIndex = 2;
					GdBreakUpRecId = id;
					GetBankInvoicesForGdBreakUp();
					ReadyByIdGdBreak(ExImInvoiceId);
					break;
				case 2:
					tabControl1.SelectedIndex = 4;
					ReadByIdMultiInvoiceToGdForm(id);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGDhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGDhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdGDhistory.CurrentColumn == null)
			{
				return;
			}
			int id = Conversion.ToInt(r.Cells["Id"].Value);
			int GdTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			int ExImInvoiceId = Conversion.ToInt(r.Cells["ExImInvoiceId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdGDhistory.CurrentColumn.Key == "Edit")
				{
					switch (GdTypeId)
					{
					case 1:
						tabControl1.SelectedIndex = 2;
						GdBreakUpRecId = id;
						GetBankInvoicesForGdBreakUp();
						ReadyByIdGdBreak(ExImInvoiceId);
						break;
					case 2:
						tabControl1.SelectedIndex = 4;
						ReadByIdMultiInvoiceToGdForm(id);
						break;
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grdGDhistory_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBankInvoicesForMultiInvoiceToGdForm()
	{
		try
		{
			DataTable dtInvoice = ExImInvoice.GetBankInvoicesForGdBreakUp(UserAccount.OrganizationId, UserAccount.CompanyId, MultiInvoiceRecId);
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoice, CmbInvoiceNoMultiInvoiceForm, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbInvoiceNoMultiInvoiceForm).Text = string.Empty;
			((UltraGridBase)CmbInvoiceNoMultiInvoiceForm).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFillForMultiInvoicesToGdForm()
	{
		try
		{
			DataTable dtBank = new DataTable();
			dtBank.Columns.Add("Id", typeof(int));
			dtBank.Columns.Add("Bank");
			DataTable dt = GDBreakUpHeader.GetDataForDropDownFromGDBreakUpHeader(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Bank")
					{
						dtBank.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtBank, CmbBankMultiInvoiceAgainstGdHistory, "Id", "Bank", "Bank Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDaysMultiInvoiceForm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtExchangeRateMultiInvoiceForm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGdValueMultiInvoiceForm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFreightMultiInvoiceForm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FttPrcntMultiInvoiceForm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGdValueMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValueMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValueMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFobValueMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFTTAmountMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FttPrcntMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFTTAmountMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FttAmountMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetAmountMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateFobValueMultiInvoiceForm()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text);
			double freight = Conversion.ToDouble((object)txtFreightMultiInvoiceForm.Text);
			if (num > 0.0 && freight > 0.0 && Conversion.ToDouble((object)txtFreightMultiInvoiceForm.Text) > Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text))
			{
				MessageBox.Show("Freight Can't be Greater Than GD Value");
				txtFreightMultiInvoiceForm.Text = "0";
			}
			double Fob = Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text) - Conversion.ToDouble((object)txtFreightMultiInvoiceForm.Text);
			txtFobValueMultiInvoiceForm.Text = Fob.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateFTTAmountMultiInvoiceForm()
	{
		try
		{
			if (Conversion.ToDouble((object)FttPrcntMultiInvoiceForm.Text) > 100.0)
			{
				FttPrcntMultiInvoiceForm.Text = "10";
				MessageBox.Show("Percent Cant be Greater Than 100", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			double FTTAmount = Conversion.ToDouble((object)txtFobValueMultiInvoiceForm.Text) * Conversion.ToDouble((object)FttPrcntMultiInvoiceForm.Text) / 100.0;
			FttAmountMultiInvoiceForm.Text = FTTAmount.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateNetAmountMultiInvoiceForm()
	{
		try
		{
			if (Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text) > 0.0 && Conversion.ToDouble((object)FttAmountMultiInvoiceForm.Text) > 0.0)
			{
				double NetAmount = Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text) - Conversion.ToDouble((object)FttAmountMultiInvoiceForm.Text);
				txtNetAmountMultiInvoiceForm.Text = NetAmount.ToString("#,##0.###");
			}
			else
			{
				txtNetAmountMultiInvoiceForm.Text = 0.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbInvoiceNoMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbInvoiceNoMultiInvoiceForm).ActiveRow != null)
			{
				txtInvoiceAmountMultiInvoiceForm.Text = Conversion.ToDouble(((UltraDropDownBase)CmbInvoiceNoMultiInvoiceForm).SelectedRow.Cells[2].Value).ToString("#,##0.####");
			}
			else
			{
				txtInvoiceAmountMultiInvoiceForm.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool MultiInvoiceAgainstGdHeaderValidation()
	{
		if (GdNoMultiInvoiceForm.Text == null || GdNoMultiInvoiceForm.Text == string.Empty)
		{
			MessageBox.Show("Gd No field is required");
			GdNoMultiInvoiceForm.Focus();
			return false;
		}
		if (BankInvoiceNoMultiInvoiceForm.Text == null || BankInvoiceNoMultiInvoiceForm.Text == string.Empty)
		{
			MessageBox.Show("Bank Invoice No field is required");
			BankInvoiceNoMultiInvoiceForm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbBankMultiInvoiceForm.Value) <= 0)
		{
			MessageBox.Show("Bank field is Required");
			CmbBankMultiInvoiceForm.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtDueDaysMultiInvoiceForm.Text.Trim()) <= 0)
		{
			MessageBox.Show("Due Days field is Required");
			txtDueDaysMultiInvoiceForm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("GD Value field is Required");
			txtGdValueMultiInvoiceForm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFobValueMultiInvoiceForm.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("FOb Value field is Required");
			txtFobValueMultiInvoiceForm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNetAmountMultiInvoiceForm.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Net Amount field is Required");
			txtNetAmountMultiInvoiceForm.Focus();
			return false;
		}
		return true;
	}

	private bool MultiInvoiceAgainstGdDetailValidation()
	{
		if (Conversion.ToInt(CmbInvoiceNoMultiInvoiceForm.Value) <= 0)
		{
			MessageBox.Show("Invoice field is Required");
			CmbInvoiceNoMultiInvoiceForm.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtInvoiceAmountMultiInvoiceForm.Text.Trim()) <= 0m)
		{
			MessageBox.Show("Invoice Amount Should Greater Than 0");
			txtInvoiceAmountMultiInvoiceForm.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		try
		{
			if (grdMultiInvoiceAgainstGd.GetRows().Count() <= 0)
			{
				throw new Exception("Invoice Grid Record not found");
			}
			if (!MultiInvoiceAgainstGdHeaderValidation())
			{
				return;
			}
			GDBreakUpHeader obj = new GDBreakUpHeader();
			if (MultiInvoiceRecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = 0;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = MultiInvoiceRecId;
			}
			if (grdMultiInvoiceAgainstGd.RowCount <= 0)
			{
				return;
			}
			obj.GDNO = GdNoMultiInvoiceForm.Text;
			obj.GDDate = GdDateMultiInvoiceForm.Value;
			obj.BankInvoiceNo = BankInvoiceNoMultiInvoiceForm.Text;
			obj.BankInvoiceDate = BankInvoiceDateMultiInvoiceForm.Value;
			obj.BankId = Conversion.ToInt(CmbBankMultiInvoiceForm.Value);
			obj.DueDays = Conversion.ToInt((object)txtDueDaysMultiInvoiceForm.Text);
			obj.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRateMultiInvoiceForm.Text);
			obj.GDValue = Conversion.ToDecimal((object)txtGdValueMultiInvoiceForm.Text);
			obj.Freight = Conversion.ToDecimal((object)txtFreightMultiInvoiceForm.Text);
			obj.FobValue = Conversion.ToDecimal((object)txtFobValueMultiInvoiceForm.Text);
			obj.CommPercent = Conversion.ToDecimal((object)FttPrcntMultiInvoiceForm.Text);
			obj.CommAmount = Conversion.ToDecimal((object)FttAmountMultiInvoiceForm.Text);
			obj.NetToBeRealized = Conversion.ToDecimal((object)txtNetAmountMultiInvoiceForm.Text);
			obj.Remarks = txtRemarksMultiInvoiceForm.Text;
			obj.GDWeight = 0m;
			obj.GDNoofContrainers = 0;
			obj.DocumentTypeId = 2;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.GDBreakUpDetailList = new List<GDBreakUpDetail>();
			if (lstRemoveRecordOfInvoiceFromGd.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecordOfInvoiceFromGd.Count; i++)
				{
					obj.GDBreakUpDetailList.Add(lstRemoveRecordOfInvoiceFromGd[i]);
				}
			}
			decimal InvoiceAmount = default(decimal);
			GridEXRow[] rows = grdMultiInvoiceAgainstGd.GetRows();
			foreach (GridEXRow r in rows)
			{
				GDBreakUpDetail vd = new GDBreakUpDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				vd.InvoiceAmount = Conversion.ToDecimal(r.Cells["InvoiceAmount"].Value);
				vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				obj.GDBreakUpDetailList.Add(vd);
				InvoiceAmount += vd.InvoiceAmount;
			}
			if (obj.GDValue != InvoiceAmount)
			{
				throw new Exception("Invoice's Amount must be equal to Total GDValue Please Check");
			}
			GDBreakUpHeader.Save(obj);
			if (MultiInvoiceRecId == 0)
			{
				MessageBox.Show("Record Save SuccessFully");
			}
			else
			{
				MessageBox.Show("Record UpDate SuccessFully");
			}
			ResetMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateMultiInvoiceGd_Click(object sender, EventArgs e)
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

	private void btnSaveMultiInvoiceGd_Click(object sender, EventArgs e)
	{
		try
		{
			MultiInvoiceRecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddMultiInvoiceForm_Click(object sender, EventArgs e)
	{
		try
		{
			if (!MultiInvoiceAgainstGdDetailValidation())
			{
				return;
			}
			if (grdMultiInvoiceAgainstGd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdMultiInvoiceAgainstGd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(CmbInvoiceNoMultiInvoiceForm.Value) == Conversion.ToInt(item.Cells["InvoiceId"].Value))
					{
						MessageBox.Show("Invoice " + ((Control)(object)CmbInvoiceNoMultiInvoiceForm).Text + " Already in Detail Grid.So you can't select this invoice again.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
			}
			dtDetailForMultiInvoiceForm.Rows.Add(0, Conversion.ToInt(CmbInvoiceNoMultiInvoiceForm.Value), ((Control)(object)CmbInvoiceNoMultiInvoiceForm).Text, Conversion.ToDouble((object)txtInvoiceAmountMultiInvoiceForm.Text), txtDetailRemarksMultiInvoiceToGdForm.Text);
			grdMultiInvoiceAgainstGd.DataSource = dtDetailForMultiInvoiceForm;
			grdMultiInvoiceAgainstGd.RetrieveStructure();
			grdSettingsForMultiInvoiceForm();
			CmbInvoiceNoMultiInvoiceForm.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsForMultiInvoiceForm()
	{
		try
		{
			grdMultiInvoiceAgainstGd.RootTable.Columns["Id"].Visible = false;
			grdMultiInvoiceAgainstGd.RootTable.Columns["InvoiceId"].Visible = false;
			grdMultiInvoiceAgainstGd.RootTable.Columns["InvoiceNo"].Width = 350;
			grdMultiInvoiceAgainstGd.RootTable.Columns["InvoiceAmount"].Width = 120;
			grdMultiInvoiceAgainstGd.RootTable.Columns["Remarks"].Width = 520;
			grdMultiInvoiceAgainstGd.RootTable.Columns["InvoiceAmount"].AggregateFunction = (AggregateFunction)2;
			grdMultiInvoiceAgainstGd.RootTable.Columns["InvoiceAmount"].TextAlignment = (TextAlignment)3;
			grdMultiInvoiceAgainstGd.RootTable.Columns["InvoiceAmount"].FormatString = "#,##0.####";
			grdMultiInvoiceAgainstGd.RootTable.Columns["InvoiceAmount"].TotalFormatString = "#,##0.####";
			grdMultiInvoiceAgainstGd.RootTable.Columns.Add("Delete");
			grdMultiInvoiceAgainstGd.RootTable.Columns["Delete"].Caption = "X";
			grdMultiInvoiceAgainstGd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdMultiInvoiceAgainstGd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdMultiInvoiceAgainstGd.RootTable.Columns["Delete"].ButtonText = "X";
			grdMultiInvoiceAgainstGd.RootTable.Columns["Delete"].Width = 20;
			grdMultiInvoiceAgainstGd.RootTable.Columns["Delete"].Position = 0;
			ctrlGrdBar8_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMultiInvoiceAgainstGd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdMultiInvoiceAgainstGd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				UpdateDetailIndexForMultiInvoiceToGd = item.RowIndex;
				CmbInvoiceNoMultiInvoiceForm.Value = item.Cells["InvoiceId"].Value;
				txtInvoiceAmountMultiInvoiceForm.Text = Conversion.ToDouble(item.Cells["InvoiceAmount"].Value).ToString("#,##0.###");
				txtDetailRemarksMultiInvoiceToGdForm.Text = item.Cells["Remarks"].Value.ToString();
				btnAddMultiInvoiceForm.Visible = false;
				btnUpdateDetailMultiInvoiceForm.Visible = true;
				btnCancelDetailMultiInvoiceForm.Visible = true;
				CmbInvoiceNoMultiInvoiceForm.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetailMultiInvoiceForm_Click(object sender, EventArgs e)
	{
		try
		{
			if (!MultiInvoiceAgainstGdDetailValidation())
			{
				return;
			}
			if (grdMultiInvoiceAgainstGd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdMultiInvoiceAgainstGd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt((object)grdMultiInvoiceAgainstGd.CurrentRow.RowIndex) != Conversion.ToInt((object)item.RowIndex) && Conversion.ToInt(CmbInvoiceNoMultiInvoiceForm.Value) == Conversion.ToInt(item.Cells["InvoiceId"].Value))
					{
						MessageBox.Show("Invoice " + ((Control)(object)CmbInvoiceNoMultiInvoiceForm).Text + " Already in Detail Grid.So you can't select this invoice again.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
			}
			dtDetailForMultiInvoiceForm.Rows[UpdateDetailIndexForMultiInvoiceToGd]["InvoiceId"] = Conversion.ToInt(CmbInvoiceNoMultiInvoiceForm.Value);
			dtDetailForMultiInvoiceForm.Rows[UpdateDetailIndexForMultiInvoiceToGd]["InvoiceNo"] = Conversion.ToString((object)((Control)(object)CmbInvoiceNoMultiInvoiceForm).Text);
			dtDetailForMultiInvoiceForm.Rows[UpdateDetailIndexForMultiInvoiceToGd]["InvoiceAmount"] = Conversion.ToDouble((object)txtInvoiceAmountMultiInvoiceForm.Text);
			dtDetailForMultiInvoiceForm.Rows[UpdateDetailIndexForMultiInvoiceToGd]["Remarks"] = Conversion.ToString((object)txtDetailRemarksMultiInvoiceToGdForm.Text);
			btnAddMultiInvoiceForm.Visible = true;
			btnUpdateDetailMultiInvoiceForm.Visible = false;
			btnCancelDetailMultiInvoiceForm.Visible = false;
			DetailFormResetMultiInvoiceAgainstGd();
			CmbInvoiceNoMultiInvoiceForm.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetailMultiInvoiceForm_Click(object sender, EventArgs e)
	{
		try
		{
			DetailFormResetMultiInvoiceAgainstGd();
			btnAddMultiInvoiceForm.Visible = true;
			btnUpdateDetailMultiInvoiceForm.Visible = false;
			btnCancelDetailMultiInvoiceForm.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMultiInvoiceAgainstGd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMultiInvoiceAgainstGd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					GDBreakUpDetail vd = new GDBreakUpDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					vd.InvoiceAmount = Conversion.ToDecimal(r.Cells["InvoiceAmount"].Value);
					vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecordOfInvoiceFromGd.Add(vd);
					dtDetailForMultiInvoiceForm.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			grdMultiInvoiceAgainstGd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMultiInvoiceAgainstGd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMultiInvoiceAgainstGd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdMultiInvoiceAgainstGd.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space)
				{
					if (grdMultiInvoiceAgainstGd.CurrentColumn.Key == "Delete")
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
						{
							GDBreakUpDetail vd = new GDBreakUpDetail();
							vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
							vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
							vd.InvoiceAmount = Conversion.ToDecimal(r.Cells["InvoiceAmount"].Value);
							vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
							vd.ActionTypeId = 3;
							lstRemoveRecordOfInvoiceFromGd.Add(vd);
							dtDetailForMultiInvoiceForm.Rows[r.RowIndex].Delete();
							if (r != null && (int)r.RowType == 0)
							{
								r.Delete();
							}
						}
						else if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
					}
				}
				else if (e.Control && e.KeyCode == Keys.Return)
				{
					grdMultiInvoiceAgainstGd_DoubleClick(null, null);
				}
			}
			grdMultiInvoiceAgainstGd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetMultiInvoiceGd_Click(object sender, EventArgs e)
	{
		try
		{
			ResetMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetMultiInvoiceForm()
	{
		try
		{
			MultiInvoiceRecId = 0;
			GdNoMultiInvoiceForm.Text = string.Empty;
			BankInvoiceNoMultiInvoiceForm.Text = string.Empty;
			((Control)(object)CmbBankMultiInvoiceForm).Text = string.Empty;
			txtDueDaysMultiInvoiceForm.Text = string.Empty;
			txtExchangeRateMultiInvoiceForm.Text = string.Empty;
			txtGdValueMultiInvoiceForm.Text = string.Empty;
			txtFreightMultiInvoiceForm.Text = string.Empty;
			txtFobValueMultiInvoiceForm.Text = string.Empty;
			FttPrcntMultiInvoiceForm.Text = "10";
			FttAmountMultiInvoiceForm.Text = string.Empty;
			txtNetAmountMultiInvoiceForm.Text = string.Empty;
			txtRemarksMultiInvoiceForm.Text = string.Empty;
			UpdateDetailIndexForMultiInvoiceToGd = -1;
			((Control)(object)CmbInvoiceNoMultiInvoiceForm).Text = string.Empty;
			txtInvoiceAmountMultiInvoiceForm.Text = string.Empty;
			txtDetailRemarksMultiInvoiceToGdForm.Text = string.Empty;
			btnAddMultiInvoiceForm.Visible = true;
			btnUpdateDetailMultiInvoiceForm.Visible = false;
			btnCancelDetailMultiInvoiceForm.Visible = false;
			dtDetailForMultiInvoiceForm.Rows.Clear();
			grdMultiInvoiceAgainstGd.DataSource = dtDetailForMultiInvoiceForm;
			GdNoMultiInvoiceForm.Focus();
			btnSaveMultiInvoiceGd.Visible = true;
			btnUpdateMultiInvoiceGd.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailFormResetMultiInvoiceAgainstGd()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbInvoiceNoMultiInvoiceForm).Text = string.Empty;
			txtInvoiceAmountMultiInvoiceForm.Text = string.Empty;
			txtDetailRemarksMultiInvoiceToGdForm.Text = string.Empty;
			btnAddMultiInvoiceForm.Visible = true;
			btnUpdateDetailMultiInvoiceForm.Visible = false;
			btnCancelDetailMultiInvoiceForm.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetMultiInvoiceToGdHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateMultiInvoiceAgainstGd.Value = DateTime.Now;
			ToDateMultiInvoiceAgainstGd.Value = DateTime.Now;
			((Control)(object)CmbBankMultiInvoiceAgainstGdHistory).Text = string.Empty;
			DataGridHistoryMultiInvoiceToGdForm.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshMultiInvoiceGd_Click(object sender, EventArgs e)
	{
		try
		{
			ImporterandExportBankFill();
			GetBankInvoicesForMultiInvoiceToGdForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshMultiInvoiceToGdHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFillForMultiInvoicesToGdForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar8_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdMultiInvoiceAgainstGd).Name);
			if (fs != null)
			{
				grdMultiInvoiceAgainstGd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar9_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistoryMultiInvoiceToGdForm).Name);
			if (fs != null)
			{
				DataGridHistoryMultiInvoiceToGdForm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowMultiInvoicesToGdForm_Click(object sender, EventArgs e)
	{
		try
		{
			MultiInvoiceToGdFormHistoryBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiInvoiceToGdFormHistoryBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			DataTable dt = GDBreakUpHeader.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDateMultiInvoiceAgainstGd.Value,
				ToDate = ToDateMultiInvoiceAgainstGd.Value,
				AccountId = Conversion.ToInt(CmbBankMultiInvoiceAgainstGdHistory.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("InvoiceNo's", typeof(string));
				dtHistory.Columns.Add("GDNo", typeof(string));
				dtHistory.Columns.Add("GDDate", typeof(DateTime));
				dtHistory.Columns.Add("BankInvoiceNo", typeof(string));
				dtHistory.Columns.Add("BankInvoiceDate", typeof(DateTime));
				dtHistory.Columns.Add("BankName", typeof(string));
				dtHistory.Columns.Add("DueDays", typeof(int));
				dtHistory.Columns.Add("ExchangeRate", typeof(double));
				dtHistory.Columns.Add("GDValue", typeof(double));
				dtHistory.Columns.Add("Freight", typeof(double));
				dtHistory.Columns.Add("FobValue", typeof(double));
				dtHistory.Columns.Add("Ftt%", typeof(double));
				dtHistory.Columns.Add("FttAmount", typeof(double));
				dtHistory.Columns.Add("NetAmount", typeof(double));
				dtHistory.Columns.Add("EntryUser", typeof(string));
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser", typeof(string));
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("Remarks", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["InvoiceNo"], dt.Rows[i]["GDNO"], Conversion.ToDateTime(dt.Rows[i]["GDDate"]).ToShortDateString(), dt.Rows[i]["BankInvoiceNo"], Conversion.ToDateTime(dt.Rows[i]["BankInvoiceDate"]).ToShortDateString(), dt.Rows[i]["BankName"], dt.Rows[i]["DueDays"], dt.Rows[i]["ExchangeRate"], dt.Rows[i]["GDValue"], dt.Rows[i]["Freight"], dt.Rows[i]["FobValue"], dt.Rows[i]["CommPercent"], dt.Rows[i]["CommAmount"], dt.Rows[i]["NetToBeRealized"], dt.Rows[i]["EntryUser"], Conversion.ToDateTime(dt.Rows[i]["EntryDate"]).ToShortDateString(), dt.Rows[i]["ModifyUser"], Conversion.ToDateTime(dt.Rows[i]["ModifyDate"]).ToShortDateString(), dt.Rows[i]["Remarks"]);
				}
				DataGridHistoryMultiInvoiceToGdForm.DataSource = dtHistory;
				DataGridHistoryMultiInvoiceToGdForm.RetrieveStructure();
				DataGridHistoryMultiInvoiceToGdForm.RootTable.Columns["Id"].Visible = false;
				CommonServices.GridAutoAdjustmentNew(DataGridHistoryMultiInvoiceToGdForm);
				DataGridHistoryMultiInvoiceToGdForm.RootTable.Columns.Add("Edit");
				DataGridHistoryMultiInvoiceToGdForm.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistoryMultiInvoiceToGdForm.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistoryMultiInvoiceToGdForm.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistoryMultiInvoiceToGdForm.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistoryMultiInvoiceToGdForm.RootTable.Columns["Edit"].Width = 40;
				DataGridHistoryMultiInvoiceToGdForm.RootTable.Columns["Edit"].Position = 0;
				DataGridHistoryMultiInvoiceToGdForm.FrozenColumns = 1;
				ctrlGrdBar9_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadByIdMultiInvoiceToGdForm(int DocId)
	{
		try
		{
			ResetMultiInvoiceForm();
			MultiInvoiceRecId = DocId;
			GDBreakUpHeader obj = GDBreakUpHeader.GetByID(DocId);
			if (obj != null)
			{
				GdNoMultiInvoiceForm.Text = obj.GDNO;
				GdDateMultiInvoiceForm.Value = obj.GDDate;
				BankInvoiceNoMultiInvoiceForm.Text = obj.BankInvoiceNo;
				BankInvoiceDateMultiInvoiceForm.Value = obj.BankInvoiceDate;
				CmbBankMultiInvoiceForm.Value = obj.BankId;
				txtDueDaysMultiInvoiceForm.Text = obj.DueDays.ToString();
				txtExchangeRateMultiInvoiceForm.Text = obj.ExchangeRate.ToString("#,##0.####");
				txtGdValueMultiInvoiceForm.Text = obj.GDValue.ToString("#,##0.####");
				txtFreightMultiInvoiceForm.Text = obj.Freight.ToString("#,##0.####");
				txtFobValueMultiInvoiceForm.Text = obj.FobValue.ToString("#,##0.####");
				FttPrcntMultiInvoiceForm.Text = obj.CommPercent.ToString("#,##0.####");
				FttAmountMultiInvoiceForm.Text = obj.CommAmount.ToString("#,##0.####");
				txtNetAmountMultiInvoiceForm.Text = obj.NetToBeRealized.ToString("#,##0.####");
				txtRemarksMultiInvoiceForm.Text = obj.Remarks;
				dtDetailForMultiInvoiceForm.Rows.Clear();
				for (int i = 0; i < obj.GDBreakUpDetailList.Count; i++)
				{
					dtDetailForMultiInvoiceForm.Rows.Add(obj.GDBreakUpDetailList[i].Id, obj.GDBreakUpDetailList[i].ExImInvoiceId, obj.GDBreakUpDetailList[i].InvoiceNo, obj.GDBreakUpDetailList[i].InvoiceAmount, obj.GDBreakUpDetailList[i].Remarks);
				}
				grdMultiInvoiceAgainstGd.DataSource = dtDetailForMultiInvoiceForm;
				grdMultiInvoiceAgainstGd.RetrieveStructure();
				grdSettingsForMultiInvoiceForm();
				btnSaveMultiInvoiceGd.Visible = false;
				btnUpdateMultiInvoiceGd.Visible = true;
				GetBankInvoicesForMultiInvoiceToGdForm();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryMultiInvoiceToGdForm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistoryMultiInvoiceToGdForm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadByIdMultiInvoiceToGdForm(id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryMultiInvoiceToGdForm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistoryMultiInvoiceToGdForm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadByIdMultiInvoiceToGdForm(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryMultiInvoiceToGdForm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistoryMultiInvoiceToGdForm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistoryMultiInvoiceToGdForm.CurrentColumn == null)
			{
				return;
			}
			int id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistoryMultiInvoiceToGdForm.CurrentColumn.Key == "Edit")
				{
					ReadByIdMultiInvoiceToGdForm(id);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				DataGridHistoryMultiInvoiceToGdForm_DoubleClick(null, null);
			}
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

	private void btnCancelAdvanceUtilize_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddAdvanceUtlize.Visible = true;
			btnUpdateAdvanceUtilize.Visible = false;
			btnCancelAdvanceUtilize.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGdBreakupManual_Click(object sender, EventArgs e)
	{
		try
		{
			new GdBreakUpManual(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPaymentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			new FinancialInstrumentsUtilizedAgainstShipment(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpaymentdetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdpaymentdetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				if (updateDetailIndexOther != -1)
				{
					throw new Exception("Reset Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					throw new Exception("You can not Delete this Row...");
				}
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl3.SelectedIndex != 1)
			{
				return;
			}
			DataTable dt = ExImInvoice.AdvanceUtilizeagainstGDs_FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt != null && dt.Rows.Count > 0)
			{
				DataTable dthis = new DataTable();
				dthis.Columns.Add("Id");
				dthis.Columns.Add("InvoiceId");
				dthis.Columns.Add("InvoiceNo");
				dthis.Columns.Add("GDId");
				dthis.Columns.Add("GDNo");
				dthis.Columns.Add("DocumentTypeId");
				dthis.Columns.Add("FIId");
				dthis.Columns.Add("FINo");
				dthis.Columns.Add("UtilizeAmount", typeof(double));
				foreach (DataRow dr in dt.Rows)
				{
					dthis.Rows.Add(dr["Id"], dr["EximInvoiceId"], dr["InvoiceNo"], dr["GDId"], dr["GDNO"], dr["RefDocumentTypeId"], dr["FIId"], dr["FINo"], dr["UtilizeAmount"]);
				}
				grdAdvanceHistory.DataSource = dthis;
				grdAdvanceHistory.RetrieveStructure();
				grdAdvanceHistory.RootTable.Columns["Id"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["GDId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["InvoiceId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["FIId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["UtilizeAmount"].AggregateFunction = (AggregateFunction)2;
				grdAdvanceHistory.RootTable.Columns["UtilizeAmount"].TextAlignment = (TextAlignment)3;
				grdAdvanceHistory.RootTable.Columns["UtilizeAmount"].FormatString = "#,##0.##";
				grdAdvanceHistory.RootTable.Columns["UtilizeAmount"].TotalFormatString = "#,##0.##";
				grdAdvanceHistory.AllowDelete = (InheritableBoolean)2;
				grdAdvanceHistory.AllowEdit = (InheritableBoolean)2;
			}
			else
			{
				grdAdvanceHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAdvanceHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdAdvanceHistory.CurrentRow;
			if (item != null)
			{
				tabControl3.SelectedIndex = 0;
				dtAdvanceUtilize.Rows.Clear();
				dtAdvanceUtilize.Rows.Add(item.Cells["Id"].Value, item.Cells["InvoiceId"].Value, item.Cells["InvoiceNo"].Value, item.Cells["GDId"].Value, item.Cells["GDNO"].Value, item.Cells["DocumentTypeId"].Value, item.Cells["FIId"].Value, item.Cells["FINo"].Value, Conversion.ToDouble(item.Cells["UtilizeAmount"].Value));
				grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
				grdAdvanceUtlize.RetrieveStructure();
				grdSettingsAdvanceUtilize();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLessRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddLessRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateNetRate()
	{
		try
		{
			if (Conversion.ToDouble((object)txtAddLessRate.Text) != 0.0)
			{
				double NetRate = Conversion.ToDouble((object)txtRateWithoutAddLess.Text) + Conversion.ToDouble((object)txtAddLessRate.Text);
				txtCostMTon.Text = NetRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			else
			{
				txtCostMTon.Text = Conversion.ToDouble((object)txtRateWithoutAddLess.Text).ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRateWithoutAddLess_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPrefixType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbPrefixType).ActiveRow != null || Conversion.ToInt(cmbPrefixType.Value) > 0)
			{
				((Control)(object)Cmbinvoiceno).Text = string.Empty;
				DataTable dt = new DataTable();
				EnumerableRowCollection<DataRow> rows = from row in dtIN.AsEnumerable()
					where row.Field<int>("PrefixTypeId") == Conversion.ToInt(cmbPrefixType.Value)
					select row;
				dt = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
				DDL.BindDDL(dt, Cmbinvoiceno, "Id", "InvoiceNo", "Commercial-Invoice_Number", ZeroIndex: false);
				((UltraGridBase)Cmbinvoiceno).DisplayLayout.Bands[0].Columns[1].Hidden = true;
				((UltraGridBase)Cmbinvoiceno).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)Cmbinvoiceno).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((UltraGridBase)Cmbinvoiceno).DataSource = dtIN;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlipHistory_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtSlip = ExportPdfReport.ExportPreInvoiceSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 211,
				FromDocNo = ((Conversion.ToInt((object)FromDocNoHistory.Text.Trim()) > 0) ? Conversion.ToInt((object)FromDocNoHistory.Text.Trim()) : 0),
				ToDocNo = ((Conversion.ToInt((object)ToDocNoHistory.Text.Trim()) > 0) ? Conversion.ToInt((object)ToDocNoHistory.Text.Trim()) : 0)
			});
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSlip, "549A-ExportCommercialInvoiceSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBankSlipHistory_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtSlip = ExportPdfReport.ExportPreInvoiceSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 211,
				FromDocNo = ((Conversion.ToInt((object)FromDocNoHistory.Text.Trim()) > 0) ? Conversion.ToInt((object)FromDocNoHistory.Text.Trim()) : 0),
				ToDocNo = ((Conversion.ToInt((object)ToDocNoHistory.Text.Trim()) > 0) ? Conversion.ToInt((object)ToDocNoHistory.Text.Trim()) : 0)
			});
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSlip, "548A-ExportCommercialInvoiceSlipForBank.rpt");
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
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Expected O, but got Unknown
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Expected O, but got Unknown
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Expected O, but got Unknown
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Expected O, but got Unknown
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Expected O, but got Unknown
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Expected O, but got Unknown
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Expected O, but got Unknown
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Expected O, but got Unknown
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Expected O, but got Unknown
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4d: Expected O, but got Unknown
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Expected O, but got Unknown
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a79: Expected O, but got Unknown
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Expected O, but got Unknown
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Expected O, but got Unknown
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Expected O, but got Unknown
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Expected O, but got Unknown
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Expected O, but got Unknown
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1e: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Expected O, but got Unknown
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Expected O, but got Unknown
		//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b54: Expected O, but got Unknown
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Expected O, but got Unknown
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Expected O, but got Unknown
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Expected O, but got Unknown
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Expected O, but got Unknown
		//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b96: Expected O, but got Unknown
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba1: Expected O, but got Unknown
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Expected O, but got Unknown
		//IL_0bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Expected O, but got Unknown
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc2: Expected O, but got Unknown
		//IL_0bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcd: Expected O, but got Unknown
		//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Expected O, but got Unknown
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Expected O, but got Unknown
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bee: Expected O, but got Unknown
		//IL_0bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf9: Expected O, but got Unknown
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c04: Expected O, but got Unknown
		//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0f: Expected O, but got Unknown
		//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1a: Expected O, but got Unknown
		//IL_0c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c25: Expected O, but got Unknown
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c30: Expected O, but got Unknown
		//IL_0c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3b: Expected O, but got Unknown
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c46: Expected O, but got Unknown
		//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Expected O, but got Unknown
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5c: Expected O, but got Unknown
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c67: Expected O, but got Unknown
		//IL_0c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c72: Expected O, but got Unknown
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Expected O, but got Unknown
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c88: Expected O, but got Unknown
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Expected O, but got Unknown
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9e: Expected O, but got Unknown
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Expected O, but got Unknown
		//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Expected O, but got Unknown
		//IL_0cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbf: Expected O, but got Unknown
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cca: Expected O, but got Unknown
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Expected O, but got Unknown
		//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce0: Expected O, but got Unknown
		//IL_0ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ceb: Expected O, but got Unknown
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf6: Expected O, but got Unknown
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d01: Expected O, but got Unknown
		//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0c: Expected O, but got Unknown
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d17: Expected O, but got Unknown
		//IL_0d19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Expected O, but got Unknown
		//IL_0d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2d: Expected O, but got Unknown
		//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d38: Expected O, but got Unknown
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d43: Expected O, but got Unknown
		//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4e: Expected O, but got Unknown
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d59: Expected O, but got Unknown
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d64: Expected O, but got Unknown
		//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6f: Expected O, but got Unknown
		//IL_0d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7a: Expected O, but got Unknown
		//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Expected O, but got Unknown
		//IL_0d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d90: Expected O, but got Unknown
		//IL_0d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9b: Expected O, but got Unknown
		//IL_0d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Expected O, but got Unknown
		//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db1: Expected O, but got Unknown
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbc: Expected O, but got Unknown
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc7: Expected O, but got Unknown
		//IL_0dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd2: Expected O, but got Unknown
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddd: Expected O, but got Unknown
		//IL_0ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de8: Expected O, but got Unknown
		//IL_0dea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Expected O, but got Unknown
		//IL_0df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfe: Expected O, but got Unknown
		//IL_0e00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e09: Expected O, but got Unknown
		//IL_0e0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Expected O, but got Unknown
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1f: Expected O, but got Unknown
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2a: Expected O, but got Unknown
		//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e35: Expected O, but got Unknown
		//IL_0e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e40: Expected O, but got Unknown
		//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4b: Expected O, but got Unknown
		//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e56: Expected O, but got Unknown
		//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Expected O, but got Unknown
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6c: Expected O, but got Unknown
		//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e77: Expected O, but got Unknown
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e82: Expected O, but got Unknown
		//IL_0e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8d: Expected O, but got Unknown
		//IL_0e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e98: Expected O, but got Unknown
		//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea3: Expected O, but got Unknown
		//IL_0ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Expected O, but got Unknown
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb9: Expected O, but got Unknown
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec4: Expected O, but got Unknown
		//IL_0ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecf: Expected O, but got Unknown
		//IL_0ed1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eda: Expected O, but got Unknown
		//IL_0edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee5: Expected O, but got Unknown
		//IL_0ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef0: Expected O, but got Unknown
		//IL_0ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efb: Expected O, but got Unknown
		//IL_0efd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Expected O, but got Unknown
		//IL_0f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f11: Expected O, but got Unknown
		//IL_0f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1c: Expected O, but got Unknown
		//IL_0f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f27: Expected O, but got Unknown
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Expected O, but got Unknown
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3d: Expected O, but got Unknown
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f48: Expected O, but got Unknown
		//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f53: Expected O, but got Unknown
		//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5e: Expected O, but got Unknown
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Expected O, but got Unknown
		//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Expected O, but got Unknown
		//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Expected O, but got Unknown
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8a: Expected O, but got Unknown
		//IL_0f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f95: Expected O, but got Unknown
		//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa0: Expected O, but got Unknown
		//IL_0fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fab: Expected O, but got Unknown
		//IL_0fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb6: Expected O, but got Unknown
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc1: Expected O, but got Unknown
		//IL_0fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcc: Expected O, but got Unknown
		//IL_0fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd7: Expected O, but got Unknown
		//IL_0fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe2: Expected O, but got Unknown
		//IL_0fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fed: Expected O, but got Unknown
		//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff8: Expected O, but got Unknown
		//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1003: Expected O, but got Unknown
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_100e: Expected O, but got Unknown
		//IL_1010: Unknown result type (might be due to invalid IL or missing references)
		//IL_1019: Expected O, but got Unknown
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Expected O, but got Unknown
		//IL_1026: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Expected O, but got Unknown
		//IL_1031: Unknown result type (might be due to invalid IL or missing references)
		//IL_103a: Expected O, but got Unknown
		//IL_103c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1045: Expected O, but got Unknown
		//IL_1047: Unknown result type (might be due to invalid IL or missing references)
		//IL_1050: Expected O, but got Unknown
		//IL_1052: Unknown result type (might be due to invalid IL or missing references)
		//IL_105b: Expected O, but got Unknown
		//IL_105d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1066: Expected O, but got Unknown
		//IL_1068: Unknown result type (might be due to invalid IL or missing references)
		//IL_1071: Expected O, but got Unknown
		//IL_1073: Unknown result type (might be due to invalid IL or missing references)
		//IL_107c: Expected O, but got Unknown
		//IL_107e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1087: Expected O, but got Unknown
		//IL_1089: Unknown result type (might be due to invalid IL or missing references)
		//IL_1092: Expected O, but got Unknown
		//IL_1094: Unknown result type (might be due to invalid IL or missing references)
		//IL_109d: Expected O, but got Unknown
		//IL_109f: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Expected O, but got Unknown
		//IL_10aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b3: Expected O, but got Unknown
		//IL_10b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10be: Expected O, but got Unknown
		//IL_10c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Expected O, but got Unknown
		//IL_10cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d4: Expected O, but got Unknown
		//IL_10d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10df: Expected O, but got Unknown
		//IL_10e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ea: Expected O, but got Unknown
		//IL_10ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f5: Expected O, but got Unknown
		//IL_10f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1100: Expected O, but got Unknown
		//IL_1102: Unknown result type (might be due to invalid IL or missing references)
		//IL_110b: Expected O, but got Unknown
		//IL_110d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1116: Expected O, but got Unknown
		//IL_1118: Unknown result type (might be due to invalid IL or missing references)
		//IL_1121: Expected O, but got Unknown
		//IL_1123: Unknown result type (might be due to invalid IL or missing references)
		//IL_112c: Expected O, but got Unknown
		//IL_112e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1137: Expected O, but got Unknown
		//IL_1139: Unknown result type (might be due to invalid IL or missing references)
		//IL_1142: Expected O, but got Unknown
		//IL_1144: Unknown result type (might be due to invalid IL or missing references)
		//IL_114d: Expected O, but got Unknown
		//IL_114f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1158: Expected O, but got Unknown
		//IL_115a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1163: Expected O, but got Unknown
		//IL_1165: Unknown result type (might be due to invalid IL or missing references)
		//IL_116e: Expected O, but got Unknown
		//IL_1170: Unknown result type (might be due to invalid IL or missing references)
		//IL_1179: Expected O, but got Unknown
		//IL_117b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1184: Expected O, but got Unknown
		//IL_1186: Unknown result type (might be due to invalid IL or missing references)
		//IL_118f: Expected O, but got Unknown
		//IL_1191: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Expected O, but got Unknown
		//IL_119c: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a5: Expected O, but got Unknown
		//IL_11a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b0: Expected O, but got Unknown
		//IL_11b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bb: Expected O, but got Unknown
		//IL_11bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c6: Expected O, but got Unknown
		//IL_11c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d1: Expected O, but got Unknown
		//IL_11d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dc: Expected O, but got Unknown
		//IL_11de: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e7: Expected O, but got Unknown
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f2: Expected O, but got Unknown
		//IL_11f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fd: Expected O, but got Unknown
		//IL_11ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1208: Expected O, but got Unknown
		//IL_120a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Expected O, but got Unknown
		//IL_1215: Unknown result type (might be due to invalid IL or missing references)
		//IL_121e: Expected O, but got Unknown
		//IL_1220: Unknown result type (might be due to invalid IL or missing references)
		//IL_1229: Expected O, but got Unknown
		//IL_122b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1234: Expected O, but got Unknown
		//IL_1236: Unknown result type (might be due to invalid IL or missing references)
		//IL_123f: Expected O, but got Unknown
		//IL_1241: Unknown result type (might be due to invalid IL or missing references)
		//IL_124a: Expected O, but got Unknown
		//IL_124c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1255: Expected O, but got Unknown
		//IL_1257: Unknown result type (might be due to invalid IL or missing references)
		//IL_1260: Expected O, but got Unknown
		//IL_1262: Unknown result type (might be due to invalid IL or missing references)
		//IL_126b: Expected O, but got Unknown
		//IL_126d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1276: Expected O, but got Unknown
		//IL_1278: Unknown result type (might be due to invalid IL or missing references)
		//IL_1281: Expected O, but got Unknown
		//IL_1283: Unknown result type (might be due to invalid IL or missing references)
		//IL_128c: Expected O, but got Unknown
		//IL_128e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1297: Expected O, but got Unknown
		//IL_1299: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a2: Expected O, but got Unknown
		//IL_12a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ad: Expected O, but got Unknown
		//IL_12af: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b8: Expected O, but got Unknown
		//IL_12ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c3: Expected O, but got Unknown
		//IL_12c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ce: Expected O, but got Unknown
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d9: Expected O, but got Unknown
		//IL_12db: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e4: Expected O, but got Unknown
		//IL_12e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ef: Expected O, but got Unknown
		//IL_12f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Expected O, but got Unknown
		//IL_12fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1305: Expected O, but got Unknown
		//IL_1307: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Expected O, but got Unknown
		//IL_1312: Unknown result type (might be due to invalid IL or missing references)
		//IL_131b: Expected O, but got Unknown
		//IL_131d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1326: Expected O, but got Unknown
		//IL_1328: Unknown result type (might be due to invalid IL or missing references)
		//IL_1331: Expected O, but got Unknown
		//IL_1333: Unknown result type (might be due to invalid IL or missing references)
		//IL_133c: Expected O, but got Unknown
		//IL_133e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Expected O, but got Unknown
		//IL_1349: Unknown result type (might be due to invalid IL or missing references)
		//IL_1352: Expected O, but got Unknown
		//IL_1354: Unknown result type (might be due to invalid IL or missing references)
		//IL_135d: Expected O, but got Unknown
		//IL_135f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1368: Expected O, but got Unknown
		//IL_136a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1373: Expected O, but got Unknown
		//IL_1375: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Expected O, but got Unknown
		//IL_1380: Unknown result type (might be due to invalid IL or missing references)
		//IL_1389: Expected O, but got Unknown
		//IL_138b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1394: Expected O, but got Unknown
		//IL_1396: Unknown result type (might be due to invalid IL or missing references)
		//IL_139f: Expected O, but got Unknown
		//IL_13a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13aa: Expected O, but got Unknown
		//IL_13ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b5: Expected O, but got Unknown
		//IL_13b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c0: Expected O, but got Unknown
		//IL_13c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cb: Expected O, but got Unknown
		//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d6: Expected O, but got Unknown
		//IL_13d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e1: Expected O, but got Unknown
		//IL_13e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ec: Expected O, but got Unknown
		//IL_13ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f7: Expected O, but got Unknown
		//IL_13f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1402: Expected O, but got Unknown
		//IL_1404: Unknown result type (might be due to invalid IL or missing references)
		//IL_140d: Expected O, but got Unknown
		//IL_140f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1418: Expected O, but got Unknown
		//IL_141a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1423: Expected O, but got Unknown
		//IL_1425: Unknown result type (might be due to invalid IL or missing references)
		//IL_142e: Expected O, but got Unknown
		//IL_1430: Unknown result type (might be due to invalid IL or missing references)
		//IL_1439: Expected O, but got Unknown
		//IL_143b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1444: Expected O, but got Unknown
		//IL_1446: Unknown result type (might be due to invalid IL or missing references)
		//IL_144f: Expected O, but got Unknown
		//IL_1451: Unknown result type (might be due to invalid IL or missing references)
		//IL_145a: Expected O, but got Unknown
		//IL_145c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1465: Expected O, but got Unknown
		//IL_1467: Unknown result type (might be due to invalid IL or missing references)
		//IL_1470: Expected O, but got Unknown
		//IL_1472: Unknown result type (might be due to invalid IL or missing references)
		//IL_147b: Expected O, but got Unknown
		//IL_147d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1486: Expected O, but got Unknown
		//IL_1488: Unknown result type (might be due to invalid IL or missing references)
		//IL_1491: Expected O, but got Unknown
		//IL_1493: Unknown result type (might be due to invalid IL or missing references)
		//IL_149c: Expected O, but got Unknown
		//IL_149e: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a7: Expected O, but got Unknown
		//IL_14a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b2: Expected O, but got Unknown
		//IL_14b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bd: Expected O, but got Unknown
		//IL_14bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c8: Expected O, but got Unknown
		//IL_14ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d3: Expected O, but got Unknown
		//IL_14d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14de: Expected O, but got Unknown
		//IL_14e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e9: Expected O, but got Unknown
		//IL_14eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f4: Expected O, but got Unknown
		//IL_14f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ff: Expected O, but got Unknown
		//IL_1501: Unknown result type (might be due to invalid IL or missing references)
		//IL_150a: Expected O, but got Unknown
		//IL_150c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1515: Expected O, but got Unknown
		//IL_1517: Unknown result type (might be due to invalid IL or missing references)
		//IL_1520: Expected O, but got Unknown
		//IL_1522: Unknown result type (might be due to invalid IL or missing references)
		//IL_152b: Expected O, but got Unknown
		//IL_152d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1536: Expected O, but got Unknown
		//IL_1538: Unknown result type (might be due to invalid IL or missing references)
		//IL_1541: Expected O, but got Unknown
		//IL_1543: Unknown result type (might be due to invalid IL or missing references)
		//IL_154c: Expected O, but got Unknown
		//IL_154e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1557: Expected O, but got Unknown
		//IL_1559: Unknown result type (might be due to invalid IL or missing references)
		//IL_1562: Expected O, but got Unknown
		//IL_1564: Unknown result type (might be due to invalid IL or missing references)
		//IL_156d: Expected O, but got Unknown
		//IL_156f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1578: Expected O, but got Unknown
		//IL_157a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1583: Expected O, but got Unknown
		//IL_1585: Unknown result type (might be due to invalid IL or missing references)
		//IL_158e: Expected O, but got Unknown
		//IL_1590: Unknown result type (might be due to invalid IL or missing references)
		//IL_1599: Expected O, but got Unknown
		//IL_159b: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a4: Expected O, but got Unknown
		//IL_15a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15af: Expected O, but got Unknown
		//IL_15b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ba: Expected O, but got Unknown
		//IL_15bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c5: Expected O, but got Unknown
		//IL_15c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d0: Expected O, but got Unknown
		//IL_15d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15db: Expected O, but got Unknown
		//IL_15dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e6: Expected O, but got Unknown
		//IL_15e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f1: Expected O, but got Unknown
		//IL_15f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fc: Expected O, but got Unknown
		//IL_15fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1607: Expected O, but got Unknown
		//IL_1609: Unknown result type (might be due to invalid IL or missing references)
		//IL_1612: Expected O, but got Unknown
		//IL_1614: Unknown result type (might be due to invalid IL or missing references)
		//IL_161d: Expected O, but got Unknown
		//IL_161f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1628: Expected O, but got Unknown
		//IL_162a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1633: Expected O, but got Unknown
		//IL_1635: Unknown result type (might be due to invalid IL or missing references)
		//IL_163e: Expected O, but got Unknown
		//IL_1640: Unknown result type (might be due to invalid IL or missing references)
		//IL_1649: Expected O, but got Unknown
		//IL_164b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1654: Expected O, but got Unknown
		//IL_1656: Unknown result type (might be due to invalid IL or missing references)
		//IL_165f: Expected O, but got Unknown
		//IL_1661: Unknown result type (might be due to invalid IL or missing references)
		//IL_166a: Expected O, but got Unknown
		//IL_166c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1675: Expected O, but got Unknown
		//IL_1677: Unknown result type (might be due to invalid IL or missing references)
		//IL_1680: Expected O, but got Unknown
		//IL_1682: Unknown result type (might be due to invalid IL or missing references)
		//IL_168b: Expected O, but got Unknown
		//IL_168d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1696: Expected O, but got Unknown
		//IL_1698: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a1: Expected O, but got Unknown
		//IL_16a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ac: Expected O, but got Unknown
		//IL_16ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b7: Expected O, but got Unknown
		//IL_16b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c2: Expected O, but got Unknown
		//IL_16c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cd: Expected O, but got Unknown
		//IL_16cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d8: Expected O, but got Unknown
		//IL_1728: Unknown result type (might be due to invalid IL or missing references)
		//IL_1732: Expected O, but got Unknown
		//IL_18f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1900: Expected O, but got Unknown
		//IL_192d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1937: Expected O, but got Unknown
		//IL_1943: Unknown result type (might be due to invalid IL or missing references)
		//IL_194d: Expected O, but got Unknown
		//IL_1985: Unknown result type (might be due to invalid IL or missing references)
		//IL_198f: Expected O, but got Unknown
		//IL_19bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c6: Expected O, but got Unknown
		//IL_19fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a08: Expected O, but got Unknown
		//IL_1a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a29: Expected O, but got Unknown
		//IL_1ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ace: Expected O, but got Unknown
		//IL_1afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b05: Expected O, but got Unknown
		//IL_1b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b89: Expected O, but got Unknown
		//IL_1bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd6: Expected O, but got Unknown
		//IL_1be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bec: Expected O, but got Unknown
		//IL_1c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0d: Expected O, but got Unknown
		//IL_1c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c23: Expected O, but got Unknown
		//IL_1c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4f: Expected O, but got Unknown
		//IL_1c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c70: Expected O, but got Unknown
		//IL_1d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0a: Expected O, but got Unknown
		//IL_1d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6d: Expected O, but got Unknown
		//IL_1d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d78: Expected O, but got Unknown
		//IL_1ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de6: Expected O, but got Unknown
		//IL_1e08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e12: Expected O, but got Unknown
		//IL_1e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e28: Expected O, but got Unknown
		//IL_1e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8b: Expected O, but got Unknown
		//IL_1efa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f04: Expected O, but got Unknown
		//IL_1f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0f: Expected O, but got Unknown
		//IL_1f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f30: Expected O, but got Unknown
		//IL_1f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f51: Expected O, but got Unknown
		//IL_1f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f67: Expected O, but got Unknown
		//IL_1faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb4: Expected O, but got Unknown
		//IL_1fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd5: Expected O, but got Unknown
		//IL_20c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d2: Expected O, but got Unknown
		//IL_20ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2109: Expected O, but got Unknown
		//IL_218e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2198: Expected O, but got Unknown
		//IL_2254: Unknown result type (might be due to invalid IL or missing references)
		//IL_225e: Expected O, but got Unknown
		//IL_2275: Unknown result type (might be due to invalid IL or missing references)
		//IL_227f: Expected O, but got Unknown
		//IL_228b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2295: Expected O, but got Unknown
		//IL_2304: Unknown result type (might be due to invalid IL or missing references)
		//IL_230e: Expected O, but got Unknown
		//IL_2372: Unknown result type (might be due to invalid IL or missing references)
		//IL_237c: Expected O, but got Unknown
		//IL_2393: Unknown result type (might be due to invalid IL or missing references)
		//IL_239d: Expected O, but got Unknown
		//IL_2459: Unknown result type (might be due to invalid IL or missing references)
		//IL_2463: Expected O, but got Unknown
		//IL_24d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24dc: Expected O, but got Unknown
		//IL_25a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ad: Expected O, but got Unknown
		//IL_25e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ef: Expected O, but got Unknown
		//IL_265e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2668: Expected O, but got Unknown
		//IL_26a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26aa: Expected O, but got Unknown
		//IL_2703: Unknown result type (might be due to invalid IL or missing references)
		//IL_270d: Expected O, but got Unknown
		//IL_2787: Unknown result type (might be due to invalid IL or missing references)
		//IL_2791: Expected O, but got Unknown
		//IL_286e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2878: Expected O, but got Unknown
		//IL_2934: Unknown result type (might be due to invalid IL or missing references)
		//IL_293e: Expected O, but got Unknown
		//IL_293f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2949: Expected O, but got Unknown
		//IL_294a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2954: Expected O, but got Unknown
		//IL_2955: Unknown result type (might be due to invalid IL or missing references)
		//IL_295f: Expected O, but got Unknown
		//IL_2960: Unknown result type (might be due to invalid IL or missing references)
		//IL_296a: Expected O, but got Unknown
		//IL_296b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2975: Expected O, but got Unknown
		//IL_2976: Unknown result type (might be due to invalid IL or missing references)
		//IL_2980: Expected O, but got Unknown
		//IL_2981: Unknown result type (might be due to invalid IL or missing references)
		//IL_298b: Expected O, but got Unknown
		//IL_298c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2996: Expected O, but got Unknown
		//IL_2997: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a1: Expected O, but got Unknown
		//IL_29a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ac: Expected O, but got Unknown
		//IL_29ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b7: Expected O, but got Unknown
		//IL_29b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29c2: Expected O, but got Unknown
		//IL_3566: Unknown result type (might be due to invalid IL or missing references)
		//IL_3570: Expected O, but got Unknown
		//IL_815c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8166: Expected O, but got Unknown
		//IL_12bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bdc: Expected O, but got Unknown
		//IL_12c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c0a: Expected O, but got Unknown
		//IL_14634: Unknown result type (might be due to invalid IL or missing references)
		//IL_1463e: Expected O, but got Unknown
		//IL_165ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_165b6: Expected O, but got Unknown
		//IL_18bf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bfd: Expected O, but got Unknown
		//IL_19afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b04: Expected O, but got Unknown
		//IL_1a92a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a934: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance518 = new Appearance();
		Appearance appearance519 = new Appearance();
		Appearance appearance520 = new Appearance();
		Appearance appearance521 = new Appearance();
		Appearance appearance522 = new Appearance();
		Appearance appearance524 = new Appearance();
		Appearance appearance525 = new Appearance();
		Appearance appearance526 = new Appearance();
		Appearance appearance527 = new Appearance();
		Appearance appearance528 = new Appearance();
		Appearance appearance529 = new Appearance();
		Appearance appearance530 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance401 = new Appearance();
		Appearance appearance412 = new Appearance();
		Appearance appearance423 = new Appearance();
		Appearance appearance434 = new Appearance();
		Appearance appearance446 = new Appearance();
		Appearance appearance457 = new Appearance();
		Appearance appearance468 = new Appearance();
		Appearance appearance479 = new Appearance();
		Appearance appearance490 = new Appearance();
		Appearance appearance544 = new Appearance();
		Appearance appearance546 = new Appearance();
		Appearance appearance547 = new Appearance();
		Appearance appearance534 = new Appearance();
		Appearance appearance548 = new Appearance();
		Appearance appearance549 = new Appearance();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance550 = new Appearance();
		Appearance appearance572 = new Appearance();
		Appearance appearance573 = new Appearance();
		Appearance appearance551 = new Appearance();
		Appearance appearance552 = new Appearance();
		Appearance appearance553 = new Appearance();
		Appearance appearance554 = new Appearance();
		Appearance appearance555 = new Appearance();
		Appearance appearance558 = new Appearance();
		Appearance appearance581 = new Appearance();
		Appearance appearance582 = new Appearance();
		Appearance appearance583 = new Appearance();
		GridEXLayout grdpaymentdetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance597 = new Appearance();
		Appearance appearance598 = new Appearance();
		Appearance appearance599 = new Appearance();
		Appearance appearance600 = new Appearance();
		Appearance appearance601 = new Appearance();
		Appearance appearance603 = new Appearance();
		Appearance appearance604 = new Appearance();
		Appearance appearance605 = new Appearance();
		Appearance appearance606 = new Appearance();
		Appearance appearance607 = new Appearance();
		Appearance appearance608 = new Appearance();
		Appearance appearance609 = new Appearance();
		Appearance appearance438 = new Appearance();
		Appearance appearance439 = new Appearance();
		Appearance appearance440 = new Appearance();
		Appearance appearance441 = new Appearance();
		Appearance appearance442 = new Appearance();
		Appearance appearance443 = new Appearance();
		Appearance appearance444 = new Appearance();
		Appearance appearance447 = new Appearance();
		Appearance appearance448 = new Appearance();
		Appearance appearance449 = new Appearance();
		Appearance appearance450 = new Appearance();
		Appearance appearance451 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance415 = new Appearance();
		Appearance appearance416 = new Appearance();
		Appearance appearance417 = new Appearance();
		Appearance appearance418 = new Appearance();
		Appearance appearance419 = new Appearance();
		Appearance appearance420 = new Appearance();
		Appearance appearance421 = new Appearance();
		Appearance appearance422 = new Appearance();
		Appearance appearance424 = new Appearance();
		Appearance appearance584 = new Appearance();
		Appearance appearance585 = new Appearance();
		Appearance appearance586 = new Appearance();
		Appearance appearance587 = new Appearance();
		Appearance appearance588 = new Appearance();
		Appearance appearance589 = new Appearance();
		Appearance appearance590 = new Appearance();
		Appearance appearance592 = new Appearance();
		Appearance appearance593 = new Appearance();
		Appearance appearance594 = new Appearance();
		Appearance appearance595 = new Appearance();
		Appearance appearance596 = new Appearance();
		Appearance appearance145 = new Appearance();
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
		Appearance appearance158 = new Appearance();
		Appearance appearance610 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance611 = new Appearance();
		Appearance appearance612 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
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
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance559 = new Appearance();
		Appearance appearance560 = new Appearance();
		Appearance appearance561 = new Appearance();
		Appearance appearance562 = new Appearance();
		Appearance appearance563 = new Appearance();
		Appearance appearance564 = new Appearance();
		Appearance appearance565 = new Appearance();
		Appearance appearance566 = new Appearance();
		Appearance appearance567 = new Appearance();
		Appearance appearance569 = new Appearance();
		Appearance appearance570 = new Appearance();
		Appearance appearance571 = new Appearance();
		Appearance appearance402 = new Appearance();
		Appearance appearance403 = new Appearance();
		Appearance appearance404 = new Appearance();
		Appearance appearance405 = new Appearance();
		Appearance appearance406 = new Appearance();
		Appearance appearance407 = new Appearance();
		Appearance appearance408 = new Appearance();
		Appearance appearance409 = new Appearance();
		Appearance appearance410 = new Appearance();
		Appearance appearance411 = new Appearance();
		Appearance appearance413 = new Appearance();
		Appearance appearance414 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout gridDetailHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance501 = new Appearance();
		Appearance appearance512 = new Appearance();
		Appearance appearance523 = new Appearance();
		Appearance appearance545 = new Appearance();
		Appearance appearance557 = new Appearance();
		Appearance appearance568 = new Appearance();
		Appearance appearance574 = new Appearance();
		Appearance appearance575 = new Appearance();
		Appearance appearance576 = new Appearance();
		Appearance appearance577 = new Appearance();
		Appearance appearance578 = new Appearance();
		Appearance appearance579 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.CommiercialInvoiceAgainstPreInvoiceTransfer));
		GridEXLayout grdAdvanceUtlize_DesignTimeLayout = new GridEXLayout();
		Appearance appearance465 = new Appearance();
		Appearance appearance466 = new Appearance();
		Appearance appearance467 = new Appearance();
		Appearance appearance469 = new Appearance();
		Appearance appearance470 = new Appearance();
		Appearance appearance471 = new Appearance();
		Appearance appearance472 = new Appearance();
		Appearance appearance473 = new Appearance();
		Appearance appearance474 = new Appearance();
		Appearance appearance475 = new Appearance();
		Appearance appearance476 = new Appearance();
		Appearance appearance477 = new Appearance();
		Appearance appearance425 = new Appearance();
		Appearance appearance426 = new Appearance();
		Appearance appearance427 = new Appearance();
		Appearance appearance428 = new Appearance();
		Appearance appearance429 = new Appearance();
		Appearance appearance430 = new Appearance();
		Appearance appearance431 = new Appearance();
		Appearance appearance432 = new Appearance();
		Appearance appearance433 = new Appearance();
		Appearance appearance435 = new Appearance();
		Appearance appearance436 = new Appearance();
		Appearance appearance437 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance397 = new Appearance();
		Appearance appearance398 = new Appearance();
		Appearance appearance399 = new Appearance();
		Appearance appearance400 = new Appearance();
		GridEXLayout grdAdvanceHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGdBreakUp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance492 = new Appearance();
		Appearance appearance493 = new Appearance();
		Appearance appearance494 = new Appearance();
		Appearance appearance495 = new Appearance();
		Appearance appearance496 = new Appearance();
		Appearance appearance497 = new Appearance();
		Appearance appearance498 = new Appearance();
		Appearance appearance499 = new Appearance();
		Appearance appearance500 = new Appearance();
		Appearance appearance502 = new Appearance();
		Appearance appearance503 = new Appearance();
		Appearance appearance504 = new Appearance();
		Appearance appearance505 = new Appearance();
		Appearance appearance506 = new Appearance();
		Appearance appearance507 = new Appearance();
		Appearance appearance508 = new Appearance();
		Appearance appearance509 = new Appearance();
		Appearance appearance510 = new Appearance();
		Appearance appearance511 = new Appearance();
		Appearance appearance513 = new Appearance();
		Appearance appearance514 = new Appearance();
		Appearance appearance515 = new Appearance();
		Appearance appearance516 = new Appearance();
		Appearance appearance517 = new Appearance();
		GridEXLayout grdGDhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance445 = new Appearance();
		Appearance appearance556 = new Appearance();
		Appearance appearance580 = new Appearance();
		Appearance appearance591 = new Appearance();
		Appearance appearance602 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		GridEXLayout DataGridHistoryMultiInvoiceToGdForm_DesignTimeLayout = new GridEXLayout();
		Appearance appearance531 = new Appearance();
		Appearance appearance532 = new Appearance();
		Appearance appearance533 = new Appearance();
		Appearance appearance535 = new Appearance();
		Appearance appearance536 = new Appearance();
		Appearance appearance537 = new Appearance();
		Appearance appearance538 = new Appearance();
		Appearance appearance539 = new Appearance();
		Appearance appearance540 = new Appearance();
		Appearance appearance541 = new Appearance();
		Appearance appearance542 = new Appearance();
		Appearance appearance543 = new Appearance();
		GridEXLayout grdMultiInvoiceAgainstGd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance478 = new Appearance();
		Appearance appearance480 = new Appearance();
		Appearance appearance481 = new Appearance();
		Appearance appearance482 = new Appearance();
		Appearance appearance483 = new Appearance();
		Appearance appearance484 = new Appearance();
		Appearance appearance485 = new Appearance();
		Appearance appearance486 = new Appearance();
		Appearance appearance487 = new Appearance();
		Appearance appearance488 = new Appearance();
		Appearance appearance489 = new Appearance();
		Appearance appearance491 = new Appearance();
		Appearance appearance452 = new Appearance();
		Appearance appearance453 = new Appearance();
		Appearance appearance454 = new Appearance();
		Appearance appearance455 = new Appearance();
		Appearance appearance456 = new Appearance();
		Appearance appearance458 = new Appearance();
		Appearance appearance459 = new Appearance();
		Appearance appearance460 = new Appearance();
		Appearance appearance461 = new Appearance();
		Appearance appearance462 = new Appearance();
		Appearance appearance463 = new Appearance();
		Appearance appearance464 = new Appearance();
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
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
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
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance358 = new Appearance();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel22 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.label85 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.txtweightcheck = new System.Windows.Forms.TextBox();
		this.txtqtycheck = new System.Windows.Forms.TextBox();
		this.GroupDetail = new System.Windows.Forms.GroupBox();
		this.txtRateWithoutAddLess = new System.Windows.Forms.TextBox();
		this.label155 = new System.Windows.Forms.Label();
		this.txtAddLessRate = new System.Windows.Forms.TextBox();
		this.label154 = new System.Windows.Forms.Label();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.label97 = new System.Windows.Forms.Label();
		this.label93 = new System.Windows.Forms.Label();
		this.txtItemCommodityDetail = new System.Windows.Forms.TextBox();
		this.txtHealthPermitnodetail = new System.Windows.Forms.TextBox();
		this.label96 = new System.Windows.Forms.Label();
		this.txtItemHsCodeDetail = new System.Windows.Forms.TextBox();
		this.label92 = new System.Windows.Forms.Label();
		this.txtOtherAmountDetil = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtOtherHSCodeDetail = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtOtherRateDetail = new System.Windows.Forms.TextBox();
		this.label90 = new System.Windows.Forms.Label();
		this.txtNoOfBagsPerCntnr = new System.Windows.Forms.TextBox();
		this.label58 = new System.Windows.Forms.Label();
		this.txtNoOfContainersDetail = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtPackingExpiryDate = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txttotalpackingweight = new System.Windows.Forms.TextBox();
		this.label88 = new System.Windows.Forms.Label();
		this.txtNetWeightdetail = new System.Windows.Forms.TextBox();
		this.label87 = new System.Windows.Forms.Label();
		this.txtgrossWeightdetail = new System.Windows.Forms.TextBox();
		this.label86 = new System.Windows.Forms.Label();
		this.txtPackingWeight = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.txtCostMTon = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.combitem = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.combpcktype = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.txtQtyMTon = new System.Windows.Forms.TextBox();
		this.btnCancel = new System.Windows.Forms.Button();
		this.label35 = new System.Windows.Forms.Label();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.CmbPackUom = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label84 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdotheritems = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.btnoCancel = new System.Windows.Forms.Button();
		this.CmbOtherItem = new UltraCombo();
		this.btnoUpdate = new System.Windows.Forms.Button();
		this.label71 = new System.Windows.Forms.Label();
		this.btnoadd = new System.Windows.Forms.Button();
		this.txtoAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtoRemarks = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.txtoQty = new System.Windows.Forms.TextBox();
		this.txtoRate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdpaymentdetail = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label82 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.CmbFinInstruments = new UltraCombo();
		this.label80 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.dtxtpercentoftotal = new System.Windows.Forms.TextBox();
		this.btndcancel = new System.Windows.Forms.Button();
		this.label83 = new System.Windows.Forms.Label();
		this.btndupdate = new System.Windows.Forms.Button();
		this.txtBalanceFI = new System.Windows.Forms.TextBox();
		this.btndadd = new System.Windows.Forms.Button();
		this.label76 = new System.Windows.Forms.Label();
		this.dtxtdueday = new System.Windows.Forms.TextBox();
		this.dcmbpaymentterm = new UltraCombo();
		this.label78 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.dtxtfcyamount = new System.Windows.Forms.TextBox();
		this.panel20 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.HeaderGropBox = new System.Windows.Forms.GroupBox();
		this.cmbPrefixType = new UltraCombo();
		this.label156 = new System.Windows.Forms.Label();
		this.Cmbinvoiceno = new UltraCombo();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label52 = new System.Windows.Forms.Label();
		this.cmbOtherDestinationPort = new UltraCombo();
		this.label73 = new System.Windows.Forms.Label();
		this.cmbOtherCustomer = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label91 = new System.Windows.Forms.Label();
		this.label94 = new System.Windows.Forms.Label();
		this.CmbCommissionDebitAccount = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label95 = new System.Windows.Forms.Label();
		this.cmbCommissionAgent = new UltraCombo();
		this.txtCommPercent = new System.Windows.Forms.TextBox();
		this.txtCommAmount = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.txtremarksheader = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtTotalNetAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.cmbdeliverytermnew = new UltraCombo();
		this.txtaddlessamount = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbLoadingPort = new UltraCombo();
		this.cmbDestinationPort = new UltraCombo();
		this.txtcertificateOfOrigion = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtaddlesscommnets = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtLotRef = new System.Windows.Forms.TextBox();
		this.cmbcareiertype = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.cmbPaymentTermsNew = new UltraCombo();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.cmbimporterBankNew = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.cmbNotifyParty2 = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.txtIformDate = new System.Windows.Forms.DateTimePicker();
		this.txtGdNo = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtLocalAmt = new System.Windows.Forms.TextBox();
		this.txtExRate = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtfcyAmount = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbfcycode = new UltraCombo();
		this.cmbexporterBankNew = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.cmbNotifyParty1 = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbPreInvoice = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.cmbSupCust = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbbranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbproject = new UltraCombo();
		this.txtcertificate = new System.Windows.Forms.TextBox();
		this.label61 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.chk548Print = new System.Windows.Forms.CheckBox();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnrefersh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoaderForm = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint548 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnPaymentDetail = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateInvoiceNo = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label89 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label153 = new System.Windows.Forms.Label();
		this.gridDetailHistory = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label148 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label149 = new System.Windows.Forms.Label();
		this.label150 = new System.Windows.Forms.Label();
		this.label151 = new System.Windows.Forms.Label();
		this.label152 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel13 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnSlipHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnBankSlipHistory = new System.Windows.Forms.ToolStripButton();
		this.label69 = new System.Windows.Forms.Label();
		this.tabPage7 = new System.Windows.Forms.TabPage();
		this.panel26 = new System.Windows.Forms.Panel();
		this.panel28 = new System.Windows.Forms.Panel();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdAdvanceUtlize = new GridEX();
		this.panel33 = new System.Windows.Forms.Panel();
		this.panel32 = new System.Windows.Forms.Panel();
		this.CmbInvoiceAdvanceUtilize = new UltraCombo();
		this.label103 = new System.Windows.Forms.Label();
		this.CmbFIAdvanceUtlize = new UltraCombo();
		this.label108 = new System.Windows.Forms.Label();
		this.txtGdValueAdvanceUtlize = new System.Windows.Forms.TextBox();
		this.label109 = new System.Windows.Forms.Label();
		this.btnAddAdvanceUtlize = new System.Windows.Forms.Button();
		this.btnUpdateAdvanceUtilize = new System.Windows.Forms.Button();
		this.label111 = new System.Windows.Forms.Label();
		this.btnCancelAdvanceUtilize = new System.Windows.Forms.Button();
		this.txtFIBalanceAdvanceUtlize = new System.Windows.Forms.TextBox();
		this.label112 = new System.Windows.Forms.Label();
		this.txtAdvanceUtlize = new System.Windows.Forms.TextBox();
		this.CmbGdNoAdvanceUtilize = new UltraCombo();
		this.label115 = new System.Windows.Forms.Label();
		this.panel29 = new System.Windows.Forms.Panel();
		this.label113 = new System.Windows.Forms.Label();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton14 = new System.Windows.Forms.ToolStripButton();
		this.tabPage8 = new System.Windows.Forms.TabPage();
		this.grdAdvanceHistory = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel31 = new System.Windows.Forms.Panel();
		this.grdGdBreakUp = new GridEX();
		this.panel30 = new System.Windows.Forms.Panel();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label121 = new System.Windows.Forms.Label();
		this.txtFttAmount = new System.Windows.Forms.TextBox();
		this.label120 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label119 = new System.Windows.Forms.Label();
		this.txtNetAmount = new System.Windows.Forms.TextBox();
		this.label118 = new System.Windows.Forms.Label();
		this.txtFTTPrcnt = new System.Windows.Forms.TextBox();
		this.label117 = new System.Windows.Forms.Label();
		this.txtFobValue = new System.Windows.Forms.TextBox();
		this.label116 = new System.Windows.Forms.Label();
		this.txtFreight = new System.Windows.Forms.TextBox();
		this.label110 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label106 = new System.Windows.Forms.Label();
		this.CmbBankName = new UltraCombo();
		this.label105 = new System.Windows.Forms.Label();
		this.lblInvoiceBalanceGdBreakUp = new System.Windows.Forms.Label();
		this.txtGDValue = new System.Windows.Forms.TextBox();
		this.label107 = new System.Windows.Forms.Label();
		this.btnAddGDBreakUp = new System.Windows.Forms.Button();
		this.btnUpdateGdBreakUp = new System.Windows.Forms.Button();
		this.btnCancelGdBreakUp = new System.Windows.Forms.Button();
		this.txtGDNoBreakUp = new System.Windows.Forms.TextBox();
		this.label102 = new System.Windows.Forms.Label();
		this.txtInvoiceNoGDBreakUp = new System.Windows.Forms.TextBox();
		this.CmbInvoiceGdBreakUp = new UltraCombo();
		this.label100 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.datGDDateGDBreakUp = new System.Windows.Forms.DateTimePicker();
		this.label101 = new System.Windows.Forms.Label();
		this.datInvoiceDateGdBreakUp = new System.Windows.Forms.DateTimePicker();
		this.label99 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label114 = new System.Windows.Forms.Label();
		this.panel27 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
		this.btnGdBreakupManual = new System.Windows.Forms.ToolStripButton();
		this.GDHistory = new System.Windows.Forms.TabPage();
		this.panel43 = new System.Windows.Forms.Panel();
		this.panel44 = new System.Windows.Forms.Panel();
		this.grdGDhistory = new GridEX();
		this.panel34 = new System.Windows.Forms.Panel();
		this.label104 = new System.Windows.Forms.Label();
		this.panel42 = new System.Windows.Forms.Panel();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.btnShowGdBreakUpHistory = new System.Windows.Forms.Button();
		this.CmbBankGdBankHistory = new UltraCombo();
		this.label143 = new System.Windows.Forms.Label();
		this.ToDateGdBreakupHistory = new System.Windows.Forms.DateTimePicker();
		this.label144 = new System.Windows.Forms.Label();
		this.FromDateGdBreakupHistory = new System.Windows.Forms.DateTimePicker();
		this.label147 = new System.Windows.Forms.Label();
		this.toolStrip7 = new System.Windows.Forms.ToolStrip();
		this.btnResetGdBreakupHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshGdBreakupHistory = new System.Windows.Forms.ToolStripButton();
		this.InvoicesAllocationToGd = new System.Windows.Forms.TabPage();
		this.panel39 = new System.Windows.Forms.Panel();
		this.DataGridHistoryMultiInvoiceToGdForm = new GridEX();
		this.panel41 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.btnRefreshMultiInvoiceToGdHistory = new System.Windows.Forms.Button();
		this.btnResetMultiInvoiceToGdHistory = new System.Windows.Forms.Button();
		this.btnShowMultiInvoicesToGdForm = new System.Windows.Forms.Button();
		this.CmbBankMultiInvoiceAgainstGdHistory = new UltraCombo();
		this.label142 = new System.Windows.Forms.Label();
		this.ToDateMultiInvoiceAgainstGd = new System.Windows.Forms.DateTimePicker();
		this.label145 = new System.Windows.Forms.Label();
		this.FromDateMultiInvoiceAgainstGd = new System.Windows.Forms.DateTimePicker();
		this.label146 = new System.Windows.Forms.Label();
		this.panel40 = new System.Windows.Forms.Panel();
		this.label141 = new System.Windows.Forms.Label();
		this.panel37 = new System.Windows.Forms.Panel();
		this.grdMultiInvoiceAgainstGd = new GridEX();
		this.panel38 = new System.Windows.Forms.Panel();
		this.label140 = new System.Windows.Forms.Label();
		this.panel35 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label139 = new System.Windows.Forms.Label();
		this.txtDetailRemarksMultiInvoiceToGdForm = new System.Windows.Forms.TextBox();
		this.btnAddMultiInvoiceForm = new System.Windows.Forms.Button();
		this.btnUpdateDetailMultiInvoiceForm = new System.Windows.Forms.Button();
		this.btnCancelDetailMultiInvoiceForm = new System.Windows.Forms.Button();
		this.txtInvoiceAmountMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label138 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoMultiInvoiceForm = new UltraCombo();
		this.label137 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtRemarksMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label130 = new System.Windows.Forms.Label();
		this.FttAmountMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label131 = new System.Windows.Forms.Label();
		this.txtNetAmountMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label132 = new System.Windows.Forms.Label();
		this.FttPrcntMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label133 = new System.Windows.Forms.Label();
		this.txtFobValueMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label134 = new System.Windows.Forms.Label();
		this.txtFreightMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label135 = new System.Windows.Forms.Label();
		this.txtGdValueMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label136 = new System.Windows.Forms.Label();
		this.txtExchangeRateMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label123 = new System.Windows.Forms.Label();
		this.txtDueDaysMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label124 = new System.Windows.Forms.Label();
		this.CmbBankMultiInvoiceForm = new UltraCombo();
		this.label125 = new System.Windows.Forms.Label();
		this.GdNoMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label126 = new System.Windows.Forms.Label();
		this.BankInvoiceNoMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label127 = new System.Windows.Forms.Label();
		this.GdDateMultiInvoiceForm = new System.Windows.Forms.DateTimePicker();
		this.label128 = new System.Windows.Forms.Label();
		this.BankInvoiceDateMultiInvoiceForm = new System.Windows.Forms.DateTimePicker();
		this.label129 = new System.Windows.Forms.Label();
		this.panel36 = new System.Windows.Forms.Panel();
		this.label122 = new System.Windows.Forms.Label();
		this.toolStrip6 = new System.Windows.Forms.ToolStrip();
		this.btnResetMultiInvoiceGd = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshMultiInvoiceGd = new System.Windows.Forms.ToolStripButton();
		this.btnSaveMultiInvoiceGd = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateMultiInvoiceGd = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbFCurrencr = new UltraCombo();
		this.cmbPaymentTerm = new UltraCombo();
		this.combranches = new UltraCombo();
		this.comproject = new UltraCombo();
		this.ultraCombo3 = new UltraCombo();
		this.CmbLCContractor = new UltraCombo();
		this.ultraCombo2 = new UltraCombo();
		this.ultraCombo1 = new UltraCombo();
		this.cmbCarierType = new UltraCombo();
		this.cmbExporterBank = new UltraCombo();
		this.cmbImporterBank = new UltraCombo();
		this.cmbEFormNo = new UltraCombo();
		this.cmbdeliveryterm = new UltraCombo();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar9 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar8 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel23.SuspendLayout();
		this.panel17.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.GroupDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		this.panel9.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).BeginInit();
		this.panel18.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).BeginInit();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstruments).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dcmbpaymentterm).BeginInit();
		this.panel20.SuspendLayout();
		this.panel11.SuspendLayout();
		this.HeaderGropBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPrefixType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbinvoiceno).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOtherDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbOtherCustomer).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliverytermnew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcareiertype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTermsNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterBankNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfcycode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbexporterBankNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPreInvoice).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupCust).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel3.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.groupBox7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel13.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage7.SuspendLayout();
		this.panel26.SuspendLayout();
		this.panel28.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAdvanceUtlize).BeginInit();
		this.panel33.SuspendLayout();
		this.panel32.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceAdvanceUtilize).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFIAdvanceUtlize).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGdNoAdvanceUtilize).BeginInit();
		this.panel29.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		this.tabPage8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAdvanceHistory).BeginInit();
		this.panel14.SuspendLayout();
		this.panel31.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGdBreakUp).BeginInit();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceGdBreakUp).BeginInit();
		this.panel25.SuspendLayout();
		this.panel27.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.GDHistory.SuspendLayout();
		this.panel43.SuspendLayout();
		this.panel44.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGDhistory).BeginInit();
		this.panel34.SuspendLayout();
		this.panel42.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankGdBankHistory).BeginInit();
		this.toolStrip7.SuspendLayout();
		this.InvoicesAllocationToGd.SuspendLayout();
		this.panel39.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryMultiInvoiceToGdForm).BeginInit();
		this.panel41.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankMultiInvoiceAgainstGdHistory).BeginInit();
		this.panel40.SuspendLayout();
		this.panel37.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiInvoiceAgainstGd).BeginInit();
		this.panel38.SuspendLayout();
		this.panel35.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoMultiInvoiceForm).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankMultiInvoiceForm).BeginInit();
		this.panel36.SuspendLayout();
		this.toolStrip6.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1164, 749);
		this.panel4.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.tabControl1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1164, 749);
		this.panel7.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage7);
		this.tabControl1.Controls.Add(this.GDHistory);
		this.tabControl1.Controls.Add(this.InvoicesAllocationToGd);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1164, 749);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1156, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel22);
		this.panel8.Controls.Add(this.panel17);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 56);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1150, 664);
		this.panel8.TabIndex = 3;
		this.panel22.Controls.Add(this.panel24);
		this.panel22.Controls.Add(this.panel23);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(0, 537);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1150, 127);
		this.panel22.TabIndex = 5;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 24);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1150, 103);
		this.panel24.TabIndex = 2;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1150, 103);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar2);
		this.panel23.Controls.Add(this.label85);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1150, 24);
		this.panel23.TabIndex = 1;
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.ForeColor = System.Drawing.Color.White;
		this.label85.Location = new System.Drawing.Point(3, 4);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(201, 16);
		this.label85.TabIndex = 27;
		this.label85.Text = "Commercial Invoice Detail Grid";
		this.panel17.Controls.Add(this.tabControl2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 302);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1150, 235);
		this.panel17.TabIndex = 4;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.25f);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1150, 235);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add(this.txtweightcheck);
		this.tabPage3.Controls.Add(this.txtqtycheck);
		this.tabPage3.Controls.Add(this.GroupDetail);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Location = new System.Drawing.Point(4, 23);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1142, 208);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Commercial Invoice Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.txtweightcheck.Enabled = false;
		this.txtweightcheck.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweightcheck.Location = new System.Drawing.Point(1116, 79);
		this.txtweightcheck.Name = "txtweightcheck";
		this.txtweightcheck.Size = new System.Drawing.Size(17, 23);
		this.txtweightcheck.TabIndex = 507;
		this.txtweightcheck.Text = "0";
		this.txtweightcheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtweightcheck.Visible = false;
		this.txtqtycheck.Enabled = false;
		this.txtqtycheck.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqtycheck.Location = new System.Drawing.Point(1116, 50);
		this.txtqtycheck.Name = "txtqtycheck";
		this.txtqtycheck.Size = new System.Drawing.Size(17, 23);
		this.txtqtycheck.TabIndex = 506;
		this.txtqtycheck.Text = "0";
		this.txtqtycheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqtycheck.Visible = false;
		this.GroupDetail.Controls.Add(this.txtRateWithoutAddLess);
		this.GroupDetail.Controls.Add(this.label155);
		this.GroupDetail.Controls.Add(this.txtAddLessRate);
		this.GroupDetail.Controls.Add(this.label154);
		this.GroupDetail.Controls.Add(this.txtProductionNo);
		this.GroupDetail.Controls.Add(this.label97);
		this.GroupDetail.Controls.Add(this.label93);
		this.GroupDetail.Controls.Add(this.txtItemCommodityDetail);
		this.GroupDetail.Controls.Add(this.txtHealthPermitnodetail);
		this.GroupDetail.Controls.Add(this.label96);
		this.GroupDetail.Controls.Add(this.txtItemHsCodeDetail);
		this.GroupDetail.Controls.Add(this.label92);
		this.GroupDetail.Controls.Add(this.txtOtherAmountDetil);
		this.GroupDetail.Controls.Add(this.label34);
		this.GroupDetail.Controls.Add(this.txtOtherHSCodeDetail);
		this.GroupDetail.Controls.Add(this.label39);
		this.GroupDetail.Controls.Add(this.txtOtherRateDetail);
		this.GroupDetail.Controls.Add(this.label90);
		this.GroupDetail.Controls.Add(this.txtNoOfBagsPerCntnr);
		this.GroupDetail.Controls.Add(this.label58);
		this.GroupDetail.Controls.Add(this.txtNoOfContainersDetail);
		this.GroupDetail.Controls.Add(this.label48);
		this.GroupDetail.Controls.Add(this.txtPackingExpiryDate);
		this.GroupDetail.Controls.Add(this.label6);
		this.GroupDetail.Controls.Add(this.txttotalpackingweight);
		this.GroupDetail.Controls.Add(this.label88);
		this.GroupDetail.Controls.Add(this.txtNetWeightdetail);
		this.GroupDetail.Controls.Add(this.label87);
		this.GroupDetail.Controls.Add(this.txtgrossWeightdetail);
		this.GroupDetail.Controls.Add(this.label86);
		this.GroupDetail.Controls.Add(this.txtPackingWeight);
		this.GroupDetail.Controls.Add(this.label59);
		this.GroupDetail.Controls.Add(this.label72);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.GroupDetail.Controls.Add(this.txtCostMTon);
		this.GroupDetail.Controls.Add(this.label29);
		this.GroupDetail.Controls.Add(this.txtNoOfBags);
		this.GroupDetail.Controls.Add(this.label30);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.GroupDetail.Controls.Add(this.label32);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combpcktype);
		this.GroupDetail.Controls.Add(this.label33);
		this.GroupDetail.Controls.Add(this.txtQtyMTon);
		this.GroupDetail.Controls.Add(this.btnCancel);
		this.GroupDetail.Controls.Add(this.label35);
		this.GroupDetail.Controls.Add(this.btnAddinGrid);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.GroupDetail.Controls.Add(this.label36);
		this.GroupDetail.Controls.Add(this.txtAmount);
		this.GroupDetail.Controls.Add(this.label37);
		this.GroupDetail.Controls.Add(this.label38);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.GroupDetail.Controls.Add(this.btnGridUpdate);
		this.GroupDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupDetail.Location = new System.Drawing.Point(6, 34);
		this.GroupDetail.Name = "GroupDetail";
		this.GroupDetail.Size = new System.Drawing.Size(1104, 164);
		this.GroupDetail.TabIndex = 0;
		this.GroupDetail.TabStop = false;
		this.GroupDetail.Text = "Detail";
		this.txtRateWithoutAddLess.Enabled = false;
		this.txtRateWithoutAddLess.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRateWithoutAddLess.Location = new System.Drawing.Point(310, 87);
		this.txtRateWithoutAddLess.Name = "txtRateWithoutAddLess";
		this.txtRateWithoutAddLess.Size = new System.Drawing.Size(93, 23);
		this.txtRateWithoutAddLess.TabIndex = 13;
		this.txtRateWithoutAddLess.Text = "0";
		this.txtRateWithoutAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateWithoutAddLess.TextChanged += new System.EventHandler(txtRateWithoutAddLess_TextChanged);
		this.label155.AutoSize = true;
		this.label155.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label155.Location = new System.Drawing.Point(310, 68);
		this.label155.Name = "label155";
		this.label155.Size = new System.Drawing.Size(69, 15);
		this.label155.TabIndex = 605;
		this.label155.Text = "Rate/M.Ton";
		this.txtAddLessRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLessRate.Location = new System.Drawing.Point(404, 87);
		this.txtAddLessRate.Name = "txtAddLessRate";
		this.txtAddLessRate.ShortcutsEnabled = false;
		this.txtAddLessRate.Size = new System.Drawing.Size(93, 23);
		this.txtAddLessRate.TabIndex = 14;
		this.txtAddLessRate.Text = "0";
		this.txtAddLessRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessRate.TextChanged += new System.EventHandler(txtAddLessRate_TextChanged);
		this.txtAddLessRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLessRate_KeyPress);
		this.label154.AutoSize = true;
		this.label154.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label154.Location = new System.Drawing.Point(404, 68);
		this.label154.Name = "label154";
		this.label154.Size = new System.Drawing.Size(77, 15);
		this.label154.TabIndex = 603;
		this.label154.Text = "AddLess Rate";
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtProductionNo.Location = new System.Drawing.Point(117, 132);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(105, 23);
		this.txtProductionNo.TabIndex = 22;
		this.txtProductionNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.Location = new System.Drawing.Point(117, 113);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(85, 15);
		this.label97.TabIndex = 601;
		this.label97.Text = "Production No";
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label93.Location = new System.Drawing.Point(692, 113);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(99, 15);
		this.label93.TabIndex = 599;
		this.label93.Text = "Health Permit No";
		this.txtItemCommodityDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemCommodityDetail.Location = new System.Drawing.Point(224, 132);
		this.txtItemCommodityDetail.Name = "txtItemCommodityDetail";
		this.txtItemCommodityDetail.Size = new System.Drawing.Size(462, 23);
		this.txtItemCommodityDetail.TabIndex = 23;
		this.txtHealthPermitnodetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHealthPermitnodetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtHealthPermitnodetail.Location = new System.Drawing.Point(687, 132);
		this.txtHealthPermitnodetail.Name = "txtHealthPermitnodetail";
		this.txtHealthPermitnodetail.Size = new System.Drawing.Size(273, 23);
		this.txtHealthPermitnodetail.TabIndex = 24;
		this.label96.AutoSize = true;
		this.label96.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label96.Location = new System.Drawing.Point(224, 113);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(132, 15);
		this.label96.TabIndex = 597;
		this.label96.Text = "Item Commodity Detail";
		this.txtItemHsCodeDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemHsCodeDetail.Location = new System.Drawing.Point(213, 39);
		this.txtItemHsCodeDetail.Name = "txtItemHsCodeDetail";
		this.txtItemHsCodeDetail.Size = new System.Drawing.Size(122, 23);
		this.txtItemHsCodeDetail.TabIndex = 1;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.Location = new System.Drawing.Point(211, 18);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(81, 15);
		this.label92.TabIndex = 519;
		this.label92.Text = "Item HS Code";
		this.txtOtherAmountDetil.Enabled = false;
		this.txtOtherAmountDetil.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherAmountDetil.Location = new System.Drawing.Point(866, 87);
		this.txtOtherAmountDetil.Name = "txtOtherAmountDetil";
		this.txtOtherAmountDetil.Size = new System.Drawing.Size(93, 23);
		this.txtOtherAmountDetil.TabIndex = 19;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(869, 68);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(80, 15);
		this.label34.TabIndex = 517;
		this.label34.Text = "Bank Amount";
		this.txtOtherHSCodeDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherHSCodeDetail.Location = new System.Drawing.Point(961, 87);
		this.txtOtherHSCodeDetail.Name = "txtOtherHSCodeDetail";
		this.txtOtherHSCodeDetail.Size = new System.Drawing.Size(104, 23);
		this.txtOtherHSCodeDetail.TabIndex = 20;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(961, 68);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(82, 15);
		this.label39.TabIndex = 516;
		this.label39.Text = "Bank HS Code";
		this.txtOtherRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherRateDetail.Location = new System.Drawing.Point(791, 87);
		this.txtOtherRateDetail.Name = "txtOtherRateDetail";
		this.txtOtherRateDetail.Size = new System.Drawing.Size(73, 23);
		this.txtOtherRateDetail.TabIndex = 18;
		this.txtOtherRateDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherRateDetail.TextChanged += new System.EventHandler(txtOtherRateDetail_TextChanged);
		this.txtOtherRateDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherRateDetail_KeyPress);
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(791, 68);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(59, 15);
		this.label90.TabIndex = 515;
		this.label90.Text = "Bank Rate";
		this.txtNoOfBagsPerCntnr.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfBagsPerCntnr.Location = new System.Drawing.Point(732, 39);
		this.txtNoOfBagsPerCntnr.Name = "txtNoOfBagsPerCntnr";
		this.txtNoOfBagsPerCntnr.Size = new System.Drawing.Size(93, 23);
		this.txtNoOfBagsPerCntnr.TabIndex = 6;
		this.txtNoOfBagsPerCntnr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBagsPerCntnr.TextChanged += new System.EventHandler(txtNoOfBagsPerCntnr_TextChanged);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(732, 18);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(95, 15);
		this.label58.TabIndex = 509;
		this.label58.Text = "NoOfBags/Cntnr";
		this.txtNoOfContainersDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfContainersDetail.Location = new System.Drawing.Point(638, 39);
		this.txtNoOfContainersDetail.Name = "txtNoOfContainersDetail";
		this.txtNoOfContainersDetail.Size = new System.Drawing.Size(91, 23);
		this.txtNoOfContainersDetail.TabIndex = 5;
		this.txtNoOfContainersDetail.Text = "0";
		this.txtNoOfContainersDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfContainersDetail.TextChanged += new System.EventHandler(txtNoOfContainersDetail_TextChanged);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(638, 18);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(92, 15);
		this.label48.TabIndex = 507;
		this.label48.Text = "NoOfContainers";
		this.txtPackingExpiryDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingExpiryDate.Location = new System.Drawing.Point(4, 132);
		this.txtPackingExpiryDate.Name = "txtPackingExpiryDate";
		this.txtPackingExpiryDate.Size = new System.Drawing.Size(112, 23);
		this.txtPackingExpiryDate.TabIndex = 21;
		this.txtPackingExpiryDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(4, 113);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(112, 15);
		this.label6.TabIndex = 505;
		this.label6.Text = "Packing Expiry Date";
		this.txttotalpackingweight.Enabled = false;
		this.txttotalpackingweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txttotalpackingweight.Location = new System.Drawing.Point(114, 87);
		this.txttotalpackingweight.Name = "txttotalpackingweight";
		this.txttotalpackingweight.Size = new System.Drawing.Size(97, 23);
		this.txttotalpackingweight.TabIndex = 11;
		this.txttotalpackingweight.Text = "0";
		this.txttotalpackingweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txttotalpackingweight.TextChanged += new System.EventHandler(txttotalpackingweight_TextChanged);
		this.txttotalpackingweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txttotalpackingweight_KeyPress);
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.Location = new System.Drawing.Point(114, 68);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(97, 15);
		this.label88.TabIndex = 503;
		this.label88.Text = "Total Packing Wt";
		this.txtNetWeightdetail.BackColor = System.Drawing.Color.White;
		this.txtNetWeightdetail.Enabled = false;
		this.txtNetWeightdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeightdetail.Location = new System.Drawing.Point(6, 87);
		this.txtNetWeightdetail.Name = "txtNetWeightdetail";
		this.txtNetWeightdetail.ReadOnly = true;
		this.txtNetWeightdetail.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeightdetail.TabIndex = 10;
		this.txtNetWeightdetail.Text = "0";
		this.txtNetWeightdetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightdetail.TextChanged += new System.EventHandler(txtNetWeightdetail_TextChanged);
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.Location = new System.Drawing.Point(6, 68);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(85, 15);
		this.label87.TabIndex = 501;
		this.label87.Text = "Net Weight Kg";
		this.txtgrossWeightdetail.Enabled = false;
		this.txtgrossWeightdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgrossWeightdetail.Location = new System.Drawing.Point(213, 87);
		this.txtgrossWeightdetail.Name = "txtgrossWeightdetail";
		this.txtgrossWeightdetail.Size = new System.Drawing.Size(96, 23);
		this.txtgrossWeightdetail.TabIndex = 12;
		this.txtgrossWeightdetail.Text = "0";
		this.txtgrossWeightdetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.Location = new System.Drawing.Point(212, 68);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(95, 15);
		this.label86.TabIndex = 499;
		this.label86.Text = "Gross Weight Kg";
		this.txtPackingWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingWeight.Location = new System.Drawing.Point(461, 39);
		this.txtPackingWeight.Name = "txtPackingWeight";
		this.txtPackingWeight.Size = new System.Drawing.Size(71, 23);
		this.txtPackingWeight.TabIndex = 3;
		this.txtPackingWeight.Text = "0";
		this.txtPackingWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingWeight.TextChanged += new System.EventHandler(txtPackingWeight_TextChanged);
		this.txtPackingWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingWeight_KeyPress);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(461, 18);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(68, 15);
		this.label59.TabIndex = 497;
		this.label59.Text = "Packing Wt";
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(531, 18);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(54, 15);
		this.label72.TabIndex = 493;
		this.label72.Text = "CropYear";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance518).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance518).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance518;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance519).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance519).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance519).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance519).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance519;
		((AppearanceBase)appearance520).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance520;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance521).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance521).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance521).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance521).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance521;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance522).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance522).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance522;
		((AppearanceBase)appearance524).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance524).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance524;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance525).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance525;
		((AppearanceBase)appearance526).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance526).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance526;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance527).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance527).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance527).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance527).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance527).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance527;
		((AppearanceBase)appearance528).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance528;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance529).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance529).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance529;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance530).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance530;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(534, 37);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		this.CmbCropYear.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabStop = false;
		this.txtCostMTon.Enabled = false;
		this.txtCostMTon.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCostMTon.Location = new System.Drawing.Point(498, 87);
		this.txtCostMTon.Name = "txtCostMTon";
		this.txtCostMTon.Size = new System.Drawing.Size(93, 23);
		this.txtCostMTon.TabIndex = 15;
		this.txtCostMTon.Text = "0";
		this.txtCostMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCostMTon.TextChanged += new System.EventHandler(txtCostMTon_TextChanged);
		this.txtCostMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCostMTon_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(498, 68);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(91, 15);
		this.label29.TabIndex = 487;
		this.label29.Text = "Net Rate/M.Ton";
		this.txtNoOfBags.BackColor = System.Drawing.Color.White;
		this.txtNoOfBags.Enabled = false;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfBags.Location = new System.Drawing.Point(992, 39);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(73, 23);
		this.txtNoOfBags.TabIndex = 9;
		this.txtNoOfBags.Tag = "NoOfBags";
		this.txtNoOfBags.Text = "0";
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBags.TextChanged += new System.EventHandler(txtNoOfBags_TextChanged);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(992, 18);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(67, 15);
		this.label30.TabIndex = 485;
		this.label30.Text = "No Of Bags";
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(6, 37);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(205, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(6, 18);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(67, 15);
		this.label32.TabIndex = 470;
		this.label32.Text = "Item Name";
		this.combpcktype.AllowNull = (DefaultableBoolean)1;
		this.combpcktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpcktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpcktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpcktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.combpcktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance268).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpcktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpcktype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpcktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpcktype).Location = new System.Drawing.Point(337, 37);
		((System.Windows.Forms.Control)(object)this.combpcktype).Name = "combpcktype";
		this.combpcktype.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.combpcktype).Size = new System.Drawing.Size(121, 26);
		((System.Windows.Forms.Control)(object)this.combpcktype).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.combpcktype).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combpcktype).Leave += new System.EventHandler(combpcktype_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(337, 18);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(77, 15);
		this.label33.TabIndex = 476;
		this.label33.Text = "Packing Type";
		this.txtQtyMTon.Enabled = false;
		this.txtQtyMTon.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyMTon.Location = new System.Drawing.Point(910, 39);
		this.txtQtyMTon.Name = "txtQtyMTon";
		this.txtQtyMTon.Size = new System.Drawing.Size(80, 23);
		this.txtQtyMTon.TabIndex = 8;
		this.txtQtyMTon.Tag = "Mton";
		this.txtQtyMTon.Text = "0";
		this.txtQtyMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMTon.TextChanged += new System.EventHandler(txtQtyMTon_TextChanged);
		this.txtQtyMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyMTon_KeyPress);
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderSize = 0;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(1031, 130);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(65, 26);
		this.btnCancel.TabIndex = 27;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(910, 18);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(41, 15);
		this.label35.TabIndex = 483;
		this.label35.Text = "M.Ton";
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatAppearance.BorderSize = 0;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(963, 130);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 26);
		this.btnAddinGrid.TabIndex = 25;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance312).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance323;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance335).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance335).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance335).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance335;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance346).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance346;
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance357).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance357;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance368;
		((AppearanceBase)appearance379).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance379).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance379;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance390).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance390).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance390).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance390).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance390).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance390;
		((AppearanceBase)appearance401).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance401;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance412).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance412).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance412;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance423).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance423;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(827, 37);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		this.CmbPackUom.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(combitempck_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(824, 18);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 474;
		this.label36.Text = "Pack UOM";
		this.txtAmount.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(687, 87);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(102, 23);
		this.txtAmount.TabIndex = 17;
		this.txtAmount.TabStop = false;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmount_TextChanged);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(687, 68);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(51, 15);
		this.label37.TabIndex = 485;
		this.label37.Text = "Amount";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(593, 68);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(61, 15);
		this.label38.TabIndex = 479;
		this.label38.Text = "Rate UOM";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance434).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance434).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance434;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance446).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance446).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance446).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance446).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance446;
		((AppearanceBase)appearance457).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance457;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance468).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance468).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance468).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance468).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance468;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance479).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance479).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance479;
		((AppearanceBase)appearance490).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance490).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance490;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance544).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance544;
		((AppearanceBase)appearance546).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance546).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance546;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance547).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance547).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance547).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance547).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance547).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance547;
		((AppearanceBase)appearance534).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance534;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance548).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance548).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance548;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance549).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance549;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(593, 85);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		this.CmbRateUom.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(92, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
		this.btnGridUpdate.FlatAppearance.BorderSize = 0;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(962, 130);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(68, 26);
		this.btnGridUpdate.TabIndex = 26;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label84);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1136, 23);
		this.panel9.TabIndex = 0;
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.ForeColor = System.Drawing.Color.White;
		this.label84.Location = new System.Drawing.Point(0, 3);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(124, 16);
		this.label84.TabIndex = 26;
		this.label84.Text = "Detail Information";
		this.tabPage4.Controls.Add(this.panel2);
		this.tabPage4.Controls.Add(this.panel19);
		this.tabPage4.Controls.Add(this.panel18);
		this.tabPage4.Location = new System.Drawing.Point(4, 23);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1142, 208);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Items";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 80);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1136, 125);
		this.panel2.TabIndex = 2;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdotheritems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1136, 125);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 0;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		this.panel19.Controls.Add(this.btnoCancel);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherItem);
		this.panel19.Controls.Add(this.btnoUpdate);
		this.panel19.Controls.Add(this.label71);
		this.panel19.Controls.Add(this.btnoadd);
		this.panel19.Controls.Add(this.txtoAmount);
		this.panel19.Controls.Add(this.label14);
		this.panel19.Controls.Add(this.label70);
		this.panel19.Controls.Add(this.txtoRemarks);
		this.panel19.Controls.Add(this.label63);
		this.panel19.Controls.Add(this.txtoQty);
		this.panel19.Controls.Add(this.txtoRate);
		this.panel19.Controls.Add(this.label15);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(3, 26);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1136, 54);
		this.panel19.TabIndex = 1;
		this.btnoCancel.BackColor = System.Drawing.Color.Teal;
		this.btnoCancel.FlatAppearance.BorderSize = 0;
		this.btnoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoCancel.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoCancel.ForeColor = System.Drawing.Color.White;
		this.btnoCancel.Location = new System.Drawing.Point(982, 21);
		this.btnoCancel.Name = "btnoCancel";
		this.btnoCancel.Size = new System.Drawing.Size(70, 24);
		this.btnoCancel.TabIndex = 7;
		this.btnoCancel.Text = "Cancel";
		this.btnoCancel.UseVisualStyleBackColor = false;
		this.btnoCancel.Visible = false;
		this.btnoCancel.Click += new System.EventHandler(btnoCancel_Click);
		this.CmbOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance550).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance550).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance550;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance572).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance572).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance572).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance572).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance572;
		((AppearanceBase)appearance573).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance573;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance551).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance551).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance551).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance551).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance551;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance552).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance552).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance552;
		((AppearanceBase)appearance553).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance553).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance553;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance554).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance554;
		((AppearanceBase)appearance555).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance555).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance555;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance558).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance558).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance558).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance558).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance558).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance558;
		((AppearanceBase)appearance581).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance581;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance582).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance582).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance582;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance583).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance583;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Location = new System.Drawing.Point(17, 21);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Name = "CmbOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Size = new System.Drawing.Size(198, 25);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TextChanged += new System.EventHandler(CmbOtherItem_TextChanged);
		this.btnoUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnoUpdate.FlatAppearance.BorderSize = 0;
		this.btnoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoUpdate.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoUpdate.ForeColor = System.Drawing.Color.White;
		this.btnoUpdate.Location = new System.Drawing.Point(912, 21);
		this.btnoUpdate.Name = "btnoUpdate";
		this.btnoUpdate.Size = new System.Drawing.Size(67, 24);
		this.btnoUpdate.TabIndex = 6;
		this.btnoUpdate.Text = "Update";
		this.btnoUpdate.UseVisualStyleBackColor = false;
		this.btnoUpdate.Visible = false;
		this.btnoUpdate.Click += new System.EventHandler(btnoUpdate_Click);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(18, 4);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(67, 15);
		this.label71.TabIndex = 504;
		this.label71.Text = "Item Name";
		this.btnoadd.BackColor = System.Drawing.Color.Teal;
		this.btnoadd.FlatAppearance.BorderSize = 0;
		this.btnoadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoadd.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoadd.ForeColor = System.Drawing.Color.White;
		this.btnoadd.Location = new System.Drawing.Point(873, 21);
		this.btnoadd.Name = "btnoadd";
		this.btnoadd.Size = new System.Drawing.Size(36, 24);
		this.btnoadd.TabIndex = 5;
		this.btnoadd.Text = "+";
		this.btnoadd.UseVisualStyleBackColor = false;
		this.btnoadd.Click += new System.EventHandler(btnoadd_Click);
		this.txtoAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoAmount.Location = new System.Drawing.Point(405, 22);
		this.txtoAmount.Name = "txtoAmount";
		this.txtoAmount.ReadOnly = true;
		this.txtoAmount.Size = new System.Drawing.Size(101, 22);
		this.txtoAmount.TabIndex = 3;
		this.txtoAmount.Text = "0";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(511, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(52, 15);
		this.label14.TabIndex = 508;
		this.label14.Text = "Remarks";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(404, 4);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(51, 15);
		this.label70.TabIndex = 506;
		this.label70.Text = "Amount";
		this.txtoRemarks.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRemarks.Location = new System.Drawing.Point(508, 22);
		this.txtoRemarks.Name = "txtoRemarks";
		this.txtoRemarks.Size = new System.Drawing.Size(359, 22);
		this.txtoRemarks.TabIndex = 4;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(321, 4);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(30, 15);
		this.label63.TabIndex = 507;
		this.label63.Text = "Rate";
		this.txtoQty.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoQty.Location = new System.Drawing.Point(217, 22);
		this.txtoQty.Name = "txtoQty";
		this.txtoQty.Size = new System.Drawing.Size(101, 22);
		this.txtoQty.TabIndex = 1;
		this.txtoQty.Text = "0";
		this.txtoQty.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.txtoRate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRate.Location = new System.Drawing.Point(322, 22);
		this.txtoRate.Name = "txtoRate";
		this.txtoRate.ReadOnly = true;
		this.txtoRate.Size = new System.Drawing.Size(80, 22);
		this.txtoRate.TabIndex = 2;
		this.txtoRate.Text = "0";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(218, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(26, 15);
		this.label15.TabIndex = 505;
		this.label15.Text = "Qty";
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 3);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1136, 23);
		this.panel18.TabIndex = 0;
		this.tabPage5.Controls.Add(this.panel6);
		this.tabPage5.Controls.Add(this.panel21);
		this.tabPage5.Controls.Add(this.panel20);
		this.tabPage5.Location = new System.Drawing.Point(4, 23);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1142, 208);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Payment Detail";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdpaymentdetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 79);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1136, 126);
		this.panel6.TabIndex = 2;
		this.grdpaymentdetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdpaymentdetail).BackColor = System.Drawing.Color.White;
		grdpaymentdetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdpaymentdetail.DesignTimeLayout = grdpaymentdetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdpaymentdetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Name = "grdpaymentdetail";
		((ScrollableBase)this.grdpaymentdetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdpaymentdetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdpaymentdetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Size = new System.Drawing.Size(1136, 126);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).TabIndex = 1;
		this.grdpaymentdetail.TotalRow = (InheritableBoolean)1;
		this.grdpaymentdetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdpaymentdetail).VisualStyle = (VisualStyle)4;
		this.grdpaymentdetail.ColumnButtonClick += new ColumnActionEventHandler(grdpaymentdetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).DoubleClick += new System.EventHandler(grdpaymentdetail_DoubleClick);
		this.panel21.Controls.Add(this.label82);
		this.panel21.Controls.Add(this.label75);
		this.panel21.Controls.Add(this.label81);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinInstruments);
		this.panel21.Controls.Add(this.label80);
		this.panel21.Controls.Add(this.label74);
		this.panel21.Controls.Add(this.label79);
		this.panel21.Controls.Add(this.dtxtpercentoftotal);
		this.panel21.Controls.Add(this.btndcancel);
		this.panel21.Controls.Add(this.label83);
		this.panel21.Controls.Add(this.btndupdate);
		this.panel21.Controls.Add(this.txtBalanceFI);
		this.panel21.Controls.Add(this.btndadd);
		this.panel21.Controls.Add(this.label76);
		this.panel21.Controls.Add(this.dtxtdueday);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.dcmbpaymentterm);
		this.panel21.Controls.Add(this.label78);
		this.panel21.Controls.Add(this.label77);
		this.panel21.Controls.Add(this.dtxtfcyamount);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(3, 26);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1136, 53);
		this.panel21.TabIndex = 1;
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.Red;
		this.label82.Location = new System.Drawing.Point(720, 2);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(14, 17);
		this.label82.TabIndex = 591;
		this.label82.Text = "*";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(12, 3);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(140, 15);
		this.label75.TabIndex = 508;
		this.label75.Text = "Financial Instruments No";
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.Red;
		this.label81.Location = new System.Drawing.Point(609, 2);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(14, 17);
		this.label81.TabIndex = 590;
		this.label81.Text = "*";
		this.CmbFinInstruments.AllowNull = (DefaultableBoolean)1;
		this.CmbFinInstruments.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinInstruments.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinInstruments.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance597).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance597).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Appearance = (AppearanceBase)(object)appearance597;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance598).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance598).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance598).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance598).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstruments).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance598;
		((AppearanceBase)appearance599).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance599;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstruments).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance600).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance600).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance600).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance600).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance600;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance601).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance601).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance601;
		((AppearanceBase)appearance603).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance603).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance603;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance604).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance604;
		((AppearanceBase)appearance605).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance605).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance605;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance606).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance606).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance606).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance606).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance606).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance606;
		((AppearanceBase)appearance607).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance607;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance608).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance608).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance608;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance609).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance609;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).Location = new System.Drawing.Point(15, 23);
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).Name = "CmbFinInstruments";
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).Size = new System.Drawing.Size(247, 25);
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).TabIndex = 0;
		this.CmbFinInstruments.ValueChanged += new System.EventHandler(dcmbfino_ValueChanged);
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.ForeColor = System.Drawing.Color.Red;
		this.label80.Location = new System.Drawing.Point(353, 2);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(14, 17);
		this.label80.TabIndex = 589;
		this.label80.Text = "*";
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.Location = new System.Drawing.Point(543, 3);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(60, 15);
		this.label74.TabIndex = 509;
		this.label74.Text = "% of Total";
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.ForeColor = System.Drawing.Color.Red;
		this.label79.Location = new System.Drawing.Point(158, 2);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(14, 17);
		this.label79.TabIndex = 588;
		this.label79.Text = "*";
		this.dtxtpercentoftotal.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtxtpercentoftotal.Location = new System.Drawing.Point(536, 24);
		this.dtxtpercentoftotal.Name = "dtxtpercentoftotal";
		this.dtxtpercentoftotal.Size = new System.Drawing.Size(101, 22);
		this.dtxtpercentoftotal.TabIndex = 2;
		this.dtxtpercentoftotal.Text = "0";
		this.dtxtpercentoftotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.dtxtpercentoftotal.Leave += new System.EventHandler(dtxtpercentoftotal_TextChanged);
		this.btndcancel.BackColor = System.Drawing.Color.Teal;
		this.btndcancel.FlatAppearance.BorderSize = 0;
		this.btndcancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btndcancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btndcancel.ForeColor = System.Drawing.Color.White;
		this.btndcancel.Location = new System.Drawing.Point(959, 22);
		this.btndcancel.Name = "btndcancel";
		this.btndcancel.Size = new System.Drawing.Size(59, 26);
		this.btndcancel.TabIndex = 7;
		this.btndcancel.Text = "Cancel";
		this.btndcancel.UseVisualStyleBackColor = false;
		this.btndcancel.Visible = false;
		this.btndcancel.Click += new System.EventHandler(btndcancel_Click);
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(428, 3);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(48, 15);
		this.label83.TabIndex = 509;
		this.label83.Text = "Balance";
		this.btndupdate.BackColor = System.Drawing.Color.Teal;
		this.btndupdate.FlatAppearance.BorderSize = 0;
		this.btndupdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btndupdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btndupdate.ForeColor = System.Drawing.Color.White;
		this.btndupdate.Location = new System.Drawing.Point(893, 22);
		this.btndupdate.Name = "btndupdate";
		this.btndupdate.Size = new System.Drawing.Size(63, 26);
		this.btndupdate.TabIndex = 6;
		this.btndupdate.Text = "Update";
		this.btndupdate.UseVisualStyleBackColor = false;
		this.btndupdate.Visible = false;
		this.btndupdate.Click += new System.EventHandler(btndupdate_Click);
		this.txtBalanceFI.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceFI.Location = new System.Drawing.Point(431, 24);
		this.txtBalanceFI.Name = "txtBalanceFI";
		this.txtBalanceFI.ReadOnly = true;
		this.txtBalanceFI.Size = new System.Drawing.Size(101, 22);
		this.txtBalanceFI.TabIndex = 2;
		this.txtBalanceFI.TabStop = false;
		this.txtBalanceFI.Text = "0";
		this.txtBalanceFI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceFI.Leave += new System.EventHandler(dtxtpercentoftotal_TextChanged);
		this.btndadd.BackColor = System.Drawing.Color.Teal;
		this.btndadd.FlatAppearance.BorderSize = 0;
		this.btndadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btndadd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btndadd.ForeColor = System.Drawing.Color.White;
		this.btndadd.Location = new System.Drawing.Point(859, 22);
		this.btndadd.Name = "btndadd";
		this.btndadd.Size = new System.Drawing.Size(31, 26);
		this.btndadd.TabIndex = 5;
		this.btndadd.Text = "+";
		this.btndadd.UseVisualStyleBackColor = false;
		this.btndadd.Click += new System.EventHandler(btndadd_Click);
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(263, 3);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(84, 15);
		this.label76.TabIndex = 511;
		this.label76.Text = "Payment Term";
		this.dtxtdueday.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtxtdueday.Location = new System.Drawing.Point(750, 24);
		this.dtxtdueday.Name = "dtxtdueday";
		this.dtxtdueday.Size = new System.Drawing.Size(101, 22);
		this.dtxtdueday.TabIndex = 4;
		this.dtxtdueday.Text = "0";
		this.dtxtdueday.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.dcmbpaymentterm.AllowNull = (DefaultableBoolean)1;
		this.dcmbpaymentterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.dcmbpaymentterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.dcmbpaymentterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance438).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance438).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance438;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance439).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance439).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance439).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance439).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance439;
		((AppearanceBase)appearance440).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance440;
		((SpecialBoxBase)((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance441).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance441).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance441).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance441).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance441;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance442).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance442).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance442;
		((AppearanceBase)appearance443).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance443).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance443;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance444).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance444;
		((AppearanceBase)appearance447).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance447).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance447;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance448).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance448).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance448).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance448).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance448).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance448;
		((AppearanceBase)appearance449).TextHAlignAsString = "Left";
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance449;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance450).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance450).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance450;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance451).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance451;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Location = new System.Drawing.Point(266, 23);
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Name = "dcmbpaymentterm";
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Size = new System.Drawing.Size(161, 25);
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).TabIndex = 1;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(757, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(58, 15);
		this.label78.TabIndex = 515;
		this.label78.Text = "Due Days";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(650, 3);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(72, 15);
		this.label77.TabIndex = 513;
		this.label77.Text = "Fcy Amount";
		this.dtxtfcyamount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtxtfcyamount.Location = new System.Drawing.Point(643, 24);
		this.dtxtfcyamount.Name = "dtxtfcyamount";
		this.dtxtfcyamount.Size = new System.Drawing.Size(101, 22);
		this.dtxtfcyamount.TabIndex = 3;
		this.dtxtfcyamount.Text = "0";
		this.dtxtfcyamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.dtxtfcyamount.Leave += new System.EventHandler(dtxtfcyamount_TextChanged);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 3);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1136, 23);
		this.panel20.TabIndex = 0;
		this.panel11.Controls.Add(this.HeaderGropBox);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranches);
		this.panel11.Controls.Add(this.label13);
		this.panel11.Controls.Add(this.label18);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.panel11.Controls.Add(this.txtcertificate);
		this.panel11.Controls.Add(this.label61);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1150, 302);
		this.panel11.TabIndex = 1;
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPrefixType);
		this.HeaderGropBox.Controls.Add(this.label156);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbinvoiceno);
		this.HeaderGropBox.Controls.Add(this.groupBox2);
		this.HeaderGropBox.Controls.Add(this.groupBox1);
		this.HeaderGropBox.Controls.Add(this.label20);
		this.HeaderGropBox.Controls.Add(this.txtremarksheader);
		this.HeaderGropBox.Controls.Add(this.label51);
		this.HeaderGropBox.Controls.Add(this.label2);
		this.HeaderGropBox.Controls.Add(this.label65);
		this.HeaderGropBox.Controls.Add(this.label1);
		this.HeaderGropBox.Controls.Add(this.label43);
		this.HeaderGropBox.Controls.Add(this.txtTotalNetAmount);
		this.HeaderGropBox.Controls.Add(this.label12);
		this.HeaderGropBox.Controls.Add(this.label45);
		this.HeaderGropBox.Controls.Add(this.label66);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew);
		this.HeaderGropBox.Controls.Add(this.txtaddlessamount);
		this.HeaderGropBox.Controls.Add(this.label60);
		this.HeaderGropBox.Controls.Add(this.label9);
		this.HeaderGropBox.Controls.Add(this.label25);
		this.HeaderGropBox.Controls.Add(this.label42);
		this.HeaderGropBox.Controls.Add(this.label64);
		this.HeaderGropBox.Controls.Add(this.label62);
		this.HeaderGropBox.Controls.Add(this.label16);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.HeaderGropBox.Controls.Add(this.txtcertificateOfOrigion);
		this.HeaderGropBox.Controls.Add(this.label56);
		this.HeaderGropBox.Controls.Add(this.label40);
		this.HeaderGropBox.Controls.Add(this.label26);
		this.HeaderGropBox.Controls.Add(this.txtaddlesscommnets);
		this.HeaderGropBox.Controls.Add(this.label19);
		this.HeaderGropBox.Controls.Add(this.label10);
		this.HeaderGropBox.Controls.Add(this.label22);
		this.HeaderGropBox.Controls.Add(this.txtLotRef);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcareiertype);
		this.HeaderGropBox.Controls.Add(this.label31);
		this.HeaderGropBox.Controls.Add(this.label41);
		this.HeaderGropBox.Controls.Add(this.label57);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew);
		this.HeaderGropBox.Controls.Add(this.txtNoOfContainer);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbimporterBankNew);
		this.HeaderGropBox.Controls.Add(this.label55);
		this.HeaderGropBox.Controls.Add(this.label7);
		this.HeaderGropBox.Controls.Add(this.txtNetWeight);
		this.HeaderGropBox.Controls.Add(this.label11);
		this.HeaderGropBox.Controls.Add(this.label54);
		this.HeaderGropBox.Controls.Add(this.txtGrossWeight);
		this.HeaderGropBox.Controls.Add(this.label53);
		this.HeaderGropBox.Controls.Add(this.label49);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty2);
		this.HeaderGropBox.Controls.Add(this.label50);
		this.HeaderGropBox.Controls.Add(this.txtIformDate);
		this.HeaderGropBox.Controls.Add(this.txtGdNo);
		this.HeaderGropBox.Controls.Add(this.label46);
		this.HeaderGropBox.Controls.Add(this.txtLocalAmt);
		this.HeaderGropBox.Controls.Add(this.txtExRate);
		this.HeaderGropBox.Controls.Add(this.label44);
		this.HeaderGropBox.Controls.Add(this.label47);
		this.HeaderGropBox.Controls.Add(this.txtfcyAmount);
		this.HeaderGropBox.Controls.Add(this.label27);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbfcycode);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbexporterBankNew);
		this.HeaderGropBox.Controls.Add(this.label24);
		this.HeaderGropBox.Controls.Add(this.label67);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty1);
		this.HeaderGropBox.Controls.Add(this.label8);
		this.HeaderGropBox.Controls.Add(this.label4);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPreInvoice);
		this.HeaderGropBox.Controls.Add(this.label17);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupCust);
		this.HeaderGropBox.Controls.Add(this.label21);
		this.HeaderGropBox.Controls.Add(this.label3);
		this.HeaderGropBox.Controls.Add(this.label68);
		this.HeaderGropBox.Controls.Add(this.txtDocDate);
		this.HeaderGropBox.Controls.Add(this.txtdocno);
		this.HeaderGropBox.Controls.Add(this.label5);
		this.HeaderGropBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.HeaderGropBox.Location = new System.Drawing.Point(10, 2);
		this.HeaderGropBox.Name = "HeaderGropBox";
		this.HeaderGropBox.Size = new System.Drawing.Size(1104, 295);
		this.HeaderGropBox.TabIndex = 0;
		this.HeaderGropBox.TabStop = false;
		this.HeaderGropBox.Text = "Main Header";
		this.cmbPrefixType.AllowNull = (DefaultableBoolean)1;
		this.cmbPrefixType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPrefixType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPrefixType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance363).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance363;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance364).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance364).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance364).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPrefixType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance364;
		((AppearanceBase)appearance365).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance365;
		((SpecialBoxBase)((UltraGridBase)this.cmbPrefixType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance415).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance415).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance415).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance415).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance415;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance416).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance416).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance416;
		((AppearanceBase)appearance417).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance417).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance417;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance418).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance418;
		((AppearanceBase)appearance419).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance419).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance419;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance420).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance420).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance420).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance420).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance420).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance420;
		((AppearanceBase)appearance421).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance421;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance422).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance422).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance422;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance424).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance424;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPrefixType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPrefixType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPrefixType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPrefixType).Location = new System.Drawing.Point(123, 69);
		((System.Windows.Forms.Control)(object)this.cmbPrefixType).Name = "cmbPrefixType";
		((System.Windows.Forms.Control)(object)this.cmbPrefixType).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbPrefixType).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbPrefixType).Leave += new System.EventHandler(cmbPrefixType_TextChanged);
		this.label156.AutoSize = true;
		this.label156.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label156.Location = new System.Drawing.Point(1, 75);
		this.label156.Name = "label156";
		this.label156.Size = new System.Drawing.Size(65, 15);
		this.label156.TabIndex = 595;
		this.label156.Text = "Prefix Type";
		this.Cmbinvoiceno.AllowNull = (DefaultableBoolean)1;
		this.Cmbinvoiceno.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbinvoiceno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbinvoiceno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance584).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance584).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance584;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance585).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance585).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance585).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance585).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance585;
		((AppearanceBase)appearance586).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance586;
		((SpecialBoxBase)((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance587).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance587).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance587).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance587).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance587;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance588).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance588).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance588;
		((AppearanceBase)appearance589).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance589).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance589;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance590).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance590;
		((AppearanceBase)appearance592).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance592).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance592;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance593).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance593).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance593).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance593).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance593).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance593;
		((AppearanceBase)appearance594).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance594;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance595).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance595).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance595;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance596).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance596;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.Cmbinvoiceno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).Location = new System.Drawing.Point(123, 97);
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).Name = "Cmbinvoiceno";
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).TabIndex = 4;
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort);
		this.groupBox2.Controls.Add(this.label73);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbOtherCustomer);
		this.groupBox2.Location = new System.Drawing.Point(820, 177);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(281, 86);
		this.groupBox2.TabIndex = 30;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail For Bank ";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(3, 55);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(93, 15);
		this.label52.TabIndex = 597;
		this.label52.Text = "Destination Port";
		this.cmbOtherDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbOtherDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOtherDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbOtherDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbOtherDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).Location = new System.Drawing.Point(96, 49);
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).Name = "cmbOtherDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).TabIndex = 1;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label73.Location = new System.Drawing.Point(3, 25);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(93, 15);
		this.label73.TabIndex = 595;
		this.label73.Text = "Customer Name";
		this.cmbOtherCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbOtherCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOtherCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance610).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance610).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance610;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance359).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance359;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance360).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance360;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance361).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance361;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbOtherCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).Location = new System.Drawing.Point(96, 19);
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).Name = "cmbOtherCustomer";
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).TabIndex = 0;
		this.groupBox1.Controls.Add(this.label91);
		this.groupBox1.Controls.Add(this.label94);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label95);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommissionAgent);
		this.groupBox1.Controls.Add(this.txtCommPercent);
		this.groupBox1.Controls.Add(this.txtCommAmount);
		this.groupBox1.Location = new System.Drawing.Point(820, 74);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(281, 102);
		this.groupBox1.TabIndex = 29;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Commission";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label91.Location = new System.Drawing.Point(3, 22);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(39, 15);
		this.label91.TabIndex = 594;
		this.label91.Text = "Agent";
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label94.Location = new System.Drawing.Point(3, 50);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(56, 15);
		this.label94.TabIndex = 598;
		this.label94.Text = "Comm %";
		this.CmbCommissionDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance362).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance362;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance611).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance611).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance611).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance611).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance611;
		((AppearanceBase)appearance612).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance612;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance366).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance366).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance366;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance367).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance367;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance369;
		((AppearanceBase)appearance370).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance370).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance370;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance371).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance371).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance371).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance371).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance371;
		((AppearanceBase)appearance372).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance372;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance373).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance373;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance374).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance374;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommissionDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Location = new System.Drawing.Point(59, 72);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Name = "CmbCommissionDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).TabIndex = 3;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(3, 78);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(53, 15);
		this.label28.TabIndex = 601;
		this.label28.Text = "Debit Ac";
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label95.Location = new System.Drawing.Point(127, 50);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(51, 15);
		this.label95.TabIndex = 1;
		this.label95.Text = "Amount";
		this.cmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance385).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance385).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance385;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance386).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance386).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance386).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance386;
		((AppearanceBase)appearance387).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance387;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance375).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance375).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance375).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance375).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance376).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance376;
		((AppearanceBase)appearance377).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance377).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance378;
		((AppearanceBase)appearance380).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance380).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance381).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance381).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance381).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance381).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance381;
		((AppearanceBase)appearance382).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance382;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance383).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance383).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance383;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance384;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Location = new System.Drawing.Point(59, 16);
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Name = "cmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).TabIndex = 0;
		this.txtCommPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommPercent.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommPercent.Location = new System.Drawing.Point(59, 46);
		this.txtCommPercent.Name = "txtCommPercent";
		this.txtCommPercent.Size = new System.Drawing.Size(65, 23);
		this.txtCommPercent.TabIndex = 1;
		this.txtCommPercent.Text = "0";
		this.txtCommPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommPercent.TextChanged += new System.EventHandler(txtCommPercent_TextChanged);
		this.txtCommPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommPercent_KeyPress);
		this.txtCommAmount.BackColor = System.Drawing.Color.White;
		this.txtCommAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAmount.Enabled = false;
		this.txtCommAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommAmount.Location = new System.Drawing.Point(179, 46);
		this.txtCommAmount.Name = "txtCommAmount";
		this.txtCommAmount.ReadOnly = true;
		this.txtCommAmount.Size = new System.Drawing.Size(97, 23);
		this.txtCommAmount.TabIndex = 2;
		this.txtCommAmount.TabStop = false;
		this.txtCommAmount.Text = "0";
		this.txtCommAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommAmount.TextChanged += new System.EventHandler(txtCommAmount_TextChanged);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(337, 269);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(52, 15);
		this.label20.TabIndex = 593;
		this.label20.Text = "Remarks";
		this.txtremarksheader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksheader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksheader.Location = new System.Drawing.Point(437, 265);
		this.txtremarksheader.Name = "txtremarksheader";
		this.txtremarksheader.Size = new System.Drawing.Size(659, 23);
		this.txtremarksheader.TabIndex = 31;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(418, 20);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(14, 17);
		this.label51.TabIndex = 19;
		this.label51.Text = "*";
		this.label51.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(565, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 590;
		this.label2.Text = "GD Date";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Red;
		this.label65.Location = new System.Drawing.Point(420, 102);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(14, 17);
		this.label65.TabIndex = 586;
		this.label65.Text = "*";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(658, 130);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 43;
		this.label1.Text = "*";
		this.label1.Visible = false;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(658, 157);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 42;
		this.label43.Text = "*";
		this.label43.Visible = false;
		this.txtTotalNetAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalNetAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalNetAmount.Location = new System.Drawing.Point(673, 182);
		this.txtTotalNetAmount.Name = "txtTotalNetAmount";
		this.txtTotalNetAmount.ReadOnly = true;
		this.txtTotalNetAmount.Size = new System.Drawing.Size(141, 23);
		this.txtTotalNetAmount.TabIndex = 25;
		this.txtTotalNetAmount.TabStop = false;
		this.txtTotalNetAmount.Text = "0";
		this.txtTotalNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalNetAmount.TextChanged += new System.EventHandler(txtTotalNetAmount_TextChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(337, 48);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(80, 15);
		this.label12.TabIndex = 526;
		this.label12.Text = "Delivery Term";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(658, 185);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 550;
		this.label45.Text = "*";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(565, 186);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(80, 15);
		this.label66.TabIndex = 585;
		this.label66.Text = "Total Amount";
		this.cmbdeliverytermnew.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliverytermnew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliverytermnew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliverytermnew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdeliverytermnew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Location = new System.Drawing.Point(437, 42);
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Name = "cmbdeliverytermnew";
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).TabIndex = 12;
		this.txtaddlessamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlessamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtaddlessamount.Location = new System.Drawing.Point(437, 182);
		this.txtaddlessamount.Name = "txtaddlessamount";
		this.txtaddlessamount.Size = new System.Drawing.Size(122, 23);
		this.txtaddlessamount.TabIndex = 17;
		this.txtaddlessamount.Text = "0";
		this.txtaddlessamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtaddlessamount.TextChanged += new System.EventHandler(txtaddlessamount_TextChanged);
		this.txtaddlessamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtaddlessamount_KeyPress);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(420, 130);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 573;
		this.label60.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(420, 47);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 528;
		this.label9.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(565, 131);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(74, 15);
		this.label25.TabIndex = 538;
		this.label25.Text = "Fcy-Amount";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(565, 158);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(82, 15);
		this.label42.TabIndex = 549;
		this.label42.Text = "Local Amount";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(337, 186);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(101, 15);
		this.label64.TabIndex = 582;
		this.label64.Text = "Add Less Amount";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(1, 269);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(99, 15);
		this.label62.TabIndex = 577;
		this.label62.Text = "Health Permit No";
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(565, 103);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(93, 15);
		this.label16.TabIndex = 533;
		this.label16.Text = "Destination Port";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(673, 69);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 21;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(673, 97);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 22;
		this.txtcertificateOfOrigion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcertificateOfOrigion.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcertificateOfOrigion.Location = new System.Drawing.Point(123, 265);
		this.txtcertificateOfOrigion.Name = "txtcertificateOfOrigion";
		this.txtcertificateOfOrigion.Size = new System.Drawing.Size(207, 23);
		this.txtcertificateOfOrigion.TabIndex = 10;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(565, 75);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(75, 15);
		this.label56.TabIndex = 534;
		this.label56.Text = "Loading Port";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(337, 214);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(99, 15);
		this.label40.TabIndex = 580;
		this.label40.Text = "Ad/Ls Comments";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(658, 74);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(14, 17);
		this.label26.TabIndex = 535;
		this.label26.Text = "*";
		this.txtaddlesscommnets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlesscommnets.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtaddlesscommnets.Location = new System.Drawing.Point(437, 207);
		this.txtaddlesscommnets.Multiline = true;
		this.txtaddlesscommnets.Name = "txtaddlesscommnets";
		this.txtaddlesscommnets.Size = new System.Drawing.Size(122, 56);
		this.txtaddlesscommnets.TabIndex = 18;
		this.txtaddlesscommnets.TextChanged += new System.EventHandler(txtaddlesscommnets_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(658, 102);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 536;
		this.label19.Text = "*";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(1, 186);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(80, 15);
		this.label10.TabIndex = 120;
		this.label10.Text = "Notify Party 2";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(337, 75);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(65, 15);
		this.label22.TabIndex = 542;
		this.label22.Text = "Carier Type";
		this.txtLotRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotRef.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLotRef.Location = new System.Drawing.Point(437, 99);
		this.txtLotRef.Name = "txtLotRef";
		this.txtLotRef.Size = new System.Drawing.Size(122, 23);
		this.txtLotRef.TabIndex = 14;
		this.cmbcareiertype.AllowNull = (DefaultableBoolean)1;
		this.cmbcareiertype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcareiertype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcareiertype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcareiertype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Location = new System.Drawing.Point(437, 69);
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Name = "cmbcareiertype";
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).TabIndex = 13;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(1, 214);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(83, 15);
		this.label31.TabIndex = 524;
		this.label31.Text = "Importer Bank";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(420, 74);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 545;
		this.label41.Text = "*";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(565, 214);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(93, 15);
		this.label57.TabIndex = 552;
		this.label57.Text = "No.Of Container";
		this.cmbPaymentTermsNew.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTermsNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTermsNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTermsNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPaymentTermsNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Location = new System.Drawing.Point(437, 15);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Name = "cmbPaymentTermsNew";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).TabIndex = 11;
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainer.Location = new System.Drawing.Point(673, 210);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(141, 23);
		this.txtNoOfContainer.TabIndex = 26;
		this.txtNoOfContainer.Text = "0";
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfContainer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfContainer_KeyPress);
		this.cmbimporterBankNew.AllowNull = (DefaultableBoolean)1;
		this.cmbimporterBankNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbimporterBankNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbimporterBankNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbimporterBankNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Location = new System.Drawing.Point(123, 208);
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Name = "cmbimporterBankNew";
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).TabIndex = 8;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(822, 48);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(86, 15);
		this.label55.TabIndex = 554;
		this.label55.Text = "Net Weight KG";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(337, 103);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(66, 15);
		this.label7.TabIndex = 57;
		this.label7.Text = "Lot Ref No.";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(960, 44);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(141, 23);
		this.txtNetWeight.TabIndex = 28;
		this.txtNetWeight.Text = "0";
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(337, 21);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(84, 15);
		this.label11.TabIndex = 527;
		this.label11.Text = "Payment Term";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(822, 21);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(96, 15);
		this.label54.TabIndex = 553;
		this.label54.Text = "Gross Weight KG";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(960, 17);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(141, 23);
		this.txtGrossWeight.TabIndex = 27;
		this.txtGrossWeight.Text = "0";
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(658, 213);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 570;
		this.label53.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(917, 47);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 560;
		this.label49.Text = "*";
		this.cmbNotifyParty2.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty2.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbNotifyParty2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Location = new System.Drawing.Point(123, 180);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Name = "cmbNotifyParty2";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).TabIndex = 7;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(917, 20);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 569;
		this.label50.Text = "*";
		this.txtIformDate.CustomFormat = "dd-MMM-yy";
		this.txtIformDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtIformDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtIformDate.Location = new System.Drawing.Point(673, 44);
		this.txtIformDate.Name = "txtIformDate";
		this.txtIformDate.Size = new System.Drawing.Size(141, 23);
		this.txtIformDate.TabIndex = 20;
		this.txtGdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGdNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGdNo.Location = new System.Drawing.Point(673, 17);
		this.txtGdNo.Name = "txtGdNo";
		this.txtGdNo.Size = new System.Drawing.Size(141, 23);
		this.txtGdNo.TabIndex = 19;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(565, 21);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(43, 15);
		this.label46.TabIndex = 561;
		this.label46.Text = "GD No";
		this.txtLocalAmt.BackColor = System.Drawing.Color.White;
		this.txtLocalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLocalAmt.Location = new System.Drawing.Point(673, 154);
		this.txtLocalAmt.Name = "txtLocalAmt";
		this.txtLocalAmt.ReadOnly = true;
		this.txtLocalAmt.Size = new System.Drawing.Size(141, 23);
		this.txtLocalAmt.TabIndex = 24;
		this.txtLocalAmt.TabStop = false;
		this.txtLocalAmt.Text = "0";
		this.txtLocalAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLocalAmt.TextChanged += new System.EventHandler(txtLocalAmt_TextChanged);
		this.txtExRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExRate.Location = new System.Drawing.Point(437, 154);
		this.txtExRate.Name = "txtExRate";
		this.txtExRate.Size = new System.Drawing.Size(122, 23);
		this.txtExRate.TabIndex = 16;
		this.txtExRate.Text = "0";
		this.txtExRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExRate.TextChanged += new System.EventHandler(txtExRate_TextChanged);
		this.txtExRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExRate_KeyPress);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(337, 158);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 15);
		this.label44.TabIndex = 546;
		this.label44.Text = "Exchange Rate";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(420, 157);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 543;
		this.label47.Text = "*";
		this.txtfcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfcyAmount.Location = new System.Drawing.Point(673, 127);
		this.txtfcyAmount.Name = "txtfcyAmount";
		this.txtfcyAmount.ReadOnly = true;
		this.txtfcyAmount.Size = new System.Drawing.Size(141, 23);
		this.txtfcyAmount.TabIndex = 23;
		this.txtfcyAmount.Text = "0";
		this.txtfcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcyAmount.TextChanged += new System.EventHandler(txtfcyAmount_TextChanged);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(337, 131);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(55, 15);
		this.label27.TabIndex = 537;
		this.label27.Text = "Fcy Code";
		this.cmbfcycode.AllowNull = (DefaultableBoolean)1;
		this.cmbfcycode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbfcycode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbfcycode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbfcycode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Location = new System.Drawing.Point(437, 125);
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Name = "cmbfcycode";
		this.cmbfcycode.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbfcycode).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).TabStop = false;
		this.cmbexporterBankNew.AllowNull = (DefaultableBoolean)1;
		this.cmbexporterBankNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbexporterBankNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbexporterBankNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbexporterBankNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Location = new System.Drawing.Point(123, 237);
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Name = "cmbexporterBankNew";
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).TabIndex = 9;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(1, 243);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(80, 15);
		this.label24.TabIndex = 525;
		this.label24.Text = "Exporter Bank";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(1, 103);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(65, 15);
		this.label67.TabIndex = 57;
		this.label67.Text = "Invoice No";
		this.cmbNotifyParty1.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbNotifyParty1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Location = new System.Drawing.Point(123, 152);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Name = "cmbNotifyParty1";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).TabIndex = 6;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(1, 158);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 117;
		this.label8.Text = "Notify Party 1";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(109, 130);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 17);
		this.label4.TabIndex = 115;
		this.label4.Text = "*";
		this.CmbPreInvoice.AllowNull = (DefaultableBoolean)1;
		this.CmbPreInvoice.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPreInvoice.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPreInvoice.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance559).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance559).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Appearance = (AppearanceBase)(object)appearance559;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance560).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance560).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance560).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance560).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPreInvoice).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance560;
		((AppearanceBase)appearance561).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance561;
		((SpecialBoxBase)((UltraGridBase)this.CmbPreInvoice).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance562).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance562).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance562).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance562).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance562;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance563).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance563).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance563;
		((AppearanceBase)appearance564).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance564).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance564;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance565).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance565;
		((AppearanceBase)appearance566).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance566).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance566;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance567).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance567).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance567).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance567).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance567).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance567;
		((AppearanceBase)appearance569).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance569;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance570).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance570).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance570;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance571).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance571;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPreInvoice).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPreInvoice).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPreInvoice.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPreInvoice).Location = new System.Drawing.Point(123, 42);
		((System.Windows.Forms.Control)(object)this.CmbPreInvoice).Name = "CmbPreInvoice";
		((System.Windows.Forms.Control)(object)this.CmbPreInvoice).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbPreInvoice).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbPreInvoice).Leave += new System.EventHandler(cmbSupCust_Leave_1);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(1, 48);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(66, 15);
		this.label17.TabIndex = 78;
		this.label17.Text = "Pre Invoice";
		this.cmbSupCust.AllowNull = (DefaultableBoolean)1;
		this.cmbSupCust.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupCust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupCust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance402).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance402).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance402;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance403).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance403).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance403).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance403).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance403;
		((AppearanceBase)appearance404).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance404;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance405).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance405).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance405).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance405).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance405;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance406).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance406).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance406;
		((AppearanceBase)appearance407).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance407).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance407;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance408).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance408;
		((AppearanceBase)appearance409).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance409).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance409;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance410).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance410).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance410).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance410).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance410).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance410;
		((AppearanceBase)appearance411).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance411;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance413).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance413).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance413;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance414).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance414;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSupCust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Location = new System.Drawing.Point(123, 125);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Name = "cmbSupCust";
		this.cmbSupCust.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Leave += new System.EventHandler(cmbSupCust_Leave_1);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(1, 131);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(110, 15);
		this.label21.TabIndex = 78;
		this.label21.Text = "Customer/Importer";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(109, 20);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 0;
		this.label3.Text = "*";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(109, 102);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(14, 17);
		this.label68.TabIndex = 50;
		this.label68.Text = "*";
		this.txtDocDate.CustomFormat = "dd-MMM-yy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(201, 17);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(129, 23);
		this.txtDocDate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(123, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(73, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(1, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 49;
		this.label5.Text = "Doc No && Date";
		this.cmbbranches.AllowNull = (DefaultableBoolean)1;
		this.cmbbranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbbranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbbranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranches).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbbranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranches).Location = new System.Drawing.Point(1053, 326);
		((System.Windows.Forms.Control)(object)this.cmbbranches).Name = "cmbbranches";
		((System.Windows.Forms.Control)(object)this.cmbbranches).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbbranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label13.Location = new System.Drawing.Point(968, 332);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 40;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label18.Location = new System.Drawing.Point(968, 355);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(46, 15);
		this.label18.TabIndex = 41;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproject).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(1053, 356);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbproject).Visible = false;
		this.txtcertificate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcertificate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcertificate.Location = new System.Drawing.Point(135, 352);
		this.txtcertificate.Name = "txtcertificate";
		this.txtcertificate.Size = new System.Drawing.Size(691, 23);
		this.txtcertificate.TabIndex = 11;
		this.txtcertificate.Visible = false;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(13, 356);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(93, 15);
		this.label61.TabIndex = 576;
		this.label61.Text = "Special Remarks";
		this.label61.Visible = false;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label23);
		this.panel10.Controls.Add(this.chk548Print);
		this.panel10.Controls.Add(this.ChkPrintPreview);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1150, 26);
		this.panel10.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(169, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Commercial Invoice";
		this.chk548Print.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chk548Print.AutoSize = true;
		this.chk548Print.Checked = true;
		this.chk548Print.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chk548Print.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chk548Print.ForeColor = System.Drawing.Color.White;
		this.chk548Print.Location = new System.Drawing.Point(885, 5);
		this.chk548Print.Name = "chk548Print";
		this.chk548Print.Size = new System.Drawing.Size(128, 19);
		this.chk548Print.TabIndex = 31;
		this.chk548Print.Text = "548_Print Preview";
		this.chk548Print.UseVisualStyleBackColor = true;
		this.ChkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreview.Location = new System.Drawing.Point(1018, 5);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(128, 19);
		this.ChkPrintPreview.TabIndex = 31;
		this.ChkPrintPreview.Text = "549_Print Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add(this.toolStrip4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1150, 27);
		this.panel12.TabIndex = 0;
		this.toolStrip4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnNew, this.btnsave, this.btnupdate, this.btnrefersh, this.btnAttachment, this.toolStripSeparator2, this.btnLoaderForm, this.btnVoucherPrint, this.btnPrint548, this.toolStripSeparator3,
			this.btnPrint, this.BtnPaymentDetail, this.btnGenerateInvoiceNo
		});
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1150, 27);
		this.toolStrip4.TabIndex = 4;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnrefersh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnrefersh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnrefersh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefersh.Name = "btnrefersh";
		this.btnrefersh.Size = new System.Drawing.Size(80, 24);
		this.btnrefersh.Text = "&Refresh";
		this.btnrefersh.Click += new System.EventHandler(btnrefersh_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnLoaderForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoaderForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoaderForm.Name = "btnLoaderForm";
		this.btnLoaderForm.Size = new System.Drawing.Size(119, 24);
		this.btnLoaderForm.Text = "&Load Forwarding";
		this.btnLoaderForm.Click += new System.EventHandler(btnLoaderForm_Click);
		this.btnVoucherPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherPrint.Name = "btnVoucherPrint";
		this.btnVoucherPrint.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherPrint.Text = "Voucher-103";
		this.btnVoucherPrint.Click += new System.EventHandler(btnVoucherPrint_Click);
		this.btnPrint548.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint548.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint548.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint548.Name = "btnPrint548";
		this.btnPrint548.Size = new System.Drawing.Size(128, 24);
		this.btnPrint548.Text = "Bank Print-548";
		this.btnPrint548.Click += new System.EventHandler(btnPrint548_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "Print-549";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.BtnPaymentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPaymentDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPaymentDetail.Name = "BtnPaymentDetail";
		this.BtnPaymentDetail.Size = new System.Drawing.Size(110, 24);
		this.BtnPaymentDetail.Text = "Payment Detail";
		this.BtnPaymentDetail.Click += new System.EventHandler(BtnPaymentDetail_Click);
		this.btnGenerateInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerateInvoiceNo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateInvoiceNo.Name = "btnGenerateInvoiceNo";
		this.btnGenerateInvoiceNo.Size = new System.Drawing.Size(126, 24);
		this.btnGenerateInvoiceNo.Text = "&Generate Inv Nos";
		this.btnGenerateInvoiceNo.Click += new System.EventHandler(btnGenerateInvoiceNo_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailHistory);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1156, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 151);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1150, 302);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 44;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label89);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel3.Location = new System.Drawing.Point(3, 453);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1150, 21);
		this.panel3.TabIndex = 46;
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label89.ForeColor = System.Drawing.Color.White;
		this.label89.Location = new System.Drawing.Point(5, 2);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(211, 17);
		this.label89.TabIndex = 46;
		this.label89.Text = "Detail Of Above Selected Row";
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label153);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 128);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1150, 23);
		this.panel15.TabIndex = 1;
		this.label153.AutoSize = true;
		this.label153.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label153.ForeColor = System.Drawing.Color.White;
		this.label153.Location = new System.Drawing.Point(4, 2);
		this.label153.Name = "label153";
		this.label153.Size = new System.Drawing.Size(142, 19);
		this.label153.TabIndex = 1;
		this.label153.Text = "Filtered Records";
		this.gridDetailHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailHistory.DesignTimeLayout = gridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.gridDetailHistory.DynamicFiltering = true;
		this.gridDetailHistory.FilterMode = (FilterMode)2;
		this.gridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Location = new System.Drawing.Point(3, 474);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Name = "gridDetailHistory";
		((ScrollableBase)this.gridDetailHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Size = new System.Drawing.Size(1150, 246);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).TabIndex = 47;
		this.gridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel16.Controls.Add(this.groupBox7);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 55);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1150, 73);
		this.panel16.TabIndex = 0;
		this.groupBox7.Controls.Add(this.rdapproveddate);
		this.groupBox7.Controls.Add(this.btnShowHistory);
		this.groupBox7.Controls.Add(this.rdmodifydate);
		this.groupBox7.Controls.Add(this.label148);
		this.groupBox7.Controls.Add(this.drdocdate);
		this.groupBox7.Controls.Add(this.rdentrydate);
		this.groupBox7.Controls.Add(this.label149);
		this.groupBox7.Controls.Add(this.label150);
		this.groupBox7.Controls.Add(this.label151);
		this.groupBox7.Controls.Add(this.label152);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox7.Controls.Add(this.ToDocNoHistory);
		this.groupBox7.Controls.Add(this.FromDocNoHistory);
		this.groupBox7.Controls.Add(this.ToDateHistory);
		this.groupBox7.Controls.Add(this.FromDateHistory);
		this.groupBox7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox7.Location = new System.Drawing.Point(5, 1);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(927, 71);
		this.groupBox7.TabIndex = 1;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label148.AutoSize = true;
		this.label148.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label148.Location = new System.Drawing.Point(373, 20);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(58, 15);
		this.label148.TabIndex = 87;
		this.label148.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(735, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(735, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(213, 46);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(64, 15);
		this.label149.TabIndex = 86;
		this.label149.Text = "To Doc No";
		this.label150.AutoSize = true;
		this.label150.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label150.Location = new System.Drawing.Point(213, 20);
		this.label150.Name = "label150";
		this.label150.Size = new System.Drawing.Size(79, 15);
		this.label150.TabIndex = 85;
		this.label150.Text = "From Doc No";
		this.label151.AutoSize = true;
		this.label151.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label151.Location = new System.Drawing.Point(3, 46);
		this.label151.Name = "label151";
		this.label151.Size = new System.Drawing.Size(48, 15);
		this.label151.TabIndex = 84;
		this.label151.Text = "To Date";
		this.label152.AutoSize = true;
		this.label152.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label152.Location = new System.Drawing.Point(3, 20);
		this.label152.Name = "label152";
		this.label152.Size = new System.Drawing.Size(63, 15);
		this.label152.TabIndex = 83;
		this.label152.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance501).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance501).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance501;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance512).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance512).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance512).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance512).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance512;
		((AppearanceBase)appearance523).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance523;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance545).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance545).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance545).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance545).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance545;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance557).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance557).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance557;
		((AppearanceBase)appearance568).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance568).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance568;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance574).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance574;
		((AppearanceBase)appearance575).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance575).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance575;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance576).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance576).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance576).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance576).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance576).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance576;
		((AppearanceBase)appearance577).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance577;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance578).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance578).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance578;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance579).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance579;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 42);
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
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.toolStrip3);
		this.panel13.Controls.Add(this.label69);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1150, 52);
		this.panel13.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnResetHistory, this.btnRefreshHistory, this.btnSlipHistory, this.toolStripSeparator1, this.btnBankSlipHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1150, 27);
		this.toolStrip3.TabIndex = 52;
		this.toolStrip3.Text = "toolStrip2";
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
		this.btnSlipHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlipHistory.Image = (System.Drawing.Image)resources.GetObject("btnSlipHistory.Image");
		this.btnSlipHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlipHistory.Name = "btnSlipHistory";
		this.btnSlipHistory.Size = new System.Drawing.Size(102, 24);
		this.btnSlipHistory.Text = "549A_Print";
		this.btnSlipHistory.Click += new System.EventHandler(btnSlipHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnBankSlipHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnBankSlipHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBankSlipHistory.Name = "btnBankSlipHistory";
		this.btnBankSlipHistory.Size = new System.Drawing.Size(110, 24);
		this.btnBankSlipHistory.Text = "548A-Bank Slip";
		this.btnBankSlipHistory.Click += new System.EventHandler(btnBankSlipHistory_Click);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(2, 29);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(232, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Commercial Invoice History";
		this.tabPage7.Controls.Add(this.panel26);
		this.tabPage7.Location = new System.Drawing.Point(4, 4);
		this.tabPage7.Name = "tabPage7";
		this.tabPage7.Size = new System.Drawing.Size(1156, 723);
		this.tabPage7.TabIndex = 2;
		this.tabPage7.Text = "GD BreakUp";
		this.tabPage7.UseVisualStyleBackColor = true;
		this.panel26.Controls.Add(this.panel28);
		this.panel26.Controls.Add(this.panel27);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1156, 723);
		this.panel26.TabIndex = 0;
		this.panel28.Controls.Add(this.tabControl3);
		this.panel28.Controls.Add(this.panel14);
		this.panel28.Controls.Add(this.panel25);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel28.Location = new System.Drawing.Point(0, 30);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1156, 693);
		this.panel28.TabIndex = 1;
		this.tabControl3.Controls.Add(this.tabPage6);
		this.tabControl3.Controls.Add(this.tabPage8);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Location = new System.Drawing.Point(0, 341);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1156, 352);
		this.tabControl3.TabIndex = 9;
		this.tabControl3.SelectedIndexChanged += new System.EventHandler(tabControl3_SelectedIndexChanged);
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize);
		this.tabPage6.Controls.Add(this.panel33);
		this.tabPage6.Controls.Add(this.panel32);
		this.tabPage6.Controls.Add(this.panel29);
		this.tabPage6.Location = new System.Drawing.Point(4, 22);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(1148, 326);
		this.tabPage6.TabIndex = 0;
		this.tabPage6.Text = "Form";
		this.tabPage6.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdAdvanceUtlize).BackColor = System.Drawing.Color.White;
		grdAdvanceUtlize_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAdvanceUtlize.DesignTimeLayout = grdAdvanceUtlize_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAdvanceUtlize.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Location = new System.Drawing.Point(3, 136);
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Name = "grdAdvanceUtlize";
		((ScrollableBase)this.grdAdvanceUtlize).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAdvanceUtlize).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdAdvanceUtlize).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Size = new System.Drawing.Size(1142, 187);
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).TabIndex = 607;
		this.grdAdvanceUtlize.TotalRow = (InheritableBoolean)1;
		this.grdAdvanceUtlize.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAdvanceUtlize).VisualStyle = (VisualStyle)4;
		this.grdAdvanceUtlize.ColumnButtonClick += new ColumnActionEventHandler(grdAdvanceUtlize_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).DoubleClick += new System.EventHandler(grdAdvanceUtlize_DoubleClick);
		this.panel33.BackColor = System.Drawing.Color.Teal;
		this.panel33.Controls.Add(this.ctrlGrdBar6);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel33.Location = new System.Drawing.Point(3, 113);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(1142, 23);
		this.panel33.TabIndex = 605;
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceAdvanceUtilize);
		this.panel32.Controls.Add(this.label103);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize);
		this.panel32.Controls.Add(this.label108);
		this.panel32.Controls.Add(this.txtGdValueAdvanceUtlize);
		this.panel32.Controls.Add(this.label109);
		this.panel32.Controls.Add(this.btnAddAdvanceUtlize);
		this.panel32.Controls.Add(this.btnUpdateAdvanceUtilize);
		this.panel32.Controls.Add(this.label111);
		this.panel32.Controls.Add(this.btnCancelAdvanceUtilize);
		this.panel32.Controls.Add(this.txtFIBalanceAdvanceUtlize);
		this.panel32.Controls.Add(this.label112);
		this.panel32.Controls.Add(this.txtAdvanceUtlize);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize);
		this.panel32.Controls.Add(this.label115);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(3, 53);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1142, 60);
		this.panel32.TabIndex = 608;
		this.CmbInvoiceAdvanceUtilize.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceAdvanceUtilize.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceAdvanceUtilize.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceAdvanceUtilize.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance465).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance465).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Appearance = (AppearanceBase)(object)appearance465;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance466).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance466).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance466).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance466).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance466;
		((AppearanceBase)appearance467).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance467;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance469).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance469).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance469).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance469).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance469;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance470).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance470).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance470;
		((AppearanceBase)appearance471).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance471).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance471;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance472).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance472;
		((AppearanceBase)appearance473).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance473).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance473;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance474).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance474).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance474).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance474).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance474).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance474;
		((AppearanceBase)appearance475).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance475;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance476).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance476).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance476;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance477).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance477;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceAdvanceUtilize).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbInvoiceAdvanceUtilize.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceAdvanceUtilize).Location = new System.Drawing.Point(8, 29);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceAdvanceUtilize).Name = "CmbInvoiceAdvanceUtilize";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceAdvanceUtilize).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceAdvanceUtilize).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceAdvanceUtilize).Leave += new System.EventHandler(CmbInvoiceAdvanceUtilize_Leave);
		this.label103.AutoSize = true;
		this.label103.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label103.Location = new System.Drawing.Point(8, 10);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(65, 15);
		this.label103.TabIndex = 605;
		this.label103.Text = "Invoice No";
		this.CmbFIAdvanceUtlize.AllowNull = (DefaultableBoolean)1;
		this.CmbFIAdvanceUtlize.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFIAdvanceUtlize.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFIAdvanceUtlize.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance425).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance425).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Appearance = (AppearanceBase)(object)appearance425;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance426).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance426).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance426).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance426).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance426;
		((AppearanceBase)appearance427).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance427;
		((SpecialBoxBase)((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance428).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance428).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance428).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance428).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance428;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance429).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance429).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance429;
		((AppearanceBase)appearance430).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance430).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance430;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance431).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance431;
		((AppearanceBase)appearance432).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance432).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance432;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance433).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance433).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance433).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance433).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance433).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance433;
		((AppearanceBase)appearance435).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance435;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance436).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance436).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance436;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance437).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance437;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Location = new System.Drawing.Point(468, 30);
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Name = "CmbFIAdvanceUtlize";
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Size = new System.Drawing.Size(181, 25);
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).TextChanged += new System.EventHandler(CmbFIAdvanceUtlize_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Leave += new System.EventHandler(CmbFIAdvanceUtlize_TextChanged);
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.Location = new System.Drawing.Point(469, 10);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(140, 15);
		this.label108.TabIndex = 600;
		this.label108.Text = "Financial Instruments No";
		this.txtGdValueAdvanceUtlize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdValueAdvanceUtlize.Location = new System.Drawing.Point(363, 31);
		this.txtGdValueAdvanceUtlize.Name = "txtGdValueAdvanceUtlize";
		this.txtGdValueAdvanceUtlize.ReadOnly = true;
		this.txtGdValueAdvanceUtlize.Size = new System.Drawing.Size(102, 22);
		this.txtGdValueAdvanceUtlize.TabIndex = 4;
		this.txtGdValueAdvanceUtlize.TabStop = false;
		this.txtGdValueAdvanceUtlize.Text = "0";
		this.txtGdValueAdvanceUtlize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label109.Location = new System.Drawing.Point(360, 10);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(56, 15);
		this.label109.TabIndex = 513;
		this.label109.Text = "GD Value";
		this.btnAddAdvanceUtlize.BackColor = System.Drawing.Color.Teal;
		this.btnAddAdvanceUtlize.FlatAppearance.BorderSize = 0;
		this.btnAddAdvanceUtlize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddAdvanceUtlize.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddAdvanceUtlize.ForeColor = System.Drawing.Color.White;
		this.btnAddAdvanceUtlize.Location = new System.Drawing.Point(858, 29);
		this.btnAddAdvanceUtlize.Name = "btnAddAdvanceUtlize";
		this.btnAddAdvanceUtlize.Size = new System.Drawing.Size(31, 26);
		this.btnAddAdvanceUtlize.TabIndex = 6;
		this.btnAddAdvanceUtlize.Text = "+";
		this.btnAddAdvanceUtlize.UseVisualStyleBackColor = false;
		this.btnAddAdvanceUtlize.Click += new System.EventHandler(btnAddAdvanceUtlize_Click);
		this.btnUpdateAdvanceUtilize.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateAdvanceUtilize.FlatAppearance.BorderSize = 0;
		this.btnUpdateAdvanceUtilize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateAdvanceUtilize.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateAdvanceUtilize.ForeColor = System.Drawing.Color.White;
		this.btnUpdateAdvanceUtilize.Location = new System.Drawing.Point(892, 29);
		this.btnUpdateAdvanceUtilize.Name = "btnUpdateAdvanceUtilize";
		this.btnUpdateAdvanceUtilize.Size = new System.Drawing.Size(63, 26);
		this.btnUpdateAdvanceUtilize.TabIndex = 7;
		this.btnUpdateAdvanceUtilize.Text = "Update";
		this.btnUpdateAdvanceUtilize.UseVisualStyleBackColor = false;
		this.btnUpdateAdvanceUtilize.Visible = false;
		this.btnUpdateAdvanceUtilize.Click += new System.EventHandler(btnUpdateAdvanceUtilize_Click);
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label111.Location = new System.Drawing.Point(652, 10);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(48, 15);
		this.label111.TabIndex = 602;
		this.label111.Text = "Balance";
		this.btnCancelAdvanceUtilize.BackColor = System.Drawing.Color.Teal;
		this.btnCancelAdvanceUtilize.FlatAppearance.BorderSize = 0;
		this.btnCancelAdvanceUtilize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelAdvanceUtilize.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelAdvanceUtilize.ForeColor = System.Drawing.Color.White;
		this.btnCancelAdvanceUtilize.Location = new System.Drawing.Point(958, 29);
		this.btnCancelAdvanceUtilize.Name = "btnCancelAdvanceUtilize";
		this.btnCancelAdvanceUtilize.Size = new System.Drawing.Size(59, 26);
		this.btnCancelAdvanceUtilize.TabIndex = 8;
		this.btnCancelAdvanceUtilize.Text = "Cancel";
		this.btnCancelAdvanceUtilize.UseVisualStyleBackColor = false;
		this.btnCancelAdvanceUtilize.Visible = false;
		this.btnCancelAdvanceUtilize.Click += new System.EventHandler(btnCancelAdvanceUtilize_Click);
		this.txtFIBalanceAdvanceUtlize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFIBalanceAdvanceUtlize.Location = new System.Drawing.Point(651, 31);
		this.txtFIBalanceAdvanceUtlize.Name = "txtFIBalanceAdvanceUtlize";
		this.txtFIBalanceAdvanceUtlize.ReadOnly = true;
		this.txtFIBalanceAdvanceUtlize.Size = new System.Drawing.Size(101, 22);
		this.txtFIBalanceAdvanceUtlize.TabIndex = 598;
		this.txtFIBalanceAdvanceUtlize.TabStop = false;
		this.txtFIBalanceAdvanceUtlize.Text = "0";
		this.txtFIBalanceAdvanceUtlize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label112.Location = new System.Drawing.Point(754, 10);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(87, 15);
		this.label112.TabIndex = 603;
		this.label112.Text = "Utilize Amount";
		this.txtAdvanceUtlize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdvanceUtlize.Location = new System.Drawing.Point(753, 31);
		this.txtAdvanceUtlize.Name = "txtAdvanceUtlize";
		this.txtAdvanceUtlize.Size = new System.Drawing.Size(101, 22);
		this.txtAdvanceUtlize.TabIndex = 5;
		this.txtAdvanceUtlize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbGdNoAdvanceUtilize.AllowNull = (DefaultableBoolean)1;
		this.CmbGdNoAdvanceUtilize.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGdNoAdvanceUtilize.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGdNoAdvanceUtilize.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance388).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Appearance = (AppearanceBase)(object)appearance388;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance389).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance389).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance389).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance389).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance389;
		((AppearanceBase)appearance391).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance391;
		((SpecialBoxBase)((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance392).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance392).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance392).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance392).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance392;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance393).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance393).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance393;
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance394).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance394;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance395).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance395;
		((AppearanceBase)appearance396).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance396).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance396;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance397).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance397).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance397).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance397).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance397).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance397;
		((AppearanceBase)appearance398).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance398;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance399).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance399).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance399;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance400).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance400;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbGdNoAdvanceUtilize.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Location = new System.Drawing.Point(203, 29);
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Name = "CmbGdNoAdvanceUtilize";
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Size = new System.Drawing.Size(158, 26);
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).TextChanged += new System.EventHandler(CmbGdNoAdvanceUtilize_TextChanged);
		this.label115.AutoSize = true;
		this.label115.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label115.Location = new System.Drawing.Point(204, 10);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(43, 15);
		this.label115.TabIndex = 595;
		this.label115.Text = "GD No";
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.label113);
		this.panel29.Controls.Add(this.toolStrip5);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 3);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1142, 50);
		this.panel29.TabIndex = 8;
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label113.ForeColor = System.Drawing.Color.White;
		this.label113.Location = new System.Drawing.Point(3, 28);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(218, 18);
		this.label113.TabIndex = 28;
		this.label113.Text = "Advance Utilize Against GDs";
		this.toolStrip5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton11, this.toolStripButton12, this.toolStripButton13, this.toolStripButton14 });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1142, 27);
		this.toolStrip5.TabIndex = 606;
		this.toolStrip5.Text = "toolStrip5";
		this.toolStripButton11.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton11.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton11.Name = "toolStripButton11";
		this.toolStripButton11.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton11.Text = "&New";
		this.toolStripButton11.Click += new System.EventHandler(toolStripButton11_Click);
		this.toolStripButton12.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton12.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton12.Name = "toolStripButton12";
		this.toolStripButton12.Size = new System.Drawing.Size(64, 24);
		this.toolStripButton12.Text = "&Save";
		this.toolStripButton12.Click += new System.EventHandler(toolStripButton12_Click);
		this.toolStripButton13.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton13.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton13.Name = "toolStripButton13";
		this.toolStripButton13.Size = new System.Drawing.Size(78, 24);
		this.toolStripButton13.Text = "&Update";
		this.toolStripButton13.Visible = false;
		this.toolStripButton14.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton14.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton14.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton14.Name = "toolStripButton14";
		this.toolStripButton14.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton14.Text = "&Refresh";
		this.toolStripButton14.Click += new System.EventHandler(toolStripButton14_Click);
		this.tabPage8.Controls.Add((System.Windows.Forms.Control)(object)this.grdAdvanceHistory);
		this.tabPage8.Location = new System.Drawing.Point(4, 22);
		this.tabPage8.Name = "tabPage8";
		this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage8.Size = new System.Drawing.Size(1148, 326);
		this.tabPage8.TabIndex = 1;
		this.tabPage8.Text = "History";
		this.tabPage8.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdAdvanceHistory).BackColor = System.Drawing.Color.White;
		grdAdvanceHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAdvanceHistory.DesignTimeLayout = grdAdvanceHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAdvanceHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Name = "grdAdvanceHistory";
		((ScrollableBase)this.grdAdvanceHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAdvanceHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdAdvanceHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Size = new System.Drawing.Size(1142, 320);
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).TabIndex = 608;
		this.grdAdvanceHistory.TotalRow = (InheritableBoolean)1;
		this.grdAdvanceHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAdvanceHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).DoubleClick += new System.EventHandler(grdAdvanceHistory_DoubleClick);
		this.panel14.Controls.Add(this.panel31);
		this.panel14.Controls.Add(this.panel30);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 23);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1156, 318);
		this.panel14.TabIndex = 6;
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.grdGdBreakUp);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel31.Location = new System.Drawing.Point(0, 109);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1156, 209);
		this.panel31.TabIndex = 605;
		this.grdGdBreakUp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGdBreakUp).BackColor = System.Drawing.Color.White;
		grdGdBreakUp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGdBreakUp.DesignTimeLayout = grdGdBreakUp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGdBreakUp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Name = "grdGdBreakUp";
		((ScrollableBase)this.grdGdBreakUp).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdGdBreakUp).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdGdBreakUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Size = new System.Drawing.Size(1156, 209);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).TabIndex = 7;
		this.grdGdBreakUp.TotalRow = (InheritableBoolean)1;
		this.grdGdBreakUp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGdBreakUp).VisualStyle = (VisualStyle)4;
		this.grdGdBreakUp.ColumnButtonClick += new ColumnActionEventHandler(grdGdBreakUp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).DoubleClick += new System.EventHandler(grdGdBreakUp_DoubleClick);
		this.panel30.Controls.Add(this.txtRemarks);
		this.panel30.Controls.Add(this.label121);
		this.panel30.Controls.Add(this.txtFttAmount);
		this.panel30.Controls.Add(this.label120);
		this.panel30.Controls.Add(this.txtExchangeRate);
		this.panel30.Controls.Add(this.label119);
		this.panel30.Controls.Add(this.txtNetAmount);
		this.panel30.Controls.Add(this.label118);
		this.panel30.Controls.Add(this.txtFTTPrcnt);
		this.panel30.Controls.Add(this.label117);
		this.panel30.Controls.Add(this.txtFobValue);
		this.panel30.Controls.Add(this.label116);
		this.panel30.Controls.Add(this.txtFreight);
		this.panel30.Controls.Add(this.label110);
		this.panel30.Controls.Add(this.txtDueDays);
		this.panel30.Controls.Add(this.label106);
		this.panel30.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankName);
		this.panel30.Controls.Add(this.label105);
		this.panel30.Controls.Add(this.lblInvoiceBalanceGdBreakUp);
		this.panel30.Controls.Add(this.txtGDValue);
		this.panel30.Controls.Add(this.label107);
		this.panel30.Controls.Add(this.btnAddGDBreakUp);
		this.panel30.Controls.Add(this.btnUpdateGdBreakUp);
		this.panel30.Controls.Add(this.btnCancelGdBreakUp);
		this.panel30.Controls.Add(this.txtGDNoBreakUp);
		this.panel30.Controls.Add(this.label102);
		this.panel30.Controls.Add(this.txtInvoiceNoGDBreakUp);
		this.panel30.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp);
		this.panel30.Controls.Add(this.label100);
		this.panel30.Controls.Add(this.label98);
		this.panel30.Controls.Add(this.datGDDateGDBreakUp);
		this.panel30.Controls.Add(this.label101);
		this.panel30.Controls.Add(this.datInvoiceDateGdBreakUp);
		this.panel30.Controls.Add(this.label99);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 0);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1156, 109);
		this.panel30.TabIndex = 604;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(685, 75);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(325, 23);
		this.txtRemarks.TabIndex = 14;
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label121.Location = new System.Drawing.Point(687, 55);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(52, 15);
		this.label121.TabIndex = 614;
		this.label121.Text = "Remarks";
		this.txtFttAmount.Enabled = false;
		this.txtFttAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFttAmount.Location = new System.Drawing.Point(424, 75);
		this.txtFttAmount.Name = "txtFttAmount";
		this.txtFttAmount.Size = new System.Drawing.Size(127, 23);
		this.txtFttAmount.TabIndex = 12;
		this.txtFttAmount.Text = "0";
		this.txtFttAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFttAmount.TextChanged += new System.EventHandler(txtFttAmount_TextChanged);
		this.txtFttAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFttAmount_KeyPress);
		this.label120.AutoSize = true;
		this.label120.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label120.Location = new System.Drawing.Point(425, 55);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(74, 15);
		this.label120.TabIndex = 612;
		this.label120.Text = "FTT Amount";
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(1011, 27);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(123, 23);
		this.txtExchangeRate.TabIndex = 7;
		this.txtExchangeRate.Text = "0";
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.label119.AutoSize = true;
		this.label119.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label119.Location = new System.Drawing.Point(1012, 7);
		this.label119.Name = "label119";
		this.label119.Size = new System.Drawing.Size(84, 15);
		this.label119.TabIndex = 610;
		this.label119.Text = "Exchange Rate";
		this.txtNetAmount.Enabled = false;
		this.txtNetAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmount.Location = new System.Drawing.Point(552, 75);
		this.txtNetAmount.Name = "txtNetAmount";
		this.txtNetAmount.Size = new System.Drawing.Size(132, 23);
		this.txtNetAmount.TabIndex = 13;
		this.txtNetAmount.Text = "0";
		this.txtNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetAmount_KeyPress);
		this.label118.AutoSize = true;
		this.label118.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label118.Location = new System.Drawing.Point(553, 55);
		this.label118.Name = "label118";
		this.label118.Size = new System.Drawing.Size(73, 15);
		this.label118.TabIndex = 608;
		this.label118.Text = "Net Amount";
		this.txtFTTPrcnt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFTTPrcnt.Location = new System.Drawing.Point(353, 75);
		this.txtFTTPrcnt.Name = "txtFTTPrcnt";
		this.txtFTTPrcnt.Size = new System.Drawing.Size(70, 23);
		this.txtFTTPrcnt.TabIndex = 11;
		this.txtFTTPrcnt.Text = "10";
		this.txtFTTPrcnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFTTPrcnt.TextChanged += new System.EventHandler(txtFTTPrcnt_TextChanged);
		this.txtFTTPrcnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFTTPrcnt_KeyPress);
		this.label117.AutoSize = true;
		this.label117.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label117.Location = new System.Drawing.Point(354, 55);
		this.label117.Name = "label117";
		this.label117.Size = new System.Drawing.Size(40, 15);
		this.label117.TabIndex = 606;
		this.label117.Text = "% FTT";
		this.txtFobValue.Enabled = false;
		this.txtFobValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFobValue.Location = new System.Drawing.Point(237, 75);
		this.txtFobValue.Name = "txtFobValue";
		this.txtFobValue.Size = new System.Drawing.Size(115, 23);
		this.txtFobValue.TabIndex = 10;
		this.txtFobValue.Text = "0";
		this.txtFobValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFobValue.TextChanged += new System.EventHandler(txtFobValue_TextChanged);
		this.txtFobValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFobValue_KeyPress);
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label116.Location = new System.Drawing.Point(238, 55);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(61, 15);
		this.label116.TabIndex = 604;
		this.label116.Text = "FOB Value";
		this.txtFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreight.Location = new System.Drawing.Point(121, 75);
		this.txtFreight.Name = "txtFreight";
		this.txtFreight.Size = new System.Drawing.Size(115, 23);
		this.txtFreight.TabIndex = 9;
		this.txtFreight.Text = "0";
		this.txtFreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreight.TextChanged += new System.EventHandler(txtFreight_TextChanged);
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label110.Location = new System.Drawing.Point(122, 55);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(44, 15);
		this.label110.TabIndex = 602;
		this.label110.Text = "Freight";
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(950, 27);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(60, 23);
		this.txtDueDays.TabIndex = 6;
		this.txtDueDays.Text = "0";
		this.txtDueDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label106.AutoSize = true;
		this.label106.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label106.Location = new System.Drawing.Point(949, 7);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(58, 15);
		this.label106.TabIndex = 600;
		this.label106.Text = "Due Days";
		this.CmbBankName.AllowNull = (DefaultableBoolean)1;
		this.CmbBankName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance492).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance492).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance492;
		((UltraGridBase)this.CmbBankName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance493).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance493).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance493).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance493).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance493;
		((AppearanceBase)appearance494).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance494;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance495).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance495).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance495).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance495).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance495;
		((UltraGridBase)this.CmbBankName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance496).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance496).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance496;
		((AppearanceBase)appearance497).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance497).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance497;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance498).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance498;
		((AppearanceBase)appearance499).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance499).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance499;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance500).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance500).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance500).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance500).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance500).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance500;
		((AppearanceBase)appearance502).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance502;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance503).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance503).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance503;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance504).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance504;
		((UltraGridBase)this.CmbBankName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankName).Location = new System.Drawing.Point(749, 25);
		((System.Windows.Forms.Control)(object)this.CmbBankName).Name = "CmbBankName";
		((System.Windows.Forms.Control)(object)this.CmbBankName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankName).TabIndex = 5;
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label105.Location = new System.Drawing.Point(749, 5);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(68, 15);
		this.label105.TabIndex = 598;
		this.label105.Text = "Bank Name";
		this.lblInvoiceBalanceGdBreakUp.AutoSize = true;
		this.lblInvoiceBalanceGdBreakUp.Location = new System.Drawing.Point(132, 8);
		this.lblInvoiceBalanceGdBreakUp.Name = "lblInvoiceBalanceGdBreakUp";
		this.lblInvoiceBalanceGdBreakUp.Size = new System.Drawing.Size(17, 13);
		this.lblInvoiceBalanceGdBreakUp.TabIndex = 596;
		this.lblInvoiceBalanceGdBreakUp.Text = "lbl";
		this.lblInvoiceBalanceGdBreakUp.Visible = false;
		this.txtGDValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGDValue.Location = new System.Drawing.Point(5, 75);
		this.txtGDValue.Name = "txtGDValue";
		this.txtGDValue.Size = new System.Drawing.Size(115, 23);
		this.txtGDValue.TabIndex = 8;
		this.txtGDValue.Text = "0";
		this.txtGDValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGDValue.TextChanged += new System.EventHandler(txtGDValue_TextChanged);
		this.txtGDValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGDValue_KeyPress_1);
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label107.Location = new System.Drawing.Point(6, 55);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(56, 15);
		this.label107.TabIndex = 513;
		this.label107.Text = "GD Value";
		this.btnAddGDBreakUp.BackColor = System.Drawing.Color.Teal;
		this.btnAddGDBreakUp.FlatAppearance.BorderSize = 0;
		this.btnAddGDBreakUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddGDBreakUp.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddGDBreakUp.ForeColor = System.Drawing.Color.White;
		this.btnAddGDBreakUp.Location = new System.Drawing.Point(1011, 73);
		this.btnAddGDBreakUp.Name = "btnAddGDBreakUp";
		this.btnAddGDBreakUp.Size = new System.Drawing.Size(31, 26);
		this.btnAddGDBreakUp.TabIndex = 15;
		this.btnAddGDBreakUp.Text = "+";
		this.btnAddGDBreakUp.UseVisualStyleBackColor = false;
		this.btnAddGDBreakUp.Click += new System.EventHandler(button3_Click);
		this.btnUpdateGdBreakUp.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateGdBreakUp.FlatAppearance.BorderSize = 0;
		this.btnUpdateGdBreakUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateGdBreakUp.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateGdBreakUp.ForeColor = System.Drawing.Color.White;
		this.btnUpdateGdBreakUp.Location = new System.Drawing.Point(1011, 73);
		this.btnUpdateGdBreakUp.Name = "btnUpdateGdBreakUp";
		this.btnUpdateGdBreakUp.Size = new System.Drawing.Size(63, 26);
		this.btnUpdateGdBreakUp.TabIndex = 16;
		this.btnUpdateGdBreakUp.Text = "Update";
		this.btnUpdateGdBreakUp.UseVisualStyleBackColor = false;
		this.btnUpdateGdBreakUp.Visible = false;
		this.btnUpdateGdBreakUp.Click += new System.EventHandler(btnUpdateGdBreakUp_Click);
		this.btnCancelGdBreakUp.BackColor = System.Drawing.Color.Teal;
		this.btnCancelGdBreakUp.FlatAppearance.BorderSize = 0;
		this.btnCancelGdBreakUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelGdBreakUp.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelGdBreakUp.ForeColor = System.Drawing.Color.White;
		this.btnCancelGdBreakUp.Location = new System.Drawing.Point(1075, 73);
		this.btnCancelGdBreakUp.Name = "btnCancelGdBreakUp";
		this.btnCancelGdBreakUp.Size = new System.Drawing.Size(59, 26);
		this.btnCancelGdBreakUp.TabIndex = 17;
		this.btnCancelGdBreakUp.Text = "Cancel";
		this.btnCancelGdBreakUp.UseVisualStyleBackColor = false;
		this.btnCancelGdBreakUp.Visible = false;
		this.btnCancelGdBreakUp.Click += new System.EventHandler(btnCancelGdBreakUp_Click);
		this.txtGDNoBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGDNoBreakUp.Location = new System.Drawing.Point(237, 27);
		this.txtGDNoBreakUp.Name = "txtGDNoBreakUp";
		this.txtGDNoBreakUp.Size = new System.Drawing.Size(115, 23);
		this.txtGDNoBreakUp.TabIndex = 1;
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.Location = new System.Drawing.Point(239, 7);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(43, 15);
		this.label102.TabIndex = 509;
		this.label102.Text = "GD No";
		this.txtInvoiceNoGDBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNoGDBreakUp.Location = new System.Drawing.Point(480, 27);
		this.txtInvoiceNoGDBreakUp.Name = "txtInvoiceNoGDBreakUp";
		this.txtInvoiceNoGDBreakUp.Size = new System.Drawing.Size(139, 23);
		this.txtInvoiceNoGDBreakUp.TabIndex = 3;
		this.CmbInvoiceGdBreakUp.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceGdBreakUp.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceGdBreakUp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceGdBreakUp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance505).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance505).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance505;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance506).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance506).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance506).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance506).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance506;
		((AppearanceBase)appearance507).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance507;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance508).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance508).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance508).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance508).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance508;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance509).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance509).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance509;
		((AppearanceBase)appearance510).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance510).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance510;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance511).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance511;
		((AppearanceBase)appearance513).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance513).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance513;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance514).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance514).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance514).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance514).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance514).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance514;
		((AppearanceBase)appearance515).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance515;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance516).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance516).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance516;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance517).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance517;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceGdBreakUp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).Location = new System.Drawing.Point(5, 25);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).Name = "CmbInvoiceGdBreakUp";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).TextChanged += new System.EventHandler(CmbInvoiceGdBreakUp_TextChanged);
		this.label100.AutoSize = true;
		this.label100.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.Location = new System.Drawing.Point(482, 7);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(94, 15);
		this.label100.TabIndex = 509;
		this.label100.Text = "Bank Invoice No";
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label98.Location = new System.Drawing.Point(5, 7);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(65, 15);
		this.label98.TabIndex = 595;
		this.label98.Text = "Invoice No";
		this.datGDDateGDBreakUp.CustomFormat = "dd-MMM-yy";
		this.datGDDateGDBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datGDDateGDBreakUp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datGDDateGDBreakUp.Location = new System.Drawing.Point(353, 27);
		this.datGDDateGDBreakUp.Name = "datGDDateGDBreakUp";
		this.datGDDateGDBreakUp.Size = new System.Drawing.Size(125, 23);
		this.datGDDateGDBreakUp.TabIndex = 2;
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label101.Location = new System.Drawing.Point(618, 7);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(103, 15);
		this.label101.TabIndex = 593;
		this.label101.Text = "Bank Invoice Date";
		this.datInvoiceDateGdBreakUp.CustomFormat = "dd-MMM-yy";
		this.datInvoiceDateGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datInvoiceDateGdBreakUp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datInvoiceDateGdBreakUp.Location = new System.Drawing.Point(622, 27);
		this.datInvoiceDateGdBreakUp.Name = "datInvoiceDateGdBreakUp";
		this.datInvoiceDateGdBreakUp.Size = new System.Drawing.Size(125, 23);
		this.datInvoiceDateGdBreakUp.TabIndex = 4;
		this.label99.AutoSize = true;
		this.label99.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.Location = new System.Drawing.Point(349, 7);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(52, 15);
		this.label99.TabIndex = 593;
		this.label99.Text = "GD Date";
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.label114);
		this.panel25.Controls.Add(this.ctrlGrdBar5);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1156, 23);
		this.panel25.TabIndex = 5;
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label114.ForeColor = System.Drawing.Color.White;
		this.label114.Location = new System.Drawing.Point(3, 2);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(98, 18);
		this.label114.TabIndex = 29;
		this.label114.Text = "GD BreakUp";
		this.panel27.Controls.Add(this.toolStrip1);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(0, 0);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1156, 30);
		this.panel27.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripButton6, this.toolStripButton8, this.toolStripButton9, this.toolStripButton10, this.btnGdBreakupManual });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1156, 27);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton6.Text = "&New";
		this.toolStripButton6.Click += new System.EventHandler(toolStripButton6_Click);
		this.toolStripButton8.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton8.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton8.Name = "toolStripButton8";
		this.toolStripButton8.Size = new System.Drawing.Size(64, 24);
		this.toolStripButton8.Text = "&Save";
		this.toolStripButton8.Click += new System.EventHandler(toolStripButton8_Click);
		this.toolStripButton9.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton9.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton9.Name = "toolStripButton9";
		this.toolStripButton9.Size = new System.Drawing.Size(78, 24);
		this.toolStripButton9.Text = "&Update";
		this.toolStripButton9.Visible = false;
		this.toolStripButton10.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton10.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton10.Name = "toolStripButton10";
		this.toolStripButton10.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton10.Text = "&Refresh";
		this.toolStripButton10.Click += new System.EventHandler(toolStripButton10_Click);
		this.btnGdBreakupManual.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGdBreakupManual.Name = "btnGdBreakupManual";
		this.btnGdBreakupManual.Size = new System.Drawing.Size(130, 24);
		this.btnGdBreakupManual.Text = "GD BreakUp Manual";
		this.btnGdBreakupManual.Click += new System.EventHandler(btnGdBreakupManual_Click);
		this.GDHistory.Controls.Add(this.panel43);
		this.GDHistory.Controls.Add(this.panel42);
		this.GDHistory.Controls.Add(this.toolStrip7);
		this.GDHistory.Location = new System.Drawing.Point(4, 4);
		this.GDHistory.Name = "GDHistory";
		this.GDHistory.Size = new System.Drawing.Size(1156, 723);
		this.GDHistory.TabIndex = 3;
		this.GDHistory.Text = "GD History";
		this.GDHistory.UseVisualStyleBackColor = true;
		this.panel43.Controls.Add(this.panel44);
		this.panel43.Controls.Add(this.panel34);
		this.panel43.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel43.Location = new System.Drawing.Point(0, 99);
		this.panel43.Name = "panel43";
		this.panel43.Size = new System.Drawing.Size(1156, 624);
		this.panel43.TabIndex = 49;
		this.panel44.Controls.Add((System.Windows.Forms.Control)(object)this.grdGDhistory);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel44.Location = new System.Drawing.Point(0, 28);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(1156, 596);
		this.panel44.TabIndex = 47;
		this.grdGDhistory.AllowEdit = (InheritableBoolean)2;
		grdGDhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGDhistory.DesignTimeLayout = grdGDhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGDhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGDhistory.DynamicFiltering = true;
		this.grdGDhistory.FilterMode = (FilterMode)2;
		this.grdGDhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGDhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGDhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdGDhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGDhistory).Name = "grdGDhistory";
		((ScrollableBase)this.grdGDhistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdGDhistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.grdGDhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGDhistory).Size = new System.Drawing.Size(1156, 596);
		((System.Windows.Forms.Control)(object)this.grdGDhistory).TabIndex = 46;
		this.grdGDhistory.TotalRow = (InheritableBoolean)1;
		this.grdGDhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGDhistory).VisualStyle = (VisualStyle)4;
		this.grdGDhistory.ColumnButtonClick += new ColumnActionEventHandler(grdGDhistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGDhistory).DoubleClick += new System.EventHandler(grdGDhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdGDhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGDhistory_KeyDown);
		this.panel34.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel34.Controls.Add(this.ctrlGrdBar7);
		this.panel34.Controls.Add(this.label104);
		this.panel34.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel34.Location = new System.Drawing.Point(0, 0);
		this.panel34.Name = "panel34";
		this.panel34.Size = new System.Drawing.Size(1156, 28);
		this.panel34.TabIndex = 45;
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.ForeColor = System.Drawing.Color.White;
		this.label104.Location = new System.Drawing.Point(7, 3);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(96, 19);
		this.label104.TabIndex = 1;
		this.label104.Text = "GD History";
		this.panel42.Controls.Add(this.groupBox6);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 27);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(1156, 72);
		this.panel42.TabIndex = 48;
		this.groupBox6.Controls.Add(this.btnShowGdBreakUpHistory);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory);
		this.groupBox6.Controls.Add(this.label143);
		this.groupBox6.Controls.Add(this.ToDateGdBreakupHistory);
		this.groupBox6.Controls.Add(this.label144);
		this.groupBox6.Controls.Add(this.FromDateGdBreakupHistory);
		this.groupBox6.Controls.Add(this.label147);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(6, 2);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(576, 67);
		this.groupBox6.TabIndex = 4;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Filters";
		this.btnShowGdBreakUpHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowGdBreakUpHistory.FlatAppearance.BorderSize = 0;
		this.btnShowGdBreakUpHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowGdBreakUpHistory.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnShowGdBreakUpHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowGdBreakUpHistory.Location = new System.Drawing.Point(515, 36);
		this.btnShowGdBreakUpHistory.Name = "btnShowGdBreakUpHistory";
		this.btnShowGdBreakUpHistory.Size = new System.Drawing.Size(57, 24);
		this.btnShowGdBreakUpHistory.TabIndex = 6;
		this.btnShowGdBreakUpHistory.Text = "Show";
		this.btnShowGdBreakUpHistory.UseVisualStyleBackColor = false;
		this.btnShowGdBreakUpHistory.Click += new System.EventHandler(btnShowGdBreakUpHistory_Click);
		this.CmbBankGdBankHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBankGdBankHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankGdBankHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankGdBankHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance334).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance334).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance334).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance445).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance445).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance445;
		((AppearanceBase)appearance556).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance556).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance556;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance580).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance580;
		((AppearanceBase)appearance591).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance591).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance591;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance602).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance602).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance602).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance602).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance602).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance602;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankGdBankHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).Location = new System.Drawing.Point(236, 35);
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).Name = "CmbBankGdBankHistory";
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).Size = new System.Drawing.Size(277, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).TabIndex = 4;
		this.label143.AutoSize = true;
		this.label143.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label143.Location = new System.Drawing.Point(238, 18);
		this.label143.Name = "label143";
		this.label143.Size = new System.Drawing.Size(68, 15);
		this.label143.TabIndex = 12;
		this.label143.Text = "Bank Name";
		this.ToDateGdBreakupHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateGdBreakupHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateGdBreakupHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateGdBreakupHistory.Location = new System.Drawing.Point(119, 37);
		this.ToDateGdBreakupHistory.Name = "ToDateGdBreakupHistory";
		this.ToDateGdBreakupHistory.Size = new System.Drawing.Size(114, 23);
		this.ToDateGdBreakupHistory.TabIndex = 1;
		this.label144.AutoSize = true;
		this.label144.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label144.Location = new System.Drawing.Point(119, 18);
		this.label144.Name = "label144";
		this.label144.Size = new System.Drawing.Size(48, 15);
		this.label144.TabIndex = 7;
		this.label144.Text = "To Date";
		this.FromDateGdBreakupHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateGdBreakupHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateGdBreakupHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateGdBreakupHistory.Location = new System.Drawing.Point(6, 37);
		this.FromDateGdBreakupHistory.Name = "FromDateGdBreakupHistory";
		this.FromDateGdBreakupHistory.Size = new System.Drawing.Size(112, 23);
		this.FromDateGdBreakupHistory.TabIndex = 0;
		this.label147.AutoSize = true;
		this.label147.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label147.Location = new System.Drawing.Point(6, 18);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(63, 15);
		this.label147.TabIndex = 5;
		this.label147.Text = "From Date";
		this.toolStrip7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip7.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetGdBreakupHistory, this.btnRefreshGdBreakupHistory });
		this.toolStrip7.Location = new System.Drawing.Point(0, 0);
		this.toolStrip7.Name = "toolStrip7";
		this.toolStrip7.Size = new System.Drawing.Size(1156, 27);
		this.toolStrip7.TabIndex = 47;
		this.toolStrip7.Text = "toolStrip7";
		this.btnResetGdBreakupHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetGdBreakupHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetGdBreakupHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetGdBreakupHistory.Name = "btnResetGdBreakupHistory";
		this.btnResetGdBreakupHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetGdBreakupHistory.Text = "&New";
		this.btnResetGdBreakupHistory.Click += new System.EventHandler(btnResetGdBreakupHistory_Click);
		this.btnRefreshGdBreakupHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshGdBreakupHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshGdBreakupHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshGdBreakupHistory.Name = "btnRefreshGdBreakupHistory";
		this.btnRefreshGdBreakupHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshGdBreakupHistory.Text = "&Refresh";
		this.btnRefreshGdBreakupHistory.Click += new System.EventHandler(btnRefreshGdBreakupHistory_Click);
		this.InvoicesAllocationToGd.Controls.Add(this.panel39);
		this.InvoicesAllocationToGd.Controls.Add(this.panel37);
		this.InvoicesAllocationToGd.Controls.Add(this.panel35);
		this.InvoicesAllocationToGd.Controls.Add(this.toolStrip6);
		this.InvoicesAllocationToGd.Location = new System.Drawing.Point(4, 4);
		this.InvoicesAllocationToGd.Name = "InvoicesAllocationToGd";
		this.InvoicesAllocationToGd.Size = new System.Drawing.Size(1156, 723);
		this.InvoicesAllocationToGd.TabIndex = 4;
		this.InvoicesAllocationToGd.Text = "Invoices Allocation To GD";
		this.InvoicesAllocationToGd.UseVisualStyleBackColor = true;
		this.panel39.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm);
		this.panel39.Controls.Add(this.panel41);
		this.panel39.Controls.Add(this.panel40);
		this.panel39.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel39.Location = new System.Drawing.Point(0, 423);
		this.panel39.Name = "panel39";
		this.panel39.Size = new System.Drawing.Size(1156, 300);
		this.panel39.TabIndex = 9;
		this.DataGridHistoryMultiInvoiceToGdForm.AllowEdit = (InheritableBoolean)2;
		DataGridHistoryMultiInvoiceToGdForm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryMultiInvoiceToGdForm.DesignTimeLayout = DataGridHistoryMultiInvoiceToGdForm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistoryMultiInvoiceToGdForm.DynamicFiltering = true;
		this.DataGridHistoryMultiInvoiceToGdForm.FilterMode = (FilterMode)2;
		this.DataGridHistoryMultiInvoiceToGdForm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryMultiInvoiceToGdForm.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm).Location = new System.Drawing.Point(0, 99);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm).Name = "DataGridHistoryMultiInvoiceToGdForm";
		((ScrollableBase)this.DataGridHistoryMultiInvoiceToGdForm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistoryMultiInvoiceToGdForm).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistoryMultiInvoiceToGdForm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm).Size = new System.Drawing.Size(1156, 201);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm).TabIndex = 47;
		this.DataGridHistoryMultiInvoiceToGdForm.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryMultiInvoiceToGdForm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryMultiInvoiceToGdForm).VisualStyle = (VisualStyle)4;
		this.DataGridHistoryMultiInvoiceToGdForm.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistoryMultiInvoiceToGdForm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm).DoubleClick += new System.EventHandler(DataGridHistoryMultiInvoiceToGdForm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryMultiInvoiceToGdForm).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistoryMultiInvoiceToGdForm_KeyDown);
		this.panel41.Controls.Add(this.groupBox5);
		this.panel41.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel41.Location = new System.Drawing.Point(0, 27);
		this.panel41.Name = "panel41";
		this.panel41.Size = new System.Drawing.Size(1156, 72);
		this.panel41.TabIndex = 1;
		this.groupBox5.Controls.Add(this.btnRefreshMultiInvoiceToGdHistory);
		this.groupBox5.Controls.Add(this.btnResetMultiInvoiceToGdHistory);
		this.groupBox5.Controls.Add(this.btnShowMultiInvoicesToGdForm);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory);
		this.groupBox5.Controls.Add(this.label142);
		this.groupBox5.Controls.Add(this.ToDateMultiInvoiceAgainstGd);
		this.groupBox5.Controls.Add(this.label145);
		this.groupBox5.Controls.Add(this.FromDateMultiInvoiceAgainstGd);
		this.groupBox5.Controls.Add(this.label146);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(6, 2);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(699, 67);
		this.groupBox5.TabIndex = 4;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.btnRefreshMultiInvoiceToGdHistory.BackColor = System.Drawing.Color.Teal;
		this.btnRefreshMultiInvoiceToGdHistory.FlatAppearance.BorderSize = 0;
		this.btnRefreshMultiInvoiceToGdHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRefreshMultiInvoiceToGdHistory.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnRefreshMultiInvoiceToGdHistory.ForeColor = System.Drawing.Color.White;
		this.btnRefreshMultiInvoiceToGdHistory.Location = new System.Drawing.Point(607, 36);
		this.btnRefreshMultiInvoiceToGdHistory.Name = "btnRefreshMultiInvoiceToGdHistory";
		this.btnRefreshMultiInvoiceToGdHistory.Size = new System.Drawing.Size(69, 24);
		this.btnRefreshMultiInvoiceToGdHistory.TabIndex = 14;
		this.btnRefreshMultiInvoiceToGdHistory.Text = "Refresh";
		this.btnRefreshMultiInvoiceToGdHistory.UseVisualStyleBackColor = false;
		this.btnRefreshMultiInvoiceToGdHistory.Click += new System.EventHandler(btnRefreshMultiInvoiceToGdHistory_Click);
		this.btnResetMultiInvoiceToGdHistory.BackColor = System.Drawing.Color.Teal;
		this.btnResetMultiInvoiceToGdHistory.FlatAppearance.BorderSize = 0;
		this.btnResetMultiInvoiceToGdHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetMultiInvoiceToGdHistory.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnResetMultiInvoiceToGdHistory.ForeColor = System.Drawing.Color.White;
		this.btnResetMultiInvoiceToGdHistory.Location = new System.Drawing.Point(548, 36);
		this.btnResetMultiInvoiceToGdHistory.Name = "btnResetMultiInvoiceToGdHistory";
		this.btnResetMultiInvoiceToGdHistory.Size = new System.Drawing.Size(57, 24);
		this.btnResetMultiInvoiceToGdHistory.TabIndex = 13;
		this.btnResetMultiInvoiceToGdHistory.Text = "Reset";
		this.btnResetMultiInvoiceToGdHistory.UseVisualStyleBackColor = false;
		this.btnResetMultiInvoiceToGdHistory.Click += new System.EventHandler(btnResetMultiInvoiceToGdHistory_Click);
		this.btnShowMultiInvoicesToGdForm.BackColor = System.Drawing.Color.Teal;
		this.btnShowMultiInvoicesToGdForm.FlatAppearance.BorderSize = 0;
		this.btnShowMultiInvoicesToGdForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowMultiInvoicesToGdForm.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnShowMultiInvoicesToGdForm.ForeColor = System.Drawing.Color.White;
		this.btnShowMultiInvoicesToGdForm.Location = new System.Drawing.Point(489, 36);
		this.btnShowMultiInvoicesToGdForm.Name = "btnShowMultiInvoicesToGdForm";
		this.btnShowMultiInvoicesToGdForm.Size = new System.Drawing.Size(57, 24);
		this.btnShowMultiInvoicesToGdForm.TabIndex = 6;
		this.btnShowMultiInvoicesToGdForm.Text = "Show";
		this.btnShowMultiInvoicesToGdForm.UseVisualStyleBackColor = false;
		this.btnShowMultiInvoicesToGdForm.Click += new System.EventHandler(btnShowMultiInvoicesToGdForm_Click);
		this.CmbBankMultiInvoiceAgainstGdHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBankMultiInvoiceAgainstGdHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankMultiInvoiceAgainstGdHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankMultiInvoiceAgainstGdHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance531).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance531).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance531;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance532).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance532).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance532).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance532).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance532;
		((AppearanceBase)appearance533).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance533;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance535).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance535).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance535).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance535).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance535;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance536).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance536).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance536;
		((AppearanceBase)appearance537).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance537).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance537;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance538).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance538;
		((AppearanceBase)appearance539).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance539).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance539;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance540).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance540).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance540).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance540).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance540).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance540;
		((AppearanceBase)appearance541).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance541;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance542).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance542).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance542;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance543).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance543;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankMultiInvoiceAgainstGdHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).Location = new System.Drawing.Point(236, 35);
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).Name = "CmbBankMultiInvoiceAgainstGdHistory";
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).TabIndex = 4;
		this.label142.AutoSize = true;
		this.label142.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label142.Location = new System.Drawing.Point(238, 18);
		this.label142.Name = "label142";
		this.label142.Size = new System.Drawing.Size(68, 15);
		this.label142.TabIndex = 12;
		this.label142.Text = "Bank Name";
		this.ToDateMultiInvoiceAgainstGd.CustomFormat = "dd-MMM-yy";
		this.ToDateMultiInvoiceAgainstGd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateMultiInvoiceAgainstGd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateMultiInvoiceAgainstGd.Location = new System.Drawing.Point(119, 37);
		this.ToDateMultiInvoiceAgainstGd.Name = "ToDateMultiInvoiceAgainstGd";
		this.ToDateMultiInvoiceAgainstGd.Size = new System.Drawing.Size(114, 23);
		this.ToDateMultiInvoiceAgainstGd.TabIndex = 1;
		this.label145.AutoSize = true;
		this.label145.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label145.Location = new System.Drawing.Point(119, 18);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(48, 15);
		this.label145.TabIndex = 7;
		this.label145.Text = "To Date";
		this.FromDateMultiInvoiceAgainstGd.CustomFormat = "dd-MMM-yy";
		this.FromDateMultiInvoiceAgainstGd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateMultiInvoiceAgainstGd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateMultiInvoiceAgainstGd.Location = new System.Drawing.Point(6, 37);
		this.FromDateMultiInvoiceAgainstGd.Name = "FromDateMultiInvoiceAgainstGd";
		this.FromDateMultiInvoiceAgainstGd.Size = new System.Drawing.Size(112, 23);
		this.FromDateMultiInvoiceAgainstGd.TabIndex = 0;
		this.label146.AutoSize = true;
		this.label146.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label146.Location = new System.Drawing.Point(6, 18);
		this.label146.Name = "label146";
		this.label146.Size = new System.Drawing.Size(63, 15);
		this.label146.TabIndex = 5;
		this.label146.Text = "From Date";
		this.panel40.BackColor = System.Drawing.Color.Teal;
		this.panel40.Controls.Add(this.ctrlGrdBar9);
		this.panel40.Controls.Add(this.label141);
		this.panel40.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel40.Location = new System.Drawing.Point(0, 0);
		this.panel40.Name = "panel40";
		this.panel40.Size = new System.Drawing.Size(1156, 27);
		this.panel40.TabIndex = 0;
		this.label141.AutoSize = true;
		this.label141.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label141.ForeColor = System.Drawing.Color.White;
		this.label141.Location = new System.Drawing.Point(7, 4);
		this.label141.Name = "label141";
		this.label141.Size = new System.Drawing.Size(63, 18);
		this.label141.TabIndex = 31;
		this.label141.Text = "History";
		this.panel37.Controls.Add((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd);
		this.panel37.Controls.Add(this.panel38);
		this.panel37.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel37.Location = new System.Drawing.Point(0, 257);
		this.panel37.Name = "panel37";
		this.panel37.Size = new System.Drawing.Size(1156, 166);
		this.panel37.TabIndex = 8;
		this.grdMultiInvoiceAgainstGd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdMultiInvoiceAgainstGd).BackColor = System.Drawing.Color.White;
		grdMultiInvoiceAgainstGd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMultiInvoiceAgainstGd.DesignTimeLayout = grdMultiInvoiceAgainstGd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMultiInvoiceAgainstGd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd).Name = "grdMultiInvoiceAgainstGd";
		((ScrollableBase)this.grdMultiInvoiceAgainstGd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdMultiInvoiceAgainstGd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdMultiInvoiceAgainstGd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd).Size = new System.Drawing.Size(1156, 141);
		((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd).TabIndex = 8;
		this.grdMultiInvoiceAgainstGd.TotalRow = (InheritableBoolean)1;
		this.grdMultiInvoiceAgainstGd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMultiInvoiceAgainstGd).VisualStyle = (VisualStyle)4;
		this.grdMultiInvoiceAgainstGd.ColumnButtonClick += new ColumnActionEventHandler(grdMultiInvoiceAgainstGd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd).DoubleClick += new System.EventHandler(grdMultiInvoiceAgainstGd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdMultiInvoiceAgainstGd).KeyDown += new System.Windows.Forms.KeyEventHandler(grdMultiInvoiceAgainstGd_KeyDown);
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.label140);
		this.panel38.Controls.Add(this.ctrlGrdBar8);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1156, 25);
		this.panel38.TabIndex = 0;
		this.label140.AutoSize = true;
		this.label140.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label140.ForeColor = System.Drawing.Color.White;
		this.label140.Location = new System.Drawing.Point(8, 3);
		this.label140.Name = "label140";
		this.label140.Size = new System.Drawing.Size(111, 18);
		this.label140.TabIndex = 30;
		this.label140.Text = "Invoice's Info";
		this.panel35.Controls.Add(this.groupBox4);
		this.panel35.Controls.Add(this.groupBox3);
		this.panel35.Controls.Add(this.panel36);
		this.panel35.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel35.Location = new System.Drawing.Point(0, 27);
		this.panel35.Name = "panel35";
		this.panel35.Size = new System.Drawing.Size(1156, 230);
		this.panel35.TabIndex = 7;
		this.groupBox4.Controls.Add(this.label139);
		this.groupBox4.Controls.Add(this.txtDetailRemarksMultiInvoiceToGdForm);
		this.groupBox4.Controls.Add(this.btnAddMultiInvoiceForm);
		this.groupBox4.Controls.Add(this.btnUpdateDetailMultiInvoiceForm);
		this.groupBox4.Controls.Add(this.btnCancelDetailMultiInvoiceForm);
		this.groupBox4.Controls.Add(this.txtInvoiceAmountMultiInvoiceForm);
		this.groupBox4.Controls.Add(this.label138);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoMultiInvoiceForm);
		this.groupBox4.Controls.Add(this.label137);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(8, 158);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1003, 67);
		this.groupBox4.TabIndex = 8;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Detail Information";
		this.label139.AutoSize = true;
		this.label139.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label139.Location = new System.Drawing.Point(377, 17);
		this.label139.Name = "label139";
		this.label139.Size = new System.Drawing.Size(52, 15);
		this.label139.TabIndex = 639;
		this.label139.Text = "Remarks";
		this.txtDetailRemarksMultiInvoiceToGdForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDetailRemarksMultiInvoiceToGdForm.Location = new System.Drawing.Point(378, 37);
		this.txtDetailRemarksMultiInvoiceToGdForm.Name = "txtDetailRemarksMultiInvoiceToGdForm";
		this.txtDetailRemarksMultiInvoiceToGdForm.Size = new System.Drawing.Size(492, 23);
		this.txtDetailRemarksMultiInvoiceToGdForm.TabIndex = 2;
		this.btnAddMultiInvoiceForm.BackColor = System.Drawing.Color.Teal;
		this.btnAddMultiInvoiceForm.FlatAppearance.BorderSize = 0;
		this.btnAddMultiInvoiceForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddMultiInvoiceForm.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddMultiInvoiceForm.ForeColor = System.Drawing.Color.White;
		this.btnAddMultiInvoiceForm.Location = new System.Drawing.Point(873, 35);
		this.btnAddMultiInvoiceForm.Name = "btnAddMultiInvoiceForm";
		this.btnAddMultiInvoiceForm.Size = new System.Drawing.Size(31, 26);
		this.btnAddMultiInvoiceForm.TabIndex = 3;
		this.btnAddMultiInvoiceForm.Text = "+";
		this.btnAddMultiInvoiceForm.UseVisualStyleBackColor = false;
		this.btnAddMultiInvoiceForm.Click += new System.EventHandler(btnAddMultiInvoiceForm_Click);
		this.btnUpdateDetailMultiInvoiceForm.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetailMultiInvoiceForm.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetailMultiInvoiceForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetailMultiInvoiceForm.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetailMultiInvoiceForm.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetailMultiInvoiceForm.Location = new System.Drawing.Point(873, 35);
		this.btnUpdateDetailMultiInvoiceForm.Name = "btnUpdateDetailMultiInvoiceForm";
		this.btnUpdateDetailMultiInvoiceForm.Size = new System.Drawing.Size(63, 26);
		this.btnUpdateDetailMultiInvoiceForm.TabIndex = 4;
		this.btnUpdateDetailMultiInvoiceForm.Text = "Update";
		this.btnUpdateDetailMultiInvoiceForm.UseVisualStyleBackColor = false;
		this.btnUpdateDetailMultiInvoiceForm.Visible = false;
		this.btnUpdateDetailMultiInvoiceForm.Click += new System.EventHandler(btnUpdateDetailMultiInvoiceForm_Click);
		this.btnCancelDetailMultiInvoiceForm.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetailMultiInvoiceForm.FlatAppearance.BorderSize = 0;
		this.btnCancelDetailMultiInvoiceForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetailMultiInvoiceForm.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetailMultiInvoiceForm.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetailMultiInvoiceForm.Location = new System.Drawing.Point(937, 35);
		this.btnCancelDetailMultiInvoiceForm.Name = "btnCancelDetailMultiInvoiceForm";
		this.btnCancelDetailMultiInvoiceForm.Size = new System.Drawing.Size(59, 26);
		this.btnCancelDetailMultiInvoiceForm.TabIndex = 5;
		this.btnCancelDetailMultiInvoiceForm.Text = "Cancel";
		this.btnCancelDetailMultiInvoiceForm.UseVisualStyleBackColor = false;
		this.btnCancelDetailMultiInvoiceForm.Visible = false;
		this.btnCancelDetailMultiInvoiceForm.Click += new System.EventHandler(btnCancelDetailMultiInvoiceForm_Click);
		this.txtInvoiceAmountMultiInvoiceForm.Enabled = false;
		this.txtInvoiceAmountMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceAmountMultiInvoiceForm.Location = new System.Drawing.Point(245, 37);
		this.txtInvoiceAmountMultiInvoiceForm.Name = "txtInvoiceAmountMultiInvoiceForm";
		this.txtInvoiceAmountMultiInvoiceForm.Size = new System.Drawing.Size(132, 23);
		this.txtInvoiceAmountMultiInvoiceForm.TabIndex = 1;
		this.txtInvoiceAmountMultiInvoiceForm.Text = "0";
		this.txtInvoiceAmountMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label138.AutoSize = true;
		this.label138.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label138.Location = new System.Drawing.Point(245, 17);
		this.label138.Name = "label138";
		this.label138.Size = new System.Drawing.Size(93, 15);
		this.label138.TabIndex = 640;
		this.label138.Text = "Invoice Amount";
		this.CmbInvoiceNoMultiInvoiceForm.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoMultiInvoiceForm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoMultiInvoiceForm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoMultiInvoiceForm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance478).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance478).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance478;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance480).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance480).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance480).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance480).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance480;
		((AppearanceBase)appearance481).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance481;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance482).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance482).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance482).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance482).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance482;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance483).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance483).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance483;
		((AppearanceBase)appearance484).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance484).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance484;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance485).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance485;
		((AppearanceBase)appearance486).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance486).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance486;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance487).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance487).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance487).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance487).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance487).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance487;
		((AppearanceBase)appearance488).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance488;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance489).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance489).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance489;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance491).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance491;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoMultiInvoiceForm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoMultiInvoiceForm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoMultiInvoiceForm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoMultiInvoiceForm).Location = new System.Drawing.Point(8, 35);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoMultiInvoiceForm).Name = "CmbInvoiceNoMultiInvoiceForm";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoMultiInvoiceForm).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoMultiInvoiceForm).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoMultiInvoiceForm).TextChanged += new System.EventHandler(CmbInvoiceNoMultiInvoiceForm_TextChanged);
		this.label137.AutoSize = true;
		this.label137.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label137.Location = new System.Drawing.Point(8, 17);
		this.label137.Name = "label137";
		this.label137.Size = new System.Drawing.Size(65, 15);
		this.label137.TabIndex = 597;
		this.label137.Text = "Invoice No";
		this.groupBox3.Controls.Add(this.txtRemarksMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label130);
		this.groupBox3.Controls.Add(this.FttAmountMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label131);
		this.groupBox3.Controls.Add(this.txtNetAmountMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label132);
		this.groupBox3.Controls.Add(this.FttPrcntMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label133);
		this.groupBox3.Controls.Add(this.txtFobValueMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label134);
		this.groupBox3.Controls.Add(this.txtFreightMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label135);
		this.groupBox3.Controls.Add(this.txtGdValueMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label136);
		this.groupBox3.Controls.Add(this.txtExchangeRateMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label123);
		this.groupBox3.Controls.Add(this.txtDueDaysMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label124);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label125);
		this.groupBox3.Controls.Add(this.GdNoMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label126);
		this.groupBox3.Controls.Add(this.BankInvoiceNoMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label127);
		this.groupBox3.Controls.Add(this.GdDateMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label128);
		this.groupBox3.Controls.Add(this.BankInvoiceDateMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label129);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(8, 30);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1003, 126);
		this.groupBox3.TabIndex = 7;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Header Information";
		this.txtRemarksMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksMultiInvoiceForm.Location = new System.Drawing.Point(572, 87);
		this.txtRemarksMultiInvoiceForm.Name = "txtRemarksMultiInvoiceForm";
		this.txtRemarksMultiInvoiceForm.Size = new System.Drawing.Size(420, 23);
		this.txtRemarksMultiInvoiceForm.TabIndex = 13;
		this.label130.AutoSize = true;
		this.label130.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label130.Location = new System.Drawing.Point(574, 67);
		this.label130.Name = "label130";
		this.label130.Size = new System.Drawing.Size(52, 15);
		this.label130.TabIndex = 638;
		this.label130.Text = "Remarks";
		this.FttAmountMultiInvoiceForm.Enabled = false;
		this.FttAmountMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FttAmountMultiInvoiceForm.Location = new System.Drawing.Point(311, 87);
		this.FttAmountMultiInvoiceForm.Name = "FttAmountMultiInvoiceForm";
		this.FttAmountMultiInvoiceForm.ShortcutsEnabled = false;
		this.FttAmountMultiInvoiceForm.Size = new System.Drawing.Size(127, 23);
		this.FttAmountMultiInvoiceForm.TabIndex = 11;
		this.FttAmountMultiInvoiceForm.Text = "0";
		this.FttAmountMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.FttAmountMultiInvoiceForm.TextChanged += new System.EventHandler(FttAmountMultiInvoiceForm_TextChanged);
		this.label131.AutoSize = true;
		this.label131.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label131.Location = new System.Drawing.Point(312, 67);
		this.label131.Name = "label131";
		this.label131.Size = new System.Drawing.Size(74, 15);
		this.label131.TabIndex = 637;
		this.label131.Text = "FTT Amount";
		this.txtNetAmountMultiInvoiceForm.Enabled = false;
		this.txtNetAmountMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmountMultiInvoiceForm.Location = new System.Drawing.Point(439, 87);
		this.txtNetAmountMultiInvoiceForm.Name = "txtNetAmountMultiInvoiceForm";
		this.txtNetAmountMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtNetAmountMultiInvoiceForm.Size = new System.Drawing.Size(132, 23);
		this.txtNetAmountMultiInvoiceForm.TabIndex = 12;
		this.txtNetAmountMultiInvoiceForm.Text = "0";
		this.txtNetAmountMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label132.AutoSize = true;
		this.label132.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label132.Location = new System.Drawing.Point(440, 67);
		this.label132.Name = "label132";
		this.label132.Size = new System.Drawing.Size(73, 15);
		this.label132.TabIndex = 636;
		this.label132.Text = "Net Amount";
		this.FttPrcntMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FttPrcntMultiInvoiceForm.Location = new System.Drawing.Point(240, 87);
		this.FttPrcntMultiInvoiceForm.Name = "FttPrcntMultiInvoiceForm";
		this.FttPrcntMultiInvoiceForm.ShortcutsEnabled = false;
		this.FttPrcntMultiInvoiceForm.Size = new System.Drawing.Size(70, 23);
		this.FttPrcntMultiInvoiceForm.TabIndex = 10;
		this.FttPrcntMultiInvoiceForm.Text = "10";
		this.FttPrcntMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.FttPrcntMultiInvoiceForm.TextChanged += new System.EventHandler(FttPrcntMultiInvoiceForm_TextChanged);
		this.FttPrcntMultiInvoiceForm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FttPrcntMultiInvoiceForm_KeyPress);
		this.label133.AutoSize = true;
		this.label133.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label133.Location = new System.Drawing.Point(241, 67);
		this.label133.Name = "label133";
		this.label133.Size = new System.Drawing.Size(40, 15);
		this.label133.TabIndex = 635;
		this.label133.Text = "% FTT";
		this.txtFobValueMultiInvoiceForm.Enabled = false;
		this.txtFobValueMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFobValueMultiInvoiceForm.Location = new System.Drawing.Point(124, 87);
		this.txtFobValueMultiInvoiceForm.Name = "txtFobValueMultiInvoiceForm";
		this.txtFobValueMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtFobValueMultiInvoiceForm.Size = new System.Drawing.Size(115, 23);
		this.txtFobValueMultiInvoiceForm.TabIndex = 9;
		this.txtFobValueMultiInvoiceForm.Text = "0";
		this.txtFobValueMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFobValueMultiInvoiceForm.TextChanged += new System.EventHandler(txtFobValueMultiInvoiceForm_TextChanged);
		this.label134.AutoSize = true;
		this.label134.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label134.Location = new System.Drawing.Point(125, 67);
		this.label134.Name = "label134";
		this.label134.Size = new System.Drawing.Size(61, 15);
		this.label134.TabIndex = 634;
		this.label134.Text = "FOB Value";
		this.txtFreightMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightMultiInvoiceForm.Location = new System.Drawing.Point(8, 87);
		this.txtFreightMultiInvoiceForm.Name = "txtFreightMultiInvoiceForm";
		this.txtFreightMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtFreightMultiInvoiceForm.Size = new System.Drawing.Size(115, 23);
		this.txtFreightMultiInvoiceForm.TabIndex = 8;
		this.txtFreightMultiInvoiceForm.Text = "0";
		this.txtFreightMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightMultiInvoiceForm.TextChanged += new System.EventHandler(txtFreightMultiInvoiceForm_TextChanged);
		this.txtFreightMultiInvoiceForm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightMultiInvoiceForm_KeyPress);
		this.label135.AutoSize = true;
		this.label135.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label135.Location = new System.Drawing.Point(9, 67);
		this.label135.Name = "label135";
		this.label135.Size = new System.Drawing.Size(44, 15);
		this.label135.TabIndex = 633;
		this.label135.Text = "Freight";
		this.txtGdValueMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdValueMultiInvoiceForm.Location = new System.Drawing.Point(880, 41);
		this.txtGdValueMultiInvoiceForm.Name = "txtGdValueMultiInvoiceForm";
		this.txtGdValueMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtGdValueMultiInvoiceForm.Size = new System.Drawing.Size(112, 23);
		this.txtGdValueMultiInvoiceForm.TabIndex = 7;
		this.txtGdValueMultiInvoiceForm.Text = "0";
		this.txtGdValueMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGdValueMultiInvoiceForm.TextChanged += new System.EventHandler(txtGdValueMultiInvoiceForm_TextChanged);
		this.txtGdValueMultiInvoiceForm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGdValueMultiInvoiceForm_KeyPress);
		this.label136.AutoSize = true;
		this.label136.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label136.Location = new System.Drawing.Point(881, 21);
		this.label136.Name = "label136";
		this.label136.Size = new System.Drawing.Size(56, 15);
		this.label136.TabIndex = 632;
		this.label136.Text = "GD Value";
		this.txtExchangeRateMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRateMultiInvoiceForm.Location = new System.Drawing.Point(779, 41);
		this.txtExchangeRateMultiInvoiceForm.Name = "txtExchangeRateMultiInvoiceForm";
		this.txtExchangeRateMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtExchangeRateMultiInvoiceForm.Size = new System.Drawing.Size(100, 23);
		this.txtExchangeRateMultiInvoiceForm.TabIndex = 6;
		this.txtExchangeRateMultiInvoiceForm.Text = "0";
		this.txtExchangeRateMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRateMultiInvoiceForm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRateMultiInvoiceForm_KeyPress);
		this.label123.AutoSize = true;
		this.label123.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label123.Location = new System.Drawing.Point(780, 21);
		this.label123.Name = "label123";
		this.label123.Size = new System.Drawing.Size(84, 15);
		this.label123.TabIndex = 624;
		this.label123.Text = "Exchange Rate";
		this.txtDueDaysMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDaysMultiInvoiceForm.Location = new System.Drawing.Point(718, 41);
		this.txtDueDaysMultiInvoiceForm.Name = "txtDueDaysMultiInvoiceForm";
		this.txtDueDaysMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtDueDaysMultiInvoiceForm.Size = new System.Drawing.Size(60, 23);
		this.txtDueDaysMultiInvoiceForm.TabIndex = 5;
		this.txtDueDaysMultiInvoiceForm.Text = "0";
		this.txtDueDaysMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDueDaysMultiInvoiceForm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDaysMultiInvoiceForm_KeyPress);
		this.label124.AutoSize = true;
		this.label124.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label124.Location = new System.Drawing.Point(717, 21);
		this.label124.Name = "label124";
		this.label124.Size = new System.Drawing.Size(58, 15);
		this.label124.TabIndex = 623;
		this.label124.Text = "Due Days";
		this.CmbBankMultiInvoiceForm.AllowNull = (DefaultableBoolean)1;
		this.CmbBankMultiInvoiceForm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankMultiInvoiceForm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankMultiInvoiceForm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance452).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance452).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance452;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance453).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance453).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance453).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance453).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance453;
		((AppearanceBase)appearance454).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance454;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance455).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance455).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance455).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance455).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance455;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance456).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance456).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance456;
		((AppearanceBase)appearance458).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance458).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance458;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance459).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance459;
		((AppearanceBase)appearance460).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance460).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance460;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance461).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance461).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance461).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance461).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance461).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance461;
		((AppearanceBase)appearance462).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance462;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance463).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance463).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance463;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance464).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance464;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankMultiInvoiceForm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).Location = new System.Drawing.Point(517, 39);
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).Name = "CmbBankMultiInvoiceForm";
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).TabIndex = 4;
		this.label125.AutoSize = true;
		this.label125.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label125.Location = new System.Drawing.Point(517, 19);
		this.label125.Name = "label125";
		this.label125.Size = new System.Drawing.Size(68, 15);
		this.label125.TabIndex = 622;
		this.label125.Text = "Bank Name";
		this.GdNoMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GdNoMultiInvoiceForm.Location = new System.Drawing.Point(8, 41);
		this.GdNoMultiInvoiceForm.Name = "GdNoMultiInvoiceForm";
		this.GdNoMultiInvoiceForm.Size = new System.Drawing.Size(114, 23);
		this.GdNoMultiInvoiceForm.TabIndex = 0;
		this.label126.AutoSize = true;
		this.label126.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label126.Location = new System.Drawing.Point(9, 21);
		this.label126.Name = "label126";
		this.label126.Size = new System.Drawing.Size(43, 15);
		this.label126.TabIndex = 618;
		this.label126.Text = "GD No";
		this.BankInvoiceNoMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BankInvoiceNoMultiInvoiceForm.Location = new System.Drawing.Point(249, 41);
		this.BankInvoiceNoMultiInvoiceForm.Name = "BankInvoiceNoMultiInvoiceForm";
		this.BankInvoiceNoMultiInvoiceForm.Size = new System.Drawing.Size(139, 23);
		this.BankInvoiceNoMultiInvoiceForm.TabIndex = 2;
		this.label127.AutoSize = true;
		this.label127.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label127.Location = new System.Drawing.Point(251, 21);
		this.label127.Name = "label127";
		this.label127.Size = new System.Drawing.Size(94, 15);
		this.label127.TabIndex = 619;
		this.label127.Text = "Bank Invoice No";
		this.GdDateMultiInvoiceForm.CustomFormat = "dd-MMM-yy";
		this.GdDateMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GdDateMultiInvoiceForm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GdDateMultiInvoiceForm.Location = new System.Drawing.Point(123, 41);
		this.GdDateMultiInvoiceForm.Name = "GdDateMultiInvoiceForm";
		this.GdDateMultiInvoiceForm.Size = new System.Drawing.Size(125, 23);
		this.GdDateMultiInvoiceForm.TabIndex = 1;
		this.label128.AutoSize = true;
		this.label128.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label128.Location = new System.Drawing.Point(386, 21);
		this.label128.Name = "label128";
		this.label128.Size = new System.Drawing.Size(103, 15);
		this.label128.TabIndex = 620;
		this.label128.Text = "Bank Invoice Date";
		this.BankInvoiceDateMultiInvoiceForm.CustomFormat = "dd-MMM-yy";
		this.BankInvoiceDateMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BankInvoiceDateMultiInvoiceForm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.BankInvoiceDateMultiInvoiceForm.Location = new System.Drawing.Point(390, 41);
		this.BankInvoiceDateMultiInvoiceForm.Name = "BankInvoiceDateMultiInvoiceForm";
		this.BankInvoiceDateMultiInvoiceForm.Size = new System.Drawing.Size(125, 23);
		this.BankInvoiceDateMultiInvoiceForm.TabIndex = 3;
		this.label129.AutoSize = true;
		this.label129.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label129.Location = new System.Drawing.Point(119, 21);
		this.label129.Name = "label129";
		this.label129.Size = new System.Drawing.Size(52, 15);
		this.label129.TabIndex = 621;
		this.label129.Text = "GD Date";
		this.panel36.BackColor = System.Drawing.Color.Teal;
		this.panel36.Controls.Add(this.label122);
		this.panel36.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel36.Location = new System.Drawing.Point(0, 0);
		this.panel36.Name = "panel36";
		this.panel36.Size = new System.Drawing.Size(1156, 29);
		this.panel36.TabIndex = 6;
		this.label122.AutoSize = true;
		this.label122.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label122.ForeColor = System.Drawing.Color.White;
		this.label122.Location = new System.Drawing.Point(8, 5);
		this.label122.Name = "label122";
		this.label122.Size = new System.Drawing.Size(227, 18);
		this.label122.TabIndex = 29;
		this.label122.Text = "Multi Invoices Allocate To GD";
		this.toolStrip6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnResetMultiInvoiceGd, this.btnRefreshMultiInvoiceGd, this.btnSaveMultiInvoiceGd, this.btnUpdateMultiInvoiceGd });
		this.toolStrip6.Location = new System.Drawing.Point(0, 0);
		this.toolStrip6.Name = "toolStrip6";
		this.toolStrip6.Size = new System.Drawing.Size(1156, 27);
		this.toolStrip6.TabIndex = 6;
		this.toolStrip6.Text = "toolStrip6";
		this.btnResetMultiInvoiceGd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetMultiInvoiceGd.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetMultiInvoiceGd.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetMultiInvoiceGd.Name = "btnResetMultiInvoiceGd";
		this.btnResetMultiInvoiceGd.Size = new System.Drawing.Size(59, 24);
		this.btnResetMultiInvoiceGd.Text = "&New";
		this.btnResetMultiInvoiceGd.Click += new System.EventHandler(btnResetMultiInvoiceGd_Click);
		this.btnRefreshMultiInvoiceGd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshMultiInvoiceGd.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshMultiInvoiceGd.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshMultiInvoiceGd.Name = "btnRefreshMultiInvoiceGd";
		this.btnRefreshMultiInvoiceGd.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshMultiInvoiceGd.Text = "&Refresh";
		this.btnRefreshMultiInvoiceGd.Click += new System.EventHandler(btnRefreshMultiInvoiceGd_Click);
		this.btnSaveMultiInvoiceGd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveMultiInvoiceGd.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveMultiInvoiceGd.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveMultiInvoiceGd.Name = "btnSaveMultiInvoiceGd";
		this.btnSaveMultiInvoiceGd.Size = new System.Drawing.Size(64, 24);
		this.btnSaveMultiInvoiceGd.Text = "&Save";
		this.btnSaveMultiInvoiceGd.Click += new System.EventHandler(btnSaveMultiInvoiceGd_Click);
		this.btnUpdateMultiInvoiceGd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateMultiInvoiceGd.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateMultiInvoiceGd.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateMultiInvoiceGd.Name = "btnUpdateMultiInvoiceGd";
		this.btnUpdateMultiInvoiceGd.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateMultiInvoiceGd.Text = "&Update";
		this.btnUpdateMultiInvoiceGd.Visible = false;
		this.btnUpdateMultiInvoiceGd.Click += new System.EventHandler(btnUpdateMultiInvoiceGd_Click);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1164, 749);
		this.panel1.TabIndex = 1;
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFCurrencr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Location = new System.Drawing.Point(385, 129);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Name = "cmbFCurrencr";
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).TabIndex = 12;
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(111, 210);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 7;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1307, 237);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1404, 237);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.ultraCombo3.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo3.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Location = new System.Drawing.Point(111, 91);
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Name = "ultraCombo3";
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo3).TabIndex = 3;
		this.CmbLCContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbLCContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLCContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLCContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance254;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CmbLCContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Location = new System.Drawing.Point(111, 120);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Name = "CmbLCContractor";
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).TabIndex = 4;
		this.ultraCombo2.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance267;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance273).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Location = new System.Drawing.Point(385, 37);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Name = "ultraCombo2";
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).TabIndex = 9;
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance281;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance286).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance287).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(385, 67);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 10;
		this.cmbCarierType.AllowNull = (DefaultableBoolean)1;
		this.cmbCarierType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCarierType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCarierType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance294;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance295).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance295).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance295).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance299).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance300).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance302).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbCarierType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Location = new System.Drawing.Point(385, 96);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Name = "cmbCarierType";
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).TabIndex = 11;
		this.cmbExporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbExporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance307;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance308).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance313).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance313).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance314).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance314).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance314).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance315).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbExporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Location = new System.Drawing.Point(111, 180);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Name = "cmbExporterBank";
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).TabIndex = 6;
		this.cmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance319).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance319).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance319;
		((AppearanceBase)appearance320).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance320;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance321).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance321).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance321).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance322;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance324;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance325;
		((AppearanceBase)appearance326).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance326).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance326;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance327).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance327).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance327).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance327).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance327;
		((AppearanceBase)appearance328).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance330;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Location = new System.Drawing.Point(111, 151);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Name = "cmbImporterBank";
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).TabIndex = 5;
		this.cmbEFormNo.AllowNull = (DefaultableBoolean)1;
		this.cmbEFormNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEFormNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEFormNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance331).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance332).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance332).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance332).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance332;
		((AppearanceBase)appearance333).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance333;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance336).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance336).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance336).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance337).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance338).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance338).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance338;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance339;
		((AppearanceBase)appearance340).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance340).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance340;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance341).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance341).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance341).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance341).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance341;
		((AppearanceBase)appearance342).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance342;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance343).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance343).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance344;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbEFormNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Location = new System.Drawing.Point(662, 164);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Name = "cmbEFormNo";
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).TabIndex = 0;
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance345).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance347).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance347).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance347).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance347;
		((AppearanceBase)appearance348).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance348;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance349).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance349).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance349).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance349;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance350).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance350).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance350;
		((AppearanceBase)appearance351).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance351).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance351;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance352;
		((AppearanceBase)appearance353).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance353).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance353;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance354).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance354).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance354).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance354).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance354;
		((AppearanceBase)appearance355).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance355;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance356).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance356;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance358;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(385, 6);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 8;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(346, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(280, 27);
		this.miniToolStrip.TabIndex = 0;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(35, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.toolStripSeparator, this.toolStripButton4, this.toolStripButton5, this.helpToolStripButton });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(280, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip1";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton2.Text = "&Save";
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(69, 24);
		this.toolStripButton3.Text = "&Update";
		this.toolStripButton3.Visible = false;
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton4.Text = "&Attachment";
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton5.Text = "&SMS";
		this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(24, 24);
		this.helpToolStripButton.Text = "He&lp";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1110, 1);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1095, 0);
		this.ctrlGrdBar3.MyGrid = this.grdotheritems;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1096, 0);
		this.ctrlGrdBar4.MyGrid = this.grdpaymentdetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar4.TabIndex = 27;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1101, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1102, 0);
		this.ctrlGrdBar6.MyGrid = this.grdAdvanceUtlize;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar6.TabIndex = 27;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1116, 0);
		this.ctrlGrdBar5.MyGrid = this.grdGdBreakUp;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar5.TabIndex = 27;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1107, 0);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdGDhistory;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar7.TabIndex = 45;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.ctrlGrdBar9.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar9.FormName = this;
		this.ctrlGrdBar9.Location = new System.Drawing.Point(1114, 2);
		this.ctrlGrdBar9.MyGrid = this.DataGridHistoryMultiInvoiceToGdForm;
		this.ctrlGrdBar9.Name = "ctrlGrdBar9";
		this.ctrlGrdBar9.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar9.TabIndex = 30;
		this.ctrlGrdBar9.Load += new System.EventHandler(ctrlGrdBar9_Load);
		this.ctrlGrdBar8.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar8.FormName = this;
		this.ctrlGrdBar8.Location = new System.Drawing.Point(1114, 1);
		this.ctrlGrdBar8.MyGrid = this.grdMultiInvoiceAgainstGd;
		this.ctrlGrdBar8.Name = "ctrlGrdBar8";
		this.ctrlGrdBar8.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar8.TabIndex = 27;
		this.ctrlGrdBar8.Load += new System.EventHandler(ctrlGrdBar8_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1164, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "CommiercialInvoiceAgainstPreInvoiceTransfer";
		this.Text = "Commercial Invoice";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CommiercialInvoiceAgainstPreInvoiceTransfer_FormClosing);
		base.Load += new System.EventHandler(ImProformaInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.tabPage3.PerformLayout();
		this.GroupDetail.ResumeLayout(false);
		this.GroupDetail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).EndInit();
		this.panel18.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstruments).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dcmbpaymentterm).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.HeaderGropBox.ResumeLayout(false);
		this.HeaderGropBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPrefixType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbinvoiceno).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOtherDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbOtherCustomer).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliverytermnew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcareiertype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTermsNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterBankNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfcycode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbexporterBankNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPreInvoice).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupCust).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage7.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		this.panel28.ResumeLayout(false);
		this.tabControl3.ResumeLayout(false);
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAdvanceUtlize).EndInit();
		this.panel33.ResumeLayout(false);
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceAdvanceUtilize).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFIAdvanceUtlize).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGdNoAdvanceUtilize).EndInit();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		this.tabPage8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAdvanceHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel31.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGdBreakUp).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceGdBreakUp).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel27.ResumeLayout(false);
		this.panel27.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.GDHistory.ResumeLayout(false);
		this.GDHistory.PerformLayout();
		this.panel43.ResumeLayout(false);
		this.panel44.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGDhistory).EndInit();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		this.panel42.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankGdBankHistory).EndInit();
		this.toolStrip7.ResumeLayout(false);
		this.toolStrip7.PerformLayout();
		this.InvoicesAllocationToGd.ResumeLayout(false);
		this.InvoicesAllocationToGd.PerformLayout();
		this.panel39.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryMultiInvoiceToGdForm).EndInit();
		this.panel41.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankMultiInvoiceAgainstGdHistory).EndInit();
		this.panel40.ResumeLayout(false);
		this.panel40.PerformLayout();
		this.panel37.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMultiInvoiceAgainstGd).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.panel35.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoMultiInvoiceForm).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankMultiInvoiceForm).EndInit();
		this.panel36.ResumeLayout(false);
		this.panel36.PerformLayout();
		this.toolStrip6.ResumeLayout(false);
		this.toolStrip6.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
