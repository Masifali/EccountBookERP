using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.lgstcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.lgstcm;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.LogisticManagement.FromWise_Helper_Methods;
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
namespace Architecture.WinApp.Service.lgstcm
{

public class frmLogisticPurchaseServicesBill : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtCustomerDetail = new DataTable();

	private DataTable dtFreightDetail = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtServiceProvider = new DataTable();

	private DataTable dtDebitAccounts = new DataTable();

	private DataTable dtLocationType = new DataTable();

	private DataTable dtLocations = new DataTable();

	private DataTable dtCurrency = new DataTable();

	private string ScreenName = "frmLogisticPurchaseServicesBill";

	private bool UpdateMode;

	private int RecId;

	private int ForeignBaseCurrency;

	private double ForeignBaseCurrencyRate;

	private int LocalBaseCurrency;

	private double LocalBaseCurrencyRate;

	private int DocumentTypeId = 1304;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool ItemSearchWithNameOrCode = true;

	private bool TaxPercntEditableOrNot;

	private bool HasMultiCurrencyFeature;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int DefaultTranspotationServiceItemId;

	private HashSet<int> FreightVoucherIds = new HashSet<int>();

	private List<ServicesBillDetail> lstRemoveRecordcustomerDetail = new List<ServicesBillDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private GroupBox groupBox2;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	internal GridEX grdHistoryDetail;

	private Panel panel9;

	internal GridEX grdHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker ToDateHistory;

	private Label label28;

	private TextBox ToDocNoHistory;

	private DateTimePicker FromDateHistory;

	private Label label35;

	private TextBox FromDocNoHistory;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkPreview;

	private TextBox txtRemarksHeader;

	private Label label15;

	internal GridEX grdCustomerDetail;

	private ToolStripButton btnAttachment;

	private Label label1;

	private Label label5;

	private ToolStripButton BtnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel5;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbServiceType;

	private Label label11;

	internal UltraCombo CmbServiceProviderOrBrokerAgent;

	internal UltraCombo CmbServiceTypeHistory;

	private Label label3;

	private TextBox txtFcyExchangeRate;

	internal UltraCombo CmbFcyCurrency;

	private Label label10;

	private Label label9;

	private Label label22;

	private Label label21;

	internal UltraCombo CmbServiceProviderHistory;

	private Label label6;

	internal UltraCombo CmbBillToPartyName;

	private Label label14;

	private ToolStripButton BtnAgreement;

	private TextBox txtrefbillno;

	internal UltraCombo CmbInvoiceNo;

	private Label label50;

	private Label label51;

	private Label label54;

	internal UltraCombo CmbPaymentTerm;

	private TextBox txtDueDays;

	private Label label4;

	private Label label24;

	private Label label23;

	private Label label20;

	private Label label18;

	private Label label13;

	private Label label12;

	private Label label8;

	private Label label7;

	private ToolStripButton btnPrintVoucher;

	private CheckBox chkVoucher;

	private GroupBox GrpDetailInfo;

	private TextBox txtTransactionExchangeRateDetail;

	internal UltraCombo CmbTransactionCurrencyDetail;

	internal UltraCombo CmbLocationTo;

	private Label label55;

	internal UltraCombo CmbLocationFrom;

	internal UltraCombo CmbLocationType;

	private Label label34;

	private TextBox txtTotalAmount;

	private Label label56;

	private TextBox txtTaxPercnt;

	private Label label65;

	private Label label16;

	private Label label25;

	private Label label63;

	private TextBox txtTaxAmount;

	private Label label62;

	private TextBox txtQty;

	private Label label58;

	private Label label26;

	private Label label57;

	private Label label61;

	private Label label45;

	private Label label27;

	private Label label29;

	internal UltraCombo CmbTaxName;

	private Label label30;

	private TextBox txtPartyRefDocNo;

	private Label label31;

	private Label label52;

	private DateTimePicker txtPartyRefDocDate;

	private Panel panel34;

	private RadioButton RadCategory;

	private RadioButton RadMasterItem;

	private Label label53;

	private Label label67;

	private Label label66;

	private Label label64;

	private Label label33;

	private TextBox txtServiceRate;

	private Label label49;

	internal UltraCombo CmbMasterItem;

	private Label label48;

	private RadioButton RadItemCodePackListDetail;

	private Label label36;

	private RadioButton RadItemNamePackListDetail;

	internal UltraCombo CmbItemName;

	private Label label38;

	private Label label39;

	private Label label40;

	private Label label41;

	private Button BtnAddCustomer;

	private Button BtnCancelCustomer;

	private TextBox txtRemarksDetail;

	private Button BtnUpdateCustmer;

	private Label label42;

	private TextBox txtTcyAmount;

	private TextBox txtLcyAmountDetail;

	private Label label43;

	internal UltraCombo CmbRateBaseType;

	private Panel PanelGlcy;

	internal UltraCombo CmbGlcyCurrency;

	private TextBox txtGlcyAmount;

	private Label label44;

	private Label label46;

	private Label label68;

	private Label label59;

	private Label label60;

	private TextBox txtGlcyRate;

	private ToolStripButton BtnLoadFreightVoucher;

	private TextBox txtNetWeight;

	private Label label69;

	private Label label2;

