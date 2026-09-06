using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.lgstcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.lgstcm;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Supplier_Purchase;
using Architecture.WinApp.lgstcm.LookUp;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Service.lgstcm;

public class frmLogisticRateNegotiation : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtCustomerDetail = new DataTable();

	private DataTable dtSourceDocument = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtServiceProvider = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtInvoice = new DataTable();

	private DataTable dtBrokeragentFromDetailGrid = new DataTable();

	private string ScreenName = "frmLogisticRateNegotiation";

	private bool UpdateMode;

	private int RecId;

	private int ForeignBaseCurrency;

	private double ForeignBaseCurrencyRate;

	private int DocumentTypeId = 1300;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool ItemSearchWithNameOrCode = true;

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<logisticRateNegotiationDetail> lstRemoveRecordcustomerDetail = new List<logisticRateNegotiationDetail>();

	private List<logisticRateNegotiationSourceDocument> lstRemoveRecordSourceDocument = new List<logisticRateNegotiationSourceDocument>();

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

	private Label label21;

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

	private GroupBox groupBox3;

	internal UltraCombo CmbBrokerAgentName;

	private CheckBox ChkPreview;

	private TextBox txtRemarksHeader;

	private Label label15;

	internal GridEX grdCustomerDetail;

	private Button BtnAddCustomer;

	private Button BtnCancelCustomer;

	private Button BtnUpdateCustmer;

	private ToolStripButton btnSaveAs;

	private ToolStripButton btnAttachment;

	private Label label1;

	private Label label5;

	private ToolStripButton BtnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel5;

	private Panel panel2;

	private Panel panel4;

	internal GridEX grdSourceDocument;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label4;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbServiceType;

	private Label label11;

	private Label label14;

	private Button BtnReferenceParty;

	private Label label22;

	internal UltraCombo CmbDestinationPort;

	internal UltraCombo CmbLoadingPort;

	private Label label24;

	private Label label27;

	private Label label29;

	private TextBox txtTransactionExchangeRate;

	private Label label30;

	internal UltraCombo CmbTransactionCurrency;

	private Label label31;

	internal UltraCombo CmbDealStatus;

	internal UltraCombo CmbServiceProvider;

	private Label label7;

	private Label label54;

	internal UltraCombo CmbPaymentTerm;

	internal UltraCombo CmbVesselRoutingType;

	private TextBox txtDueDays;

	private Label label12;

	private Label label13;

	private Label label25;

	private TextBox txtLcyAmountDetail;

	private Label label34;

	private TextBox txtOfferValidityDays;

	private Label label33;

	private DateTimePicker txtCargoCutOffDate;

	internal UltraCombo CmbRateBaseType;

	private Label label40;

	private Label label39;

	private Label label44;

	private Label label41;

	private TextBox txtFreeDaysAtPOD;

	private TextBox txtTransitDays;

	private DateTimePicker txtEtdSailing;

	private Label label2;

	private TextBox txtVesselRoutingDescription;

	private ToolStripButton BtnDefineLookUps;

	internal UltraCombo CmbDealStatusHistory;

	private Label label6;

	internal UltraCombo CmbServiceTypeHistory;

	private Label label3;

	private Label label8;

	private TextBox txtFcyExchangeRate;

	internal UltraCombo CmbFcyCurrency;

	private Label label10;

	private Label label9;

	private Panel panel34;

	private RadioButton RadCategory;

	private RadioButton RadMasterItem;

	private TextBox txtServiceRate;

	internal UltraCombo CmbMasterItem;

	private RadioButton RadItemCodePackListDetail;

	private Label label20;

	private RadioButton RadItemNamePackListDetail;

	internal UltraCombo CmbItemName;

	private Label label16;

	private Label label23;

	private TextBox txtTcyAmount;

	internal UltraCombo CmbTransactionCurrencyDetail;

	private Label label18;

	private TextBox txtRemarksDetail;

	private Label label26;

	private Label label36;

	private DateTimePicker txtOfferValidityDate;

	public frmLogisticRateNegotiation(UserAccount user)
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
			dtServiceProvider = dtSupplier.Clone();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("MasterItemId", typeof(int));
			dtItem.Columns.Add("MasterItem");
			dtItem.Columns.Add("ItemCategoryId", typeof(int));
			dtItem.Columns.Add("ItemCategory");
			dtBrokeragentFromDetailGrid.Columns.Add("BrokerAgentId", typeof(int));
			dtBrokeragentFromDetailGrid.Columns.Add("BrokerAgent", typeof(string));
			dtBrokeragentFromDetailGrid.Columns.Add("ServiceProviderId", typeof(int));
			dtBrokeragentFromDetailGrid.Columns.Add("ServiceProviderName", typeof(string));
			dtCustomerDetail.Columns.Add("Id", typeof(int));
			dtCustomerDetail.Columns.Add("BrokerAgentId", typeof(int));
			dtCustomerDetail.Columns.Add("BrokerAgent", typeof(string));
			dtCustomerDetail.Columns.Add("ServiceProviderId", typeof(int));
			dtCustomerDetail.Columns.Add("ServiceProviderName", typeof(string));
			dtCustomerDetail.Columns.Add("PaymentTermId", typeof(int));
			dtCustomerDetail.Columns.Add("PaymentTerm", typeof(string));
			dtCustomerDetail.Columns.Add("DueDays", typeof(int));
			dtCustomerDetail.Columns.Add("VesselRoutingTypeId", typeof(int));
			dtCustomerDetail.Columns.Add("VesselRoutingType", typeof(string));
			dtCustomerDetail.Columns.Add("VesselRoutingDescription", typeof(string));
			dtCustomerDetail.Columns.Add("ItemId", typeof(int));
			dtCustomerDetail.Columns.Add("ItemCode");
			dtCustomerDetail.Columns.Add("ItemName");
			dtCustomerDetail.Columns.Add("TransactionCurrencyId", typeof(int));
			dtCustomerDetail.Columns.Add("TransactionCurrency", typeof(string));
			dtCustomerDetail.Columns.Add("ServiceRate", typeof(decimal));
			dtCustomerDetail.Columns.Add("RateBaseId", typeof(int));
			dtCustomerDetail.Columns.Add("RateBase", typeof(string));
			dtCustomerDetail.Columns.Add("RateBaseEquivalent", typeof(decimal));
			dtCustomerDetail.Columns.Add("TcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("LcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("OfferValidityDays", typeof(int));
			dtCustomerDetail.Columns.Add("OfferValidityDate", typeof(DateTime));
			dtCustomerDetail.Columns.Add("CargoCutOffDate", typeof(DateTime));
			dtCustomerDetail.Columns.Add("EtdSealingDate", typeof(DateTime));
			dtCustomerDetail.Columns.Add("TransitsTimeDays", typeof(int));
			dtCustomerDetail.Columns.Add("FreeDaysAtPOD", typeof(int));
			dtCustomerDetail.Columns.Add("Remarks");
			dtSourceDocument.Columns.Add("Id", typeof(int));
			dtSourceDocument.Columns.Add("DocumentTypeId", typeof(int));
			dtSourceDocument.Columns.Add("DocumentId", typeof(int));
			dtSourceDocument.Columns.Add("NoOfContainers", typeof(int));
			dtSourceDocument.Columns.Add("MTon", typeof(double));
			dtSourceDocument.Columns.Add("BrokerAgentId", typeof(int));
			dtSourceDocument.Columns.Add("ServiceProviderId", typeof(int));
			dtSourceDocument.Columns.Add("Remarks");
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
			DataTable portsData = PortsDbCall();
			dtExportInvoiceDbCall();
			return new
			{
				HistoryComboData = historyComboData,
				DocumentNoData = documentNoData,
				LookUpData = lookUpData,
				PortsData = portsData
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
				btnSaveAs.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnPrint.Enabled = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				ctrlGrdBar2.mGridSaveLayouts.Visible = false;
				ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
				ctrlGrdBar3.mGridSaveLayouts.Visible = false;
				ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
				GetConfigurationsFromGlobal();
				if (RecId == 0)
				{
					txtdocno.Text = result.DocumentNoData;
				}
				AllLookUpComboBind(result.LookUpData);
				PortsBind(result.PortsData);
				CurrencyBindFromGlobal();
				SupplierDtFillFromGlobal();
				BrokerAgentOrServiceProviderBind(CmbBrokerAgentName, dtSupplier, "Broker Agent");
				BrokerAgentOrServiceProviderBind(CmbServiceProvider, dtServiceProvider, "Service Provider");
				ItemCategoryOrMasterItemBind();
				ItemDtsFillFromGlobal();
				ItemNameBind();
				GetConfigurationsFromGlobalandBind();
				DataTable dataTable = dtCustomerDetail;
				if (dataTable != null && dataTable.Columns.Count > 0)
				{
					grdCustomerDetail.DataSource = dtCustomerDetail;
					grdCustomerDetail.RetrieveStructure();
					grdCustomerDetailSetting(grdCustomerDetail);
				}
				DataTable dataTable2 = dtSourceDocument;
				if (dataTable2 != null && dataTable2.Columns.Count > 0)
				{
					AddRowInSourceDocumentGrid();
					grdSourceDocument.DataSource = dtSourceDocument;
					grdSourceDocument.RetrieveStructure();
					grdSourceDocumentSettings();
				}
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
			if (ForeignBaseCurrency > 0)
			{
				CmbFcyCurrency.Value = ForeignBaseCurrency;
				txtFcyExchangeRate.Text = ((Conversion.ToDouble((object)txtFcyExchangeRate.Text) > 0.0) ? txtFcyExchangeRate.Text : ForeignBaseCurrencyRate.ToString(clsGlobalVariables.DecimalFCYRateFormate));
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
			return logisticRateNegotiationHeader.GenerateCode(new logisticRateNegotiationHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				documentTypeId = DocumentTypeId,
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
			DataTable dtStatus = dtSP.Clone();
			DataTable dtRoutingType = dtSP.Clone();
			DataTable DtUom = dtSP.Clone();
			DataTable DtPaymentTerm = dtSP.Clone();
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
				case "DealStatus":
					dtStatus.Rows.Add(id, name);
					break;
				case "RoutingType":
					dtRoutingType.Rows.Add(id, name);
					break;
				case "RateBaseUom":
				{
					double equivalent = Conversion.ToDouble(row["OtherReference"]);
					DtUom.Rows.Add(id, name, equivalent);
					break;
				}
				case "PaymentTerm":
					DtPaymentTerm.Rows.Add(id, name);
					break;
				}
			}
			ServiceTypeBind(dtSP);
			DealStatusBind(dtStatus);
			VesselRoutingTypeBind(dtRoutingType);
			RateBaseTypeTypeBind(DtUom);
			PaymentTermsBind(DtPaymentTerm);
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

	private void PortsBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbLoadingPort, dt, "Id", "PortName", "Loading Port", AllColumns: false, Conversion.ToInt(CmbLoadingPort.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDestinationPort, dt, "Id", "PortName", "Destination Port", AllColumns: false, Conversion.ToInt(CmbDestinationPort.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CurrencyCode", typeof(string));
			dt.Columns.Add("CurrencyRate", typeof(double));
			foreach (MultiCurrency dd in clsGlobalVariables.globalMultiCurrency)
			{
				dt.Rows.Add(dd.Id, dd.CurrencyCode, dd.CurrencyRate);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbTransactionCurrency, dt, "Id", "CurrencyCode", "Transaction Currency", AllColumns: true, Conversion.ToInt(CmbTransactionCurrency.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbFcyCurrency, dt, "Id", "CurrencyCode", "Fcy Currency", AllColumns: true, Conversion.ToInt(CmbFcyCurrency.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbTransactionCurrencyDetail, dt, "Id", "CurrencyCode", "Transaction Currency", AllColumns: true, Conversion.ToInt(CmbTransactionCurrencyDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DealStatusBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbDealStatus, dt, "Id", "Name", "Deal Status", AllColumns: true, Conversion.ToInt(CmbDealStatus.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTransactionCurrency_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			CmbTransactionCurrencyDetail.Value = CmbTransactionCurrency.Value;
			CalculateAmountsInDetail();
			AmountUpdateInCustomerDetailGrid();
			if (Conversion.ToInt(CmbTransactionCurrencyDetail.Value) > 0 && ((UltraGridBase)CmbTransactionCurrencyDetail).ActiveRow != null)
			{
				double Rate = Conversion.ToDouble(((UltraDropDownBase)CmbTransactionCurrencyDetail).SelectedRow.Cells[2].Value);
				if (Rate > 0.0)
				{
					txtTransactionExchangeRate.Text = ((Conversion.ToDouble((object)txtTransactionExchangeRate.Text) > 0.0) ? txtTransactionExchangeRate.Text : Rate.ToString(clsGlobalVariables.DecimalFCYRateFormate));
				}
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
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 10).ToList();
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				object[] rowData = new object[6] { item.Id, item.CompanyName, item.PartyCode, item.GlAccountId, item.CityName, item.MobilePersonal };
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
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomerName_Leave(object sender, EventArgs e)
	{
	}

	private void PaymentTermsBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTerm, dt, "Id", "Name", "Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTerm.Value), insertDefaultRow: true, null, null, ActivateRow: true, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VesselRoutingTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbVesselRoutingType, dt, "Id", "Name", "Vessel Routing Type", AllColumns: true, Conversion.ToInt(CmbVesselRoutingType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			string DisplayMember = (RadItemNamePackListDetail.Checked ? "Item Name" : "Item Code");
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

	private void dtExportInvoiceDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtInvoice.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtInvoice = ExImInvoice.getExportInvoicePendingAndAll(obj);
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
			((Control)(object)CmbBrokerAgentName, "Broker Agent", FormHelper.ValidType.INT),
			((Control)(object)CmbServiceProvider, "Service Provider", FormHelper.ValidType.INT),
			((Control)(object)CmbPaymentTerm, "PaymentTerm", FormHelper.ValidType.INT),
			((Control)(object)CmbVesselRoutingType, "Vessel Routing Type", FormHelper.ValidType.INT),
			((Control)(object)CmbItemName, "Item Name", FormHelper.ValidType.INT),
			(txtServiceRate, "Service Rate", FormHelper.ValidType.Double),
			((Control)(object)CmbRateBaseType, "Rate Base", FormHelper.ValidType.INT),
			(txtTcyAmount, "Tcy Amount", FormHelper.ValidType.Double),
			(txtLcyAmountDetail, "Lcy Amount", FormHelper.ValidType.Double),
			(txtOfferValidityDays, "Offer Validity Days", FormHelper.ValidType.INT)
		}))
		{
			return false;
		}
		if (txtCargoCutOffDate.Value.Date > txtEtdSailing.Value.Date)
		{
			MessageBox.Show("ETD/Sailing Date must be greater than or equal to Cargo Cut-Off Date.");
			txtEtdSailing.Focus();
			return false;
		}
		return true;
	}

	private void BtnAddCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationCustomerDetail())
			{
				DataRow dr = dtCustomerDetail.NewRow();
				dr["Id"] = 0;
				FillCustomerDetailRow(dr);
				dtCustomerDetail.Rows.Add(dr);
				grdCustomerDetail.DataSource = dtCustomerDetail;
				grdCustomerDetail.RetrieveStructure();
				grdCustomerDetailSetting(grdCustomerDetail);
				ResetCustomerDetail();
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
				SourceDocumentGridcomboBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCustomerDetailRow(DataRow dr)
	{
		dr["BrokerAgentId"] = Conversion.ToInt(CmbBrokerAgentName.Value);
		dr["BrokerAgent"] = Conversion.ToString((object)((Control)(object)CmbBrokerAgentName).Text);
		dr["ServiceProviderId"] = Conversion.ToInt(CmbServiceProvider.Value);
		dr["ServiceProviderName"] = Conversion.ToString((object)((Control)(object)CmbServiceProvider).Text);
		dr["PaymentTermId"] = Conversion.ToInt(CmbPaymentTerm.Value);
		dr["PaymentTerm"] = Conversion.ToString((object)((Control)(object)CmbPaymentTerm).Text);
		dr["DueDays"] = Conversion.ToInt((object)txtDueDays.Text);
		dr["VesselRoutingTypeId"] = Conversion.ToInt(CmbVesselRoutingType.Value);
		dr["VesselRoutingType"] = Conversion.ToString((object)((Control)(object)CmbVesselRoutingType).Text);
		dr["VesselRoutingDescription"] = Conversion.ToString((object)txtVesselRoutingDescription.Text);
		int itemId = Conversion.ToInt(CmbItemName.Value);
		dr["ItemId"] = itemId;
		object value;
		if (itemId <= 0)
		{
			value = "";
		}
		else
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbItemName).SelectedRow;
			value = Conversion.ToString((selectedRow != null) ? selectedRow.Cells["ItemName"].Value : null);
		}
		dr["ItemName"] = value;
		object value2;
		if (itemId <= 0)
		{
			value2 = "";
		}
		else
		{
			UltraGridRow selectedRow2 = ((UltraDropDownBase)CmbItemName).SelectedRow;
			value2 = Conversion.ToString((selectedRow2 != null) ? selectedRow2.Cells["ItemCode"].Value : null);
		}
		dr["ItemCode"] = value2;
		dr["TransactionCurrencyId"] = Conversion.ToInt(CmbTransactionCurrencyDetail.Value);
		dr["TransactionCurrency"] = Conversion.ToString((object)((Control)(object)CmbTransactionCurrencyDetail).Text);
		dr["ServiceRate"] = Conversion.ToDecimal((object)txtServiceRate.Text);
		int rateBaseId = Conversion.ToInt(CmbRateBaseType.Value);
		dr["RateBaseId"] = rateBaseId;
		dr["RateBase"] = Conversion.ToString((object)((Control)(object)CmbRateBaseType).Text);
		double num;
		if (rateBaseId <= 0)
		{
			num = 0.0;
		}
		else
		{
			UltraGridRow selectedRow3 = ((UltraDropDownBase)CmbRateBaseType).SelectedRow;
			num = Conversion.ToDouble((selectedRow3 != null) ? selectedRow3.Cells[2].Value : null);
		}
		dr["RateBaseEquivalent"] = num;
		dr["TcyAmount"] = Conversion.ToDecimal((object)txtTcyAmount.Text);
		dr["LcyAmount"] = Conversion.ToDecimal((object)txtLcyAmountDetail.Text);
		dr["OfferValidityDays"] = Conversion.ToInt((object)txtOfferValidityDays.Text);
		dr["OfferValidityDate"] = Conversion.ToDateTime((object)txtOfferValidityDate.Value);
		dr["CargoCutOffDate"] = Conversion.ToDateTime((object)txtCargoCutOffDate.Value);
		dr["EtdSealingDate"] = Conversion.ToDateTime((object)txtEtdSailing.Value);
		dr["TransitsTimeDays"] = Conversion.ToInt((object)txtTransitDays.Text);
		dr["FreeDaysAtPOD"] = Conversion.ToInt((object)txtFreeDaysAtPOD.Text);
		dr["Remarks"] = Conversion.ToString((object)txtRemarksHeader.Text);
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
				updateDetailIndex = row.RowIndex;
				((Control)(object)CmbMasterItem).Text = "";
				ItemDtsFillFromGlobal();
				ItemNameBind();
				CmbBrokerAgentName.Value = row.Cells["BrokerAgentId"].Value;
				CmbServiceProvider.Value = row.Cells["ServiceProviderId"].Value;
				CmbPaymentTerm.Value = row.Cells["PaymentTermId"].Value;
				txtDueDays.Text = Conversion.ToString(row.Cells["DueDays"].Value);
				CmbVesselRoutingType.Value = row.Cells["VesselRoutingTypeId"].Value;
				txtVesselRoutingDescription.Text = Conversion.ToString(row.Cells["VesselRoutingDescription"].Value);
				CmbItemName.Value = row.Cells["ItemId"].Value;
				CmbTransactionCurrencyDetail.Value = row.Cells["TransactionCurrencyId"].Value;
				txtServiceRate.Text = Conversion.ToString(row.Cells["ServiceRate"].Value);
				CmbRateBaseType.Value = row.Cells["RateBaseId"].Value;
				txtTcyAmount.Text = Conversion.ToString(row.Cells["TcyAmount"].Value);
				txtLcyAmountDetail.Text = Conversion.ToString(row.Cells["LcyAmount"].Value);
				txtOfferValidityDays.Text = Conversion.ToString(row.Cells["OfferValidityDays"].Value);
				txtOfferValidityDate.Value = Conversion.ToDateTime(row.Cells["OfferValidityDate"].Value);
				txtCargoCutOffDate.Value = Conversion.ToDateTime(row.Cells["CargoCutOffDate"].Value);
				txtEtdSailing.Value = Conversion.ToDateTime(row.Cells["EtdSealingDate"].Value);
				txtTransitDays.Text = Conversion.ToString(row.Cells["TransitsTimeDays"].Value);
				txtFreeDaysAtPOD.Text = Conversion.ToString(row.Cells["FreeDaysAtPOD"].Value);
				txtRemarksHeader.Text = Conversion.ToString(row.Cells["Remarks"].Value);
				BtnAddCustomer.Visible = false;
				BtnUpdateCustmer.Visible = true;
				BtnCancelCustomer.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetailSetting(GridEX gridEX)
	{
		try
		{
			bool num = ((Control)(object)gridEX).Name == "grdCustomerDetail";
			List<string> hiddenColumns = new List<string> { "Id", "BrokerAgentId", "ServiceProviderId", "PaymentTermId", "VesselRoutingTypeId", "ItemId", "TransactionCurrencyId", "RateBaseId", "RateBaseEquivalent" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>();
			if (num)
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"ServiceRate",
						(EditType)1
					},
					{
						"OfferValidityDays",
						(EditType)1
					},
					{
						"OfferValidityDate",
						(EditType)2
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["BrokerAgent"].Width = 150;
			gridEX.RootTable.Columns["ServiceProviderName"].Width = 150;
			gridEX.RootTable.Columns["PaymentTerm"].Width = 100;
			gridEX.RootTable.Columns["DueDays"].Width = 70;
			gridEX.RootTable.Columns["VesselRoutingType"].Width = 120;
			gridEX.RootTable.Columns["ItemName"].Width = 160;
			gridEX.RootTable.Columns["TransactionCurrency"].Width = 80;
			gridEX.RootTable.Columns["ServiceRate"].Width = 80;
			gridEX.RootTable.Columns["RateBase"].Width = 120;
			gridEX.RootTable.Columns["TcyAmount"].Width = 90;
			gridEX.RootTable.Columns["LcyAmount"].Width = 90;
			gridEX.RootTable.Columns["OfferValidityDays"].Width = 90;
			gridEX.RootTable.Columns["OfferValidityDate"].Width = 90;
			gridEX.RootTable.Columns["CargoCutOffDate"].Width = 80;
			gridEX.RootTable.Columns["EtdSealingDate"].Width = 80;
			gridEX.RootTable.Columns["TransitsTimeDays"].Width = 90;
			gridEX.RootTable.Columns["FreeDaysAtPOD"].Width = 90;
			gridEX.RootTable.Columns["Remarks"].Width = 180;
			if (num)
			{
				GridEX_Helper.AddButton(grdCustomerDetail, "Delete", "X", 20, 0);
				GridEX_Helper.AddButton(grdCustomerDetail, "Edit", "Edit", 40, 1);
				grdCustomerDetail.FrozenColumns = 3;
				SourceDocumentGridcomboBind();
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
			if (row != null)
			{
				if (e.Column.Key == "ServiceRate" || e.Column.Key == "RateBaseEquivalent")
				{
					CustomerDetail_AmountUpdateInRow(row);
				}
				else if (e.Column.Key == "OfferValidityDays")
				{
					int offerValidityDays = Conversion.ToInt(row.Cells["OfferValidityDays"].Value);
					DateTime offerValidityDate = txtdocdate.Value.AddDays(offerValidityDays);
					row.Cells["OfferValidityDate"].Value = offerValidityDate;
				}
				else if (e.Column.Key == "OfferValidityDate")
				{
					int offerValidityDays2 = (Conversion.ToDateTime(row.Cells["OfferValidityDate"].Value) - txtdocdate.Value).Days;
					row.Cells["OfferValidityDays"].Value = offerValidityDays2;
				}
				grdCustomerDetail.UpdateData();
				SourceDocumentGridcomboBind();
			}
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
			if (row != null)
			{
				decimal num = Conversion.ToDecimal(row.Cells["ServiceRate"].Value);
				decimal rateBaseEquivalent = Conversion.ToDecimal(row.Cells["RateBaseEquivalent"].Value);
				decimal tcyAmount = num * rateBaseEquivalent;
				row.Cells["TcyAmount"].Value = tcyAmount;
				decimal exchangeRate = Conversion.ToDecimal((object)txtTransactionExchangeRate.Text);
				row.Cells["LcyAmount"].Value = tcyAmount * exchangeRate;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
				row.Cells["TransactionCurrencyId"].Value = Conversion.ToInt(CmbTransactionCurrency.Value);
				row.Cells["TransactionCurrency"].Value = Conversion.ToString((object)((Control)(object)CmbTransactionCurrency).Text);
				row.EndEdit();
			}
			grdCustomerDetail.UpdateData();
			SourceDocumentGridcomboBind();
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
				if (e.Column.Key == "Add")
				{
					AddCustomerDetailRow(r);
				}
				grdCustomerDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddCustomerDetailRow(GridEXRow r)
	{
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
					logisticRateNegotiationDetail d = new logisticRateNegotiationDetail();
					d.logisticRateNegotiationDetailId = Conversion.ToInt(r.Cells["Id"].Value);
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
			SourceDocumentGridcomboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilldtBrokeragentFromDetailGrid()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			dtBrokeragentFromDetailGrid.Clear();
			if (grdCustomerDetail.CurrentRow == null || (int)grdCustomerDetail.CurrentRow.RowType != 0)
			{
				return;
			}
			HashSet<int> brokerAgentIds = new HashSet<int>();
			GridEXRow[] rows = grdCustomerDetail.GetRows();
			foreach (GridEXRow row in rows)
			{
				if ((int)row.RowType == 0)
				{
					int brokerAgentId = Conversion.ToInt(row.Cells["BrokerAgentId"].Value);
					if (brokerAgentIds.Add(brokerAgentId))
					{
						dtBrokeragentFromDetailGrid.Rows.Add(brokerAgentId, Conversion.ToString(row.Cells["BrokerAgent"].Value), Conversion.ToInt(row.Cells["ServiceProviderId"].Value), Conversion.ToString(row.Cells["ServiceProviderName"].Value));
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInSourceDocumentGrid()
	{
		dtSourceDocument.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
		grdSourceDocument.DataSource = dtSourceDocument;
	}

	private void SourceDocumentGridcomboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdSourceDocument.RootTable.Columns["DocumentId"], dtInvoice, "Id", "InvoiceNo", "Source Document No", 100);
			FilldtBrokeragentFromDetailGrid();
			GridEX_Helper.ComboBind(grdSourceDocument.RootTable.Columns["BrokerAgentId"], dtBrokeragentFromDetailGrid, "BrokerAgentId", "BrokerAgent", "Broker Agent", 100);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSourceDocumentSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "ServiceProviderId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"Remarks",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdSourceDocument, 2, 3, hiddenColumns, editableColumns);
			grdSourceDocument.RootTable.Columns["NoOfContainers"].Width = Constants.InventoryConstants.ContainerConstant;
			grdSourceDocument.RootTable.Columns["MTon"].Width = Constants.InventoryConstants.QtyConstant;
			GridEX_Helper.AddButton(grdSourceDocument, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdSourceDocument, "Add", "+", 20, 1);
			grdSourceDocument.FrozenColumns = 2;
			SourceDocumentGridcomboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSourceDocument_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdSourceDocument.UpdateData();
			GridEXRow r = grdSourceDocument.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "DocumentId")
				{
					int documentId = Conversion.ToInt(r.Cells["DocumentId"].Value);
					GridEXRow[] dataRows = grdSourceDocument.GetDataRows();
					foreach (GridEXRow row in dataRows)
					{
						if (Conversion.ToInt(row.Cells["DocumentId"].Value) == documentId && row.RowIndex != r.RowIndex)
						{
							r.Cells["DocumentId"].Value = 0;
							r.Cells["NoOfContainers"].Value = 0;
							r.Cells["MTon"].Value = 0;
							grdSourceDocument.UpdateData();
							throw new Exception("This Document Already Exists in another row");
						}
					}
					DataRow[] dr2 = dtInvoice.Select("Id=" + documentId);
					if (dr2.Length != 0)
					{
						r.Cells["DocumentTypeId"].Value = Conversion.ToInt(dr2[0]["DocumentTypeId"]);
						r.Cells["NoOfContainers"].Value = Conversion.ToInt(dr2[0]["NoOfContainers"]);
						r.Cells["MTon"].Value = Conversion.ToDouble(dr2[0]["MTon"]);
					}
				}
				if (e.Column.Key == "BrokerAgentId")
				{
					int brokerAgentId = Conversion.ToInt(r.Cells["BrokerAgentId"].Value);
					DataRow[] dr = dtBrokeragentFromDetailGrid.Select("BrokerAgentId=" + brokerAgentId);
					if (dr.Length != 0)
					{
						r.Cells["ServiceProviderId"].Value = Conversion.ToInt(dr[0]["ServiceProviderId"]);
					}
				}
			}
			grdSourceDocument.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string GetBrokerNameFromDt(int brokerAgentId)
	{
		try
		{
			string Name = "";
			DataRow[] dr = dtSupplier.Select("Id=" + brokerAgentId);
			if (dr.Length != 0)
			{
				Name = Conversion.ToString(dr[0]["CompanyName"]);
			}
			return Name;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdSourceDocument_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSourceDocument.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteSourceDocumentDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInSourceDocumentGrid();
				}
				grdSourceDocument.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteSourceDocumentDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				logisticRateNegotiationSourceDocument d1 = new logisticRateNegotiationSourceDocument();
				d1.logisticRateNegotiationSourceDocumentId = Id;
				FillSourceDocumentDetailListCommonForInsertAndDelete(d1, r);
				d1.ActionTypeId = 3;
				lstRemoveRecordSourceDocument.Add(d1);
				dtSourceDocument.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			if (dtSourceDocument.Rows.Count == 0)
			{
				AddRowInSourceDocumentGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSourceDocument_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdSourceDocument.CurrentRow;
			GridEXColumn col = grdSourceDocument.CurrentColumn;
			if (row == null || (int)row.RowType != 0 || col == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (col.Key == "Add")
				{
					AddRowInSourceDocumentGrid();
				}
				else if (col.Key == "Delete")
				{
					DeleteSourceDocumentDetailRow(row);
				}
				grdSourceDocument.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInSourceDocumentGrid();
				grdSourceDocument.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteSourceDocumentDetailRow(row);
				grdSourceDocument.UpdateData();
			}
			else if (e.KeyCode == Keys.F1)
			{
				HandleF1Popup(row, col.Key);
				grdSourceDocument.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HandleF1Popup(GridEXRow row, string colKey)
	{
		if (colKey == "ItemName")
		{
			Conversion.ToInt(row.Cells["ItemId"].Value);
		}
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
			lstRemoveRecordSourceDocument.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			((Control)(object)CmbDealStatus).Enabled = false;
			((Control)(object)CmbLoadingPort).Text = string.Empty;
			((Control)(object)CmbDestinationPort).Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			ResetCustomerDetail();
			dtCustomerDetail.Rows.Clear();
			grdCustomerDetail.DataSource = null;
			dtSourceDocument.Rows.Clear();
			AddRowInSourceDocumentGrid();
			grdSourceDocument.DataSource = dtSourceDocument;
			grdSourceDocument.RetrieveStructure();
			grdSourceDocumentSettings();
			txtdocno.Text = DocumentNoDbCall();
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
			((Control)(object)CmbBrokerAgentName).Text = string.Empty;
			((Control)(object)CmbServiceProvider).Text = string.Empty;
			((Control)(object)CmbPaymentTerm).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			txtServiceRate.Text = string.Empty;
			((Control)(object)CmbRateBaseType).Text = string.Empty;
			txtTcyAmount.Text = string.Empty;
			txtLcyAmountDetail.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			updateDetailIndex = -1;
			BtnAddCustomer.Visible = true;
			BtnCancelCustomer.Visible = false;
			BtnUpdateCustmer.Visible = false;
			CmbBrokerAgentName.Focus();
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
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AllSerivesItems", "MultiCurrency" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			AllLookUpComboBind(LookUps.LookUpAllServices(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
			PortsBind(PortsDbCall());
			CurrencyBindFromGlobal();
			SupplierDtFillFromGlobal();
			BrokerAgentOrServiceProviderBind(CmbBrokerAgentName, dtSupplier, "Broker Agent");
			BrokerAgentOrServiceProviderBind(CmbServiceProvider, dtServiceProvider, "Service Provider");
			ItemCategoryOrMasterItemBind();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			GetConfigurationsFromGlobalandBind();
			dtExportInvoiceDbCall();
			SourceDocumentGridcomboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmLogisticRateNegotiation_FormClosed(object sender, FormClosedEventArgs e)
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

	private void ValidateBrokerDuplicationInDetailGrid()
	{
		var duplicates = (from x in grdCustomerDetail.GetDataRows().Select((GridEXRow row, int index) => new
			{
				RowNo = index + 1,
				BrokerAgentId = Conversion.ToInt(row.Cells["BrokerAgentId"].Value)
			})
			where x.BrokerAgentId > 0
			group x by x.BrokerAgentId into g
			where g.Count() > 1
			select new
			{
				BrokerAgentId = g.Key,
				Rows = g.Select(x => x.RowNo)
			}).ToList();
		if (duplicates.Any())
		{
			throw new Exception(string.Join(Environment.NewLine, duplicates.Select(d => "BrokerAgent '" + GetBrokerNameFromDt(d.BrokerAgentId) + "' is duplicated in row(s): " + string.Join(", ", d.Rows) + " in Broker Detail")));
		}
	}

	public void ValidateBrokerAgentMismatch(DataTable dtTarget, DataTable dtSource, string targetGridName = "Broker agent Detail", string sourceGridName = "Source Document Detail", int checkSpecific = 0)
	{
		var targetAgents = (from r in dtTarget.AsEnumerable()
			where r.Field<int>("BrokerAgentId") > 0
			select new
			{
				Id = r.Field<int>("BrokerAgentId"),
				Name = r.Field<string>("BrokerAgent")
			}).ToList();
		var sourceAgents = (from r in dtSource.AsEnumerable()
			where r.Field<int>("BrokerAgentId") > 0
			select new
			{
				Id = r.Field<int>("BrokerAgentId"),
				Name = GetBrokerNameFromDt(r.Field<int>("BrokerAgentId"))
			}).ToList();
		HashSet<int> targetIds = targetAgents.Select(x => x.Id).ToHashSet();
		HashSet<int> sourceIds = sourceAgents.Select(x => x.Id).ToHashSet();
		if (checkSpecific == 0 || checkSpecific == 1)
		{
			List<string> missingInTarget = (from x in sourceAgents
				where !targetIds.Contains(x.Id)
				select x.Name).Distinct().ToList();
			if (missingInTarget.Any())
			{
				throw new Exception("BrokerAgent '" + string.Join(", ", missingInTarget) + "' found in '" + sourceGridName + "' Grid but not in '" + targetGridName + "' Grid.");
			}
		}
		if (checkSpecific == 0 || checkSpecific == 2)
		{
			List<string> missingInSource = (from x in targetAgents
				where !sourceIds.Contains(x.Id)
				select x.Name).Distinct().ToList();
			if (missingInSource.Any())
			{
				throw new Exception("BrokerAgent " + string.Join(", '", missingInSource) + "' found in '" + targetGridName + "' Grid but not in '" + sourceGridName + "' Grid.");
			}
		}
	}

	private void Insert()
	{
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Expected O, but got Unknown
		try
		{
			if (grdCustomerDetail.GetRows().Count() == 0)
			{
				throw new Exception("Broker Agent Detail Record Not Found");
			}
			if (grdSourceDocument.GetRows().Count() == 0)
			{
				throw new Exception("Source Document Detail Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbServiceType, "Service Type", FormHelper.ValidType.INT),
				((Control)(object)CmbDestinationPort, "Destination Port", FormHelper.ValidType.INT),
				((Control)(object)CmbLoadingPort, "Loading Port", FormHelper.ValidType.INT),
				((Control)(object)CmbTransactionCurrency, "Transaction Currency", FormHelper.ValidType.INT),
				(txtTransactionExchangeRate, "Tcy Exchange Rate", FormHelper.ValidType.Double),
				((Control)(object)CmbFcyCurrency, "Foreign Currency", FormHelper.ValidType.INT),
				(txtFcyExchangeRate, "Fcy Rate", FormHelper.ValidType.Double)
			}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			ValidateBrokerDuplicationInDetailGrid();
			ValidateBrokerAgentMismatch(dtCustomerDetail, dtSourceDocument);
			logisticRateNegotiationHeader Or = new logisticRateNegotiationHeader();
			Or.logisticRateNegotiationHeaderId = RecId;
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchesId = UserAccount.BranchesId;
			Or.ProjectsId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Or.documentTypeId = DocumentTypeId;
			Or.documentDate = Conversion.ToDateTime((object)txtdocdate.Value);
			Or.documentNo = Conversion.ToInt((object)txtdocno.Text);
			Or.ServiceTypeId = Conversion.ToInt(CmbServiceType.Value);
			Or.fcyCurrencyId = Conversion.ToInt(CmbFcyCurrency.Value);
			Or.fcyExchangeRate = Conversion.ToDouble((object)txtFcyExchangeRate.Text);
			Or.transactionCurrencyId = Conversion.ToInt(CmbTransactionCurrency.Value);
			Or.transactionExchangeRate = Conversion.ToDouble((object)txtTransactionExchangeRate.Text);
			Or.DealStatusId = Conversion.ToInt(CmbDealStatus.Value);
			Or.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ApprovedUserId = UserAccount.ID;
			Or.ApprovedDate = DateTime.Now;
			Or.IsApproved = false;
			Or.logisticRateNegotiationDetailList = new List<logisticRateNegotiationDetail>();
			Or.logisticRateNegotiationSourceDocumentList = new List<logisticRateNegotiationSourceDocument>();
			if (lstRemoveRecordcustomerDetail.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < lstRemoveRecordcustomerDetail.Count; j++)
				{
					Or.logisticRateNegotiationDetailList.Add(lstRemoveRecordcustomerDetail[j]);
				}
			}
			GridEXRow[] rows = grdCustomerDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				logisticRateNegotiationDetail vd2 = new logisticRateNegotiationDetail();
				vd2.logisticRateNegotiationDetailId = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				vd2.ActionTypeId = ((vd2.logisticRateNegotiationDetailId <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd2, r2);
				FormHelper.ValidateField(vd2.partyIdAgent, "Broker Agent", r2.RowIndex);
				FormHelper.ValidateField(vd2.serviceProviderId, "ServiceProvider", r2.RowIndex);
				FormHelper.ValidateField(vd2.paymentTermId, "Payment Term", r2.RowIndex);
				FormHelper.ValidateField(vd2.DueDays, "DueDays", r2.RowIndex);
				FormHelper.ValidateField(vd2.VesselRoutingTypeId, "VesselRouting Type", r2.RowIndex);
				FormHelper.ValidateField(vd2.ServiceItemId, "Service Item", r2.RowIndex);
				FormHelper.ValidateField(vd2.tcyCurrencyId, "Transaction Currency", r2.RowIndex);
				FormHelper.ValidateField(vd2.serviceRate, "Service Rate", r2.RowIndex);
				FormHelper.ValidateField(vd2.serviceRateBaseUomId, "Rate Base", r2.RowIndex);
				FormHelper.ValidateField(vd2.tcyAmount, "Tcy amount", r2.RowIndex);
				FormHelper.ValidateField(vd2.lcyAmount, "Lcy amount", r2.RowIndex);
				FormHelper.ValidateField(vd2.OffervalidityDays, "Offer Validity Days", r2.RowIndex);
				if (string.IsNullOrWhiteSpace(Or.RemarksHeader))
				{
					Or.RemarksHeader = vd2.Remarks;
				}
				Or.logisticRateNegotiationDetailList.Add(vd2);
			}
			if (lstRemoveRecordSourceDocument.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecordSourceDocument.Count; i++)
				{
					Or.logisticRateNegotiationSourceDocumentList.Add(lstRemoveRecordSourceDocument[i]);
				}
			}
			rows = grdSourceDocument.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["DocumentTypeId"].Value) > 0)
				{
					logisticRateNegotiationSourceDocument vd = new logisticRateNegotiationSourceDocument();
					vd.logisticRateNegotiationSourceDocumentId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.ActionTypeId = ((vd.logisticRateNegotiationSourceDocumentId <= 0) ? 1 : 2);
					FillSourceDocumentDetailListCommonForInsertAndDelete(vd, r);
					FormHelper.ValidateField(vd.SourceDocumentTypeId, "Document Type", r.RowIndex, "Source Document Detail");
					FormHelper.ValidateField(vd.SourceDocumentId, "Source Document No", r.RowIndex, "Source Document Detail");
					FormHelper.ValidateField(vd.BrokerAgentNameId, "Broker Agent Name", r.RowIndex, "Source Document Detail");
					FormHelper.ValidateField(vd.ServiceProviderId, "Service Provider", r.RowIndex, "Source Document Detail");
					Or.logisticRateNegotiationSourceDocumentList.Add(vd);
				}
			}
			Or.AttachmentsList = new List<DMSAttachments>();
			Or.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<logisticRateNegotiationHeader>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Or.AttachmentsValues = AttachmentsValues;
			Or.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = logisticRateNegotiationHeader.Save(Or);
			Or.logisticRateNegotiationHeaderId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + Or.documentNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + Or.documentNo);
			}
			FromReset();
			if (ChkPreview.Checked && formright.DoHavePrintRights)
			{
				LogisticRateNegotiationSlip(Or.logisticRateNegotiationHeaderId);
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
			logisticRateNegotiationHeader Or = logisticRateNegotiationHeader.ReadById(RecId);
			if (Or == null || Or.logisticRateNegotiationDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocdate.Value = Or.documentDate;
			txtdocno.Text = Conversion.ToString((object)Or.documentNo);
			CmbServiceType.Value = Conversion.ToInt((object)Or.ServiceTypeId);
			UltraCombo cmbDestinationPort = CmbDestinationPort;
			logisticRateNegotiationDetail obj = Or.logisticRateNegotiationDetailList[0];
			cmbDestinationPort.Value = Conversion.ToInt((object)((obj != null) ? new int?(obj.portOfDischargeId) : null));
			UltraCombo cmbLoadingPort = CmbLoadingPort;
			logisticRateNegotiationDetail obj2 = Or.logisticRateNegotiationDetailList[0];
			cmbLoadingPort.Value = Conversion.ToInt((object)((obj2 != null) ? new int?(obj2.portOfLoadingId) : null));
			CmbTransactionCurrency.Value = Conversion.ToInt((object)Or.transactionCurrencyId);
			txtTransactionExchangeRate.Text = Or.transactionExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			CmbFcyCurrency.Value = Conversion.ToInt((object)Or.fcyCurrencyId);
			txtFcyExchangeRate.Text = Or.fcyExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			CmbDealStatus.Value = Conversion.ToInt((object)Or.DealStatusId);
			txtRemarksHeader.Text = Conversion.ToString((object)Or.RemarksHeader);
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			((Control)(object)CmbDealStatus).Enabled = Or.IsApproved;
			dtCustomerDetail.Rows.Clear();
			FillDetailFromListCommonForReadById(Or, dtCustomerDetail);
			grdCustomerDetail.DataSource = dtCustomerDetail;
			grdCustomerDetail.RetrieveStructure();
			grdCustomerDetailSetting(grdCustomerDetail);
			dtSourceDocument.Rows.Clear();
			FillSourceDocumentDetailFromListCommonForReadById(Or, dtSourceDocument);
			if (dtSourceDocument.Rows.Count == 0)
			{
				AddRowInSourceDocumentGrid();
			}
			grdSourceDocument.DataSource = dtSourceDocument;
			grdSourceDocument.RetrieveStructure();
			grdSourceDocumentSettings();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			txtdocdate.Focus();
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
					logisticRateNegotiationHeader.DeleteByID(UserAccount.ID, RecId);
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

	private void btnSaveAs_Click(object sender, EventArgs e)
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

	private void FillSourceDocumentDetailListCommonForInsertAndDelete(logisticRateNegotiationSourceDocument vd, GridEXRow r)
	{
		vd.SourceDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
		vd.SourceDocumentId = Conversion.ToInt(r.Cells["DocumentId"].Value);
		vd.SourceDocumentNoOfContainers = Conversion.ToInt(r.Cells["NoOfContainers"].Value);
		vd.SourceDocumentMTon = Conversion.ToInt(r.Cells["MTon"].Value);
		vd.BrokerAgentNameId = Conversion.ToInt(r.Cells["BrokerAgentId"].Value);
		vd.ServiceProviderId = Conversion.ToInt(r.Cells["ServiceProviderId"].Value);
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void FillDetailListCommonForInsertAndDelete(logisticRateNegotiationDetail vd, GridEXRow r)
	{
		vd.serviceTypeId = Conversion.ToInt(CmbServiceType.Value);
		vd.partyIdAgent = Conversion.ToInt(r.Cells["BrokerAgentId"].Value);
		vd.BrokerAgentName = Conversion.ToString(r.Cells["BrokerAgent"].Value);
		vd.serviceProviderId = Conversion.ToInt(r.Cells["ServiceProviderId"].Value);
		vd.ServiceProviderName = Conversion.ToString(r.Cells["ServiceProviderName"].Value);
		vd.portOfDischargeId = Conversion.ToInt(CmbDestinationPort.Value);
		vd.portOfLoadingId = Conversion.ToInt(CmbLoadingPort.Value);
		vd.paymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
		vd.PaymentTerm = Conversion.ToString(r.Cells["PaymentTerm"].Value);
		vd.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
		vd.VesselRoutingTypeId = Conversion.ToInt(r.Cells["VesselRoutingTypeId"].Value);
		vd.VesselRoutingType = Conversion.ToString(r.Cells["VesselRoutingType"].Value);
		vd.VesselRoutingDescription = Conversion.ToString(r.Cells["VesselRoutingDescription"].Value);
		vd.ServiceItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.tcyCurrencyId = Conversion.ToInt(r.Cells["TransactionCurrencyId"].Value);
		vd.TransactionCurrencyCode = Conversion.ToString(r.Cells["TransactionCurrency"].Value);
		vd.tcyExchangeRate = Conversion.ToDouble((object)txtTransactionExchangeRate.Text);
		vd.serviceRate = Conversion.ToDouble(r.Cells["ServiceRate"].Value);
		vd.serviceRateBaseUomId = Conversion.ToInt(r.Cells["RateBaseId"].Value);
		vd.RateBaseUom = Conversion.ToString(r.Cells["RateBase"].Value);
		vd.RateBaseUomEquivalent = Conversion.ToDouble(r.Cells["RateBaseEquivalent"].Value);
		vd.tcyAmount = Conversion.ToDouble(r.Cells["TcyAmount"].Value);
		vd.lcyAmount = Conversion.ToDouble(r.Cells["LcyAmount"].Value);
		vd.fcyAmount = ((Conversion.ToDouble((object)txtFcyExchangeRate.Text) > 0.0) ? (vd.lcyAmount / Conversion.ToDouble((object)txtFcyExchangeRate.Text)) : 0.0);
		vd.OffervalidityDays = Conversion.ToInt(r.Cells["OfferValidityDays"].Value);
		vd.OfferValidityDate = Conversion.ToDateTime(r.Cells["OfferValidityDate"].Value);
		vd.cutOffDate = Conversion.ToDateTime(r.Cells["CargoCutOffDate"].Value);
		vd.etdPOL = Conversion.ToDateTime(r.Cells["EtdSealingDate"].Value);
		vd.vesselTransitDays = Conversion.ToInt(r.Cells["TransitsTimeDays"].Value);
		vd.freeDaysAtPOD = Conversion.ToInt(r.Cells["FreeDaysAtPOD"].Value);
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void FillSourceDocumentDetailFromListCommonForReadById(logisticRateNegotiationHeader invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.logisticRateNegotiationSourceDocumentList : null) == null || invGrn.logisticRateNegotiationSourceDocumentList.Count == 0)
		{
			return;
		}
		foreach (logisticRateNegotiationSourceDocument detail in invGrn.logisticRateNegotiationSourceDocumentList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.logisticRateNegotiationSourceDocumentId;
			dr["DocumentTypeId"] = detail.SourceDocumentTypeId;
			dr["DocumentId"] = detail.SourceDocumentId;
			dr["NoOfContainers"] = detail.SourceDocumentNoOfContainers;
			dr["MTon"] = detail.SourceDocumentMTon;
			dr["BrokerAgentId"] = detail.BrokerAgentNameId;
			dr["ServiceProviderId"] = detail.ServiceProviderId;
			dr["Remarks"] = detail.Remarks;
			dt.Rows.Add(dr);
		}
	}

	private void FillDetailFromListCommonForReadById(logisticRateNegotiationHeader invGrn, DataTable dt)
	{
		if (invGrn != null && invGrn.logisticRateNegotiationDetailList.Count == 0)
		{
			return;
		}
		foreach (logisticRateNegotiationDetail detail in invGrn.logisticRateNegotiationDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.logisticRateNegotiationDetailId;
			dr["BrokerAgentId"] = detail.partyIdAgent;
			dr["BrokerAgent"] = detail.BrokerAgentName;
			dr["ServiceProviderId"] = detail.serviceProviderId;
			dr["ServiceProviderName"] = detail.ServiceProviderName;
			dr["PaymentTermId"] = detail.paymentTermId;
			dr["PaymentTerm"] = detail.PaymentTerm;
			dr["DueDays"] = detail.DueDays;
			dr["VesselRoutingTypeId"] = detail.VesselRoutingTypeId;
			dr["VesselRoutingType"] = detail.VesselRoutingType;
			dr["VesselRoutingDescription"] = detail.VesselRoutingDescription;
			dr["ItemId"] = detail.ServiceItemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.ItemName;
			dr["TransactionCurrencyId"] = detail.tcyCurrencyId;
			dr["TransactionCurrency"] = detail.TransactionCurrencyCode;
			dr["ServiceRate"] = detail.serviceRate;
			dr["RateBaseId"] = detail.serviceRateBaseUomId;
			dr["RateBase"] = detail.RateBaseUom;
			dr["RateBaseEquivalent"] = detail.RateBaseUomEquivalent;
			dr["TcyAmount"] = detail.tcyAmount;
			dr["LcyAmount"] = detail.lcyAmount;
			dr["OfferValidityDays"] = detail.OffervalidityDays;
			dr["OfferValidityDate"] = detail.OfferValidityDate;
			dr["CargoCutOffDate"] = detail.cutOffDate;
			dr["EtdSealingDate"] = detail.etdPOL;
			dr["TransitsTimeDays"] = detail.vesselTransitDays;
			dr["FreeDaysAtPOD"] = detail.freeDaysAtPOD;
			dr["Remarks"] = detail.Remarks;
			dt.Rows.Add(dr);
		}
	}

	private void txtServiceRate_TextChanged(object sender, EventArgs e)
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

	private void CmbRateBaseType_Leave(object sender, EventArgs e)
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
			double tcyAmount = num * rateBaseEquivalent;
			txtTcyAmount.Text = tcyAmount.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			double tcyExchangeRate = Conversion.ToDouble((object)txtTransactionExchangeRate.Text);
			double lcyAmount = ((tcyExchangeRate > 0.0) ? (tcyAmount * tcyExchangeRate) : 0.0);
			txtLcyAmountDetail.Text = lcyAmount.ToString(clsGlobalVariables.DecimalFCYRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOfferValidityDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int offerValidityDays = Conversion.ToInt((object)txtOfferValidityDays.Text);
			DateTime offerValidityDate = Conversion.ToDateTime((object)txtdocdate.Value).AddDays(offerValidityDays);
			txtOfferValidityDate.Value = offerValidityDate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOfferValidityDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime dateTime = Conversion.ToDateTime((object)txtOfferValidityDate.Value);
			DateTime docDate = Conversion.ToDateTime((object)txtdocdate.Value);
			int offerValidityDays = (dateTime - docDate).Days;
			txtOfferValidityDays.Text = offerValidityDays.ToString();
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
			LogisticRateNegotiationSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LogisticRateNegotiationSlip(int PrintId)
	{
		try
		{
			CrystalReportPrint_Helper.LogisticRateNegotiationSlip_1300(PrintId);
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
		//IL_002c: Expected O, but got Unknown
		try
		{
			return logisticRateNegotiationHeader.GetDataForDropDownFromlogisticRateNegotiationHeader(new ReportsParameters
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
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "DealStatus")
				{
					dtStatus.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbServiceTypeHistory, dtST, "Id", "Name", "Service Type", AllColumns: true, Conversion.ToInt(CmbServiceTypeHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDealStatusHistory, dtStatus, "Id", "Name", "Deal Status", AllColumns: true, Conversion.ToInt(CmbDealStatusHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
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
			((Control)(object)CmbDealStatusHistory).Text = string.Empty;
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
			obj.StatusId = Conversion.ToInt(CmbDealStatusHistory.Value);
			DataTable dtHistory = logisticRateNegotiationHeader.FormHistory(obj);
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
					dtHistoryGrid.Columns.Add("DealStatusId", typeof(int));
					dtHistoryGrid.Columns.Add("DealStatus", typeof(string));
					dtHistoryGrid.Columns.Add("TransactionCurrencyId", typeof(int));
					dtHistoryGrid.Columns.Add("TcyCode", typeof(string));
					dtHistoryGrid.Columns.Add("TransactionExchangeRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("FcyCurrencyId", typeof(int));
					dtHistoryGrid.Columns.Add("FcyCode", typeof(string));
					dtHistoryGrid.Columns.Add("FcyExchangeRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUserName", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUserName", typeof(string));
					dtHistoryGrid.Columns.Add("IsApproved", typeof(bool));
					dtHistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ApprovalUserName", typeof(string));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistoryGrid.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtHistoryGrid.Rows.Add(row["logisticRateNegotiationHeaderId"], row["documentTypeId"], row["documentNo"], row["documentDate"], row["ServiceTypeId"], row["ServiceType"], row["DealStatusId"], row["DealStatus"], row["transactionCurrencyId"], row["TcyCode"], row["transactionExchangeRate"], row["fcyCurrencyId"], row["FcyCode"], row["fcyExchangeRate"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovalUserName"], row["NoOfAttachments"]);
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "ServiceTypeId", "DealStatusId", "TransactionCurrencyId", "FcyCurrencyId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 3, hiddenColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocumentDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["DocumentNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			grdHistory.RootTable.Columns["DocumentDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ServiceType"].Width = 150;
			grdHistory.RootTable.Columns["DealStatus"].Width = 120;
			grdHistory.RootTable.Columns["TcyCode"].Width = 60;
			grdHistory.RootTable.Columns["TransactionExchangeRate"].Width = 90;
			grdHistory.RootTable.Columns["FcyCode"].Width = 60;
			grdHistory.RootTable.Columns["FcyExchangeRate"].Width = 90;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ApprovalUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(grdHistory, "SaveAs", "Save As", 60, 1);
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 50, 2);
			grdHistory.FrozenColumns = 3;
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
				LogisticRateNegotiationSlip(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "SaveAs")
			{
				DataGridHistory_SaveAs(Conversion.ToInt(r.Cells["Id"].Value));
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

	private void DataGridHistory_SaveAs(int CurrId)
	{
		try
		{
			ReadById(CurrId);
			btnsave.Visible = false;
			btnDelete.Visible = false;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = true;
			txtdocdate.Value = DateTime.Now;
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
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			logisticRateNegotiationHeader Obj = logisticRateNegotiationHeader.ReadById(Id);
			if (Obj != null && Obj.logisticRateNegotiationDetailList.Count == 0)
			{
				grdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtCustomerDetail.Clone();
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			grddetailhistorySettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings()
	{
		try
		{
			grdCustomerDetailSetting(grdHistoryDetail);
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
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdCustomerDetail, dtCustomerDetail);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdSourceDocument, dtSourceDocument);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				else if (e.Control && e.KeyCode == Keys.F12)
				{
					if (btnSaveAs.Visible && btnSaveAs.Enabled)
					{
						btnSaveAs_Click(null, null);
					}
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
				if (grdCustomerDetail.CurrentColumn.Key == "Edit")
				{
					grdCustomerDetail_DoubleClick(null, null);
				}
				grdCustomerDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteCustomerDetailRow(r);
				grdCustomerDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdCustomerDetail_DoubleClick(null, null);
			}
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
					LogisticRateNegotiationSlip(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
				if (grdHistory.CurrentColumn.Key == "SaveAs")
				{
					DataGridHistory_SaveAs(CurrId);
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

	private void BtnReferenceParty_Click(object sender, EventArgs e)
	{
		try
		{
			FormHelper.CanOpenForm("supfrmDefineSupplier", delegate
			{
				supfrmDefineSupplier supfrmDefineSupplier = new supfrmDefineSupplier(UserAccount);
				supfrmDefineSupplier.Show();
				supfrmDefineSupplier.BringToFront();
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineLookUps_Click(object sender, EventArgs e)
	{
		try
		{
			new InvLookup(UserAccount).Show();
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
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Expected O, but got Unknown
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Expected O, but got Unknown
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Expected O, but got Unknown
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Expected O, but got Unknown
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Expected O, but got Unknown
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Expected O, but got Unknown
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Expected O, but got Unknown
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Expected O, but got Unknown
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Expected O, but got Unknown
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Expected O, but got Unknown
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Expected O, but got Unknown
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Expected O, but got Unknown
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Expected O, but got Unknown
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Expected O, but got Unknown
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a09: Expected O, but got Unknown
		//IL_0a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2a: Expected O, but got Unknown
		//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Expected O, but got Unknown
		//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Expected O, but got Unknown
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac4: Expected O, but got Unknown
		//IL_1044: Unknown result type (might be due to invalid IL or missing references)
		//IL_104e: Expected O, but got Unknown
		//IL_105b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1065: Expected O, but got Unknown
		//IL_1393: Unknown result type (might be due to invalid IL or missing references)
		//IL_139d: Expected O, but got Unknown
		//IL_13aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b4: Expected O, but got Unknown
		//IL_7d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d62: Expected O, but got Unknown
		//IL_7d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d90: Expected O, but got Unknown
		GridEXLayout grdCustomerDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSourceDocument_DesignTimeLayout = new GridEXLayout();
		Appearance appearance141 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
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
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
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
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
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
		Appearance appearance154 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
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
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
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
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.lgstcm.frmLogisticRateNegotiation));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdCustomerDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdSourceDocument = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtFcyExchangeRate = new System.Windows.Forms.TextBox();
		this.txtTransactionExchangeRate = new System.Windows.Forms.TextBox();
		this.CmbFcyCurrency = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbTransactionCurrency = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.CmbDealStatus = new UltraCombo();
		this.CmbDestinationPort = new UltraCombo();
		this.CmbLoadingPort = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbServiceType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.panel34 = new System.Windows.Forms.Panel();
		this.RadCategory = new System.Windows.Forms.RadioButton();
		this.RadMasterItem = new System.Windows.Forms.RadioButton();
		this.txtServiceRate = new System.Windows.Forms.TextBox();
		this.CmbMasterItem = new UltraCombo();
		this.RadItemCodePackListDetail = new System.Windows.Forms.RadioButton();
		this.CmbTransactionCurrencyDetail = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.RadItemNamePackListDetail = new System.Windows.Forms.RadioButton();
		this.CmbItemName = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.BtnAddCustomer = new System.Windows.Forms.Button();
		this.label40 = new System.Windows.Forms.Label();
		this.BtnCancelCustomer = new System.Windows.Forms.Button();
		this.label39 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.BtnUpdateCustmer = new System.Windows.Forms.Button();
		this.label44 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.txtTcyAmount = new System.Windows.Forms.TextBox();
		this.txtLcyAmountDetail = new System.Windows.Forms.TextBox();
		this.txtFreeDaysAtPOD = new System.Windows.Forms.TextBox();
		this.txtTransitDays = new System.Windows.Forms.TextBox();
		this.txtVesselRoutingDescription = new System.Windows.Forms.TextBox();
		this.txtOfferValidityDays = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.BtnReferenceParty = new System.Windows.Forms.Button();
		this.CmbPaymentTerm = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbServiceProvider = new UltraCombo();
		this.txtEtdSailing = new System.Windows.Forms.DateTimePicker();
		this.txtCargoCutOffDate = new System.Windows.Forms.DateTimePicker();
		this.CmbVesselRoutingType = new UltraCombo();
		this.CmbRateBaseType = new UltraCombo();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbBrokerAgentName = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnDefineLookUps = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
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
		this.cmbDateTypeHistory = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbDealStatusHistory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbServiceTypeHistory = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
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
		this.txtOfferValidityDate = new System.Windows.Forms.DateTimePicker();
		this.label36 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSourceDocument).BeginInit();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDealStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceType).BeginInit();
		this.groupBox3.SuspendLayout();
		this.panel34.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrencyDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVesselRoutingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateBaseType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgentName).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDealStatusHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1209, 611);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1209, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerDetail);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1201, 582);
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
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Location = new System.Drawing.Point(3, 353);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Name = "grdCustomerDetail";
		((ScrollableBase)this.grdCustomerDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Size = new System.Drawing.Size(1195, 226);
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
		this.panel14.Location = new System.Drawing.Point(3, 332);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1195, 21);
		this.panel14.TabIndex = 131;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1149, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdCustomerDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel5.Controls.Add(this.panel4);
		this.panel5.Controls.Add(this.panel2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 58);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1195, 274);
		this.panel5.TabIndex = 130;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdSourceDocument);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(899, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(296, 274);
		this.panel4.TabIndex = 2;
		((ScrollableBase)this.grdSourceDocument).BackColor = System.Drawing.Color.White;
		grdSourceDocument_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSourceDocument.DesignTimeLayout = grdSourceDocument_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSourceDocument).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSourceDocument).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSourceDocument.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSourceDocument).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdSourceDocument).Name = "grdSourceDocument";
		((ScrollableBase)this.grdSourceDocument).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSourceDocument).Size = new System.Drawing.Size(296, 252);
		((System.Windows.Forms.Control)(object)this.grdSourceDocument).TabIndex = 0;
		this.grdSourceDocument.TotalRow = (InheritableBoolean)1;
		this.grdSourceDocument.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSourceDocument).VisualStyle = (VisualStyle)4;
		this.grdSourceDocument.CellUpdated += new ColumnActionEventHandler(grdSourceDocument_CellUpdated);
		this.grdSourceDocument.ColumnButtonClick += new ColumnActionEventHandler(grdSourceDocument_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdSourceDocument).KeyDown += new System.Windows.Forms.KeyEventHandler(grdSourceDocument_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(296, 22);
		this.panel7.TabIndex = 5;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(249, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdSourceDocument;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(151, 16);
		this.label4.TabIndex = 126;
		this.label4.Text = "Source Document Info\t\r\n";
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Controls.Add(this.groupBox3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(899, 274);
		this.panel2.TabIndex = 1;
		this.groupBox2.Controls.Add(this.txtFcyExchangeRate);
		this.groupBox2.Controls.Add(this.txtTransactionExchangeRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCurrency);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDealStatus);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPort);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingPort);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceType);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(3, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(892, 99);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtFcyExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyExchangeRate.Location = new System.Drawing.Point(820, 44);
		this.txtFcyExchangeRate.Name = "txtFcyExchangeRate";
		this.txtFcyExchangeRate.Size = new System.Drawing.Size(70, 23);
		this.txtFcyExchangeRate.TabIndex = 9;
		this.txtFcyExchangeRate.Tag = "double";
		this.txtTransactionExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTransactionExchangeRate.Location = new System.Drawing.Point(820, 16);
		this.txtTransactionExchangeRate.Name = "txtTransactionExchangeRate";
		this.txtTransactionExchangeRate.Size = new System.Drawing.Size(70, 23);
		this.txtTransactionExchangeRate.TabIndex = 7;
		this.txtTransactionExchangeRate.Tag = "double";
		this.CmbFcyCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance171).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance172).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbFcyCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Location = new System.Drawing.Point(634, 42);
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Name = "CmbFcyCurrency";
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(757, 48);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(51, 15);
		this.label10.TabIndex = 158;
		this.label10.Text = "Fcy Rate";
		this.CmbTransactionCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbTransactionCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransactionCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransactionCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransactionCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Location = new System.Drawing.Point(634, 14);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Name = "CmbTransactionCurrency";
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).TabIndex = 6;
		this.CmbTransactionCurrency.ValueChanged += new System.EventHandler(CmbTransactionCurrency_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Leave += new System.EventHandler(CmbTransactionCurrency_ValueChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(757, 12);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(58, 30);
		this.label29.TabIndex = 158;
		this.label29.Text = "Exchange\r\nRate";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(561, 48);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(75, 15);
		this.label9.TabIndex = 159;
		this.label9.Text = "Fcy Currency";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(561, 75);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(67, 15);
		this.label31.TabIndex = 159;
		this.label31.Text = "Deal Status";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(561, 12);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(68, 30);
		this.label30.TabIndex = 159;
		this.label30.Text = "Transaction\r\nCurrency";
		this.CmbDealStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbDealStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDealStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDealStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDealStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDealStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDealStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDealStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDealStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDealStatus).Location = new System.Drawing.Point(634, 69);
		((System.Windows.Forms.Control)(object)this.CmbDealStatus).Name = "CmbDealStatus";
		((System.Windows.Forms.Control)(object)this.CmbDealStatus).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.CmbDealStatus).TabIndex = 10;
		this.CmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Location = new System.Drawing.Point(398, 14);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Name = "CmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabIndex = 4;
		this.CmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Location = new System.Drawing.Point(398, 42);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Name = "CmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabIndex = 5;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(306, 20);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(93, 15);
		this.label24.TabIndex = 155;
		this.label24.Text = "Destination Port";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(306, 48);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(75, 15);
		this.label27.TabIndex = 154;
		this.label27.Text = "Loading Port";
		this.CmbServiceType.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Location = new System.Drawing.Point(76, 42);
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Name = "CmbServiceType";
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceType).TabIndex = 2;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(2, 48);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(73, 15);
		this.label11.TabIndex = 151;
		this.label11.Text = "Service Type";
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(76, 71);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(482, 23);
		this.txtRemarksHeader.TabIndex = 3;
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(181, 16);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(125, 22);
		this.txtdocdate.TabIndex = 1;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(76, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(104, 23);
		this.txtdocno.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 75);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Remarks";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(2, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(76, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc No,Date";
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.panel34);
		this.groupBox3.Controls.Add(this.txtServiceRate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterItem);
		this.groupBox3.Controls.Add(this.RadItemCodePackListDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.RadItemNamePackListDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label23);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add(this.BtnAddCustomer);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.BtnCancelCustomer);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.txtRemarksDetail);
		this.groupBox3.Controls.Add(this.BtnUpdateCustmer);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add(this.txtTcyAmount);
		this.groupBox3.Controls.Add(this.txtLcyAmountDetail);
		this.groupBox3.Controls.Add(this.txtFreeDaysAtPOD);
		this.groupBox3.Controls.Add(this.txtTransitDays);
		this.groupBox3.Controls.Add(this.txtVesselRoutingDescription);
		this.groupBox3.Controls.Add(this.txtOfferValidityDays);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add(this.BtnReferenceParty);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceProvider);
		this.groupBox3.Controls.Add(this.txtEtdSailing);
		this.groupBox3.Controls.Add(this.txtOfferValidityDate);
		this.groupBox3.Controls.Add(this.txtCargoCutOffDate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVesselRoutingType);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateBaseType);
		this.groupBox3.Controls.Add(this.txtDueDays);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(3, 103);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(892, 170);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail Information";
		this.panel34.Controls.Add(this.RadCategory);
		this.panel34.Controls.Add(this.RadMasterItem);
		this.panel34.Location = new System.Drawing.Point(3, 58);
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
		this.txtServiceRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtServiceRate.Location = new System.Drawing.Point(534, 83);
		this.txtServiceRate.Name = "txtServiceRate";
		this.txtServiceRate.Size = new System.Drawing.Size(70, 23);
		this.txtServiceRate.TabIndex = 10;
		this.txtServiceRate.Tag = "double";
		this.txtServiceRate.TextChanged += new System.EventHandler(txtServiceRate_TextChanged);
		this.CmbMasterItem.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Location = new System.Drawing.Point(3, 81);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Name = "CmbMasterItem";
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Leave += new System.EventHandler(CmbMasterItem_Leave);
		this.RadItemCodePackListDetail.AutoSize = true;
		this.RadItemCodePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodePackListDetail.Location = new System.Drawing.Point(294, 60);
		this.RadItemCodePackListDetail.Name = "RadItemCodePackListDetail";
		this.RadItemCodePackListDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodePackListDetail.TabIndex = 633;
		this.RadItemCodePackListDetail.Text = "Code";
		this.RadItemCodePackListDetail.UseVisualStyleBackColor = true;
		this.CmbTransactionCurrencyDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbTransactionCurrencyDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransactionCurrencyDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransactionCurrencyDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransactionCurrencyDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Location = new System.Drawing.Point(413, 81);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Name = "CmbTransactionCurrencyDetail";
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).TabIndex = 9;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(534, 62);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 158;
		this.label20.Text = "Service Rate";
		this.RadItemNamePackListDetail.AutoSize = true;
		this.RadItemNamePackListDetail.Checked = true;
		this.RadItemNamePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNamePackListDetail.Location = new System.Drawing.Point(238, 60);
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
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance202;
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
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(165, 81);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 8;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(413, 62);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(118, 15);
		this.label18.TabIndex = 159;
		this.label18.Text = "Transaction Currency";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(165, 62);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(73, 15);
		this.label16.TabIndex = 631;
		this.label16.Text = "Service Item";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(698, 62);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(72, 15);
		this.label23.TabIndex = 158;
		this.label23.Text = "Tcy Amount";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(789, 62);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(72, 15);
		this.label25.TabIndex = 158;
		this.label25.Text = "Lcy Amount";
		this.BtnAddCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnAddCustomer.FlatAppearance.BorderSize = 0;
		this.BtnAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnAddCustomer.Location = new System.Drawing.Point(825, 141);
		this.BtnAddCustomer.Name = "BtnAddCustomer";
		this.BtnAddCustomer.Size = new System.Drawing.Size(39, 23);
		this.BtnAddCustomer.TabIndex = 21;
		this.BtnAddCustomer.Text = "+";
		this.BtnAddCustomer.UseVisualStyleBackColor = false;
		this.BtnAddCustomer.Click += new System.EventHandler(BtnAddCustomer_Click);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(316, 118);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(98, 15);
		this.label40.TabIndex = 158;
		this.label40.Text = "ETD(Sailing) POL\r\n";
		this.BtnCancelCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelCustomer.FlatAppearance.BorderSize = 0;
		this.BtnCancelCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnCancelCustomer.Location = new System.Drawing.Point(825, 116);
		this.BtnCancelCustomer.Name = "BtnCancelCustomer";
		this.BtnCancelCustomer.Size = new System.Drawing.Size(64, 23);
		this.BtnCancelCustomer.TabIndex = 22;
		this.BtnCancelCustomer.Text = "Cancel";
		this.BtnCancelCustomer.UseVisualStyleBackColor = false;
		this.BtnCancelCustomer.Visible = false;
		this.BtnCancelCustomer.Click += new System.EventHandler(BtnCancelCustomer_Click);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(195, 118);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(107, 15);
		this.label39.TabIndex = 158;
		this.label39.Text = "Cargo Cut Off Date";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(579, 141);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(243, 23);
		this.txtRemarksDetail.TabIndex = 20;
		this.BtnUpdateCustmer.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateCustmer.FlatAppearance.BorderSize = 0;
		this.BtnUpdateCustmer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateCustmer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateCustmer.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateCustmer.Location = new System.Drawing.Point(825, 141);
		this.BtnUpdateCustmer.Name = "BtnUpdateCustmer";
		this.BtnUpdateCustmer.Size = new System.Drawing.Size(64, 23);
		this.BtnUpdateCustmer.TabIndex = 21;
		this.BtnUpdateCustmer.Text = "Update";
		this.BtnUpdateCustmer.UseVisualStyleBackColor = false;
		this.BtnUpdateCustmer.Visible = false;
		this.BtnUpdateCustmer.Click += new System.EventHandler(BtnUpdateCustmer_Click);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(508, 110);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(61, 30);
		this.label44.TabIndex = 158;
		this.label44.Text = "Free Days \r\nAt POD";
		this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(576, 118);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(52, 15);
		this.label26.TabIndex = 131;
		this.label26.Text = "Remarks";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(437, 110);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(75, 30);
		this.label41.TabIndex = 158;
		this.label41.Text = "Transit Time \r\n(Days)\r\n";
		this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(708, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(148, 15);
		this.label2.TabIndex = 158;
		this.label2.Text = "Vessel Routing Description";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(3, 110);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(79, 30);
		this.label34.TabIndex = 158;
		this.label34.Text = "Offer Validity \r\n(Days)";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtTcyAmount.Enabled = false;
		this.txtTcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTcyAmount.Location = new System.Drawing.Point(698, 83);
		this.txtTcyAmount.Name = "txtTcyAmount";
		this.txtTcyAmount.Size = new System.Drawing.Size(90, 23);
		this.txtTcyAmount.TabIndex = 12;
		this.txtLcyAmountDetail.Enabled = false;
		this.txtLcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLcyAmountDetail.Location = new System.Drawing.Point(789, 83);
		this.txtLcyAmountDetail.Name = "txtLcyAmountDetail";
		this.txtLcyAmountDetail.Size = new System.Drawing.Size(100, 23);
		this.txtLcyAmountDetail.TabIndex = 13;
		this.txtFreeDaysAtPOD.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFreeDaysAtPOD.Location = new System.Drawing.Point(508, 141);
		this.txtFreeDaysAtPOD.Name = "txtFreeDaysAtPOD";
		this.txtFreeDaysAtPOD.Size = new System.Drawing.Size(70, 23);
		this.txtFreeDaysAtPOD.TabIndex = 19;
		this.txtFreeDaysAtPOD.Tag = "int";
		this.txtTransitDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTransitDays.Location = new System.Drawing.Point(437, 141);
		this.txtTransitDays.Name = "txtTransitDays";
		this.txtTransitDays.Size = new System.Drawing.Size(70, 23);
		this.txtTransitDays.TabIndex = 18;
		this.txtTransitDays.Tag = "int";
		this.txtVesselRoutingDescription.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVesselRoutingDescription.Location = new System.Drawing.Point(708, 33);
		this.txtVesselRoutingDescription.Name = "txtVesselRoutingDescription";
		this.txtVesselRoutingDescription.Size = new System.Drawing.Size(181, 23);
		this.txtVesselRoutingDescription.TabIndex = 6;
		this.txtOfferValidityDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOfferValidityDays.Location = new System.Drawing.Point(3, 141);
		this.txtOfferValidityDays.Name = "txtOfferValidityDays";
		this.txtOfferValidityDays.Size = new System.Drawing.Size(70, 23);
		this.txtOfferValidityDays.TabIndex = 14;
		this.txtOfferValidityDays.Tag = "int";
		this.txtOfferValidityDays.TextChanged += new System.EventHandler(txtOfferValidityDays_TextChanged);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(413, 15);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(84, 15);
		this.label54.TabIndex = 504;
		this.label54.Text = "Payment Term";
		this.BtnReferenceParty.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnReferenceParty.Location = new System.Drawing.Point(204, 31);
		this.BtnReferenceParty.Name = "BtnReferenceParty";
		this.BtnReferenceParty.Size = new System.Drawing.Size(26, 26);
		this.BtnReferenceParty.TabIndex = 1;
		this.BtnReferenceParty.UseVisualStyleBackColor = true;
		this.BtnReferenceParty.Click += new System.EventHandler(BtnReferenceParty_Click);
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance191;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(413, 31);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 3;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(606, 62);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(57, 15);
		this.label33.TabIndex = 159;
		this.label33.Text = "Rate Base";
		this.CmbServiceProvider.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceProvider.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceProvider.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceProvider.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Appearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceProvider.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Location = new System.Drawing.Point(231, 31);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Name = "CmbServiceProvider";
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Leave += new System.EventHandler(CmbCustomerName_Leave);
		this.txtEtdSailing.CustomFormat = "dd-MMM-yy";
		this.txtEtdSailing.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEtdSailing.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEtdSailing.Location = new System.Drawing.Point(316, 141);
		this.txtEtdSailing.Name = "txtEtdSailing";
		this.txtEtdSailing.Size = new System.Drawing.Size(120, 22);
		this.txtEtdSailing.TabIndex = 17;
		this.txtCargoCutOffDate.CustomFormat = "dd-MMM-yy";
		this.txtCargoCutOffDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtCargoCutOffDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtCargoCutOffDate.Location = new System.Drawing.Point(195, 141);
		this.txtCargoCutOffDate.Name = "txtCargoCutOffDate";
		this.txtCargoCutOffDate.Size = new System.Drawing.Size(120, 22);
		this.txtCargoCutOffDate.TabIndex = 16;
		this.CmbVesselRoutingType.AllowNull = (DefaultableBoolean)1;
		this.CmbVesselRoutingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVesselRoutingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVesselRoutingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVesselRoutingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVesselRoutingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVesselRoutingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVesselRoutingType).Location = new System.Drawing.Point(596, 31);
		((System.Windows.Forms.Control)(object)this.CmbVesselRoutingType).Name = "CmbVesselRoutingType";
		((System.Windows.Forms.Control)(object)this.CmbVesselRoutingType).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.CmbVesselRoutingType).TabIndex = 5;
		this.CmbRateBaseType.AllowNull = (DefaultableBoolean)1;
		this.CmbRateBaseType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateBaseType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateBaseType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateBaseType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Location = new System.Drawing.Point(606, 81);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Name = "CmbRateBaseType";
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).TabIndex = 11;
		this.CmbRateBaseType.ValueChanged += new System.EventHandler(CmbRateBaseType_Leave);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Leave += new System.EventHandler(CmbRateBaseType_Leave);
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(534, 33);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(60, 23);
		this.txtDueDays.TabIndex = 4;
		this.txtDueDays.Tag = "int";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(231, 15);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(92, 15);
		this.label7.TabIndex = 140;
		this.label7.Text = "Service Provider";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(534, 15);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(58, 15);
		this.label12.TabIndex = 13;
		this.label12.Text = "Due Days";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(596, 15);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(112, 15);
		this.label13.TabIndex = 154;
		this.label13.Text = "Vessel Routing Type";
		this.CmbBrokerAgentName.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokerAgentName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokerAgentName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokerAgentName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokerAgentName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Location = new System.Drawing.Point(3, 31);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Name = "CmbBrokerAgentName";
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Leave += new System.EventHandler(CmbCustomerName_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(3, 15);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(119, 15);
		this.label14.TabIndex = 140;
		this.label14.Text = "Broker / Agent Name";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1195, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.BtnDefineLookUps,
			this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1195, 27);
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
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
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
		this.btnPrint.Text = "1300-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.BtnDefineLookUps.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDefineLookUps.Image = Architecture.WinApp.Properties.Resources.Purchase_Trading;
		this.BtnDefineLookUps.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineLookUps.Name = "BtnDefineLookUps";
		this.BtnDefineLookUps.Size = new System.Drawing.Size(131, 24);
		this.BtnDefineLookUps.Text = "Define LookUps";
		this.BtnDefineLookUps.Visible = false;
		this.BtnDefineLookUps.Click += new System.EventHandler(BtnDefineLookUps_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.Yellow;
		this.label22.Location = new System.Drawing.Point(373, 35);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(603, 14);
		this.label22.TabIndex = 126;
		this.label22.Text = "A centralized record for evaluating, negotiating, and approving freight and transportation rates.\r\n";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(4, 31);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(371, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Logistic Rate Negotiation and Comparisons :";
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(1114, 30);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(78, 20);
		this.ChkPreview.TabIndex = 12;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1201, 582);
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 138);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1201, 227);
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
		this.panel9.Size = new System.Drawing.Size(1201, 24);
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
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1201, 193);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 115);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1201, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1154, 0);
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
		this.panel13.Size = new System.Drawing.Size(1201, 63);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.rdapproveddate);
		this.groupBox1.Controls.Add(this.rdmodifydate);
		this.groupBox1.Controls.Add(this.drdocdate);
		this.groupBox1.Controls.Add(this.rdentrydate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDealStatusHistory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.ToDateHistory);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.ToDocNoHistory);
		this.groupBox1.Controls.Add(this.FromDateHistory);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.FromDocNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1160, 62);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(3, 32);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1053, 39);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1053, 21);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(973, 21);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(973, 39);
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
		this.btnshow.Location = new System.Drawing.Point(915, 33);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbDealStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDealStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDealStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDealStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance187).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDealStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDealStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDealStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDealStatusHistory).Location = new System.Drawing.Point(783, 32);
		((System.Windows.Forms.Control)(object)this.CmbDealStatusHistory).Name = "CmbDealStatusHistory";
		((System.Windows.Forms.Control)(object)this.CmbDealStatusHistory).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbDealStatusHistory).TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(780, 16);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 180;
		this.label6.Text = "Deal Status";
		this.CmbServiceTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance183).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance184).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Location = new System.Drawing.Point(602, 32);
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Name = "CmbServiceTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).TabIndex = 5;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(602, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(73, 15);
		this.label3.TabIndex = 180;
		this.label3.Text = "Service Type";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(3, 16);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(307, 34);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(150, 22);
		this.ToDateHistory.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(307, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 15);
		this.label8.TabIndex = 171;
		this.label8.Text = "Date To";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(155, 16);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(529, 34);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(155, 34);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(150, 22);
		this.FromDateHistory.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(529, 16);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(45, 15);
		this.label5.TabIndex = 173;
		this.label5.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(458, 16);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(458, 34);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1201, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1201, 27);
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
		this.label32.Size = new System.Drawing.Size(433, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Logistic Rate Negotiation and Comparisons History  ";
		this.txtOfferValidityDate.CustomFormat = "dd-MMM-yy";
		this.txtOfferValidityDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtOfferValidityDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtOfferValidityDate.Location = new System.Drawing.Point(74, 141);
		this.txtOfferValidityDate.Name = "txtOfferValidityDate";
		this.txtOfferValidityDate.Size = new System.Drawing.Size(120, 22);
		this.txtOfferValidityDate.TabIndex = 15;
		this.txtOfferValidityDate.ValueChanged += new System.EventHandler(txtOfferValidityDate_ValueChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(74, 118);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(104, 15);
		this.label36.TabIndex = 158;
		this.label36.Text = "Offer Validity Date";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1209, 611);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmLogisticRateNegotiation";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmLogisticRateNegotiation_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSourceDocument).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDealStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceType).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrencyDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVesselRoutingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateBaseType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgentName).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDealStatusHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
