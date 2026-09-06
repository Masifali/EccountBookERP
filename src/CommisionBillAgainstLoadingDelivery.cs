// using Architecture.Reports;
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
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
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
namespace Architecture.WinApp.CmTr.TransactionForms
{

public class CommisionBillAgainstLoadingDelivery : Form
{
	private UserAccount UserAccount = new UserAccount();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtsummaryBySupplier = new DataTable();

	private DataTable dtPackingMaterial = new DataTable();

	private DataTable dtOtherRevenue = new DataTable();

	private DataTable dtCustomerRevenueExpense = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtCommission = new DataTable();

	private DataTable dtCustomerPTDetail = new DataTable();

	private DataTable dtSupplierPTDetail = new DataTable();

	private DataTable dtVehicleDetail = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtUOM = new DataTable();

	private DataTable dtReveneuExpenseAccount = new DataTable();

	private DataTable dtCustomerRevExpAccounts = new DataTable();

	private DataTable dtgrid = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtitemOfDetail = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable dtStockType = new DataTable();

	private DataTable dtTerm = new DataTable();

	private DataTable dtCustomerBindForPT = new DataTable();

	private DataTable dtSupplierBindForPT = new DataTable();

	private DataTable dtWare = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private bool HasMultiCurrencyFeature;

	private int RecId;

	private int FluctuationDifferenceAccountId;

	private decimal FluctuationDifferenceAccountValue;

	private decimal PmCurrRowQty;

	private decimal PmCurrRowAmount;

	private frmLoadFromLoadingDelivery LoadFromLoadingDelivery;

	public string LoadDeliveryDetailIds = "";

	public string LoadDeliveryMainIds = "";

	private bool HasMultiLanguageFeature;

	private List<CommTradeTransactionDetail> lstRemoveRecord = new List<CommTradeTransactionDetail>();

	private IContainer components;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnDelete;

	private ToolStripButton btnRefresh;

	private GroupBox groupBox1;

	private Label label7;

	internal UltraCombo cmbCustomername;

	private Label label17;

	private TextBox txtbillno;

	private Label label2;

	internal UltraCombo cmbpayterm;

	private Label label3;

	private TextBox txtduedays;

	private DateTimePicker DocDate;

	private Label label6;

	internal UltraCombo cmbdeliveryterm;

	private Label label5;

	private TextBox txtmanualref;

	private Label label9;

	private TextBox txtBiltyNos;

	private Label label8;

	private TextBox txtVehicleNos;

	private Label label10;

	private RichTextBox txtRemarks;

	private Label label11;

	private Label label12;

	internal UltraCombo cmbCommAgent;

	private Label label19;

	private TextBox txtbrokeryamount;

	private Label label18;

	private TextBox txtbrokeryrate;

	private Label label15;

	internal UltraCombo cmbcommtype;

	private Label label14;

	private TextBox txtcommamount;

	private Label label13;

	private TextBox txtcommrate;

	private Label label20;

	private TextBox txtcustbillamnt;

	private Label label21;

	internal UltraCombo cmbcurrency;

	private Label label23;

	private TextBox txtfcyamount;

	private Label label22;

	private TextBox txtexchangerate;

	private Label label25;

	internal UltraCombo cmbsubbrokeryacc;

	private Label label27;

	private TextBox txtsubbrokeryamnt;

	private Label label26;

	private TextBox txtsubbrokeryrate;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnShortcutKeys;

	private Panel panel10;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label29;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label28;

	private Label label4;

	private TabControl tabControl1;

	private TabPage Form;

	internal GridEX grdrevenue;

	internal GridEX grdPackingMaterial;

	private Panel panel12;

	private Label label30;

	private Panel panel1;

	private Panel panel17;

	private Label label1;

	private TabPage History;

	internal GridEX grdHistoryDetail;

	private Panel panel8;

	private Label label31;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdHistory;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnLoadAllHistory;

	private Label label16;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton BtnLoadFromLoadingDelivery;

	private TextBox txtBiltyDates;

	internal UltraCombo cmbDifferencAccount;

	private Label label33;

	private GroupBox groupBox2;

	internal UltraCombo cmbBrokeryAccount;

	private Label label32;

	private ToolStripButton btnVoucher;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripSeparator toolStripSeparator6;

	private Panel panel3;

	private Panel panel4;

	private Label label24;

	private Panel panel11;

	private GroupBox groupBox3;

	internal GridEX grdsummary;

	private CheckBox ChkPrintslip;

	private CheckBox ChkBok;

	private ToolStripButton btnMultiLangPrint;

	internal UltraCombo CmbMultiLanguage;

	private Label labelmultilang;

	private ToolStripDropDownButton tsMultiLangPrint;

	private CheckBox ChkUrduPrintPreview;

	private TabControl tabControl2;

	private TabPage tabPage1;

	internal GridEX grdDetail;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdCustomerRevenueExpense;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar5;

	private Label label34;

	private TabPage tabCustomerPaymentDetail;

	private TabPage tabPage6;

	internal GridEX grdPaymentTermCustomer;

	private Panel panel5;

	private Label label57;

	internal GridEX grdPaymentTermSupplier;

	private Panel panel13;

	private Label label35;

	private ToolStripButton btnPrintSupplier1704_01;

	private TabPage tabVehicle;

	internal GridEX grdVehicleDetail;

	private Panel panel14;

	private Label label36;

