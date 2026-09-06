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
using Architecture.BLL.Export;
using Architecture.BLL.Import;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Import;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Import
{

public class frmImportInvoice : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private long RecId;

	private int AppActionId;

	private long RowVersionLong;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOtherItem = -1;

	private int updateDetailIndexPT = -1;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private DataTable dtdetail = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtPerformaNo = new DataTable();

	private DataTable dtInvoiceAndLcNo = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtFiList = new DataTable();

	private bool BranchFeature;

	private bool FinancialisActiveonCommercialInvoice;

	private bool AllowExportMultiCompanies;

	private List<invoiceDetail> LstRemoveRecordDetail = new List<invoiceDetail>();

	private List<invoicePaymentTerm> LstRemoveRecordPaymentTerm = new List<invoicePaymentTerm>();

	private List<invoiceOtherItem> LstRemoveRecordOtherItem = new List<invoiceOtherItem>();

	private IContainer components;

	private Panel panel4;

	private Panel panel5;

	private Panel panel1;

	private Panel panel7;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel8;

	private GroupBox HeaderGropBox;

	private Label label28;

	internal UltraCombo CmbExporterName;

	private Label label21;

	private Label label1;

	internal GridEX grdDetail;

	private Panel panel10;

	private Label label23;

	private Panel panel12;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private Panel panel14;

	private Panel panel16;

	private ToolStrip toolStrip3;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo cmbFCurrencr;

	internal UltraCombo combranches;

	internal UltraCombo comproject;

	internal UltraCombo CmbLCContractor;

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

	private Label label7;

	private TextBox txtLotRef;

	private Label label11;

	internal UltraCombo cmbexporterBankNew;

	internal UltraCombo cmbimporterBankNew;

	internal UltraCombo CmbPaymentTermsNew;

	private Label label24;

	private Label label56;

	internal UltraCombo cmbDestinationPort;

	internal UltraCombo cmbLoadingPort;

	private Label label16;

	private Label label47;

	private TextBox txtfcyAmountHeader;

	private Label label25;

	private Label label27;

	internal UltraCombo CmbFcyCode;

	private TextBox txtExRate;

	private Label label44;

	private DateTimePicker txtGdDate;

	private TextBox txtGdNo;

	private Label label49;

	private Label label46;

	private TextBox txtGrossWeightHeader;

	private Label label54;

	private TextBox txtNetWeightHeader;

	private Label label55;

	private TextBox txtNoOfContainerHeader;

	private Label label57;

	private Label label53;

	private Label label50;

	private Label label31;

	private Label label60;

	internal GridEX gridDetailHistory;

	private CheckBox ChkPrintPreview;

	internal UltraCombo CmbDabitAccount;

	private Label lblCreditAc;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel22;

	private Panel panel23;

	private Panel panel17;

	private Panel panel11;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TextBox txtitemDescription;

	private Label label34;

	private TextBox txtCostMTon;

	private Label label29;

	private TextBox txtNoOfBags;

	private Label label30;

	internal UltraCombo CmbItemName;

	private Label label32;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnAddinGrid;

	internal UltraCombo CmbPackUom;

	private Label label36;

	private TextBox txtAmount;

	private Label label37;

	private Label label38;

	internal UltraCombo CmbRateUom;

	private TabPage tabPage4;

	private Panel panel19;

	internal GridEX grdotheritems;

	private Panel panel18;

	private Button BtnCancelOtherItem;

	private Button BtnUpdateOtherItem;

	private Button BtnAddOtherItem;

	private Label label14;

	private TextBox txtRemarksOtherItem;

	private TextBox txtQtyOtherItem;

	private Label label15;

	private TextBox txtRateOtherItem;

	private Label label63;

	private Label label70;

	private TextBox txtAmountOtherItem;

	internal UltraCombo CmbItemOtherItem;

	private Label label71;

	private TabPage tabPage5;

	private Panel panel21;

	internal GridEX grdpaymentdetail;

	private Panel panel20;

	private Label label82;

	private Label label81;

	private Label label80;

	private Label label79;

	private Button btnCancelPaymentDetail;

	private Button BtnUpdatePaymentDetail;

	private Button BtnAddPaymentDetail;

	private TextBox txtDueDaysPaymentDetail;

	private Label label78;

	private TextBox txtFcyAmountPaymentDetail;

	private Label label77;

	internal UltraCombo CmbPaymentTermPaymentDetail;

	private Label label76;

	private TextBox txtBalanceFIPaymentDetail;

	private Label label83;

	private TextBox dtxtpercentoftotal;

	private Label label74;

	internal UltraCombo CmbFinInstrumentsPaymentDetail;

	private Label label75;

	private Label label85;

	private Label label51;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	private Label label59;

	internal UltraCombo cmbConsignee;

	private Label label86;

	private Label label58;

	private TextBox txtRemarksPaymentDetail;

	private Panel panel15;

	private Label label69;

	private Panel panel3;

	internal GridEX DataGridHistory;

	private Label label92;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label20;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label93;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label94;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbImportCompany;

	private Label labelExportCompany;

	private Label label104;

	private Label label109;

	internal UltraCombo CmbImporterName;

	private DateTimePicker txtPerformaDate;

	internal UltraCombo CmbContractNo;

	internal UltraCombo CmbPerformaNo;

	internal UltraCombo CmbSalesPerson;

	private Label label103;

	private Label label98;

	private Label label101;

	private Label label99;

	private Label label97;

	private Label label96;

	private Label label108;

	internal UltraCombo CmbIncomeTerm;

	private TextBox txtNoOfPackagesHeader;

	private Label label110;

	private TextBox txtBLNo;

	private DateTimePicker txtBlDate;

	private Label label107;

	private Label label48;

	private DateTimePicker txtContractDate;

	internal UltraCombo cmbInvoiceNo;

	private Label label114;

	private Label label113;

	private TextBox txtGdValue;

	private Button BtnDefineInvoiceNo;

	private Button BtnDefineContractNo;

	private Button BtnDefinePerformaNo;

	private Label label17;

	private TextBox txtWeightDetail;

	private Label label2;

	private TextBox txtRemarksDetail;

	private Label label19;

	private TextBox txtDocNo;

	private DateTimePicker txtInvoiceDate;

	private TextBox txtQtyMTon;

	private Label label35;

	internal UltraCombo CmbBranchName;

	private Label labelBranchName;

	private Label label5;

	internal UltraCombo cmbStatus;

	private Label label3;

	private Label label6;

	private Label label8;

	public frmImportInvoice(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		base.Load += ImProformaInvoice_Load;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			dtInvoiceAndLcNo.Columns.Add("InvoiceId", typeof(long));
			dtInvoiceAndLcNo.Columns.Add("InvoiceNo", typeof(string));
			dtInvoiceAndLcNo.Columns.Add("InvoiceDate", typeof(DateTime));
			dtInvoiceAndLcNo.Columns.Add("ContractId", typeof(long));
			dtInvoiceAndLcNo.Columns.Add("ContractNo", typeof(string));
			dtInvoiceAndLcNo.Columns.Add("ContractDate", typeof(DateTime));
			dtPerformaNo.Columns.Add("Id", typeof(long));
			dtPerformaNo.Columns.Add("ProformaNo", typeof(string));
			dtPerformaNo.Columns.Add("ProformaDate", typeof(DateTime));
			dtdetail.Columns.Add("Id", typeof(long));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("ItemDetail");
			dtdetail.Columns.Add("JobLotId", typeof(int));
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("PackSizeId", typeof(int));
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("PackEquivalent", typeof(double));
			dtdetail.Columns.Add("NoOfBags", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("Cost/M.Ton", typeof(double));
			dtdetail.Columns.Add("RateUOMId", typeof(int));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateEquivalent", typeof(double));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("Remarks");
			dtdetail.Columns.Add("OtherItemAmount", typeof(double));
			dtdetail.Columns.Add("RowVersionLong", typeof(long));
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			dtOtherItem.Columns.Add("Id", typeof(int));
			dtOtherItem.Columns.Add("ItemId", typeof(int));
			dtOtherItem.Columns.Add("ItemName");
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("Rate", typeof(double));
			dtOtherItem.Columns.Add("Amount", typeof(double));
			dtOtherItem.Columns.Add("Remarks");
			dtOtherItem.Columns.Add("RowVersionLong", typeof(long));
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			dtPaymentTerm.Columns.Add("Id", typeof(int));
			dtPaymentTerm.Columns.Add("PaymentTermId", typeof(int));
			dtPaymentTerm.Columns.Add("PaymentTerm");
			dtPaymentTerm.Columns.Add("DocumentTypeId", typeof(int));
			dtPaymentTerm.Columns.Add("ExImEFormRegistrationId", typeof(int));
			dtPaymentTerm.Columns.Add("FinancialInstrumentNo");
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(double));
			dtPaymentTerm.Columns.Add("FcyAmount", typeof(double));
			dtPaymentTerm.Columns.Add("DueDays");
			dtPaymentTerm.Columns.Add("Remarks");
			dtPaymentTerm.Columns.Add("RowVersionLong", typeof(long));
			grdpaymentdetail.DataSource = dtPaymentTerm;
			grdpaymentdetail.RetrieveStructure();
			grdSettingsPaymentTerm();
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
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FinancialisActiveonCommercialInvoice = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("FinancialisActiveonCommercialInvoice"));
			AllowExportMultiCompanies = CommonServices.GetERPFeatureById(9);
			BranchFeature = CommonServices.GetERPFeatureById(11);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			string documentNoData = generateCode();
			DataTable suppCustForExportData = BindImporterAndExporter();
			DataTable salePersonData = BindSalesPerson();
			DataTable proformaNoData = BindProformaNo();
			DataTable importRelatedData = BindImportRelatedCombo();
			DataTable currencyData = BindMultiCurrency();
			DataTable itemData = CommonServices.ItemGetForComboServiceBind();
			DataTable jobLotData = CommonServices.JobLotGetAllService();
			DataTable invoiceAndLcOrderNo = BindInvoiceAndLcOrderNo();
			DataTable debitAccountData = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,10");
			List<Bank> banklstData = BindImporterandExportBank();
			DataTable exportCompaniesByCompany = ExImLcOrder.GetExportCompaniesByCompany(UserAccount.OrganizationId, UserAccount.CompanyId);
			return new
			{
				formrights = formrights,
				documentNoData = documentNoData,
				SuppCustForExportData = suppCustForExportData,
				SalePersonData = salePersonData,
				proformaNoData = proformaNoData,
				ImportRelatedData = importRelatedData,
				CurrencyData = currencyData,
				ItemData = itemData,
				JobLotData = jobLotData,
				InvoiceAndLcOrderNo = invoiceAndLcOrderNo,
				DebitAccountData = debitAccountData,
				BanklstData = banklstData,
				ExportCompanyData = exportCompaniesByCompany
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
				AutoScroll = true;
				base.AutoScrollMargin = new Size(20, 20);
				base.AutoScrollMinSize = new Size(1200, 700);
				((Control)(object)cmbInvoiceNo).Enabled = true;
				lblCreditAc.Visible = false;
				((Control)(object)CmbDabitAccount).Visible = false;
				if (FinancialisActiveonCommercialInvoice)
				{
					lblCreditAc.Visible = true;
					((Control)(object)CmbDabitAccount).Visible = true;
				}
				btnsave.Enabled = formrights.DoHaveSaveRight;
				btnupdate.Enabled = formrights.DoHaveUpdateRights;
				btnPrint.Enabled = formrights.DoHavePrintRights;
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
				ctrlGrdBar3.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlGrdBar3.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlGrdBar3.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar3.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlGrdBar3.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar3.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				((Control)(object)CmbBranchName).Visible = BranchFeature;
				labelBranchName.Visible = BranchFeature;
				if (RecId == 0L)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				BranchesFill();
				ProformaNoFill(result.proformaNoData);
				InvoiceAndLcOrderNoFill(result.InvoiceAndLcOrderNo);
				ImporterAndExporterFill(result.SuppCustForExportData);
				SalesPersonFill(result.SalePersonData);
				ImportRelatedComboFill(result.ImportRelatedData);
				MultiCurrencyFill(result.CurrencyData);
				ItemFill(result.ItemData);
				JobLotFill(result.JobLotData);
				DebitAccountFill(result.DebitAccountData);
				ImporterandExportBankFill(result.BanklstData);
				OrderStatus();
				bindConsigneeAgainstCustomer();
				BindOtherItems();
				BindFinancialInstrument();
				tabControl2.SelectedIndex = 0;
				btnAddinGrid.Visible = true;
				btnGridUpdate.Visible = false;
				btnCancel.Visible = false;
				ChkPrintPreview.Checked = true;
				if (BranchFeature)
				{
					CmbBranchName.Focus();
				}
				else
				{
					CmbImporterName.Focus();
				}
				if (AllowExportMultiCompanies)
				{
					((Control)(object)CmbImportCompany).Visible = true;
					labelExportCompany.Visible = true;
					ImportCompaniesByCompanyIdBind(result.ExportCompanyData);
				}
				else
				{
					((Control)(object)CmbImportCompany).Visible = false;
					labelExportCompany.Visible = false;
				}
				btnsave.Visible = true;
				btnupdate.Visible = false;
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void ImProformaInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbImporterName).Enabled = true;
			((Control)(object)CmbExporterName).Enabled = true;
			((Control)(object)CmbItemName).Enabled = true;
			((Control)(object)cmbFCurrencr).Enabled = true;
			((Control)(object)CmbFcyCode).Enabled = true;
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			List<getGlobalBranchesAllocateToUser> lst = clsGlobalVariables.globalBranchesAllocateToUser;
			DataTable dtBranch = new DataTable();
			if (dtBranch.Columns.Count == 0)
			{
				dtBranch.Columns.Add("BranchId", typeof(int));
				dtBranch.Columns.Add("BranchName", typeof(string));
			}
			if (lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtBranch.Rows.Add(lst[i].BranchId, lst[i].BranchName);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBranchName, dtBranch, "BranchId", "BranchName", "Branch Name", AllColumns: true, Conversion.ToInt(CmbBranchName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtBranch.Rows.Count > 0)
			{
				((Control)(object)CmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string generateCode()
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
			return Conversion.ToString((object)invoiceMaster.GenerateCode(new invoiceMaster
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				documentTypeId = 902,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtDocNo.Text = documentData;
	}

	private DataTable BindProformaNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			return ProformaMaster.GetProformaNoFromMasterDocumentSerial(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				RecId = RecId,
				SkipZero = 1
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ProformaNoFill(DataTable dt)
	{
		try
		{
			long id = Conversion.ToInt(CmbPerformaNo.Value);
			if (dt.Rows.Count > 0)
			{
				dtPerformaNo.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtPerformaNo.Rows.Add(dt.Rows[i]["proformaMasterId"], dt.Rows[i]["proformaNo"], dt.Rows[i]["proformaDate"]);
				}
			}
			if (dtPerformaNo != null && dtPerformaNo.Rows.Count > 0)
			{
				DDL.BindDDL(dtPerformaNo, CmbPerformaNo, "Id", "ProformaNo", "Proforma No", ZeroIndex: false);
				if (id > 0)
				{
					if (dtPerformaNo.AsEnumerable().Any((DataRow row) => row.Field<long>("Id") == id))
					{
						CmbPerformaNo.Value = id;
					}
					else
					{
						((Control)(object)CmbPerformaNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbPerformaNo).DataSource = null;
				((Control)(object)CmbPerformaNo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable BindInvoiceAndLcOrderNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return masterDocumentSerial.ComboForInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				RecId = RecId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InvoiceAndLcOrderNoFill(DataTable dt)
	{
		try
		{
			long id = Conversion.ToInt(CmbContractNo.Value);
			long id2 = Conversion.ToInt(cmbInvoiceNo.Value);
			if (dt.Rows.Count > 0)
			{
				dtInvoiceAndLcNo.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtInvoiceAndLcNo.Rows.Add(dt.Rows[i]["invoiceMasterId"], dt.Rows[i]["invoiceNo"], dt.Rows[i]["invoiceDate"], dt.Rows[i]["LocOrderMasterId"], dt.Rows[i]["locOrderNo"], dt.Rows[i]["locOrderDate"]);
				}
			}
			if (dtInvoiceAndLcNo != null && dtInvoiceAndLcNo.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoiceAndLcNo, CmbContractNo, "ContractId", "ContractNo", "Contract No", ZeroIndex: false);
				((UltraGridBase)CmbContractNo).DisplayLayout.Bands[0].Columns[1].Hidden = true;
				((UltraGridBase)CmbContractNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (id > 0)
				{
					if (dtInvoiceAndLcNo.AsEnumerable().Any((DataRow row) => row.Field<long>("ContractId") == id))
					{
						CmbContractNo.Value = id;
					}
					else
					{
						((Control)(object)CmbContractNo).Text = string.Empty;
					}
				}
				DDL.BindDDL(dtInvoiceAndLcNo, cmbInvoiceNo, "InvoiceId", "InvoiceNo", "Invoice No", ZeroIndex: false);
				((UltraGridBase)cmbInvoiceNo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbInvoiceNo).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)cmbInvoiceNo).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				if (id2 > 0)
				{
					if (dtInvoiceAndLcNo.AsEnumerable().Any((DataRow row) => row.Field<long>("InvoiceId") == id2))
					{
						cmbInvoiceNo.Value = id2;
					}
					else
					{
						((Control)(object)cmbInvoiceNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
				((UltraGridBase)CmbContractNo).DataSource = null;
				((Control)(object)cmbInvoiceNo).Text = string.Empty;
				((Control)(object)CmbContractNo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable BindImporterAndExporter()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
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

	private void ImporterAndExporterFill(DataTable dtSupplier)
	{
		try
		{
			int id = Conversion.ToInt(CmbImporterName.Value);
			Conversion.ToInt(CmbExporterName.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, CmbImporterName, "Id", "CompanyName", "Importer Name", true);
				if (id > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbImporterName.Value = id;
					}
					else
					{
						((Control)(object)CmbImporterName).Text = string.Empty;
					}
				}
				DDL.BindDDLNew(dtSupplier, CmbExporterName, "Id", "CompanyName", "Exporter Name", true);
				if (id > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbExporterName.Value = id;
					}
					else
					{
						((Control)(object)CmbExporterName).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbExporterName).DataSource = null;
				((UltraGridBase)CmbImporterName).DataSource = null;
				((Control)(object)CmbExporterName).Text = string.Empty;
				((Control)(object)CmbImporterName).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable BindSalesPerson()
	{
		try
		{
			return CommonServices.GetSupplierustomerByCustomerGroupId("9");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SalesPersonFill(DataTable dtSalePerson)
	{
		try
		{
			if (dtSalePerson.Rows.Count > 0)
			{
				int id = Conversion.ToInt(CmbSalesPerson.Value);
				DDL.BindDDL(dtSalePerson, CmbSalesPerson, "Id", "CompanyName", "Sales Person", ZeroIndex: true);
				((UltraGridBase)CmbSalesPerson).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (id > 0)
				{
					if (dtSalePerson.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbSalesPerson.Value = id;
					}
					else
					{
						((Control)(object)CmbSalesPerson).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbSalesPerson).DataSource = null;
				((Control)(object)CmbSalesPerson).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable BindImportRelatedCombo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return invoiceMaster.GetDataForDropDownFrom_ImportRelated(new ReportsParameters
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

	private void ImportRelatedComboFill(DataTable dt)
	{
		try
		{
			int id1 = Conversion.ToInt(CmbIncomeTerm.Value);
			int id2 = Conversion.ToInt(CmbPaymentTermPaymentDetail.Value);
			int id3 = Conversion.ToInt(CmbPaymentTermsNew.Value);
			int id4 = Conversion.ToInt(cmbLoadingPort.Value);
			int id5 = Conversion.ToInt(cmbDestinationPort.Value);
			DataTable dtInco = new DataTable();
			dtInco.Columns.Add("Id", typeof(int));
			dtInco.Columns.Add("Name");
			DataTable dtPayterm = new DataTable();
			dtPayterm.Columns.Add("Id", typeof(int));
			dtPayterm.Columns.Add("Name");
			DataTable dtSeaAirport = new DataTable();
			dtSeaAirport.Columns.Add("Id", typeof(int));
			dtSeaAirport.Columns.Add("Name");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "IncoTerm")
					{
						dtInco.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "PaymentTermTitle")
					{
						dtPayterm.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "SeaAirport")
					{
						dtSeaAirport.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			if (dtInco.Rows.Count > 0)
			{
				DDL.BindDDL(dtInco, CmbIncomeTerm, "Id", "Name", "Inco Term", ZeroIndex: true);
				((UltraGridBase)CmbIncomeTerm).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (id1 > 0)
				{
					if (dtInco.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id1))
					{
						CmbIncomeTerm.Value = id1;
					}
					else
					{
						((Control)(object)CmbIncomeTerm).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbIncomeTerm).Text = string.Empty;
				((UltraGridBase)CmbIncomeTerm).DataSource = null;
			}
			if (dtPayterm.Rows.Count > 0)
			{
				DDL.BindDDL(dtPayterm, CmbPaymentTermPaymentDetail, "Id", "Name", "Payment Term", ZeroIndex: true);
				((UltraGridBase)CmbPaymentTermPaymentDetail).DisplayLayout.Bands[0].Columns[1].Width = 200;
				if (id2 > 0)
				{
					if (dtPayterm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id2))
					{
						CmbPaymentTermPaymentDetail.Value = id2;
					}
					else
					{
						((Control)(object)CmbPaymentTermPaymentDetail).Text = string.Empty;
					}
				}
				DDL.BindDDL(dtPayterm, CmbPaymentTermsNew, "Id", "Name", "Payment Term", ZeroIndex: true);
				((UltraGridBase)CmbPaymentTermsNew).DisplayLayout.Bands[0].Columns[1].Width = 200;
				if (id3 > 0)
				{
					if (dtPayterm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id3))
					{
						CmbPaymentTermsNew.Value = id3;
					}
					else
					{
						((Control)(object)CmbPaymentTermsNew).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPaymentTermPaymentDetail).Text = string.Empty;
				((UltraGridBase)CmbPaymentTermPaymentDetail).DataSource = null;
				((Control)(object)CmbPaymentTermsNew).Text = string.Empty;
				((UltraGridBase)CmbPaymentTermsNew).DataSource = null;
			}
			if (dtSeaAirport.Rows.Count > 0)
			{
				DDL.BindDDL(dtSeaAirport, cmbLoadingPort, "Id", "Name", "loading port", ZeroIndex: true);
				if (id4 > 0)
				{
					if (dtSeaAirport.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id4))
					{
						cmbLoadingPort.Value = id4;
					}
					else
					{
						((Control)(object)cmbLoadingPort).Text = string.Empty;
					}
				}
				DDL.BindDDL(dtSeaAirport, cmbDestinationPort, "Id", "Name", "Destination port", ZeroIndex: true);
				if (id5 > 0)
				{
					if (dtSeaAirport.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id5))
					{
						cmbDestinationPort.Value = id5;
					}
					else
					{
						((Control)(object)cmbDestinationPort).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbLoadingPort).Text = string.Empty;
				((UltraGridBase)cmbLoadingPort).DataSource = null;
				((Control)(object)cmbDestinationPort).Text = string.Empty;
				((UltraGridBase)cmbDestinationPort).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable BindMultiCurrency()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return MultiCurrency.GetAll(new MultiCurrency
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

	private void MultiCurrencyFill(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbFcyCode.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbFcyCode, "Id", "CurrencyName", "CurrencyName", true);
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbFcyCode.Value = id;
					}
					else
					{
						((Control)(object)CmbFcyCode).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbFcyCode).Text = string.Empty;
				((UltraGridBase)CmbFcyCode).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbItemName.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 400;
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbItemName.Value = id;
					}
					else
					{
						((Control)(object)CmbItemName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItemName).Text = string.Empty;
				((UltraGridBase)CmbItemName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobLotFill(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbJobLot.Value);
			DDL.BindDDLNew(dt, CmbJobLot, "Id", "JobLotDescription", "Job Lot", true);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbJobLot.Value = id;
				}
				else
				{
					((Control)(object)CmbJobLot).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			string packUom = ((Control)(object)CmbPackUom).Text;
			string RateUom = ((Control)(object)CmbRateUom).Text;
			DataTable dtUOM = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemName.Value));
			DDL.BindDDL(dtUOM, CmbPackUom, "Id", "UOMCode", "Pack Uom", ZeroIndex: true);
			DDL.BindDDL(dtUOM, CmbRateUom, "Id", "UOMCode", "Rate Uom", ZeroIndex: true);
			if (packUom != null && packUom != string.Empty)
			{
				if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == packUom))
				{
					((Control)(object)CmbPackUom).Text = packUom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			if (RateUom != null && RateUom != string.Empty)
			{
				if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == RateUom))
				{
					((Control)(object)CmbRateUom).Text = RateUom;
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DebitAccountFill(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbDabitAccount.Value);
			DDL.BindDDL(dt, CmbDabitAccount, "Id", "AccountTitle", "AccountTitle", ZeroIndex: true);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbDabitAccount.Value = id;
				}
				else
				{
					((Control)(object)CmbDabitAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private List<Bank> BindImporterandExportBank()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return Bank.GetAll(new Bank
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

	private void ImporterandExportBankFill(List<Bank> lst)
	{
		try
		{
			int id = Conversion.ToInt(cmbimporterBankNew.Value);
			int id2 = Conversion.ToInt(cmbexporterBankNew.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("BranchName");
			DataTable dtimporter = new DataTable();
			dtimporter.Columns.Add("Id", typeof(int));
			dtimporter.Columns.Add("BranchName");
			if (lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					if (lst[i].IsHomeland == "Home Country")
					{
						dtimporter.Rows.Add(lst[i].Id, lst[i].BranchName);
					}
					if (lst[i].IsHomeland == "Foreign Country")
					{
						dt.Rows.Add(lst[i].Id, lst[i].BranchName);
					}
				}
			}
			DDL.BindDDL(dt, cmbexporterBankNew, "Id", "BranchName", "ExporterBank", ZeroIndex: true);
			DDL.BindDDL(dtimporter, cmbimporterBankNew, "Id", "BranchName", "ImporterBank", ZeroIndex: true);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbexporterBankNew).Rows[1].Activate();
			}
			if (id > 0)
			{
				if (dtimporter.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbimporterBankNew.Value = id;
				}
				else
				{
					((Control)(object)cmbimporterBankNew).Text = string.Empty;
				}
			}
			if (id2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id2))
				{
					cmbexporterBankNew.Value = id2;
				}
				else
				{
					((Control)(object)cmbexporterBankNew).Text = string.Empty;
				}
			}
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
			DDL.BindDDL(dtstatus, cmbStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImportCompaniesByCompanyIdBind(DataTable lst)
	{
		try
		{
			int id = Conversion.ToInt(CmbImportCompany.Value);
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(lst, CmbImportCompany, "Id", "CompName", "Export Company ", ZeroIndex: true);
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbImportCompany.Value = id;
				}
				else
				{
					((Control)(object)CmbImportCompany).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindConsigneeAgainstCustomer()
	{
		try
		{
			int id = Conversion.ToInt(cmbConsignee.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CompanyName");
			if (Conversion.ToInt(CmbExporterName.Value) > 0)
			{
				DataTable dt2 = CommonServices.GetSupplierustomerByCustomerGroupId(null, Conversion.ToInt(CmbExporterName.Value));
				if (dt2.Rows.Count > 0)
				{
					for (int i = 0; i < dt2.Rows.Count; i++)
					{
						dt.Rows.Add(dt2.Rows[i]["Id"], dt2.Rows[i]["CompanyName"]);
					}
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbConsignee, "Id", "CompanyName", "Consignee", ZeroIndex: true);
				((UltraGridBase)cmbConsignee).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbConsignee.Value = id;
				}
				else
				{
					((Control)(object)cmbConsignee).Text = string.Empty;
				}
			}
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

	private void BindOtherItems()
	{
		try
		{
			int id = Conversion.ToInt(CmbItemOtherItem.Value);
			DataTable lst = CommonServices.GetItemByItemTypeId("14");
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(lst, CmbItemOtherItem, "Id", "ItemName", "Item Name", true);
			((UltraGridBase)CmbItemOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbItemOtherItem.Value = id;
				}
				else
				{
					((Control)(object)CmbItemOtherItem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindFinancialInstrument()
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value);
			dtFiList.Rows.Clear();
			if (dtFiList.Columns.Count == 0)
			{
				dtFiList.Columns.Add("Id", typeof(int));
				dtFiList.Columns.Add("EFormNo");
				dtFiList.Columns.Add("PaymenttermId");
				dtFiList.Columns.Add("DocumnetTypeId");
			}
			if (Conversion.ToInt(CmbExporterName.Value) > 0)
			{
				DataTable dt = ExImEFormRegistration.GetFINoForInvoice(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(CmbExporterName.Value)
				});
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtFiList.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["EFormNo"], dt.Rows[i]["PaymenttermId"], dt.Rows[i]["DocumentTypeId"]);
					}
					DDL.BindDDL(dtFiList, CmbFinInstrumentsPaymentDetail, "Id", "EFormNo", "FI No", ZeroIndex: true);
					((UltraGridBase)CmbFinInstrumentsPaymentDetail).DisplayLayout.Bands[0].Columns[1].Width = 400;
				}
			}
			if (id > 0)
			{
				if (dtFiList.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbFinInstrumentsPaymentDetail.Value = id;
				}
				else
				{
					((Control)(object)CmbFinInstrumentsPaymentDetail).Text = string.Empty;
				}
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
			if (Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) > 0)
			{
				CmbPaymentTermPaymentDetail.Value = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPaymentDetail).SelectedRow.Cells[2].Value);
				GetFinancialInstrumentsBalance();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPerformaNo_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt64(CmbPerformaNo.Value) > 0)
			{
				txtPerformaDate.Value = Conversion.ToDateTime(((UltraDropDownBase)CmbPerformaNo).SelectedRow.Cells[2].Value);
			}
			else
			{
				txtPerformaDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbContractNo_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbContractNo).ActiveRow != null && Conversion.ToInt64(CmbContractNo.Value) > 0)
			{
				txtContractDate.Value = Conversion.ToDateTime(((UltraDropDownBase)CmbContractNo).SelectedRow.Cells["ContractDate"].Value);
				cmbInvoiceNo.Value = Conversion.ToInt(((UltraDropDownBase)CmbContractNo).SelectedRow.Cells["InvoiceId"].Value);
				txtInvoiceDate.Value = Conversion.ToDateTime(((UltraDropDownBase)CmbContractNo).SelectedRow.Cells["InvoiceDate"].Value);
			}
			else
			{
				((Control)(object)cmbInvoiceNo).Text = "";
				txtContractDate.Value = DateTime.Now;
				txtInvoiceDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbInvoiceNo).ActiveRow != null && Conversion.ToInt64(cmbInvoiceNo.Value) > 0)
			{
				txtInvoiceDate.Value = Conversion.ToDateTime(((UltraDropDownBase)cmbInvoiceNo).SelectedRow.Cells["InvoiceDate"].Value);
				CmbContractNo.Value = Conversion.ToInt(((UltraDropDownBase)cmbInvoiceNo).SelectedRow.Cells["ContractId"].Value);
				txtContractDate.Value = Conversion.ToDateTime(((UltraDropDownBase)cmbInvoiceNo).SelectedRow.Cells["ContractDate"].Value);
			}
			else
			{
				((Control)(object)CmbContractNo).Text = "";
				txtContractDate.Value = DateTime.Now;
				txtInvoiceDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormReset()
	{
		AT = new Attachment();
		Attachment.dtAttachments.Rows.Clear();
		LstRemoveRecordDetail.Clear();
		LstRemoveRecordPaymentTerm.Clear();
		LstRemoveRecordOtherItem.Clear();
		AttachmentsValues = "";
		CustomAttachmentsValues = "";
		RowVersionLong = 0L;
		AppActionId = 0;
		btnsave.Visible = true;
		btnupdate.Visible = false;
		UpdateMode = false;
		RecId = 0L;
		UpdateDocumentNoUI(generateCode());
		((UltraGridBase)cmbStatus).Rows[0].Activate();
		((Control)(object)cmbStatus).Enabled = false;
		((Control)(object)CmbImporterName).Text = string.Empty;
		CmbExporterName.Value = 0;
		cmbSupCust_Leave(null, null);
		cmbConsignee.Value = 0;
		((Control)(object)CmbPerformaNo).Text = string.Empty;
		txtPerformaDate.Value = DateTime.Now;
		BindProformaNo();
		((Control)(object)CmbContractNo).Text = string.Empty;
		txtContractDate.Value = DateTime.Now;
		((Control)(object)cmbInvoiceNo).Text = string.Empty;
		txtInvoiceDate.Value = DateTime.Now;
		BindInvoiceAndLcOrderNo();
		txtLotRef.Text = string.Empty;
		((Control)(object)CmbPerformaNo).Enabled = true;
		((Control)(object)CmbContractNo).Enabled = true;
		((Control)(object)cmbInvoiceNo).Enabled = true;
		CmbIncomeTerm.Value = 0;
		CmbPaymentTermsNew.Value = 0;
		CmbFcyCode.Value = 0;
		txtExRate.Text = string.Empty;
		txtfcyAmountHeader.Text = string.Empty;
		txtNoOfPackagesHeader.Text = string.Empty;
		txtNoOfContainerHeader.Text = string.Empty;
		txtGrossWeightHeader.Text = string.Empty;
		txtNetWeightHeader.Text = string.Empty;
		cmbimporterBankNew.Value = 0;
		cmbexporterBankNew.Value = 0;
		cmbLoadingPort.Value = 0;
		cmbDestinationPort.Value = 0;
		txtBLNo.Text = string.Empty;
		txtBlDate.Value = DateTime.Now;
		txtGdNo.Text = string.Empty;
		txtGdDate.Value = DateTime.Now;
		txtGdValue.Text = string.Empty;
		dtdetail.Rows.Clear();
		grdDetail.ClearStructure();
		grdDetail.DataSource = dtdetail;
		grdDetail.RetrieveStructure();
		grdSettings();
		dtPaymentTerm.Rows.Clear();
		grdpaymentdetail.ClearStructure();
		grdpaymentdetail.DataSource = dtPaymentTerm;
		grdpaymentdetail.RetrieveStructure();
		grdSettingsPaymentTerm();
		dtOtherItem.Rows.Clear();
		grdotheritems.ClearStructure();
		grdotheritems.DataSource = dtOtherItem;
		grdotheritems.RetrieveStructure();
		grdSettingsOtherItems();
		DetailFormReset();
		ResePaymentDetails();
		ResetOtherItems();
		if (BranchFeature)
		{
			CmbBranchName.Focus();
		}
		else
		{
			CmbExporterName.Focus();
		}
	}

	private void DetailFormReset()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbItemName).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtCostMTon.Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtitemDescription.Text = string.Empty;
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			tabControl2.SelectedIndex = 0;
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
			updateDetailIndexPT = -1;
			CmbPaymentTermPaymentDetail.Value = 0;
			CmbFinInstrumentsPaymentDetail.Value = 0;
			dtxtpercentoftotal.Text = Conversion.ToInt((object)0).ToString();
			txtFcyAmountPaymentDetail.Text = Conversion.ToInt((object)0).ToString();
			txtDueDaysPaymentDetail.Text = Conversion.ToInt((object)0).ToString();
			BtnAddPaymentDetail.Visible = true;
			BtnUpdatePaymentDetail.Visible = false;
			btnCancelPaymentDetail.Visible = false;
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
			updateDetailIndexOtherItem = -1;
			((Control)(object)CmbItemOtherItem).Text = string.Empty;
			txtQtyOtherItem.Text = string.Empty;
			txtRateOtherItem.Text = string.Empty;
			txtAmountOtherItem.Text = string.Empty;
			txtRemarksOtherItem.Text = string.Empty;
			BtnAddOtherItem.Visible = true;
			BtnUpdateOtherItem.Visible = false;
			BtnCancelOtherItem.Visible = false;
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
			List<string> allServices = new List<string> { "BranchesAllocatedToUser" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			BranchesFill();
			ProformaNoFill(BindProformaNo());
			InvoiceAndLcOrderNoFill(BindInvoiceAndLcOrderNo());
			ImporterAndExporterFill(BindImporterAndExporter());
			SalesPersonFill(BindSalesPerson());
			ImportRelatedComboFill(BindImportRelatedCombo());
			MultiCurrencyFill(BindMultiCurrency());
			ItemFill(CommonServices.ItemGetForComboServiceBind());
			JobLotFill(CommonServices.JobLotGetAllService());
			ImporterandExportBankFill(BindImporterandExportBank());
			DebitAccountFill(CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,10"));
			if (AllowExportMultiCompanies)
			{
				ImportCompaniesByCompanyIdBind(ExImLcOrder.GetExportCompaniesByCompany(UserAccount.OrganizationId, UserAccount.CompanyId));
			}
			BindOtherItems();
			BindFinancialInstrument();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmImportInvoice_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnoadd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grdotheritems")
			{
				CmbItemOtherItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grdotheritems")
			{
				CmbItemOtherItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndadd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grdpaymentdetail")
			{
				CmbPaymentTermPaymentDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndcancel_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grdpaymentdetail")
			{
				CmbPaymentTermPaymentDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "tabControl1")
			{
				CmbItemName.Focus();
			}
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

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows.Add(0, Conversion.ToInt(CmbItemName.Value), Conversion.ToString((object)((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Text), Conversion.ToString((object)((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Text), Conversion.ToString((object)txtitemDescription.Text.Trim()), Conversion.ToString(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text.Trim()), Conversion.ToDouble((object)txtQtyMTon.Text.Trim()), Conversion.ToString(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text.Trim()), Conversion.ToString((object)((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Text), Conversion.ToDouble((object)txtNoOfBags.Text.Trim()), Conversion.ToDouble((object)txtWeightDetail.Text.Trim()), Conversion.ToDouble((object)txtCostMTon.Text.Trim()), Conversion.ToString(CmbRateUom.Value), Conversion.ToString((object)((Control)(object)CmbRateUom).Text.Trim()), Conversion.ToString((object)((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Text), Conversion.ToDouble((object)txtAmount.Text), Conversion.ToString((object)txtRemarksDetail.Text.Trim()), 0, 0);
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				getTotalnetWeightFromGrid();
				ProportionateOtherItemAmount();
				DetailFormReset();
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
				dtdetail.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
				dtdetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbItemName).Text;
				dtdetail.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value;
				dtdetail.Rows[updateDetailIndex]["ItemDetail"] = txtitemDescription.Text;
				dtdetail.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				dtdetail.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
				dtdetail.Rows[updateDetailIndex]["Qty/M.Ton"] = txtQtyMTon.Text;
				dtdetail.Rows[updateDetailIndex]["PackSizeId"] = CmbPackUom.Value;
				dtdetail.Rows[updateDetailIndex]["PackSize"] = ((Control)(object)CmbPackUom).Text;
				dtdetail.Rows[updateDetailIndex]["PackEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value);
				dtdetail.Rows[updateDetailIndex]["NoOfBags"] = txtNoOfBags.Text;
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = txtWeightDetail.Text;
				dtdetail.Rows[updateDetailIndex]["Cost/M.Ton"] = txtCostMTon.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOMId"] = CmbRateUom.Value;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)CmbRateUom).Text;
				dtdetail.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells["Equivalent"].Value);
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
				grdDetail.DataSource = dtdetail;
				getTotalnetWeightFromGrid();
				ProportionateOtherItemAmount();
				DetailFormReset();
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
			DetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		try
		{
			grdDetail.AllowEdit = (InheritableBoolean)1;
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["PackSizeId"].Visible = false;
			grdDetail.RootTable.Columns["PackEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdDetail.RootTable.Columns["RateEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["RowVersionLong"].Visible = false;
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["ItemDetail"].Width = Constants.InventoryConstants.OtherDescription;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["Qty/M.Ton"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["PackEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["NoOfBags"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["Cost/M.Ton"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom();
				txtitemDescription.Text = item.Cells["ItemDetail"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value;
				CmbPackUom.Value = item.Cells["PackSizeId"].Value;
				txtQtyMTon.Text = item.Cells["Qty/M.Ton"].Value.ToString();
				txtNoOfBags.Text = item.Cells["NoOfBags"].Value.ToString();
				CmbRateUom.Value = item.Cells["RateUOMId"].Value;
				txtCostMTon.Text = item.Cells["Cost/M.Ton"].Value.ToString();
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				btnAddinGrid.Visible = false;
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
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
					DeleteDetailGridRow(r);
				}
				else if (e.Column.Key == "Add")
				{
					AddDetailGridRow(r);
				}
				grdDetail.UpdateData();
				getTotalnetWeightFromGrid();
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailGridRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (grdDetail.GetRows().Count() <= 1)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						invoiceDetail vd = new invoiceDetail();
						vd.invoiceDetailId = Conversion.ToInt64(r.Cells["Id"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.ItemDescription = Conversion.ToString((object)r.Cells["ItemDetail"].Text);
						vd.lotJobId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd.packSizeIdinner = Conversion.ToInt(r.Cells["PackSizeId"].Value);
						vd.packSizeIdOuter = Conversion.ToInt(r.Cells["PackSizeId"].Value);
						vd.qtyInner = Conversion.ToInt(r.Cells["NoOfBags"].Value);
						vd.qtyOuter = Conversion.ToInt(r.Cells["NoOfBags"].Value);
						vd.netWeightInner = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) / 1000.0;
						vd.netWeightOuter = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) / 1000.0;
						vd.ItemRate = Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value);
						vd.uomIdRate = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						vd.FcAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
						vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
						vd.isApproved = false;
						vd.createdOn = DateTime.Now;
						vd.createdUserId = UserAccount.ID;
						vd.lastModifiedOn = DateTime.Now;
						vd.lastModifiedUserId = UserAccount.ID;
						vd.approvedOn = DateTime.Now;
						vd.appActionId = 3;
						LstRemoveRecordDetail.Add(vd);
						dtdetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					r.Delete();
				}
				return;
			}
			throw new Exception("Reset the Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddDetailGridRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				DataRow dr = dtdetail.Rows[r.RowIndex];
				dtdetail.Rows.Add(dr.ItemArray);
				dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
				grdDetail.DataSource = dtdetail;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				_ = r.Cells.Count;
				_ = 0;
			}
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
			if (grdDetail.RowCount > 0)
			{
				object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
				object NoOfBagsTotal = grdDetail.GetTotal(grdDetail.RootTable.Columns["NoOfBags"], (AggregateFunction)2);
				double Amount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
				double NetWeightkg = Conversion.ToDouble(total);
				txtNetWeightHeader.Text = Conversion.ToDouble((object)NetWeightkg).ToString("#,##0.#####");
				txtNoOfPackagesHeader.Text = Conversion.ToDouble(NoOfBagsTotal).ToString("#,##0.#####");
				txtfcyAmountHeader.Text = Amount.ToString("#,##0.###");
			}
			else
			{
				txtNetWeightHeader.Text = "";
				txtfcyAmountHeader.Text = "";
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
				dtOtherItem.Rows.Add(0, CmbItemOtherItem.Value, ((Control)(object)CmbItemOtherItem).Text, Conversion.ToDouble((object)txtQtyOtherItem.Text.Trim()), Conversion.ToString((object)txtRateOtherItem.Text), Conversion.ToDouble((object)txtAmountOtherItem.Text.Trim()), txtRemarksOtherItem.Text.Trim(), 0);
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				ResetOtherItems();
				TotalAmountCalculate();
				ProportionateOtherItemAmount();
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
				updateDetailIndexOtherItem = item.RowIndex;
				CmbItemOtherItem.Value = item.Cells["ItemId"].Value.ToString();
				txtQtyOtherItem.Text = item.Cells["Qty"].Value.ToString();
				txtRateOtherItem.Text = item.Cells["Rate"].Value.ToString();
				txtAmountOtherItem.Text = item.Cells["Amount"].Value.ToString();
				txtRemarksOtherItem.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddOtherItem.Visible = false;
				BtnUpdateOtherItem.Visible = true;
				BtnCancelOtherItem.Visible = true;
				CmbItemOtherItem.Focus();
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
				dtOtherItem.Rows[updateDetailIndexOtherItem]["ItemId"] = CmbItemOtherItem.Value;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["ItemName"] = ((Control)(object)CmbItemOtherItem).Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Qty"] = txtQtyOtherItem.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Rate"] = txtRateOtherItem.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Amount"] = txtAmountOtherItem.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Remarks"] = txtRemarksOtherItem.Text;
				grdotheritems.DataSource = dtOtherItem;
				BtnAddOtherItem.Visible = true;
				BtnUpdateOtherItem.Visible = false;
				BtnCancelOtherItem.Visible = false;
				ResetOtherItems();
				txtQtyOtherItem.Focus();
				TotalAmountCalculate();
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ResetOtherItems();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.RootTable.Columns["Id"].Visible = false;
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["RowVersionLong"].Visible = false;
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
			grdotheritems.RootTable.Columns["ItemName"].Width = 350;
			grdotheritems.RootTable.Columns["Remarks"].Width = 350;
			grdotheritems.RootTable.Columns.Add("Delete");
			grdotheritems.RootTable.Columns["Delete"].Key = "Delete";
			grdotheritems.RootTable.Columns["Delete"].Caption = "X";
			grdotheritems.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdotheritems.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdotheritems.RootTable.Columns["Delete"].ButtonText = "X";
			grdotheritems.RootTable.Columns["Delete"].Width = 20;
			grdotheritems.RootTable.Columns["Delete"].Position = 0;
			grdotheritems.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdotheritems.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteOtherItemDetailGridRow(r);
				}
				grdotheritems.UpdateData();
				TotalAmountCalculate();
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOtherItemDetailGridRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexOtherItem == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						invoiceOtherItem vd = new invoiceOtherItem();
						vd.invoiceOtherItemId = Conversion.ToInt64(r.Cells["Id"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.ItemQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						vd.ItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
						vd.ItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.isApproved = false;
						vd.createdOn = DateTime.Now;
						vd.createdUserId = UserAccount.ID;
						vd.lastModifiedOn = DateTime.Now;
						vd.lastModifiedUserId = UserAccount.ID;
						vd.approvedOn = DateTime.Now;
						vd.appActionId = 3;
						LstRemoveRecordOtherItem.Add(vd);
						dtOtherItem.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					r.Delete();
				}
				return;
			}
			throw new Exception("Reset the Detail First");
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
				if (Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) > 0 && Conversion.ToInt(item.Cells["ExImEFormRegistrationId"].Value) == Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) && Conversion.ToInt(item.Cells["DocumentTypeId"].Value) == Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPaymentDetail).SelectedRow.Cells[3].Value))
				{
					throw new Exception("Financial Instrument No already add in Grid Please Check");
				}
			}
			dtPaymentTerm.Rows.Add(0, CmbPaymentTermPaymentDetail.Value, ((Control)(object)CmbPaymentTermPaymentDetail).Text, (Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) > 0) ? ((UltraDropDownBase)CmbFinInstrumentsPaymentDetail).SelectedRow.Cells[3].Value : ((object)0), CmbFinInstrumentsPaymentDetail.Value, (Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) > 0) ? ((Control)(object)CmbFinInstrumentsPaymentDetail).Text : "", Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim()), Conversion.ToDouble((object)txtFcyAmountPaymentDetail.Text.Trim()), Conversion.ToDouble((object)txtDueDaysPaymentDetail.Text.Trim()), Conversion.ToString((object)txtRemarksPaymentDetail.Text.Trim()), 0);
			grdpaymentdetail.DataSource = dtPaymentTerm;
			grdpaymentdetail.RetrieveStructure();
			grdSettingsPaymentTerm();
			ResePaymentDetails();
			CmbPaymentTermPaymentDetail.Focus();
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
				if (((UltraGridBase)CmbFinInstrumentsPaymentDetail).ActiveRow != null && Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) > 0 && updateDetailIndexPT != Conversion.ToInt((object)item.RowIndex) && Conversion.ToInt(item.Cells["ExImEFormRegistrationId"].Value) == Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) && Conversion.ToInt(item.Cells["DocumentTypeId"].Value) == Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPaymentDetail).SelectedRow.Cells["DocumentTypeId"].Value))
				{
					throw new Exception("Financial Instrument No already add in Grid Please Check");
				}
			}
			dtPaymentTerm.Rows[updateDetailIndexPT]["PaymentTermId"] = CmbPaymentTermPaymentDetail.Value;
			dtPaymentTerm.Rows[updateDetailIndexPT]["PaymentTerm"] = ((Control)(object)CmbPaymentTermPaymentDetail).Text;
			dtPaymentTerm.Rows[updateDetailIndexPT]["DocumentTypeId"] = ((Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) > 0) ? ((UltraDropDownBase)CmbFinInstrumentsPaymentDetail).SelectedRow.Cells["DocumentTypeId"].Value : ((object)0));
			dtPaymentTerm.Rows[updateDetailIndexPT]["ExImEFormRegistrationId"] = Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value);
			dtPaymentTerm.Rows[updateDetailIndexPT]["FinancialInstrumentNo"] = ((Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) > 0) ? ((Control)(object)CmbFinInstrumentsPaymentDetail).Text : "");
			dtPaymentTerm.Rows[updateDetailIndexPT]["%OfTotal"] = dtxtpercentoftotal.Text.Trim();
			dtPaymentTerm.Rows[updateDetailIndexPT]["FcyAmount"] = txtFcyAmountPaymentDetail.Text.Trim();
			dtPaymentTerm.Rows[updateDetailIndexPT]["DueDays"] = txtDueDaysPaymentDetail.Text;
			dtPaymentTerm.Rows[updateDetailIndexPT]["Remarks"] = txtRemarksPaymentDetail.Text;
			grdpaymentdetail.DataSource = dtPaymentTerm;
			BtnAddPaymentDetail.Visible = true;
			BtnUpdatePaymentDetail.Visible = false;
			btnCancelPaymentDetail.Visible = false;
			ResePaymentDetails();
			CmbPaymentTermPaymentDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndcancel_Click(object sender, EventArgs e)
	{
		try
		{
			ResePaymentDetails();
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
				updateDetailIndexPT = item.RowIndex;
				CmbPaymentTermPaymentDetail.Value = item.Cells["PaymentTermId"].Value.ToString();
				CmbFinInstrumentsPaymentDetail.Value = item.Cells["ExImEFormRegistrationId"].Value.ToString();
				dtxtpercentoftotal.Text = item.Cells["%OfTotal"].Value.ToString();
				txtFcyAmountPaymentDetail.Text = item.Cells["FcyAmount"].Value.ToString();
				txtDueDaysPaymentDetail.Text = item.Cells["DueDays"].Value.ToString();
				txtRemarksPaymentDetail.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddPaymentDetail.Visible = false;
				BtnUpdatePaymentDetail.Visible = true;
				btnCancelPaymentDetail.Visible = true;
				CmbPaymentTermPaymentDetail.Focus();
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
			grdpaymentdetail.RootTable.Columns["Id"].Visible = false;
			grdpaymentdetail.RootTable.Columns["RowVersionLong"].Visible = false;
			grdpaymentdetail.RootTable.Columns["FinancialInstrumentNo"].Width = 350;
			grdpaymentdetail.RootTable.Columns["PaymentTerm"].Width = 350;
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

	private void grdpaymentdetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdpaymentdetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeletePaymentItemDetailGridRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePaymentItemDetailGridRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexPT == -1)
			{
				if (grdpaymentdetail.GetRows().Count() <= 1)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						invoicePaymentTerm vd = new invoicePaymentTerm();
						vd.InvoicePaymentTermId = Conversion.ToInt64(r.Cells["Id"].Value);
						vd.paymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
						vd.DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
						vd.FinancialInstrumentId = Conversion.ToInt(r.Cells["ExImEFormRegistrationId"].Value);
						vd.FinancialInstrumentNo = Conversion.ToString((object)r.Cells["FinancialInstrumentNo"].Text);
						vd.pctOfTotal = Conversion.ToDouble(r.Cells["%OfTotal"].Value);
						vd.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
						vd.FcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
						vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.SortNo = r.RowIndex + 1;
						vd.isApproved = false;
						vd.createdOn = DateTime.Now;
						vd.createdUserId = UserAccount.ID;
						vd.lastModifiedOn = DateTime.Now;
						vd.lastModifiedUserId = UserAccount.ID;
						vd.approvedOn = DateTime.Now;
						vd.appActionId = 3;
						LstRemoveRecordPaymentTerm.Add(vd);
						dtOtherItem.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					r.Delete();
				}
				return;
			}
			throw new Exception("Reset the Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (BranchFeature && (((UltraGridBase)CmbBranchName).ActiveRow == null || Conversion.ToInt(CmbBranchName.Value) == 0))
		{
			MessageBox.Show("BranchName Is required");
			CmbBranchName.Focus();
			return false;
		}
		if (txtDocNo.Text == "" || Conversion.ToInt((object)txtDocNo.Text) == 0)
		{
			MessageBox.Show("Doc No Is Required");
			txtDocNo.Focus();
			return false;
		}
		if ((btnupdate.Visible && Conversion.ToString((object)((Control)(object)cmbStatus).Text) == string.Empty) || ((Control)(object)cmbStatus).Text == "0")
		{
			MessageBox.Show("Status Is Required");
			cmbStatus.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbImporterName.Value) == 0)
		{
			MessageBox.Show("Importer Is Required");
			CmbImporterName.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbExporterName.Value) == 0)
		{
			MessageBox.Show("Exporter Is Required");
			CmbExporterName.Focus();
			return false;
		}
		if (Conversion.ToString((object)((Control)(object)CmbContractNo).Text) == string.Empty || ((Control)(object)CmbContractNo).Text == "")
		{
			MessageBox.Show("ContractNo No Is Required");
			CmbContractNo.Focus();
			return false;
		}
		if (Conversion.ToString((object)((Control)(object)cmbInvoiceNo).Text) == string.Empty || ((Control)(object)cmbInvoiceNo).Text == "")
		{
			MessageBox.Show("Invoice No Is Required");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbConsignee.Value) == 0)
		{
			MessageBox.Show("Consignee Is Required");
			cmbConsignee.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTermsNew.Value) == 0)
		{
			MessageBox.Show("Payment Term Is Required");
			CmbPaymentTermsNew.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbLoadingPort.Value) == 0)
		{
			MessageBox.Show("Loading Port Is Required");
			cmbLoadingPort.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbDestinationPort.Value) == 0)
		{
			MessageBox.Show("Destination Port  Is Required");
			cmbDestinationPort.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbFcyCode.Value) == 0)
		{
			MessageBox.Show("Fcy Code Is Required");
			CmbFcyCode.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfcyAmountHeader.Text) == 0.0)
		{
			MessageBox.Show("Fcy Amount  Is Required");
			txtfcyAmountHeader.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtNoOfContainerHeader.Text) == 0)
		{
			MessageBox.Show("No.of Containers Field Is Required");
			txtNoOfContainerHeader.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtNetWeightHeader.Text) > 0.0))
		{
			MessageBox.Show("Net Weight  Is Required");
			txtNetWeightHeader.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtGrossWeightHeader.Text) >= Conversion.ToDouble((object)txtNetWeightHeader.Text)))
		{
			MessageBox.Show("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
			txtGrossWeightHeader.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtExRate.Text) > 0.0))
		{
			MessageBox.Show("ExchangeRate Field is Required");
			txtExRate.Focus();
			return false;
		}
		if (FinancialisActiveonCommercialInvoice && Conversion.ToInt(CmbDabitAccount.Value) == 0 && MessageBox.Show("You have not Selected Debit Account,Are you sure to Debit Value to Purchase Account?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
		{
			MessageBox.Show("Please select Debit Ac");
			CmbDabitAccount.Focus();
			return false;
		}
		if (AllowExportMultiCompanies && (((UltraGridBase)CmbImportCompany).ActiveRow == null || Conversion.ToInt(CmbImportCompany.Value) == 0))
		{
			MessageBox.Show("Export Company Is required");
			CmbImportCompany.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("Item field required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot field required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack uom field required");
			CmbPackUom.Focus();
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
		if (txtCostMTon.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtCostMTon.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Cost/M.Ton field required");
			txtCostMTon.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
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
		return true;
	}

	private bool OtherItemFormValidation()
	{
		if (Conversion.ToInt(CmbItemOtherItem.Value) == 0)
		{
			MessageBox.Show("Item field required");
			CmbItemOtherItem.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRateOtherItem.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtRateOtherItem.Focus();
			return false;
		}
		return true;
	}

	private bool PaymentDetailFormValidation()
	{
		if (Conversion.ToInt(CmbPaymentTermPaymentDetail.Value) == 0)
		{
			MessageBox.Show("Payment Term field required");
			CmbPaymentTermPaymentDetail.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTermPaymentDetail.Value) == 1 && (((UltraGridBase)CmbFinInstrumentsPaymentDetail).ActiveRow == null || Conversion.ToInt(CmbFinInstrumentsPaymentDetail.Value) == 0))
		{
			throw new Exception("Financial Instrument field is required against Advance");
		}
		if (Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim()) == 0.0 || dtxtpercentoftotal.Text == "")
		{
			MessageBox.Show("Percent Field Required");
			dtxtpercentoftotal.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFcyAmountPaymentDetail.Text.Trim()) == 0.0 || txtFcyAmountPaymentDetail.Text == "")
		{
			MessageBox.Show("Fcy Amount  Field Required");
			txtFcyAmountPaymentDetail.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Expected O, but got Unknown
		//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Expected O, but got Unknown
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Expected O, but got Unknown
		//IL_0cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Expected O, but got Unknown
		//IL_0d8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d93: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() <= 0)
			{
				tabControl2.SelectedIndex = 0;
				throw new Exception("Detail Grid Record Not Found. Please Check! ");
			}
			if (grdpaymentdetail.RowCount == 0)
			{
				tabControl2.SelectedIndex = 2;
				CmbPaymentTermPaymentDetail.Focus();
				throw new Exception("Payment Detail Not Found. Please Check! ");
			}
			invoiceMaster PIH = new invoiceMaster();
			if (RecId == 0L)
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
			PIH.OrganizationId = UserAccount.OrganizationId;
			PIH.CompanyId = UserAccount.CompanyId;
			PIH.BranchesId = UserAccount.BranchesId;
			PIH.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PIH.ProjectsId = UserAccount.BranchesId;
			PIH.documentTypeId = 902;
			PIH.AppActionId = AppActionId;
			PIH.RowVersionLong = RowVersionLong;
			PIH.isApproved = false;
			PIH.createdOn = DateTime.Now;
			PIH.createdUserId = UserAccount.ID;
			PIH.lastModifiedOn = DateTime.Now;
			PIH.lastModifiedUserId = UserAccount.ID;
			PIH.approvedOn = DateTime.Now;
			PIH.docNo = Conversion.ToInt((object)txtDocNo.Text);
			PIH.docStatus = Conversion.ToString((object)((Control)(object)cmbStatus).Text);
			PIH.importerId = Conversion.ToInt(CmbImporterName.Value);
			PIH.exporterId = Conversion.ToInt(CmbExporterName.Value);
			PIH.salePersonId = Conversion.ToInt(CmbSalesPerson.Value);
			PIH.ConsigneeId = Conversion.ToInt(cmbConsignee.Value);
			PIH.proformaMasterId = Conversion.ToInt64(CmbPerformaNo.Value);
			PIH.proformaMasterDate = Conversion.ToDateTime((object)txtPerformaDate.Value);
			PIH.lcOrderMasterDate = Conversion.ToDateTime((object)txtPerformaDate.Value);
			PIH.lcOrderMasterId = Conversion.ToInt64(cmbInvoiceNo.Value);
			PIH.invoiceMasterNo = Conversion.ToString((object)((Control)(object)cmbInvoiceNo).Text.Trim());
			PIH.invoiceMasterId = Conversion.ToInt(cmbInvoiceNo.Value);
			PIH.docDate = Conversion.ToDateTime((object)txtInvoiceDate.Value);
			PIH.importerRefNo = Conversion.ToString((object)txtLotRef.Text);
			PIH.incoTermId = Conversion.ToInt(CmbIncomeTerm.Value);
			PIH.payementTermId = Conversion.ToInt(CmbPaymentTermsNew.Value);
			PIH.currencyId = Conversion.ToInt(CmbFcyCode.Value);
			PIH.exchangeRate = Conversion.ToDecimal((object)txtExRate.Text);
			PIH.fcyAmountTotal = Conversion.ToDouble((object)txtfcyAmountHeader.Text.Trim());
			PIH.NoOfPackages = Conversion.ToDouble((object)txtNoOfBags.Text.Trim());
			PIH.fclTotal = Conversion.ToInt((object)txtNoOfContainerHeader.Text.Trim());
			PIH.grossWeightTotal = Conversion.ToDouble((object)txtGrossWeightHeader.Text.Trim()) * 1000.0;
			PIH.netWeightTotal = Conversion.ToDouble((object)txtNetWeightHeader.Text) * 1000.0;
			PIH.bankIdImporter = Conversion.ToInt(cmbimporterBankNew.Value);
			PIH.bankIdExporter = Conversion.ToInt(cmbexporterBankNew.Value);
			PIH.loadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			PIH.destinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			PIH.BillOfLadingDate = Conversion.ToDateTime((object)txtBlDate.Value);
			PIH.BillOfLadingNo = Conversion.ToString((object)txtBLNo.Text);
			PIH.GoodsDeclarationDate = Conversion.ToDateTime((object)txtGdDate.Value);
			PIH.GoodsDeclarationNo = Conversion.ToString((object)txtGdNo.Text);
			PIH.GoodsDeclarationValue = Conversion.ToDouble((object)txtGdValue.Text);
			if (FinancialisActiveonCommercialInvoice)
			{
				PIH.DabitAccountId = Conversion.ToInt(CmbDabitAccount.Value);
			}
			PIH.legalEntityId = Conversion.ToInt(CmbImportCompany.Value);
			PIH.invoiceDetailList = new List<invoiceDetail>();
			PIH.invoicePaymentTermList = new List<invoicePaymentTerm>();
			PIH.invoiceOtherItemList = new List<invoiceOtherItem>();
			PIH.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			if (LstRemoveRecordDetail.Count > 0)
			{
				for (int l = 0; l < LstRemoveRecordDetail.Count; l++)
				{
					PIH.invoiceDetailList.Add(LstRemoveRecordDetail[l]);
				}
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				invoiceDetail vd2 = new invoiceDetail();
				vd2.invoiceDetailId = ((RecId == 0L) ? 0 : Conversion.ToInt64(r3.Cells["Id"].Value));
				vd2.appActionId = ((vd2.invoiceDetailId <= 0) ? 1 : 2);
				vd2.LocOrderMasterId = Conversion.ToInt(CmbContractNo.Value);
				vd2.locOrderDate = Conversion.ToDateTime((object)txtContractDate.Value);
				vd2.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				vd2.ItemDescription = Conversion.ToString((object)r3.Cells["ItemDetail"].Text);
				vd2.lotJobId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
				vd2.packSizeIdinner = Conversion.ToInt(r3.Cells["PackSizeId"].Value);
				vd2.packSizeIdOuter = Conversion.ToInt(r3.Cells["PackSizeId"].Value);
				vd2.qtyInner = Conversion.ToInt(r3.Cells["NoOfBags"].Value);
				vd2.qtyOuter = Conversion.ToInt(r3.Cells["NoOfBags"].Value);
				vd2.netWeightInner = Conversion.ToDouble(r3.Cells["Qty/M.Ton"].Value) * 1000.0;
				vd2.netWeightOuter = Conversion.ToDouble(r3.Cells["Qty/M.Ton"].Value) * 1000.0;
				vd2.NetWeightKgs = Conversion.ToDouble(r3.Cells["Qty/M.Ton"].Value) * 1000.0;
				vd2.ItemRate = Conversion.ToDouble(r3.Cells["Cost/M.Ton"].Value);
				vd2.uomIdRate = Conversion.ToInt(r3.Cells["RateUOMId"].Value);
				vd2.FcAmount = Conversion.ToDouble(r3.Cells["Amount"].Value);
				vd2.RemarksDetail = Conversion.ToString((object)r3.Cells["Remarks"].Text);
				vd2.isApproved = false;
				vd2.createdOn = DateTime.Now;
				vd2.createdUserId = UserAccount.ID;
				vd2.lastModifiedOn = DateTime.Now;
				vd2.lastModifiedUserId = UserAccount.ID;
				vd2.approvedOn = DateTime.Now;
				vd2.RowVersionLong = Conversion.ToInt64(r3.Cells["RowVersionLong"].Value);
				PIH.invoiceDetailList.Add(vd2);
			}
			if (LstRemoveRecordOtherItem.Count > 0)
			{
				for (int k = 0; k < LstRemoveRecordOtherItem.Count; k++)
				{
					PIH.invoiceOtherItemList.Add(LstRemoveRecordOtherItem[k]);
				}
			}
			if (grdotheritems.RowCount > 0)
			{
				rows = grdotheritems.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					invoiceOtherItem vd = new invoiceOtherItem();
					vd.invoiceOtherItemId = ((RecId == 0L) ? 0 : Conversion.ToInt64(r2.Cells["Id"].Value));
					vd.appActionId = ((vd.invoiceOtherItemId <= 0) ? 1 : 2);
					vd.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					vd.ItemQty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
					vd.ItemRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
					vd.ItemAmount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
					vd.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					vd.isApproved = false;
					vd.createdOn = DateTime.Now;
					vd.createdUserId = UserAccount.ID;
					vd.lastModifiedOn = DateTime.Now;
					vd.lastModifiedUserId = UserAccount.ID;
					vd.approvedOn = DateTime.Now;
					vd.RowVersionLong = Conversion.ToInt64(r2.Cells["RowVersionLong"].Value);
					PIH.invoiceOtherItemList.Add(vd);
				}
			}
			double UtilizeAmount = 0.0;
			if (LstRemoveRecordPaymentTerm.Count > 0)
			{
				for (int j = 0; j < LstRemoveRecordPaymentTerm.Count; j++)
				{
					PIH.invoicePaymentTermList.Add(LstRemoveRecordPaymentTerm[j]);
				}
			}
			rows = grdpaymentdetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				invoicePaymentTerm vdp = new invoicePaymentTerm();
				vdp.InvoicePaymentTermId = ((RecId == 0L) ? 0 : Conversion.ToInt64(r.Cells["Id"].Value));
				vdp.appActionId = ((vdp.InvoicePaymentTermId <= 0) ? 1 : 2);
				vdp.paymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
				vdp.DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				vdp.FinancialInstrumentId = Conversion.ToInt(r.Cells["ExImEFormRegistrationId"].Value);
				vdp.FinancialInstrumentNo = Conversion.ToString((object)r.Cells["FinancialInstrumentNo"].Text);
				vdp.pctOfTotal = Conversion.ToDouble(r.Cells["%OfTotal"].Value);
				vdp.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
				vdp.FcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
				vdp.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
				UtilizeAmount += vdp.FcyAmount;
				vdp.SortNo = r.RowIndex + 1;
				vdp.isApproved = false;
				vdp.createdOn = DateTime.Now;
				vdp.createdUserId = UserAccount.ID;
				vdp.lastModifiedOn = DateTime.Now;
				vdp.lastModifiedUserId = UserAccount.ID;
				vdp.approvedOn = DateTime.Now;
				vdp.RowVersionLong = Conversion.ToInt64(r.Cells["RowVersionLong"].Value);
				PIH.invoicePaymentTermList.Add(vdp);
			}
			if (grdpaymentdetail.GetRows().Length != 0 && Math.Round(PIH.fcyAmountTotal, 0) != Math.Round(UtilizeAmount, 0))
			{
				throw new Exception("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.UploadedFileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
				attach.ScreenName = base.Name;
				PIH.AttachmentsList.Add(attach);
			}
			PIH.AttachmentsValues = AttachmentsValues;
			PIH.CustomAttachmentsValues = CustomAttachmentsValues;
			if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				PIH.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				for (int m = 0; m < AT.RemovedAttachmentListInUpdateCase.Count; m++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[m].Path);
					delete.ScreenName = base.Name;
					PIH.DeleteAttachmentsList.Add(delete);
				}
			}
			object success = invoiceMaster.Save(PIH, (ParameterObject)null);
			if (RecId == 0L)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (RecId > 0)
			{
				MessageBox.Show("Update SuccessFully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(Conversion.ToInt((object)RecId), base.Name, 902, 0);
				}
			}
			FormReset();
			if (ChkPrintPreview.Checked)
			{
				InvoiceSlip(Conversion.ToInt64(success));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0L;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0L)
			{
				throw new Exception("Rec Id not found...");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(long Id)
	{
		try
		{
			FormReset();
			btnsave.Visible = false;
			btnupdate.Visible = true;
			RecId = Id;
			invoiceMaster PIH = invoiceMaster.ReadById(RecId);
			if (PIH != null)
			{
				tabControl1.SelectedIndex = 0;
				((Control)(object)CmbPerformaNo).Enabled = false;
				txtPerformaDate.Enabled = false;
				((Control)(object)CmbContractNo).Enabled = false;
				txtContractDate.Enabled = false;
				((Control)(object)cmbInvoiceNo).Enabled = false;
				txtInvoiceDate.Enabled = false;
				txtDocNo.Text = PIH.docNo.ToString();
				CmbImporterName.Value = PIH.importerId;
				CmbExporterName.Value = PIH.exporterId;
				cmbSupCust_Leave(null, null);
				CmbSalesPerson.Value = PIH.salePersonId;
				cmbConsignee.Value = PIH.ConsigneeId;
				CmbPerformaNo.Value = PIH.proformaMasterId;
				txtPerformaDate.Value = PIH.proformaMasterDate;
				cmbInvoiceNo.Value = PIH.invoiceMasterId;
				txtInvoiceDate.Value = Conversion.ToDateTime((object)PIH.docDate);
				if (PIH.invoiceDetailList != null && PIH.invoiceDetailList.Count > 0)
				{
					CmbBranchName.Value = PIH.invoiceDetailList[0].locationBranchId;
					CmbContractNo.Value = PIH.invoiceDetailList[0].LocOrderMasterId;
					if (!Conversion.CheckDateTimeNull(PIH.invoiceDetailList[0].locOrderDate))
					{
						txtContractDate.Value = PIH.invoiceDetailList[0].locOrderDate;
					}
				}
				txtLotRef.Text = PIH.importerRefNo;
				CmbIncomeTerm.Value = PIH.incoTermId;
				CmbPaymentTermsNew.Value = PIH.payementTermId;
				CmbFcyCode.Value = PIH.currencyId;
				txtExRate.Text = PIH.exchangeRate.ToString("#,##0.####");
				txtfcyAmountHeader.Text = PIH.fcyAmountTotal.ToString("#,##0.####");
				txtNoOfPackagesHeader.Text = PIH.NoOfPackages.ToString("#,##0.####");
				txtNoOfContainerHeader.Text = PIH.fclTotal.ToString();
				txtGrossWeightHeader.Text = (PIH.grossWeightTotal / 1000.0).ToString("#,##0.####");
				txtNetWeightHeader.Text = (PIH.netWeightTotal / 1000.0).ToString("#,##0.####");
				cmbimporterBankNew.Value = PIH.bankIdImporter;
				cmbexporterBankNew.Value = PIH.bankIdExporter;
				cmbLoadingPort.Value = PIH.loadingPortId;
				cmbDestinationPort.Value = PIH.destinationPortId;
				txtBLNo.Text = PIH.BillOfLadingNo;
				txtBlDate.Value = PIH.BillOfLadingDate;
				txtGdNo.Text = PIH.GoodsDeclarationNo;
				txtGdDate.Value = PIH.GoodsDeclarationDate;
				txtGdValue.Text = PIH.GoodsDeclarationValue.ToString("#,##0.####");
				if (FinancialisActiveonCommercialInvoice)
				{
					CmbDabitAccount.Value = PIH.DabitAccountId;
				}
				CmbImportCompany.Value = PIH.legalEntityId;
				AttachmentsValues = PIH.AttachmentsValues;
				CustomAttachmentsValues = PIH.CustomAttachmentsValues;
				RowVersionLong = PIH.RowVersionLong;
				AppActionId = PIH.AppActionId;
				dtdetail.Rows.Clear();
				for (int k = 0; k < PIH.invoiceDetailList.Count; k++)
				{
					dtdetail.Rows.Add(PIH.invoiceDetailList[k].invoiceDetailId, PIH.invoiceDetailList[k].ItemId, PIH.invoiceDetailList[k].ItemName, PIH.invoiceDetailList[k].ItemCode, PIH.invoiceDetailList[k].ItemDescription, PIH.invoiceDetailList[k].lotJobId, PIH.invoiceDetailList[k].JobLotDescription, PIH.invoiceDetailList[k].netWeightOuter / 1000.0, PIH.invoiceDetailList[k].packSizeIdOuter, PIH.invoiceDetailList[k].OuterUOM, PIH.invoiceDetailList[k].OuterEquivalent, PIH.invoiceDetailList[k].qtyOuter, PIH.invoiceDetailList[k].netWeightOuter, PIH.invoiceDetailList[k].ItemRate, PIH.invoiceDetailList[k].uomIdRate, PIH.invoiceDetailList[k].RateUOM, PIH.invoiceDetailList[k].RateEquivalent, PIH.invoiceDetailList[k].FcAmount, PIH.invoiceDetailList[k].RemarksDetail, PIH.invoiceDetailList[k].RowVersionLong);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				dtOtherItem.Rows.Clear();
				for (int j = 0; j < PIH.invoiceOtherItemList.Count; j++)
				{
					dtOtherItem.Rows.Add(PIH.invoiceOtherItemList[j].invoiceOtherItemId, PIH.invoiceOtherItemList[j].ItemId, PIH.invoiceOtherItemList[j].ItemName, PIH.invoiceOtherItemList[j].ItemQty, PIH.invoiceOtherItemList[j].ItemRate, PIH.invoiceOtherItemList[j].ItemAmount, PIH.invoiceOtherItemList[j].Remarks, PIH.invoiceOtherItemList[j].RowVersionLong);
				}
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				dtPaymentTerm.Rows.Clear();
				for (int i = 0; i < PIH.invoicePaymentTermList.Count; i++)
				{
					dtPaymentTerm.Rows.Add(PIH.invoicePaymentTermList[i].InvoicePaymentTermId, PIH.invoicePaymentTermList[i].paymentTermId, PIH.invoicePaymentTermList[i].PaymentTerm, PIH.invoicePaymentTermList[i].DocumentTypeId, PIH.invoicePaymentTermList[i].FinancialInstrumentId, PIH.invoicePaymentTermList[i].FinancialInstrumentNo, PIH.invoicePaymentTermList[i].pctOfTotal, PIH.invoicePaymentTermList[i].FcyAmount, PIH.invoicePaymentTermList[i].DueDays, PIH.invoicePaymentTermList[i].Remarks, PIH.invoicePaymentTermList[i].RowVersionLong);
				}
				grdpaymentdetail.DataSource = dtPaymentTerm;
				grdpaymentdetail.RetrieveStructure();
				grdSettingsPaymentTerm();
				List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(Conversion.ToInt((object)RecId), base.Name);
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
				ProportionateOtherItemAmount();
				UpdateMode = true;
				CmbImporterName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BindFinancialInstrument();
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				ReadById(Conversion.ToInt64(r.Cells["Id"].Value));
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
		else if (BranchFeature)
		{
			CmbBranchName.Focus();
		}
		else
		{
			CmbExporterName.Focus();
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
			dtCustomer.Columns.Add("name", typeof(string));
			DataTable dt = invoiceMaster.GetDataForDropDownFrom_Invoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ExporterName")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "name", "Exporter Name", ZeroIndex: false);
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
			Obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			Obj.EntryUser = UserAccount.ID;
			Obj.DocumentTypeId = 902;
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
			DataTable dtgrd = invoiceMaster.SEARCHHistory(Obj);
			if (dtgrd.Rows.Count > 0)
			{
				DataTable dtInvoiceDocuments = new DataTable();
				dtInvoiceDocuments.Columns.Add("Id", typeof(int));
				dtInvoiceDocuments.Columns.Add("DocNo", typeof(string));
				dtInvoiceDocuments.Columns.Add("InvoiceDate", typeof(DateTime));
				dtInvoiceDocuments.Columns.Add("InvoiceNo", typeof(string));
				dtInvoiceDocuments.Columns.Add("ProformaMasterId", typeof(int));
				dtInvoiceDocuments.Columns.Add("ProformaNo", typeof(string));
				dtInvoiceDocuments.Columns.Add("ProformaDate", typeof(DateTime));
				dtInvoiceDocuments.Columns.Add("LcOrderMasterId", typeof(int));
				dtInvoiceDocuments.Columns.Add("LcOrderNo", typeof(string));
				dtInvoiceDocuments.Columns.Add("LcOrderDate", typeof(DateTime));
				dtInvoiceDocuments.Columns.Add("ExchangeRate", typeof(decimal));
				dtInvoiceDocuments.Columns.Add("FcyAmountTotal", typeof(decimal));
				dtInvoiceDocuments.Columns.Add("NoOfPackages", typeof(int));
				dtInvoiceDocuments.Columns.Add("FclTotal", typeof(int));
				dtInvoiceDocuments.Columns.Add("GrossWeightTotal", typeof(decimal));
				dtInvoiceDocuments.Columns.Add("NetWeightTotal", typeof(decimal));
				dtInvoiceDocuments.Columns.Add("BLNo", typeof(string));
				dtInvoiceDocuments.Columns.Add("BLDate", typeof(DateTime));
				dtInvoiceDocuments.Columns.Add("GDNo", typeof(string));
				dtInvoiceDocuments.Columns.Add("GDDate", typeof(DateTime));
				dtInvoiceDocuments.Columns.Add("GDValue", typeof(decimal));
				dtInvoiceDocuments.Columns.Add("DocStatus", typeof(string));
				dtInvoiceDocuments.Columns.Add("RemarksHeader", typeof(string));
				dtInvoiceDocuments.Columns.Add("EntryDate", typeof(DateTime));
				dtInvoiceDocuments.Columns.Add("EntryUser", typeof(string));
				dtInvoiceDocuments.Columns.Add("ModifyDate", typeof(DateTime));
				dtInvoiceDocuments.Columns.Add("ModifyUser", typeof(string));
				dtInvoiceDocuments.Columns.Add("IsApproved", typeof(bool));
				dtInvoiceDocuments.Columns.Add("ApprovedDate", typeof(DateTime));
				dtInvoiceDocuments.Columns.Add("ApprovedUser", typeof(string));
				dtInvoiceDocuments.Columns.Add("NoOfAttachments", typeof(int));
				dtInvoiceDocuments.Columns.Add("RowVersionLong", typeof(long));
				dtInvoiceDocuments.Rows.Clear();
				foreach (DataRow row in dtgrd.Rows)
				{
					dtInvoiceDocuments.Rows.Add(row["InvoiceMasterId"], row["DocNo"], row["DocDate"], row["InvoiceMasterNo"], row["ProformaMasterId"], row["ProformaMasterNo"], row["ProformaMasterDate"], row["LcOrderMasterId"], row["LcOrderMasterNo"], row["LcOrderMasterDate"], row["ExchangeRate"], row["FcyAmountTotal"], row["NoOfPackages"], row["FclTotal"], row["GrossWeightTotal"], row["NetWeightTotal"], row["BillOfLadingNo"], row["BillOfLadingDate"], row["GoodsDeclarationNo"], row["GoodsDeclarationDate"], row["GoodsDeclarationValue"], row["DocStatus"], row["RemarksHeader"], row["CreatedOn"], row["CreatedUserName"], row["LastModifiedOn"], row["LastModifiedUserName"], row["IsApproved"], row["ApprovedOn"], row["ApprovedUserName"], row["NoOfAttachments"], row["RowVersionLong"]);
				}
				DataGridHistory.DataSource = dtInvoiceDocuments;
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
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		try
		{
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["ProformaMasterId"].Visible = false;
			DataGridHistory.RootTable.Columns["LcOrderMasterId"].Visible = false;
			DataGridHistory.RootTable.Columns["RowVersionLong"].Visible = false;
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["docNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["invoiceDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["invoiceNo"].Width = Constants.InventoryConstants.InvoiceNoConstant;
			DataGridHistory.RootTable.Columns["proformaNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			DataGridHistory.RootTable.Columns["proformaDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["lcOrderNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			DataGridHistory.RootTable.Columns["lcOrderDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["exchangeRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["fcyAmountTotal"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["NoOfPackages"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["fclTotal"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["grossWeightTotal"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["netWeightTotal"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["BLNo"].Width = Constants.InventoryConstants.BLNumberConstant;
			DataGridHistory.RootTable.Columns["BLDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["GDNo"].Width = Constants.InventoryConstants.BLNumberConstant;
			DataGridHistory.RootTable.Columns["GDDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["GDValue"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["remarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["docStatus"].Width = Constants.InventoryConstants.StatusConstant;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			if (formrights.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Slip");
				DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
				DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
				DataGridHistory.RootTable.Columns["Slip"].Width = 70;
				DataGridHistory.RootTable.Columns["Slip"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formrights.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 70;
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formrights.DoHavePrintRights && formrights.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.RootTable.Columns["Slip"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 902);
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
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				long Id = Conversion.ToInt64(r.Cells["Id"].Value);
				if (e.Column.Key == "Slip")
				{
					InvoiceSlip(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
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
				long Id = Conversion.ToInt64(r.Cells["Id"].Value);
				GetDetailByHeaderId(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailByHeaderId(long rr)
	{
		try
		{
			invoiceMaster PIH = invoiceMaster.ReadById(rr);
			if (PIH.invoiceDetailList.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(long));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemDetail");
				dt.Columns.Add("JobLotId", typeof(int));
				dt.Columns.Add("JobLot");
				dt.Columns.Add("Qty/M.Ton", typeof(double));
				dt.Columns.Add("PackSizeId", typeof(int));
				dt.Columns.Add("PackSize");
				dt.Columns.Add("PackEquivalent", typeof(double));
				dt.Columns.Add("NoOfBags", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("Cost/M.Ton", typeof(double));
				dt.Columns.Add("RateUOMId", typeof(int));
				dt.Columns.Add("RateUOM");
				dt.Columns.Add("RateEquivalent", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Remarks");
				dt.Rows.Clear();
				for (int i = 0; i < PIH.invoiceDetailList.Count; i++)
				{
					dt.Rows.Add(PIH.invoiceDetailList[i].invoiceDetailId, PIH.invoiceDetailList[i].ItemId, PIH.invoiceDetailList[i].ItemName, PIH.invoiceDetailList[i].ItemCode, PIH.invoiceDetailList[i].ItemDescription, PIH.invoiceDetailList[i].lotJobId, PIH.invoiceDetailList[i].JobLotDescription, PIH.invoiceDetailList[i].netWeightOuter / 1000.0, PIH.invoiceDetailList[i].packSizeIdOuter, PIH.invoiceDetailList[i].OuterUOM, PIH.invoiceDetailList[i].OuterEquivalent, PIH.invoiceDetailList[i].qtyOuter, PIH.invoiceDetailList[i].netWeightOuter, PIH.invoiceDetailList[i].ItemRate, PIH.invoiceDetailList[i].uomIdRate, PIH.invoiceDetailList[i].RateUOM, PIH.invoiceDetailList[i].RateEquivalent, PIH.invoiceDetailList[i].FcAmount, PIH.invoiceDetailList[i].RemarksDetail);
				}
				gridDetailHistory.DataSource = dt;
				gridDetailHistory.RetrieveStructure();
				grdHistorySettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistorySettings()
	{
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		try
		{
			gridDetailHistory.AllowEdit = (InheritableBoolean)1;
			gridDetailHistory.RootTable.Columns["Id"].Visible = false;
			gridDetailHistory.RootTable.Columns["ItemId"].Visible = false;
			gridDetailHistory.RootTable.Columns["JobLotId"].Visible = false;
			gridDetailHistory.RootTable.Columns["PackSizeId"].Visible = false;
			gridDetailHistory.RootTable.Columns["PackEquivalent"].Visible = false;
			gridDetailHistory.RootTable.Columns["RateUOMId"].Visible = false;
			gridDetailHistory.RootTable.Columns["RateEquivalent"].Visible = false;
			gridDetailHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)gridDetailHistory.RootTable.Columns)
			{
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			gridDetailHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			gridDetailHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			gridDetailHistory.RootTable.Columns["ItemDetail"].Width = Constants.InventoryConstants.OtherDescription;
			gridDetailHistory.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			gridDetailHistory.RootTable.Columns["Qty/M.Ton"].Width = Constants.InventoryConstants.QtyConstant;
			gridDetailHistory.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			gridDetailHistory.RootTable.Columns["PackEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			gridDetailHistory.RootTable.Columns["NoOfBags"].Width = Constants.InventoryConstants.QtyConstant;
			gridDetailHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridDetailHistory.RootTable.Columns["Cost/M.Ton"].Width = Constants.InventoryConstants.ExRateConstant;
			gridDetailHistory.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			gridDetailHistory.RootTable.Columns["RateEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			gridDetailHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridDetailHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
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
			if (Conversion.ToDouble((object)txtfcyAmountHeader.Text) > 0.0 && Conversion.ToDouble((object)txtFcyAmountPaymentDetail.Text) > 0.0)
			{
				double TotalAmount = Conversion.ToDouble((object)txtfcyAmountHeader.Text.Trim());
				double Percent = Conversion.ToDouble((object)txtFcyAmountPaymentDetail.Text.Trim()) / TotalAmount * 100.0;
				dtxtpercentoftotal.Text = Percent.ToString();
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
			if (Conversion.ToDouble((object)txtfcyAmountHeader.Text) > 0.0 && grdpaymentdetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdpaymentdetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double TotalAmount = Conversion.ToDouble((object)txtfcyAmountHeader.Text);
					double Amount = Conversion.ToDouble(obj.Cells["%OfTotal"].Value) * TotalAmount / 100.0;
					obj.Cells["FcyAmount"].Value = Amount;
					obj.EndEdit();
				}
			}
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
			if (Conversion.ToDouble((object)txtfcyAmountHeader.Text.Trim()) > 0.0 && Conversion.ToDouble((object)dtxtpercentoftotal.Text) > 0.0)
			{
				if (Conversion.ToDouble((object)dtxtpercentoftotal.Text) > 100.0)
				{
					dtxtpercentoftotal.Text = "0";
					throw new Exception("Percent cannot be greater than 100 Please check");
				}
				double num = Conversion.ToDouble((object)txtfcyAmountHeader.Text.Trim());
				double Percent = Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim());
				double FcAmount = num * Percent / 100.0;
				txtFcyAmountPaymentDetail.Text = FcAmount.ToString("#,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetFinancialInstrumentsBalance()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			decimal Balance = ExImEFormRegistration.GetFinancialInstrumentsBalance(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPaymentDetail).SelectedRow.Cells[3].Value),
				Id = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPaymentDetail).SelectedRow.Cells["Id"].Value)
			});
			txtBalanceFIPaymentDetail.Text = Balance.ToString("0,0");
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
			double Qty = Conversion.ToDouble((object)txtQtyOtherItem.Text.Trim());
			double Rate = Conversion.ToDouble((object)txtRateOtherItem.Text.Trim());
			if (Qty > 0.0 && Rate > 0.0)
			{
				txtAmountOtherItem.Text = (Qty * Rate).ToString();
			}
			else
			{
				txtAmountOtherItem.Text = "0";
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
			double FcyAmount = Conversion.ToDouble((object)txtfcyAmountHeader.Text);
			double OtherAmount = Conversion.ToDouble(total);
			if (FcyAmount > 0.0)
			{
				txtfcyAmountHeader.Text = (FcyAmount + OtherAmount).ToString("#,#.###");
			}
			if (Conversion.ToDouble((object)txtfcyAmountHeader.Text) > 0.0 && grdpaymentdetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdpaymentdetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double TotalAmount = Conversion.ToDouble((object)txtfcyAmountHeader.Text);
					double Amount = Conversion.ToDouble(obj.Cells["%OfTotal"].Value) * TotalAmount / 100.0;
					obj.Cells["FcyAmount"].Value = Amount;
					obj.EndEdit();
				}
			}
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
			double MTon = Conversion.ToDouble((object)txtQtyMTon.Text);
			double NoOfBags = Conversion.ToDouble((object)txtNoOfBags.Text);
			double OuterUom = 0.0;
			string Bool_Mton = txtQtyMTon.AccessibleName;
			string Bool_NoOfBags = txtNoOfBags.AccessibleName;
			if (((UltraGridBase)CmbPackUom).ActiveRow != null)
			{
				OuterUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				if (Conversion.ToString(base.ActiveControl.Tag) == "Mton")
				{
					double noofbags = ((MTon > 0.0) ? (MTon * 1000.0 / OuterUom) : 0.0);
					txtNoOfBags.Text = Math.Round(noofbags).ToString();
					txtQtyMTon.AccessibleName = "True";
					txtNoOfBags.AccessibleName = "False";
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "NoOfBags")
				{
					MTon = ((NoOfBags > 0.0) ? (NoOfBags * OuterUom / 1000.0) : 0.0);
					txtQtyMTon.Text = MTon.ToString("#,##0.###");
					txtNoOfBags.AccessibleName = "True";
					txtQtyMTon.AccessibleName = "False";
				}
				else if (Bool_Mton == "True")
				{
					double noofbags2 = ((MTon > 0.0) ? (MTon * 1000.0 / OuterUom) : 0.0);
					txtNoOfBags.Text = Math.Round(noofbags2).ToString();
				}
				else if (Bool_NoOfBags == "True")
				{
					MTon = ((NoOfBags > 0.0) ? (NoOfBags * OuterUom / 1000.0) : 0.0);
					txtQtyMTon.Text = MTon.ToString("#,##0.###");
				}
			}
			else
			{
				txtNoOfBags.Text = 0.ToString();
				txtQtyMTon.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculatAmount()
	{
		try
		{
			double ItemRate = 0.0;
			double ItemRateUom = 0.0;
			double Qty = 0.0;
			txtAmount.Text = 0.ToString();
			if (((UltraGridBase)CmbRateUom).ActiveRow != null)
			{
				ItemRate = Conversion.ToDouble((object)txtCostMTon.Text);
				ItemRateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				Qty = Conversion.ToDouble((object)txtQtyMTon.Text);
				if (ItemRate > 0.0 && ItemRateUom > 0.0 && Qty > 0.0)
				{
					double weightkgs = Qty * 1000.0;
					double amount = ItemRate / ItemRateUom * weightkgs;
					txtAmount.Text = amount.ToString("0,0");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtQtyMTon.Text) > 0.0)
			{
				double Weight = Conversion.ToDouble((object)txtQtyMTon.Text) * 1000.0;
				txtWeightDetail.Text = Weight.ToString();
			}
			else
			{
				txtWeightDetail.Text = "0";
			}
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void txtNoOfBags_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbSupCust_Leave(object sender, EventArgs e)
	{
		try
		{
			BindFinancialInstrument();
			bindConsigneeAgainstCustomer();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtoRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			OtherItemsCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProportionateOtherItemAmount()
	{
		try
		{
			double ContractAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			double OtherItemAmount = Conversion.ToDouble(grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (OtherItemAmount > 0.0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double amount = Conversion.ToDouble(obj.Cells["Amount"].Value);
					double propAmount = OtherItemAmount / ContractAmount * amount;
					obj.Cells["OtherItemAmount"].Value = propAmount;
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["OtherItemAmount"].Value = 0;
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceSlip(long PrintId)
	{
		try
		{
			CommonServices.ImportInvoiceSlip902(PrintId);
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
			InvoiceSlip(RecId);
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

	private void BtnDefinePerformaNo_Click(object sender, EventArgs e)
	{
		try
		{
			new frmProformaDocumentSerial(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineInvoiceNo_Click(object sender, EventArgs e)
	{
		try
		{
			new frmMasterDocumentSerial(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineContractNo_Click(object sender, EventArgs e)
	{
		try
		{
			new frmMasterDocumentSerial(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					if (BranchFeature)
					{
						CmbBranchName.Focus();
					}
					else
					{
						CmbExporterName.Focus();
					}
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && UpdateMode)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
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
					if (BranchFeature)
					{
						CmbBranchName.Focus();
					}
					else
					{
						CmbExporterName.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grdDetail).Focus();
						tabControl2.SelectedIndex = 0;
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)grdotheritems).Focus();
						tabControl2.SelectedIndex = 1;
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						((Control)(object)grdpaymentdetail).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						CmbItemName.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						CmbItemOtherItem.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						CmbPaymentTermPaymentDetail.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						CmbItemOtherItem.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 2;
						CmbPaymentTermPaymentDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						tabControl2.SelectedIndex = 0;
						CmbItemName.Focus();
					}
				}
				return;
			}
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
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)gridDetailHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)gridDetailHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Tabs");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Selected Tab Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Entry of Detail Selected Tab");
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDetail.CurrentColumn != null && grdDetail.CurrentColumn.Key == "Delete")
				{
					DeleteDetailGridRow(item);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailGridRow(item);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdotheritems.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdotheritems_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdotheritems.CurrentColumn != null && grdotheritems.CurrentColumn.Key == "Delete")
				{
					DeleteOtherItemDetailGridRow(item);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteOtherItemDetailGridRow(item);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpaymentdetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdpaymentdetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdpaymentdetail_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdpaymentdetail.CurrentColumn != null && grdpaymentdetail.CurrentColumn.Key == "Delete")
				{
					DeletePaymentItemDetailGridRow(item);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeletePaymentItemDetailGridRow(item);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			long Id = Conversion.ToInt64(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Slip" && formrights.DoHavePrintRights)
				{
					InvoiceSlip(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 902);
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
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b60: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Expected O, but got Unknown
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Expected O, but got Unknown
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfa: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1b: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Expected O, but got Unknown
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Expected O, but got Unknown
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Expected O, but got Unknown
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Expected O, but got Unknown
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caa: Expected O, but got Unknown
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Expected O, but got Unknown
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccb: Expected O, but got Unknown
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd6: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce1: Expected O, but got Unknown
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cec: Expected O, but got Unknown
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Expected O, but got Unknown
		//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Expected O, but got Unknown
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0d: Expected O, but got Unknown
		//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Expected O, but got Unknown
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Expected O, but got Unknown
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2e: Expected O, but got Unknown
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d44: Expected O, but got Unknown
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4f: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5a: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d65: Expected O, but got Unknown
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Expected O, but got Unknown
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7b: Expected O, but got Unknown
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Expected O, but got Unknown
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Expected O, but got Unknown
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9c: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da7: Expected O, but got Unknown
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db2: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbd: Expected O, but got Unknown
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc8: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd3: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dde: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de9: Expected O, but got Unknown
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df4: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dff: Expected O, but got Unknown
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e15: Expected O, but got Unknown
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e20: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Expected O, but got Unknown
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e36: Expected O, but got Unknown
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e41: Expected O, but got Unknown
		//IL_0eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Expected O, but got Unknown
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Expected O, but got Unknown
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d6: Expected O, but got Unknown
		//IL_113a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Expected O, but got Unknown
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_1170: Expected O, but got Unknown
		//IL_120b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1215: Expected O, but got Unknown
		//IL_128f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1299: Expected O, but got Unknown
		//IL_12b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ba: Expected O, but got Unknown
		//IL_12d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12db: Expected O, but got Unknown
		//IL_12dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e6: Expected O, but got Unknown
		//IL_12f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fc: Expected O, but got Unknown
		//IL_12fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1307: Expected O, but got Unknown
		//IL_1308: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Expected O, but got Unknown
		//IL_133f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1349: Expected O, but got Unknown
		//IL_13ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d8: Expected O, but got Unknown
		//IL_13d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e3: Expected O, but got Unknown
		//IL_13ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f9: Expected O, but got Unknown
		//IL_13fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1404: Expected O, but got Unknown
		//IL_1410: Unknown result type (might be due to invalid IL or missing references)
		//IL_141a: Expected O, but got Unknown
		//IL_141b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1425: Expected O, but got Unknown
		//IL_145d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1467: Expected O, but got Unknown
		//IL_14b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bf: Expected O, but got Unknown
		//IL_1586: Unknown result type (might be due to invalid IL or missing references)
		//IL_1590: Expected O, but got Unknown
		//IL_1620: Unknown result type (might be due to invalid IL or missing references)
		//IL_162a: Expected O, but got Unknown
		//IL_16d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16da: Expected O, but got Unknown
		//IL_175f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1769: Expected O, but got Unknown
		//IL_1775: Unknown result type (might be due to invalid IL or missing references)
		//IL_177f: Expected O, but got Unknown
		//IL_1780: Unknown result type (might be due to invalid IL or missing references)
		//IL_178a: Expected O, but got Unknown
		//IL_178b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Expected O, but got Unknown
		//IL_1796: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a0: Expected O, but got Unknown
		//IL_17a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ab: Expected O, but got Unknown
		//IL_17ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b6: Expected O, but got Unknown
		//IL_17b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c1: Expected O, but got Unknown
		//IL_17c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cc: Expected O, but got Unknown
		//IL_17cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d7: Expected O, but got Unknown
		//IL_2129: Unknown result type (might be due to invalid IL or missing references)
		//IL_2133: Expected O, but got Unknown
		//IL_2140: Unknown result type (might be due to invalid IL or missing references)
		//IL_214a: Expected O, but got Unknown
		//IL_4455: Unknown result type (might be due to invalid IL or missing references)
		//IL_445f: Expected O, but got Unknown
		//IL_5455: Unknown result type (might be due to invalid IL or missing references)
		//IL_545f: Expected O, but got Unknown
		//IL_ec9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_eca8: Expected O, but got Unknown
		//IL_eccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_ecd6: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance390 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance80 = new Appearance();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdpaymentdetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance399 = new Appearance();
		Appearance appearance410 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance374 = new Appearance();
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
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
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
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance398 = new Appearance();
		Appearance appearance400 = new Appearance();
		Appearance appearance401 = new Appearance();
		Appearance appearance402 = new Appearance();
		Appearance appearance403 = new Appearance();
		Appearance appearance404 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance418 = new Appearance();
		Appearance appearance419 = new Appearance();
		Appearance appearance420 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance405 = new Appearance();
		Appearance appearance406 = new Appearance();
		Appearance appearance407 = new Appearance();
		Appearance appearance408 = new Appearance();
		Appearance appearance409 = new Appearance();
		Appearance appearance411 = new Appearance();
		Appearance appearance412 = new Appearance();
		Appearance appearance413 = new Appearance();
		Appearance appearance414 = new Appearance();
		Appearance appearance415 = new Appearance();
		Appearance appearance416 = new Appearance();
		Appearance appearance417 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance397 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance345 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance387 = new Appearance();
		GridEXLayout gridDetailHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance295 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
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
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
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
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
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
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Import.frmImportInvoice));
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label85 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.txtQtyMTon = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.label32 = new System.Windows.Forms.Label();
		this.label94 = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtitemDescription = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.txtCostMTon = new System.Windows.Forms.TextBox();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.label29 = new System.Windows.Forms.Label();
		this.btnCancel = new System.Windows.Forms.Button();
		this.txtWeightDetail = new System.Windows.Forms.TextBox();
		this.CmbItemName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdotheritems = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel19 = new System.Windows.Forms.Panel();
		this.BtnAddOtherItem = new System.Windows.Forms.Button();
		this.BtnCancelOtherItem = new System.Windows.Forms.Button();
		this.CmbItemOtherItem = new UltraCombo();
		this.BtnUpdateOtherItem = new System.Windows.Forms.Button();
		this.label71 = new System.Windows.Forms.Label();
		this.txtAmountOtherItem = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtRemarksOtherItem = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.txtQtyOtherItem = new System.Windows.Forms.TextBox();
		this.txtRateOtherItem = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdpaymentdetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel21 = new System.Windows.Forms.Panel();
		this.BtnAddPaymentDetail = new System.Windows.Forms.Button();
		this.label58 = new System.Windows.Forms.Label();
		this.txtRemarksPaymentDetail = new System.Windows.Forms.TextBox();
		this.label82 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.CmbFinInstrumentsPaymentDetail = new UltraCombo();
		this.label80 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.CmbPaymentTermPaymentDetail = new UltraCombo();
		this.label76 = new System.Windows.Forms.Label();
		this.dtxtpercentoftotal = new System.Windows.Forms.TextBox();
		this.btnCancelPaymentDetail = new System.Windows.Forms.Button();
		this.label83 = new System.Windows.Forms.Label();
		this.BtnUpdatePaymentDetail = new System.Windows.Forms.Button();
		this.txtBalanceFIPaymentDetail = new System.Windows.Forms.TextBox();
		this.txtDueDaysPaymentDetail = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFcyAmountPaymentDetail = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.HeaderGropBox = new System.Windows.Forms.GroupBox();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbImportCompany = new UltraCombo();
		this.labelExportCompany = new System.Windows.Forms.Label();
		this.BtnDefineInvoiceNo = new System.Windows.Forms.Button();
		this.BtnDefineContractNo = new System.Windows.Forms.Button();
		this.BtnDefinePerformaNo = new System.Windows.Forms.Button();
		this.label17 = new System.Windows.Forms.Label();
		this.label104 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label109 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbDabitAccount = new UltraCombo();
		this.lblCreditAc = new System.Windows.Forms.Label();
		this.txtContractDate = new System.Windows.Forms.DateTimePicker();
		this.CmbImporterName = new UltraCombo();
		this.label60 = new System.Windows.Forms.Label();
		this.txtPerformaDate = new System.Windows.Forms.DateTimePicker();
		this.cmbInvoiceNo = new UltraCombo();
		this.CmbContractNo = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbPerformaNo = new UltraCombo();
		this.CmbSalesPerson = new UltraCombo();
		this.cmbConsignee = new UltraCombo();
		this.label103 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.label101 = new System.Windows.Forms.Label();
		this.CmbExporterName = new UltraCombo();
		this.label99 = new System.Windows.Forms.Label();
		this.label97 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label96 = new System.Windows.Forms.Label();
		this.label114 = new System.Windows.Forms.Label();
		this.txtLotRef = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label108 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.cmbStatus = new UltraCombo();
		this.CmbIncomeTerm = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbFcyCode = new UltraCombo();
		this.cmbexporterBankNew = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbPaymentTermsNew = new UltraCombo();
		this.cmbimporterBankNew = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.txtNoOfPackagesHeader = new System.Windows.Forms.TextBox();
		this.label110 = new System.Windows.Forms.Label();
		this.label113 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.cmbLoadingPort = new UltraCombo();
		this.txtInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.txtBLNo = new System.Windows.Forms.TextBox();
		this.txtBlDate = new System.Windows.Forms.DateTimePicker();
		this.txtGdValue = new System.Windows.Forms.TextBox();
		this.txtGdNo = new System.Windows.Forms.TextBox();
		this.txtGdDate = new System.Windows.Forms.DateTimePicker();
		this.txtNoOfContainerHeader = new System.Windows.Forms.TextBox();
		this.cmbDestinationPort = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label107 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.txtfcyAmountHeader = new System.Windows.Forms.TextBox();
		this.txtNetWeightHeader = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtExRate = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.txtGrossWeightHeader = new System.Windows.Forms.TextBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.CmbBranchName = new UltraCombo();
		this.labelBranchName = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label20 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label93 = new System.Windows.Forms.Label();
		this.gridDetailHistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbFCurrencr = new UltraCombo();
		this.combranches = new UltraCombo();
		this.comproject = new UltraCombo();
		this.CmbLCContractor = new UltraCombo();
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
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel17.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel23.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel18.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOtherItem).BeginInit();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).BeginInit();
		this.panel20.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstrumentsPaymentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPaymentDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.HeaderGropBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbImportCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDabitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPerformaNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalesPerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbConsignee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIncomeTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbexporterBankNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermsNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterBankNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).BeginInit();
		this.panel12.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).BeginInit();
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
		this.panel4.Size = new System.Drawing.Size(1060, 711);
		this.panel4.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.tabControl1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1060, 711);
		this.panel7.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1060, 711);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1052, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel17);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 57);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1046, 622);
		this.panel8.TabIndex = 3;
		this.panel17.AutoScroll = true;
		this.panel17.Controls.Add(this.tabControl2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 263);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1046, 359);
		this.panel17.TabIndex = 4;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1046, 359);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage3.Controls.Add(this.panel23);
		this.tabPage3.Controls.Add(this.panel22);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1038, 330);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Invoice Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 125);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1032, 202);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar2);
		this.panel23.Controls.Add(this.label85);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(3, 101);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1032, 24);
		this.panel23.TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(993, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label85.ForeColor = System.Drawing.Color.White;
		this.label85.Location = new System.Drawing.Point(3, 2);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(49, 20);
		this.label85.TabIndex = 27;
		this.label85.Text = "Detail";
		this.panel22.Controls.Add(this.txtQtyMTon);
		this.panel22.Controls.Add(this.label35);
		this.panel22.Controls.Add(this.btnAddinGrid);
		this.panel22.Controls.Add(this.label32);
		this.panel22.Controls.Add(this.label94);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel22.Controls.Add(this.label38);
		this.panel22.Controls.Add(this.txtRemarksDetail);
		this.panel22.Controls.Add(this.label37);
		this.panel22.Controls.Add(this.label19);
		this.panel22.Controls.Add(this.txtAmount);
		this.panel22.Controls.Add(this.txtitemDescription);
		this.panel22.Controls.Add(this.label36);
		this.panel22.Controls.Add(this.label34);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel22.Controls.Add(this.txtCostMTon);
		this.panel22.Controls.Add(this.btnGridUpdate);
		this.panel22.Controls.Add(this.label29);
		this.panel22.Controls.Add(this.btnCancel);
		this.panel22.Controls.Add(this.txtWeightDetail);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel22.Controls.Add(this.label2);
		this.panel22.Controls.Add(this.label30);
		this.panel22.Controls.Add(this.txtNoOfBags);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1032, 98);
		this.panel22.TabIndex = 5;
		this.txtQtyMTon.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyMTon.Location = new System.Drawing.Point(623, 24);
		this.txtQtyMTon.Name = "txtQtyMTon";
		this.txtQtyMTon.Size = new System.Drawing.Size(98, 22);
		this.txtQtyMTon.TabIndex = 3;
		this.txtQtyMTon.Tag = "Mton";
		this.txtQtyMTon.Text = "0";
		this.txtQtyMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMTon.TextChanged += new System.EventHandler(txtQtyMTon_TextChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(621, 6);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(65, 15);
		this.label35.TabIndex = 498;
		this.label35.Text = "Qty/M.Ton";
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatAppearance.BorderSize = 0;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(904, 66);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(31, 26);
		this.btnAddinGrid.TabIndex = 11;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(4, 5);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(67, 15);
		this.label32.TabIndex = 470;
		this.label32.Text = "Item Name";
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.Location = new System.Drawing.Point(417, 5);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(24, 15);
		this.label94.TabIndex = 493;
		this.label94.Text = "Lot";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance390).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance390).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance390;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance391).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance391).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance391).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance391).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance391;
		((AppearanceBase)appearance389).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance389;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(101, 67);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(100, 25);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance286;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance226).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(417, 23);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(204, 25);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 2;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(101, 51);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(61, 15);
		this.label38.TabIndex = 479;
		this.label38.Text = "Rate UOM";
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(313, 68);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(580, 22);
		this.txtRemarksDetail.TabIndex = 10;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(203, 51);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(51, 15);
		this.label37.TabIndex = 485;
		this.label37.Text = "Amount";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(313, 51);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(52, 15);
		this.label19.TabIndex = 491;
		this.label19.Text = "Remarks";
		this.txtAmount.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(203, 68);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(109, 22);
		this.txtAmount.TabIndex = 9;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtitemDescription.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtitemDescription.Location = new System.Drawing.Point(203, 24);
		this.txtitemDescription.Name = "txtitemDescription";
		this.txtitemDescription.Size = new System.Drawing.Size(212, 22);
		this.txtitemDescription.TabIndex = 1;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(723, 6);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(57, 15);
		this.label36.TabIndex = 474;
		this.label36.Text = "Pack Size";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(203, 5);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(66, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Item Detail";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance148).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(723, 23);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(104, 25);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(combitempck_TextChanged);
		this.txtCostMTon.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCostMTon.Location = new System.Drawing.Point(4, 68);
		this.txtCostMTon.Name = "txtCostMTon";
		this.txtCostMTon.Size = new System.Drawing.Size(95, 22);
		this.txtCostMTon.TabIndex = 7;
		this.txtCostMTon.Text = "0";
		this.txtCostMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCostMTon.TextChanged += new System.EventHandler(txtCostMTon_TextChanged);
		this.txtCostMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCostMTon_KeyPress);
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
		this.btnGridUpdate.FlatAppearance.BorderSize = 0;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(896, 66);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(65, 26);
		this.btnGridUpdate.TabIndex = 12;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(4, 51);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(69, 15);
		this.label29.TabIndex = 487;
		this.label29.Text = "Rate/M.Ton";
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderSize = 0;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(964, 66);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(65, 26);
		this.btnCancel.TabIndex = 13;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnCancel.Leave += new System.EventHandler(btnCancel_Leave);
		this.txtWeightDetail.BackColor = System.Drawing.Color.White;
		this.txtWeightDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightDetail.Location = new System.Drawing.Point(915, 24);
		this.txtWeightDetail.Name = "txtWeightDetail";
		this.txtWeightDetail.ReadOnly = true;
		this.txtWeightDetail.Size = new System.Drawing.Size(115, 22);
		this.txtWeightDetail.TabIndex = 6;
		this.txtWeightDetail.Text = "0";
		this.txtWeightDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance260;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance266).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance267).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(4, 23);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(197, 25);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(combitem_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(915, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 485;
		this.label2.Text = "Weight(kg)";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(829, 6);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(26, 15);
		this.label30.TabIndex = 485;
		this.label30.Text = "Qty";
		this.txtNoOfBags.BackColor = System.Drawing.Color.White;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(829, 24);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(85, 22);
		this.txtNoOfBags.TabIndex = 5;
		this.txtNoOfBags.Tag = "NoOfBags";
		this.txtNoOfBags.Text = "0";
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBags.TextChanged += new System.EventHandler(txtNoOfBags_TextChanged);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.tabPage4.Controls.Add(this.panel18);
		this.tabPage4.Controls.Add(this.panel19);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1038, 330);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Items";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(3, 78);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdotheritems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1032, 249);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 0;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		this.grdotheritems.ColumnButtonClick += new ColumnActionEventHandler(grdotheritems_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdotheritems_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 55);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1032, 23);
		this.panel18.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(992, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdotheritems;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel19.Controls.Add(this.BtnAddOtherItem);
		this.panel19.Controls.Add(this.BtnCancelOtherItem);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemOtherItem);
		this.panel19.Controls.Add(this.BtnUpdateOtherItem);
		this.panel19.Controls.Add(this.label71);
		this.panel19.Controls.Add(this.txtAmountOtherItem);
		this.panel19.Controls.Add(this.label14);
		this.panel19.Controls.Add(this.label70);
		this.panel19.Controls.Add(this.txtRemarksOtherItem);
		this.panel19.Controls.Add(this.label63);
		this.panel19.Controls.Add(this.txtQtyOtherItem);
		this.panel19.Controls.Add(this.txtRateOtherItem);
		this.panel19.Controls.Add(this.label15);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(3, 3);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1032, 52);
		this.panel19.TabIndex = 1;
		this.BtnAddOtherItem.BackColor = System.Drawing.Color.Teal;
		this.BtnAddOtherItem.FlatAppearance.BorderSize = 0;
		this.BtnAddOtherItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddOtherItem.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.BtnAddOtherItem.ForeColor = System.Drawing.Color.White;
		this.BtnAddOtherItem.Location = new System.Drawing.Point(861, 24);
		this.BtnAddOtherItem.Name = "BtnAddOtherItem";
		this.BtnAddOtherItem.Size = new System.Drawing.Size(36, 24);
		this.BtnAddOtherItem.TabIndex = 5;
		this.BtnAddOtherItem.Text = "+";
		this.BtnAddOtherItem.UseVisualStyleBackColor = false;
		this.BtnAddOtherItem.Click += new System.EventHandler(btnoadd_Click);
		this.BtnAddOtherItem.Leave += new System.EventHandler(btnoadd_Leave);
		this.BtnCancelOtherItem.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelOtherItem.FlatAppearance.BorderSize = 0;
		this.BtnCancelOtherItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelOtherItem.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.BtnCancelOtherItem.ForeColor = System.Drawing.Color.White;
		this.BtnCancelOtherItem.Location = new System.Drawing.Point(931, 24);
		this.BtnCancelOtherItem.Name = "BtnCancelOtherItem";
		this.BtnCancelOtherItem.Size = new System.Drawing.Size(70, 24);
		this.BtnCancelOtherItem.TabIndex = 7;
		this.BtnCancelOtherItem.Text = "Cancel";
		this.BtnCancelOtherItem.UseVisualStyleBackColor = false;
		this.BtnCancelOtherItem.Visible = false;
		this.BtnCancelOtherItem.Click += new System.EventHandler(btnoCancel_Click);
		this.BtnCancelOtherItem.Leave += new System.EventHandler(btnoCancel_Leave);
		this.CmbItemOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItemOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance239).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemOtherItem).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbItemOtherItem).Location = new System.Drawing.Point(5, 24);
		((System.Windows.Forms.Control)(object)this.CmbItemOtherItem).Name = "CmbItemOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbItemOtherItem).Size = new System.Drawing.Size(198, 25);
		((System.Windows.Forms.Control)(object)this.CmbItemOtherItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemOtherItem).TextChanged += new System.EventHandler(CmbOtherItem_TextChanged);
		this.BtnUpdateOtherItem.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateOtherItem.FlatAppearance.BorderSize = 0;
		this.BtnUpdateOtherItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateOtherItem.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateOtherItem.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateOtherItem.Location = new System.Drawing.Point(861, 24);
		this.BtnUpdateOtherItem.Name = "BtnUpdateOtherItem";
		this.BtnUpdateOtherItem.Size = new System.Drawing.Size(67, 24);
		this.BtnUpdateOtherItem.TabIndex = 6;
		this.BtnUpdateOtherItem.Text = "Update";
		this.BtnUpdateOtherItem.UseVisualStyleBackColor = false;
		this.BtnUpdateOtherItem.Visible = false;
		this.BtnUpdateOtherItem.Click += new System.EventHandler(btnoUpdate_Click);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(8, 6);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(67, 15);
		this.label71.TabIndex = 504;
		this.label71.Text = "Item Name";
		this.txtAmountOtherItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountOtherItem.Location = new System.Drawing.Point(392, 25);
		this.txtAmountOtherItem.Name = "txtAmountOtherItem";
		this.txtAmountOtherItem.ReadOnly = true;
		this.txtAmountOtherItem.Size = new System.Drawing.Size(101, 22);
		this.txtAmountOtherItem.TabIndex = 3;
		this.txtAmountOtherItem.Text = "0";
		this.txtAmountOtherItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(499, 6);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(52, 15);
		this.label14.TabIndex = 508;
		this.label14.Text = "Remarks";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(393, 6);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(51, 15);
		this.label70.TabIndex = 506;
		this.label70.Text = "Amount";
		this.txtRemarksOtherItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksOtherItem.Location = new System.Drawing.Point(494, 25);
		this.txtRemarksOtherItem.Name = "txtRemarksOtherItem";
		this.txtRemarksOtherItem.Size = new System.Drawing.Size(364, 22);
		this.txtRemarksOtherItem.TabIndex = 4;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(308, 6);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(30, 15);
		this.label63.TabIndex = 507;
		this.label63.Text = "Rate";
		this.txtQtyOtherItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyOtherItem.Location = new System.Drawing.Point(205, 25);
		this.txtQtyOtherItem.Name = "txtQtyOtherItem";
		this.txtQtyOtherItem.Size = new System.Drawing.Size(101, 22);
		this.txtQtyOtherItem.TabIndex = 1;
		this.txtQtyOtherItem.Text = "0";
		this.txtQtyOtherItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyOtherItem.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.txtRateOtherItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateOtherItem.Location = new System.Drawing.Point(307, 25);
		this.txtRateOtherItem.Name = "txtRateOtherItem";
		this.txtRateOtherItem.Size = new System.Drawing.Size(84, 22);
		this.txtRateOtherItem.TabIndex = 2;
		this.txtRateOtherItem.Text = "0";
		this.txtRateOtherItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateOtherItem.TextChanged += new System.EventHandler(txtoRate_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(208, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(26, 15);
		this.label15.TabIndex = 505;
		this.label15.Text = "Qty";
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdpaymentdetail);
		this.tabPage5.Controls.Add(this.panel20);
		this.tabPage5.Controls.Add(this.panel21);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1038, 330);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Payment Detail";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.grdpaymentdetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdpaymentdetail).BackColor = System.Drawing.Color.White;
		grdpaymentdetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdpaymentdetail.DesignTimeLayout = grdpaymentdetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdpaymentdetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Location = new System.Drawing.Point(3, 84);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Name = "grdpaymentdetail";
		((ScrollableBase)this.grdpaymentdetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdpaymentdetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdpaymentdetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Size = new System.Drawing.Size(1032, 243);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).TabIndex = 0;
		this.grdpaymentdetail.TotalRow = (InheritableBoolean)1;
		this.grdpaymentdetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdpaymentdetail).VisualStyle = (VisualStyle)4;
		this.grdpaymentdetail.ColumnButtonClick += new ColumnActionEventHandler(grdpaymentdetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).DoubleClick += new System.EventHandler(grdpaymentdetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdpaymentdetail_KeyDown);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 61);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1032, 23);
		this.panel20.TabIndex = 0;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(992, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdpaymentdetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar4.TabIndex = 27;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel21.Controls.Add(this.BtnAddPaymentDetail);
		this.panel21.Controls.Add(this.label58);
		this.panel21.Controls.Add(this.txtRemarksPaymentDetail);
		this.panel21.Controls.Add(this.label82);
		this.panel21.Controls.Add(this.label75);
		this.panel21.Controls.Add(this.label81);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPaymentDetail);
		this.panel21.Controls.Add(this.label80);
		this.panel21.Controls.Add(this.label74);
		this.panel21.Controls.Add(this.label79);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermPaymentDetail);
		this.panel21.Controls.Add(this.label76);
		this.panel21.Controls.Add(this.dtxtpercentoftotal);
		this.panel21.Controls.Add(this.btnCancelPaymentDetail);
		this.panel21.Controls.Add(this.label83);
		this.panel21.Controls.Add(this.BtnUpdatePaymentDetail);
		this.panel21.Controls.Add(this.txtBalanceFIPaymentDetail);
		this.panel21.Controls.Add(this.txtDueDaysPaymentDetail);
		this.panel21.Controls.Add(this.label78);
		this.panel21.Controls.Add(this.label77);
		this.panel21.Controls.Add(this.txtFcyAmountPaymentDetail);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(3, 3);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1032, 58);
		this.panel21.TabIndex = 0;
		this.BtnAddPaymentDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddPaymentDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddPaymentDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddPaymentDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddPaymentDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddPaymentDetail.Location = new System.Drawing.Point(967, 30);
		this.BtnAddPaymentDetail.Name = "BtnAddPaymentDetail";
		this.BtnAddPaymentDetail.Size = new System.Drawing.Size(31, 22);
		this.BtnAddPaymentDetail.TabIndex = 7;
		this.BtnAddPaymentDetail.Text = "+";
		this.BtnAddPaymentDetail.UseVisualStyleBackColor = false;
		this.BtnAddPaymentDetail.Click += new System.EventHandler(btndadd_Click);
		this.BtnAddPaymentDetail.Leave += new System.EventHandler(btndadd_Leave);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(748, 8);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(52, 15);
		this.label58.TabIndex = 593;
		this.label58.Text = "Remarks";
		this.txtRemarksPaymentDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPaymentDetail.Location = new System.Drawing.Point(748, 28);
		this.txtRemarksPaymentDetail.Name = "txtRemarksPaymentDetail";
		this.txtRemarksPaymentDetail.Size = new System.Drawing.Size(216, 22);
		this.txtRemarksPaymentDetail.TabIndex = 6;
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.Red;
		this.label82.Location = new System.Drawing.Point(648, 7);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(14, 17);
		this.label82.TabIndex = 591;
		this.label82.Text = "*";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(171, 8);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(140, 15);
		this.label75.TabIndex = 508;
		this.label75.Text = "Financial Instruments No";
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.Red;
		this.label81.Location = new System.Drawing.Point(553, 7);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(14, 17);
		this.label81.TabIndex = 590;
		this.label81.Text = "*";
		this.CmbFinInstrumentsPaymentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbFinInstrumentsPaymentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinInstrumentsPaymentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinInstrumentsPaymentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance246).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance246).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance247;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance252).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance255).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinInstrumentsPaymentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPaymentDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPaymentDetail).Location = new System.Drawing.Point(171, 27);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPaymentDetail).Name = "CmbFinInstrumentsPaymentDetail";
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPaymentDetail).Size = new System.Drawing.Size(198, 25);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPaymentDetail).TabIndex = 1;
		this.CmbFinInstrumentsPaymentDetail.ValueChanged += new System.EventHandler(dcmbfino_ValueChanged);
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.ForeColor = System.Drawing.Color.Red;
		this.label80.Location = new System.Drawing.Point(101, 7);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(14, 17);
		this.label80.TabIndex = 589;
		this.label80.Text = "*";
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.Location = new System.Drawing.Point(485, 8);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(60, 15);
		this.label74.TabIndex = 509;
		this.label74.Text = "% of Total";
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.ForeColor = System.Drawing.Color.Red;
		this.label79.Location = new System.Drawing.Point(317, 7);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(14, 17);
		this.label79.TabIndex = 588;
		this.label79.Text = "*";
		this.CmbPaymentTermPaymentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermPaymentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermPaymentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermPaymentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance209;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance265).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance268).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance268).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance268).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance268;
		((AppearanceBase)appearance272).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermPaymentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPaymentDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPaymentDetail).Location = new System.Drawing.Point(6, 27);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPaymentDetail).Name = "CmbPaymentTermPaymentDetail";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPaymentDetail).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPaymentDetail).TabIndex = 0;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(6, 8);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(84, 15);
		this.label76.TabIndex = 511;
		this.label76.Text = "Payment Term";
		this.dtxtpercentoftotal.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtxtpercentoftotal.Location = new System.Drawing.Point(485, 28);
		this.dtxtpercentoftotal.Name = "dtxtpercentoftotal";
		this.dtxtpercentoftotal.Size = new System.Drawing.Size(82, 22);
		this.dtxtpercentoftotal.TabIndex = 3;
		this.dtxtpercentoftotal.Text = "0";
		this.dtxtpercentoftotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.dtxtpercentoftotal.Leave += new System.EventHandler(dtxtpercentoftotal_TextChanged);
		this.btnCancelPaymentDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelPaymentDetail.FlatAppearance.BorderSize = 0;
		this.btnCancelPaymentDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelPaymentDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelPaymentDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelPaymentDetail.Location = new System.Drawing.Point(967, 4);
		this.btnCancelPaymentDetail.Name = "btnCancelPaymentDetail";
		this.btnCancelPaymentDetail.Size = new System.Drawing.Size(63, 22);
		this.btnCancelPaymentDetail.TabIndex = 9;
		this.btnCancelPaymentDetail.Text = "Cancel";
		this.btnCancelPaymentDetail.UseVisualStyleBackColor = false;
		this.btnCancelPaymentDetail.Visible = false;
		this.btnCancelPaymentDetail.Click += new System.EventHandler(btndcancel_Click);
		this.btnCancelPaymentDetail.Leave += new System.EventHandler(btndcancel_Leave);
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(370, 8);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(48, 15);
		this.label83.TabIndex = 509;
		this.label83.Text = "Balance";
		this.BtnUpdatePaymentDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdatePaymentDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdatePaymentDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdatePaymentDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdatePaymentDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdatePaymentDetail.Location = new System.Drawing.Point(967, 29);
		this.BtnUpdatePaymentDetail.Name = "BtnUpdatePaymentDetail";
		this.BtnUpdatePaymentDetail.Size = new System.Drawing.Size(63, 23);
		this.BtnUpdatePaymentDetail.TabIndex = 8;
		this.BtnUpdatePaymentDetail.Text = "Update";
		this.BtnUpdatePaymentDetail.UseVisualStyleBackColor = false;
		this.BtnUpdatePaymentDetail.Visible = false;
		this.BtnUpdatePaymentDetail.Click += new System.EventHandler(btndupdate_Click);
		this.txtBalanceFIPaymentDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceFIPaymentDetail.Location = new System.Drawing.Point(370, 28);
		this.txtBalanceFIPaymentDetail.Name = "txtBalanceFIPaymentDetail";
		this.txtBalanceFIPaymentDetail.ReadOnly = true;
		this.txtBalanceFIPaymentDetail.Size = new System.Drawing.Size(114, 22);
		this.txtBalanceFIPaymentDetail.TabIndex = 2;
		this.txtBalanceFIPaymentDetail.TabStop = false;
		this.txtBalanceFIPaymentDetail.Text = "0";
		this.txtBalanceFIPaymentDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceFIPaymentDetail.Leave += new System.EventHandler(dtxtpercentoftotal_TextChanged);
		this.txtDueDaysPaymentDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDaysPaymentDetail.Location = new System.Drawing.Point(683, 28);
		this.txtDueDaysPaymentDetail.Name = "txtDueDaysPaymentDetail";
		this.txtDueDaysPaymentDetail.Size = new System.Drawing.Size(63, 22);
		this.txtDueDaysPaymentDetail.TabIndex = 5;
		this.txtDueDaysPaymentDetail.Text = "0";
		this.txtDueDaysPaymentDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(683, 8);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(58, 15);
		this.label78.TabIndex = 515;
		this.label78.Text = "Due Days";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(567, 8);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(72, 15);
		this.label77.TabIndex = 513;
		this.label77.Text = "Fcy Amount";
		this.txtFcyAmountPaymentDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountPaymentDetail.Location = new System.Drawing.Point(568, 28);
		this.txtFcyAmountPaymentDetail.Name = "txtFcyAmountPaymentDetail";
		this.txtFcyAmountPaymentDetail.Size = new System.Drawing.Size(114, 22);
		this.txtFcyAmountPaymentDetail.TabIndex = 4;
		this.txtFcyAmountPaymentDetail.Text = "0";
		this.txtFcyAmountPaymentDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountPaymentDetail.Leave += new System.EventHandler(dtxtfcyamount_TextChanged);
		this.panel11.Controls.Add(this.HeaderGropBox);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1046, 263);
		this.panel11.TabIndex = 3;
		this.HeaderGropBox.Controls.Add(this.txtDocNo);
		this.HeaderGropBox.Controls.Add(this.label8);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImportCompany);
		this.HeaderGropBox.Controls.Add(this.labelExportCompany);
		this.HeaderGropBox.Controls.Add(this.BtnDefineInvoiceNo);
		this.HeaderGropBox.Controls.Add(this.BtnDefineContractNo);
		this.HeaderGropBox.Controls.Add(this.BtnDefinePerformaNo);
		this.HeaderGropBox.Controls.Add(this.label17);
		this.HeaderGropBox.Controls.Add(this.label104);
		this.HeaderGropBox.Controls.Add(this.label5);
		this.HeaderGropBox.Controls.Add(this.label109);
		this.HeaderGropBox.Controls.Add(this.label51);
		this.HeaderGropBox.Controls.Add(this.label59);
		this.HeaderGropBox.Controls.Add(this.label1);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDabitAccount);
		this.HeaderGropBox.Controls.Add(this.lblCreditAc);
		this.HeaderGropBox.Controls.Add(this.txtContractDate);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImporterName);
		this.HeaderGropBox.Controls.Add(this.label60);
		this.HeaderGropBox.Controls.Add(this.txtPerformaDate);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractNo);
		this.HeaderGropBox.Controls.Add(this.label25);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPerformaNo);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalesPerson);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbConsignee);
		this.HeaderGropBox.Controls.Add(this.label103);
		this.HeaderGropBox.Controls.Add(this.label6);
		this.HeaderGropBox.Controls.Add(this.label98);
		this.HeaderGropBox.Controls.Add(this.label101);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExporterName);
		this.HeaderGropBox.Controls.Add(this.label99);
		this.HeaderGropBox.Controls.Add(this.label97);
		this.HeaderGropBox.Controls.Add(this.label86);
		this.HeaderGropBox.Controls.Add(this.label4);
		this.HeaderGropBox.Controls.Add(this.label96);
		this.HeaderGropBox.Controls.Add(this.label114);
		this.HeaderGropBox.Controls.Add(this.txtLotRef);
		this.HeaderGropBox.Controls.Add(this.label7);
		this.HeaderGropBox.Controls.Add(this.label28);
		this.HeaderGropBox.Controls.Add(this.label21);
		this.HeaderGropBox.Controls.Add(this.label108);
		this.HeaderGropBox.Controls.Add(this.label57);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIncomeTerm);
		this.HeaderGropBox.Controls.Add(this.label24);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCode);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbexporterBankNew);
		this.HeaderGropBox.Controls.Add(this.label16);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbimporterBankNew);
		this.HeaderGropBox.Controls.Add(this.label31);
		this.HeaderGropBox.Controls.Add(this.txtNoOfPackagesHeader);
		this.HeaderGropBox.Controls.Add(this.label110);
		this.HeaderGropBox.Controls.Add(this.label113);
		this.HeaderGropBox.Controls.Add(this.label46);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.HeaderGropBox.Controls.Add(this.txtInvoiceDate);
		this.HeaderGropBox.Controls.Add(this.txtBLNo);
		this.HeaderGropBox.Controls.Add(this.txtBlDate);
		this.HeaderGropBox.Controls.Add(this.txtGdValue);
		this.HeaderGropBox.Controls.Add(this.txtGdNo);
		this.HeaderGropBox.Controls.Add(this.txtGdDate);
		this.HeaderGropBox.Controls.Add(this.txtNoOfContainerHeader);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.HeaderGropBox.Controls.Add(this.label27);
		this.HeaderGropBox.Controls.Add(this.label3);
		this.HeaderGropBox.Controls.Add(this.label55);
		this.HeaderGropBox.Controls.Add(this.label107);
		this.HeaderGropBox.Controls.Add(this.label56);
		this.HeaderGropBox.Controls.Add(this.txtfcyAmountHeader);
		this.HeaderGropBox.Controls.Add(this.txtNetWeightHeader);
		this.HeaderGropBox.Controls.Add(this.label11);
		this.HeaderGropBox.Controls.Add(this.label50);
		this.HeaderGropBox.Controls.Add(this.txtExRate);
		this.HeaderGropBox.Controls.Add(this.label49);
		this.HeaderGropBox.Controls.Add(this.label47);
		this.HeaderGropBox.Controls.Add(this.label54);
		this.HeaderGropBox.Controls.Add(this.label48);
		this.HeaderGropBox.Controls.Add(this.label44);
		this.HeaderGropBox.Controls.Add(this.label53);
		this.HeaderGropBox.Controls.Add(this.txtGrossWeightHeader);
		this.HeaderGropBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.HeaderGropBox.Location = new System.Drawing.Point(4, 2);
		this.HeaderGropBox.Name = "HeaderGropBox";
		this.HeaderGropBox.Size = new System.Drawing.Size(1038, 259);
		this.HeaderGropBox.TabIndex = 0;
		this.HeaderGropBox.TabStop = false;
		this.HeaderGropBox.Text = "Main Header";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(118, 16);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(97, 22);
		this.txtDocNo.TabIndex = 0;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(3, 20);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 15);
		this.label8.TabIndex = 561;
		this.label8.Text = "Doc No";
		this.CmbImportCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbImportCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImportCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImportCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImportCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbImportCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance174).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance287).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance323;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImportCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImportCompany).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbImportCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImportCompany).Location = new System.Drawing.Point(778, 229);
		((System.Windows.Forms.Control)(object)this.CmbImportCompany).Name = "CmbImportCompany";
		((System.Windows.Forms.Control)(object)this.CmbImportCompany).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbImportCompany).TabIndex = 35;
		this.labelExportCompany.AutoSize = true;
		this.labelExportCompany.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelExportCompany.ForeColor = System.Drawing.Color.Black;
		this.labelExportCompany.Location = new System.Drawing.Point(680, 235);
		this.labelExportCompany.Name = "labelExportCompany";
		this.labelExportCompany.Size = new System.Drawing.Size(100, 15);
		this.labelExportCompany.TabIndex = 501;
		this.labelExportCompany.Text = "Import Company";
		this.BtnDefineInvoiceNo.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineInvoiceNo.Location = new System.Drawing.Point(374, 204);
		this.BtnDefineInvoiceNo.Name = "BtnDefineInvoiceNo";
		this.BtnDefineInvoiceNo.Size = new System.Drawing.Size(29, 22);
		this.BtnDefineInvoiceNo.TabIndex = 14;
		this.BtnDefineInvoiceNo.TabStop = false;
		this.BtnDefineInvoiceNo.UseVisualStyleBackColor = true;
		this.BtnDefineInvoiceNo.Click += new System.EventHandler(BtnDefineInvoiceNo_Click);
		this.BtnDefineContractNo.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineContractNo.Location = new System.Drawing.Point(374, 178);
		this.BtnDefineContractNo.Name = "BtnDefineContractNo";
		this.BtnDefineContractNo.Size = new System.Drawing.Size(29, 22);
		this.BtnDefineContractNo.TabIndex = 11;
		this.BtnDefineContractNo.TabStop = false;
		this.BtnDefineContractNo.UseVisualStyleBackColor = true;
		this.BtnDefineContractNo.Click += new System.EventHandler(BtnDefineContractNo_Click);
		this.BtnDefinePerformaNo.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefinePerformaNo.Location = new System.Drawing.Point(374, 151);
		this.BtnDefinePerformaNo.Name = "BtnDefinePerformaNo";
		this.BtnDefinePerformaNo.Size = new System.Drawing.Size(29, 22);
		this.BtnDefinePerformaNo.TabIndex = 8;
		this.BtnDefinePerformaNo.TabStop = false;
		this.BtnDefinePerformaNo.UseVisualStyleBackColor = true;
		this.BtnDefinePerformaNo.Click += new System.EventHandler(BtnDefinePerformaNo_Click);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(104, 207);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 595;
		this.label17.Text = "*";
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.ForeColor = System.Drawing.Color.Red;
		this.label104.Location = new System.Drawing.Point(104, 181);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(14, 17);
		this.label104.TabIndex = 9;
		this.label104.Text = "*";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Red;
		this.label5.Location = new System.Drawing.Point(252, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(14, 17);
		this.label5.TabIndex = 1;
		this.label5.Text = "*";
		this.label5.Visible = false;
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label109.ForeColor = System.Drawing.Color.Red;
		this.label109.Location = new System.Drawing.Point(500, 19);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(14, 17);
		this.label109.TabIndex = 592;
		this.label109.Text = "*";
		this.label109.Visible = false;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(500, 46);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(14, 17);
		this.label51.TabIndex = 592;
		this.label51.Text = "*";
		this.label51.Visible = false;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.Red;
		this.label59.Location = new System.Drawing.Point(104, 127);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(14, 17);
		this.label59.TabIndex = 595;
		this.label59.Text = "*";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(500, 127);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 43;
		this.label1.Text = "*";
		this.label1.Visible = false;
		this.CmbDabitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDabitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDabitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDabitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDabitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.CmbDabitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance294).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance377).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance377).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance388;
		((AppearanceBase)appearance399).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance399).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance399;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance410).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance410).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance410).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance410).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance410).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance410;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDabitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDabitAccount).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDabitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDabitAccount).Location = new System.Drawing.Point(779, 203);
		((System.Windows.Forms.Control)(object)this.CmbDabitAccount).Name = "CmbDabitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDabitAccount).Size = new System.Drawing.Size(253, 25);
		((System.Windows.Forms.Control)(object)this.CmbDabitAccount).TabIndex = 34;
		this.lblCreditAc.AutoSize = true;
		this.lblCreditAc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblCreditAc.Location = new System.Drawing.Point(680, 208);
		this.lblCreditAc.Name = "lblCreditAc";
		this.lblCreditAc.Size = new System.Drawing.Size(58, 15);
		this.lblCreditAc.TabIndex = 589;
		this.lblCreditAc.Text = "Debit A/c";
		this.txtContractDate.CustomFormat = "dd-MMM-yy";
		this.txtContractDate.Enabled = false;
		this.txtContractDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContractDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtContractDate.Location = new System.Drawing.Point(266, 178);
		this.txtContractDate.Name = "txtContractDate";
		this.txtContractDate.Size = new System.Drawing.Size(106, 22);
		this.txtContractDate.TabIndex = 10;
		this.CmbImporterName.AllowNull = (DefaultableBoolean)1;
		this.CmbImporterName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImporterName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImporterName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance270).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance270).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance271;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance277).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance278).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance279).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbImporterName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Location = new System.Drawing.Point(118, 42);
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Name = "CmbImporterName";
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.CmbImporterName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Leave += new System.EventHandler(cmbSupCust_Leave);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(500, 73);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 573;
		this.label60.Text = "*";
		this.txtPerformaDate.CustomFormat = "dd-MMM-yy";
		this.txtPerformaDate.Enabled = false;
		this.txtPerformaDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPerformaDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPerformaDate.Location = new System.Drawing.Point(266, 151);
		this.txtPerformaDate.Name = "txtPerformaDate";
		this.txtPerformaDate.Size = new System.Drawing.Size(106, 22);
		this.txtPerformaDate.TabIndex = 7;
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(118, 203);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(146, 25);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_ValueChanged);
		this.CmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.CmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance362).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance362;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance363).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance363).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance363).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance363;
		((AppearanceBase)appearance364).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance364;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance365).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance365).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance365).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance365;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance366).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance366).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance366;
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance368).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance368;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance369;
		((AppearanceBase)appearance370).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance370).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance370;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance371).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance371).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance371).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance371).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance371;
		((AppearanceBase)appearance372).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance372;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance373).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance373;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance374).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance374;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Location = new System.Drawing.Point(118, 177);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Name = "CmbContractNo";
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Size = new System.Drawing.Size(146, 25);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Leave += new System.EventHandler(CmbContractNo_ValueChanged);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(408, 128);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(78, 15);
		this.label25.TabIndex = 538;
		this.label25.Text = "Invoice Value";
		this.CmbPerformaNo.AllowNull = (DefaultableBoolean)1;
		this.CmbPerformaNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPerformaNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPerformaNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance297).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance297).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance297).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPerformaNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance297;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance298;
		((SpecialBoxBase)((UltraGridBase)this.CmbPerformaNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance299).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance299).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance302).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance303;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance304).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance305).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance305).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance305).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance305;
		((AppearanceBase)appearance306).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance307).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPerformaNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).Location = new System.Drawing.Point(118, 150);
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).Name = "CmbPerformaNo";
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).Size = new System.Drawing.Size(146, 25);
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).TabIndex = 6;
		this.CmbPerformaNo.ValueChanged += new System.EventHandler(CmbPerformaNo_ValueChanged);
		this.CmbSalesPerson.AllowNull = (DefaultableBoolean)1;
		this.CmbSalesPerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalesPerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalesPerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance310).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance310).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalesPerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance310;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance311;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalesPerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance313).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance313).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance314).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance315).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance316;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance317).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance318).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance318).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance318).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance318;
		((AppearanceBase)appearance319).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance320).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalesPerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalesPerson).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalesPerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalesPerson).Location = new System.Drawing.Point(118, 96);
		((System.Windows.Forms.Control)(object)this.CmbSalesPerson).Name = "CmbSalesPerson";
		((System.Windows.Forms.Control)(object)this.CmbSalesPerson).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.CmbSalesPerson).TabIndex = 4;
		this.cmbConsignee.AllowNull = (DefaultableBoolean)1;
		this.cmbConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance324).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance324).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance324).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance324;
		((AppearanceBase)appearance325).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance325;
		((SpecialBoxBase)((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance326).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance326).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance326).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance326;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance327).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance327;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance328).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance329;
		((AppearanceBase)appearance330).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance330).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance330;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance331).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance331).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance331).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance331).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance331;
		((AppearanceBase)appearance332).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance333).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance335;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbConsignee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Location = new System.Drawing.Point(118, 123);
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Name = "cmbConsignee";
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.cmbConsignee).TabIndex = 5;
		this.label103.AutoSize = true;
		this.label103.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label103.Location = new System.Drawing.Point(3, 182);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(99, 15);
		this.label103.TabIndex = 594;
		this.label103.Text = "Contract No,Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(104, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 17);
		this.label6.TabIndex = 115;
		this.label6.Text = "*";
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.ForeColor = System.Drawing.Color.Red;
		this.label98.Location = new System.Drawing.Point(104, 46);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(14, 17);
		this.label98.TabIndex = 115;
		this.label98.Text = "*";
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label101.Location = new System.Drawing.Point(3, 155);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(103, 15);
		this.label101.TabIndex = 594;
		this.label101.Text = "Performa No,Date";
		this.CmbExporterName.AllowNull = (DefaultableBoolean)1;
		this.CmbExporterName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExporterName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExporterName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance336).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance337).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance337).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance337).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance338).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance338;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance339).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance339).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance339).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance340).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance340;
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance341).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance343).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance343).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance344).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance344).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance344).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance344).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance344;
		((AppearanceBase)appearance346).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance346;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance347).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance347;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance348;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExporterName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Location = new System.Drawing.Point(118, 69);
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Name = "CmbExporterName";
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.CmbExporterName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Leave += new System.EventHandler(cmbSupCust_Leave);
		this.label99.AutoSize = true;
		this.label99.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label99.Location = new System.Drawing.Point(3, 101);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(73, 15);
		this.label99.TabIndex = 594;
		this.label99.Text = "Sales Person";
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.ForeColor = System.Drawing.Color.Red;
		this.label97.Location = new System.Drawing.Point(104, 46);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(14, 17);
		this.label97.TabIndex = 111;
		this.label97.Text = "*";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label86.Location = new System.Drawing.Point(3, 128);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(62, 15);
		this.label86.TabIndex = 594;
		this.label86.Text = "Consignee";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(104, 73);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 17);
		this.label4.TabIndex = 115;
		this.label4.Text = "*";
		this.label96.AutoSize = true;
		this.label96.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label96.Location = new System.Drawing.Point(3, 74);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(51, 15);
		this.label96.TabIndex = 78;
		this.label96.Text = "Exporter";
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label114.Location = new System.Drawing.Point(3, 208);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(93, 15);
		this.label114.TabIndex = 57;
		this.label114.Text = "Invoice No,Date";
		this.txtLotRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotRef.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotRef.Location = new System.Drawing.Point(118, 231);
		this.txtLotRef.Name = "txtLotRef";
		this.txtLotRef.Size = new System.Drawing.Size(285, 22);
		this.txtLotRef.TabIndex = 15;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(3, 235);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(119, 15);
		this.label7.TabIndex = 57;
		this.label7.Text = "Importer Reference #";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(104, 73);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 111;
		this.label28.Text = "*";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(3, 47);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(54, 15);
		this.label21.TabIndex = 78;
		this.label21.Text = "Importer";
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label108.Location = new System.Drawing.Point(408, 155);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(91, 15);
		this.label108.TabIndex = 552;
		this.label108.Text = "No.Of Packages";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(408, 182);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(92, 15);
		this.label57.TabIndex = 552;
		this.label57.Text = "Total Containers";
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(266, 15);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(137, 25);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 1;
		this.CmbIncomeTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbIncomeTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIncomeTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIncomeTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIncomeTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).Location = new System.Drawing.Point(514, 15);
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).Name = "CmbIncomeTerm";
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).TabIndex = 16;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(680, 47);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(80, 15);
		this.label24.TabIndex = 525;
		this.label24.Text = "Exporter Bank";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbFcyCode.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFcyCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Location = new System.Drawing.Point(514, 69);
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Name = "CmbFcyCode";
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).TabIndex = 18;
		this.cmbexporterBankNew.AllowNull = (DefaultableBoolean)1;
		this.cmbexporterBankNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbexporterBankNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbexporterBankNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance398).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance398).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance398;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance400).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance400).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance400).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance400).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance400;
		((AppearanceBase)appearance401).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance401;
		((SpecialBoxBase)((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance402).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance402).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance402).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance402).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance402;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance403).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance403).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance403;
		((AppearanceBase)appearance404).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance404).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance404;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbexporterBankNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Location = new System.Drawing.Point(779, 42);
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Name = "cmbexporterBankNew";
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Size = new System.Drawing.Size(253, 25);
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).TabIndex = 26;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(680, 101);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(93, 15);
		this.label16.TabIndex = 533;
		this.label16.Text = "Destination Port";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbPaymentTermsNew.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermsNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermsNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermsNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance349).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance349;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance350).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance350).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance350).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance350).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance350;
		((AppearanceBase)appearance351).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance351;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance352).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance352).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance352).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance352;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance353).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance353).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance353;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance354).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance354;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance355;
		((AppearanceBase)appearance357).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance357).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance357;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance358).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance358).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance358).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance358).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance358;
		((AppearanceBase)appearance359).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance359;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance360).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance360;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance361).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance361;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermsNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).Location = new System.Drawing.Point(514, 42);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).Name = "CmbPaymentTermsNew";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).TabIndex = 17;
		this.cmbimporterBankNew.AllowNull = (DefaultableBoolean)1;
		this.cmbimporterBankNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbimporterBankNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbimporterBankNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance71).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbimporterBankNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Location = new System.Drawing.Point(779, 15);
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Name = "cmbimporterBankNew";
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Size = new System.Drawing.Size(253, 25);
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).TabIndex = 25;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(680, 20);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(83, 15);
		this.label31.TabIndex = 524;
		this.label31.Text = "Importer Bank";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtNoOfPackagesHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfPackagesHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfPackagesHeader.Location = new System.Drawing.Point(514, 151);
		this.txtNoOfPackagesHeader.Name = "txtNoOfPackagesHeader";
		this.txtNoOfPackagesHeader.Size = new System.Drawing.Size(163, 22);
		this.txtNoOfPackagesHeader.TabIndex = 21;
		this.txtNoOfPackagesHeader.Text = "0";
		this.txtNoOfPackagesHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label110.Location = new System.Drawing.Point(680, 128);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(70, 15);
		this.label110.TabIndex = 561;
		this.label110.Text = "B.L No,Date";
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label113.Location = new System.Drawing.Point(680, 182);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(59, 15);
		this.label113.TabIndex = 561;
		this.label113.Text = "G.D Value";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(680, 155);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(74, 15);
		this.label46.TabIndex = 561;
		this.label46.Text = "G.D No,Date";
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance418).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance418).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance418;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance419).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance419).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance419).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance419).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance419;
		((AppearanceBase)appearance420).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance420;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(779, 69);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(253, 25);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 27;
		this.txtInvoiceDate.CustomFormat = "dd-MMM-yy";
		this.txtInvoiceDate.Enabled = false;
		this.txtInvoiceDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtInvoiceDate.Location = new System.Drawing.Point(266, 204);
		this.txtInvoiceDate.Name = "txtInvoiceDate";
		this.txtInvoiceDate.Size = new System.Drawing.Size(106, 22);
		this.txtInvoiceDate.TabIndex = 13;
		this.txtBLNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBLNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBLNo.Location = new System.Drawing.Point(779, 124);
		this.txtBLNo.Name = "txtBLNo";
		this.txtBLNo.Size = new System.Drawing.Size(146, 22);
		this.txtBLNo.TabIndex = 29;
		this.txtBlDate.CustomFormat = "dd-MMM-yy";
		this.txtBlDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBlDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBlDate.Location = new System.Drawing.Point(926, 124);
		this.txtBlDate.Name = "txtBlDate";
		this.txtBlDate.Size = new System.Drawing.Size(106, 22);
		this.txtBlDate.TabIndex = 30;
		this.txtGdValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGdValue.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdValue.Location = new System.Drawing.Point(779, 178);
		this.txtGdValue.Name = "txtGdValue";
		this.txtGdValue.Size = new System.Drawing.Size(253, 22);
		this.txtGdValue.TabIndex = 33;
		this.txtGdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGdNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdNo.Location = new System.Drawing.Point(779, 151);
		this.txtGdNo.Name = "txtGdNo";
		this.txtGdNo.Size = new System.Drawing.Size(146, 22);
		this.txtGdNo.TabIndex = 31;
		this.txtGdDate.CustomFormat = "dd-MMM-yy";
		this.txtGdDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGdDate.Location = new System.Drawing.Point(926, 151);
		this.txtGdDate.Name = "txtGdDate";
		this.txtGdDate.Size = new System.Drawing.Size(106, 22);
		this.txtGdDate.TabIndex = 32;
		this.txtNoOfContainerHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainerHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainerHeader.Location = new System.Drawing.Point(514, 178);
		this.txtNoOfContainerHeader.Name = "txtNoOfContainerHeader";
		this.txtNoOfContainerHeader.Size = new System.Drawing.Size(163, 22);
		this.txtNoOfContainerHeader.TabIndex = 22;
		this.txtNoOfContainerHeader.Text = "0";
		this.txtNoOfContainerHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance405).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance405).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance405;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance406).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance406).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance406).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance406).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance406;
		((AppearanceBase)appearance407).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance407;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance408).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance408).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance408).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance408).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance408;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance409).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance409).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance409;
		((AppearanceBase)appearance411).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance411).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance411;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance412).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance412;
		((AppearanceBase)appearance413).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance413).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance413;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance414).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance414).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance414).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance414).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance414).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance414;
		((AppearanceBase)appearance415).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance415;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance416).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance416).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance416;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance417).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance417;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(779, 96);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(253, 25);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 28;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(408, 74);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(84, 15);
		this.label27.TabIndex = 537;
		this.label27.Text = "Currency Code";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(215, 20);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(40, 15);
		this.label3.TabIndex = 527;
		this.label3.Text = "Status";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(408, 208);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(67, 15);
		this.label55.TabIndex = 554;
		this.label55.Text = "Net MTon's";
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label107.Location = new System.Drawing.Point(408, 20);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(78, 15);
		this.label107.TabIndex = 527;
		this.label107.Text = "Income Term";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(680, 74);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(75, 15);
		this.label56.TabIndex = 534;
		this.label56.Text = "Loading Port";
		this.txtfcyAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyAmountHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfcyAmountHeader.Location = new System.Drawing.Point(514, 124);
		this.txtfcyAmountHeader.Name = "txtfcyAmountHeader";
		this.txtfcyAmountHeader.ReadOnly = true;
		this.txtfcyAmountHeader.Size = new System.Drawing.Size(163, 22);
		this.txtfcyAmountHeader.TabIndex = 20;
		this.txtfcyAmountHeader.Text = "0";
		this.txtfcyAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcyAmountHeader.TextChanged += new System.EventHandler(txtfcyAmount_TextChanged);
		this.txtNetWeightHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightHeader.Location = new System.Drawing.Point(514, 204);
		this.txtNetWeightHeader.Name = "txtNetWeightHeader";
		this.txtNetWeightHeader.ReadOnly = true;
		this.txtNetWeightHeader.Size = new System.Drawing.Size(163, 22);
		this.txtNetWeightHeader.TabIndex = 23;
		this.txtNetWeightHeader.Text = "0";
		this.txtNetWeightHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(408, 47);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(84, 15);
		this.label11.TabIndex = 527;
		this.label11.Text = "Payment Term";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(500, 234);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 569;
		this.label50.Text = "*";
		this.txtExRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExRate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExRate.Location = new System.Drawing.Point(514, 97);
		this.txtExRate.Name = "txtExRate";
		this.txtExRate.Size = new System.Drawing.Size(163, 22);
		this.txtExRate.TabIndex = 19;
		this.txtExRate.Text = "0";
		this.txtExRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(500, 207);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 560;
		this.label49.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(500, 100);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 543;
		this.label47.Text = "*";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(408, 235);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(77, 15);
		this.label54.TabIndex = 553;
		this.label54.Text = "Gross MTon's";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(500, 154);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 17);
		this.label48.TabIndex = 570;
		this.label48.Text = "*";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(408, 101);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 15);
		this.label44.TabIndex = 546;
		this.label44.Text = "Exchange Rate";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(500, 181);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 570;
		this.label53.Text = "*";
		this.txtGrossWeightHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeightHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeightHeader.Location = new System.Drawing.Point(514, 231);
		this.txtGrossWeightHeader.Name = "txtGrossWeightHeader";
		this.txtGrossWeightHeader.Size = new System.Drawing.Size(163, 22);
		this.txtGrossWeightHeader.TabIndex = 24;
		this.txtGrossWeightHeader.Text = "0";
		this.txtGrossWeightHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchName);
		this.panel10.Controls.Add(this.labelBranchName);
		this.panel10.Controls.Add(this.label23);
		this.panel10.Controls.Add(this.ChkPrintPreview);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1046, 27);
		this.panel10.TabIndex = 1;
		this.CmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance375).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance375).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance392).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance392).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance392).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance392).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance392;
		((AppearanceBase)appearance393).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance393;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance394).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance394).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance394).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance394;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance395).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance395).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance395;
		((AppearanceBase)appearance396).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance396).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance396;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance397).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance397;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranchName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Location = new System.Drawing.Point(218, 1);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Name = "CmbBranchName";
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).TabIndex = 500;
		this.labelBranchName.AutoSize = true;
		this.labelBranchName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelBranchName.ForeColor = System.Drawing.Color.White;
		this.labelBranchName.Location = new System.Drawing.Point(168, 7);
		this.labelBranchName.Name = "labelBranchName";
		this.labelBranchName.Size = new System.Drawing.Size(49, 15);
		this.labelBranchName.TabIndex = 501;
		this.labelBranchName.Text = "Branch ";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(129, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Import Invoice";
		this.ChkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreview.Location = new System.Drawing.Point(958, 4);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPrintPreview.TabIndex = 32;
		this.ChkPrintPreview.Text = "Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add(this.toolStrip4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1046, 27);
		this.panel12.TabIndex = 0;
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnRefresh, this.btnsave, this.btnupdate, this.btnAttachment, this.btnPrint, this.btnShortCutKey });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1046, 27);
		this.toolStrip4.TabIndex = 4;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
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
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "Print-902";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Controls.Add(this.panel14);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailHistory);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1052, 682);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 156);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1046, 252);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label92);
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 128);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1046, 28);
		this.panel15.TabIndex = 47;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(2, 3);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 1;
		this.label92.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(998, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1046, 73);
		this.panel3.TabIndex = 46;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.label73);
		this.groupBox2.Controls.Add(this.label89);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.label91);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(932, 71);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
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
		this.btnShowHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(373, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(89, 15);
		this.label20.TabIndex = 87;
		this.label20.Text = "Importer Name";
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
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 46);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 46);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance345).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance356).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance356).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance356).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance356;
		((AppearanceBase)appearance367).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance367;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance376).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance376).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance376).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance376;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance378).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance378;
		((AppearanceBase)appearance379).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance379).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance379;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance382;
		((AppearanceBase)appearance383).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance383).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance383;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance384).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance384).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance384).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance384).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance384;
		((AppearanceBase)appearance385).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance385;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance386).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance386;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance387).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance387;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(462, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(266, 26);
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
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label69);
		this.panel16.Controls.Add(this.toolStrip3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1046, 52);
		this.panel16.TabIndex = 0;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(2, 28);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(192, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Import Invoice History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.btnRefreshHistory, this.toolStripSeparator1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1046, 27);
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
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label93);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel14.Location = new System.Drawing.Point(3, 408);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1046, 22);
		this.panel14.TabIndex = 2;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label93.ForeColor = System.Drawing.Color.White;
		this.label93.Location = new System.Drawing.Point(3, 3);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(213, 14);
		this.label93.TabIndex = 1;
		this.label93.Text = "Detail of the Selected above Row";
		this.gridDetailHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailHistory.DesignTimeLayout = gridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.gridDetailHistory.DynamicFiltering = true;
		this.gridDetailHistory.FilterMode = (FilterMode)2;
		this.gridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Location = new System.Drawing.Point(3, 430);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Name = "gridDetailHistory";
		((ScrollableBase)this.gridDetailHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Size = new System.Drawing.Size(1046, 249);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).TabIndex = 1;
		this.gridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1060, 711);
		this.panel1.TabIndex = 1;
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance301).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance301).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance301;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance312;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance380).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance380).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance380).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance380).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance381).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance381;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance289).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance290).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFCurrencr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Location = new System.Drawing.Point(385, 129);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Name = "cmbFCurrencr";
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).TabIndex = 12;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
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
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance291).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance293;
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
		this.CmbLCContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbLCContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLCContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLCContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance184).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CmbLCContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Location = new System.Drawing.Point(111, 120);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Name = "CmbLCContractor";
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).TabIndex = 4;
		this.cmbCarierType.AllowNull = (DefaultableBoolean)1;
		this.cmbCarierType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCarierType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCarierType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance193;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance199).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance200).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance203;
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
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance206;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
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
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance210;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance215).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance216).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
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
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance164).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance166;
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
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance169;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance175).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance179;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1060, 711);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmImportInvoice";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmImportInvoice_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOtherItem).EndInit();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstrumentsPaymentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPaymentDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.HeaderGropBox.ResumeLayout(false);
		this.HeaderGropBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbImportCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDabitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPerformaNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalesPerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbConsignee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIncomeTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbexporterBankNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermsNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterBankNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).EndInit();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).EndInit();
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