	public frmLogisticPurchaseServicesBill(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1200, 650);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtSupplier.Columns.Add("CurrencyId", typeof(int));
			dtSupplier.Columns.Add("CurrencyCode", typeof(string));
			dtServiceProvider = dtSupplier.Clone();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("MasterItemId", typeof(int));
			dtItem.Columns.Add("MasterItem");
			dtItem.Columns.Add("ItemCategoryId", typeof(int));
			dtItem.Columns.Add("ItemCategory");
			LogisticsServicesBill_Helper.InitializeDetailtable(dtCustomerDetail);
			LogisticsServicesBill_Helper.InitializeFreightvoucherDetailtable(dtFreightDetail);
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
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable historyComboData = HistoryComboDbCall();
			string documentNoData = DocumentNoDbCall();
			DataTable lookUpData = LookUps.LookUpAllServices(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			return new
			{
				HistoryComboData = historyComboData,
				DocumentNoData = documentNoData,
				LookUpData = lookUpData
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
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnPrint.Enabled = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				ctrlGrdBar3.mGridSaveLayouts.Visible = false;
				ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
				HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
				PanelGlcy.Visible = HasMultiCurrencyFeature;
				GetConfigurationsFromGlobal();
				if (RecId == 0)
				{
					txtdocno.Text = result.DocumentNoData;
				}
				AllLookUpComboBind(result.LookUpData);
				SupplierDtFillFromGlobal();
				BrokerAgentOrServiceProviderBind(CmbBillToPartyName, dtSupplier, "Broker Agent");
				BrokerAgentOrServiceProviderBind(CmbServiceProviderOrBrokerAgent, dtServiceProvider, "Service Provider");
				CurrencyBindFromGlobal();
				ItemCategoryOrMasterItemBind();
				ItemDtsFillFromGlobal();
				ItemNameBind();
				GetConfigurationsFromGlobalandBind();
				BindGrids();
				HistoryComboBind(result.HistoryComboData);
				Datetypefill();
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				txtdocdate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultTranspotationServiceItemId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultTranspotationServiceItemId"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalandBind()
	{
		try
		{
			ForeignBaseCurrency = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ForeignBaseCurrency"));
			ForeignBaseCurrencyRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FcyBaseCurrencyRate"));
			LocalBaseCurrency = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			LocalBaseCurrencyRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
			if (ForeignBaseCurrency > 0)
			{
				CmbFcyCurrency.Value = ForeignBaseCurrency;
				txtFcyExchangeRate.Text = ((Conversion.ToDouble((object)txtFcyExchangeRate.Text) > 0.0) ? txtFcyExchangeRate.Text : ForeignBaseCurrencyRate.ToString(clsGlobalVariables.DecimalFCYRateFormate));
			}
			TaxPercntEditableOrNot = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("TaxPercentEditable"));
			txtTaxPercnt.Enabled = TaxPercntEditableOrNot;
			if (grdCustomerDetail.CurrentRow != null)
			{
				grdCustomerDetail.RootTable.Columns["Tax%"].EditType = (EditType)(TaxPercntEditableOrNot ? 1 : 0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return ServicesBillHeader.GenerateCode(new ServicesBillHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllLookUpComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtSP = new DataTable();
			dtSP.Columns.Add("Id", typeof(int));
			dtSP.Columns.Add("Name");
			DataTable DtPaymentTerm = dtSP.Clone();
			DataTable DtUom = dtSP.Clone();
			dtLocationType.Clear();
			DataTable dataTable = dtLocationType;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtLocationType = dtSP.Clone();
			}
			DtUom.Columns.Add("Equivalent", typeof(double));
			foreach (DataRow row in dtcombo.Rows)
			{
				string activity = row["Activity"]?.ToString();
				int id = Conversion.ToInt(row["Id"]);
				string name = row["ReferenceName"]?.ToString();
				switch (activity)
				{
				case "ServiceType":
					dtSP.Rows.Add(id, name);
					break;
				case "PaymentTerm":
					DtPaymentTerm.Rows.Add(id, name);
					break;
				case "LocationType":
					dtLocationType.Rows.Add(id, name);
					break;
				case "RateBaseUom":
				{
					double equivalent = Conversion.ToDouble(row["OtherReference"]);
					DtUom.Rows.Add(id, name, equivalent);
					break;
				}
				}
			}
			ServiceTypeBind(dtSP);
			PaymentTermsBind(DtPaymentTerm);
			RateBaseTypeTypeBind(DtUom);
			LocationTypeBind(dtLocationType);
			dtDebitAccounts.Clear();
			dtDebitAccounts = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[3] { 6, 8, 10 });
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ServiceTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbServiceType, dt, "Id", "Name", "ServiceType", AllColumns: true, Conversion.ToInt(CmbServiceType.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNoBind(int InvoiceId, string InvoiceNo, int InvoiceAccountId)
	{
		try
		{
			DataTable dtInvoice = new DataTable();
			dtInvoice.Columns.Add("Id", typeof(int));
			dtInvoice.Columns.Add("InvoiceNo", typeof(string));
			dtInvoice.Columns.Add("InvoiceAccountId", typeof(int));
			dtInvoice.Rows.Add(InvoiceId, InvoiceNo, InvoiceAccountId);
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNo, dtInvoice, "Id", "InvoiceNo", "InvoiceNo", AllColumns: true, Conversion.ToInt(CmbInvoiceNo.Value));
			if (dtInvoice.Rows.Count > 0)
			{
				((UltraGridBase)CmbInvoiceNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			dtServiceProvider.Clear();
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer.ToList();
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				object[] rowData = new object[8] { item.Id, item.CompanyName, item.PartyCode, item.GlAccountId, item.CityName, item.MobilePersonal, item.GlAccountCurrencyId, item.GlAccountCurrencyCode };
				dtSupplier.Rows.Add(rowData);
				dtServiceProvider.Rows.Add(rowData);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BrokerAgentOrServiceProviderBind(UltraCombo ultraCombo, DataTable dataTable, string Caption)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(ultraCombo, dataTable, "Id", "CompanyName", Caption, AllColumns: true, Conversion.ToInt(ultraCombo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dataTable.Rows.Count > 0)
			{
				((UltraGridBase)ultraCombo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)ultraCombo).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyBindFromGlobal()
	{
		try
		{
			dtCurrency.Clear();
			if (dtCurrency.Columns.Count == 0)
			{
				dtCurrency.Columns.Add("Id", typeof(int));
				dtCurrency.Columns.Add("CurrencyCode", typeof(string));
				dtCurrency.Columns.Add("CurrencyRate", typeof(double));
			}
			foreach (MultiCurrency dd in clsGlobalVariables.globalMultiCurrency)
			{
				dtCurrency.Rows.Add(dd.Id, dd.CurrencyCode, dd.CurrencyRate);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbGlcyCurrency, dtCurrency, "Id", "CurrencyCode", "Glcy Currency", AllColumns: true, Conversion.ToInt(CmbGlcyCurrency.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbFcyCurrency, dtCurrency, "Id", "CurrencyCode", "Fcy Currency", AllColumns: true, Conversion.ToInt(CmbFcyCurrency.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbTransactionCurrencyDetail, dtCurrency, "Id", "CurrencyCode", "Transaction Currency", AllColumns: true, Conversion.ToInt(CmbTransactionCurrencyDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokerAgentName_Leave(object sender, EventArgs e)
	{
		try
		{
			int currencyId = 0;
			if (((UltraGridBase)CmbBillToPartyName).ActiveRow != null && Conversion.ToInt(CmbBillToPartyName.Value) > 0)
			{
				currencyId = Conversion.ToInt(((UltraDropDownBase)CmbBillToPartyName).SelectedRow.Cells["CurrencyId"].Value);
			}
			if (currencyId > 0)
			{
				CmbGlcyCurrency.Value = currencyId;
			}
			else
			{
				((Control)(object)CmbGlcyCurrency).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsBind(DataTable dtterm)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTerm, dtterm, "Id", "Name", "Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTerm.Value), insertDefaultRow: true, null, null, ActivateRow: true, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategoryOrMasterItemBind()
	{
		try
		{
			List<getGlobalAllSerivesItems> allItems = clsGlobalVariables.getGlobalAllSerivesItems;
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			dt.Rows.Clear();
			if (RadCategory.Checked)
			{
				foreach (var category in (from row in allItems
					where !string.IsNullOrEmpty(row.ItemCategory) && !dt.Rows.Cast<DataRow>().Any((DataRow r) => r["Description"].ToString() == row.ItemCategory)
					select new
					{
						Id = Conversion.ToInt((object)row.ItemCategoryId),
						Description = row.ItemCategory
					}).Distinct().ToList())
				{
					dt.Rows.Add(category.Id, category.Description);
				}
			}
			else if (RadMasterItem.Checked)
			{
				foreach (var type in (from row in allItems
					where !string.IsNullOrEmpty(row.ServicesMasterItem) && !dt.Rows.Cast<DataRow>().Any((DataRow r) => r["Description"].ToString() == row.ServicesMasterItem)
					select new
					{
						Id = Conversion.ToInt((object)row.ServicesMasterItemId),
						Description = row.ServicesMasterItem
					}).Distinct().ToList())
				{
					dt.Rows.Add(type.Id, type.Description);
				}
			}
			string DisplayName = (RadCategory.Checked ? "Item Category" : "Master Item");
			InfragisticsHelper.BindAndRetainSelection(CmbMasterItem, dt, "Id", "Description", DisplayName, AllColumns: true, Conversion.ToInt(CmbMasterItem.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal(int CategoryOrMasterItemId = 0)
	{
		try
		{
			dtItem.Clear();
			List<getGlobalAllSerivesItems> allItems = clsGlobalVariables.getGlobalAllSerivesItems;
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			foreach (getGlobalAllSerivesItems row in allItems)
			{
				int MasterItemId = Conversion.ToInt((object)row.ServicesMasterItemId);
				int ItemCategoryId = Conversion.ToInt((object)row.ItemCategoryId);
				if (CategoryOrMasterItemId == 0 || (RadCategory.Checked && CategoryOrMasterItemId == ItemCategoryId) || (RadMasterItem.Checked && CategoryOrMasterItemId == MasterItemId))
				{
					dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.ServicesMasterItemId, row.ServicesMasterItem, row.ItemCategoryId, row.ItemCategory);
				}
			}
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
			string ValueMember = (RadItemNamePackListDetail.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (RadItemNamePackListDetail.Checked ? "Service Item Name" : "Service Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbTransactionCurrencyDetail_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbTransactionCurrencyDetail.Value) > 0 && ((UltraGridBase)CmbTransactionCurrencyDetail).ActiveRow != null)
			{
				double Rate = Conversion.ToDouble(((UltraDropDownBase)CmbTransactionCurrencyDetail).SelectedRow.Cells[2].Value);
				if (Rate > 0.0)
				{
					txtTransactionExchangeRateDetail.Text = ((Conversion.ToDouble((object)txtTransactionExchangeRateDetail.Text) > 0.0) ? txtTransactionExchangeRateDetail.Text : Rate.ToString(clsGlobalVariables.DecimalFCYRateFormate));
				}
			}
			CalculateAmountsInDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RateBaseTypeTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbRateBaseType, dt, "Id", "Name", "Rate Base", AllColumns: true, Conversion.ToInt(CmbRateBaseType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadItemNamePackListDetail_CheckedChanged(object sender, EventArgs e)
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

	private void CmbMasterItem_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDtsFillFromGlobal(Conversion.ToInt(CmbMasterItem.Value));
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadMasterItem_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemCategoryOrMasterItemBind();
			ItemDtsFillFromGlobal(Conversion.ToInt(CmbMasterItem.Value));
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(CmbItemName.Value);
			if (((UltraGridBase)CmbItemName).ActiveRow != null && ItemId > 0)
			{
				TaxTypeBind(TaxTypeDbCall(ItemId, txtdocdate.Value));
				return;
			}
			((Control)(object)CmbTaxName).Text = string.Empty;
			((UltraGridBase)CmbTaxName).DataSource = null;
			txtTaxPercnt.Clear();
			txtTaxAmount.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable TaxTypeDbCall(int ItemId, DateTime DocDate)
	{
		try
		{
			DataTable dtTax = new DataTable();
			dtTax.Columns.Add("Id");
			dtTax.Columns.Add("TaxType");
			dtTax.Columns.Add("TaxPrcnt");
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(ItemId, DocDate);
			if (dt.Rows.Count > 0)
			{
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
			}
			return dtTax;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TaxTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbTaxName, dt, "Id", "TaxType", "Tax Name", AllColumns: true, Conversion.ToInt(CmbTaxName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (updateDetailIndex > -1 && grdCustomerDetail.CurrentRow != null && Conversion.ToInt(grdCustomerDetail.CurrentRow.Cells["TaxNameId"].Value) > 0)
			{
				CmbTaxName.Value = Conversion.ToInt(grdCustomerDetail.CurrentRow.Cells["TaxNameId"].Value);
			}
			CmbTaxName_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbLocationType, dt, "Id", "Name", "LocationType", AllColumns: true, Conversion.ToInt(CmbLocationType.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbLocationType_Leave(object sender, EventArgs e)
	{
		try
		{
			int locTypeId = Conversion.ToInt(CmbLocationType.Value);
			LocationdtFill(locTypeId);
			LocationsBind(dtLocations);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable PortsDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return SeaPorts.Getall(new SeaPorts
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

	private void LocationdtFill(int locTypeId)
	{
		try
		{
			dtLocations.Clear();
			DataTable dataTable = dtLocations;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtLocations.Columns.Add("Id", typeof(int));
				dtLocations.Columns.Add("LocationName", typeof(string));
			}
			switch (locTypeId)
			{
			case 16:
			{
				foreach (DataRow dr in PortsDbCall().Rows)
				{
					dtLocations.Rows.Add(dr["Id"], dr["PortName"]);
				}
				break;
			}
			case 17:
			{
				foreach (GlobalAllCity dr2 in clsGlobalVariables.globalAllCities)
				{
					dtLocations.Rows.Add(dr2.Id, dr2.CityName);
				}
				break;
			}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationsBind(DataTable dt)
	{
		try
		{
			int num = Conversion.ToInt(CmbLocationType.Value);
			string FromDesc = ((num == 16) ? "Loading Port" : "City From");
			string ToDesc = ((num == 16) ? "Destination Port" : "City To");
			InfragisticsHelper.BindAndRetainSelection(CmbLocationFrom, dt, "Id", "LocationName", FromDesc, AllColumns: false, Conversion.ToInt(CmbLocationFrom.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbLocationTo, dt, "Id", "LocationName", ToDesc, AllColumns: false, Conversion.ToInt(CmbLocationTo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationCustomerDetail()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbServiceProviderOrBrokerAgent, "Service Provider", FormHelper.ValidType.INT),
			(txtPartyRefDocNo, "Ref Doc No", FormHelper.ValidType.String),
			((Control)(object)CmbItemName, "Item Name", FormHelper.ValidType.INT),
			((Control)(object)CmbTransactionCurrencyDetail, "Transaction Currency", FormHelper.ValidType.INT),
			(txtTransactionExchangeRateDetail, "Transaction Exchange Rate", FormHelper.ValidType.Double),
			(txtServiceRate, "Service Rate", FormHelper.ValidType.Double),
			((Control)(object)CmbRateBaseType, "Rate Base", FormHelper.ValidType.INT),
			(txtQty, "Qty", FormHelper.ValidType.Double),
			(txtNetWeight, "NetWeight", FormHelper.ValidType.Double),
			(txtTcyAmount, "Tcy Amount", FormHelper.ValidType.Double),
			(txtLcyAmountDetail, "Lcy Amount", FormHelper.ValidType.Double),
			(txtTotalAmount, "Total Tcy Amount", FormHelper.ValidType.Double),
			((Control)(object)CmbLocationType, "Location Type", FormHelper.ValidType.INT),
			((Control)(object)CmbLocationFrom, "Location From", FormHelper.ValidType.INT),
			((Control)(object)CmbLocationTo, "Location To", FormHelper.ValidType.INT)
		}))
		{
			return false;
		}
		if (Conversion.ToInt(CmbLocationFrom.Value) == Conversion.ToInt(CmbLocationTo.Value))
		{
			CmbLocationTo.Focus();
			MessageBox.Show("From Location Can't be Same as Location To");
			return false;
		}
		return true;
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtCustomerDetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdCustomerDetail.DataSource = dtCustomerDetail;
				grdCustomerDetail.RetrieveStructure();
				grdCustomerDetailSetting(grdCustomerDetail, FreightVoucherIds.Any());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			if (FreightVoucherIds.Any())
			{
				MessageBox.Show("You cannot add new row as there are rows linked with Freight Voucher in the grid. Please remove those rows to add new row.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			if (grdCustomerDetail.RowCount == 0)
			{
				throw new Exception("No rows available in the customer detail grid.Please load atleast one row in detail grid from Purchase Order");
			}
			if (FormValidationCustomerDetail())
			{
				DataRow dr = dtCustomerDetail.NewRow();
				dr["Id"] = 0;
				dr["ChargesTypeId"] = 19;
				FillCustomerDetailRow(dr);
				dtCustomerDetail.Rows.Add(dr);
				BindGrids();
				ResetCustomerDetail();
				txtGlcyRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateCustmer_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationCustomerDetail() && updateDetailIndex >= 0 && updateDetailIndex < dtCustomerDetail.Rows.Count)
			{
				DataRow dr = dtCustomerDetail.Rows[updateDetailIndex];
				FillCustomerDetailRow(dr);
				grdCustomerDetail.DataSource = dtCustomerDetail;
				((Control)(object)grdCustomerDetail).Refresh();
				ResetCustomerDetail();
				txtGlcyRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCustomerDetailRow(DataRow dr)
	{
		dr["PartyRefDocNo"] = Conversion.ToString((object)txtPartyRefDocNo.Text);
		dr["PartyRefDocDate"] = Conversion.ToDateTime((object)txtPartyRefDocDate.Value);
		int itemId = Conversion.ToInt(CmbItemName.Value);
		dr["ItemId"] = itemId;
		string value;
		if (itemId <= 0)
		{
			value = string.Empty;
		}
		else
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbItemName).SelectedRow;
			value = Conversion.ToString((selectedRow != null) ? selectedRow.Cells["ItemName"].Value : null);
		}
		dr["ServiceItemName"] = value;
		string value2;
		if (itemId <= 0)
		{
			value2 = string.Empty;
		}
		else
		{
			UltraGridRow selectedRow2 = ((UltraDropDownBase)CmbItemName).SelectedRow;
			value2 = Conversion.ToString((selectedRow2 != null) ? selectedRow2.Cells["ItemCode"].Value : null);
		}
		dr["ServiceItemCode"] = value2;
		dr["TransactionCurrencyId"] = Conversion.ToInt(CmbTransactionCurrencyDetail.Value);
		dr["TransactionCurrency"] = Conversion.ToString((object)((Control)(object)CmbTransactionCurrencyDetail).Text);
		dr["TransactionExchangeRate"] = Conversion.ToDecimal((object)txtTransactionExchangeRateDetail.Text);
		dr["ServiceRate"] = Conversion.ToDecimal((object)txtServiceRate.Text);
		int rateBaseId = Conversion.ToInt(CmbRateBaseType.Value);
		dr["RateBaseId"] = rateBaseId;
		dr["RateBase"] = Conversion.ToString((object)((Control)(object)CmbRateBaseType).Text);
		decimal obj;
		if (rateBaseId <= 0 || ((UltraGridBase)CmbRateBaseType).ActiveRow == null)
		{
			obj = 0m;
		}
		else
		{
			UltraGridRow selectedRow3 = ((UltraDropDownBase)CmbRateBaseType).SelectedRow;
			obj = Conversion.ToDecimal((selectedRow3 != null) ? selectedRow3.Cells[2].Value : null);
		}
		dr["RateBaseEquivalent"] = obj;
		dr["Qty"] = Conversion.ToDecimal((object)txtQty.Text);
		dr["NetWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text);
		decimal tcyAmount = Conversion.ToDecimal((object)txtTcyAmount.Text);
		dr["TcyAmount"] = tcyAmount;
		dr["LcyAmount"] = Conversion.ToDecimal((object)txtLcyAmountDetail.Text);
		int taxId = Conversion.ToInt(CmbTaxName.Value);
		decimal taxPercent = Conversion.ToDecimal((object)txtTaxPercnt.Text);
		decimal taxAmount = tcyAmount * taxPercent / 100m;
		decimal totalTcyAmount = tcyAmount + taxAmount;
		dr["TaxNameId"] = taxId;
		dr["TaxName"] = ((taxId > 0) ? Conversion.ToString((object)((Control)(object)CmbTaxName).Text) : string.Empty);
		dr["Tax%"] = taxPercent;
		dr["TaxAmount"] = taxAmount;
		dr["TotalTcyAmount"] = totalTcyAmount;
		dr["LocationTypeId"] = Conversion.ToInt(CmbLocationType.Value);
		dr["LocationType"] = Conversion.ToString((object)((Control)(object)CmbLocationType).Text);
		dr["LocationFromId"] = Conversion.ToInt(CmbLocationFrom.Value);
		dr["LocationFrom"] = Conversion.ToString((object)((Control)(object)CmbLocationFrom).Text);
		dr["LocationToId"] = Conversion.ToInt(CmbLocationTo.Value);
		dr["LocationTo"] = Conversion.ToString((object)((Control)(object)CmbLocationTo).Text);
		dr["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
	}

	private void BtnCancelCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			ResetCustomerDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow row = grdCustomerDetail.CurrentRow;
			if (row != null)
			{
				if (FreightVoucherIds.Any())
				{
					MessageBox.Show("You cannot Update row linked with Freight Voucher.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					return;
				}
				if (Conversion.ToInt(row.Cells["PurchaseOrderHeaderId"].Value) > 0 || Conversion.ToInt(row.Cells["ChargesTypeId"].Value) == 18)
				{
					MessageBox.Show("You cannot edit this Row as it is linked with Purchase Order.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					return;
				}
				updateDetailIndex = row.RowIndex;
				((Control)(object)CmbMasterItem).Text = "";
				ItemDtsFillFromGlobal();
				ItemNameBind();
				txtPartyRefDocNo.Text = Conversion.ToString(row.Cells["PartyRefDocNo"].Value);
				txtPartyRefDocDate.Value = Conversion.ToDateTime(row.Cells["PartyRefDocDate"].Value);
				CmbItemName.Value = row.Cells["ItemId"].Value;
				CmbTransactionCurrencyDetail.Value = row.Cells["TransactionCurrencyId"].Value;
				txtTransactionExchangeRateDetail.Text = Conversion.ToString(row.Cells["TransactionExchangeRate"].Value);
				txtServiceRate.Text = Conversion.ToString(row.Cells["ServiceRate"].Value);
				CmbRateBaseType.Value = row.Cells["RateBaseId"].Value;
				txtQty.Text = Conversion.ToString(row.Cells["Qty"].Value);
				txtNetWeight.Text = Conversion.ToString(row.Cells["NetWeight"].Value);
				txtTcyAmount.Text = Conversion.ToString(row.Cells["TcyAmount"].Value);
				txtLcyAmountDetail.Text = Conversion.ToString(row.Cells["LcyAmount"].Value);
				CmbTaxName.Value = row.Cells["TaxNameId"].Value;
				txtTaxPercnt.Text = Conversion.ToString(row.Cells["Tax%"].Value);
				CmbLocationType.Value = row.Cells["LocationTypeId"].Value;
				CmbLocationType_Leave(null, null);
				CmbLocationFrom.Value = row.Cells["LocationFromId"].Value;
				CmbLocationTo.Value = row.Cells["LocationToId"].Value;
				txtRemarksDetail.Text = Conversion.ToString(row.Cells["Remarks"].Value);
				BtnAddCustomer.Visible = false;
				BtnUpdateCustmer.Visible = true;
				txtPartyRefDocNo.Focus();
				BtnCancelCustomer.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdCustomerDetail.RootTable.Columns["DebitAccountId"], dtDebitAccounts, "Id", "AccountTitle", "Debit Account Title", 150);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetailSetting(GridEX gridEX, bool FreightExists = false)
	{
		try
		{
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == "grdCustomerDetail")
			{
				editableColumns = ((!FreightExists) ? new Dictionary<string, EditType>
				{
					{
						"Qty",
						(EditType)1
					},
					{
						"NetWeight",
						(EditType)1
					},
					{
						"TransactionExchangeRate",
						(EditType)1
					},
					{
						"Remarks",
						(EditType)1
					},
					{
						"Tax%",
						(EditType)(TaxPercntEditableOrNot ? 1 : 0)
					}
				} : new Dictionary<string, EditType>
				{
					{
						"Remarks",
						(EditType)1
					},
					{
						"Tax%",
						(EditType)(TaxPercntEditableOrNot ? 1 : 0)
					}
				});
			}
			LogisticsServicesBill_Helper.DetailGridCommonSetting(gridEX, editableColumns);
			if (FreightExists)
			{
				gridEX.RootTable.Columns["AgreementNo"].Visible = false;
				gridEX.RootTable.Columns["PurchaseOrderNo"].Visible = false;
				gridEX.RootTable.Columns["PartyRefDocNo"].Visible = false;
				gridEX.RootTable.Columns["PartyRefDocDate"].Visible = false;
				gridEX.RootTable.Columns["ServiceRate"].Visible = false;
				gridEX.RootTable.Columns["RateBase"].Visible = false;
			}
			if (((Control)(object)gridEX).Name == "grdCustomerDetail")
			{
				DetailGridComboBind();
				if (FreightExists)
				{
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.RootTable.Columns["LocationType"].EditType = (EditType)0;
					gridEX.RootTable.Columns["LocationFrom"].EditType = (EditType)0;
					gridEX.RootTable.Columns["LocationTo"].EditType = (EditType)0;
				}
				else
				{
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					GridEX_Helper.AddButton(gridEX, "Edit", "Edit", 40, 1);
				}
				gridEX.FrozenColumns = 3;
				ctrlGrdBar3_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdCustomerDetail.UpdateData();
			GridEXRow row = grdCustomerDetail.CurrentRow;
			if (row == null)
			{
				return;
			}
			if (e.Column.Key == "TransactionExchangeRate" || e.Column.Key == "Qty" || e.Column.Key == "NetWeight" || e.Column.Key == "RateBaseEquivalent")
			{
				CustomerDetail_AmountUpdateInRow(row);
				int rateBaseId = Conversion.ToInt(row.Cells["RateBaseId"].Value);
				if (e.Column.Key == "Qty" && rateBaseId == 5)
				{
					MessageBox.Show("Qty Will be 01 In Case Of " + Conversion.ToString(row.Cells["RateBase"].Value));
				}
				if (e.Column.Key == "NetWeight" && (rateBaseId == 3 || rateBaseId == 4))
				{
					MessageBox.Show("NetWeight Is Auto-Calculated In Case Of " + Conversion.ToString(row.Cells["RateBase"].Value));
				}
			}
			if (e.Column.Key == "TcyAmount" || e.Column.Key == "TaxNameId" || e.Column.Key == "Tax%")
			{
				CustomerDetail_TaxUpdateInRow(row);
			}
			grdCustomerDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerDetail_AmountUpdateInRow(GridEXRow row)
	{
		try
		{
			if (row == null)
			{
				return;
			}
			if (FreightVoucherIds.Any())
			{
				decimal TransactionExchangeRate = Conversion.ToDecimal(row.Cells["TransactionExchangeRate"].Value);
				decimal tcyAmount = Conversion.ToDecimal(row.Cells["TcyAmount"].Value);
				row.Cells["LcyAmount"].Value = tcyAmount * TransactionExchangeRate;
			}
			else
			{
				decimal TransactionExchangeRate2 = Conversion.ToDecimal(row.Cells["TransactionExchangeRate"].Value);
				decimal num = Conversion.ToDecimal(row.Cells["ServiceRate"].Value);
				decimal rateBaseEquivalent = Conversion.ToDecimal(row.Cells["RateBaseEquivalent"].Value);
				decimal Qty = Conversion.ToDecimal(row.Cells["Qty"].Value);
				int RateBaseId = Conversion.ToInt(row.Cells["RateBaseId"].Value);
				if (RateBaseId == 3 || RateBaseId == 4)
				{
					decimal NetWeight = ((RateBaseId == 3) ? (Qty * 100m) : Qty);
					row.Cells["NetWeight"].Value = NetWeight;
				}
				if (RateBaseId == 5)
				{
					row.Cells["Qty"].Value = 1;
				}
				decimal tcyAmount2 = num * rateBaseEquivalent * Qty;
				row.Cells["TcyAmount"].Value = tcyAmount2;
				row.Cells["LcyAmount"].Value = tcyAmount2 * TransactionExchangeRate2;
			}
			CustomerDetail_TaxUpdateInRow(row);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerDetail_TaxUpdateInRow(GridEXRow row)
	{
		double num = Conversion.ToDouble(row.Cells["TcyAmount"].Value);
		double taxPercent = 0.0;
		double taxAmount = 0.0;
		double totalAmount = num;
		if (Conversion.ToDouble(row.Cells["Tax%"].Value) > 0.0)
		{
			taxPercent = Conversion.ToDouble(row.Cells["Tax%"].Value);
		}
		else if (Conversion.ToInt(row.Cells["TaxNameId"].Value) > 0)
		{
			taxPercent = Conversion.ToDouble(row.Cells["Tax%"].Value);
		}
		taxAmount = num * taxPercent / 100.0;
		totalAmount = num + taxAmount;
		row.Cells["Tax%"].Value = taxPercent;
		row.Cells["TaxAmount"].Value = Math.Round(taxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
		row.Cells["TotalTcyAmount"].Value = Math.Round(totalAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
	}

	private void AmountUpdateInCustomerDetailGrid()
	{
		try
		{
			GridEXRow[] dataRows = grdCustomerDetail.GetDataRows();
			foreach (GridEXRow row in dataRows)
			{
				row.BeginEdit();
				CustomerDetail_AmountUpdateInRow(row);
				row.EndEdit();
			}
			grdCustomerDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdCustomerDetail.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grdCustomerDetail_DoubleClick(null, null);
				}
				grdCustomerDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteCustomerDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail first..");
				}
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ServicesBillDetail d = new ServicesBillDetail();
					d.ServicesBillDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(d, r);
					d.ActionTypeId = 3;
					lstRemoveRecordcustomerDetail.Add(d);
					dtCustomerDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtCustomerDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			EnableDisableHeaderColumns();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDisableHeaderColumns()
	{
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	private void FromReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			lstRemoveRecordcustomerDetail.Clear();
			FreightVoucherIds.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			((Control)(object)CmbPaymentTerm).Enabled = false;
			txtDueDays.Enabled = false;
			txtrefbillno.Text = string.Empty;
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			((Control)(object)CmbBillToPartyName).Text = string.Empty;
			((Control)(object)CmbServiceProviderOrBrokerAgent).Text = string.Empty;
			txtDueDays.Text = string.Empty;
			txtGlcyRate.Text = string.Empty;
			txtGlcyAmount.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			ResetCustomerDetail();
			dtCustomerDetail.Rows.Clear();
			grdCustomerDetail.DataSource = null;
			dtFreightDetail.Rows.Clear();
			txtdocno.Text = DocumentNoDbCall();
			EnableDisableHeaderColumns();
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetCustomerDetail()
	{
		try
		{
			txtPartyRefDocNo.Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			txtServiceRate.Text = string.Empty;
			((Control)(object)CmbRateBaseType).Text = string.Empty;
			txtTcyAmount.Text = string.Empty;
			txtLcyAmountDetail.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			((Control)(object)CmbLocationType).Text = string.Empty;
			((Control)(object)CmbLocationFrom).Text = string.Empty;
			((Control)(object)CmbLocationTo).Text = string.Empty;
			updateDetailIndex = -1;
			BtnAddCustomer.Visible = true;
			BtnCancelCustomer.Visible = false;
			BtnUpdateCustmer.Visible = false;
			txtPartyRefDocNo.Focus();
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
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AllSerivesItems", "MultiCurrency", "AccountsWithCustomGroupId", "AllCities" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			AllLookUpComboBind(LookUps.LookUpAllServices(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
			SupplierDtFillFromGlobal();
			BrokerAgentOrServiceProviderBind(CmbBillToPartyName, dtSupplier, "Broker Agent");
			BrokerAgentOrServiceProviderBind(CmbServiceProviderOrBrokerAgent, dtServiceProvider, "Service Provider");
			CurrencyBindFromGlobal();
			ItemCategoryOrMasterItemBind();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			GetConfigurationsFromGlobalandBind();
			DetailGridComboBind();
			CmbBrokerAgentName_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmLogisticPurchaseServicesBill_FormClosed(object sender, FormClosedEventArgs e)
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

	private void Insert()
	{
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Expected O, but got Unknown
		try
		{
			if (grdCustomerDetail.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbServiceType, "Service Type", FormHelper.ValidType.INT),
				((Control)(object)CmbPaymentTerm, "Payment Term", FormHelper.ValidType.INT),
				(txtDueDays, "Due Days", FormHelper.ValidType.INT),
				((Control)(object)CmbFcyCurrency, "Foreign Currency", FormHelper.ValidType.INT),
				(txtFcyExchangeRate, "Fcy Rate", FormHelper.ValidType.Double)
			};
			if (HasMultiCurrencyFeature)
			{
				controlsToValidate.Add(((Control)(object)CmbGlcyCurrency, "Glcy Currency", FormHelper.ValidType.INT));
				controlsToValidate.Add((txtGlcyRate, "Glcy Rate", FormHelper.ValidType.Double));
				controlsToValidate.Add((txtGlcyAmount, "Glcy Amount", FormHelper.ValidType.Double));
			}
			if (!FormHelper.ValidateControls(controlsToValidate) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			ServicesBillHeader Or = new ServicesBillHeader();
			Or.ServicesBillHeaderId = RecId;
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchesId = UserAccount.BranchesId;
			Or.ProjectsId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Or.DocumentTypeId = DocumentTypeId;
			Or.DocumentDate = Conversion.ToDateTime((object)txtdocdate.Value);
			Or.DocumentNo = Conversion.ToInt((object)txtdocno.Text);
			Or.serviceTypeId = Conversion.ToInt(CmbServiceType.Value);
			Or.ExportInvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			Or.ExportInvoiceNo = Conversion.ToString((object)((Control)(object)CmbInvoiceNo).Text);
			string ExportInvoiceCustomerName = "";
			DataTable dt = ExImInvoice.GetInvoiceInformationByInvoiceIdForServices(new ExImInvoice
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = Or.ExportInvoiceId
			});
			if (dt.Rows.Count > 0)
			{
				ExportInvoiceCustomerName = Convert.ToString(dt.Rows[0]["CustomerName"]);
			}
			Or.ExportInvoiceCustomerName = ExportInvoiceCustomerName;
			Or.BilltoPartyId = Conversion.ToInt(CmbBillToPartyName.Value);
			Or.BrokerOrServiceProviderId = Conversion.ToInt(CmbServiceProviderOrBrokerAgent.Value);
			Or.ReferenceNo = Conversion.ToString((object)txtrefbillno.Text);
			Or.paymentTermId = Conversion.ToInt(CmbPaymentTerm.Value);
			Or.DueDays = Conversion.ToInt((object)txtDueDays.Text);
			Or.fcyCurrencyId = Conversion.ToInt(CmbFcyCurrency.Value);
			Or.fcyExchangeRate = Conversion.ToDouble((object)txtFcyExchangeRate.Text);
			Or.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			string VoucherIds = string.Join(",", FreightVoucherIds);
			Or.FreightVoucherOutwardIds = VoucherIds;
			Or.GlcyCurrencyId = Conversion.ToInt(CmbGlcyCurrency.Value);
			Or.GlcyExchangeRate = Conversion.ToDouble((object)txtGlcyRate.Text);
			Or.GlcyAmount = Conversion.ToDouble((object)txtGlcyAmount.Text);
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ApprovedUserId = UserAccount.ID;
			Or.ApprovedDate = DateTime.Now;
			Or.IsApproved = false;
			Or.ServicesBillDetailList = new List<ServicesBillDetail>();
			Or.ServicesBillFreightVoucherDetailList = new List<ServicesBillFreightVoucherDetail>();
			if (lstRemoveRecordcustomerDetail.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecordcustomerDetail.Count; i++)
				{
					Or.ServicesBillDetailList.Add(lstRemoveRecordcustomerDetail[i]);
				}
			}
			double TotalGlcyamount = 0.0;
			bool PoExists = false;
			GridEXRow[] rows = grdCustomerDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				ServicesBillDetail vd2 = new ServicesBillDetail();
				vd2.ServicesBillDetailId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd2.ActionTypeId = ((vd2.ServicesBillDetailId <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd2, r);
				if (string.IsNullOrEmpty(Or.FreightVoucherOutwardIds))
				{
					FormHelper.ValidateField(vd2.partyRefDocNo, "Party Ref DocNo", r.RowIndex);
				}
				FormHelper.ValidateField(vd2.serviceItemId, "Service Item", r.RowIndex);
				FormHelper.ValidateField(vd2.transactionCurrencyId, "Transaction Currency", r.RowIndex);
				FormHelper.ValidateField(vd2.transactionExchangeRate, "Transaction Exchange Rate", r.RowIndex);
				if (string.IsNullOrEmpty(Or.FreightVoucherOutwardIds))
				{
					FormHelper.ValidateField(vd2.ServiceRate, "Rate", r.RowIndex);
					FormHelper.ValidateField(vd2.RateBaseUomId, "Rate Base", r.RowIndex);
				}
				FormHelper.ValidateField(vd2.Qty, "Qty", r.RowIndex);
				FormHelper.ValidateField(vd2.NetWeight, "NetWeight", r.RowIndex);
				FormHelper.ValidateField(vd2.tcyAmount, "Tcy amount", r.RowIndex);
				FormHelper.ValidateField(vd2.lcyAmount, "Lcy amount", r.RowIndex);
				if (vd2.TaxNameId > 0 || vd2.TaxPercent > 0.0 || vd2.TaxAmount > 0.0)
				{
					FormHelper.ValidateField(vd2.TaxNameId, "Tax Name", r.RowIndex);
					FormHelper.ValidateField(vd2.TaxPercent, "Tax Percent", r.RowIndex);
					FormHelper.ValidateField(vd2.TaxAmount, "Tax Amount", r.RowIndex);
				}
				FormHelper.ValidateField(vd2.TotalAmount, "Total Tcy Amount", r.RowIndex);
				if (!string.IsNullOrEmpty(Or.FreightVoucherOutwardIds))
				{
					FormHelper.ValidateField(vd2.DebitAccountId, "Debit Account", r.RowIndex);
				}
				if (string.IsNullOrEmpty(Or.FreightVoucherOutwardIds))
				{
					FormHelper.ValidateField(vd2.LocationTypeId, "Location Type", r.RowIndex);
					if (vd2.LocationTypeId == 16)
					{
						FormHelper.ValidateField(vd2.LoadingPortId, "Loading Port", r.RowIndex);
						FormHelper.ValidateField(vd2.DestinationPortId, "Destination Port", r.RowIndex);
						if (vd2.LoadingPortId == vd2.DestinationPortId)
						{
							throw new Exception($"Loading Port And Destination Port Can't be Same in Detail  at row No: {r.RowIndex + 1}");
						}
					}
					if (vd2.LocationTypeId == 17)
					{
						FormHelper.ValidateField(vd2.FromCityId, "From City", r.RowIndex);
						FormHelper.ValidateField(vd2.ToCityId, "To City", r.RowIndex);
						if (vd2.FromCityId == vd2.ToCityId)
						{
							throw new Exception($"FromCity And ToCity Can't be Same in Detail  at row No: {r.RowIndex + 1}");
						}
					}
				}
				if (string.IsNullOrWhiteSpace(Or.RemarksHeader))
				{
					Or.RemarksHeader = vd2.Remarks;
				}
				if (Or.transactionCurrencyId == 0)
				{
					Or.transactionCurrencyId = vd2.transactionCurrencyId;
					Or.transactionExchangeRate = vd2.transactionExchangeRate;
				}
				if (!PoExists)
				{
					PoExists = vd2.PurchaseOrderHeaderId > 0 && vd2.PurchaseOrderDetailId > 0;
				}
				TotalGlcyamount += vd2.GlcyAmount;
				Or.ServicesBillDetailList.Add(vd2);
			}
			if (string.IsNullOrEmpty(Or.FreightVoucherOutwardIds) && !PoExists)
			{
				throw new Exception("At least one detail row must be from a Purchase Order (PO).");
			}
			foreach (DataRow dr in dtFreightDetail.Rows)
			{
				ServicesBillFreightVoucherDetail vd = new ServicesBillFreightVoucherDetail();
				vd.ServicesBillFreightVoucherDetailId = ((RecId != 0) ? Conversion.ToInt(dr["Id"]) : 0);
				FillFreightDetailListCommonForInsertAndDelete(vd, dr);
				Or.ServicesBillFreightVoucherDetailList.Add(vd);
			}
			Or.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			Or.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ServicesBillHeader>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Or.AttachmentsValues = AttachmentsValues;
			Or.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = ServicesBillHeader.Save(Or);
			Or.ServicesBillHeaderId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + Or.DocumentNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + Or.DocumentNo);
			}
			FromReset();
			if (ChkPreview.Checked && formright.DoHavePrintRights)
			{
				ServicesBillHeader_Slip(Or.ServicesBillHeaderId);
			}
			if (chkVoucher.Checked && formright.DoHavePrintRights)
			{
				CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(Or.ServicesBillHeaderId, DocumentTypeId), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
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

	public void ReadById(int Id)
	{
		try
		{
			FromReset();
			RecId = Id;
			ServicesBillHeader Or = ServicesBillHeader.ReadById(RecId);
			if (Or == null || Or.ServicesBillDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocdate.Value = Or.DocumentDate;
			txtdocno.Text = Conversion.ToString((object)Or.DocumentNo);
			CmbServiceType.Value = Conversion.ToInt((object)Or.serviceTypeId);
			InvoiceNoBind(Or.ExportInvoiceId, Or.ExportInvoiceNo, Or.InvoiceAccountId);
			CmbBillToPartyName.Value = Conversion.ToInt((object)Or.BilltoPartyId);
			CmbServiceProviderOrBrokerAgent.Value = Conversion.ToInt((object)Or.BrokerOrServiceProviderId);
			txtrefbillno.Text = Conversion.ToString((object)Or.ReferenceNo);
			CmbPaymentTerm.Value = Conversion.ToInt((object)Or.paymentTermId);
			txtDueDays.Text = Conversion.ToString((object)Or.DueDays);
			CmbFcyCurrency.Value = Conversion.ToInt((object)Or.fcyCurrencyId);
			txtFcyExchangeRate.Text = Or.fcyExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			txtRemarksHeader.Text = Conversion.ToString((object)Or.RemarksHeader);
			CmbGlcyCurrency.Value = Conversion.ToInt((object)Or.GlcyCurrencyId);
			txtGlcyRate.Text = Or.GlcyExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			txtGlcyAmount.Text = Or.GlcyAmount.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			FreightVoucherIds = ((Or.FreightVoucherOutwardIds != null) ? (from id in Or.FreightVoucherOutwardIds.Split(',')
				select Conversion.ToInt((object)id)).ToHashSet() : new HashSet<int>());
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			dtCustomerDetail.Rows.Clear();
			LogisticsServicesBill_Helper.FillDetailFromListCommonForReadById(Or, dtCustomerDetail);
			dtFreightDetail.Rows.Clear();
			LogisticsServicesBill_Helper.FillFreightVoucherDetailFromListCommonForReadById(Or, dtFreightDetail);
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			txtdocdate.Focus();
			EnableDisableHeaderColumns();
			txtGlcyRate_TextChanged(null, null);
			((Control)(object)CmbPaymentTerm).Enabled = FreightVoucherIds.Any();
			txtDueDays.Enabled = FreightVoucherIds.Any();
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
					ServicesBillHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					FromReset();
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

	private void FillDetailListCommonForInsertAndDelete(ServicesBillDetail vd, GridEXRow r)
	{
		vd.ChargesTypeId = Conversion.ToInt(r.Cells["ChargesTypeId"].Value);
		vd.AgreementHeaderId = Conversion.ToInt(r.Cells["AgreementHeaderId"].Value);
		vd.AgreementDetailId = Conversion.ToInt(r.Cells["AgreementDetailId"].Value);
		vd.AgreementDocNo = Conversion.ToInt(r.Cells["AgreementNo"].Value);
		vd.PurchaseOrderHeaderId = Conversion.ToInt(r.Cells["PurchaseOrderHeaderId"].Value);
		vd.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["PurchaseOrderDetailId"].Value);
		vd.PurchaseOrderDocNo = Conversion.ToInt(r.Cells["PurchaseOrderNo"].Value);
		vd.partyRefDocNo = Conversion.ToString(r.Cells["PartyRefDocNo"].Value);
		vd.partyRefDocDate = Conversion.ToDateTime(r.Cells["PartyRefDocDate"].Value);
		vd.serviceItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ServiceItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ServiceItemName"].Value);
		vd.transactionCurrencyId = Conversion.ToInt(r.Cells["TransactionCurrencyId"].Value);
		vd.TransactionCurrencyCode = Conversion.ToString(r.Cells["TransactionCurrency"].Value);
		vd.transactionExchangeRate = Conversion.ToDouble(r.Cells["TransactionExchangeRate"].Value);
		vd.ServiceRate = Conversion.ToDouble(r.Cells["ServiceRate"].Value);
		vd.RateBaseUomId = Conversion.ToInt(r.Cells["RateBaseId"].Value);
		vd.RateBaseUom = Conversion.ToString(r.Cells["RateBase"].Value);
		vd.RateBaseUomEquivalent = Conversion.ToDouble(r.Cells["RateBaseEquivalent"].Value);
		vd.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
		vd.NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
		vd.tcyAmount = Conversion.ToDouble(r.Cells["TcyAmount"].Value);
		vd.lcyAmount = Conversion.ToDouble(r.Cells["LcyAmount"].Value);
		vd.FcyCurrencyId = Conversion.ToInt(CmbFcyCurrency.Value);
		vd.FcyCurrencyCode = Conversion.ToString((object)((Control)(object)CmbFcyCurrency).Text);
		double FcyExchangeRate = ((vd.FcyCurrencyId == vd.transactionCurrencyId) ? vd.transactionExchangeRate : Conversion.ToDouble((object)txtFcyExchangeRate.Text));
		vd.FcyExchangeRate = FcyExchangeRate;
		vd.fcyAmount = ((vd.FcyExchangeRate > 0.0) ? (vd.lcyAmount / vd.FcyExchangeRate) : 0.0);
		vd.GlcyCurrencyId = Conversion.ToInt(CmbGlcyCurrency.Value);
		vd.GlcyCurrencyCode = Conversion.ToString((object)((Control)(object)CmbGlcyCurrency).Text);
		vd.GlcyExchangeRate = ((vd.GlcyCurrencyId == vd.transactionCurrencyId) ? vd.transactionExchangeRate : Conversion.ToDouble((object)txtGlcyRate.Text));
		vd.GlcyAmount = ((vd.GlcyExchangeRate > 0.0) ? (vd.lcyAmount / vd.GlcyExchangeRate) : 0.0);
		vd.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
		vd.TaxPercent = Conversion.ToDouble(r.Cells["Tax%"].Value);
		vd.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
		vd.TotalAmount = Conversion.ToDouble(r.Cells["TotalTcyAmount"].Value);
		vd.DebitAccountId = Conversion.ToInt(r.Cells["DebitAccountId"].Value);
		vd.DebitAccountTitle = Conversion.ToString((object)r.Cells["DebitAccountId"].Text);
		vd.LocationTypeId = Conversion.ToInt(r.Cells["LocationTypeId"].Value);
		vd.LocationType = Conversion.ToString(r.Cells["LocationType"].Value);
		if (vd.LocationTypeId == 16)
		{
			vd.LoadingPortId = Conversion.ToInt(r.Cells["LocationFromId"].Value);
			vd.LoadingPort = Conversion.ToString(r.Cells["LocationFrom"].Value);
			vd.DestinationPortId = Conversion.ToInt(r.Cells["LocationToId"].Value);
			vd.DestinationPort = Conversion.ToString(r.Cells["LocationTo"].Value);
		}
		else
		{
			vd.FromCityId = Conversion.ToInt(r.Cells["LocationFromId"].Value);
			vd.FromCity = Conversion.ToString(r.Cells["LocationFrom"].Value);
			vd.ToCityId = Conversion.ToInt(r.Cells["LocationToId"].Value);
			vd.ToCity = Conversion.ToString(r.Cells["LocationTo"].Value);
		}
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void FillFreightDetailListCommonForInsertAndDelete(ServicesBillFreightVoucherDetail vd, DataRow r)
	{
		vd.FreightVoucherOutwardId = Conversion.ToInt(r["FreightVoucherOutwardId"]);
		vd.GatePassOutwardId = Conversion.ToInt(r["GatePassOutwardId"]);
		vd.DeliveryOrderId = Conversion.ToInt(r["DeliveryOrderId"]);
		vd.Qty = Conversion.ToDouble(r["Qty"]);
		vd.Weight = Conversion.ToDouble(r["Weight"]);
		vd.FreightRate = Conversion.ToDouble(r["FreightRate"]);
		vd.RateBaseUomId = Conversion.ToInt(r["RateBaseUomId"]);
		vd.QtyForRate = Conversion.ToDouble(r["QtyForRate"]);
		vd.BiltyFreight = Conversion.ToDouble(r["BiltyFreight"]);
		vd.OtherCharges = Conversion.ToDouble(r["OtherCharges"]);
		vd.TotalBiltyFreight = Conversion.ToDouble(r["TotalBiltyFreight"]);
		vd.ThisRowFreightAmount = Conversion.ToDouble(r["ThisRowFreightAmount"]);
		vd.Remarks = Conversion.ToString(r["Remarks"]);
	}

	private void txtTransactionExchangeRateDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmountsInDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtServiceRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtNetWeight.Enabled = true;
			int Id = Conversion.ToInt(CmbRateBaseType.Value);
			if (Id == 3 || Id == 4)
			{
				txtNetWeight.Enabled = false;
				double Weight = ((Id == 3) ? (Conversion.ToDouble((object)txtQty.Text) * 1000.0) : Conversion.ToDouble((object)txtQty.Text));
				txtNetWeight.Text = Weight.ToString("#,##.##");
			}
			CalculateAmountsInDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateBaseType_Leave(object sender, EventArgs e)
	{
		try
		{
			txtNetWeight.Enabled = true;
			txtQty.Enabled = true;
			int Id = Conversion.ToInt(CmbRateBaseType.Value);
			if (Id == 3 || Id == 4)
			{
				txtNetWeight.Enabled = false;
				double Weight = ((Id == 3) ? (Conversion.ToDouble((object)txtQty.Text) * 1000.0) : Conversion.ToDouble((object)txtQty.Text));
				txtNetWeight.Text = Weight.ToString("#,##.##");
			}
			if (Id == 5)
			{
				txtQty.Text = 1.ToString("#,##.##");
				txtQty.Enabled = false;
			}
			CalculateAmountsInDetail();
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
			CalculateAmountsInDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateAmountsInDetail()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtServiceRate.Text);
			double num2;
			if (Conversion.ToInt(CmbRateBaseType.Value) <= 0)
			{
				num2 = 0.0;
			}
			else
			{
				UltraGridRow selectedRow = ((UltraDropDownBase)CmbRateBaseType).SelectedRow;
				num2 = Conversion.ToDouble((selectedRow != null) ? selectedRow.Cells[2].Value : null);
			}
			double rateBaseEquivalent = num2;
			double Qty = Conversion.ToDouble((object)txtQty.Text);
			double tcyAmount = num * rateBaseEquivalent * Qty;
			txtTcyAmount.Text = tcyAmount.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			double tcyExchangeRate = Conversion.ToDouble((object)txtTransactionExchangeRateDetail.Text);
			double lcyAmount = ((tcyExchangeRate > 0.0) ? (tcyAmount * tcyExchangeRate) : 0.0);
			txtLcyAmountDetail.Text = lcyAmount.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTaxAmount()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtTcyAmount.Text);
			double TaxPercent = 0.0;
			double TaxAmt = 0.0;
			double TotalAmount = 0.0;
			if (((UltraGridBase)CmbTaxName).ActiveRow != null)
			{
				TaxPercent = ((Conversion.ToDouble((object)txtTaxPercnt.Text) > 0.0) ? Conversion.ToDouble((object)txtTaxPercnt.Text) : ((((UltraDropDownBase)CmbTaxName).SelectedRow != null) ? Conversion.ToDouble(((UltraDropDownBase)CmbTaxName).SelectedRow.Cells[2].Value) : 0.0));
			}
			TaxAmt = num * TaxPercent / 100.0;
			TotalAmount = num + TaxAmt;
			txtTaxPercnt.Text = TaxPercent.ToString("#,##.###");
			txtTaxAmount.Text = Math.Round(TaxAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			txtTotalAmount.Text = Math.Round(TotalAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxName_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercnt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGlcyRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double LcyAmount = GridEX_Helper.GetColumnSum(grdCustomerDetail, "LcyAmount");
			double glcyRate = Conversion.ToDouble((object)txtGlcyRate.Text);
			double glcyAmount = ((glcyRate > 0.0) ? (LcyAmount / glcyRate) : 0.0);
			txtGlcyAmount.Text = glcyAmount.ToString(clsGlobalVariables.DecimalFCYRateFormate);
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
			ServicesBillHeader_Slip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ServicesBillHeader_Slip(int PrintId)
	{
		try
		{
			CrystalReportPrint_Helper.ServicesBillHeader_Slip_1304(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(RecId, DocumentTypeId), DocumentTypeId);
				return;
			}
			throw new Exception("Record Not Found For Display");
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
			cmbDateTypeHistory.Focus();
		}
		else
		{
			txtdocdate.Focus();
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			return ServicesBillHeader.GetDataForDropDownFromServicesBillHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = DocumentTypeId.ToString()
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			DataTable dtStatus = dtST.Clone();
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ServiceType")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BrokerOrServiceProvider")
				{
					dtStatus.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbServiceTypeHistory, dtST, "Id", "Name", "Service Type", AllColumns: true, Conversion.ToInt(CmbServiceTypeHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbServiceProviderHistory, dtStatus, "Id", "Name", "Service Provider", AllColumns: true, Conversion.ToInt(CmbServiceProviderHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbDateTypeHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				FromDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				FromDateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDateHistory.Value = first;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				FromDateHistory.Value = first2;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				FromDateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)CmbServiceTypeHistory).Text = string.Empty;
			((Control)(object)CmbServiceProviderHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdHistoryDetail.DataSource = null;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		Datetypefill();
		HistoryComboBind(HistoryComboDbCall());
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			obj.EntryUser = UserAccount.ID;
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
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.TypeId = Conversion.ToInt(CmbServiceTypeHistory.Value);
			obj.ServiceProviderId = Conversion.ToInt(CmbServiceProviderHistory.Value);
			DataTable dtHistory = ServicesBillHeader.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dataTable = dtHistoryGrid;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("DocumentNo", typeof(int));
					dtHistoryGrid.Columns.Add("DocumentDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ServiceTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("ServiceType", typeof(string));
					dtHistoryGrid.Columns.Add("ExportInvoiceId", typeof(int));
					dtHistoryGrid.Columns.Add("ExportInvoiceNo", typeof(string));
					dtHistoryGrid.Columns.Add("BillToPartyId", typeof(int));
					dtHistoryGrid.Columns.Add("BillToPartyName", typeof(string));
					dtHistoryGrid.Columns.Add("BrokerOrServiceProviderId", typeof(int));
					dtHistoryGrid.Columns.Add("BrokerAgentOrServiceProvider", typeof(string));
					dtHistoryGrid.Columns.Add("ReferenceNo", typeof(string));
					dtHistoryGrid.Columns.Add("PaymentTermId", typeof(int));
					dtHistoryGrid.Columns.Add("PaymentTerm", typeof(string));
					dtHistoryGrid.Columns.Add("DueDays", typeof(int));
					dtHistoryGrid.Columns.Add("FcyCurrencyId", typeof(int));
					dtHistoryGrid.Columns.Add("FcyCode", typeof(string));
					dtHistoryGrid.Columns.Add("FcyExchangeRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("GlcyCurrencyCode", typeof(string));
					dtHistoryGrid.Columns.Add("GlcyExchangeRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("GlcyAmount", typeof(decimal));
					dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUserName", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUserName", typeof(string));
					dtHistoryGrid.Columns.Add("IsApproved", typeof(bool));
					dtHistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ApprovalUserName", typeof(string));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
					dtHistoryGrid.Columns.Add("FreightVoucherOutwardIds", typeof(string));
				}
				dtHistoryGrid.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtHistoryGrid.Rows.Add(row["ServicesBillHeaderId"], row["DocumentTypeId"], row["DocumentNo"], row["DocumentDate"], row["ServiceTypeId"], row["ServiceType"], row["ExportInvoiceId"], row["ExportInvoiceNo"], row["BillToPartyId"], row["BillToPartyName"], row["BrokerOrServiceProviderId"], row["BrokerAgentOrServiceProvider"], row["ReferenceNo"], row["PaymentTermId"], row["PaymentTerm"], row["DueDays"], row["FcyCurrencyId"], row["FcyCode"], row["FcyExchangeRate"], row["GlcyCurrencyCode"], row["GlcyExchangeRate"], row["GlcyAmount"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovalUserName"], row["NoOfAttachments"], row["FreightVoucherOutwardIds"]);
				}
				grdHistory.DataSource = dtHistoryGrid;
				grdHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				grdHistory.DataSource = null;
				grdHistoryDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "ServiceTypeId", "BrokerOrServiceProviderId", "BillToPartyId", "TransactionCurrencyId", "FcyCurrencyId", "ExportInvoiceId", "PaymentTermId", "FreightVoucherOutwardIds" };
			if (!HasMultiCurrencyFeature)
			{
				hiddenColumns.AddRange(new string[3] { "GlCurrencyCode", "GlcyExchangeRate", "GlcyAmount" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocumentDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["DocumentNo"].Width = Constants.InventoryConstants.DocNoConstant + 20;
			grdHistory.RootTable.Columns["DocumentDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ServiceType"].Width = 150;
			grdHistory.RootTable.Columns["ExportInvoiceNo"].Width = 120;
			grdHistory.RootTable.Columns["BillToPartyName"].Width = 180;
			grdHistory.RootTable.Columns["BrokerAgentOrServiceProvider"].Width = 180;
			grdHistory.RootTable.Columns["ReferenceNo"].Width = 120;
			grdHistory.RootTable.Columns["FcyCode"].Width = 60;
			grdHistory.RootTable.Columns["FcyExchangeRate"].Width = 90;
			grdHistory.RootTable.Columns["GlcyCurrencyCode"].Width = 60;
			grdHistory.RootTable.Columns["GlcyExchangeRate"].Width = 90;
			grdHistory.RootTable.Columns["GlcyAmount"].Width = 120;
			grdHistory.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ApprovalUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 50, grdHistory.RootTable.Columns["DocumentNo"].Position - 1);
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 50, grdHistory.RootTable.Columns["DocumentNo"].Position - 1);
			GridEX_Helper.AddButton(grdHistory, "Voucher", "Voucher", 70, grdHistory.RootTable.Columns["DocumentNo"].Position - 1);
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				if (!formright.DoHavePrintRights)
				{
					throw new Exception("you don't have print rights...");
				}
				ServicesBillHeader_Slip(Conversion.ToInt(r.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				if (!formright.DoHavePrintRights)
				{
					throw new Exception("you don't have print rights...");
				}
				CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value)));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				bool FreightExists = !string.IsNullOrEmpty(Conversion.ToString(r.Cells["FreightVoucherOutwardIds"].Value));
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value), FreightExists);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id, bool FreightExists)
	{
		try
		{
			ServicesBillHeader Obj = ServicesBillHeader.ReadById(Id);
			if (Obj != null && Obj.ServicesBillDetailList.Count == 0)
			{
				grdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtCustomerDetail.Clone();
			dtDetailHistory.Columns.Add("DebitAccountTitle");
			LogisticsServicesBill_Helper.FillDetailFromListCommonForReadById(Obj, dtDetailHistory, AddExtraColumns: true);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			grddetailhistorySettings(FreightExists);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings(bool FreightExists)
	{
		try
		{
			grdCustomerDetailSetting(grdHistoryDetail, FreightExists);
			grdHistoryDetail.RootTable.Columns["DebitAccountId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["DebitAccountTitle"].Position = grdHistoryDetail.RootTable.Columns["DebitAccountId"].Position + 1;
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		if (FormInitialized)
		{
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdCustomerDetail, dtCustomerDetail);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
					txtdocdate.Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
				{
					if (formright.DoHaveCanDelete && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnPrint.Visible && btnPrint.Enabled)
					{
						btnPrint_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdCustomerDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					txtdocdate.Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistory).Focus();
				}
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Doc Date");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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

	private void grdCustomerDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdCustomerDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdCustomerDetail.CurrentColumn.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				grdCustomerDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteCustomerDetailRow(r);
				grdCustomerDetail.UpdateData();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string NameCode = "";
			decimal Name = default(decimal);
			if (!FreightVoucherIds.Any() && (grdCustomerDetail.CurrentColumn.Key == "TransactionCurrencyId" || grdCustomerDetail.CurrentColumn.Key == "TransactionCurrency"))
			{
				GrdPopUp obj5 = new GrdPopUp(dtCurrency, "Id", "CurrencyCode");
				obj5.StartPosition = FormStartPosition.CenterScreen;
				obj5.ShowDialog();
				obj5.BringToFront();
				Id = Conversion.ToInt((object)obj5.ReturnId);
				if (Id > 0)
				{
					NameCode = Conversion.ToString((object)obj5.ReturnName);
					r.Cells["TransactionCurrencyId"].Value = Id;
					r.Cells["TransactionCurrency"].Value = NameCode;
				}
			}
			if (grdCustomerDetail.CurrentColumn.Key == "TaxNameId" || grdCustomerDetail.CurrentColumn.Key == "TaxName")
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (ItemId == 0)
				{
					throw new Exception("Service Item Not Found In Current Row");
				}
				GrdPopUp obj4 = new GrdPopUp(TaxTypeDbCall(ItemId, txtdocdate.Value), "Id", "TaxType", "TaxPrcnt");
				obj4.StartPosition = FormStartPosition.CenterScreen;
				obj4.ShowDialog();
				obj4.BringToFront();
				Id = Conversion.ToInt((object)obj4.ReturnId);
				if (Id > 0)
				{
					NameCode = Conversion.ToString((object)obj4.ReturnName);
					Name = Conversion.ToDecimal((object)obj4.ReturnCode);
					r.Cells["TaxNameId"].Value = Id;
					r.Cells["TaxName"].Value = NameCode;
					r.Cells["Tax%"].Value = Name;
					CustomerDetail_TaxUpdateInRow(r);
				}
			}
			if (grdCustomerDetail.CurrentColumn.Key == "DebitAccountId")
			{
				GrdPopUp grdPopUp = new GrdPopUp(dtDebitAccounts, "Id", "AccountTitle", "AccountCode");
				grdPopUp.StartPosition = FormStartPosition.CenterScreen;
				grdPopUp.ShowDialog();
				grdPopUp.BringToFront();
				Id = Conversion.ToInt((object)grdPopUp.ReturnId);
				if (Id > 0)
				{
					r.Cells["DebitAccountId"].Value = Id;
				}
			}
			if (grdCustomerDetail.CurrentColumn.Key == "LocationTypeId" || grdCustomerDetail.CurrentColumn.Key == "LocationType")
			{
				int num = Conversion.ToInt(r.Cells["LocationTypeId"].Value);
				GrdPopUp obj3 = new GrdPopUp(dtLocationType, "Id", "Name");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				if (Id > 0)
				{
					NameCode = Conversion.ToString((object)obj3.ReturnName);
					r.Cells["LocationTypeId"].Value = Id;
					r.Cells["LocationType"].Value = NameCode;
				}
				if (num != Id)
				{
					r.Cells["LocationFromId"].Value = 0;
					r.Cells["LocationFrom"].Value = "";
					r.Cells["LocationToId"].Value = 0;
					r.Cells["LocationTo"].Value = "";
				}
			}
			if (grdCustomerDetail.CurrentColumn.Key == "LocationFromId" || grdCustomerDetail.CurrentColumn.Key == "LocationFrom")
			{
				int LocationTypeId2 = Conversion.ToInt(r.Cells["LocationTypeId"].Value);
				if (LocationTypeId2 == 0)
				{
					throw new Exception("Location Type Not Found In Current Row");
				}
				LocationdtFill(LocationTypeId2);
				GrdPopUp obj2 = new GrdPopUp(dtLocations, "Id", "LocationName");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				if (Id > 0)
				{
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					Name = Conversion.ToDecimal((object)obj2.ReturnCode);
					r.Cells["LocationFromId"].Value = Id;
					r.Cells["LocationFrom"].Value = NameCode;
				}
			}
			if (grdCustomerDetail.CurrentColumn.Key == "LocationToId" || grdCustomerDetail.CurrentColumn.Key == "LocationTo")
			{
				int LocationTypeId = Conversion.ToInt(r.Cells["LocationTypeId"].Value);
				if (LocationTypeId == 0)
				{
					throw new Exception("Location Type Not Found In Current Row");
				}
				LocationdtFill(LocationTypeId);
				GrdPopUp obj = new GrdPopUp(dtLocations, "Id", "LocationName");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				if (Id > 0)
				{
					NameCode = Conversion.ToString((object)obj.ReturnName);
					Name = Conversion.ToDecimal((object)obj.ReturnCode);
					r.Cells["LocationToId"].Value = Id;
					r.Cells["LocationTo"].Value = NameCode;
				}
			}
			grdCustomerDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)CurrId));
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					if (!formright.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					ServicesBillHeader_Slip(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					if (!formright.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(CurrId, Conversion.ToInt(r.Cells["DocumentTypeId"].Value)));
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)CurrId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAgreement_Click(object sender, EventArgs e)
	{
		try
		{
			if (FreightVoucherIds.Any())
			{
				MessageBox.Show("Data Already loaded from 'Freight Voucher Export'. You cannot load from Order until you remove those rows.");
				return;
			}
			using frmLoadPurchaseOrderForServicesBill loadSo = new frmLoadPurchaseOrderForServicesBill(UserAccount);
			loadSo.ShowDialog();
			if (loadSo.dtLoader != null && loadSo.dtLoader.Rows.Count > 0)
			{
				LoadInGridDetail(loadSo.dtLoader);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				if (grdCustomerDetail.GetDataRows().Any())
				{
					GridEXRow existingRow = grdCustomerDetail.GetDataRows().FirstOrDefault((GridEXRow r) => Conversion.ToInt(r.Cells["PurchaseOrderHeaderId"].Value) > 0);
					if (existingRow != null)
					{
						int num = Conversion.ToInt(existingRow.Cells["PurchaseOrderHeaderId"].Value);
						int incomingPurchaseOrderId = Conversion.ToInt(dt.Rows[0]["PurchaseOrderHeaderId"]);
						if (num != incomingPurchaseOrderId)
						{
							throw new Exception("Already loaded rows belong to a different PurchaseOrder. You cannot load rows from another PurchaseOrder.");
						}
					}
				}
				CmbServiceType.Value = Conversion.ToInt(dt.Rows[0]["ServiceTypeId"]);
				InvoiceNoBind(Conversion.ToInt(dt.Rows[0]["ExportInvoiceId"]), Conversion.ToString(dt.Rows[0]["ExportInvoiceNo"]), Conversion.ToInt(dt.Rows[0]["InvoiceAccountId"]));
				CmbBillToPartyName.Value = Conversion.ToInt(dt.Rows[0]["brokerAgentId"]);
				CmbServiceProviderOrBrokerAgent.Value = Conversion.ToInt(dt.Rows[0]["serviceProviderId"]);
				if (Conversion.ToInt(CmbFcyCurrency.Value) == 0)
				{
					CmbFcyCurrency.Value = Conversion.ToInt(dt.Rows[0]["fcyCurrencyId"]);
				}
				if (Conversion.ToInt(CmbFcyCurrency.Value) != 0 && Conversion.ToDouble((object)txtFcyExchangeRate.Text) == 0.0)
				{
					txtFcyExchangeRate.Text = Conversion.ToDouble(dt.Rows[0]["FcyExchangeRate"]).ToString(clsGlobalVariables.DecimalFCYRateFormate);
				}
				CmbPaymentTerm.Value = Conversion.ToInt(dt.Rows[0]["paymentTermId"]);
				txtDueDays.Text = Conversion.ToDouble(dt.Rows[0]["DueDays"]).ToString("#,#0");
				txtGlcyRate.Text = Conversion.ToDouble(dt.Rows[0]["GlcyExchangeRate"]).ToString("#,#0");
				HashSet<int> existingDetailIds = new HashSet<int>(from r in grdCustomerDetail.GetRows()
					select Conversion.ToInt(r.Cells["PurchaseOrderDetailId"].Value));
				foreach (DataRow dr in dt.Rows)
				{
					int PurchaseOrderDetailId = Conversion.ToInt(dr["DetailId"]);
					if (existingDetailIds.Add(PurchaseOrderDetailId))
					{
						DataRow newRow = dtCustomerDetail.NewRow();
						newRow["Id"] = 0;
						newRow["ChargesTypeId"] = 18;
						newRow["AgreementHeaderId"] = dr["AgreementHeaderId"];
						newRow["AgreementDetailId"] = dr["AgreementDetailId"];
						newRow["PurchaseOrderHeaderId"] = dr["PurchaseOrderHeaderId"];
						newRow["PurchaseOrderDetailId"] = PurchaseOrderDetailId;
						newRow["PurchaseOrderNo"] = dr["DocumentNo"];
						newRow["PartyRefDocNo"] = dr["PartyRefDocNo"];
						newRow["PartyRefDocDate"] = dr["PartyRefDocDate"];
						newRow["ItemId"] = dr["ServiceItemId"];
						newRow["ServiceItemCode"] = dr["ItemCode"];
						newRow["ServiceItemName"] = dr["ItemName"];
						newRow["TransactionCurrencyId"] = dr["transactionCurrencyId"];
						newRow["TransactionCurrency"] = dr["TransactionCurrencyCode"];
						newRow["TransactionExchangeRate"] = dr["TransactionExchangeRate"];
						newRow["ServiceRate"] = dr["OrderRate"];
						newRow["RateBaseId"] = dr["RateBaseUomId"];
						newRow["RateBase"] = dr["RateBaseUom"];
						newRow["RateBaseEquivalent"] = dr["RateBaseUomEquivalent"];
						newRow["Qty"] = dr["BalQty"];
						newRow["NetWeight"] = dr["BalWeight"];
						double tcyAmount = Conversion.ToDouble(dr["TcyAmount"]);
						newRow["TcyAmount"] = tcyAmount;
						newRow["LcyAmount"] = dr["LcyAmount"];
						double taxPercent = (dr.Table.Columns.Contains("TaxPercent") ? Conversion.ToDouble(dr["TaxPercent"]) : 0.0);
						double taxAmount = tcyAmount * taxPercent / 100.0;
						double totalTcyAmount = tcyAmount + taxAmount;
						newRow["TaxNameId"] = (dr.Table.Columns.Contains("TaxNameId") ? dr["TaxNameId"] : ((object)0));
						newRow["TaxName"] = (dr.Table.Columns.Contains("TaxName") ? dr["TaxName"] : string.Empty);
						newRow["Tax%"] = taxPercent;
						newRow["TaxAmount"] = taxAmount;
						newRow["TotalTcyAmount"] = totalTcyAmount;
						int LocationTypeId = Conversion.ToInt(dr["LocationTypeId"]);
						newRow["LocationTypeId"] = LocationTypeId;
						newRow["LocationType"] = Conversion.ToString(dr["LocationType"]);
						newRow["LocationFromId"] = ((LocationTypeId == 16) ? dr["LoadingPortId"] : dr["FromCityId"]);
						newRow["LocationFrom"] = ((LocationTypeId == 16) ? dr["LoadingPort"] : dr["FromCity"]);
						newRow["LocationToId"] = ((LocationTypeId == 16) ? dr["DestinationPortId"] : dr["ToCityId"]);
						newRow["LocationTo"] = ((LocationTypeId == 16) ? dr["DestinationPort"] : dr["ToCity"]);
						newRow["Remarks"] = dr["Remarks"];
						dtCustomerDetail.Rows.Add(newRow);
					}
				}
			}
			BindGrids();
			EnableDisableHeaderColumns();
			CmbBrokerAgentName_Leave(null, null);
			txtGlcyRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadFreightVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdCustomerDetail.RowCount > 0 && grdCustomerDetail.GetDataRows().FirstOrDefault((GridEXRow r) => Conversion.ToInt(r.Cells["PurchaseOrderHeaderId"].Value) > 0) != null)
			{
				MessageBox.Show("Data already loaded from 'Order'. You cannot load from Freight Voucher until you remove those rows.");
				return;
			}
			using frmLoadPendingFreightVoucherExportForServicesBill loadSo = new frmLoadPendingFreightVoucherExportForServicesBill(UserAccount);
			loadSo.FreightVoucherIds = FreightVoucherIds;
			loadSo.ShowDialog();
			if (loadSo.dtLoader != null && loadSo.dtLoader.Rows.Count > 0)
			{
				FreightVoucherDataFromLoader(loadSo.dtLoader);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightVoucherDataFromLoader(DataTable dtLoader)
	{
		try
		{
			if (dtLoader == null || dtLoader.Rows.Count == 0)
			{
				return;
			}
			if (grdCustomerDetail.GetDataRows().Any())
			{
				int incomingInvoiceId = Conversion.ToInt(dtLoader.Rows[0]["ExImInvoiceId"]);
				if (Conversion.ToInt(CmbInvoiceNo.Value) != incomingInvoiceId)
				{
					throw new Exception("Already loaded rows belong to a different Invoice. You cannot load rows from another Invoice.");
				}
			}
			foreach (DataRow row in dtLoader.Rows)
			{
				int freightVoucherId = Conversion.ToInt(row["Id"]);
				FreightVoucherIds.Add(freightVoucherId);
			}
			CmbServiceType.Value = 3;
			int invoiceId = Conversion.ToInt(dtLoader.Rows[0]["ExImInvoiceId"]);
			int InvoiceAccountId = Conversion.ToInt(dtLoader.Rows[0]["InvoiceAccountId"]);
			string exportInvoiceNo = Conversion.ToString(dtLoader.Rows[0]["ExportInvoiceNo"]);
			InvoiceNoBind(invoiceId, exportInvoiceNo, InvoiceAccountId);
			CmbBillToPartyName.Value = Conversion.ToInt(dtLoader.Rows[0]["BillToPartyId"]);
			LoadInGridDetailFromVoucherByIds();
			VoucherSummaryInfoFillInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetailFromVoucherByIds()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			DataTable dt = FreightVoucherOutward.FreightVoucherOutward_PendingForLogisticServicesBill(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1305,
				ExImInvoiceId = Conversion.ToInt(CmbInvoiceNo.Value),
				Ids = string.Join(",", FreightVoucherIds)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				throw new Exception("No pending freight voucher data found.");
			}
			HashSet<int> existingDetailIds = new HashSet<int>(from r in dtFreightDetail.AsEnumerable()
				select r.Field<int?>("FreightVoucherOutwardId").GetValueOrDefault());
			foreach (DataRow dr in dt.Rows)
			{
				int Id = Conversion.ToInt(dr["Id"]);
				if (existingDetailIds.Add(Id))
				{
					DataRow newRow = dtFreightDetail.NewRow();
					newRow["Id"] = 0;
					newRow["FreightVoucherOutwardId"] = Id;
					newRow["GatePassOutwardId"] = dr["GatePassOutwardId"];
					newRow["DeliveryOrderId"] = dr["InvDeliveryOrderId"];
					double Qty = Conversion.ToDouble(dr["DoQty"]);
					double weight = Conversion.ToDouble(dr["DoWeight"]);
					newRow["Qty"] = Qty;
					newRow["Weight"] = weight;
					double freightRate = Conversion.ToDouble(dr["freightRate"]);
					newRow["FreightRate"] = freightRate;
					newRow["RateBaseUomId"] = dr["rateUomId"];
					double qtyForRate = Conversion.ToDouble(dr["QtyforRate"]);
					newRow["QtyForRate"] = qtyForRate;
					double biltyFreight = Conversion.ToDouble(dr["biltyFreight"]);
					double otherCharges = Conversion.ToDouble(dr["otherCharges"]);
					newRow["BiltyFreight"] = biltyFreight;
					newRow["OtherCharges"] = otherCharges;
					newRow["TotalBiltyFreight"] = Conversion.ToDouble(dr["totalbiltyfreight"]);
					newRow["ThisRowFreightAmount"] = Conversion.ToDouble(dr["ProportionatedBiltyFreight"]);
					newRow["Remarks"] = "";
					dtFreightDetail.Rows.Add(newRow);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherSummaryInfoFillInGrid()
	{
		try
		{
			if (dtFreightDetail == null || dtFreightDetail.Rows.Count == 0)
			{
				throw new Exception("No pending freight voucher data found.");
			}
			double qty = dtFreightDetail.AsEnumerable().Sum((DataRow r) => r.Field<double?>("Qty").GetValueOrDefault());
			double Weight = dtFreightDetail.AsEnumerable().Sum((DataRow r) => r.Field<double?>("Weight").GetValueOrDefault());
			double amount = dtFreightDetail.AsEnumerable().Sum((DataRow r) => r.Field<double?>("ThisRowFreightAmount").GetValueOrDefault());
			getGlobalAllSerivesItems serviceItem = clsGlobalVariables.getGlobalAllSerivesItems.FirstOrDefault((getGlobalAllSerivesItems r) => r.Id == DefaultTranspotationServiceItemId);
			MultiCurrency currency = clsGlobalVariables.globalMultiCurrency.FirstOrDefault((MultiCurrency r) => r.Id == LocalBaseCurrency);
			int InvoiceAcId = Conversion.ToInt(((UltraDropDownBase)CmbInvoiceNo).SelectedRow.Cells["InvoiceAccountId"].Value);
			DataRow drDetail;
			if (dtCustomerDetail.Rows.Count > 0)
			{
				drDetail = dtCustomerDetail.Rows[0];
			}
			else
			{
				drDetail = dtCustomerDetail.NewRow();
				dtCustomerDetail.Rows.Add(drDetail);
			}
			drDetail["ChargesTypeId"] = 19;
			drDetail["ItemId"] = DefaultTranspotationServiceItemId;
			drDetail["ServiceItemCode"] = ((serviceItem != null) ? serviceItem.ItemCode : null);
			drDetail["ServiceItemName"] = ((serviceItem != null) ? serviceItem.ItemName : null);
			drDetail["TransactionCurrencyId"] = LocalBaseCurrency;
			drDetail["TransactionCurrency"] = ((currency != null) ? currency.CurrencyCode : null);
			drDetail["TransactionExchangeRate"] = LocalBaseCurrencyRate;
			drDetail["Qty"] = qty;
			drDetail["NetWeight"] = Weight;
			drDetail["TcyAmount"] = amount;
			drDetail["LcyAmount"] = amount * LocalBaseCurrencyRate;
			drDetail["TotalTcyAmount"] = amount;
			if (InvoiceAcId > 0)
			{
				drDetail["DebitAccountId"] = InvoiceAcId;
			}
			BindGrids();
			EnableDisableHeaderColumns();
			CmbBrokerAgentName_Leave(null, null);
			txtGlcyRate_TextChanged(null, null);
			((Control)(object)CmbPaymentTerm).Enabled = true;
			txtDueDays.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceNo_MouseHover(object sender, EventArgs e)
	{
		LogisticsServicesBill_Helper.ShowInvoiceInfo(CmbInvoiceNo);
	}

	private void CmbInvoiceNo_MouseLeave(object sender, EventArgs e)
	{
		LogisticsServicesBill_Helper.StartHidePreviewTimer();
	}

	private void InvoicePreviewCard_MouseLeave(object sender, EventArgs e)
	{
		LogisticsServicesBill_Helper.StartHidePreviewTimer();
	}

	private void txtdocdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdCustomerDetail.RowCount <= 0)
			{
				return;
			}
			StringBuilder sb = new StringBuilder();
			GridEXRow[] rows = grdCustomerDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (r2.Cells["ItemId"].Value != null)
				{
					sb.Append(",").Append(r2.Cells["ItemId"].Value);
				}
			}
			Dictionary<int, DataRow> taxLookup = (from r in CommonServices.GetTaxScheduleDetailbyItemIds(sb.ToString(), txtdocdate.Value).AsEnumerable()
				group r by Conversion.ToInt(r["ItemId"])).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => g.First());
			rows = grdCustomerDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				int itemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				if (taxLookup.TryGetValue(itemId, out var row))
				{
					double taxPercent = Conversion.ToDouble(row["TaxPercent"]);
					item.Cells["TaxNameId"].Value = Conversion.ToInt(row["TaxNameId"]);
					item.Cells["TaxName"].Value = Conversion.ToString(row["TaxName"]);
					item.Cells["Tax%"].Value = taxPercent;
					CustomerDetail_TaxUpdateInRow(item);
				}
				else
				{
					item.Cells["TaxNameId"].Value = 0;
					item.Cells["TaxName"].Value = 0;
					item.Cells["Tax%"].Value = 0;
					item.Cells["TaxAmount"].Value = 0;
					item.Cells["TotalTcyAmount"].Value = 0;
				}
				item.EndEdit();
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
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Expected O, but got Unknown
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Expected O, but got Unknown
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Expected O, but got Unknown
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Expected O, but got Unknown
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Expected O, but got Unknown
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Expected O, but got Unknown
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Expected O, but got Unknown
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Expected O, but got Unknown
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Expected O, but got Unknown
		//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Expected O, but got Unknown
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Expected O, but got Unknown
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Expected O, but got Unknown
		//IL_0b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Expected O, but got Unknown
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Expected O, but got Unknown
		//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c03: Expected O, but got Unknown
		//IL_0c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c19: Expected O, but got Unknown
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5b: Expected O, but got Unknown
		//IL_11a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ae: Expected O, but got Unknown
		//IL_11bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c5: Expected O, but got Unknown
		//IL_9642: Unknown result type (might be due to invalid IL or missing references)
		//IL_964c: Expected O, but got Unknown
		//IL_9670: Unknown result type (might be due to invalid IL or missing references)
		//IL_967a: Expected O, but got Unknown
		GridEXLayout grdCustomerDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance174 = new Appearance();
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
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance87 = new Appearance();
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
		Appearance appearance111 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
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
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
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
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance197 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.lgstcm.frmLogisticPurchaseServicesBill));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdCustomerDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GrpDetailInfo = new System.Windows.Forms.GroupBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label69 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtTransactionExchangeRateDetail = new System.Windows.Forms.TextBox();
		this.CmbTransactionCurrencyDetail = new UltraCombo();
		this.CmbLocationTo = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.CmbLocationFrom = new UltraCombo();
		this.CmbLocationType = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.txtTaxPercnt = new System.Windows.Forms.TextBox();
		this.label65 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label58 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbTaxName = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.txtPartyRefDocNo = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtPartyRefDocDate = new System.Windows.Forms.DateTimePicker();
		this.panel34 = new System.Windows.Forms.Panel();
		this.RadCategory = new System.Windows.Forms.RadioButton();
		this.RadMasterItem = new System.Windows.Forms.RadioButton();
		this.label53 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtServiceRate = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.CmbMasterItem = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.RadItemCodePackListDetail = new System.Windows.Forms.RadioButton();
		this.label36 = new System.Windows.Forms.Label();
		this.RadItemNamePackListDetail = new System.Windows.Forms.RadioButton();
		this.CmbItemName = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.BtnAddCustomer = new System.Windows.Forms.Button();
		this.BtnCancelCustomer = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.BtnUpdateCustmer = new System.Windows.Forms.Button();
		this.label42 = new System.Windows.Forms.Label();
		this.txtTcyAmount = new System.Windows.Forms.TextBox();
		this.txtLcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbRateBaseType = new UltraCombo();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.PanelGlcy = new System.Windows.Forms.Panel();
		this.CmbGlcyCurrency = new UltraCombo();
		this.txtGlcyAmount = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.txtGlcyRate = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbInvoiceNo = new UltraCombo();
		this.CmbServiceType = new UltraCombo();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.CmbPaymentTerm = new UltraCombo();
		this.CmbFcyCurrency = new UltraCombo();
		this.txtrefbillno = new System.Windows.Forms.TextBox();
		this.CmbBillToPartyName = new UltraCombo();
		this.CmbServiceProviderOrBrokerAgent = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtFcyExchangeRate = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrintVoucher = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadFreightVoucher = new System.Windows.Forms.ToolStripButton();
		this.BtnAgreement = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.chkVoucher = new System.Windows.Forms.CheckBox();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.grdHistoryDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbServiceProviderHistory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbServiceTypeHistory = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label28 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		this.GrpDetailInfo.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrencyDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLocationTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLocationFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLocationType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxName).BeginInit();
		this.panel34.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateBaseType).BeginInit();
		this.groupBox2.SuspendLayout();
		this.PanelGlcy.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGlcyCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProviderOrBrokerAgent).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProviderHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 611);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerDetail);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 582);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdCustomerDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdCustomerDetail).BackColor = System.Drawing.Color.White;
		grdCustomerDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerDetail.DesignTimeLayout = grdCustomerDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Location = new System.Drawing.Point(3, 300);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Name = "grdCustomerDetail";
		((ScrollableBase)this.grdCustomerDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Size = new System.Drawing.Size(1170, 279);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).TabIndex = 0;
		this.grdCustomerDetail.TotalRow = (InheritableBoolean)1;
		this.grdCustomerDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerDetail).VisualStyle = (VisualStyle)4;
		this.grdCustomerDetail.CellUpdated += new ColumnActionEventHandler(grdCustomerDetail_CellUpdated);
		this.grdCustomerDetail.ColumnButtonClick += new ColumnActionEventHandler(grdCustomerDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).DoubleClick += new System.EventHandler(grdCustomerDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdCustomerDetail_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 279);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1170, 21);
		this.panel14.TabIndex = 131;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1124, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdCustomerDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel5.Controls.Add(this.GrpDetailInfo);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 54);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 225);
		this.panel5.TabIndex = 130;
		this.GrpDetailInfo.BackColor = System.Drawing.Color.White;
		this.GrpDetailInfo.Controls.Add(this.txtNetWeight);
		this.GrpDetailInfo.Controls.Add(this.label69);
		this.GrpDetailInfo.Controls.Add(this.label2);
		this.GrpDetailInfo.Controls.Add(this.txtTransactionExchangeRateDetail);
		this.GrpDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail);
		this.GrpDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLocationTo);
		this.GrpDetailInfo.Controls.Add(this.label55);
		this.GrpDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLocationFrom);
		this.GrpDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLocationType);
		this.GrpDetailInfo.Controls.Add(this.label34);
		this.GrpDetailInfo.Controls.Add(this.txtTotalAmount);
		this.GrpDetailInfo.Controls.Add(this.label56);
		this.GrpDetailInfo.Controls.Add(this.txtTaxPercnt);
		this.GrpDetailInfo.Controls.Add(this.label65);
		this.GrpDetailInfo.Controls.Add(this.label16);
		this.GrpDetailInfo.Controls.Add(this.label25);
		this.GrpDetailInfo.Controls.Add(this.label63);
		this.GrpDetailInfo.Controls.Add(this.txtTaxAmount);
		this.GrpDetailInfo.Controls.Add(this.label62);
		this.GrpDetailInfo.Controls.Add(this.txtQty);
		this.GrpDetailInfo.Controls.Add(this.label58);
		this.GrpDetailInfo.Controls.Add(this.label26);
		this.GrpDetailInfo.Controls.Add(this.label57);
		this.GrpDetailInfo.Controls.Add(this.label61);
		this.GrpDetailInfo.Controls.Add(this.label45);
		this.GrpDetailInfo.Controls.Add(this.label27);
		this.GrpDetailInfo.Controls.Add(this.label29);
		this.GrpDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxName);
		this.GrpDetailInfo.Controls.Add(this.label30);
		this.GrpDetailInfo.Controls.Add(this.txtPartyRefDocNo);
		this.GrpDetailInfo.Controls.Add(this.label31);
		this.GrpDetailInfo.Controls.Add(this.label52);
		this.GrpDetailInfo.Controls.Add(this.txtPartyRefDocDate);
		this.GrpDetailInfo.Controls.Add(this.panel34);
		this.GrpDetailInfo.Controls.Add(this.label53);
		this.GrpDetailInfo.Controls.Add(this.label67);
		this.GrpDetailInfo.Controls.Add(this.label66);
		this.GrpDetailInfo.Controls.Add(this.label64);
		this.GrpDetailInfo.Controls.Add(this.label33);
		this.GrpDetailInfo.Controls.Add(this.txtServiceRate);
		this.GrpDetailInfo.Controls.Add(this.label49);
		this.GrpDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterItem);
		this.GrpDetailInfo.Controls.Add(this.label48);
		this.GrpDetailInfo.Controls.Add(this.RadItemCodePackListDetail);
		this.GrpDetailInfo.Controls.Add(this.label36);
		this.GrpDetailInfo.Controls.Add(this.RadItemNamePackListDetail);
		this.GrpDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.GrpDetailInfo.Controls.Add(this.label38);
		this.GrpDetailInfo.Controls.Add(this.label39);
		this.GrpDetailInfo.Controls.Add(this.label40);
		this.GrpDetailInfo.Controls.Add(this.label41);
		this.GrpDetailInfo.Controls.Add(this.BtnAddCustomer);
		this.GrpDetailInfo.Controls.Add(this.BtnCancelCustomer);
		this.GrpDetailInfo.Controls.Add(this.txtRemarksDetail);
		this.GrpDetailInfo.Controls.Add(this.BtnUpdateCustmer);
		this.GrpDetailInfo.Controls.Add(this.label42);
		this.GrpDetailInfo.Controls.Add(this.txtTcyAmount);
		this.GrpDetailInfo.Controls.Add(this.txtLcyAmountDetail);
		this.GrpDetailInfo.Controls.Add(this.label43);
		this.GrpDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateBaseType);
		this.GrpDetailInfo.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.GrpDetailInfo.Location = new System.Drawing.Point(4, 104);
		this.GrpDetailInfo.Name = "GrpDetailInfo";
		this.GrpDetailInfo.Size = new System.Drawing.Size(1161, 119);
		this.GrpDetailInfo.TabIndex = 1;
		this.GrpDetailInfo.TabStop = false;
		this.GrpDetailInfo.Text = "Detail Information";
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(1069, 42);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(90, 23);
		this.txtNetWeight.TabIndex = 9;
		this.txtNetWeight.Tag = "double";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label69.Location = new System.Drawing.Point(1069, 18);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(68, 15);
		this.label69.TabIndex = 649;
		this.label69.Text = "Net Weight";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Red;
		this.label2.Location = new System.Drawing.Point(1142, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(14, 15);
		this.label2.TabIndex = 648;
		this.label2.Text = "*";
		this.txtTransactionExchangeRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTransactionExchangeRateDetail.Location = new System.Drawing.Point(750, 42);
		this.txtTransactionExchangeRateDetail.Name = "txtTransactionExchangeRateDetail";
		this.txtTransactionExchangeRateDetail.Size = new System.Drawing.Size(61, 23);
		this.txtTransactionExchangeRateDetail.TabIndex = 5;
		this.txtTransactionExchangeRateDetail.Tag = "double";
		this.txtTransactionExchangeRateDetail.TextChanged += new System.EventHandler(txtTransactionExchangeRateDetail_TextChanged);
		this.CmbTransactionCurrencyDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbTransactionCurrencyDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransactionCurrencyDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransactionCurrencyDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance2).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransactionCurrencyDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Location = new System.Drawing.Point(631, 40);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Name = "CmbTransactionCurrencyDetail";
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).TabIndex = 4;
		this.CmbTransactionCurrencyDetail.ValueChanged += new System.EventHandler(CmbTransactionCurrencyDetail_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Leave += new System.EventHandler(CmbTransactionCurrencyDetail_ValueChanged);
		this.CmbLocationTo.AllowNull = (DefaultableBoolean)1;
		this.CmbLocationTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLocationTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLocationTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocationTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocationTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLocationTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLocationTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLocationTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLocationTo).Location = new System.Drawing.Point(788, 87);
		((System.Windows.Forms.Control)(object)this.CmbLocationTo).Name = "CmbLocationTo";
		((System.Windows.Forms.Control)(object)this.CmbLocationTo).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbLocationTo).TabIndex = 18;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(788, 70);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(69, 15);
		this.label55.TabIndex = 158;
		this.label55.Text = "To Location";
		this.CmbLocationFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbLocationFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLocationFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLocationFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocationFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocationFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLocationFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLocationFrom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLocationFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLocationFrom).Location = new System.Drawing.Point(641, 87);
		((System.Windows.Forms.Control)(object)this.CmbLocationFrom).Name = "CmbLocationFrom";
		((System.Windows.Forms.Control)(object)this.CmbLocationFrom).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbLocationFrom).TabIndex = 17;
		this.CmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.CmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLocationType).Location = new System.Drawing.Point(546, 87);
		((System.Windows.Forms.Control)(object)this.CmbLocationType).Name = "CmbLocationType";
		((System.Windows.Forms.Control)(object)this.CmbLocationType).Size = new System.Drawing.Size(93, 26);
		((System.Windows.Forms.Control)(object)this.CmbLocationType).TabIndex = 16;
		this.CmbLocationType.ValueChanged += new System.EventHandler(CmbLocationType_Leave);
		((System.Windows.Forms.Control)(object)this.CmbLocationType).Leave += new System.EventHandler(CmbLocationType_Leave);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(641, 70);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(84, 15);
		this.label34.TabIndex = 159;
		this.label34.Text = "From Location";
		this.txtTotalAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(439, 89);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(105, 23);
		this.txtTotalAmount.TabIndex = 15;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(546, 70);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(81, 15);
		this.label56.TabIndex = 159;
		this.label56.Text = "Location Type";
		this.txtTaxPercnt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercnt.Location = new System.Drawing.Point(296, 89);
		this.txtTaxPercnt.Name = "txtTaxPercnt";
		this.txtTaxPercnt.ShortcutsEnabled = false;
		this.txtTaxPercnt.Size = new System.Drawing.Size(50, 23);
		this.txtTaxPercnt.TabIndex = 13;
		this.txtTaxPercnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercnt.TextChanged += new System.EventHandler(txtTaxPercnt_TextChanged);
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Red;
		this.label65.Location = new System.Drawing.Point(532, 70);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(14, 15);
		this.label65.TabIndex = 175;
		this.label65.Text = "*";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(296, 70);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(35, 15);
		this.label16.TabIndex = 644;
		this.label16.Text = "Tax%";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(432, 70);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(101, 15);
		this.label25.TabIndex = 646;
		this.label25.Text = "Total Tcy Amount";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.Red;
		this.label63.Location = new System.Drawing.Point(165, 70);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(14, 15);
		this.label63.TabIndex = 173;
		this.label63.Text = "*";
		this.txtTaxAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(347, 89);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(90, 23);
		this.txtTaxAmount.TabIndex = 14;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Red;
		this.label62.Location = new System.Drawing.Point(71, 70);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(14, 15);
		this.label62.TabIndex = 172;
		this.label62.Text = "*";
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(987, 42);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(80, 23);
		this.txtQty.TabIndex = 8;
		this.txtQty.Tag = "double";
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(1013, 18);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(14, 15);
		this.label58.TabIndex = 171;
		this.label58.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(350, 70);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(72, 15);
		this.label26.TabIndex = 645;
		this.label26.Text = "Tax Amount";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(987, 18);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(26, 15);
		this.label57.TabIndex = 158;
		this.label57.Text = "Qty";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Red;
		this.label61.Location = new System.Drawing.Point(881, 18);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(14, 15);
		this.label61.TabIndex = 171;
		this.label61.Text = "*";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(783, 25);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 15);
		this.label45.TabIndex = 159;
		this.label45.Text = "*";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(195, 70);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(60, 15);
		this.label27.TabIndex = 643;
		this.label27.Text = "Tax Name";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(751, 10);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(61, 30);
		this.label29.TabIndex = 158;
		this.label29.Text = "Exchange \r\nRate";
		this.CmbTaxName.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance124).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Location = new System.Drawing.Point(195, 87);
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Name = "CmbTaxName";
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxName).TabIndex = 12;
		this.CmbTaxName.ValueChanged += new System.EventHandler(CmbTaxName_Leave);
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Leave += new System.EventHandler(CmbTaxName_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(153, 18);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(77, 15);
		this.label30.TabIndex = 638;
		this.label30.Text = "Ref Doc Date";
		this.txtPartyRefDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyRefDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyRefDocNo.Location = new System.Drawing.Point(2, 42);
		this.txtPartyRefDocNo.Name = "txtPartyRefDocNo";
		this.txtPartyRefDocNo.Size = new System.Drawing.Size(150, 23);
		this.txtPartyRefDocNo.TabIndex = 0;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(2, 18);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(98, 15);
		this.label31.TabIndex = 637;
		this.label31.Text = "Party Ref Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(955, 18);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 15);
		this.label52.TabIndex = 163;
		this.label52.Text = "*";
		this.txtPartyRefDocDate.CustomFormat = "dd-MMM-yy";
		this.txtPartyRefDocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtPartyRefDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPartyRefDocDate.Location = new System.Drawing.Point(153, 42);
		this.txtPartyRefDocDate.Name = "txtPartyRefDocDate";
		this.txtPartyRefDocDate.Size = new System.Drawing.Size(120, 22);
		this.txtPartyRefDocDate.TabIndex = 1;
		this.panel34.Controls.Add(this.RadCategory);
		this.panel34.Controls.Add(this.RadMasterItem);
		this.panel34.Location = new System.Drawing.Point(274, 14);
		this.panel34.Name = "panel34";
		this.panel34.Size = new System.Drawing.Size(162, 22);
		this.panel34.TabIndex = 634;
		this.RadCategory.AutoSize = true;
		this.RadCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCategory.Location = new System.Drawing.Point(89, 3);
		this.RadCategory.Name = "RadCategory";
		this.RadCategory.Size = new System.Drawing.Size(72, 19);
		this.RadCategory.TabIndex = 35;
		this.RadCategory.Text = "Category";
		this.RadCategory.UseVisualStyleBackColor = true;
		this.RadMasterItem.AutoSize = true;
		this.RadMasterItem.Checked = true;
		this.RadMasterItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadMasterItem.Location = new System.Drawing.Point(4, 3);
		this.RadMasterItem.Name = "RadMasterItem";
		this.RadMasterItem.Size = new System.Drawing.Size(89, 19);
		this.RadMasterItem.TabIndex = 36;
		this.RadMasterItem.TabStop = true;
		this.RadMasterItem.Text = "Master Item";
		this.RadMasterItem.UseVisualStyleBackColor = true;
		this.RadMasterItem.CheckedChanged += new System.EventHandler(RadMasterItem_CheckedChanged);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(102, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 15);
		this.label53.TabIndex = 164;
		this.label53.Text = "*";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.ForeColor = System.Drawing.Color.Red;
		this.label67.Location = new System.Drawing.Point(863, 70);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(14, 15);
		this.label67.TabIndex = 162;
		this.label67.Text = "*";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.Red;
		this.label66.Location = new System.Drawing.Point(731, 70);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(14, 15);
		this.label66.TabIndex = 162;
		this.label66.Text = "*";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Red;
		this.label64.Location = new System.Drawing.Point(623, 70);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(14, 15);
		this.label64.TabIndex = 162;
		this.label64.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(711, 18);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(14, 15);
		this.label33.TabIndex = 162;
		this.label33.Text = "*";
		this.txtServiceRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtServiceRate.Location = new System.Drawing.Point(812, 42);
		this.txtServiceRate.Name = "txtServiceRate";
		this.txtServiceRate.Size = new System.Drawing.Size(82, 23);
		this.txtServiceRate.TabIndex = 6;
		this.txtServiceRate.Tag = "double";
		this.txtServiceRate.TextChanged += new System.EventHandler(txtServiceRate_TextChanged);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(608, 18);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 15);
		this.label49.TabIndex = 161;
		this.label49.Text = "*";
		this.CmbMasterItem.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Location = new System.Drawing.Point(274, 40);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Name = "CmbMasterItem";
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).TabIndex = 2;
		this.CmbMasterItem.ValueChanged += new System.EventHandler(CmbMasterItem_Leave);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Leave += new System.EventHandler(CmbMasterItem_Leave);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(230, 18);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 15);
		this.label48.TabIndex = 160;
		this.label48.Text = "*";
		this.RadItemCodePackListDetail.AutoSize = true;
		this.RadItemCodePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodePackListDetail.Location = new System.Drawing.Point(560, 16);
		this.RadItemCodePackListDetail.Name = "RadItemCodePackListDetail";
		this.RadItemCodePackListDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodePackListDetail.TabIndex = 633;
		this.RadItemCodePackListDetail.Text = "Code";
		this.RadItemCodePackListDetail.UseVisualStyleBackColor = true;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(812, 18);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(71, 15);
		this.label36.TabIndex = 158;
		this.label36.Text = "Service Rate";
		this.RadItemNamePackListDetail.AutoSize = true;
		this.RadItemNamePackListDetail.Checked = true;
		this.RadItemNamePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNamePackListDetail.Location = new System.Drawing.Point(507, 16);
		this.RadItemNamePackListDetail.Name = "RadItemNamePackListDetail";
		this.RadItemNamePackListDetail.Size = new System.Drawing.Size(57, 19);
		this.RadItemNamePackListDetail.TabIndex = 632;
		this.RadItemNamePackListDetail.TabStop = true;
		this.RadItemNamePackListDetail.Text = "Name";
		this.RadItemNamePackListDetail.UseVisualStyleBackColor = true;
		this.RadItemNamePackListDetail.CheckedChanged += new System.EventHandler(RadItemNamePackListDetail_CheckedChanged);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance8).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(436, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 3;
		this.CmbItemName.ValueChanged += new System.EventHandler(CmbItemName_Leave);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(634, 10);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(68, 30);
		this.label38.TabIndex = 159;
		this.label38.Text = "Transaction\r\nCurrency";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(436, 18);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(73, 15);
		this.label39.TabIndex = 631;
		this.label39.Text = "Service Item";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(2, 70);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(72, 15);
		this.label40.TabIndex = 158;
		this.label40.Text = "Tcy Amount";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(94, 70);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(72, 15);
		this.label41.TabIndex = 158;
		this.label41.Text = "Lcy Amount";
		this.BtnAddCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnAddCustomer.FlatAppearance.BorderSize = 0;
		this.BtnAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnAddCustomer.Location = new System.Drawing.Point(1096, 90);
		this.BtnAddCustomer.Name = "BtnAddCustomer";
		this.BtnAddCustomer.Size = new System.Drawing.Size(39, 21);
		this.BtnAddCustomer.TabIndex = 20;
		this.BtnAddCustomer.Text = "+";
		this.BtnAddCustomer.UseVisualStyleBackColor = false;
		this.BtnAddCustomer.Click += new System.EventHandler(BtnAddCustomer_Click);
		this.BtnCancelCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelCustomer.FlatAppearance.BorderSize = 0;
		this.BtnCancelCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnCancelCustomer.Location = new System.Drawing.Point(1096, 69);
		this.BtnCancelCustomer.Name = "BtnCancelCustomer";
		this.BtnCancelCustomer.Size = new System.Drawing.Size(64, 20);
		this.BtnCancelCustomer.TabIndex = 22;
		this.BtnCancelCustomer.Text = "Cancel";
		this.BtnCancelCustomer.UseVisualStyleBackColor = false;
		this.BtnCancelCustomer.Visible = false;
		this.BtnCancelCustomer.Click += new System.EventHandler(BtnCancelCustomer_Click);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(934, 89);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(160, 23);
		this.txtRemarksDetail.TabIndex = 19;
		this.BtnUpdateCustmer.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateCustmer.FlatAppearance.BorderSize = 0;
		this.BtnUpdateCustmer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateCustmer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateCustmer.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateCustmer.Location = new System.Drawing.Point(1096, 90);
		this.BtnUpdateCustmer.Name = "BtnUpdateCustmer";
		this.BtnUpdateCustmer.Size = new System.Drawing.Size(64, 21);
		this.BtnUpdateCustmer.TabIndex = 21;
		this.BtnUpdateCustmer.Text = "Update";
		this.BtnUpdateCustmer.UseVisualStyleBackColor = false;
		this.BtnUpdateCustmer.Visible = false;
		this.BtnUpdateCustmer.Click += new System.EventHandler(BtnUpdateCustmer_Click);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(934, 70);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(52, 15);
		this.label42.TabIndex = 131;
		this.label42.Text = "Remarks";
		this.txtTcyAmount.Enabled = false;
		this.txtTcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTcyAmount.Location = new System.Drawing.Point(2, 89);
		this.txtTcyAmount.Name = "txtTcyAmount";
		this.txtTcyAmount.Size = new System.Drawing.Size(90, 23);
		this.txtTcyAmount.TabIndex = 10;
		this.txtLcyAmountDetail.Enabled = false;
		this.txtLcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLcyAmountDetail.Location = new System.Drawing.Point(94, 89);
		this.txtLcyAmountDetail.Name = "txtLcyAmountDetail";
		this.txtLcyAmountDetail.Size = new System.Drawing.Size(100, 23);
		this.txtLcyAmountDetail.TabIndex = 11;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(895, 18);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(57, 15);
		this.label43.TabIndex = 159;
		this.label43.Text = "Rate Base";
		this.CmbRateBaseType.AllowNull = (DefaultableBoolean)1;
		this.CmbRateBaseType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateBaseType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateBaseType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance199;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateBaseType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Location = new System.Drawing.Point(895, 40);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Name = "CmbRateBaseType";
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).TabIndex = 7;
		this.CmbRateBaseType.ValueChanged += new System.EventHandler(CmbRateBaseType_Leave);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Leave += new System.EventHandler(CmbRateBaseType_Leave);
		this.groupBox2.Controls.Add(this.PanelGlcy);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceType);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCurrency);
		this.groupBox2.Controls.Add(this.txtrefbillno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillToPartyName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceProviderOrBrokerAgent);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.txtDueDays);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtFcyExchangeRate);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1161, 101);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.PanelGlcy.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGlcyCurrency);
		this.PanelGlcy.Controls.Add(this.txtGlcyAmount);
		this.PanelGlcy.Controls.Add(this.label44);
		this.PanelGlcy.Controls.Add(this.label46);
		this.PanelGlcy.Controls.Add(this.label68);
		this.PanelGlcy.Controls.Add(this.label59);
		this.PanelGlcy.Controls.Add(this.label60);
		this.PanelGlcy.Controls.Add(this.txtGlcyRate);
		this.PanelGlcy.Location = new System.Drawing.Point(956, 10);
		this.PanelGlcy.Name = "PanelGlcy";
		this.PanelGlcy.Size = new System.Drawing.Size(203, 85);
		this.PanelGlcy.TabIndex = 11;
		this.CmbGlcyCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbGlcyCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGlcyCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGlcyCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGlcyCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGlcyCurrency).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbGlcyCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbGlcyCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGlcyCurrency).Location = new System.Drawing.Point(82, 3);
		((System.Windows.Forms.Control)(object)this.CmbGlcyCurrency).Name = "CmbGlcyCurrency";
		((System.Windows.Forms.Control)(object)this.CmbGlcyCurrency).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.CmbGlcyCurrency).TabIndex = 0;
		this.txtGlcyAmount.Enabled = false;
		this.txtGlcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGlcyAmount.Location = new System.Drawing.Point(82, 58);
		this.txtGlcyAmount.Name = "txtGlcyAmount";
		this.txtGlcyAmount.Size = new System.Drawing.Size(116, 23);
		this.txtGlcyAmount.TabIndex = 2;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(5, 9);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(60, 15);
		this.label44.TabIndex = 159;
		this.label44.Text = "Party Glcy";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(68, 9);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 15);
		this.label46.TabIndex = 159;
		this.label46.Text = "*";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(68, 37);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(14, 15);
		this.label68.TabIndex = 159;
		this.label68.Text = "*";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(5, 37);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(56, 15);
		this.label59.TabIndex = 158;
		this.label59.Text = "Glcy Rate";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(5, 62);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(77, 15);
		this.label60.TabIndex = 158;
		this.label60.Text = "Glcy Amount";
		this.txtGlcyRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGlcyRate.Location = new System.Drawing.Point(82, 33);
		this.txtGlcyRate.Name = "txtGlcyRate";
		this.txtGlcyRate.Size = new System.Drawing.Size(116, 23);
		this.txtGlcyRate.TabIndex = 1;
		this.txtGlcyRate.Tag = "double";
		this.txtGlcyRate.TextChanged += new System.EventHandler(txtGlcyRate_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(282, 39);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(92, 30);
		this.label7.TabIndex = 510;
		this.label7.Text = "Broker Agent / \r\nService Provider";
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance46;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(87, 69);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		this.CmbInvoiceNo.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).MouseLeave += new System.EventHandler(CmbInvoiceNo_MouseLeave);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).MouseHover += new System.EventHandler(CmbInvoiceNo_MouseHover);
		this.CmbServiceType.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance129).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Location = new System.Drawing.Point(87, 41);
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Name = "CmbServiceType";
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceType).TabIndex = 2;
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(651, 71);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(304, 23);
		this.txtRemarksHeader.TabIndex = 10;
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(651, 13);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 7;
		this.CmbFcyCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance185).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbFcyCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Location = new System.Drawing.Point(651, 41);
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Name = "CmbFcyCurrency";
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).TabIndex = 8;
		this.txtrefbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrefbillno.Location = new System.Drawing.Point(385, 71);
		this.txtrefbillno.Name = "txtrefbillno";
		this.txtrefbillno.Size = new System.Drawing.Size(200, 23);
		this.txtrefbillno.TabIndex = 6;
		this.CmbBillToPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbBillToPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillToPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillToPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillToPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyName).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillToPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyName).Location = new System.Drawing.Point(385, 13);
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyName).Name = "CmbBillToPartyName";
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyName).TabIndex = 4;
		this.CmbBillToPartyName.ValueChanged += new System.EventHandler(CmbBrokerAgentName_Leave);
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyName).Leave += new System.EventHandler(CmbBrokerAgentName_Leave);
		this.CmbServiceProviderOrBrokerAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceProviderOrBrokerAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceProviderOrBrokerAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceProviderOrBrokerAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceProviderOrBrokerAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderOrBrokerAgent).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderOrBrokerAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceProviderOrBrokerAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderOrBrokerAgent).Location = new System.Drawing.Point(385, 41);
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderOrBrokerAgent).Name = "CmbServiceProviderOrBrokerAgent";
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderOrBrokerAgent).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderOrBrokerAgent).TabIndex = 5;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(873, 47);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 15);
		this.label24.TabIndex = 10;
		this.label24.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(873, 19);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(14, 15);
		this.label23.TabIndex = 9;
		this.label23.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(639, 19);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 15);
		this.label20.TabIndex = 509;
		this.label20.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(639, 47);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 15);
		this.label18.TabIndex = 509;
		this.label18.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(372, 47);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 15);
		this.label13.TabIndex = 509;
		this.label13.Text = "*";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Red;
		this.label12.Location = new System.Drawing.Point(372, 19);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(14, 15);
		this.label12.TabIndex = 509;
		this.label12.Text = "*";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Red;
		this.label8.Location = new System.Drawing.Point(74, 47);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(14, 15);
		this.label8.TabIndex = 509;
		this.label8.Text = "*";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(589, 11);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(57, 30);
		this.label54.TabIndex = 508;
		this.label54.Text = "Payment \r\nTerm";
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Enabled = false;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(887, 15);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(68, 23);
		this.txtDueDays.TabIndex = 8;
		this.txtDueDays.Tag = "int";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(820, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 15);
		this.label4.TabIndex = 507;
		this.label4.Text = "Due Days";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(282, 75);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(78, 15);
		this.label50.TabIndex = 168;
		this.label50.Text = "Reference No";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(2, 67);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(71, 30);
		this.label51.TabIndex = 167;
		this.label51.Text = "Commercial\r\nInvoice No";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(282, 17);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(84, 19);
		this.label14.TabIndex = 164;
		this.label14.Text = "Bill To Party";
		this.txtFcyExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyExchangeRate.Location = new System.Drawing.Point(887, 43);
		this.txtFcyExchangeRate.Name = "txtFcyExchangeRate";
		this.txtFcyExchangeRate.Size = new System.Drawing.Size(68, 23);
		this.txtFcyExchangeRate.TabIndex = 9;
		this.txtFcyExchangeRate.Tag = "double";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(820, 47);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(51, 15);
		this.label10.TabIndex = 158;
		this.label10.Text = "Fcy Rate";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(589, 39);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(54, 30);
		this.label9.TabIndex = 159;
		this.label9.Text = "Fcy \r\nCurrency";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(2, 47);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(73, 15);
		this.label11.TabIndex = 151;
		this.label11.Text = "Service Type";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(158, 15);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(120, 22);
		this.txtdocdate.TabIndex = 1;
		this.txtdocdate.ValueChanged += new System.EventHandler(txtdocdate_ValueChanged);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(87, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(70, 23);
		this.txtdocno.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(589, 75);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Remarks";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(2, 19);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(76, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc No,Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.chkVoucher);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1170, 51);
		this.panel3.TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.Yellow;
		this.label22.Location = new System.Drawing.Point(198, 31);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(501, 14);
		this.label22.TabIndex = 127;
		this.label22.Text = "A centralized record that supports management of logistics-related purchase Bill";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(4, 27);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(197, 19);
		this.label21.TabIndex = 128;
		this.label21.Text = "Purchase Services Bill :";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.btnPrintVoucher, this.BtnLoadFreightVoucher,
			this.BtnAgreement, this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
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
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1304-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPrintVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintVoucher.Name = "btnPrintVoucher";
		this.btnPrintVoucher.Size = new System.Drawing.Size(115, 24);
		this.btnPrintVoucher.Text = "Voucher-118";
		this.btnPrintVoucher.Click += new System.EventHandler(btnPrintVoucher_Click);
		this.BtnLoadFreightVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadFreightVoucher.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadFreightVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadFreightVoucher.Name = "BtnLoadFreightVoucher";
		this.BtnLoadFreightVoucher.Size = new System.Drawing.Size(171, 24);
		this.BtnLoadFreightVoucher.Text = "Load Freight Voucher";
		this.BtnLoadFreightVoucher.Click += new System.EventHandler(BtnLoadFreightVoucher_Click);
		this.BtnAgreement.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAgreement.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnAgreement.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAgreement.Name = "BtnAgreement";
		this.BtnAgreement.Size = new System.Drawing.Size(85, 24);
		this.BtnAgreement.Text = "Load PO";
		this.BtnAgreement.Click += new System.EventHandler(BtnAgreement_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.chkVoucher.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkVoucher.AutoSize = true;
		this.chkVoucher.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkVoucher.ForeColor = System.Drawing.Color.White;
		this.chkVoucher.Location = new System.Drawing.Point(1057, 29);
		this.chkVoucher.Name = "chkVoucher";
		this.chkVoucher.Size = new System.Drawing.Size(106, 20);
		this.chkVoucher.TabIndex = 12;
		this.chkVoucher.Text = "Preview 118";
		this.chkVoucher.UseVisualStyleBackColor = true;
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(939, 29);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(114, 20);
		this.ChkPreview.TabIndex = 12;
		this.ChkPreview.Text = "Preview 1304";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1176, 582);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 150);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1176, 215);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label15);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(0, 365);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1176, 24);
		this.panel9.TabIndex = 51;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(225, 16);
		this.label15.TabIndex = 129;
		this.label15.Text = "Detail of the Above Selected Row";
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 389);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1176, 193);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 127);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1176, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(112, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 52);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1176, 75);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceProviderHistory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.rdapproveddate);
		this.groupBox1.Controls.Add(this.rdmodifydate);
		this.groupBox1.Controls.Add(this.drdocdate);
		this.groupBox1.Controls.Add(this.rdentrydate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.ToDateHistory);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.ToDocNoHistory);
		this.groupBox1.Controls.Add(this.FromDateHistory);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.FromDocNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(969, 73);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbServiceProviderHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceProviderHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceProviderHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceProviderHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance215;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance178).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceProviderHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderHistory).Location = new System.Drawing.Point(543, 42);
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderHistory).Name = "CmbServiceProviderHistory";
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderHistory).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceProviderHistory).TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(471, 38);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(51, 30);
		this.label6.TabIndex = 182;
		this.label6.Text = "Service \r\nProvider";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance157).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(69, 14);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(867, 47);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(867, 28);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(787, 28);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(787, 47);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(727, 43);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbServiceTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance181;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance187).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Location = new System.Drawing.Point(543, 14);
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Name = "CmbServiceTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).TabIndex = 5;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(471, 20);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(73, 15);
		this.label3.TabIndex = 180;
		this.label3.Text = "Service Type";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(3, 20);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(201, 44);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(3, 40);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 30);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From\r\n&& To";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(397, 44);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 44);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 22);
		this.FromDateHistory.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(336, 48);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(45, 15);
		this.label5.TabIndex = 173;
		this.label5.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(336, 20);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(397, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1176, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1176, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 31);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(259, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Purchase Services Bill History  ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 611);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmLogisticPurchaseServicesBill";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmLogisticPurchaseServicesBill_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.GrpDetailInfo.ResumeLayout(false);
		this.GrpDetailInfo.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrencyDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLocationTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLocationFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLocationType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxName).EndInit();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateBaseType).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.PanelGlcy.ResumeLayout(false);
		this.PanelGlcy.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGlcyCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProviderOrBrokerAgent).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProviderHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