	public CommisionBillAgainstLoadingDelivery(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void CommisionBill_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			BtnPrint.Enabled = formright.DoHavePrintRights;
			btnMultiLangPrint.Enabled = formright.DoHavePrintRights;
			tsMultiLangPrint.Enabled = formright.DoHavePrintRights;
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
			ctrlGrdBar7.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar7.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar7.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar7.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar7.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar7.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			tabControl2.TabPages.Remove(tabCustomerPaymentDetail);
			PaymentTerms();
			dtStockType.Columns.Add("Id", typeof(int));
			dtStockType.Columns.Add("StockType", typeof(string));
			dtCommission.Columns.Add("Id", typeof(int));
			dtCommission.Columns.Add("CommissionAgentGLId", typeof(int));
			dtCommission.Columns.Add("CommissionAgentId", typeof(int));
			dtCommission.Columns.Add("ChargesIncentiveId", typeof(int));
			dtCommission.Columns.Add("RevenueExpenseId", typeof(int));
			dtCommission.Columns.Add("CalcParameterId", typeof(int));
			dtCommission.Columns.Add("Rate", typeof(decimal));
			dtCommission.Columns.Add("Amount", typeof(decimal));
			dtCommission.Columns.Add("SupplierCustomerId", typeof(int));
			dtsummaryBySupplier.Columns.Add("SupplierId", typeof(int));
			dtsummaryBySupplier.Columns.Add("ItemAmount", typeof(decimal));
			dtsummaryBySupplier.Columns.Add("PackingAmount", typeof(decimal));
			dtsummaryBySupplier.Columns.Add("Expenses", typeof(decimal));
			dtsummaryBySupplier.Columns.Add("BillAmount", typeof(decimal));
			grdsummary.DataSource = dtsummaryBySupplier;
			grdsummary.RetrieveStructure();
			grdsummarysetting();
			dtPackingMaterial.Columns.Add("Id", typeof(int));
			dtPackingMaterial.Columns.Add("WareHouseId", typeof(int));
			dtPackingMaterial.Columns.Add("StockTypeId", typeof(int));
			dtPackingMaterial.Columns.Add("LoadingDeliveryId", typeof(int));
			dtPackingMaterial.Columns.Add("LoadingDeliveryPmOtherId", typeof(int));
			dtPackingMaterial.Columns.Add("PackingTypeId", typeof(int));
			dtPackingMaterial.Columns.Add("ItemId", typeof(int));
			dtPackingMaterial.Columns.Add("ItemSize", typeof(string));
			dtPackingMaterial.Columns.Add("Weight", typeof(decimal));
			dtPackingMaterial.Columns.Add("Qty", typeof(decimal));
			dtPackingMaterial.Columns.Add("Rate", typeof(decimal));
			dtPackingMaterial.Columns.Add("Amount", typeof(decimal));
			dtPackingMaterial.Columns.Add("CreditAccountId", typeof(int));
			grdPackingMaterial.DataSource = dtPackingMaterial;
			grdPackingMaterial.RetrieveStructure();
			AddRowPackingmaterial();
			grdpackingsetting();
			dtOtherRevenue.Columns.Add("Id", typeof(int));
			dtOtherRevenue.Columns.Add("LoadingDeliveryId", typeof(int));
			dtOtherRevenue.Columns.Add("LoadingDeliveryRevExpId", typeof(int));
			dtOtherRevenue.Columns.Add("OtherRevenueId", typeof(int));
			dtOtherRevenue.Columns.Add("Qty", typeof(decimal));
			dtOtherRevenue.Columns.Add("Rate", typeof(decimal));
			dtOtherRevenue.Columns.Add("AmountDr", typeof(decimal));
			dtOtherRevenue.Columns.Add("AmountCr", typeof(decimal));
			dtOtherRevenue.Columns.Add("SupplierAccountId", typeof(int));
			grdrevenue.DataSource = dtOtherRevenue;
			grdrevenue.RetrieveStructure();
			grdrevenuesetting();
			AddExpenseAccountRowsOtherRevenue();
			dtCustomerRevenueExpense.Columns.Add("Id", typeof(int));
			dtCustomerRevenueExpense.Columns.Add("LoadingDeliveryId", typeof(int));
			dtCustomerRevenueExpense.Columns.Add("LoadingDeliveryRevExpId", typeof(int));
			dtCustomerRevenueExpense.Columns.Add("OtherRevenueId", typeof(int));
			dtCustomerRevenueExpense.Columns.Add("Qty", typeof(decimal));
			dtCustomerRevenueExpense.Columns.Add("Rate", typeof(decimal));
			dtCustomerRevenueExpense.Columns.Add("AmountDr", typeof(decimal));
			dtCustomerRevenueExpense.Columns.Add("AmountCr", typeof(decimal));
			dtCustomerRevenueExpense.Columns.Add("SupplierAccountId", typeof(int));
			grdCustomerRevenueExpense.DataSource = dtCustomerRevenueExpense;
			grdCustomerRevenueExpense.RetrieveStructure();
			grdCustomerRevenueExpensesetting();
			dtVehicleDetail.Columns.Add("Id", typeof(int));
			dtVehicleDetail.Columns.Add("LoadingDeliveryId", typeof(int));
			dtVehicleDetail.Columns.Add("CustomerId", typeof(int));
			dtVehicleDetail.Columns.Add("VehicleNo", typeof(string));
			dtVehicleDetail.Columns.Add("BiltyNo", typeof(string));
			dtVehicleDetail.Columns.Add("VehicleQty", typeof(decimal));
			dtVehicleDetail.Columns.Add("BiltyFreight", typeof(decimal));
			dtVehicleDetail.Columns.Add("NetPaid", typeof(decimal));
			dtVehicleDetail.Columns.Add("Remarks", typeof(string));
			dtVehicleDetail.Columns.Add("CreditAccountId", typeof(int));
			grdVehicleDetail.DataSource = dtVehicleDetail;
			grdVehicleDetail.RetrieveStructure();
			grdVehicleDetailsetting();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("OrderId", typeof(int));
			dtDetail.Columns.Add("OrderDetailId", typeof(int));
			dtDetail.Columns.Add("OrderNo", typeof(string));
			dtDetail.Columns.Add("LoadingDeliveryId", typeof(int));
			dtDetail.Columns.Add("LoadingDeliveryDetailId", typeof(int));
			dtDetail.Columns.Add("LoadingDeliveryNo", typeof(string));
			dtDetail.Columns.Add("BiltyNo", typeof(string));
			dtDetail.Columns.Add("BiltyDate", typeof(DateTime));
			dtDetail.Columns.Add("VehicleNo", typeof(string));
			dtDetail.Columns.Add("VehicleQty", typeof(decimal));
			dtDetail.Columns.Add("VehicleGrossWeight", typeof(decimal));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("PackingTypeId", typeof(int));
			dtDetail.Columns.Add("UomId", typeof(int));
			dtDetail.Columns.Add("Uom", typeof(string));
			dtDetail.Columns.Add("ItemQty", typeof(decimal));
			dtDetail.Columns.Add("CustomerAccountId", typeof(int));
			dtDetail.Columns.Add("CusGrossWeight", typeof(decimal));
			dtDetail.Columns.Add("CusPMWeight", typeof(decimal));
			dtDetail.Columns.Add("CusAdlsWeight", typeof(decimal));
			dtDetail.Columns.Add("CusNetWeight", typeof(decimal));
			dtDetail.Columns.Add("CusRate", typeof(decimal));
			dtDetail.Columns.Add("CusAmount", typeof(decimal));
			dtDetail.Columns.Add("CusFcyAmount", typeof(decimal));
			dtDetail.Columns.Add("SupplierAccountId", typeof(int));
			dtDetail.Columns.Add("SupGrossWeight", typeof(decimal));
			dtDetail.Columns.Add("SupPMWeight", typeof(decimal));
			dtDetail.Columns.Add("SupAdlsWeight", typeof(decimal));
			dtDetail.Columns.Add("SupNetWeight", typeof(decimal));
			dtDetail.Columns.Add("SupRate", typeof(decimal));
			dtDetail.Columns.Add("SupAmount", typeof(decimal));
			dtDetail.Columns.Add("SupFcyAmount", typeof(decimal));
			dtDetail.Columns.Add("ProductTypeId", typeof(int));
			dtDetail.Columns.Add("BiltyFreight", typeof(decimal));
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			grdDetailsetting();
			dtSupplierPTDetail.Columns.Add("Id", typeof(int));
			dtSupplierPTDetail.Columns.Add("CommTradeLoadingDeliveryId", typeof(int));
			dtSupplierPTDetail.Columns.Add("LoadingDeliveryPaymentDetailId", typeof(int));
			dtSupplierPTDetail.Columns.Add("SupplierId", typeof(int));
			dtSupplierPTDetail.Columns.Add("PaymentTermId", typeof(int));
			dtSupplierPTDetail.Columns.Add("%ofTotal", typeof(decimal));
			dtSupplierPTDetail.Columns.Add("Amount", typeof(decimal));
			dtSupplierPTDetail.Columns.Add("DueDays", typeof(int));
			dtSupplierPTDetail.Columns.Add("DueDate", typeof(DateTime));
			dtSupplierPTDetail.Columns.Add("Remarks");
			AddRowInPaymentTermSupplierGrid();
			grdPaymentTermSupplier.RetrieveStructure();
			grdSettingsPaymentTermSupplier();
			dtitemOfDetail.Columns.Add("ItemId", typeof(int));
			dtitemOfDetail.Columns.Add("ItemName", typeof(string));
			dtitemOfDetail.Columns.Add("ItemCode", typeof(string));
			dtSupplierBindForPT.Columns.Add("SupplierId", typeof(int));
			dtSupplierBindForPT.Columns.Add("SupplierName", typeof(string));
			HasMultiLanguageFeature = CommonServices.GetERPFeatureById(7);
			if (HasMultiLanguageFeature)
			{
				labelmultilang.Visible = true;
				((Control)(object)CmbMultiLanguage).Visible = true;
				btnMultiLangPrint.Visible = true;
				tsMultiLangPrint.Visible = true;
				MultiLanguagefill();
				ReportsLoad();
			}
			else
			{
				labelmultilang.Visible = false;
				((Control)(object)CmbMultiLanguage).Visible = false;
				btnMultiLangPrint.Visible = false;
				tsMultiLangPrint.Visible = false;
			}
			DocumentNoFill();
			suppliercustomer();
			FillDifferenceAc();
			FillCommissionBrokeryAc();
			DeliveryTerm();
			CurrencyFill();
			CommissionTypeFill();
			multiCurrencyFeature();
			ConfigurationDefault();
			GridDtFill();
			PreviousCommBrokeryAndDiffAccounts();
			DocDate.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
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
			int CurrId = Conversion.ToInt(cmbcurrency.Value);
			decimal CurrRate = Conversion.ToDecimal((object)txtexchangerate.Text);
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int BaseCurrency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				cmbcurrency.Value = BaseCurrency;
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				decimal BaseCurrencyRate = Conversion.ToDecimal((object)BaseRatelist[0].ConfigKey.ToString());
				txtexchangerate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			List<ConfigrationsAllocation> FluctuationDifferenceAccountList = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FluctuationDifferenceAccount").ToList();
			if (FluctuationDifferenceAccountList.Count > 0)
			{
				FluctuationDifferenceAccountId = Conversion.ToInt((object)FluctuationDifferenceAccountList[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> FluctuationDifferenceAccountValueList = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FluctuationDifferenceAccountValue").ToList();
			if (FluctuationDifferenceAccountValueList.Count > 0)
			{
				FluctuationDifferenceAccountValue = Conversion.ToDecimal((object)FluctuationDifferenceAccountValueList[0].ConfigKey.ToString());
			}
			if (HasMultiCurrencyFeature)
			{
				if (CurrId > 0)
				{
					cmbcurrency.Value = CurrId;
				}
				if (CurrRate > 0m)
				{
					txtexchangerate.Text = CurrRate.ToString();
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void multiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbcurrency).Visible = true;
				txtexchangerate.Visible = true;
				txtfcyamount.Visible = true;
				label21.Visible = true;
				label22.Visible = true;
				label23.Visible = true;
				txtRemarks.Size = new Size(229, 79);
			}
			else
			{
				((Control)(object)cmbcurrency).Visible = false;
				txtexchangerate.Visible = false;
				txtfcyamount.Visible = false;
				label21.Visible = false;
				label22.Visible = false;
				label23.Visible = false;
				txtRemarks.Size = new Size(450, 79);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtexchangerate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateUpdateFcyInDetailGrid();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PreviousCommBrokeryAndDiffAccounts()
	{
		try
		{
			DataTable dtIds = CommTradeTransactionHeader.GetLastSavedCommBrokeryAndDiffAccounts(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtIds.Rows.Count > 0)
			{
				int CommId = Conversion.ToInt(dtIds.Rows[0]["CommissionAgentId"]);
				int BrokeryId = Conversion.ToInt(dtIds.Rows[0]["BrokerAgentId"]);
				int DiffId = Conversion.ToInt(dtIds.Rows[0]["DifferenceAccountId"]);
				if (CommId > 0)
				{
					cmbCommAgent.Value = CommId;
				}
				if (BrokeryId > 0)
				{
					cmbBrokeryAccount.Value = BrokeryId;
				}
				if (DiffId > 0)
				{
					cmbDifferencAccount.Value = DiffId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("MultiLangReports");
			if (files != null && files.Length != 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					tsMultiLangPrint.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void DocumentNoFill()
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
			int code = CommTradeTransactionHeader.GenerateCode(new CommTradeTransactionHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1704,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtbillno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule uom = new UOMSchedule();
			uom.ItemId = Conversion.ToInt((object)ID);
			uom.OrganizationId = UserAccount.OrganizationId;
			uom.CompanyId = UserAccount.CompanyId;
			dtUOM = UOMSchedule.SearchByObject(uom);
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
			int suppId = Conversion.ToInt(cmbCustomername.Value);
			int SubId = Conversion.ToInt(cmbsubbrokeryacc.Value);
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbCustomername, "Id", "CompanyName", "Customer Name", true);
				DDL.BindDDLNew(dtSupplier, cmbsubbrokeryacc, "Id", "CompanyName", "Broker Ac", true);
				((UltraGridBase)cmbCustomername).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbsubbrokeryacc).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			if (suppId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == suppId))
				{
					cmbCustomername.Value = suppId;
				}
				else
				{
					((Control)(object)cmbCustomername).Text = string.Empty;
				}
			}
			if (SubId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SubId))
				{
					cmbsubbrokeryacc.Value = SubId;
				}
				else
				{
					((Control)(object)cmbsubbrokeryacc).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Supplier Name Not Found");
		}
	}

	private void PaymentTerms()
	{
		try
		{
			int PayId = Conversion.ToInt(cmbpayterm.Value);
			dtTerm.Clear();
			dtTerm = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(5, 0);
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, cmbpayterm, "Id", "LookUpName", "Payment Term", true);
				((UltraGridBase)cmbpayterm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (PayId > 0)
			{
				if (dtTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == PayId))
				{
					cmbpayterm.Value = PayId;
				}
				else
				{
					((Control)(object)cmbpayterm).Text = string.Empty;
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
			int DeliveryId = Conversion.ToInt(cmbdeliveryterm.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DeliveryTerm", typeof(string));
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, cmbdeliveryterm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: true);
			((UltraGridBase)cmbdeliveryterm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (DeliveryId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DeliveryId))
				{
					cmbdeliveryterm.Value = DeliveryId;
				}
				else
				{
					((Control)(object)cmbdeliveryterm).Text = string.Empty;
				}
			}
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
			int OrderId = Conversion.ToInt(cmbcurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbcurrency, "Id", "CurrencyCode", "Currency", true);
				((UltraGridBase)cmbcurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbcurrency.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbcurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			int OrderId = Conversion.ToInt(cmbcommtype.Value);
			DataTable dt = new DataTable();
			DataTable dtlook = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(4, 0);
			if (dtlook.Rows.Count > 0)
			{
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CommissionType", typeof(string));
				dt.Clear();
				for (int i = 0; i < dtlook.Rows.Count; i++)
				{
					dt.Rows.Add(dtlook.Rows[i]["Id"], dtlook.Rows[i]["LookupName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: true);
					((UltraGridBase)cmbcommtype).Rows[2].Activate();
					((UltraGridBase)cmbcommtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				else
				{
					((UltraGridBase)cmbcommtype).DataSource = null;
					((Control)(object)cmbcommtype).Text = string.Empty;
				}
			}
			if (OrderId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbcommtype.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbcommtype).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCommissionBrokeryAc()
	{
		try
		{
			int commId = Conversion.ToInt(cmbCommAgent.Value);
			int BrokeryId = Conversion.ToInt(cmbBrokeryAccount.Value);
			dtAccounts = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10");
			if (dtAccounts.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAccounts, cmbCommAgent, "Id", "AccountTitle", "Commission Account", true);
				DDL.BindDDLNew(dtAccounts, cmbBrokeryAccount, "Id", "AccountTitle", "Brokery Account", true);
				((UltraGridBase)cmbCommAgent).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbBrokeryAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (commId > 0)
				{
					if (dtAccounts.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == commId))
					{
						cmbCommAgent.Value = commId;
					}
					else
					{
						((Control)(object)cmbCommAgent).Text = string.Empty;
					}
				}
				if (BrokeryId > 0)
				{
					if (dtAccounts.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BrokeryId))
					{
						cmbBrokeryAccount.Value = BrokeryId;
					}
					else
					{
						((Control)(object)cmbBrokeryAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbDifferencAccount).DataSource = null;
				((Control)(object)cmbBrokeryAccount).Text = string.Empty;
				((UltraGridBase)cmbCommAgent).DataSource = null;
				((Control)(object)cmbCommAgent).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDifferenceAc()
	{
		try
		{
			int OrderId = Conversion.ToInt(cmbDifferencAccount.Value);
			dtAccounts = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15");
			if (dtAccounts.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAccounts, cmbDifferencAccount, "Id", "AccountTitle", "AccountTitle", true);
				((UltraGridBase)cmbDifferencAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (OrderId > 0)
				{
					if (dtAccounts.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
					{
						cmbDifferencAccount.Value = OrderId;
					}
					else
					{
						((Control)(object)cmbDifferencAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbDifferencAccount).DataSource = null;
				((Control)(object)cmbDifferencAccount).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDtFill()
	{
		try
		{
			dtStockType.Clear();
			dtWare.Clear();
			dtStockType.Rows.Add(1, "Company");
			dtStockType.Rows.Add(2, "Other");
			dtWare = CommonServices.getActiveWareHouse();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailComboFill()
	{
		try
		{
			dtPackingType = InvPackingType.Getall();
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			dtitem = CommonServices.ItemGetForComboServiceBind();
			grdDetail.RootTable.Columns["CropYearId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["CropYearId"].HasValueList = true;
			grdDetail.RootTable.Columns["CropYearId"].LimitToList = true;
			grdDetail.RootTable.Columns["CropYearId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["CropYearId"].Caption = "Crop Year";
			grdDetail.RootTable.Columns["CropYearId"].ValueList.PopulateValueList((IEnumerable)dtCrop.DefaultView, "Id", "CropYear");
			grdDetail.RootTable.Columns["CropYearId"].Width = 250;
			grdDetail.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grdDetail.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grdDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
			grdDetail.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "PackTypeDesc");
			grdDetail.RootTable.Columns["PackingTypeId"].Width = 250;
			grdDetail.RootTable.Columns["SupplierAccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["SupplierAccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["SupplierAccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["SupplierAccountId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["SupplierAccountId"].Caption = "Supplier Account";
			grdDetail.RootTable.Columns["SupplierAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdDetail.RootTable.Columns["SupplierAccountId"].Width = 250;
			grdDetail.RootTable.Columns["CustomerAccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["CustomerAccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["CustomerAccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["CustomerAccountId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["CustomerAccountId"].Caption = "Customer Account";
			grdDetail.RootTable.Columns["CustomerAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdDetail.RootTable.Columns["CustomerAccountId"].Width = 250;
			grdDetail.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["ItemId"].HasValueList = true;
			grdDetail.RootTable.Columns["ItemId"].LimitToList = true;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdDetail.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
			grdDetail.RootTable.Columns["ItemId"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailsetting()
	{
		try
		{
			GridDetailItemsForRevenueGrid();
			GridDetailComboFill();
			SupplierCustomerBindForPT();
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["OrderNo"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdDetail.RootTable.Columns["LoadingDeliveryDetailId"].Visible = false;
			grdDetail.RootTable.Columns["UomId"].Visible = false;
			grdDetail.RootTable.Columns["CusFcyAmount"].Visible = false;
			grdDetail.RootTable.Columns["SupFcyAmount"].Visible = false;
			grdDetail.RootTable.Columns["ProductTypeId"].Visible = false;
			grdDetail.RootTable.Columns["BiltyFreight"].Visible = false;
			grdDetail.RootTable.Columns["LoadingDeliveryNo"].Caption = "LoadingNo";
			grdDetail.RootTable.Columns["CusGrossWeight"].Caption = "Gross Weight";
			grdDetail.RootTable.Columns["CusAdlsWeight"].Caption = "Adls Weight";
			grdDetail.RootTable.Columns["CusPMWeight"].Caption = "PM Weight";
			grdDetail.RootTable.Columns["CusNetWeight"].Caption = "Net Weight";
			grdDetail.RootTable.Columns["SupGrossWeight"].Caption = "Gross Weight";
			grdDetail.RootTable.Columns["SupAdlsWeight"].Caption = "Adls Weight";
			grdDetail.RootTable.Columns["SupPMWeight"].Caption = "PM Weight";
			grdDetail.RootTable.Columns["SupNetWeight"].Caption = "Net Weight";
			grdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CustomerAccountId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusGrossWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusAdlsWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusPMWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusNetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusFcyAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupFcyAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupGrossWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupAdlsWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupPMWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupNetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupplierAccountId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["UomId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["BiltyNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["BiltyDate"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["VehicleQty"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["VehicleGrossWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ProductTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["VehicleQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["VehicleQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["VehicleQty"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["VehicleQty"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["VehicleGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["VehicleGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["VehicleGrossWeight"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["VehicleGrossWeight"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["SupAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupAmount"].FormatString = "#,##0.######";
			grdDetail.RootTable.Columns["SupAmount"].TotalFormatString = "#,##0.######";
			grdDetail.RootTable.Columns["CusAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusAmount"].FormatString = "#,##0.######";
			grdDetail.RootTable.Columns["CusAmount"].TotalFormatString = "#,##0.######";
			grdDetail.RootTable.Columns["CusRate"].AggregateFunction = (AggregateFunction)3;
			grdDetail.RootTable.Columns["CusRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusRate"].FormatString = "#,##0.######";
			grdDetail.RootTable.Columns["CusRate"].TotalFormatString = "#,##0.######";
			grdDetail.RootTable.Columns["SupRate"].AggregateFunction = (AggregateFunction)3;
			grdDetail.RootTable.Columns["SupRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupRate"].FormatString = "#,##0.######";
			grdDetail.RootTable.Columns["SupRate"].TotalFormatString = "#,##0.######";
			grdDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdDetail.RootTable.Columns["SupFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupFcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdDetail.RootTable.Columns["SupFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,###.###";
			grdDetail.RootTable.Columns["SupPMWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupPMWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupPMWeight"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["SupPMWeight"].TotalFormatString = "#,###.###";
			grdDetail.RootTable.Columns["SupAdlsWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupAdlsWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupAdlsWeight"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["SupAdlsWeight"].TotalFormatString = "#,###.###";
			grdDetail.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupGrossWeight"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,###.###";
			grdDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,###.###";
			grdDetail.RootTable.Columns["CusPMWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusPMWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusPMWeight"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["CusPMWeight"].TotalFormatString = "#,###.###";
			grdDetail.RootTable.Columns["CusAdlsWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusAdlsWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusAdlsWeight"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["CusAdlsWeight"].TotalFormatString = "#,###.###";
			grdDetail.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusGrossWeight"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,###.###";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,###.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,###.###";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].Width = 30;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				bool ClearCustomerRevenueExpensegrd = true;
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow row in rows)
				{
					if (Conversion.ToInt(r.Cells["ProductTypeId"].Value) == 1 && row != r)
					{
						ClearCustomerRevenueExpensegrd = false;
						break;
					}
				}
				if (ClearCustomerRevenueExpensegrd)
				{
					dtCustomerRevenueExpense.Rows.Clear();
					grdCustomerRevenueExpense.ClearStructure();
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					CommTradeTransactionDetail vd = new CommTradeTransactionDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.CommTradeOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					vd.CommTradeOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
					vd.CommTradeLoadingDeliveryId = Conversion.ToInt(r.Cells["LoadingDeliveryId"].Value);
					vd.CommTradeLoadingDeliveryDetailId = Conversion.ToInt(r.Cells["LoadingDeliveryDetailId"].Value);
					vd.dDocDate = Conversion.ToDateTime((object)DocDate.Value);
					vd.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
					vd.CusDeliveryTerm = Conversion.ToString((object)((Control)(object)cmbdeliveryterm).Text);
					vd.CusDueDays = Conversion.ToInt((object)txtduedays.Text);
					vd.SupDueDays = Conversion.ToInt((object)txtduedays.Text);
					vd.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
					vd.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
					vd.BiltyNo = Conversion.ToString(r.Cells["BiltyNo"].Value);
					vd.BiltyDate = Conversion.ToDateTime(r.Cells["BiltyDate"].Value);
					vd.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					vd.VehicleQty = Conversion.ToDecimal(r.Cells["VehicleQty"].Value);
					vd.VehicleGrossWeight = Conversion.ToDecimal(r.Cells["VehicleGrossWeight"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.UomId = Conversion.ToInt(r.Cells["UomId"].Value);
					vd.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					vd.SupplierId = Conversion.ToInt(r.Cells["SupplierAccountId"].Value);
					vd.SupGrossWeight = Conversion.ToDecimal(r.Cells["SupGrossWeight"].Value);
					vd.SupPckMtrlWeight = Conversion.ToDecimal(r.Cells["SupPMWeight"].Value);
					vd.SupAdLsWt = Conversion.ToDecimal(r.Cells["SupAdLsWeight"].Value);
					vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
					vd.SupRate = Conversion.ToDecimal(r.Cells["SupRate"].Value);
					vd.SupAmount = Conversion.ToDecimal(r.Cells["SupAmount"].Value);
					vd.SupFcyAmount = Conversion.ToDecimal(r.Cells["SupFcyAmount"].Value);
					vd.CustomerId = Conversion.ToInt(cmbCustomername.Value);
					vd.CusGrossWeight = Conversion.ToDecimal(r.Cells["CusGrossWeight"].Value);
					vd.CusPckMtrlWeight = Conversion.ToDecimal(r.Cells["CusPMWeight"].Value);
					vd.CusAdLsWt = Conversion.ToDecimal(r.Cells["CusAdLsWeight"].Value);
					vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
					vd.CusRate = Conversion.ToDecimal(r.Cells["CusRate"].Value);
					vd.CusAmount = Conversion.ToDecimal(r.Cells["CusAmount"].Value);
					vd.CustFcyAmount = Conversion.ToDecimal(r.Cells["CusFcyAmount"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdDetail.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			GetIds();
			txtexchangerate_TextChanged(null, null);
			TotalCommissionAmount();
			CustomerBillAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CalcluationInGrid(e.Column.Key, grdDetail.CurrentRow);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalcluationInGrid(string ColumnName, GridEXRow item)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		grdDetail.UpdateData();
		item.BeginEdit();
		if (ColumnName == "CusRate" && item != null && (int)item.RowType == 0)
		{
			int CusRateUom = 40;
			string CusRate = item.Cells["CusRate"].Value.ToString();
			double CusAmount = Conversion.ToDouble((object)item.Cells["CusNetWeight"].Value.ToString()) / Conversion.ToDouble((object)CusRateUom) * Conversion.ToDouble((object)CusRate);
			item.Cells["CusAmount"].Value = Math.Round(CusAmount, 0, MidpointRounding.AwayFromZero);
			item.Cells["SupRate"].Value = CusRate;
			double SupAmount2 = Conversion.ToDouble((object)item.Cells["SupNetWeight"].Value.ToString()) / Conversion.ToDouble((object)CusRateUom) * Conversion.ToDouble((object)CusRate);
			item.Cells["SupAmount"].Value = Math.Round(SupAmount2, 0, MidpointRounding.AwayFromZero);
		}
		if (ColumnName == "SupRate" && item != null && (int)item.RowType == 0)
		{
			int SupRateUom = 40;
			string SupRate = item.Cells["SupRate"].Value.ToString();
			double SupAmount = Conversion.ToDouble((object)item.Cells["SupNetWeight"].Value.ToString()) / Conversion.ToDouble((object)SupRateUom) * Conversion.ToDouble((object)SupRate);
			item.Cells["SupAmount"].Value = Math.Round(SupAmount, 0, MidpointRounding.AwayFromZero);
		}
		item.EndEdit();
		grdDetail.UpdateData();
		txtexchangerate_TextChanged(null, null);
		TotalCommissionAmount();
		CustomerBillAmount();
		FillSummaryGrid();
	}

	private void GridPackingMaterialCombobind()
	{
		try
		{
			dtAccounts = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15");
			dtPackingType = InvPackingType.Getall();
			dtitem = CommonServices.GetItemByItemTypeId("14");
			GridDtFill();
			if (dtPackingType.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "PackTypeDesc");
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].Width = 250;
			}
			if (dtitem.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["ItemId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["ItemId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
				grdPackingMaterial.RootTable.Columns["ItemId"].Width = 250;
			}
			if (dtAccounts.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].Width = 250;
			}
			if (dtStockType.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["StockTypeId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["StockTypeId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["StockTypeId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["StockTypeId"].ValueList.PopulateValueList((IEnumerable)dtStockType.DefaultView, "Id", "StockType");
				grdPackingMaterial.RootTable.Columns["StockTypeId"].Width = 250;
			}
			if (dtWare.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["WareHouseId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["WareHouseId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dtWare.DefaultView, "Id", "WareHouseName");
				grdPackingMaterial.RootTable.Columns["WareHouseId"].Width = 250;
			}
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
			grdPackingMaterial.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].Caption = "CreditAccount";
			grdPackingMaterial.RootTable.Columns["StockTypeId"].Caption = "Stock Type";
			grdPackingMaterial.RootTable.Columns["StockTypeId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["WareHouseId"].Caption = "WareHouse";
			grdPackingMaterial.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpackingsetting()
	{
		try
		{
			GridPackingMaterialCombobind();
			grdPackingMaterial.RootTable.Columns["Id"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["LoadingDeliveryId"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["LoadingDeliveryPmOtherId"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["StockTypeId"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterial.RootTable.Columns["ItemSize"].Visible = false;
			grdPackingMaterial.RootTable.Columns["Weight"].Visible = false;
			grdPackingMaterial.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["LoadingDeliveryPmOtherId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Amount"].FormatString = "#,##0.######";
			grdPackingMaterial.RootTable.Columns["Amount"].TotalFormatString = "#,##0.######";
			grdPackingMaterial.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Qty"].FormatString = "#,##0.######";
			grdPackingMaterial.RootTable.Columns["Qty"].TotalFormatString = "#,##0.######";
			grdPackingMaterial.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Weight"].FormatString = "#,##0.######";
			grdPackingMaterial.RootTable.Columns["Weight"].TotalFormatString = "#,##0.######";
			grdPackingMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdPackingMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Rate"].FormatString = "#,##0.######";
			grdPackingMaterial.RootTable.Columns["Rate"].TotalFormatString = "#,##0.######";
			CommonServices.GridAutoAdjustmentNew(grdPackingMaterial);
			grdPackingMaterial.RootTable.Columns.Add("Delete");
			grdPackingMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Delete"].Width = 30;
			grdPackingMaterial.RootTable.Columns["Delete"].Position = 0;
			grdPackingMaterial.RootTable.Columns.Add("Add");
			grdPackingMaterial.RootTable.Columns["Add"].Key = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].Caption = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonText = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Add"].Width = 40;
			grdPackingMaterial.RootTable.Columns["Add"].Position = 1;
			grdPackingMaterial.FrozenColumns = 2;
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowPackingmaterial()
	{
		try
		{
			dtPackingMaterial.Rows.Add(0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			grdPackingMaterial.DataSource = dtPackingMaterial;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_EditingCell(object sender, EditingCellEventArgs e)
	{
		try
		{
			GridEXRow r = grdPackingMaterial.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Amount")
			{
				PmCurrRowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
				PmCurrRowAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_EditModeChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPackingMaterial.CurrentRow;
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

	private void grdPackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPackingMaterial.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["StockTypeId"].Value) == 1 && e.Column.Key == "Qty")
			{
				r.Cells["Qty"].Value = PmCurrRowQty;
				MessageBox.Show("you can not Change Qty When StockType is company");
				return;
			}
			if (Conversion.ToInt(r.Cells["StockTypeId"].Value) == 1 && e.Column.Key == "Amount")
			{
				r.Cells["Amount"].Value = PmCurrRowAmount;
				MessageBox.Show("you can not Change Amount Directly When StockType is company,You Can Change Rate Instead");
				return;
			}
			grdPackingMaterial.UpdateData();
			r.BeginEdit();
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				decimal Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
				decimal num = Conversion.ToDecimal((object)r.Cells["Qty"].Value.ToString());
				PmCurrRowQty = Conversion.ToDecimal((object)r.Cells["Qty"].Value.ToString());
				decimal Amount = Conversion.ToDecimal((object)num) * Conversion.ToDecimal((object)Rate);
				r.Cells["Amount"].Value = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
				PmCurrRowAmount = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "Amount")
			{
				r.Cells["Rate"].Value = 0;
				r.Cells["Qty"].Value = 0;
				PmCurrRowQty = default(decimal);
			}
			r.EndEdit();
			grdPackingMaterial.UpdateData();
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(item.Cells["StockTypeId"].Value) == 1)
				{
					MessageBox.Show("you can not delete row With StockType company");
					return;
				}
				item.Delete();
				if (dtPackingMaterial.Rows.Count == 0)
				{
					AddRowPackingmaterial();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowPackingmaterial();
			}
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSupplierForRevenueGrid()
	{
		try
		{
			dtReveneuExpenseAccount = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(6, 0);
			grdrevenue.RootTable.Columns["OtherRevenueId"].ColumnType = (ColumnType)0;
			grdrevenue.RootTable.Columns["OtherRevenueId"].HasValueList = true;
			grdrevenue.RootTable.Columns["OtherRevenueId"].LimitToList = true;
			grdrevenue.RootTable.Columns["OtherRevenueId"].EditType = (EditType)4;
			grdrevenue.RootTable.Columns["OtherRevenueId"].Caption = "Other Revenue";
			grdrevenue.RootTable.Columns["OtherRevenueId"].ValueList.PopulateValueList((IEnumerable)dtReveneuExpenseAccount.DefaultView, "Id", "LookUpName");
			grdrevenue.RootTable.Columns["SupplierAccountId"].ColumnType = (ColumnType)0;
			grdrevenue.RootTable.Columns["SupplierAccountId"].HasValueList = true;
			grdrevenue.RootTable.Columns["SupplierAccountId"].LimitToList = true;
			grdrevenue.RootTable.Columns["SupplierAccountId"].EditType = (EditType)4;
			grdrevenue.RootTable.Columns["SupplierAccountId"].Caption = "Supplier Name";
			grdrevenue.RootTable.Columns["SupplierAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddExpenseAccountRowsOtherRevenue()
	{
		try
		{
			if (grdrevenue.RowCount > 0)
			{
				foreach (DataRow r in dtReveneuExpenseAccount.Rows)
				{
					if (!dtOtherRevenue.AsEnumerable().Any((DataRow row) => row.Field<int>("OtherRevenueId") == Conversion.ToInt(r["Id"])))
					{
						dtOtherRevenue.Rows.Add(0, 0, 0, r["Id"], 0, 0, 0, 0, 0);
					}
				}
				grdrevenue.DataSource = dtOtherRevenue;
				return;
			}
			dtOtherRevenue.Clear();
			foreach (DataRow r2 in dtReveneuExpenseAccount.Rows)
			{
				dtOtherRevenue.Rows.Add(0, 0, 0, r2["Id"], 0, 0, 0, 0, 0);
			}
			grdrevenue.DataSource = dtOtherRevenue;
			grdrevenue.RetrieveStructure();
			grdrevenuesetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowOtherRevenue()
	{
		try
		{
			dtOtherRevenue.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0);
			grdrevenue.DataSource = dtOtherRevenue;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdrevenue_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdrevenue.CurrentRow;
				dtOtherRevenue.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtOtherRevenue.Rows.Count == 0)
				{
					AddRowOtherRevenue();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowOtherRevenue();
			}
			FillSummaryGrid();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdrevenuesetting()
	{
		try
		{
			GridDetailSupplierForRevenueGrid();
			grdrevenue.RootTable.Columns["Id"].EditType = (EditType)0;
			grdrevenue.RootTable.Columns["LoadingDeliveryId"].EditType = (EditType)0;
			grdrevenue.RootTable.Columns["LoadingDeliveryRevExpId"].EditType = (EditType)0;
			grdrevenue.RootTable.Columns["Id"].Visible = false;
			grdrevenue.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdrevenue.RootTable.Columns["LoadingDeliveryRevExpId"].Visible = false;
			grdrevenue.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdrevenue.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdrevenue.RootTable.Columns["AmountCr"].FormatString = "#,##0.######";
			grdrevenue.RootTable.Columns["AmountCr"].TotalFormatString = "#,##0.######";
			grdrevenue.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdrevenue.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdrevenue.RootTable.Columns["Qty"].FormatString = "#,##0.######";
			grdrevenue.RootTable.Columns["Qty"].TotalFormatString = "#,##0.######";
			grdrevenue.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdrevenue.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdrevenue.RootTable.Columns["AmountDr"].FormatString = "#,##0.######";
			grdrevenue.RootTable.Columns["AmountDr"].TotalFormatString = "#,##0.######";
			grdrevenue.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdrevenue.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdrevenue.RootTable.Columns["Rate"].FormatString = "#,##0.######";
			grdrevenue.RootTable.Columns["Rate"].TotalFormatString = "#,##0.######";
			CommonServices.GridAutoAdjustmentNew(grdrevenue);
			grdrevenue.RootTable.Columns.Add("Delete");
			grdrevenue.RootTable.Columns["Delete"].Key = "Delete";
			grdrevenue.RootTable.Columns["Delete"].ButtonText = "X";
			grdrevenue.RootTable.Columns["Delete"].Caption = "X";
			grdrevenue.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdrevenue.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdrevenue.RootTable.Columns["Delete"].Width = 40;
			grdrevenue.RootTable.Columns["Delete"].Position = 0;
			grdrevenue.RootTable.Columns.Add("Add");
			grdrevenue.RootTable.Columns["Add"].Key = "Add";
			grdrevenue.RootTable.Columns["Add"].ButtonText = "Add";
			grdrevenue.RootTable.Columns["Add"].Caption = "Add";
			grdrevenue.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdrevenue.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdrevenue.RootTable.Columns["Add"].Width = 40;
			grdrevenue.RootTable.Columns["Add"].Position = 1;
			grdrevenue.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdrevenue_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdrevenue.UpdateData();
			GridEXRow item = grdrevenue.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
				decimal Amount = Conversion.ToDecimal((object)Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString())) * Conversion.ToDecimal((object)Rate);
				item.Cells["AmountCr"].Value = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
				item.Cells["AmountDr"].Value = 0;
			}
			if (e.Column.Key == "AmountCr" && item != null && (int)item.RowType == 0)
			{
				item.Cells["AmountCr"].Value = Math.Round(Conversion.ToDouble(item.Cells["AmountCr"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["AmountDr"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			if (e.Column.Key == "AmountDr" && item != null && (int)item.RowType == 0)
			{
				item.Cells["AmountDr"].Value = Math.Round(Conversion.ToDouble(item.Cells["AmountDr"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["AmountCr"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			_ = e.Column.Key == "AccountId";
			grdrevenue.UpdateData();
			CustomerBillAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RevenueCalcluationInGrid(string ColumnName, GridEXRow item)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdrevenue.UpdateData();
			item.BeginEdit();
			if ((ColumnName == "Qty" || ColumnName == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
				decimal Amount = Conversion.ToDecimal((object)Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString())) * Conversion.ToDecimal((object)Rate);
				item.Cells["AmountCr"].Value = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
				item.Cells["AmountDr"].Value = 0;
			}
			if (ColumnName == "AmountCr" && item != null && (int)item.RowType == 0)
			{
				item.Cells["AmountCr"].Value = Math.Round(Conversion.ToDouble(item.Cells["AmountCr"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["AmountDr"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			if (ColumnName == "AmountDr" && item != null && (int)item.RowType == 0)
			{
				item.Cells["AmountDr"].Value = Math.Round(Conversion.ToDouble(item.Cells["AmountDr"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["AmountCr"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			_ = ColumnName == "AccountId";
			item.EndEdit();
			grdrevenue.UpdateData();
			CustomerBillAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboForgrdVehicleDetail()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("5,6,7,8,3,11,10");
			grdVehicleDetail.RootTable.Columns["CreditAccountId"].ColumnType = (ColumnType)0;
			grdVehicleDetail.RootTable.Columns["CreditAccountId"].HasValueList = true;
			grdVehicleDetail.RootTable.Columns["CreditAccountId"].LimitToList = true;
			grdVehicleDetail.RootTable.Columns["CreditAccountId"].EditType = (EditType)4;
			grdVehicleDetail.RootTable.Columns["CreditAccountId"].Caption = "Credit Account";
			grdVehicleDetail.RootTable.Columns["CreditAccountId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "AccountTitle");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdVehicleDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdVehicleDetailsetting()
	{
		try
		{
			ComboForgrdVehicleDetail();
			grdVehicleDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdVehicleDetail.RootTable.Columns["LoadingDeliveryId"].EditType = (EditType)0;
			grdVehicleDetail.RootTable.Columns["CustomerId"].EditType = (EditType)0;
			grdVehicleDetail.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			grdVehicleDetail.RootTable.Columns["BiltyNo"].EditType = (EditType)0;
			grdVehicleDetail.RootTable.Columns["VehicleQty"].EditType = (EditType)0;
			grdVehicleDetail.RootTable.Columns["BiltyFreight"].EditType = (EditType)0;
			grdVehicleDetail.RootTable.Columns["NetPaid"].EditType = (EditType)0;
			grdVehicleDetail.RootTable.Columns["Id"].Visible = false;
			grdVehicleDetail.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdVehicleDetail.RootTable.Columns["CustomerId"].Visible = false;
			grdVehicleDetail.RootTable.Columns["VehicleQty"].AggregateFunction = (AggregateFunction)2;
			grdVehicleDetail.RootTable.Columns["VehicleQty"].TextAlignment = (TextAlignment)3;
			grdVehicleDetail.RootTable.Columns["VehicleQty"].FormatString = "#,##0.######";
			grdVehicleDetail.RootTable.Columns["VehicleQty"].TotalFormatString = "#,##0.######";
			grdVehicleDetail.RootTable.Columns["BiltyFreight"].AggregateFunction = (AggregateFunction)2;
			grdVehicleDetail.RootTable.Columns["BiltyFreight"].TextAlignment = (TextAlignment)3;
			grdVehicleDetail.RootTable.Columns["BiltyFreight"].FormatString = "#,##0.######";
			grdVehicleDetail.RootTable.Columns["BiltyFreight"].TotalFormatString = "#,##0.######";
			grdVehicleDetail.RootTable.Columns["NetPaid"].AggregateFunction = (AggregateFunction)2;
			grdVehicleDetail.RootTable.Columns["NetPaid"].TextAlignment = (TextAlignment)3;
			grdVehicleDetail.RootTable.Columns["NetPaid"].FormatString = "#,##0.######";
			grdVehicleDetail.RootTable.Columns["NetPaid"].TotalFormatString = "#,##0.######";
			CommonServices.GridAutoAdjustmentNew(grdVehicleDetail);
			grdVehicleDetail.RootTable.Columns.Add("Select");
			grdVehicleDetail.RootTable.Columns["Select"].ActAsSelector = true;
			grdVehicleDetail.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdVehicleDetail.RootTable.Columns["Select"].Width = 30;
			grdVehicleDetail.RootTable.Columns["Select"].Position = 0;
			grdVehicleDetail.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdVehicleDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdVehicleDetail.UpdateData();
			_ = grdVehicleDetail.CurrentRow;
			grdVehicleDetail.UpdateData();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddExpenseAccountRowsforgrdCustomerRevenueExpense()
	{
		try
		{
			GridDetailSupplierForgrdCustomerRevenueExpense();
			if (grdCustomerRevenueExpense.RowCount > 0)
			{
				foreach (DataRow r in dtCustomerRevExpAccounts.Rows)
				{
					if (!dtCustomerRevenueExpense.AsEnumerable().Any((DataRow row) => row.Field<int>("OtherRevenueId") == Conversion.ToInt(r["Id"])))
					{
						dtCustomerRevenueExpense.Rows.Add(0, 0, 0, r["Id"], 0, 0, 0, 0, 0);
					}
				}
				grdCustomerRevenueExpense.DataSource = dtCustomerRevenueExpense;
				return;
			}
			dtCustomerRevenueExpense.Clear();
			foreach (DataRow r2 in dtCustomerRevExpAccounts.Rows)
			{
				dtCustomerRevenueExpense.Rows.Add(0, 0, 0, r2["Id"], 0, 0, 0, 0, 0);
			}
			grdCustomerRevenueExpense.DataSource = dtCustomerRevenueExpense;
			grdCustomerRevenueExpense.RetrieveStructure();
			grdCustomerRevenueExpensesetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailItemsForRevenueGrid()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			dtitemOfDetail.Clear();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (!dtitemOfDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == Conversion.ToInt(item.Cells["ItemId"].Value)))
				{
					dtitemOfDetail.Rows.Add(Conversion.ToInt(item.Cells["ItemId"].Value), Conversion.ToString((object)item.Cells["ItemId"].Text), Conversion.ToString((object)item.Cells["ItemCode"].Text));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSupplierForgrdCustomerRevenueExpense()
	{
		try
		{
			if (dtitemOfDetail.Rows.Count > 0)
			{
				foreach (DataRow row in dtitemOfDetail.Rows)
				{
					int ItemId = Conversion.ToInt(row["ItemId"]);
					int CusId = Conversion.ToInt(cmbCustomername.Value);
					int OrgId = Conversion.ToInt((object)UserAccount.OrganizationId);
					int ComId = Conversion.ToInt((object)UserAccount.CompanyId);
					if (ItemId > 0 && CusId > 0)
					{
						dtCustomerRevExpAccounts = LookUps.RevExpAcAllocatedToProductAndDiscTypeForComboService(OrgId, ComId, ItemId, CusId);
						if (dtCustomerRevExpAccounts.Rows.Count > 0)
						{
							break;
						}
					}
				}
			}
			grdCustomerRevenueExpense.RootTable.Columns["SupplierAccountId"].ColumnType = (ColumnType)0;
			grdCustomerRevenueExpense.RootTable.Columns["SupplierAccountId"].HasValueList = true;
			grdCustomerRevenueExpense.RootTable.Columns["SupplierAccountId"].LimitToList = true;
			grdCustomerRevenueExpense.RootTable.Columns["SupplierAccountId"].EditType = (EditType)4;
			grdCustomerRevenueExpense.RootTable.Columns["SupplierAccountId"].Caption = "Supplier Name";
			grdCustomerRevenueExpense.RootTable.Columns["SupplierAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdCustomerRevenueExpense.RootTable.Columns["OtherRevenueId"].ColumnType = (ColumnType)0;
			grdCustomerRevenueExpense.RootTable.Columns["OtherRevenueId"].HasValueList = true;
			grdCustomerRevenueExpense.RootTable.Columns["OtherRevenueId"].LimitToList = true;
			grdCustomerRevenueExpense.RootTable.Columns["OtherRevenueId"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["OtherRevenueId"].Caption = "Other Revenue";
			grdCustomerRevenueExpense.RootTable.Columns["OtherRevenueId"].ValueList.PopulateValueList((IEnumerable)dtCustomerRevExpAccounts.DefaultView, "Id", "ReveneuExpenseAccount");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowgrdCustomerRevenueExpense()
	{
		try
		{
			dtCustomerRevenueExpense.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0);
			grdCustomerRevenueExpense.DataSource = dtCustomerRevenueExpense;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerRevenueExpense_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdCustomerRevenueExpensesetting()
	{
		try
		{
			GridDetailSupplierForgrdCustomerRevenueExpense();
			grdCustomerRevenueExpense.RootTable.Columns["Id"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["LoadingDeliveryId"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["LoadingDeliveryRevExpId"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["OtherRevenueId"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["Qty"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["AmountDr"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["AmountCr"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["SupplierAccountId"].EditType = (EditType)0;
			grdCustomerRevenueExpense.RootTable.Columns["Id"].Visible = false;
			grdCustomerRevenueExpense.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdCustomerRevenueExpense.RootTable.Columns["LoadingDeliveryRevExpId"].Visible = false;
			grdCustomerRevenueExpense.RootTable.Columns["Qty"].Visible = false;
			grdCustomerRevenueExpense.RootTable.Columns["Rate"].Visible = false;
			grdCustomerRevenueExpense.RootTable.Columns["AmountDr"].Visible = false;
			grdCustomerRevenueExpense.RootTable.Columns["AmountCr"].Visible = false;
			grdCustomerRevenueExpense.RootTable.Columns["SupplierAccountId"].Visible = false;
			grdCustomerRevenueExpense.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdCustomerRevenueExpense.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdCustomerRevenueExpense.RootTable.Columns["AmountCr"].FormatString = "#,##0.######";
			grdCustomerRevenueExpense.RootTable.Columns["AmountCr"].TotalFormatString = "#,##0.######";
			grdCustomerRevenueExpense.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdCustomerRevenueExpense.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdCustomerRevenueExpense.RootTable.Columns["Qty"].FormatString = "#,##0.######";
			grdCustomerRevenueExpense.RootTable.Columns["Qty"].TotalFormatString = "#,##0.######";
			grdCustomerRevenueExpense.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdCustomerRevenueExpense.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdCustomerRevenueExpense.RootTable.Columns["AmountDr"].FormatString = "#,##0.######";
			grdCustomerRevenueExpense.RootTable.Columns["AmountDr"].TotalFormatString = "#,##0.######";
			grdCustomerRevenueExpense.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdCustomerRevenueExpense.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdCustomerRevenueExpense.RootTable.Columns["Rate"].FormatString = "#,##0.######";
			grdCustomerRevenueExpense.RootTable.Columns["Rate"].TotalFormatString = "#,##0.######";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerRevenueExpense_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdCustomerRevenueExpense.UpdateData();
			GridEXRow item = grdCustomerRevenueExpense.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
				decimal Amount = Conversion.ToDecimal((object)Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString())) * Conversion.ToDecimal((object)Rate);
				item.Cells["AmountCr"].Value = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
				item.Cells["AmountDr"].Value = 0;
			}
			if (e.Column.Key == "AmountCr" && item != null && (int)item.RowType == 0)
			{
				item.Cells["AmountCr"].Value = Math.Round(Conversion.ToDouble(item.Cells["AmountCr"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["AmountDr"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			if (e.Column.Key == "AmountDr" && item != null && (int)item.RowType == 0)
			{
				item.Cells["AmountDr"].Value = Math.Round(Conversion.ToDouble(item.Cells["AmountDr"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["AmountCr"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			_ = e.Column.Key == "AccountId";
			grdCustomerRevenueExpense.UpdateData();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillSummaryGrid()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			GridEXRow PM = grdPackingMaterial.CurrentRow;
			GridEXRow RS = grdrevenue.CurrentRow;
			dtsummaryBySupplier.Clear();
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			string supplierIds = "";
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow row in rows)
			{
				supplierIds = supplierIds + "," + Conversion.ToString(row.Cells["SupplierAccountId"].Value);
			}
			if (string.IsNullOrEmpty(supplierIds))
			{
				return;
			}
			foreach (int Id in from id in supplierIds.Split(',').Distinct()
				select Conversion.ToInt((object)id))
			{
				if (Id == 0)
				{
					continue;
				}
				decimal ItemAmount = default(decimal);
				decimal ItemFcyAmount = default(decimal);
				decimal PackingAmount = default(decimal);
				decimal DrExpenses = default(decimal);
				decimal CrExpenses = default(decimal);
				rows = grdDetail.GetRows();
				foreach (GridEXRow rdetail in rows)
				{
					if (Conversion.ToInt(rdetail.Cells["SupplierAccountId"].Value) == Id)
					{
						ItemAmount += Conversion.ToDecimal(rdetail.Cells["SupAmount"].Value);
						ItemFcyAmount += Conversion.ToDecimal(rdetail.Cells["SupFcyAmount"].Value);
					}
				}
				if (PM != null && (int)PM.RowType == 0 && PM.Cells.Count > 0)
				{
					PackingAmount = default(decimal);
					rows = grdPackingMaterial.GetRows();
					foreach (GridEXRow rPM in rows)
					{
						if (SupplierCustomer.GetSupplierCustomerIdbyGlAccountId(Conversion.ToInt(rPM.Cells["CreditAccountId"].Value)) == Id)
						{
							PackingAmount += Conversion.ToDecimal(rPM.Cells["Amount"].Value);
						}
					}
				}
				if (RS != null && (int)RS.RowType == 0 && RS.Cells.Count > 0)
				{
					DrExpenses = default(decimal);
					CrExpenses = default(decimal);
					rows = grdrevenue.GetRows();
					foreach (GridEXRow rRS in rows)
					{
						if (Conversion.ToInt(rRS.Cells["SupplierAccountId"].Value) == Id)
						{
							DrExpenses += Conversion.ToDecimal(rRS.Cells["AmountDr"].Value);
							CrExpenses += Conversion.ToDecimal(rRS.Cells["AmountCr"].Value);
						}
					}
				}
				dtsummaryBySupplier.Rows.Add(Id, ItemAmount, PackingAmount, DrExpenses + CrExpenses, ItemAmount + PackingAmount + (DrExpenses + CrExpenses));
			}
			if (dtsummaryBySupplier.Rows.Count > 0)
			{
				grdsummary.DataSource = dtsummaryBySupplier;
				grdsummary.RetrieveStructure();
				grdsummarysetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsummarysetting()
	{
		try
		{
			grdsummary.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
			grdsummary.RootTable.Columns["SupplierId"].HasValueList = true;
			grdsummary.RootTable.Columns["SupplierId"].LimitToList = true;
			grdsummary.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdsummary.RootTable.Columns["SupplierId"].Caption = "Supplier Name";
			grdsummary.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdsummary.RootTable.Columns["SupplierId"].Width = 250;
			grdsummary.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["ItemAmount"].FormatString = "#,##0.######";
			grdsummary.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.######";
			grdsummary.RootTable.Columns["Expenses"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["Expenses"].FormatString = "#,##0.######";
			grdsummary.RootTable.Columns["Expenses"].TotalFormatString = "#,##0.######";
			grdsummary.RootTable.Columns["Expenses"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["PackingAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["PackingAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["PackingAmount"].FormatString = "#,##0.######";
			grdsummary.RootTable.Columns["PackingAmount"].TotalFormatString = "#,##0.######";
			grdsummary.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["BillAmount"].FormatString = "#,##0.######";
			grdsummary.RootTable.Columns["BillAmount"].TotalFormatString = "#,##0.######";
			CommonServices.GridAutoAdjustmentNew(grdsummary);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomerBindForPT()
	{
		try
		{
			dtCustomerBindForPT.Clear();
			dtSupplierBindForPT.Clear();
			if (grdDetail.RowCount > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow row in rows)
				{
					int SupplierId = Conversion.ToInt(row.Cells["SupplierAccountId"].Value);
					string SupplierName = Conversion.ToString((object)row.Cells["SupplierAccountId"].Text);
					if (!dtSupplierBindForPT.AsEnumerable().Any((DataRow rr) => rr.Field<int>("SupplierId") == SupplierId))
					{
						dtSupplierBindForPT.Rows.Add(SupplierId, SupplierName);
					}
				}
			}
			if (grdPaymentTermSupplier.RowCount > 0)
			{
				if (dtSupplierBindForPT.Rows.Count > 0)
				{
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].HasValueList = true;
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].LimitToList = true;
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtSupplierBindForPT.DefaultView, "SupplierId", "SupplierName");
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].Width = 120;
				}
				if (dtTerm.Rows.Count > 0)
				{
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].ColumnType = (ColumnType)0;
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].HasValueList = true;
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].LimitToList = true;
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].ValueList.PopulateValueList((IEnumerable)dtTerm.DefaultView, "Id", "LookUpName");
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].Width = 120;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPaymentTermCustomerGrid()
	{
	}

	private void grdSettingsPaymentTermCustomer()
	{
	}

	private void grdPaymentTermCustomer_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdPaymentTermCustomer_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
	}

	private void grdPaymentTermCustomer_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdPaymentTermCustomer_KeyDown(object sender, KeyEventArgs e)
	{
	}

	private void AddRowInPaymentTermSupplierGrid()
	{
		try
		{
			dtSupplierPTDetail.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, DateTime.Now, "");
			grdPaymentTermSupplier.DataSource = dtSupplierPTDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentTermSupplier()
	{
		try
		{
			SupplierCustomerBindForPT();
			grdPaymentTermSupplier.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdPaymentTermSupplier.RootTable.Columns["SupplierId"].Caption = "Supplier Name";
			grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].EditType = (EditType)4;
			grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].Caption = "Payment Term";
			grdPaymentTermSupplier.RootTable.Columns["Id"].Visible = false;
			grdPaymentTermSupplier.RootTable.Columns["LoadingDeliveryPaymentDetailId"].Visible = false;
			grdPaymentTermSupplier.RootTable.Columns["CommTradeLoadingDeliveryId"].Visible = false;
			grdPaymentTermSupplier.RootTable.Columns["Id"].EditType = (EditType)0;
			grdPaymentTermSupplier.RootTable.Columns["LoadingDeliveryPaymentDetailId"].EditType = (EditType)0;
			grdPaymentTermSupplier.RootTable.Columns["CommTradeLoadingDeliveryId"].EditType = (EditType)0;
			grdPaymentTermSupplier.RootTable.Columns["DueDate"].EditType = (EditType)2;
			grdPaymentTermSupplier.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yyyy";
			grdPaymentTermSupplier.RootTable.Columns["%ofTotal"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTermSupplier.RootTable.Columns["%ofTotal"].TextAlignment = (TextAlignment)3;
			grdPaymentTermSupplier.RootTable.Columns["%ofTotal"].FormatString = "#,##0.####";
			grdPaymentTermSupplier.RootTable.Columns["%ofTotal"].TotalFormatString = "#,##0.####";
			grdPaymentTermSupplier.RootTable.Columns["DueDays"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTermSupplier.RootTable.Columns["DueDays"].TextAlignment = (TextAlignment)3;
			grdPaymentTermSupplier.RootTable.Columns["DueDays"].FormatString = "#,##0.###";
			grdPaymentTermSupplier.RootTable.Columns["DueDays"].TotalFormatString = "#,##0.###";
			grdPaymentTermSupplier.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTermSupplier.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPaymentTermSupplier.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdPaymentTermSupplier.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdPaymentTermSupplier);
			grdPaymentTermSupplier.RootTable.Columns.Add("Delete");
			grdPaymentTermSupplier.RootTable.Columns["Delete"].Key = "Delete";
			grdPaymentTermSupplier.RootTable.Columns["Delete"].Caption = "X";
			grdPaymentTermSupplier.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTermSupplier.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTermSupplier.RootTable.Columns["Delete"].ButtonText = "X";
			grdPaymentTermSupplier.RootTable.Columns["Delete"].Width = 20;
			grdPaymentTermSupplier.RootTable.Columns.Add("Add");
			grdPaymentTermSupplier.RootTable.Columns["Add"].Key = "Add";
			grdPaymentTermSupplier.RootTable.Columns["Add"].Caption = "+";
			grdPaymentTermSupplier.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTermSupplier.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTermSupplier.RootTable.Columns["Add"].ButtonText = "+";
			grdPaymentTermSupplier.RootTable.Columns["Add"].Width = 20;
			grdPaymentTermSupplier.RootTable.Columns["Add"].Position = 0;
			grdPaymentTermSupplier.RootTable.Columns["Delete"].Position = 1;
			grdPaymentTermSupplier.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTermSupplier_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTermSupplier.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int SupplierId = Conversion.ToInt(item.Cells["SupplierId"].Value);
				if (SupplierId <= 0)
				{
					if (Conversion.ToDecimal(item.Cells["Amount"].Value) == 0m)
					{
						item.Cells["%ofTotal"].Value = 0;
					}
					if (Conversion.ToDecimal(item.Cells["%ofTotal"].Value) == 0m)
					{
						item.Cells["Amount"].Value = 0;
					}
					MessageBox.Show("Supplier Should be Selected...", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (e.Column.Key == "%ofTotal" && grdDetail.GetRows().Count() > 0)
				{
					double num = Conversion.ToDouble((object)(from row in dtsummaryBySupplier.AsEnumerable()
						where row.Field<int>("SupplierId") == SupplierId
						select row).Sum((DataRow row) => row.Field<decimal>("BillAmount")));
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
					if (grdDetail.GetRows().Count() <= 0)
					{
						throw new Exception("No Detail Record Found");
					}
					double billAmount = Conversion.ToDouble((object)(from row in dtsummaryBySupplier.AsEnumerable()
						where row.Field<int>("SupplierId") == SupplierId
						select row).Sum((DataRow row) => row.Field<decimal>("BillAmount")));
					double amount = Conversion.ToDouble(item.Cells["Amount"].Value);
					if (billAmount < amount)
					{
						MessageBox.Show("Amount Cant be Greater than Order Amount", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["Amount"].Value = billAmount;
						amount = billAmount;
					}
					double prcnt = amount * 100.0 / billAmount;
					item.Cells["%ofTotal"].Value = Math.Round(prcnt, 8);
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
			}
			grdPaymentTermSupplier.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTermSupplier_UpdatingCell(object sender, UpdatingCellEventArgs e)
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

	private void grdPaymentTermSupplier_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdPaymentTermSupplier.CurrentRow;
				dtSupplierPTDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtSupplierPTDetail.Rows.Count == 0)
				{
					AddRowInPaymentTermSupplierGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPaymentTermSupplierGrid();
			}
			grdPaymentTermSupplier.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTermSupplier_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTermSupplier.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentTermSupplier.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPaymentTermSupplier.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtSupplierPTDetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtSupplierPTDetail.Rows.Count == 0)
					{
						AddRowInPaymentTermSupplierGrid();
					}
				}
				if (grdPaymentTermSupplier.CurrentColumn.Key == "Add")
				{
					AddRowInPaymentTermSupplierGrid();
				}
				grdPaymentTermSupplier.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtSupplierPTDetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtSupplierPTDetail.Rows.Count == 0)
					{
						AddRowInPaymentTermSupplierGrid();
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPaymentTermSupplierGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierPTCalcluationInGrid(string ColumnName, GridEXRow item)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPaymentTermSupplier.UpdateData();
			if (item != null && (int)item.RowType == 0)
			{
				item.BeginEdit();
				int SupplierId = Conversion.ToInt(item.Cells["SupplierId"].Value);
				if (SupplierId <= 0)
				{
					if (Conversion.ToDecimal(item.Cells["Amount"].Value) == 0m)
					{
						item.Cells["%ofTotal"].Value = 0;
					}
					if (Conversion.ToDecimal(item.Cells["%ofTotal"].Value) == 0m)
					{
						item.Cells["Amount"].Value = 0;
					}
					MessageBox.Show("Supplier Should be Selected...", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (ColumnName.Contains("%ofTotal") && grdDetail.GetRows().Count() > 0)
				{
					double num = Conversion.ToDouble((object)(from row in dtsummaryBySupplier.AsEnumerable()
						where row.Field<int>("SupplierId") == SupplierId
						select row).Sum((DataRow row) => row.Field<decimal>("BillAmount")));
					double prcnt = Conversion.ToDouble(item.Cells["%ofTotal"].Value);
					if (prcnt > 100.0)
					{
						MessageBox.Show("%of Total Can't Greater than 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["%ofTotal"].Value = 100;
						prcnt = 100.0;
					}
					double amount = num * prcnt / 100.0;
					item.Cells["Amount"].Value = Math.Round(amount, 4);
				}
				if (ColumnName.Contains("DueDays"))
				{
					int days = Conversion.ToInt(item.Cells["DueDays"].Value);
					item.Cells["DueDate"].Value = DocDate.Value.AddDays(days);
				}
				item.EndEdit();
			}
			grdPaymentTermSupplier.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtbillno.Text.Trim() == string.Empty || txtbillno.Text.Trim() == "0" || txtbillno.Text.Trim() == null)
		{
			MessageBox.Show("Bill No Field is Required");
			txtbillno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCustomername).ActiveRow == null || Conversion.ToInt(cmbCustomername.Value) == 0)
		{
			MessageBox.Show("Customer Name Field is Required");
			cmbCustomername.Focus();
			return false;
		}
		if (((UltraGridBase)cmbpayterm).ActiveRow == null || Conversion.ToInt(cmbpayterm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			cmbpayterm.Focus();
			return false;
		}
		if (((Control)(object)cmbpayterm).Text == "Credit" && Conversion.ToInt((object)txtduedays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required when Payment Term is Credit");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)cmbdeliveryterm).ActiveRow == null || Conversion.ToInt(cmbdeliveryterm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			cmbdeliveryterm.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbcurrency).ActiveRow == null || Conversion.ToInt(cmbcurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbcurrency.Focus();
				return false;
			}
			if (txtexchangerate.Text.Trim() == string.Empty || txtexchangerate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtexchangerate.Focus();
				return false;
			}
			if (txtfcyamount.Text.Trim() == string.Empty || txtfcyamount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtfcyamount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbcurrency).ActiveRow == null || Conversion.ToInt(cmbcurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbcurrency.Focus();
				return false;
			}
			if (txtexchangerate.Text.Trim() == string.Empty || txtexchangerate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtexchangerate.Focus();
				return false;
			}
		}
		return true;
	}

	private void ValidationforSupplierPT()
	{
		try
		{
			foreach (int supplierId2 in (from row in dtDetail.AsEnumerable()
				select row.Field<int>("SupplierAccountId")).Distinct().ToList())
			{
				if (!dtSupplierPTDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierId") == supplierId2))
				{
					string supplierName3 = (from r in dtSupplier.AsEnumerable()
						where r.Field<int>("Id") == supplierId2
						select r.Field<string>("CompanyName")).FirstOrDefault();
					throw new Exception("Please check payment detail of supplier '" + supplierName3 + "' not found in supplier payment detail grid.");
				}
			}
			foreach (int supplierId in (from row in dtSupplierPTDetail.AsEnumerable()
				select row.Field<int>("SupplierId")).Distinct().ToList())
			{
				List<DataRow> matchingRows = (from r in dtSupplierPTDetail.AsEnumerable()
					where r.Field<int>("SupplierId") == supplierId
					select r).ToList();
				if (Math.Round(matchingRows.Sum((DataRow r) => r.Field<decimal>("%ofTotal")), 0) != 100m)
				{
					string supplierName = (from r in dtSupplier.AsEnumerable()
						where r.Field<int>("Id") == supplierId
						select r.Field<string>("CompanyName")).FirstOrDefault();
					throw new Exception("Please check %ofTotal against SupplierName '" + supplierName + "' is not equal to 100.");
				}
				if (matchingRows.Select((DataRow r) => r.Field<int>("PaymentTermId")).Distinct().ToList()
					.Count != matchingRows.Count)
				{
					string supplierName2 = (from r in dtSupplier.AsEnumerable()
						where r.Field<int>("Id") == supplierId
						select r.Field<string>("CompanyName")).FirstOrDefault();
					throw new Exception("Supplier '" + supplierName2 + "' has duplicate Payment Terms in the payment detail grid.");
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Expected O, but got Unknown
		//IL_16e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ea: Expected O, but got Unknown
		//IL_13c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ce: Expected O, but got Unknown
		//IL_1a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a15: Expected O, but got Unknown
		//IL_1d12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d19: Expected O, but got Unknown
		//IL_207d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2084: Expected O, but got Unknown
		//IL_2266: Unknown result type (might be due to invalid IL or missing references)
		//IL_226d: Expected O, but got Unknown
		//IL_2384: Unknown result type (might be due to invalid IL or missing references)
		//IL_238b: Expected O, but got Unknown
		//IL_2549: Unknown result type (might be due to invalid IL or missing references)
		//IL_2550: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			CommTradeTransactionHeader ct = new CommTradeTransactionHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ct.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
			GridEXRow[] rows = grdrevenue.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if ((Conversion.ToDouble(r4.Cells["AmountCr"].Value) > 0.0 || Conversion.ToInt(r4.Cells["AmountDr"].Value) > 0) && Conversion.ToInt(r4.Cells["OtherRevenueId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Other Item In Revenue Grid In Row No" + (r4.RowIndex + 1));
					return;
				}
			}
			rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Packing Material Expense In Row No" + (r6.RowIndex + 1));
					return;
				}
			}
			ValidationforSupplierPT();
			dtCommission.Clear();
			if (Conversion.ToInt(cmbCommAgent.Value) != 0 && Conversion.ToDecimal((object)txtcommrate.Text) != 0m)
			{
				dtCommission.Rows.Add(0, Conversion.ToInt(cmbCommAgent.Value), 0, 5, 1, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtcommrate.Text), Conversion.ToDecimal((object)txtcommamount.Text), Conversion.ToInt64(cmbCustomername.Value));
				dtCommission.Rows.Add(0, Conversion.ToInt(cmbBrokeryAccount.Value), 0, 6, 1, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtbrokeryrate.Text), Conversion.ToDecimal((object)txtbrokeryamount.Text), Conversion.ToInt64(cmbCustomername.Value));
				dtCommission.Rows.Add(0, 0, Conversion.ToInt(cmbsubbrokeryacc.Value), 6, 2, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtsubbrokeryrate.Text), Conversion.ToDecimal((object)txtsubbrokeryamnt.Text), Conversion.ToInt64(cmbCommAgent.Value));
			}
			ct.OrganizationId = UserAccount.OrganizationId;
			ct.CompanyId = UserAccount.CompanyId;
			ct.BranchesId = UserAccount.BranchesId;
			ct.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ct.DocumentTypeId = 1704;
			ct.ModifyUserId = UserAccount.ID;
			ct.EntryUserId = UserAccount.ID;
			ct.ApprovedUserId = UserAccount.ID;
			ct.EntryDate = DateTime.Now;
			ct.ModifyDate = DateTime.Now;
			ct.ApprovedDate = DateTime.Now;
			ct.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			ct.DocNo = Conversion.ToInt((object)txtbillno.Text);
			ct.ManualBillNo = txtmanualref.Text;
			ct.IsApproved = false;
			ct.VehicleNos = Conversion.ToString((object)txtVehicleNos.Text);
			ct.BiltyNos = Conversion.ToString((object)txtBiltyNos.Text);
			ct.BiltyDates = Conversion.ToString((object)txtBiltyDates.Text);
			ct.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			ct.ScreenName = Conversion.ToString((object)base.Name);
			ct.DifferenceAccountId = Conversion.ToInt(cmbDifferencAccount.Value);
			ct.CommissionAgentId = Conversion.ToInt(cmbCommAgent.Value);
			ct.CalculationParameterId = Conversion.ToInt(cmbcommtype.Value);
			ct.CommRate = Conversion.ToDecimal((object)txtcommrate.Text);
			ct.CommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			ct.BrokerAgentId = Conversion.ToInt(cmbBrokeryAccount.Value);
			ct.BrokeryRate = Conversion.ToDecimal((object)txtbrokeryrate.Text);
			ct.BrokeryAmount = Conversion.ToDecimal((object)txtbrokeryamount.Text);
			ct.SubBrokerId = Conversion.ToInt(cmbsubbrokeryacc.Value);
			ct.SubBrokeryRate = Conversion.ToDecimal((object)txtsubbrokeryrate.Text);
			ct.SubBrokeryAmount = Conversion.ToDecimal((object)txtsubbrokeryamnt.Text);
			ct.BillAmount = Conversion.ToDecimal((object)txtcustbillamnt.Text);
			ct.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
			ct.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
			ct.FcyAmount = Conversion.ToDecimal((object)txtfcyamount.Text);
			ct.CommTradeTransactionDetailList = new List<CommTradeTransactionDetail>();
			ct.CommTradeTransactionPmOtherList = new List<CommTradeTransactionPmOther>();
			ct.CommTradeTransactionRevExpList = new List<CommTradeTransactionRevExp>();
			ct.CommTradeTransactionCommBrokeryList = new List<CommTradeTransactionCommBrokery>();
			ct.CommTradeTransactionSupplierSummaryList = new List<CommTradeTransactionSupplierSummary>();
			ct.CommTradeTransactionPaymentTermsDetailList = new List<CommTradeTransactionPaymentTermsDetail>();
			ct.CommTradeTransactionVehicleFreightDetailList = new List<CommTradeTransactionVehicleFreightDetail>();
			ct.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			ct.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			decimal TotalSupAmount = default(decimal);
			decimal TotalCusAmount = default(decimal);
			rows = grdDetail.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				CommTradeTransactionDetail vd = new CommTradeTransactionDetail();
				vd.Id = Conversion.ToInt(r7.Cells["Id"].Value);
				if (vd.Id > 0)
				{
					vd.ActionTypeId = 2;
				}
				else
				{
					vd.ActionTypeId = 1;
				}
				vd.CommTradeOrderId = Conversion.ToInt(r7.Cells["OrderId"].Value);
				vd.CommTradeOrderDetailId = Conversion.ToInt(r7.Cells["OrderDetailId"].Value);
				vd.CommTradeLoadingDeliveryId = Conversion.ToInt(r7.Cells["LoadingDeliveryId"].Value);
				vd.CommTradeLoadingDeliveryDetailId = Conversion.ToInt(r7.Cells["LoadingDeliveryDetailId"].Value);
				vd.dDocDate = Conversion.ToDateTime((object)DocDate.Value);
				vd.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
				vd.CusDeliveryTerm = Conversion.ToString((object)((Control)(object)cmbdeliveryterm).Text);
				vd.CusDueDays = Conversion.ToInt((object)txtduedays.Text);
				vd.SupDueDays = Conversion.ToInt((object)txtduedays.Text);
				vd.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
				vd.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
				vd.BiltyFreight = Conversion.ToDecimal(r7.Cells["BiltyFreight"].Value);
				vd.BiltyNo = Conversion.ToString(r7.Cells["BiltyNo"].Value);
				vd.BiltyDate = Conversion.ToDateTime(r7.Cells["BiltyDate"].Value);
				vd.VehicleNo = Conversion.ToString(r7.Cells["VehicleNo"].Value);
				vd.VehicleQty = Conversion.ToDecimal(r7.Cells["VehicleQty"].Value);
				vd.VehicleGrossWeight = Conversion.ToDecimal(r7.Cells["VehicleGrossWeight"].Value);
				if (Conversion.ToInt(r7.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.ItemId = Conversion.ToInt(r7.Cells["ItemId"].Value);
				if (Conversion.ToInt(r7.Cells["CropYearId"].Value) == 0)
				{
					MessageBox.Show("CropYear Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.CropYearId = Conversion.ToInt(r7.Cells["CropYearId"].Value);
				if (Conversion.ToInt(r7.Cells["PackingTypeId"].Value) == 0)
				{
					MessageBox.Show("PackingType Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.PackingTypeId = Conversion.ToInt(r7.Cells["PackingTypeId"].Value);
				if (Conversion.ToInt(r7.Cells["UomId"].Value) == 0)
				{
					MessageBox.Show("Uom Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.UomId = Conversion.ToInt(r7.Cells["UomId"].Value);
				if (Conversion.ToDecimal(r7.Cells["ItemQty"].Value) == 0m)
				{
					MessageBox.Show("ItemQty Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.Qty = Conversion.ToDecimal(r7.Cells["ItemQty"].Value);
				if (Conversion.ToInt(r7.Cells["SupplierAccountId"].Value) == 0)
				{
					MessageBox.Show("SupplierAccount Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.SupplierId = Conversion.ToInt(r7.Cells["SupplierAccountId"].Value);
				if (Conversion.ToDecimal(r7.Cells["SupGrossWeight"].Value) == 0m)
				{
					MessageBox.Show("SupGrossWeight Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.SupGrossWeight = Conversion.ToDecimal(r7.Cells["SupGrossWeight"].Value);
				vd.SupPckMtrlWeight = Conversion.ToDecimal(r7.Cells["SupPMWeight"].Value);
				vd.SupAdLsWt = Conversion.ToDecimal(r7.Cells["SupAdLsWeight"].Value);
				if (Conversion.ToDecimal(r7.Cells["SupNetWeight"].Value) == 0m)
				{
					MessageBox.Show("SupNetWeight Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.SupNetWeight = Conversion.ToDecimal(r7.Cells["SupNetWeight"].Value);
				if (Conversion.ToDecimal(r7.Cells["SupRate"].Value) == 0m)
				{
					MessageBox.Show("SupRate Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.SupRate = Conversion.ToDecimal(r7.Cells["SupRate"].Value);
				if (Conversion.ToDecimal(r7.Cells["SupAmount"].Value) == 0m)
				{
					MessageBox.Show("SupAmount Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.SupAmount = Conversion.ToDecimal(r7.Cells["SupAmount"].Value);
				TotalSupAmount += Conversion.ToDecimal(r7.Cells["SupAmount"].Value);
				vd.SupFcyAmount = Conversion.ToDecimal(r7.Cells["SupFcyAmount"].Value);
				vd.CustomerId = Conversion.ToInt(cmbCustomername.Value);
				if (Conversion.ToDecimal(r7.Cells["CusGrossWeight"].Value) == 0m)
				{
					MessageBox.Show("CusGrossWeight Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.CusGrossWeight = Conversion.ToDecimal(r7.Cells["CusGrossWeight"].Value);
				vd.CusPckMtrlWeight = Conversion.ToDecimal(r7.Cells["CusPMWeight"].Value);
				vd.CusAdLsWt = Conversion.ToDecimal(r7.Cells["CusAdLsWeight"].Value);
				if (Conversion.ToDecimal(r7.Cells["CusNetWeight"].Value) == 0m)
				{
					MessageBox.Show("CusNetWeight Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.CusNetWeight = Conversion.ToDecimal(r7.Cells["CusNetWeight"].Value);
				if (Conversion.ToDecimal(r7.Cells["CusRate"].Value) == 0m)
				{
					MessageBox.Show("CusRate Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.CusRate = Conversion.ToDecimal(r7.Cells["CusRate"].Value);
				if (Conversion.ToDecimal(r7.Cells["CusAmount"].Value) == 0m)
				{
					MessageBox.Show("CusAmount Is Required In Detail Grid In row No " + (r7.RowIndex + 1));
					return;
				}
				vd.CusAmount = Conversion.ToDecimal(r7.Cells["CusAmount"].Value);
				TotalCusAmount += Conversion.ToDecimal(r7.Cells["CusAmount"].Value);
				vd.CustFcyAmount = Conversion.ToDecimal(r7.Cells["CusFcyAmount"].Value);
				DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r7.Cells["ItemId"].Value));
				if (dt.Rows.Count > 0)
				{
					DataRow[] dr = dt.Select("Equivalent='" + 40 + "'");
					if (dr.Length == 0)
					{
						MessageBox.Show("RateUom of the Item " + Conversion.ToString((object)r7.Cells["ItemId"].Text) + " in row No " + (r7.RowIndex + 1) + " is not define please check ");
						return;
					}
					vd.CusRateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
					vd.SupRateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				ct.CommTradeTransactionDetailList.Add(vd);
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int k = 0; k < lstRemoveRecord.Count; k++)
				{
					ct.CommTradeTransactionDetailList.Add(lstRemoveRecord[k]);
				}
			}
			if (TotalCusAmount != TotalSupAmount && ct.DifferenceAccountId == 0)
			{
				if (FluctuationDifferenceAccountValue > 0m && TotalCusAmount - TotalSupAmount > FluctuationDifferenceAccountValue)
				{
					MessageBox.Show("DifferenceAccount Field is Required");
					cmbDifferencAccount.Focus();
					return;
				}
				if (TotalCusAmount - TotalSupAmount > 1m)
				{
					MessageBox.Show("DifferenceAccount Field is Required");
					cmbDifferencAccount.Focus();
					return;
				}
				ct.DifferenceAccountId = FluctuationDifferenceAccountId;
			}
			rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				if (Conversion.ToInt((object)r8.Cells["ItemId"].Value.ToString()) != 0)
				{
					int PackingTypeId = Conversion.ToInt(r8.Cells["PackingTypeId"].Value);
					int num = Conversion.ToInt(r8.Cells["ItemId"].Value);
					Conversion.ToInt(r8.Cells["CreditAccountId"].Value);
					int StockTypeId = Conversion.ToInt(r8.Cells["StockTypeId"].Value);
					if (num == 0)
					{
						MessageBox.Show("Please Select an Item Against Packing Material Expense In Row No" + (r8.RowIndex + 1));
						return;
					}
					if (PackingTypeId == 0)
					{
						MessageBox.Show("Please Select PackingType  Against Packing Material Expense In Row No" + (r8.RowIndex + 1));
						return;
					}
					if (StockTypeId == 0)
					{
						MessageBox.Show("Please Select StockType Against Packing Material In Row No" + (r8.RowIndex + 1));
						return;
					}
					CommTradeTransactionPmOther PE = new CommTradeTransactionPmOther();
					PE.Id = Conversion.ToInt(r8.Cells["Id"].Value);
					PE.StockTypeId = StockTypeId;
					PE.WareHouseId = Conversion.ToInt(r8.Cells["WareHouseId"].Value);
					PE.LoadingDeliveryId = Conversion.ToInt(r8.Cells["LoadingDeliveryId"].Value);
					PE.LoadingDeliveryPmOtherId = Conversion.ToInt(r8.Cells["LoadingDeliveryPmOtherId"].Value);
					PE.PackingMaterialId = Conversion.ToInt(r8.Cells["PackingTypeId"].Value);
					PE.ItemId = Conversion.ToInt(r8.Cells["ItemId"].Value);
					PE.ItemSize = Conversion.ToString(r8.Cells["ItemSize"].Value);
					PE.ItemWeight = Conversion.ToDecimal(r8.Cells["Weight"].Value);
					PE.Qty = Conversion.ToInt(r8.Cells["Qty"].Value);
					if (PE.StockTypeId == 1 && PE.Qty == 0m)
					{
						MessageBox.Show("Please Insert Qty (because StockType is Company) Packing Material In Row No" + (r8.RowIndex + 1));
						return;
					}
					PE.Rate = Conversion.ToDecimal(r8.Cells["Rate"].Value);
					if (PE.StockTypeId == 1 && PE.Rate == 0m)
					{
						MessageBox.Show("Please Insert Rate (because StockType is Company) Packing Material In Row No" + (r8.RowIndex + 1));
						return;
					}
					PE.Amount = Conversion.ToDecimal(r8.Cells["Amount"].Value);
					if (PE.Amount == 0m)
					{
						MessageBox.Show("Please Insert Amount Against Packing Material Expense In Row No" + (r8.RowIndex + 1));
						return;
					}
					PE.CreditAccountId = Conversion.ToInt(r8.Cells["CreditAccountId"].Value);
					ct.CommTradeTransactionPmOtherList.Add(PE);
				}
			}
			rows = grdrevenue.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToInt((object)r5.Cells["SupplierAccountId"].Value.ToString()) != 0 || Conversion.ToDecimal((object)r5.Cells["AmountCr"].Text) != 0m || Conversion.ToDecimal((object)r5.Cells["AmountDr"].Text) != 0m)
				{
					CommTradeTransactionRevExp PJ7 = new CommTradeTransactionRevExp();
					PJ7.Id = Conversion.ToInt(r5.Cells["Id"].Value);
					PJ7.TypeId = 1;
					PJ7.LoadingDeliveryId = Conversion.ToInt(r5.Cells["LoadingDeliveryId"].Value);
					PJ7.LoadingDeliveryRevExpId = Conversion.ToInt(r5.Cells["LoadingDeliveryRevExpId"].Value);
					if (Conversion.ToInt(r5.Cells["OtherRevenueId"].Value) == 0)
					{
						MessageBox.Show("OtherRevenue Ac Is Required In Revenue Grid In row No " + (r5.RowIndex + 1));
						return;
					}
					PJ7.AccountId = Conversion.ToInt(r5.Cells["OtherRevenueId"].Value);
					PJ7.SupplierCustomerId = Conversion.ToInt(r5.Cells["SupplierAccountId"].Value);
					if (Conversion.ToDecimal((object)r5.Cells["AmountCr"].Text) == 0m && Conversion.ToDecimal((object)r5.Cells["AmountDr"].Text) == 0m)
					{
						MessageBox.Show("AmountCr/AmountDr Is Required In Revenue Grid In row No " + (r5.RowIndex + 1));
						return;
					}
					PJ7.Qty = Conversion.ToDecimal(r5.Cells["Qty"].Value);
					PJ7.Rate = Conversion.ToDecimal(r5.Cells["Rate"].Value);
					PJ7.DebitAmt = Conversion.ToDecimal(r5.Cells["AmountDr"].Value);
					PJ7.CreditAmt = Conversion.ToDecimal(r5.Cells["AmountCr"].Value);
					PJ7.ReRemarks = "OtherRevenue : " + Conversion.ToString((object)r5.Cells["OtherRevenueId"].Text) + " Qty: " + Conversion.ToString((object)r5.Cells["Qty"].Text) + " Rate: " + Conversion.ToString((object)r5.Cells["Rate"].Text) + " AmountDr: " + Conversion.ToString((object)r5.Cells["AmountDr"].Text) + " AmountCr: " + Conversion.ToString((object)r5.Cells["AmountCr"].Text);
					ct.CommTradeTransactionRevExpList.Add(PJ7);
				}
			}
			rows = grdVehicleDetail.GetCheckedRows();
			foreach (GridEXRow r3 in rows)
			{
				if (!string.IsNullOrEmpty(Conversion.ToString(r3.Cells["VehicleNo"].Value)))
				{
					CommTradeTransactionVehicleFreightDetail PJ6 = new CommTradeTransactionVehicleFreightDetail();
					if (Conversion.ToInt(r3.Cells["CreditAccountId"].Value) == 0)
					{
						tabControl2.SelectedIndex = 6;
						MessageBox.Show("CreditAccount Is Required In Vehicle Grid In row No " + (r3.RowIndex + 1));
						return;
					}
					if (Conversion.ToDecimal(r3.Cells["NetPaid"].Value) == 0m)
					{
						tabControl2.SelectedIndex = 6;
						MessageBox.Show("NetPaid Is Required In Vehicle Grid In row No " + (r3.RowIndex + 1));
						return;
					}
					PJ6.Id = Conversion.ToInt(r3.Cells["Id"].Value);
					PJ6.LoadingDeliveryId = Conversion.ToInt(r3.Cells["LoadingDeliveryId"].Value);
					PJ6.CustomerId = Conversion.ToInt(r3.Cells["CustomerId"].Value);
					PJ6.CustomerId = ((Conversion.ToInt(r3.Cells["CustomerId"].Value) > 0) ? Conversion.ToInt(r3.Cells["CustomerId"].Value) : Conversion.ToInt(cmbCustomername.Value));
					PJ6.VehicleNo = Conversion.ToString(r3.Cells["VehicleNo"].Value);
					PJ6.BiltyNo = Conversion.ToString(r3.Cells["BiltyNo"].Value);
					PJ6.VehicleQty = Conversion.ToDecimal(r3.Cells["VehicleQty"].Value);
					PJ6.BiltyFreight = Conversion.ToDecimal(r3.Cells["BiltyFreight"].Value);
					PJ6.NetPaid = Conversion.ToDecimal(r3.Cells["NetPaid"].Value);
					PJ6.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					PJ6.CreditAccountId = Conversion.ToInt(r3.Cells["CreditAccountId"].Value);
					ct.CommTradeTransactionVehicleFreightDetailList.Add(PJ6);
				}
			}
			rows = grdCustomerRevenueExpense.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["SupplierAccountId"].Value.ToString()) != 0 || Conversion.ToDecimal((object)r2.Cells["AmountCr"].Text) != 0m || Conversion.ToDecimal((object)r2.Cells["AmountDr"].Text) != 0m)
				{
					CommTradeTransactionRevExp PJ5 = new CommTradeTransactionRevExp();
					PJ5.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					PJ5.TypeId = 2;
					PJ5.LoadingDeliveryId = Conversion.ToInt(r2.Cells["LoadingDeliveryId"].Value);
					PJ5.LoadingDeliveryRevExpId = Conversion.ToInt(r2.Cells["LoadingDeliveryRevExpId"].Value);
					if (Conversion.ToInt(r2.Cells["OtherRevenueId"].Value) == 0)
					{
						MessageBox.Show("OtherRevenue Ac Is Required In Customer Revenue Grid In row No " + (r2.RowIndex + 1));
						return;
					}
					PJ5.AccountId = Conversion.ToInt(r2.Cells["OtherRevenueId"].Value);
					PJ5.SupplierCustomerId = Conversion.ToInt(r2.Cells["SupplierAccountId"].Value);
					if (Conversion.ToDecimal((object)r2.Cells["AmountCr"].Text) == 0m && Conversion.ToDecimal((object)r2.Cells["AmountDr"].Text) == 0m)
					{
						MessageBox.Show("AmountCr/AmountDr Is Required In Customer Revenue Grid In row No " + (r2.RowIndex + 1));
						return;
					}
					PJ5.Qty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
					PJ5.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					PJ5.DebitAmt = Conversion.ToDecimal(r2.Cells["AmountDr"].Value);
					PJ5.CreditAmt = Conversion.ToDecimal(r2.Cells["AmountCr"].Value);
					PJ5.ReRemarks = "OtherRevenue : " + Conversion.ToString((object)r2.Cells["OtherRevenueId"].Text) + " Qty: " + Conversion.ToString((object)r2.Cells["Qty"].Text) + " Rate: " + Conversion.ToString((object)r2.Cells["Rate"].Text) + " AmountDr: " + Conversion.ToString((object)r2.Cells["AmountDr"].Text) + " AmountCr: " + Conversion.ToString((object)r2.Cells["AmountCr"].Text);
					ct.CommTradeTransactionRevExpList.Add(PJ5);
				}
			}
			for (int j = 0; j < dtCommission.Rows.Count; j++)
			{
				if ((Conversion.ToInt(dtCommission.Rows[j]["CommissionAgentId"]) != 0 || Conversion.ToInt(dtCommission.Rows[j]["CommissionAgentGLId"]) != 0) && Conversion.ToDecimal(dtCommission.Rows[j]["Rate"]) != 0m)
				{
					CommTradeTransactionCommBrokery PJ4 = new CommTradeTransactionCommBrokery();
					PJ4.Id = Conversion.ToInt(dtCommission.Rows[j]["Id"]);
					PJ4.CommissionAgentId = Conversion.ToInt(dtCommission.Rows[j]["CommissionAgentId"]);
					PJ4.CommissionAgentGLId = Conversion.ToInt(dtCommission.Rows[j]["CommissionAgentGLId"]);
					if (PJ4.CommissionAgentId == 0 && PJ4.CommissionAgentGLId == 0)
					{
						MessageBox.Show("CommissionAgent  Is Required");
						return;
					}
					PJ4.ChargesIncentiveId = Conversion.ToInt(dtCommission.Rows[j]["ChargesIncentiveId"]);
					PJ4.RevenueExpenseId = Conversion.ToInt(dtCommission.Rows[j]["RevenueExpenseId"]);
					PJ4.CalcParameterId = Conversion.ToInt(dtCommission.Rows[j]["CalcParameterId"]);
					PJ4.Rate = Conversion.ToDecimal(dtCommission.Rows[j]["Rate"]);
					PJ4.Amount = Conversion.ToDecimal(dtCommission.Rows[j]["Amount"]);
					PJ4.SupplierCustomerId = Conversion.ToInt(dtCommission.Rows[j]["SupplierCustomerId"]);
					ct.CommTradeTransactionCommBrokeryList.Add(PJ4);
				}
			}
			for (int i = 0; i < dtsummaryBySupplier.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtsummaryBySupplier.Rows[i]["SupplierId"]) != 0)
				{
					CommTradeTransactionSupplierSummary PJ3 = new CommTradeTransactionSupplierSummary();
					PJ3.SupplierId = Conversion.ToInt(dtsummaryBySupplier.Rows[i]["SupplierId"]);
					PJ3.ItemAmount = Conversion.ToInt(dtsummaryBySupplier.Rows[i]["ItemAmount"]);
					PJ3.PackingAmount = Conversion.ToInt(dtsummaryBySupplier.Rows[i]["PackingAmount"]);
					PJ3.OtherExpenses = Conversion.ToInt(dtsummaryBySupplier.Rows[i]["Expenses"]);
					PJ3.BillAmount = Conversion.ToDecimal(dtsummaryBySupplier.Rows[i]["BillAmount"]);
					ct.CommTradeTransactionSupplierSummaryList.Add(PJ3);
				}
			}
			if (Conversion.ToInt(cmbCustomername.Value) > 0)
			{
				CommTradeTransactionPaymentTermsDetail PJ2 = new CommTradeTransactionPaymentTermsDetail();
				PJ2.Id = 0;
				PJ2.SupplierCustomerTypeId = 2;
				PJ2.SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value);
				PJ2.PaymentTermId = Conversion.ToInt(cmbpayterm.Value);
				PJ2.PrcntOfTotal = 100m;
				PJ2.Amount = Conversion.ToDecimal((object)txtcustbillamnt.Text);
				PJ2.DueDays = Conversion.ToInt((object)txtduedays.Text);
				PJ2.DueDate = Conversion.ToDateTime((object)DocDate.Value).AddDays(Conversion.ToInt((object)txtduedays.Text));
				PJ2.PaymentRemarks = "CustomerName " + Conversion.ToString((object)((Control)(object)cmbCustomername).Text) + " And PaymentTerm: " + Conversion.ToString((object)((Control)(object)cmbpayterm).Text) + " " + $"And DueDays: {PJ2.DueDays} " + $"And DueDate: {PJ2.DueDate} " + $"And Amount: {PJ2.Amount} ";
				ct.CommTradeTransactionPaymentTermsDetailList.Add(PJ2);
			}
			if (grdPaymentTermSupplier.GetRows().Count() <= 0)
			{
				tabControl2.SelectedIndex = 4;
				((Control)(object)grdPaymentTermSupplier).Focus();
				MessageBox.Show("Supplier Payment Detail Record Not Found", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string SupplierIds = "";
			rows = grdPaymentTermSupplier.GetRows();
			foreach (GridEXRow r in rows)
			{
				CommTradeTransactionPaymentTermsDetail PJ = new CommTradeTransactionPaymentTermsDetail();
				PJ.Id = Conversion.ToInt(r.Cells["Id"].Value);
				PJ.SupplierCustomerTypeId = 1;
				PJ.CommTradeLoadingDeliveryId = Conversion.ToInt(r.Cells["CommTradeLoadingDeliveryId"].Value);
				PJ.LoadingDeliveryPaymentDetailId = Conversion.ToInt(r.Cells["LoadingDeliveryPaymentDetailId"].Value);
				PJ.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierId"].Value);
				SupplierIds = SupplierIds + "," + Conversion.ToString((object)PJ.SupplierCustomerId);
				if (Conversion.ToInt(r.Cells["SupplierId"].Value) == 0)
				{
					tabControl2.SelectedIndex = 4;
					MessageBox.Show("Supplier Is Required In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
				if (Conversion.ToInt(r.Cells["PaymentTermId"].Value) == 0)
				{
					tabControl2.SelectedIndex = 4;
					MessageBox.Show("PaymentTerm Is Required In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.PrcntOfTotal = Conversion.ToDecimal(r.Cells["%ofTotal"].Value);
				if (Conversion.ToInt(r.Cells["%ofTotal"].Value) == 0)
				{
					tabControl2.SelectedIndex = 4;
					MessageBox.Show("%ofTotal Is Required In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
				if (Conversion.ToDecimal((object)r.Cells["Amount"].Text) == 0m)
				{
					tabControl2.SelectedIndex = 4;
					MessageBox.Show("Amount Is Required In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
				if (PJ.PaymentTermId == 11 && PJ.DueDays == 0)
				{
					tabControl2.SelectedIndex = 4;
					MessageBox.Show("DueDays Is Required When PaymentTerm is Credit In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.DueDate = Conversion.ToDateTime(r.Cells["DueDate"].Value);
				if (Conversion.CheckDateTimeNull(PJ.DueDate) && PJ.PaymentTermId == 11)
				{
					tabControl2.SelectedIndex = 4;
					MessageBox.Show("DueDate Is Required When PaymentTerm is Credit In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.PaymentRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				ct.CommTradeTransactionPaymentTermsDetailList.Add(PJ);
			}
			if (!string.IsNullOrEmpty(SupplierIds))
			{
				IEnumerable<int> enumerable = from id in SupplierIds.Split(',').Distinct()
					select Conversion.ToInt((object)id);
				decimal PaymentDetailAmount = default(decimal);
				decimal PrcntOfTotal = default(decimal);
				foreach (int Id in enumerable)
				{
					if (Id <= 0)
					{
						continue;
					}
					decimal SupplierAmountInDetail = (from row in dtsummaryBySupplier.AsEnumerable()
						where row.Field<int>("SupplierId") == Id
						select row).Sum((DataRow row) => row.Field<decimal>("BillAmount"));
					PaymentDetailAmount = (from row in dtSupplierPTDetail.AsEnumerable()
						where row.Field<int>("SupplierId") == Id
						select row).Sum((DataRow row) => row.Field<decimal>("Amount"));
					PrcntOfTotal = (from row in dtSupplierPTDetail.AsEnumerable()
						where row.Field<int>("SupplierId") == Id
						select row).Sum((DataRow row) => row.Field<decimal>("%ofTotal"));
					if (Math.Round(PaymentDetailAmount, 4) != Math.Round(SupplierAmountInDetail, 4))
					{
						string SupplierName = (from row in dtSupplier.AsEnumerable()
							where row.Field<int>("Id") == Id
							select row.Field<string>("CompanyName")).FirstOrDefault();
						MessageBox.Show("Payment Detail Amount " + PaymentDetailAmount.ToString("#,##0.####") + " Not Equal to Total Bill Amount " + SupplierAmountInDetail.ToString("#,##0.####") + " for Supplier " + SupplierName, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 4;
						((Control)(object)grdPaymentTermSupplier).Focus();
						return;
					}
					if (Math.Round(PrcntOfTotal, 2) != 100m)
					{
						string SupplierName2 = (from row in dtSupplier.AsEnumerable()
							where row.Field<int>("Id") == Id
							select row.Field<string>("CompanyName")).FirstOrDefault();
						MessageBox.Show("Payment Detail Total% not equal to 100 for Supplier " + SupplierName2, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 4;
						((Control)(object)grdPaymentTermSupplier).Focus();
						return;
					}
				}
			}
			FormHelper.UpdateAttachmentsForObject<CommTradeTransactionHeader>(ct, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = (ct.Id = CommTradeTransactionHeader.Save(ct));
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + ct.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, ct.DocumentTypeId, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + ct.DocNo);
			}
			Reset();
			if (ChkPrintslip.Checked)
			{
				GenerateSlip(success);
			}
			if (ChkBok.Checked)
			{
				GenerateVoucherSlip(success);
			}
			if (ChkUrduPrintPreview.Checked)
			{
				GenerateMultiLangPrint(success);
			}
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
			Insert();
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
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					CommTradeTransactionHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			CommTradeTransactionHeader po = new CommTradeTransactionHeader();
			RecId = Conversion.ToInt((object)ID);
			po = CommTradeTransactionHeader.ReadById(RecId);
			tabControl1.SelectedIndex = 0;
			DocDate.Value = po.DocDate;
			cmbcurrency.Value = po.CurrencyId;
			txtexchangerate.Text = Conversion.ToString((object)po.ExchangeRate);
			txtbillno.Text = Conversion.ToString((object)po.DocNo);
			txtmanualref.Text = Conversion.ToString((object)po.ManualBillNo);
			txtRemarks.Text = Conversion.ToString((object)po.RemarksHeader);
			txtBiltyNos.Text = Conversion.ToString((object)po.BiltyNos);
			txtBiltyDates.Text = Conversion.ToString((object)po.BiltyDates);
			txtVehicleNos.Text = Conversion.ToString((object)po.VehicleNos);
			cmbDifferencAccount.Value = Conversion.ToInt((object)po.DifferenceAccountId);
			if (po.CommTradeTransactionCommBrokeryList.Count > 0)
			{
				for (int n = 0; n < po.CommTradeTransactionCommBrokeryList.Count; n++)
				{
					if (po.CommTradeTransactionCommBrokeryList[n].RevenueExpenseId == 1 && po.CommTradeTransactionCommBrokeryList[n].ChargesIncentiveId == 5)
					{
						cmbCommAgent.Value = Conversion.ToInt((object)po.CommTradeTransactionCommBrokeryList[n].CommissionAgentId);
						cmbcommtype.Value = Conversion.ToInt((object)po.CommTradeTransactionCommBrokeryList[n].CalcParameterId);
						txtcommrate.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[n].Rate);
						txtcommamount.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[n].Amount);
					}
					if (po.CommTradeTransactionCommBrokeryList[n].RevenueExpenseId == 1 && po.CommTradeTransactionCommBrokeryList[n].ChargesIncentiveId == 6)
					{
						cmbBrokeryAccount.Value = Conversion.ToInt((object)po.CommTradeTransactionCommBrokeryList[n].CommissionAgentId);
						txtbrokeryrate.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[n].Rate);
						txtbrokeryamount.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[n].Amount);
					}
					if (po.CommTradeTransactionCommBrokeryList[n].RevenueExpenseId == 2 && po.CommTradeTransactionCommBrokeryList[n].ChargesIncentiveId == 6)
					{
						cmbsubbrokeryacc.Value = Conversion.ToInt((object)po.CommTradeTransactionCommBrokeryList[n].CommissionAgentId);
						txtsubbrokeryrate.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[n].Rate);
						txtsubbrokeryamnt.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[n].Amount);
					}
				}
			}
			if (po.CommTradeTransactionDetailList.Count > 0)
			{
				cmbCustomername.Value = po.CommTradeTransactionDetailList[0].CustomerId;
				((Control)(object)cmbCustomername).Enabled = false;
				cmbpayterm.Value = po.CommTradeTransactionDetailList[0].CusPaymentTermsId;
				((Control)(object)cmbdeliveryterm).Text = po.CommTradeTransactionDetailList[0].CusDeliveryTerm;
				txtduedays.Text = Conversion.ToString((object)po.CommTradeTransactionDetailList[0].CusDueDays);
				dtDetail.Clear();
				for (int m = 0; m < po.CommTradeTransactionDetailList.Count; m++)
				{
					dtDetail.Rows.Add(po.CommTradeTransactionDetailList[m].Id, po.CommTradeTransactionDetailList[m].CommTradeOrderId, po.CommTradeTransactionDetailList[m].CommTradeOrderDetailId, po.CommTradeTransactionDetailList[m].OrderNo, po.CommTradeTransactionDetailList[m].CommTradeLoadingDeliveryId, po.CommTradeTransactionDetailList[m].CommTradeLoadingDeliveryDetailId, po.CommTradeTransactionDetailList[m].LoadingDeliveryDocNo, po.CommTradeTransactionDetailList[m].BiltyNo, po.CommTradeTransactionDetailList[m].BiltyDate, po.CommTradeTransactionDetailList[m].VehicleNo, po.CommTradeTransactionDetailList[m].VehicleQty, po.CommTradeTransactionDetailList[m].VehicleGrossWeight, po.CommTradeTransactionDetailList[m].ItemId, po.CommTradeTransactionDetailList[m].ItemCode, po.CommTradeTransactionDetailList[m].CropYearId, po.CommTradeTransactionDetailList[m].PackingTypeId, po.CommTradeTransactionDetailList[m].UomId, po.CommTradeTransactionDetailList[m].PackUom, po.CommTradeTransactionDetailList[m].Qty, po.CommTradeTransactionDetailList[m].CustomerId, po.CommTradeTransactionDetailList[m].CusGrossWeight, po.CommTradeTransactionDetailList[m].CusPckMtrlWeight, po.CommTradeTransactionDetailList[m].CusAdLsWt, po.CommTradeTransactionDetailList[m].CusNetWeight, po.CommTradeTransactionDetailList[m].CusRate, po.CommTradeTransactionDetailList[m].CusAmount, po.CommTradeTransactionDetailList[m].CustFcyAmount, po.CommTradeTransactionDetailList[m].SupplierId, po.CommTradeTransactionDetailList[m].SupGrossWeight, po.CommTradeTransactionDetailList[m].SupPckMtrlWeight, po.CommTradeTransactionDetailList[m].SupAdLsWt, po.CommTradeTransactionDetailList[m].SupNetWeight, po.CommTradeTransactionDetailList[m].SupRate, po.CommTradeTransactionDetailList[m].SupAmount, po.CommTradeTransactionDetailList[m].SupFcyAmount, po.CommTradeTransactionDetailList[m].ProductTypeId, po.CommTradeTransactionDetailList[m].BiltyFreight);
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				grdDetailsetting();
			}
			dtPackingMaterial.Rows.Clear();
			if (po.CommTradeTransactionPmOtherList.Count > 0)
			{
				for (int l = 0; l < po.CommTradeTransactionPmOtherList.Count; l++)
				{
					dtPackingMaterial.Rows.Add(po.CommTradeTransactionPmOtherList[l].Id, po.CommTradeTransactionPmOtherList[l].WareHouseId, po.CommTradeTransactionPmOtherList[l].StockTypeId, po.CommTradeTransactionPmOtherList[l].LoadingDeliveryId, po.CommTradeTransactionPmOtherList[l].LoadingDeliveryPmOtherId, po.CommTradeTransactionPmOtherList[l].PackingMaterialId, po.CommTradeTransactionPmOtherList[l].ItemId, po.CommTradeTransactionPmOtherList[l].ItemSize, po.CommTradeTransactionPmOtherList[l].ItemWeight, po.CommTradeTransactionPmOtherList[l].Qty, po.CommTradeTransactionPmOtherList[l].Rate, po.CommTradeTransactionPmOtherList[l].Amount, po.CommTradeTransactionPmOtherList[l].CreditAccountId);
				}
			}
			dtVehicleDetail.Rows.Clear();
			if (po.CommTradeTransactionVehicleFreightDetailList.Count > 0)
			{
				for (int k = 0; k < po.CommTradeTransactionVehicleFreightDetailList.Count; k++)
				{
					dtVehicleDetail.Rows.Add(po.CommTradeTransactionVehicleFreightDetailList[k].Id, po.CommTradeTransactionVehicleFreightDetailList[k].LoadingDeliveryId, po.CommTradeTransactionVehicleFreightDetailList[k].CustomerId, po.CommTradeTransactionVehicleFreightDetailList[k].VehicleNo, po.CommTradeTransactionVehicleFreightDetailList[k].BiltyNo, po.CommTradeTransactionVehicleFreightDetailList[k].VehicleQty, po.CommTradeTransactionVehicleFreightDetailList[k].BiltyFreight, po.CommTradeTransactionVehicleFreightDetailList[k].NetPaid, po.CommTradeTransactionVehicleFreightDetailList[k].Remarks, po.CommTradeTransactionVehicleFreightDetailList[k].CreditAccountId);
				}
			}
			dtOtherRevenue.Rows.Clear();
			dtCustomerRevenueExpense.Rows.Clear();
			if (po.CommTradeTransactionRevExpList.Count > 0)
			{
				for (int j = 0; j < po.CommTradeTransactionRevExpList.Count; j++)
				{
					if (Conversion.ToInt((object)po.CommTradeTransactionRevExpList[j].TypeId) == 1)
					{
						dtOtherRevenue.Rows.Add(po.CommTradeTransactionRevExpList[j].Id, po.CommTradeTransactionRevExpList[j].LoadingDeliveryId, po.CommTradeTransactionRevExpList[j].LoadingDeliveryRevExpId, po.CommTradeTransactionRevExpList[j].AccountId, po.CommTradeTransactionRevExpList[j].Qty, po.CommTradeTransactionRevExpList[j].Rate, po.CommTradeTransactionRevExpList[j].DebitAmt, po.CommTradeTransactionRevExpList[j].CreditAmt, po.CommTradeTransactionRevExpList[j].SupplierCustomerId);
					}
					else if (Conversion.ToInt((object)po.CommTradeTransactionRevExpList[j].TypeId) == 2)
					{
						dtCustomerRevenueExpense.Rows.Add(po.CommTradeTransactionRevExpList[j].Id, po.CommTradeTransactionRevExpList[j].LoadingDeliveryId, po.CommTradeTransactionRevExpList[j].LoadingDeliveryRevExpId, po.CommTradeTransactionRevExpList[j].AccountId, po.CommTradeTransactionRevExpList[j].Qty, po.CommTradeTransactionRevExpList[j].Rate, po.CommTradeTransactionRevExpList[j].DebitAmt, po.CommTradeTransactionRevExpList[j].CreditAmt, po.CommTradeTransactionRevExpList[j].SupplierCustomerId);
					}
				}
			}
			dtCustomerPTDetail.Rows.Clear();
			dtSupplierPTDetail.Rows.Clear();
			if (po.CommTradeTransactionPaymentTermsDetailList.Count > 0)
			{
				for (int i = 0; i < po.CommTradeTransactionPaymentTermsDetailList.Count; i++)
				{
					if (Conversion.ToInt((object)po.CommTradeTransactionPaymentTermsDetailList[i].SupplierCustomerTypeId) == 1)
					{
						dtSupplierPTDetail.Rows.Add(po.CommTradeTransactionPaymentTermsDetailList[i].Id, po.CommTradeTransactionPaymentTermsDetailList[i].CommTradeLoadingDeliveryId, po.CommTradeTransactionPaymentTermsDetailList[i].LoadingDeliveryPaymentDetailId, po.CommTradeTransactionPaymentTermsDetailList[i].SupplierCustomerId, po.CommTradeTransactionPaymentTermsDetailList[i].PaymentTermId, po.CommTradeTransactionPaymentTermsDetailList[i].PrcntOfTotal, po.CommTradeTransactionPaymentTermsDetailList[i].Amount, po.CommTradeTransactionPaymentTermsDetailList[i].DueDays, po.CommTradeTransactionPaymentTermsDetailList[i].DueDate, po.CommTradeTransactionPaymentTermsDetailList[i].PaymentRemarks);
					}
				}
			}
			if (dtPackingMaterial.Rows.Count == 0)
			{
				AddRowPackingmaterial();
			}
			grdPackingMaterial.DataSource = dtPackingMaterial;
			grdPackingMaterial.RetrieveStructure();
			grdpackingsetting();
			if (dtOtherRevenue.Rows.Count == 0)
			{
				AddRowOtherRevenue();
			}
			grdrevenue.DataSource = dtOtherRevenue;
			grdrevenue.RetrieveStructure();
			grdrevenuesetting();
			grdCustomerRevenueExpense.DataSource = dtCustomerRevenueExpense;
			grdCustomerRevenueExpense.RetrieveStructure();
			grdCustomerRevenueExpensesetting();
			LoadInGridVehicleInfo(dtDetail);
			grdVehicleDetail.DataSource = dtVehicleDetail;
			grdVehicleDetail.RetrieveStructure();
			grdVehicleDetailsetting();
			if (dtSupplierPTDetail.Rows.Count == 0)
			{
				AddRowInPaymentTermSupplierGrid();
			}
			grdPaymentTermSupplier.DataSource = dtSupplierPTDetail;
			grdPaymentTermSupplier.RetrieveStructure();
			grdSettingsPaymentTermSupplier();
			AddExpenseAccountRowsOtherRevenue();
			AddExpenseAccountRowsforgrdCustomerRevenueExpense();
			TotalCommissionAmount();
			CustomerBillAmount();
			FillSummaryGrid();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			suppliercustomer();
			FillDifferenceAc();
			PaymentTerms();
			CurrencyFill();
			FillCommissionBrokeryAc();
			CommissionTypeFill();
			ConfigurationDefault();
			GridDtFill();
			if (HasMultiLanguageFeature)
			{
				MultiLanguagefill();
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
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			((Control)(object)cmbCustomername).Text = string.Empty;
			txtbillno.Text = string.Empty;
			((Control)(object)cmbpayterm).Text = string.Empty;
			((Control)(object)cmbdeliveryterm).Text = string.Empty;
			DocDate.Value = DateTime.Now;
			txtduedays.Text = string.Empty;
			txtmanualref.Text = string.Empty;
			txtVehicleNos.Text = string.Empty;
			txtBiltyNos.Text = string.Empty;
			txtBiltyDates.Text = string.Empty;
			txtcustbillamnt.Text = string.Empty;
			txtfcyamount.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			txtbrokeryamount.Text = string.Empty;
			txtbrokeryrate.Text = string.Empty;
			((Control)(object)cmbsubbrokeryacc).Text = string.Empty;
			txtsubbrokeryamnt.Text = string.Empty;
			txtsubbrokeryrate.Text = string.Empty;
			LoadDeliveryDetailIds = string.Empty;
			LoadDeliveryMainIds = string.Empty;
			cmbCustomername.Focus();
			((Control)(object)cmbCustomername).Enabled = true;
			btnSave.Enabled = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			grdDetail.DataSource = null;
			dtDetail.Clear();
			btnSave.Visible = true;
			dtDetail.Rows.Clear();
			dtPackingMaterial.Rows.Clear();
			dtOtherRevenue.Rows.Clear();
			dtCustomerRevenueExpense.Rows.Clear();
			dtsummaryBySupplier.Rows.Clear();
			dtCustomerPTDetail.Rows.Clear();
			dtSupplierPTDetail.Rows.Clear();
			dtVehicleDetail.Rows.Clear();
			grdDetail.DataSource = dtDetail;
			grdsummary.DataSource = dtsummaryBySupplier;
			grdVehicleDetail.DataSource = dtVehicleDetail;
			AddExpenseAccountRowsOtherRevenue();
			grdrevenue.RetrieveStructure();
			grdrevenuesetting();
			grdCustomerRevenueExpense.DataSource = dtCustomerRevenueExpense;
			grdCustomerRevenueExpense.RetrieveStructure();
			grdCustomerRevenueExpensesetting();
			AddRowPackingmaterial();
			grdPackingMaterial.RetrieveStructure();
			grdpackingsetting();
			AddRowInPaymentTermCustomerGrid();
			grdPaymentTermCustomer.RetrieveStructure();
			grdSettingsPaymentTermCustomer();
			AddRowInPaymentTermSupplierGrid();
			grdPaymentTermSupplier.RetrieveStructure();
			grdSettingsPaymentTermSupplier();
			DocumentNoFill();
			txtcustbillamnt.Text = string.Empty;
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

	private void CommisionBill_FormClosed(object sender, FormClosedEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdrevenue).Name);
			if (fs != null)
			{
				grdrevenue.LoadLayoutFile((Stream)fs);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryDetail).Name);
			if (fs != null)
			{
				grdHistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPackingMaterial).Name);
			if (fs != null)
			{
				grdPackingMaterial.LoadLayoutFile((Stream)fs);
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
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommisionBill_KeyDown(object sender, KeyEventArgs e)
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
					cmbCustomername.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
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
					cmbCustomername.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdDetail).Focused)
					{
						((Control)(object)grdPackingMaterial).Focus();
					}
					else if (((Control)(object)grdPackingMaterial).Focused)
					{
						((Control)(object)grdrevenue).Focus();
					}
					else
					{
						((Control)(object)grdDetail).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbCustomername.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (HasMultiLanguageFeature)
				{
					if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1) && formright.DoHavePrintRights)
					{
						BtnPrint_Click(null, null);
					}
					if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2) && formright.DoHavePrintRights && btnMultiLangPrint.Visible)
					{
						btnMultiLangPrint_Click(null, null);
					}
					if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3) && formright.DoHavePrintRights)
					{
						btnPrintSupplier1704_01_Click(null, null);
					}
					if (e.Alt && (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4) && formright.DoHavePrintRights)
					{
						btnVoucher_Click(null, null);
					}
				}
				else
				{
					if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1) && formright.DoHavePrintRights)
					{
						BtnPrint_Click(null, null);
					}
					if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2) && formright.DoHavePrintRights)
					{
						btnVoucher_Click(null, null);
					}
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.L && HasMultiLanguageFeature)
				{
					CmbMultiLanguage.Focus();
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					BtnLoadFromLoadingDelivery_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistoryDetail).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistoryDetail).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Customer Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+L", "For Load Form Loading Delivery");
			if (HasMultiLanguageFeature)
			{
				dt.Rows.Add("Ctrl+Shift+L ", "For Focus On MultiLanguageCombo in Form Tab");
				dt.Rows.Add("Alt+1", "For 1704-Print");
				dt.Rows.Add("Alt+2", "For 1704-Urdu Print");
				dt.Rows.Add("Alt+3", "For 1704_01-Urdu Print");
				dt.Rows.Add("Alt+4", "For Print Voucher 103");
			}
			else
			{
				dt.Rows.Add("Alt+1", "For Print Slip 1704");
				dt.Rows.Add("Alt+2", "For Print Voucher 103");
			}
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow item2 = grdDetail.CurrentRow;
				if (item2 != null && (int)item2.RowType == 0 && grdDetail.CurrentColumn != null)
				{
					if (grdDetail.CurrentColumn.Key == "Delete")
					{
						GridEXRow r2 = grdDetail.CurrentRow;
						if (Conversion.ToInt(r2.Cells["Id"].Value) > 0)
						{
							if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
							{
								return;
							}
							CommTradeTransactionDetail vd2 = new CommTradeTransactionDetail();
							vd2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
							vd2.CommTradeOrderId = Conversion.ToInt(r2.Cells["OrderId"].Value);
							vd2.CommTradeOrderDetailId = Conversion.ToInt(r2.Cells["OrderDetailId"].Value);
							vd2.CommTradeLoadingDeliveryId = Conversion.ToInt(r2.Cells["LoadingDeliveryId"].Value);
							vd2.CommTradeLoadingDeliveryDetailId = Conversion.ToInt(r2.Cells["LoadingDeliveryDetailId"].Value);
							vd2.dDocDate = Conversion.ToDateTime((object)DocDate.Value);
							vd2.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
							vd2.CusDeliveryTerm = Conversion.ToString((object)((Control)(object)cmbdeliveryterm).Text);
							vd2.CusDueDays = Conversion.ToInt((object)txtduedays.Text);
							vd2.SupDueDays = Conversion.ToInt((object)txtduedays.Text);
							vd2.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
							vd2.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
							vd2.BiltyNo = Conversion.ToString(r2.Cells["BiltyNo"].Value);
							vd2.BiltyDate = Conversion.ToDateTime(r2.Cells["BiltyDate"].Value);
							vd2.VehicleNo = Conversion.ToString(r2.Cells["VehicleNo"].Value);
							vd2.VehicleQty = Conversion.ToDecimal(r2.Cells["VehicleQty"].Value);
							vd2.VehicleGrossWeight = Conversion.ToDecimal(r2.Cells["VehicleGrossWeight"].Value);
							vd2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
							vd2.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
							vd2.PackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
							vd2.UomId = Conversion.ToInt(r2.Cells["UomId"].Value);
							vd2.Qty = Conversion.ToDecimal(r2.Cells["ItemQty"].Value);
							vd2.SupplierId = Conversion.ToInt(r2.Cells["SupplierAccountId"].Value);
							vd2.SupGrossWeight = Conversion.ToDecimal(r2.Cells["SupGrossWeight"].Value);
							vd2.SupPckMtrlWeight = Conversion.ToDecimal(r2.Cells["SupPMWeight"].Value);
							vd2.SupAdLsWt = Conversion.ToDecimal(r2.Cells["SupAdLsWeight"].Value);
							vd2.SupNetWeight = Conversion.ToDecimal(r2.Cells["SupNetWeight"].Value);
							vd2.SupRate = Conversion.ToDecimal(r2.Cells["SupRate"].Value);
							vd2.SupAmount = Conversion.ToDecimal(r2.Cells["SupAmount"].Value);
							vd2.SupFcyAmount = Conversion.ToDecimal(r2.Cells["SupFcyAmount"].Value);
							vd2.CustomerId = Conversion.ToInt(cmbCustomername.Value);
							vd2.CusGrossWeight = Conversion.ToDecimal(r2.Cells["CusGrossWeight"].Value);
							vd2.CusPckMtrlWeight = Conversion.ToDecimal(r2.Cells["CusPMWeight"].Value);
							vd2.CusAdLsWt = Conversion.ToDecimal(r2.Cells["CusAdLsWeight"].Value);
							vd2.CusNetWeight = Conversion.ToDecimal(r2.Cells["CusNetWeight"].Value);
							vd2.CusRate = Conversion.ToDecimal(r2.Cells["CusRate"].Value);
							vd2.CusAmount = Conversion.ToDecimal(r2.Cells["CusAmount"].Value);
							vd2.CustFcyAmount = Conversion.ToDecimal(r2.Cells["CusFcyAmount"].Value);
							vd2.ActionTypeId = 3;
							lstRemoveRecord.Add(vd2);
							dtDetail.Rows[r2.RowIndex].Delete();
							if (r2 != null && (int)r2.RowType == 0)
							{
								r2.Delete();
							}
							grdDetail.UpdateData();
						}
						else if (r2 != null && (int)r2.RowType == 0)
						{
							r2.Delete();
						}
					}
					GetIds();
					txtexchangerate_TextChanged(null, null);
					TotalCommissionAmount();
					CustomerBillAmount();
					FillSummaryGrid();
				}
			}
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			GridEXRow r = grdDetail.CurrentRow;
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				CommTradeTransactionDetail vd = new CommTradeTransactionDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.CommTradeOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
				vd.CommTradeOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
				vd.CommTradeLoadingDeliveryId = Conversion.ToInt(r.Cells["LoadingDeliveryId"].Value);
				vd.CommTradeLoadingDeliveryDetailId = Conversion.ToInt(r.Cells["LoadingDeliveryDetailId"].Value);
				vd.dDocDate = Conversion.ToDateTime((object)DocDate.Value);
				vd.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
				vd.CusDeliveryTerm = Conversion.ToString((object)((Control)(object)cmbdeliveryterm).Text);
				vd.CusDueDays = Conversion.ToInt((object)txtduedays.Text);
				vd.SupDueDays = Conversion.ToInt((object)txtduedays.Text);
				vd.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
				vd.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
				vd.BiltyNo = Conversion.ToString(r.Cells["BiltyNo"].Value);
				vd.BiltyDate = Conversion.ToDateTime(r.Cells["BiltyDate"].Value);
				vd.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
				vd.VehicleQty = Conversion.ToDecimal(r.Cells["VehicleQty"].Value);
				vd.VehicleGrossWeight = Conversion.ToDecimal(r.Cells["VehicleGrossWeight"].Value);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				vd.UomId = Conversion.ToInt(r.Cells["UomId"].Value);
				vd.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
				vd.SupplierId = Conversion.ToInt(r.Cells["SupplierAccountId"].Value);
				vd.SupGrossWeight = Conversion.ToDecimal(r.Cells["SupGrossWeight"].Value);
				vd.SupPckMtrlWeight = Conversion.ToDecimal(r.Cells["SupPMWeight"].Value);
				vd.SupAdLsWt = Conversion.ToDecimal(r.Cells["SupAdLsWeight"].Value);
				vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
				vd.SupRate = Conversion.ToDecimal(r.Cells["SupRate"].Value);
				vd.SupAmount = Conversion.ToDecimal(r.Cells["SupAmount"].Value);
				vd.SupFcyAmount = Conversion.ToDecimal(r.Cells["SupFcyAmount"].Value);
				vd.CustomerId = Conversion.ToInt(cmbCustomername.Value);
				vd.CusGrossWeight = Conversion.ToDecimal(r.Cells["CusGrossWeight"].Value);
				vd.CusPckMtrlWeight = Conversion.ToDecimal(r.Cells["CusPMWeight"].Value);
				vd.CusAdLsWt = Conversion.ToDecimal(r.Cells["CusAdLsWeight"].Value);
				vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
				vd.CusRate = Conversion.ToDecimal(r.Cells["CusRate"].Value);
				vd.CusAmount = Conversion.ToDecimal(r.Cells["CusAmount"].Value);
				vd.CustFcyAmount = Conversion.ToDecimal(r.Cells["CusFcyAmount"].Value);
				vd.ActionTypeId = 3;
				lstRemoveRecord.Add(vd);
				dtDetail.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grdDetail.UpdateData();
			}
			else if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
			GetIds();
			txtexchangerate_TextChanged(null, null);
			TotalCommissionAmount();
			CustomerBillAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPackingMaterial.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPackingMaterial.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(item.Cells["StockTypeId"].Value) == 1)
					{
						MessageBox.Show("you can not delete row With StockType company");
						return;
					}
					item.Delete();
					if (dtPackingMaterial.Rows.Count == 0)
					{
						AddRowPackingmaterial();
					}
				}
				if (grdPackingMaterial.CurrentColumn.Key == "Add")
				{
					AddRowPackingmaterial();
				}
				CalculateUpdateFcyInDetailGrid();
				TotalCommissionAmount();
				CustomerBillAmount();
				FillSummaryGrid();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(item.Cells["StockTypeId"].Value) == 1)
				{
					MessageBox.Show("you can not delete row With StockType company");
					return;
				}
				item.Delete();
				if (dtPackingMaterial.Rows.Count == 0)
				{
					AddRowPackingmaterial();
				}
				CalculateUpdateFcyInDetailGrid();
				TotalCommissionAmount();
				CustomerBillAmount();
				FillSummaryGrid();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowPackingmaterial();
				CalculateUpdateFcyInDetailGrid();
				TotalCommissionAmount();
				CustomerBillAmount();
				FillSummaryGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				if (grdPackingMaterial.CurrentColumn != null && grdPackingMaterial.CurrentColumn.Key == "StockTypeId")
				{
					GridDtFill();
					GrdPopUp grdPopUp = new GrdPopUp(dtStockType, "Id", "StockType");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Conversion.ToString((object)grdPopUp.ReturnName);
					item.Cells["StockTypeId"].Value = Id;
				}
				CommonServices.GridAutoAdjustmentNew(grdPackingMaterial);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdrevenue_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdrevenue.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdrevenue.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdrevenue.CurrentColumn.Key == "Delete")
				{
					dtOtherRevenue.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtOtherRevenue.Rows.Count == 0)
					{
						AddRowOtherRevenue();
					}
				}
				if (grdrevenue.CurrentColumn.Key == "Add")
				{
					AddRowOtherRevenue();
				}
				FillSummaryGrid();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtOtherRevenue.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtOtherRevenue.Rows.Count == 0)
				{
					AddRowOtherRevenue();
				}
				FillSummaryGrid();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowOtherRevenue();
				FillSummaryGrid();
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
			int Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					DocDate.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "Print" && formright.DoHavePrintRights)
				{
					GenerateSlip(Id);
				}
				if (grdHistory.CurrentColumn.Key == "UrduPrint" && formright.DoHavePrintRights)
				{
					GenerateMultiLangPrint(Id);
				}
				if (grdHistory.CurrentColumn.Key == "Voucher" && formright.DoHavePrintRights)
				{
					GenerateVoucherSlip(Id);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)Id.ToString()), 1704);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					DocDate.Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				GenerateSlip(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadFromLoadingDelivery_Click(object sender, EventArgs e)
	{
		try
		{
			LoadFromLoadingDelivery = new frmLoadFromLoadingDelivery(UserAccount);
			if (LoadDeliveryDetailIds != string.Empty)
			{
				LoadFromLoadingDelivery.orderDetailIds = LoadDeliveryDetailIds;
				LoadFromLoadingDelivery.orderMainIds = LoadDeliveryMainIds;
			}
			LoadFromLoadingDelivery.ShowDialog();
			LoadFromLoadingDelivery.BringToFront();
			LoadInGridDetail();
			LoadInGridRevenueExp();
			LoadInGridPmOther();
			LoadInGridSupplierPaymentExp();
			FillSummaryGrid();
			AddExpenseAccountRowsforgrdCustomerRevenueExpense();
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		try
		{
			string GdnIds = "";
			if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNS.Rows[0][0].ToString()) == 0)
			{
				return;
			}
			for (int j = 0; j < LoadFromLoadingDelivery.dtGDNS.Rows.Count; j++)
			{
				if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNS.Rows[j][0].ToString()) != 0)
				{
					GdnIds = GdnIds + "," + Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNS.Rows[j][0].ToString());
				}
			}
			DataTable dtLoaderData = CommTradeLoadingDelivery.PendingCommTradeLoadingDeliveryForBill(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1702,
				GdnIds = GdnIds
			});
			if (dtLoaderData.Rows.Count > 0)
			{
				cmbCustomername.Value = Conversion.ToInt(dtLoaderData.Rows[0]["CustomerId"]);
				((Control)(object)cmbCustomername).Enabled = false;
				cmbsubbrokeryacc.Value = Conversion.ToInt(dtLoaderData.Rows[0]["BrokerId"]);
				txtmanualref.Text = Conversion.ToString(dtLoaderData.Rows[0]["ManualRefNo"]);
				string VehicleNos = "";
				string BiltyNos = "";
				string BiltyDates = "";
				bool flag = false;
				GridEXRow[] rows;
				for (int i = 0; i < dtLoaderData.Rows.Count; i++)
				{
					if (grdDetail.GetRows().Length != 0)
					{
						rows = grdDetail.GetRows();
						foreach (GridEXRow item in rows)
						{
							if (Conversion.ToInt(cmbCustomername.Value) != Conversion.ToInt(dtLoaderData.Rows[i]["CustomerId"]))
							{
								MessageBox.Show("Data against another Customer Already Exist in Detail");
								return;
							}
							if (Conversion.ToInt(item.Cells["LoadingDeliveryDetailId"].Value) == Conversion.ToInt(dtLoaderData.Rows[i]["DetailId"]))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtDetail.Rows.Add(0, Conversion.ToInt(dtLoaderData.Rows[i]["OrderId"]), Conversion.ToInt(dtLoaderData.Rows[i]["OrderDetailId"]), dtLoaderData.Rows[i]["OrderNo"], Conversion.ToInt(dtLoaderData.Rows[i]["Id"]), Conversion.ToInt(dtLoaderData.Rows[i]["DetailId"]), Conversion.ToInt(dtLoaderData.Rows[i]["DocNo"]), Conversion.ToString(dtLoaderData.Rows[i]["BiltyNo"]), Conversion.ToDateTime(dtLoaderData.Rows[i]["BiltyDate"]), Conversion.ToString(dtLoaderData.Rows[i]["VehicleNo"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["VehicleQty"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["VehicleGrossWeight"]), Conversion.ToInt(dtLoaderData.Rows[i]["ItemId"]), dtLoaderData.Rows[i]["ItemCode"], Conversion.ToInt(dtLoaderData.Rows[i]["CropYearId"]), Conversion.ToInt(dtLoaderData.Rows[i]["PackingTypeId"]), Conversion.ToInt(dtLoaderData.Rows[i]["UomId"]), Conversion.ToString(dtLoaderData.Rows[i]["PackUom"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Qty"]), Conversion.ToInt(dtLoaderData.Rows[i]["CustomerId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusGrossWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusPckMtrlWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusAdLsWt"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusNetWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusRate"]), 0, 0, Conversion.ToInt(dtLoaderData.Rows[i]["SupplierId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupGrossWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupPckMtrlWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupAdLsWt"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupNetWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupRate"]), 0, 0, Conversion.ToInt(dtLoaderData.Rows[i]["ProductTypeId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["BiltyFreight"]));
						flag = false;
					}
					VehicleNos = VehicleNos + "," + Conversion.ToString(dtLoaderData.Rows[i]["VehicleNo"]);
					BiltyNos = BiltyNos + "," + Conversion.ToString(dtLoaderData.Rows[i]["BiltyNo"]);
					BiltyDates = BiltyDates + "," + Conversion.ToString((object)Conversion.ToDateTime(dtLoaderData.Rows[i]["BiltyDate"]).ToShortDateString());
				}
				IEnumerable<string> DistinctVehicleNos = (from V in VehicleNos.Split(',')
					where !string.IsNullOrEmpty(V)
					select V).Distinct();
				IEnumerable<string> DistinctBiltyNos = (from B in BiltyNos.Split(',')
					where !string.IsNullOrEmpty(B)
					select B).Distinct();
				IEnumerable<string> DistinctBiltyDates = (from BT in BiltyDates.Split(',')
					where !string.IsNullOrEmpty(BT)
					select BT).Distinct();
				txtVehicleNos.Text = string.Join(",", DistinctVehicleNos);
				txtBiltyNos.Text = string.Join(",", DistinctBiltyNos);
				txtBiltyDates.Text = string.Join(",", DistinctBiltyDates);
				txtVehicleNos.Enabled = false;
				txtBiltyNos.Enabled = false;
				txtBiltyDates.Enabled = false;
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				grdDetailsetting();
				rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					string ColumnName = "";
					ColumnName = ((!(Conversion.ToDecimal(r.Cells["CusRate"].Value) > 0m)) ? "SupRate" : "CusRate");
					CalcluationInGrid(ColumnName, r);
				}
				LoadInGridVehicleInfo(dtDetail);
				GetIds();
			}
			else
			{
				Reset();
			}
			GdnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridVehicleInfo(DataTable dtLoaderData)
	{
		try
		{
			if (dtLoaderData.Rows.Count > 0)
			{
				int i;
				for (i = 0; i < dtLoaderData.Rows.Count; i++)
				{
					decimal TotalDetailQty = (from row in dtDetail.AsEnumerable()
						where row.Field<string>("VehicleNo") == Conversion.ToString(dtLoaderData.Rows[i]["VehicleNo"])
						select row).Sum((DataRow row) => row.Field<decimal>("ItemQty"));
					if (!dtVehicleDetail.AsEnumerable().Any((DataRow row) => row.Field<string>("VehicleNo") == Conversion.ToString(dtLoaderData.Rows[i]["VehicleNo"])))
					{
						dtVehicleDetail.Rows.Add(0, Conversion.ToInt(dtLoaderData.Rows[i]["Id"]), Conversion.ToInt(dtLoaderData.Rows[i]["CustomerAccountId"]), Conversion.ToString(dtLoaderData.Rows[i]["VehicleNo"]), Conversion.ToString(dtLoaderData.Rows[i]["BiltyNo"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["VehicleQty"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["BiltyFreight"]), (Conversion.ToDecimal(dtLoaderData.Rows[i]["VehicleQty"]) > 0m) ? (Conversion.ToDecimal(dtLoaderData.Rows[i]["BiltyFreight"]) / Conversion.ToDecimal(dtLoaderData.Rows[i]["VehicleQty"]) * TotalDetailQty) : 0m, "", 0);
					}
				}
				grdVehicleDetail.DataSource = dtVehicleDetail;
				grdVehicleDetail.RetrieveStructure();
				grdVehicleDetailsetting();
			}
			if (grdVehicleDetail.GetDataRows().Count() <= 0)
			{
				return;
			}
			GridEXRow[] rows = grdVehicleDetail.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				if (Conversion.ToInt(row2.Cells["Id"].Value) > 0)
				{
					row2.BeginEdit();
					row2.IsChecked = Convert.ToBoolean(value: true);
					row2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridRevenueExp()
	{
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string GdnIds = "";
			if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[0][0].ToString()) != 0)
			{
				for (int j = 0; j < LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows.Count; j++)
				{
					if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[j][0].ToString()) != 0)
					{
						GdnIds = GdnIds + "," + Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[j][0].ToString());
					}
				}
				IEnumerable<string> DistinctGdnIds = (from V in GdnIds.Split(',')
					where !string.IsNullOrEmpty(V)
					select V).Distinct();
				DataTable dtLoaderData = CommTradeLoadingDelivery.getCustomerExpensesFromLoading(string.Join(",", DistinctGdnIds), Conversion.ToInt(cmbCustomername.Value));
				if (dtLoaderData.Rows.Count > 0)
				{
					GridEXRow rr = grdrevenue.GetRow(0);
					if (grdrevenue.GetRows().Length != 0 && rr != null && (int)rr.RowType == 0 && Conversion.ToInt(rr.Cells["SupplierAccountId"].Value) == 0 && Conversion.ToDecimal(rr.Cells["AmountDr"].Value) == 0m && Conversion.ToDecimal(rr.Cells["Qty"].Value) == 0m && Conversion.ToDecimal(rr.Cells["Rate"].Value) == 0m && Conversion.ToDecimal(rr.Cells["AmountCr"].Value) == 0m)
					{
						dtOtherRevenue.Clear();
					}
					decimal TotalDetailQty = dtDetail.AsEnumerable().Sum((DataRow row) => row.Field<decimal>("ItemQty"));
					bool flag = false;
					HashSet<int> ExpenseForSupplierEmpty = new HashSet<int>(from row in CommonServices.StaticColumnsService("RevenueExpenseIdsForSupplierEmpty").AsEnumerable()
						select row.Field<int>("Id"));
					GridEXRow[] rows;
					for (int i = 0; i < dtLoaderData.Rows.Count; i++)
					{
						TotalDetailQty = Math.Round(TotalDetailQty, 2);
						if (grdrevenue.GetRows().Length != 0)
						{
							rows = grdrevenue.GetRows();
							foreach (GridEXRow item in rows)
							{
								if (Conversion.ToInt(item.Cells["LoadingDeliveryRevExpId"].Value) > 0 && Conversion.ToInt(item.Cells["LoadingDeliveryRevExpId"].Value) == Conversion.ToInt(dtLoaderData.Rows[i]["Id"]))
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							if (Conversion.ToInt(dtLoaderData.Rows[i]["SupplierCustomerId"]) == Conversion.ToInt(cmbCustomername.Value))
							{
								dtOtherRevenue.Rows.Add(0, Conversion.ToInt(dtLoaderData.Rows[i]["CommTradeLoadingDeliveryId"]), Conversion.ToInt(dtLoaderData.Rows[i]["Id"]), Conversion.ToInt(dtLoaderData.Rows[i]["AccountId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Qty"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Rate"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["DebitAmt"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CreditAmt"]), (!ExpenseForSupplierEmpty.Contains(Conversion.ToInt(dtLoaderData.Rows[i]["AccountId"]))) ? Conversion.ToInt(dtLoaderData.Rows[i]["SupplierCustomerIdDr"]) : 0);
							}
							else if (Conversion.ToInt(dtLoaderData.Rows[i]["SupplierCustomerIdDr"]) == Conversion.ToInt(cmbCustomername.Value))
							{
								dtOtherRevenue.Rows.Add(0, Conversion.ToInt(dtLoaderData.Rows[i]["CommTradeLoadingDeliveryId"]), Conversion.ToInt(dtLoaderData.Rows[i]["Id"]), Conversion.ToInt(dtLoaderData.Rows[i]["AccountId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Qty"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Rate"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["DebitAmt"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CreditAmt"]), (!ExpenseForSupplierEmpty.Contains(Conversion.ToInt(dtLoaderData.Rows[i]["AccountId"]))) ? Conversion.ToInt(dtLoaderData.Rows[i]["SupplierCustomerId"]) : 0);
							}
							flag = false;
						}
					}
					grdrevenue.DataSource = dtOtherRevenue;
					grdrevenue.RetrieveStructure();
					grdrevenuesetting();
					rows = grdrevenue.GetRows();
					foreach (GridEXRow r in rows)
					{
						string ColumnName = "Qty";
						RevenueCalcluationInGrid(ColumnName, r);
					}
				}
			}
			GdnIds = "";
			AddExpenseAccountRowsOtherRevenue();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridSupplierPaymentExp()
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string GdnIds = "";
			if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[0][0].ToString()) != 0)
			{
				for (int j = 0; j < LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows.Count; j++)
				{
					if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[j][0].ToString()) != 0)
					{
						GdnIds = GdnIds + "," + Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[j][0].ToString());
					}
				}
				IEnumerable<string> DistinctGdnIds = (from V in GdnIds.Split(',')
					where !string.IsNullOrEmpty(V)
					select V).Distinct();
				string Ids = string.Join(",", DistinctGdnIds);
				DataTable dtLoaderData = CommTradeLoadingDelivery.CommTradeLoadingDeliveryPaymentTermsDetailByHeaderIds(Ids);
				if (dtLoaderData.Rows.Count > 0)
				{
					GridEXRow rr = grdPaymentTermSupplier.GetRow(0);
					if (grdPaymentTermSupplier.GetRows().Length != 0 && rr != null && (int)rr.RowType == 0 && Conversion.ToInt(rr.Cells["SupplierId"].Value) == 0 && Conversion.ToDecimal(rr.Cells["PaymentTermId"].Value) == 0m && Conversion.ToDecimal(rr.Cells["%ofTotal"].Value) == 0m && Conversion.ToDecimal(rr.Cells["Amount"].Value) == 0m)
					{
						dtSupplierPTDetail.Clear();
					}
					bool flag = false;
					int i;
					GridEXRow[] rows;
					for (i = 0; i < dtLoaderData.Rows.Count; i++)
					{
						if (grdPaymentTermSupplier.GetRows().Length != 0)
						{
							rows = grdPaymentTermSupplier.GetRows();
							for (int k = 0; k < rows.Length; k++)
							{
								if (Conversion.ToInt(rows[k].Cells["LoadingDeliveryPaymentDetailId"].Value) == Conversion.ToInt(dtLoaderData.Rows[i]["Id"]))
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							if (dtDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierAccountId") == Conversion.ToInt(dtLoaderData.Rows[i]["SupplierCustomerId"])))
							{
								dtSupplierPTDetail.Rows.Add(0, Conversion.ToInt(dtLoaderData.Rows[i]["CommTradeLoadingDeliveryId"]), Conversion.ToInt(dtLoaderData.Rows[i]["Id"]), Conversion.ToInt(dtLoaderData.Rows[i]["SupplierCustomerId"]), Conversion.ToInt(dtLoaderData.Rows[i]["PaymentTermId"]), Conversion.ToDouble(dtLoaderData.Rows[i]["PrcntOfTotal"]), 0, Conversion.ToInt(dtLoaderData.Rows[i]["DueDays"]), Conversion.ToDateTime(dtLoaderData.Rows[i]["DueDate"]), Conversion.ToString(dtLoaderData.Rows[i]["PaymentRemarks"]));
							}
							flag = false;
						}
					}
					grdPaymentTermSupplier.DataSource = dtSupplierPTDetail;
					grdPaymentTermSupplier.RetrieveStructure();
					grdSettingsPaymentTermSupplier();
					rows = grdPaymentTermSupplier.GetRows();
					foreach (GridEXRow r in rows)
					{
						string ColumnName = "DueDays,%ofTotal";
						SupplierPTCalcluationInGrid(ColumnName, r);
					}
				}
			}
			GdnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridPmOther()
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string GdnIds = "";
			if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[0][0].ToString()) != 0)
			{
				for (int j = 0; j < LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows.Count; j++)
				{
					if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[j][0].ToString()) != 0)
					{
						GdnIds = GdnIds + "," + Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNHeaderIdS.Rows[j][0].ToString());
					}
				}
				IEnumerable<string> DistinctGdnIds = (from V in GdnIds.Split(',')
					where !string.IsNullOrEmpty(V)
					select V).Distinct();
				DataTable dtLoaderData = CommTradeLoadingDelivery.CommTradeLoadingDeliveryPmOtherHeaderIds(string.Join(",", DistinctGdnIds));
				if (dtLoaderData.Rows.Count > 0)
				{
					GridEXRow rr = grdPackingMaterial.GetRow(0);
					if (grdPackingMaterial.GetRows().Length != 0 && rr != null && (int)rr.RowType == 0 && Conversion.ToInt(rr.Cells["ItemId"].Value) == 0 && Conversion.ToDecimal(rr.Cells["Amount"].Value) == 0m)
					{
						dtPackingMaterial.Clear();
					}
					dtDetail.AsEnumerable().Sum((DataRow row) => row.Field<decimal>("ItemQty"));
					bool flag = false;
					for (int i = 0; i < dtLoaderData.Rows.Count; i++)
					{
						if (grdPackingMaterial.GetRows().Length != 0)
						{
							GridEXRow[] rows = grdPackingMaterial.GetRows();
							for (int k = 0; k < rows.Length; k++)
							{
								if (Conversion.ToInt(rows[k].Cells["LoadingDeliveryPmOtherId"].Value) == Conversion.ToInt(dtLoaderData.Rows[i]["Id"]))
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							if (Conversion.ToInt(dtLoaderData.Rows[i]["DebitAccountId"]) > 0 && Conversion.ToInt(cmbCustomername.Value) == Conversion.ToInt(dtLoaderData.Rows[i]["DebitAccountId"]))
							{
								dtPackingMaterial.Rows.Add(0, Conversion.ToInt(dtLoaderData.Rows[i]["WareHouseId"]), Conversion.ToInt(dtLoaderData.Rows[i]["StockTypeId"]), Conversion.ToInt(dtLoaderData.Rows[i]["CommTradeLoadingDeliveryId"]), Conversion.ToInt(dtLoaderData.Rows[i]["Id"]), Conversion.ToInt(dtLoaderData.Rows[i]["PackingTypeId"]), Conversion.ToInt(dtLoaderData.Rows[i]["ItemId"]), Conversion.ToString(dtLoaderData.Rows[i]["ItemSize"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["ItemWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Qty"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Rate"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Amount"]), Conversion.ToInt(dtLoaderData.Rows[i]["CreditAccountId"]));
							}
							flag = false;
						}
					}
					grdPackingMaterial.DataSource = dtPackingMaterial;
					grdPackingMaterial.RetrieveStructure();
					grdpackingsetting();
				}
			}
			GdnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetIds()
	{
		try
		{
			LoadDeliveryDetailIds = "";
			LoadDeliveryMainIds = "";
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				LoadDeliveryDetailIds = LoadDeliveryDetailIds + "," + Conversion.ToInt(item.Cells["LoadingDeliveryDetailId"].Value);
				LoadDeliveryMainIds = LoadDeliveryMainIds + "," + Conversion.ToInt(item.Cells["LoadingDeliveryId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateUpdateFcyInDetailGrid()
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdDetail.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtexchangerate.Text) > 0m)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Cusamount = Conversion.ToDouble(obj.Cells["CusAmount"].Value) / Conversion.ToDouble((object)txtexchangerate.Text);
					obj.Cells["CusFcyAmount"].Value = Math.Round(Cusamount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					double Supamount = Conversion.ToDouble(obj.Cells["SupAmount"].Value) / Conversion.ToDouble((object)txtexchangerate.Text);
					obj.Cells["SupFcyAmount"].Value = Math.Round(Supamount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					obj.EndEdit();
				}
			}
			else if (grdDetail.CurrentRow != null && (int)grdDetail.CurrentRow.RowType == 0 && grdDetail.CurrentRow.Cells.Count > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["CusFcyAmount"].Value = 0;
					obj2.Cells["SupFcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalCommissionAmount()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Expected O, but got Unknown
		//IL_0db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3b: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCustomername.Value) != 0 && Conversion.ToInt(cmbcommtype.Value) != 0)
			{
				bool CommExist = false;
				bool BrokeryExist = false;
				if (grdCustomerRevenueExpense.RowCount > 0)
				{
					GridEXRow[] rows = grdCustomerRevenueExpense.GetRows();
					foreach (GridEXRow obj in rows)
					{
						if (Conversion.ToInt(obj.Cells["OtherRevenueId"].Value) == 17)
						{
							CommExist = true;
						}
						if (Conversion.ToInt(obj.Cells["OtherRevenueId"].Value) == 18)
						{
							BrokeryExist = true;
						}
					}
				}
				if (Conversion.ToString((object)((Control)(object)cmbcommtype).Text) == "Flat")
				{
					if (CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					}).Rows.Count > 0)
					{
						double CommRate2 = Conversion.ToDouble((object)((Control)(object)cmbcommtype).Text.Trim());
						txtcommrate.Text = CommRate2.ToString(clsGlobalVariables.DecimalRateFormate);
						txtcommamount.Text = CommRate2.ToString(clsGlobalVariables.stringFormatsingle);
					}
					else
					{
						txtcommrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtcommamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
					DataTable dtBrokery2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					});
					if (dtBrokery2.Rows.Count > 0)
					{
						decimal BrokeryRate2 = Conversion.ToDecimal(dtBrokery2.Rows[0]["Rate"]);
						txtbrokeryrate.Text = BrokeryRate2.ToString(clsGlobalVariables.DecimalRateFormate);
						txtbrokeryamount.Text = BrokeryRate2.ToString(clsGlobalVariables.stringFormatsingle);
					}
					else
					{
						txtbrokeryrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtbrokeryamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
					DataTable dtSubBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbsubbrokeryacc.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					});
					if (dtSubBrokery.Rows.Count > 0)
					{
						decimal SubBrokeryRate3 = Conversion.ToDecimal(dtSubBrokery.Rows[0]["Rate"]);
						txtsubbrokeryrate.Text = SubBrokeryRate3.ToString(clsGlobalVariables.DecimalRateFormate);
						txtsubbrokeryamnt.Text = SubBrokeryRate3.ToString(clsGlobalVariables.stringFormatsingle);
					}
					else
					{
						txtsubbrokeryrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtsubbrokeryamnt.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
				if (((Control)(object)cmbcommtype).Text == "Percent")
				{
					DataTable dtcomm2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					});
					if (dtcomm2.Rows.Count > 0)
					{
						decimal TotalAmount = default(decimal);
						decimal CommRate3 = Conversion.ToDecimal(dtcomm2.Rows[0]["Rate"]);
						txtcommrate.Text = CommRate3.ToString();
						TotalAmount = (CommExist ? dtDetail.AsEnumerable().Sum((DataRow row) => row.Field<decimal>("CusAmount")) : (from row in dtDetail.AsEnumerable()
							where row.Field<int>("ProductTypeId") != 1
							select row).Sum((DataRow row) => row.Field<decimal>("CusAmount")));
						if (TotalAmount != 0m)
						{
							decimal CommAmt5 = TotalAmount * CommRate3 / 100m;
							txtcommamount.Text = Math.Round(CommAmt5).ToString();
						}
						else
						{
							txtcommamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
						}
					}
					else
					{
						txtcommrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtcommamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
					DataTable dtBrokery3 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					});
					if (dtBrokery3.Rows.Count > 0)
					{
						decimal TotalAmount2 = default(decimal);
						decimal BrokeryRate3 = Conversion.ToDecimal(dtBrokery3.Rows[0]["Rate"]);
						txtbrokeryrate.Text = BrokeryRate3.ToString();
						TotalAmount2 = (BrokeryExist ? dtDetail.AsEnumerable().Sum((DataRow row) => row.Field<decimal>("CusAmount")) : (from row in dtDetail.AsEnumerable()
							where row.Field<int>("ProductTypeId") != 1
							select row).Sum((DataRow row) => row.Field<decimal>("CusAmount")));
						if (TotalAmount2 != 0m)
						{
							decimal CommAmt4 = TotalAmount2 * BrokeryRate3 / 100m;
							txtbrokeryamount.Text = Math.Round(CommAmt4).ToString();
						}
						else
						{
							txtbrokeryamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
						}
					}
					else
					{
						txtbrokeryrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtbrokeryamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
					DataTable SubdtBrokery2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbsubbrokeryacc.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					});
					if (SubdtBrokery2.Rows.Count > 0)
					{
						object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2);
						decimal SubBrokeryRate2 = Conversion.ToDecimal(SubdtBrokery2.Rows[0]["Rate"]);
						txtsubbrokeryrate.Text = SubBrokeryRate2.ToString();
						if (total != null)
						{
							decimal CommAmt3 = Conversion.ToDecimal((object)total.ToString()) * SubBrokeryRate2 / 100m;
							txtsubbrokeryamnt.Text = Math.Round(CommAmt3).ToString();
						}
						else
						{
							txtsubbrokeryamnt.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
						}
					}
					else
					{
						txtsubbrokeryrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtsubbrokeryamnt.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
				if (((Control)(object)cmbcommtype).Text == "Weight")
				{
					DataTable dtcomm = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					});
					if (dtcomm.Rows.Count > 0)
					{
						decimal detailWeight2 = default(decimal);
						decimal CommRate = Conversion.ToDecimal(dtcomm.Rows[0]["Rate"]);
						txtcommrate.Text = CommRate.ToString();
						decimal UOM3 = Conversion.ToDecimal(dtcomm.Rows[0]["RateUom"]);
						detailWeight2 = (CommExist ? dtDetail.AsEnumerable().Sum((DataRow row) => row.Field<decimal>("CusNetWeight")) : (from row in dtDetail.AsEnumerable()
							where row.Field<int>("ProductTypeId") != 1
							select row).Sum((DataRow row) => row.Field<decimal>("CusNetWeight")));
						if (detailWeight2 != 0m)
						{
							decimal CommAmt2 = detailWeight2 / UOM3 * CommRate;
							txtcommamount.Text = Math.Round(CommAmt2).ToString();
						}
						else
						{
							txtcommamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
						}
					}
					else
					{
						txtcommrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtcommamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
					DataTable dtBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					});
					if (dtBrokery.Rows.Count > 0)
					{
						decimal detailWeight = default(decimal);
						decimal BrokeryRate = Conversion.ToDecimal(dtBrokery.Rows[0]["Rate"]);
						decimal UOM2 = Conversion.ToDecimal(dtBrokery.Rows[0]["RateUom"]);
						txtbrokeryrate.Text = BrokeryRate.ToString();
						detailWeight = (CommExist ? dtDetail.AsEnumerable().Sum((DataRow row) => row.Field<decimal>("CusNetWeight")) : (from row in dtDetail.AsEnumerable()
							where row.Field<int>("ProductTypeId") != 1
							select row).Sum((DataRow row) => row.Field<decimal>("CusNetWeight")));
						if (detailWeight != 0m)
						{
							decimal CommAmt = detailWeight / UOM2 * BrokeryRate;
							txtbrokeryamount.Text = Math.Round(CommAmt).ToString();
						}
						else
						{
							txtbrokeryamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
						}
					}
					else
					{
						txtbrokeryrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtbrokeryamount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
					DataTable SubdtBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbsubbrokeryacc.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						PaymenetTermId = Conversion.ToInt(cmbpayterm.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2,
						IntervalDays = Conversion.ToInt((object)txtduedays.Text)
					});
					if (SubdtBrokery.Rows.Count > 0)
					{
						object weight = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusNetWeight"], (AggregateFunction)2);
						decimal SubBrokeryRate = Conversion.ToDecimal(SubdtBrokery.Rows[0]["Rate"]);
						decimal UOM = Conversion.ToDecimal(SubdtBrokery.Rows[0]["RateUom"]);
						txtsubbrokeryrate.Text = SubBrokeryRate.ToString();
						if (weight != null)
						{
							decimal BrokeryAmt = Conversion.ToDecimal((object)weight.ToString()) / UOM * SubBrokeryRate;
							txtsubbrokeryamnt.Text = Math.Round(BrokeryAmt).ToString();
						}
						else
						{
							txtsubbrokeryamnt.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
						}
					}
					else
					{
						txtsubbrokeryrate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
						txtsubbrokeryamnt.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			_ = Conversion.ToDecimal((object)txtsubbrokeryamnt.Text) > 0m;
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
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsubbrokeryacc_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerBillAmount()
	{
		try
		{
			if (grdDetail.GetRows().Count() > 0)
			{
				decimal JournalDebit = default(decimal);
				decimal JournalCredit = default(decimal);
				decimal JournalDebitCus = default(decimal);
				decimal JournalCreditCus = default(decimal);
				decimal TotalCommAmount = default(decimal);
				decimal TotalBokeryAmount = default(decimal);
				decimal TotalPMGridAmount = default(decimal);
				decimal vehicleFreightTotal = default(decimal);
				GridEXRow[] checkedRows = grdVehicleDetail.GetCheckedRows();
				if (checkedRows.Count() > 0)
				{
					vehicleFreightTotal = checkedRows.Sum((GridEXRow row) => Conversion.ToDecimal(row.Cells["NetPaid"].Value));
				}
				decimal TotalAmount = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2));
				decimal TotalSupAmount = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["SupAmount"], (AggregateFunction)2));
				if (TotalAmount == TotalSupAmount)
				{
					((Control)(object)cmbDifferencAccount).Text = string.Empty;
					((Control)(object)cmbDifferencAccount).Enabled = false;
				}
				else
				{
					((Control)(object)cmbDifferencAccount).Enabled = true;
				}
				decimal TotalFcyAmount = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["CusFcyAmount"], (AggregateFunction)2));
				if (grdPackingMaterial.GetRows().Count() > 0)
				{
					GridEXRow[] rows = grdPackingMaterial.GetRows();
					foreach (GridEXRow r3 in rows)
					{
						TotalPMGridAmount += Conversion.ToDecimal(r3.Cells["Amount"].Value);
					}
					if (TotalPMGridAmount > 0m)
					{
						TotalAmount += TotalPMGridAmount;
					}
				}
				if (grdrevenue.GetRows().Count() > 0)
				{
					GridEXRow[] rows = grdrevenue.GetRows();
					foreach (GridEXRow r2 in rows)
					{
						if (Conversion.ToInt((object)r2.Cells["OtherRevenueId"].Value.ToString()) > 0)
						{
							JournalDebit += Conversion.ToDecimal((object)r2.Cells["AmountDr"].Value.ToString());
							JournalCredit += Conversion.ToDecimal((object)r2.Cells["AmountCr"].Value.ToString());
						}
					}
					if (JournalDebit > 0m)
					{
						TotalAmount -= JournalDebit;
					}
					else if (JournalCredit > 0m)
					{
						TotalAmount += JournalCredit;
					}
				}
				if (grdCustomerRevenueExpense.GetRows().Count() > 0)
				{
					GridEXRow[] rows = grdCustomerRevenueExpense.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (Conversion.ToInt((object)r.Cells["OtherRevenueId"].Value.ToString()) > 0)
						{
							JournalDebitCus += Conversion.ToDecimal((object)r.Cells["AmountDr"].Value.ToString());
							JournalCreditCus += Conversion.ToDecimal((object)r.Cells["AmountCr"].Value.ToString());
						}
					}
					if (JournalDebitCus > 0m)
					{
						TotalAmount -= JournalDebitCus;
					}
					else if (JournalCreditCus > 0m)
					{
						TotalAmount += JournalCreditCus;
					}
				}
				if (((UltraGridBase)cmbCommAgent).ActiveRow != null && Conversion.ToDecimal((object)txtcommamount.Text) > 0m)
				{
					TotalCommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
					TotalAmount += TotalCommAmount;
				}
				if (((UltraGridBase)cmbBrokeryAccount).ActiveRow != null && Conversion.ToDecimal((object)txtbrokeryamount.Text) > 0m)
				{
					TotalBokeryAmount = Conversion.ToDecimal((object)txtbrokeryamount.Text);
					TotalAmount += TotalBokeryAmount;
				}
				if (Conversion.ToDecimal((object)txtexchangerate.Text) > 0m)
				{
					TotalFcyAmount += (TotalCommAmount + TotalBokeryAmount) / Conversion.ToDecimal((object)txtexchangerate.Text);
				}
				else
				{
					TotalFcyAmount += TotalCommAmount + TotalBokeryAmount;
				}
				TotalAmount += vehicleFreightTotal;
				if (TotalAmount > 0m)
				{
					txtcustbillamnt.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
					if (TotalAmount == 0m || Conversion.ToDecimal((object)txtexchangerate.Text) == 0m)
					{
						txtfcyamount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
					}
					else
					{
						txtfcyamount.Text = (TotalAmount / Conversion.ToDecimal((object)txtexchangerate.Text)).ToString(clsGlobalVariables.stringFormatsingleForFcy);
					}
				}
				else
				{
					txtcustbillamnt.Text = "0";
					txtfcyamount.Text = "0";
				}
			}
			else
			{
				txtcustbillamnt.Text = "0";
				txtfcyamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCustomerLeave_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbpayterm_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				GridHistoryFill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryFill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 1704;
			obj.EntryUser = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.NoOfRecords = NoOfRecords;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtgrid = CommTradeTransactionHeader.FormHistory(obj);
			if (dtgrid.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("ManualBillNo", typeof(string));
				dtcol.Columns.Add("DifferenceAccount", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("CommRate", typeof(decimal));
				dtcol.Columns.Add("CommAmount", typeof(decimal));
				dtcol.Columns.Add("BrokeryRate", typeof(decimal));
				dtcol.Columns.Add("BrokeryAmount", typeof(decimal));
				dtcol.Columns.Add("SubBrokeryRate", typeof(decimal));
				dtcol.Columns.Add("SubBrokeryAmount", typeof(decimal));
				dtcol.Columns.Add("BiltyNos", typeof(string));
				dtcol.Columns.Add("BiltyDates", typeof(string));
				dtcol.Columns.Add("VehicleNos", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("TotalQty", typeof(int));
				dtcol.Columns.Add("SupTotalNetWeight", typeof(decimal));
				dtcol.Columns.Add("CusTotalNetWeight", typeof(decimal));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtgrid.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtgrid.Rows[i]["RecordNo"], dtgrid.Rows[i]["DocumentTypeId"], dtgrid.Rows[i]["Id"], dtgrid.Rows[i]["DocDate"], dtgrid.Rows[i]["DocNo"], dtgrid.Rows[i]["ManualBillNo"], dtgrid.Rows[i]["DifferenceAccountTitle"], dtgrid.Rows[i]["RemarksHeader"], dtgrid.Rows[i]["CommRate"], dtgrid.Rows[i]["CommAmount"], dtgrid.Rows[i]["BrokeryRate"], dtgrid.Rows[i]["BrokeryAmount"], dtgrid.Rows[i]["SubBrokeryRate"], dtgrid.Rows[i]["SubBrokeryAmount"], dtgrid.Rows[i]["BiltyNos"], dtgrid.Rows[i]["BiltyDates"], dtgrid.Rows[i]["VehicleNos"], dtgrid.Rows[i]["EntryDate"], dtgrid.Rows[i]["ModifyDate"], dtgrid.Rows[i]["IsApproved"], dtgrid.Rows[i]["TotalQty"], dtgrid.Rows[i]["SupTotalNetWeight"], dtgrid.Rows[i]["CusTotalNetWeight"], dtgrid.Rows[i]["EntryUserName"], dtgrid.Rows[i]["ModifyUserName"], dtgrid.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtcol;
				grdHistory.RetrieveStructure();
				GridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistorySetting()
	{
		try
		{
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalQty"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CommRate"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = "#,##0.######";
			grdHistory.RootTable.Columns["CommRate"].TotalFormatString = "#,##0.######";
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = "#,##0.######";
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = "#,##0.######";
			grdHistory.RootTable.Columns["BrokeryRate"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BrokeryRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BrokeryRate"].FormatString = "#,##0.######";
			grdHistory.RootTable.Columns["BrokeryRate"].TotalFormatString = "#,##0.######";
			grdHistory.RootTable.Columns["BrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BrokeryAmount"].FormatString = "#,##0.######";
			grdHistory.RootTable.Columns["BrokeryAmount"].TotalFormatString = "#,##0.######";
			grdHistory.RootTable.Columns["SubBrokeryRate"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SubBrokeryRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SubBrokeryRate"].FormatString = "#,##0.######";
			grdHistory.RootTable.Columns["SubBrokeryRate"].TotalFormatString = "#,##0.######";
			grdHistory.RootTable.Columns["SubBrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].FormatString = "#,##0.######";
			grdHistory.RootTable.Columns["SubBrokeryAmount"].TotalFormatString = "#,##0.######";
			grdHistory.RootTable.Columns["SupTotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SupTotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SupTotalNetWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["SupTotalNetWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CusTotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CusTotalNetWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalNetWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns.Add("Edit");
				grdHistory.RootTable.Columns["Edit"].Key = "Edit";
				grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistory.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns.Add("Print");
				grdHistory.RootTable.Columns["Print"].Key = "Print";
				grdHistory.RootTable.Columns["Print"].Caption = "1704-Print";
				grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Print"].ButtonText = "1704-Print";
				grdHistory.RootTable.Columns["Print"].Width = 75;
				if (HasMultiLanguageFeature)
				{
					grdHistory.RootTable.Columns.Add("UrduPrint");
					grdHistory.RootTable.Columns["UrduPrint"].Key = "UrduPrint";
					grdHistory.RootTable.Columns["UrduPrint"].Caption = "1704-Urdu";
					grdHistory.RootTable.Columns["UrduPrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdHistory.RootTable.Columns["UrduPrint"].ButtonStyle = (ButtonStyle)5;
					grdHistory.RootTable.Columns["UrduPrint"].ButtonText = "1704-Urdu";
					grdHistory.RootTable.Columns["UrduPrint"].Width = 75;
					grdHistory.RootTable.Columns.Add("UrduPrintWithSupplier");
					grdHistory.RootTable.Columns["UrduPrintWithSupplier"].Key = "UrduPrintWithSupplier";
					grdHistory.RootTable.Columns["UrduPrintWithSupplier"].Caption = "1704_01-Urdu";
					grdHistory.RootTable.Columns["UrduPrintWithSupplier"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdHistory.RootTable.Columns["UrduPrintWithSupplier"].ButtonStyle = (ButtonStyle)5;
					grdHistory.RootTable.Columns["UrduPrintWithSupplier"].ButtonText = "1704_01-Urdu";
					grdHistory.RootTable.Columns["UrduPrintWithSupplier"].Width = 75;
				}
				grdHistory.RootTable.Columns.Add("Voucher");
				grdHistory.RootTable.Columns["Voucher"].Key = "Voucher";
				grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
				grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
				grdHistory.RootTable.Columns["Voucher"].Width = 65;
			}
			if (formright.DoHaveUpdateRights && formright.DoHavePrintRights)
			{
				if (HasMultiLanguageFeature)
				{
					grdHistory.RootTable.Columns["Print"].Position = 0;
					grdHistory.RootTable.Columns["UrduPrint"].Position = 1;
					grdHistory.RootTable.Columns["UrduPrintWithSupplier"].Position = 2;
					grdHistory.RootTable.Columns["Voucher"].Position = 3;
					grdHistory.RootTable.Columns["Edit"].Position = 4;
					grdHistory.FrozenColumns = 5;
				}
				else
				{
					grdHistory.RootTable.Columns["Print"].Position = 0;
					grdHistory.RootTable.Columns["Voucher"].Position = 1;
					grdHistory.RootTable.Columns["Edit"].Position = 2;
					grdHistory.FrozenColumns = 3;
				}
			}
			else if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns["Edit"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				if (HasMultiLanguageFeature)
				{
					grdHistory.RootTable.Columns["Print"].Position = 0;
					grdHistory.RootTable.Columns["UrduPrint"].Position = 1;
					grdHistory.RootTable.Columns["UrduPrintWithSupplier"].Position = 2;
					grdHistory.RootTable.Columns["Voucher"].Position = 3;
					grdHistory.FrozenColumns = 4;
				}
				else
				{
					grdHistory.RootTable.Columns["Print"].Position = 0;
					grdHistory.RootTable.Columns["Voucher"].Position = 1;
					grdHistory.FrozenColumns = 2;
				}
			}
			ctrlGrdBar3_Load(null, null);
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
			if (formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				DocDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit" && formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				DocDate.Focus();
			}
			if (e.Column.Key == "Print" && formright.DoHavePrintRights)
			{
				int Id4 = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				GenerateSlip(Id4);
			}
			if (e.Column.Key == "UrduPrint" && formright.DoHavePrintRights)
			{
				int Id3 = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				GenerateMultiLangPrint(Id3);
			}
			if (e.Column.Key == "UrduPrintWithSupplier" && formright.DoHavePrintRights)
			{
				int Id2 = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				CommTradeTransaction_SlipWithSupplier_1704_01(Id2);
			}
			if (e.Column.Key == "Voucher" && formright.DoHavePrintRights)
			{
				int Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				GenerateVoucherSlip(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailbind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			CommTradeTransactionHeader po = new CommTradeTransactionHeader();
			po = CommTradeTransactionHeader.ReadById(Id);
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("LoadingNo", typeof(int));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("BiltyDate", typeof(DateTime));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("VehicleQty", typeof(decimal));
				dt.Columns.Add("VehicleGrossWeight", typeof(decimal));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("CropYear", typeof(string));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("Uom", typeof(string));
				dt.Columns.Add("ItemQty", typeof(decimal));
				dt.Columns.Add("CustomerAccount", typeof(string));
				dt.Columns.Add("CusGrossWeight", typeof(decimal));
				dt.Columns.Add("CusPMWeight", typeof(decimal));
				dt.Columns.Add("CusAdlsWeight", typeof(decimal));
				dt.Columns.Add("CusNetWeight", typeof(decimal));
				dt.Columns.Add("CusRate", typeof(decimal));
				dt.Columns.Add("CusAmount", typeof(decimal));
				dt.Columns.Add("CusFcyAmount", typeof(decimal));
				dt.Columns.Add("SupplierAccount", typeof(string));
				dt.Columns.Add("SupGrossWeight", typeof(decimal));
				dt.Columns.Add("SupPMWeight", typeof(decimal));
				dt.Columns.Add("SupAdlsWeight", typeof(decimal));
				dt.Columns.Add("SupNetWeight", typeof(decimal));
				dt.Columns.Add("SupRate", typeof(decimal));
				dt.Columns.Add("SupAmount", typeof(decimal));
				dt.Columns.Add("SupFcyAmount", typeof(decimal));
				dt.Clear();
				for (int i = 0; i < po.CommTradeTransactionDetailList.Count; i++)
				{
					dt.Rows.Add(po.CommTradeTransactionDetailList[i].Id, po.CommTradeTransactionDetailList[i].OrderNo, po.CommTradeTransactionDetailList[i].LoadingDeliveryDocNo, po.CommTradeTransactionDetailList[i].BiltyNo, po.CommTradeTransactionDetailList[i].BiltyDate, po.CommTradeTransactionDetailList[i].VehicleNo, po.CommTradeTransactionDetailList[i].VehicleQty, po.CommTradeTransactionDetailList[i].VehicleGrossWeight, po.CommTradeTransactionDetailList[i].ItemId, po.CommTradeTransactionDetailList[i].ItemName, po.CommTradeTransactionDetailList[i].ItemCode, po.CommTradeTransactionDetailList[i].CropYear, po.CommTradeTransactionDetailList[i].PackingType, po.CommTradeTransactionDetailList[i].PackUom, po.CommTradeTransactionDetailList[i].Qty, po.CommTradeTransactionDetailList[i].CustomerName, po.CommTradeTransactionDetailList[i].CusGrossWeight, po.CommTradeTransactionDetailList[i].CusPckMtrlWeight, po.CommTradeTransactionDetailList[i].CusAdLsWt, po.CommTradeTransactionDetailList[i].CusNetWeight, po.CommTradeTransactionDetailList[i].CusRate, po.CommTradeTransactionDetailList[i].CusAmount, po.CommTradeTransactionDetailList[i].CustFcyAmount, po.CommTradeTransactionDetailList[i].SupplierName, po.CommTradeTransactionDetailList[i].SupGrossWeight, po.CommTradeTransactionDetailList[i].SupPckMtrlWeight, po.CommTradeTransactionDetailList[i].SupAdLsWt, po.CommTradeTransactionDetailList[i].SupNetWeight, po.CommTradeTransactionDetailList[i].SupRate, po.CommTradeTransactionDetailList[i].SupAmount, po.CommTradeTransactionDetailList[i].SupFcyAmount);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				GridHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryDetailSetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["OrderNo"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].Visible = false;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].Caption = "Gross Weight";
			grdHistoryDetail.RootTable.Columns["CusAdlsWeight"].Caption = "Adls Weight";
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].Caption = "PM Weight";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].Caption = "Net Weight";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].Caption = "Gross Weight";
			grdHistoryDetail.RootTable.Columns["SupAdlsWeight"].Caption = "Adls Weight";
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].Caption = "PM Weight";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].Caption = "Net Weight";
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["SupAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupAmount"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupAmount"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusAmount"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusAmount"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusRate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["CusRate"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusRate"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusRate"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupRate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["SupRate"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupRate"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupRate"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdHistoryDetail.RootTable.Columns["VehicleQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["VehicleQty"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["VehicleQty"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["VehicleQty"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["VehicleGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["VehicleGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["VehicleGrossWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["VehicleGrossWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupAdlsWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupAdlsWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupAdlsWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupAdlsWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusAdlsWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusAdlsWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusAdlsWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusAdlsWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.######";
			grdHistoryDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.######";
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString().ToString()), 1704);
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
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grddetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoaddAllHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GridHistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int PrintId)
	{
		try
		{
			CommonServices.CommTradeTransaction_Slip_1704(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommTradeTransaction_SlipWithSupplier_1704_01(int PrintId)
	{
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			if (Conversion.ToInt(CmbMultiLanguage.Value) <= 0)
			{
				tabControl1.SelectedIndex = 0;
				CmbMultiLanguage.Focus();
				MessageBox.Show("Please Select Language!", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				CommonServices.CommTradeTransaction_SlipWithSupplier_1704_01(PrintId, Conversion.ToInt(CmbMultiLanguage.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateMultiLangPrint(int PrintId)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			if (Conversion.ToInt(CmbMultiLanguage.Value) <= 0)
			{
				tabControl1.SelectedIndex = 0;
				CmbMultiLanguage.Focus();
				MessageBox.Show("Please Select Language!", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.DocumentTypeId = 1704;
			obj.LanguageId = Conversion.ToInt(CmbMultiLanguage.Value);
			DataTable dt = CommTradeTransactionHeader.CommissionTransactionSlipAndRegister(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject2 = new SubReportObject(CommTradeTransactionHeader.CommTradeTransactionPM_SubReport(obj.Id, obj.LanguageId), "CommTradeTransactionPM_SubReport.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject = new SubReportObject(CommTradeTransactionHeader.CommTradeTransactionCustomerExpense_SubReport(obj.Id), "1704-CommTradeTransactionCustomerExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1704-CommTradeTransactionHeader_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiLanguagefill()
	{
		try
		{
			int id = Conversion.ToInt(CmbMultiLanguage.Value);
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbMultiLanguage, "Id", "LanguageDescription", "Language", true);
			((UltraGridBase)CmbMultiLanguage).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbMultiLanguage).Rows[1].Activate();
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbMultiLanguage.Value = id;
				}
				else
				{
					((Control)(object)CmbMultiLanguage).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateVoucherSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateVoucherSlip(int Id)
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(Id, 1704)), 1704);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnMultiLangPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GenerateMultiLangPrint(RecId);
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsMultiLangPrint_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				if (Conversion.ToInt(CmbMultiLanguage.Value) <= 0)
				{
					tabControl1.SelectedIndex = 0;
					CmbMultiLanguage.Focus();
					MessageBox.Show("Please Select Language!", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.BranchesId = UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.Id = RecId;
				obj.DocumentTypeId = 1704;
				obj.LanguageId = Conversion.ToInt(CmbMultiLanguage.Value);
				DataTable dt = CommTradeTransactionHeader.CommissionTransactionSlipAndRegister(obj);
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(CommTradeTransactionHeader.CommTradeTransactionCustomerExpense_SubReport(obj.Id), "1704-CommTradeTransactionCustomerExpense_SubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, e.ClickedItem.Text + ".rpt", lstSubReports);
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintSupplier1704_01_Click(object sender, EventArgs e)
	{
		try
		{
			CommTradeTransaction_SlipWithSupplier_1704_01(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdVehicleDetail_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
	{
		try
		{
			CustomerBillAmount();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Expected O, but got Unknown
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Expected O, but got Unknown
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Expected O, but got Unknown
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Expected O, but got Unknown
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Expected O, but got Unknown
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Expected O, but got Unknown
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Expected O, but got Unknown
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Expected O, but got Unknown
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Expected O, but got Unknown
		//IL_0856: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Expected O, but got Unknown
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Expected O, but got Unknown
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Expected O, but got Unknown
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Expected O, but got Unknown
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Expected O, but got Unknown
		//IL_52c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ce: Expected O, but got Unknown
		//IL_52db: Unknown result type (might be due to invalid IL or missing references)
		//IL_52e5: Expected O, but got Unknown
		//IL_55b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_55bc: Expected O, but got Unknown
		//IL_55c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_55d3: Expected O, but got Unknown
		//IL_55e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_55ea: Expected O, but got Unknown
		//IL_5ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5abe: Expected O, but got Unknown
		//IL_5acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ad5: Expected O, but got Unknown
		//IL_5f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f5a: Expected O, but got Unknown
		//IL_5f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f71: Expected O, but got Unknown
		//IL_5f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f88: Expected O, but got Unknown
		//IL_6237: Unknown result type (might be due to invalid IL or missing references)
		//IL_6241: Expected O, but got Unknown
		//IL_624e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6258: Expected O, but got Unknown
		//IL_6265: Unknown result type (might be due to invalid IL or missing references)
		//IL_626f: Expected O, but got Unknown
		//IL_651e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6528: Expected O, but got Unknown
		//IL_67c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_67ca: Expected O, but got Unknown
		//IL_67d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_67e1: Expected O, but got Unknown
		//IL_8138: Unknown result type (might be due to invalid IL or missing references)
		//IL_8142: Expected O, but got Unknown
		//IL_8166: Unknown result type (might be due to invalid IL or missing references)
		//IL_8170: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CmTr.TransactionForms.CommisionBillAgainstLoadingDelivery));
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		GridEXLayout grdrevenue_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTermSupplier_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTermCustomer_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdVehicleDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdCustomerRevenueExpense_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdsummary_DesignTimeLayout = new GridEXLayout();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnMultiLangPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSupplier1704_01 = new System.Windows.Forms.ToolStripButton();
		this.btnVoucher = new System.Windows.Forms.ToolStripButton();
		this.tsMultiLangPrint = new System.Windows.Forms.ToolStripDropDownButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadFromLoadingDelivery = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtfcyamount = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtBiltyDates = new System.Windows.Forms.TextBox();
		this.txtexchangerate = new System.Windows.Forms.TextBox();
		this.txtBiltyNos = new System.Windows.Forms.TextBox();
		this.txtmanualref = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbpayterm = new UltraCombo();
		this.txtVehicleNos = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.cmbcurrency = new UltraCombo();
		this.cmbDifferencAccount = new UltraCombo();
		this.cmbCustomername = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbdeliveryterm = new UltraCombo();
		this.txtcustbillamnt = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtsubbrokeryamnt = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtsubbrokeryrate = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbsubbrokeryacc = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtbrokeryamount = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtbrokeryrate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbcommtype = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbCommAgent = new UltraCombo();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdrevenue = new GridEX();
		this.label29 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdPackingMaterial = new GridEX();
		this.label28 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdPaymentTermSupplier = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.tabCustomerPaymentDetail = new System.Windows.Forms.TabPage();
		this.grdPaymentTermCustomer = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label57 = new System.Windows.Forms.Label();
		this.tabVehicle = new System.Windows.Forms.TabPage();
		this.grdVehicleDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label36 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdCustomerRevenueExpense = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label34 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdsummary = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbBrokeryAccount = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ChkUrduPrintPreview = new System.Windows.Forms.CheckBox();
		this.CmbMultiLanguage = new UltraCombo();
		this.labelmultilang = new System.Windows.Forms.Label();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.label1 = new System.Windows.Forms.Label();
		this.History = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label31 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnLoadAllHistory = new System.Windows.Forms.ToolStripButton();
		this.label16 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbpayterm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDifferencAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomername).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsubbrokeryacc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommAgent).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdrevenue).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).BeginInit();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermSupplier).BeginInit();
		this.panel13.SuspendLayout();
		this.tabCustomerPaymentDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermCustomer).BeginInit();
		this.panel5.SuspendLayout();
		this.tabVehicle.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdVehicleDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerRevenueExpense).BeginInit();
		this.panel7.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsummary).BeginInit();
		this.panel4.SuspendLayout();
		this.panel11.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBrokeryAccount).BeginInit();
		this.groupBox3.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMultiLanguage).BeginInit();
		this.History.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1337, 28);
		this.panel2.TabIndex = 1;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[20]
		{
			this.btnNew, this.toolStripSeparator5, this.btnRefresh, this.toolStripSeparator3, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator2,
			this.BtnPrint, this.btnMultiLangPrint, this.btnPrintSupplier1704_01, this.btnVoucher, this.tsMultiLangPrint, this.toolStripSeparator1, this.BtnLoadFromLoadingDelivery, this.toolStripSeparator7, this.btnShortcutKeys, this.toolStripSeparator6
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1337, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(100, 24);
		this.BtnPrint.Text = "1704-&Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnMultiLangPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnMultiLangPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnMultiLangPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnMultiLangPrint.Name = "btnMultiLangPrint";
		this.btnMultiLangPrint.Size = new System.Drawing.Size(100, 24);
		this.btnMultiLangPrint.Text = "1704-Urdu";
		this.btnMultiLangPrint.Click += new System.EventHandler(btnMultiLangPrint_Click);
		this.btnPrintSupplier1704_01.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintSupplier1704_01.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSupplier1704_01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSupplier1704_01.Name = "btnPrintSupplier1704_01";
		this.btnPrintSupplier1704_01.Size = new System.Drawing.Size(124, 24);
		this.btnPrintSupplier1704_01.Text = "1704_01-Urdu";
		this.btnPrintSupplier1704_01.Click += new System.EventHandler(btnPrintSupplier1704_01_Click);
		this.btnVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucher.Name = "btnVoucher";
		this.btnVoucher.Size = new System.Drawing.Size(115, 24);
		this.btnVoucher.Text = "103-&Voucher";
		this.btnVoucher.Click += new System.EventHandler(btnVoucher_Click);
		this.tsMultiLangPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsMultiLangPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsMultiLangPrint.Name = "tsMultiLangPrint";
		this.tsMultiLangPrint.Size = new System.Drawing.Size(140, 24);
		this.tsMultiLangPrint.Text = "Multi Lang Reports";
		this.tsMultiLangPrint.ToolTipText = "MultiLangReports";
		this.tsMultiLangPrint.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsMultiLangPrint_DropDownItemClicked);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnLoadFromLoadingDelivery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnLoadFromLoadingDelivery.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadFromLoadingDelivery.Image = (System.Drawing.Image)resources.GetObject("BtnLoadFromLoadingDelivery.Image");
		this.BtnLoadFromLoadingDelivery.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadFromLoadingDelivery.Name = "BtnLoadFromLoadingDelivery";
		this.BtnLoadFromLoadingDelivery.Size = new System.Drawing.Size(191, 24);
		this.BtnLoadFromLoadingDelivery.Text = "Load From Loading Delivery";
		this.BtnLoadFromLoadingDelivery.Click += new System.EventHandler(BtnLoadFromLoadingDelivery_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.groupBox1.Controls.Add(this.txtfcyamount);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtBiltyDates);
		this.groupBox1.Controls.Add(this.txtexchangerate);
		this.groupBox1.Controls.Add(this.txtBiltyNos);
		this.groupBox1.Controls.Add(this.txtmanualref);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpayterm);
		this.groupBox1.Controls.Add(this.txtVehicleNos);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtbillno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDifferencAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomername);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliveryterm);
		this.groupBox1.Controls.Add(this.txtcustbillamnt);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(541, 271);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.txtfcyamount.BackColor = System.Drawing.Color.White;
		this.txtfcyamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyamount.Enabled = false;
		this.txtfcyamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfcyamount.Location = new System.Drawing.Point(407, 243);
		this.txtfcyamount.Name = "txtfcyamount";
		this.txtfcyamount.ReadOnly = true;
		this.txtfcyamount.Size = new System.Drawing.Size(130, 23);
		this.txtfcyamount.TabIndex = 15;
		this.txtfcyamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(87, 187);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(229, 79);
		this.txtRemarks.TabIndex = 7;
		this.txtRemarks.Text = "";
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(3, 197);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 26;
		this.label11.Text = "Remarks";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(318, 247);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(72, 15);
		this.label23.TabIndex = 35;
		this.label23.Text = "Fcy Amount";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 136);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(63, 15);
		this.label10.TabIndex = 24;
		this.label10.Text = "Bilty Dates";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(3, 109);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 21;
		this.label9.Text = "Bilty Nos.";
		this.txtBiltyDates.BackColor = System.Drawing.Color.White;
		this.txtBiltyDates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyDates.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyDates.Location = new System.Drawing.Point(87, 132);
		this.txtBiltyDates.Name = "txtBiltyDates";
		this.txtBiltyDates.Size = new System.Drawing.Size(229, 23);
		this.txtBiltyDates.TabIndex = 5;
		this.txtexchangerate.BackColor = System.Drawing.Color.White;
		this.txtexchangerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexchangerate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtexchangerate.Location = new System.Drawing.Point(407, 216);
		this.txtexchangerate.Name = "txtexchangerate";
		this.txtexchangerate.Size = new System.Drawing.Size(130, 23);
		this.txtexchangerate.TabIndex = 14;
		this.txtexchangerate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtexchangerate.TextChanged += new System.EventHandler(txtexchangerate_TextChanged);
		this.txtBiltyNos.BackColor = System.Drawing.Color.White;
		this.txtBiltyNos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNos.Location = new System.Drawing.Point(87, 105);
		this.txtBiltyNos.Name = "txtBiltyNos";
		this.txtBiltyNos.Size = new System.Drawing.Size(229, 23);
		this.txtBiltyNos.TabIndex = 4;
		this.txtmanualref.BackColor = System.Drawing.Color.White;
		this.txtmanualref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtmanualref.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmanualref.Location = new System.Drawing.Point(407, 76);
		this.txtmanualref.Name = "txtmanualref";
		this.txtmanualref.Size = new System.Drawing.Size(130, 23);
		this.txtmanualref.TabIndex = 9;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(318, 220);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(84, 15);
		this.label22.TabIndex = 33;
		this.label22.Text = "Exchange Rate";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(3, 80);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 19;
		this.label8.Text = "Vehicle Nos.";
		this.cmbpayterm.AllowNull = (DefaultableBoolean)1;
		this.cmbpayterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpayterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpayterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbpayterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Location = new System.Drawing.Point(407, 103);
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Name = "cmbpayterm";
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbpayterm).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Leave += new System.EventHandler(cmbpayterm_Leave);
		this.txtVehicleNos.BackColor = System.Drawing.Color.White;
		this.txtVehicleNos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNos.Location = new System.Drawing.Point(87, 76);
		this.txtVehicleNos.Name = "txtVehicleNos";
		this.txtVehicleNos.Size = new System.Drawing.Size(229, 23);
		this.txtVehicleNos.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(318, 109);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 15);
		this.label2.TabIndex = 9;
		this.label2.Text = "Payment Term";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(204, 19);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(112, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.Leave += new System.EventHandler(cmbCustomerLeave_Leave);
		this.txtduedays.BackColor = System.Drawing.Color.White;
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(407, 132);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(130, 23);
		this.txtduedays.TabIndex = 11;
		this.txtduedays.TextChanged += new System.EventHandler(cmbCustomerLeave_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(153, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(51, 15);
		this.label4.TabIndex = 7;
		this.label4.Text = "Bill Date";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(318, 193);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(55, 15);
		this.label21.TabIndex = 31;
		this.label21.Text = "Fcy Code";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(3, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(42, 15);
		this.label17.TabIndex = 7;
		this.label17.Text = "Bill No";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(318, 136);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(58, 15);
		this.label3.TabIndex = 11;
		this.label3.Text = "Due Days";
		this.txtbillno.BackColor = System.Drawing.Color.White;
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Enabled = false;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(87, 19);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.ReadOnly = true;
		this.txtbillno.Size = new System.Drawing.Size(65, 23);
		this.txtbillno.TabIndex = 0;
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(407, 187);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 13;
		this.cmbDifferencAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbDifferencAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDifferencAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDifferencAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDifferencAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDifferencAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDifferencAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDifferencAccount).Location = new System.Drawing.Point(87, 158);
		((System.Windows.Forms.Control)(object)this.cmbDifferencAccount).Name = "cmbDifferencAccount";
		((System.Windows.Forms.Control)(object)this.cmbDifferencAccount).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbDifferencAccount).TabIndex = 6;
		this.cmbDifferencAccount.ValueChanged += new System.EventHandler(cmbCustomerLeave_Leave);
		this.cmbCustomername.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Location = new System.Drawing.Point(87, 45);
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Name = "cmbCustomername";
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomername).TabIndex = 2;
		this.cmbCustomername.ValueChanged += new System.EventHandler(cmbCustomerLeave_Leave);
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.White;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(3, 164);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(84, 15);
		this.label33.TabIndex = 5;
		this.label33.Text = "Difference A/c";
		this.label20.BackColor = System.Drawing.Color.LightGray;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(321, 19);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(216, 23);
		this.label20.TabIndex = 29;
		this.label20.Text = "Bill Amount";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 51);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(58, 15);
		this.label7.TabIndex = 5;
		this.label7.Text = "Customer";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(318, 80);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 15;
		this.label5.Text = "Manual Ref";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(318, 164);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 17;
		this.label6.Text = "Delivery Term";
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(407, 158);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 12;
		this.txtcustbillamnt.BackColor = System.Drawing.Color.White;
		this.txtcustbillamnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustbillamnt.Enabled = false;
		this.txtcustbillamnt.Font = new System.Drawing.Font("Verdana", 13f);
		this.txtcustbillamnt.Location = new System.Drawing.Point(321, 44);
		this.txtcustbillamnt.Name = "txtcustbillamnt";
		this.txtcustbillamnt.ReadOnly = true;
		this.txtcustbillamnt.Size = new System.Drawing.Size(216, 29);
		this.txtcustbillamnt.TabIndex = 8;
		this.txtcustbillamnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(180, 55);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(67, 15);
		this.label27.TabIndex = 33;
		this.label27.Text = "Broker Amt";
		this.txtsubbrokeryamnt.BackColor = System.Drawing.Color.White;
		this.txtsubbrokeryamnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsubbrokeryamnt.Enabled = false;
		this.txtsubbrokeryamnt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsubbrokeryamnt.Location = new System.Drawing.Point(251, 51);
		this.txtsubbrokeryamnt.Name = "txtsubbrokeryamnt";
		this.txtsubbrokeryamnt.ReadOnly = true;
		this.txtsubbrokeryamnt.Size = new System.Drawing.Size(100, 23);
		this.txtsubbrokeryamnt.TabIndex = 2;
		this.txtsubbrokeryamnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 55);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(67, 15);
		this.label26.TabIndex = 31;
		this.label26.Text = "Broker Rate";
		this.txtsubbrokeryrate.BackColor = System.Drawing.Color.White;
		this.txtsubbrokeryrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsubbrokeryrate.Enabled = false;
		this.txtsubbrokeryrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsubbrokeryrate.Location = new System.Drawing.Point(98, 51);
		this.txtsubbrokeryrate.Name = "txtsubbrokeryrate";
		this.txtsubbrokeryrate.Size = new System.Drawing.Size(82, 23);
		this.txtsubbrokeryrate.TabIndex = 1;
		this.txtsubbrokeryrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 28);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(87, 15);
		this.label25.TabIndex = 29;
		this.label25.Text = "Sub Broker A/c";
		this.cmbsubbrokeryacc.AllowNull = (DefaultableBoolean)1;
		this.cmbsubbrokeryacc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsubbrokeryacc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsubbrokeryacc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsubbrokeryacc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Location = new System.Drawing.Point(98, 22);
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Name = "cmbsubbrokeryacc";
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Leave += new System.EventHandler(cmbsubbrokeryacc_Leave);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(182, 149);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(73, 15);
		this.label19.TabIndex = 27;
		this.label19.Text = "Brokery Amt";
		this.txtbrokeryamount.BackColor = System.Drawing.Color.White;
		this.txtbrokeryamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbrokeryamount.Enabled = false;
		this.txtbrokeryamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbrokeryamount.Location = new System.Drawing.Point(253, 145);
		this.txtbrokeryamount.Name = "txtbrokeryamount";
		this.txtbrokeryamount.ReadOnly = true;
		this.txtbrokeryamount.Size = new System.Drawing.Size(100, 23);
		this.txtbrokeryamount.TabIndex = 6;
		this.txtbrokeryamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(5, 149);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(73, 15);
		this.label18.TabIndex = 25;
		this.label18.Text = "Brokery Rate";
		this.txtbrokeryrate.BackColor = System.Drawing.Color.White;
		this.txtbrokeryrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbrokeryrate.Enabled = false;
		this.txtbrokeryrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbrokeryrate.Location = new System.Drawing.Point(100, 145);
		this.txtbrokeryrate.Name = "txtbrokeryrate";
		this.txtbrokeryrate.Size = new System.Drawing.Size(82, 23);
		this.txtbrokeryrate.TabIndex = 5;
		this.txtbrokeryrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 52);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(74, 15);
		this.label15.TabIndex = 21;
		this.label15.Text = "Comm. Type";
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(100, 46);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Leave += new System.EventHandler(cmbCustomerLeave_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(182, 52);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 19;
		this.label14.Text = "Comm Amt";
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Enabled = false;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommamount.Location = new System.Drawing.Point(253, 48);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(100, 23);
		this.txtcommamount.TabIndex = 3;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(5, 80);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 15);
		this.label13.TabIndex = 17;
		this.label13.Text = "Comm Rate";
		this.txtcommrate.BackColor = System.Drawing.Color.White;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Enabled = false;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.Location = new System.Drawing.Point(100, 76);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(82, 23);
		this.txtcommrate.TabIndex = 2;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 23);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(95, 15);
		this.label12.TabIndex = 11;
		this.label12.Text = "Commission A/c";
		this.cmbCommAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbCommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Location = new System.Drawing.Point(100, 17);
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Name = "cmbCommAgent";
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).TabIndex = 0;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label29);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1329, 24);
		this.panel10.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1282, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdrevenue;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(44, 22);
		this.ctrlGrdBar1.TabIndex = 43;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		((ScrollableBase)this.grdrevenue).BackColor = System.Drawing.Color.White;
		grdrevenue_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdrevenue.DesignTimeLayout = grdrevenue_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdrevenue.GroupByBoxVisible = false;
		this.grdrevenue.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdrevenue).Name = "grdrevenue";
		((ScrollableBase)this.grdrevenue).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Size = new System.Drawing.Size(1329, 299);
		((System.Windows.Forms.Control)(object)this.grdrevenue).TabIndex = 0;
		this.grdrevenue.TotalRow = (InheritableBoolean)1;
		this.grdrevenue.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdrevenue).VisualStyle = (VisualStyle)4;
		this.grdrevenue.CellUpdated += new ColumnActionEventHandler(grdrevenue_CellUpdated);
		this.grdrevenue.ColumnButtonClick += new ColumnActionEventHandler(grdrevenue_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdrevenue).KeyDown += new System.Windows.Forms.KeyEventHandler(grdrevenue_KeyDown);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(3, 3);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(99, 14);
		this.label29.TabIndex = 40;
		this.label29.Text = "Other Revenue";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar7);
		this.panel9.Controls.Add(this.label28);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1323, 24);
		this.panel9.TabIndex = 4;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1280, -1);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdPackingMaterial;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar7.TabIndex = 43;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		((ScrollableBase)this.grdPackingMaterial).BackColor = System.Drawing.Color.White;
		grdPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterial.DesignTimeLayout = grdPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterial.GroupByBoxVisible = false;
		this.grdPackingMaterial.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Name = "grdPackingMaterial";
		((ScrollableBase)this.grdPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(1323, 293);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 0;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterial.EditModeChanged += new System.EventHandler(grdPackingMaterial_EditModeChanged);
		this.grdPackingMaterial.EditingCell += new EditingCellEventHandler(grdPackingMaterial_EditingCell);
		this.grdPackingMaterial.CellUpdated += new ColumnActionEventHandler(grdPackingMaterial_CellUpdated);
		this.grdPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackingMaterial_KeyDown);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(3, 3);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(100, 14);
		this.label28.TabIndex = 40;
		this.label28.Text = "Packing Matrial";
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1351, 711);
		this.tabControl1.TabIndex = 6;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.tabControl2);
		this.Form.Controls.Add(this.panel1);
		this.Form.Controls.Add(this.panel17);
		this.Form.Controls.Add(this.panel2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(3);
		this.Form.Size = new System.Drawing.Size(1343, 682);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Controls.Add(this.tabCustomerPaymentDetail);
		this.tabControl2.Controls.Add(this.tabVehicle);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(3, 327);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1337, 352);
		this.tabControl2.TabIndex = 5;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1329, 323);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1323, 294);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label30);
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1323, 23);
		this.panel12.TabIndex = 44;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(3, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(42, 14);
		this.label30.TabIndex = 40;
		this.label30.Text = "Detail";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1282, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 43;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterial);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1329, 323);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Packing Material";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdrevenue);
		this.tabPage3.Controls.Add(this.panel10);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1329, 323);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Other Revenue";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier);
		this.tabPage6.Controls.Add(this.panel13);
		this.tabPage6.Location = new System.Drawing.Point(4, 25);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(1329, 323);
		this.tabPage6.TabIndex = 5;
		this.tabPage6.Text = "Supplier Payment Detail";
		this.tabPage6.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPaymentTermSupplier).BackColor = System.Drawing.Color.White;
		this.grdPaymentTermSupplier.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTermSupplier_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTermSupplier.DesignTimeLayout = grdPaymentTermSupplier_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTermSupplier.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Name = "grdPaymentTermSupplier";
		((ScrollableBase)this.grdPaymentTermSupplier).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Size = new System.Drawing.Size(1329, 297);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).TabIndex = 4;
		this.grdPaymentTermSupplier.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTermSupplier.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTermSupplier).VisualStyle = (VisualStyle)4;
		this.grdPaymentTermSupplier.CellUpdated += new ColumnActionEventHandler(grdPaymentTermSupplier_CellUpdated);
		this.grdPaymentTermSupplier.UpdatingCell += new UpdatingCellEventHandler(grdPaymentTermSupplier_UpdatingCell);
		this.grdPaymentTermSupplier.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTermSupplier_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTermSupplier_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label35);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Margin = new System.Windows.Forms.Padding(2);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1329, 26);
		this.panel13.TabIndex = 3;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(3, 3);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(135, 19);
		this.label35.TabIndex = 561;
		this.label35.Text = "Payment Detail";
		this.tabCustomerPaymentDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTermCustomer);
		this.tabCustomerPaymentDetail.Controls.Add(this.panel5);
		this.tabCustomerPaymentDetail.Location = new System.Drawing.Point(4, 25);
		this.tabCustomerPaymentDetail.Name = "tabCustomerPaymentDetail";
		this.tabCustomerPaymentDetail.Size = new System.Drawing.Size(1329, 323);
		this.tabCustomerPaymentDetail.TabIndex = 4;
		this.tabCustomerPaymentDetail.Text = "Customer Payment Detail";
		this.tabCustomerPaymentDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPaymentTermCustomer).BackColor = System.Drawing.Color.White;
		this.grdPaymentTermCustomer.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTermCustomer_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTermCustomer.DesignTimeLayout = grdPaymentTermCustomer_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermCustomer).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermCustomer).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTermCustomer.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermCustomer).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermCustomer).Name = "grdPaymentTermCustomer";
		((ScrollableBase)this.grdPaymentTermCustomer).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermCustomer).Size = new System.Drawing.Size(1329, 297);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermCustomer).TabIndex = 4;
		this.grdPaymentTermCustomer.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTermCustomer.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTermCustomer).VisualStyle = (VisualStyle)4;
		this.grdPaymentTermCustomer.CellUpdated += new ColumnActionEventHandler(grdPaymentTermCustomer_CellUpdated);
		this.grdPaymentTermCustomer.UpdatingCell += new UpdatingCellEventHandler(grdPaymentTermCustomer_UpdatingCell);
		this.grdPaymentTermCustomer.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTermCustomer_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermCustomer).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTermCustomer_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label57);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1329, 26);
		this.panel5.TabIndex = 3;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(3, 3);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(135, 19);
		this.label57.TabIndex = 561;
		this.label57.Text = "Payment Detail";
		this.tabVehicle.Controls.Add((System.Windows.Forms.Control)(object)this.grdVehicleDetail);
		this.tabVehicle.Controls.Add(this.panel14);
		this.tabVehicle.Location = new System.Drawing.Point(4, 25);
		this.tabVehicle.Name = "tabVehicle";
		this.tabVehicle.Size = new System.Drawing.Size(1329, 323);
		this.tabVehicle.TabIndex = 6;
		this.tabVehicle.Text = "Vehicle Freight";
		this.tabVehicle.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdVehicleDetail).BackColor = System.Drawing.Color.White;
		this.grdVehicleDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdVehicleDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdVehicleDetail.DesignTimeLayout = grdVehicleDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdVehicleDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdVehicleDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdVehicleDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdVehicleDetail).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdVehicleDetail).Name = "grdVehicleDetail";
		((ScrollableBase)this.grdVehicleDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdVehicleDetail).Size = new System.Drawing.Size(1329, 297);
		((System.Windows.Forms.Control)(object)this.grdVehicleDetail).TabIndex = 6;
		this.grdVehicleDetail.TotalRow = (InheritableBoolean)1;
		this.grdVehicleDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdVehicleDetail).VisualStyle = (VisualStyle)4;
		this.grdVehicleDetail.RowCheckStateChanged += new RowCheckStateChangeEventHandler(grdVehicleDetail_RowCheckStateChanged);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label36);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Margin = new System.Windows.Forms.Padding(2);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1329, 26);
		this.panel14.TabIndex = 5;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(3, 3);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(170, 19);
		this.label36.TabIndex = 561;
		this.label36.Text = "Vehicle Information";
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerRevenueExpense);
		this.tabPage4.Controls.Add(this.panel7);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1329, 323);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Expenses Allocated To Brand And Customer";
		this.tabPage4.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdCustomerRevenueExpense).BackColor = System.Drawing.Color.White;
		grdCustomerRevenueExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerRevenueExpense.DesignTimeLayout = grdCustomerRevenueExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerRevenueExpense).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCustomerRevenueExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerRevenueExpense.GroupByBoxVisible = false;
		this.grdCustomerRevenueExpense.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdCustomerRevenueExpense).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdCustomerRevenueExpense).Name = "grdCustomerRevenueExpense";
		((ScrollableBase)this.grdCustomerRevenueExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerRevenueExpense).Size = new System.Drawing.Size(1329, 299);
		((System.Windows.Forms.Control)(object)this.grdCustomerRevenueExpense).TabIndex = 5;
		this.grdCustomerRevenueExpense.TotalRow = (InheritableBoolean)1;
		this.grdCustomerRevenueExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerRevenueExpense).VisualStyle = (VisualStyle)4;
		this.grdCustomerRevenueExpense.CellUpdated += new ColumnActionEventHandler(grdCustomerRevenueExpense_CellUpdated);
		this.grdCustomerRevenueExpense.ColumnButtonClick += new ColumnActionEventHandler(grdCustomerRevenueExpense_ColumnButtonClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar5);
		this.panel7.Controls.Add(this.label34);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1329, 24);
		this.panel7.TabIndex = 6;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1282, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar5.MyGrid = this.grdCustomerRevenueExpense;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(44, 22);
		this.ctrlGrdBar5.TabIndex = 43;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 3);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(120, 14);
		this.label34.TabIndex = 40;
		this.label34.Text = "Customer Expense";
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel11);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 56);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1337, 271);
		this.panel1.TabIndex = 10;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdsummary);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(909, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(428, 271);
		this.panel3.TabIndex = 37;
		this.grdsummary.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdsummary).BackColor = System.Drawing.Color.White;
		grdsummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsummary.DesignTimeLayout = grdsummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsummary).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdsummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsummary.GroupByBoxVisible = false;
		this.grdsummary.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdsummary).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdsummary).Name = "grdsummary";
		((ScrollableBase)this.grdsummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsummary).Size = new System.Drawing.Size(428, 249);
		((System.Windows.Forms.Control)(object)this.grdsummary).TabIndex = 4;
		this.grdsummary.TotalRow = (InheritableBoolean)1;
		this.grdsummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsummary).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label24);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(428, 22);
		this.panel4.TabIndex = 3;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Teal;
		this.label24.Location = new System.Drawing.Point(6, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(143, 14);
		this.label24.TabIndex = 40;
		this.label24.Text = "Summary By Suppliers";
		this.panel11.Controls.Add(this.groupBox2);
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(541, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(368, 271);
		this.panel11.TabIndex = 1;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBrokeryAccount);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommAgent);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.groupBox2.Controls.Add(this.txtbrokeryrate);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.txtcommamount);
		this.groupBox2.Controls.Add(this.txtbrokeryamount);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.txtcommrate);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(360, 176);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Commission && Brokery Revenue";
		this.cmbBrokeryAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbBrokeryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBrokeryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBrokeryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBrokeryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBrokeryAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbBrokeryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBrokeryAccount).Location = new System.Drawing.Point(101, 115);
		((System.Windows.Forms.Control)(object)this.cmbBrokeryAccount).Name = "cmbBrokeryAccount";
		((System.Windows.Forms.Control)(object)this.cmbBrokeryAccount).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.cmbBrokeryAccount).TabIndex = 4;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(5, 121);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(69, 15);
		this.label32.TabIndex = 11;
		this.label32.Text = "Brokery A/c";
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add(this.txtsubbrokeryrate);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.txtsubbrokeryamnt);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(6, 189);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(360, 81);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Broker Expense";
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ChkUrduPrintPreview);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMultiLanguage);
		this.panel17.Controls.Add(this.labelmultilang);
		this.panel17.Controls.Add(this.ChkBok);
		this.panel17.Controls.Add(this.ChkPrintslip);
		this.panel17.Controls.Add(this.label1);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 31);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1337, 25);
		this.panel17.TabIndex = 3;
		this.ChkUrduPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkUrduPrintPreview.AutoSize = true;
		this.ChkUrduPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkUrduPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkUrduPrintPreview.Location = new System.Drawing.Point(1236, 3);
		this.ChkUrduPrintPreview.Margin = new System.Windows.Forms.Padding(2);
		this.ChkUrduPrintPreview.Name = "ChkUrduPrintPreview";
		this.ChkUrduPrintPreview.Size = new System.Drawing.Size(98, 20);
		this.ChkUrduPrintPreview.TabIndex = 45;
		this.ChkUrduPrintPreview.Text = "Urdu Print";
		this.ChkUrduPrintPreview.UseVisualStyleBackColor = true;
		this.CmbMultiLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbMultiLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMultiLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMultiLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMultiLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Location = new System.Drawing.Point(780, 1);
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Name = "CmbMultiLanguage";
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Size = new System.Drawing.Size(128, 23);
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).TabIndex = 43;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Visible = false;
		this.labelmultilang.AutoSize = true;
		this.labelmultilang.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelmultilang.ForeColor = System.Drawing.Color.White;
		this.labelmultilang.Location = new System.Drawing.Point(698, 4);
		this.labelmultilang.Name = "labelmultilang";
		this.labelmultilang.Size = new System.Drawing.Size(78, 16);
		this.labelmultilang.TabIndex = 44;
		this.labelmultilang.Text = "Language";
		this.labelmultilang.Visible = false;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1109, 3);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(124, 20);
		this.ChkBok.TabIndex = 42;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Checked = true;
		this.ChkPrintslip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintslip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(1014, 3);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(90, 20);
		this.ChkPrintslip.TabIndex = 41;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(137, 19);
		this.label1.TabIndex = 40;
		this.label1.Text = "Commission Bill";
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.History.Controls.Add(this.panel8);
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.History.Controls.Add(this.panel6);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(3);
		this.History.Size = new System.Drawing.Size(1343, 682);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 570);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1337, 109);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 47;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label31);
		this.panel8.Controls.Add(this.ctrlGrdBar4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 545);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1337, 25);
		this.panel8.TabIndex = 48;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(3, 6);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(215, 14);
		this.label31.TabIndex = 40;
		this.label31.Text = "Detail of the Above Selected Row";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1296, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdHistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar4.TabIndex = 43;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 61);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1337, 484);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 45;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1337, 58);
		this.panel6.TabIndex = 46;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnLoadAllHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1337, 27);
		this.toolStrip2.TabIndex = 44;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnLoadAllHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadAllHistory.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadAllHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadAllHistory.Name = "BtnLoadAllHistory";
		this.BtnLoadAllHistory.Size = new System.Drawing.Size(82, 24);
		this.BtnLoadAllHistory.Text = "&Load All";
		this.BtnLoadAllHistory.Click += new System.EventHandler(btnLoaddAllHistory_Click);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 32);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(200, 19);
		this.label16.TabIndex = 40;
		this.label16.Text = "Commission Bill History";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1299, 34);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 43;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1351, 711);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CommisionBillAgainstLoadingDelivery";
		this.Text = "CommisionBill";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(CommisionBill_FormClosed);
		base.Load += new System.EventHandler(CommisionBill_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommisionBill_KeyDown);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbpayterm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDifferencAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomername).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsubbrokeryacc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommAgent).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdrevenue).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermSupplier).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.tabCustomerPaymentDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermCustomer).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabVehicle.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdVehicleDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerRevenueExpense).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsummary).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBrokeryAccount).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMultiLanguage).EndInit();
		this.History.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
