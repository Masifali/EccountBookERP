using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.lgstcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.Model.lgstcm;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Service.lgstcm;

public class frmLogisticAgreement : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtCustomerDetail = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtServiceProvider = new DataTable();

	private DataTable dtItem = new DataTable();

	private string ScreenName = "frmLogisticAgreement";

	private bool UpdateMode;

	private int RecId;

	private int ForeignBaseCurrency;

	private double ForeignBaseCurrencyRate;

	private int DocumentTypeId = 1301;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool ItemSearchWithNameOrCode = true;

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<AgreementDetail> lstRemoveRecordcustomerDetail = new List<AgreementDetail>();

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

	private GroupBox groupBox3;

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

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbServiceType;

	private Label label11;

	private Label label29;

	private TextBox txtTransactionExchangeRate;

	private Label label30;

	internal UltraCombo CmbTransactionCurrency;

	internal UltraCombo CmbServiceProvider;

	private Label label25;

	private TextBox txtLcyAmountDetail;

	private Label label33;

	internal UltraCombo CmbRateBaseType;

	internal UltraCombo CmbServiceTypeHistory;

	private Label label3;

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

	private Label label22;

	private Label label21;

	private CheckBox chkIsActive;

	private DateTimePicker txtEffectiveDateFrom;

	private Label label4;

	private Label label2;

	private TextBox txtPartyRefDocNo;

	private Label label12;

	private DateTimePicker txtPartyRefDocDate;

	internal UltraCombo CmbServiceProviderHistory;

	private Label label6;

	private Label label8;

	internal UltraCombo CmbBrokerAgentName;

	private Label label14;

	private Label label7;

	private DateTimePicker txtEffectiveDateTo;

	public frmLogisticAgreement(UserAccount user)
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
			dtCustomerDetail.Columns.Add("Id", typeof(int));
			dtCustomerDetail.Columns.Add("PartyRefDocNo", typeof(string));
			dtCustomerDetail.Columns.Add("PartyRefDocDate", typeof(DateTime));
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
			dtCustomerDetail.Columns.Add("Remarks");
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
				btnSaveAs.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnPrint.Enabled = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				ctrlGrdBar3.mGridSaveLayouts.Visible = false;
				ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
				GetConfigurationsFromGlobal();
				if (RecId == 0)
				{
					txtdocno.Text = result.DocumentNoData;
				}
				AllLookUpComboBind(result.LookUpData);
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
			return AgreementHeader.GenerateCode(new AgreementHeader
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
			DataTable dtStatus = dtSP.Clone();
			DataTable dtRoutingType = dtSP.Clone();
			DataTable DtUom = dtSP.Clone();
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
				}
			}
			ServiceTypeBind(dtSP);
			RateBaseTypeTypeBind(DtUom);
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

	private bool FormValidationCustomerDetail()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbServiceProvider, "Service Provider", FormHelper.ValidType.INT),
			(txtPartyRefDocNo, "Party Ref Doc No", FormHelper.ValidType.String),
			((Control)(object)CmbItemName, "Item Name", FormHelper.ValidType.INT),
			(txtServiceRate, "Service Rate", FormHelper.ValidType.Double),
			((Control)(object)CmbRateBaseType, "Rate Base", FormHelper.ValidType.INT),
			(txtTcyAmount, "Tcy Amount", FormHelper.ValidType.Double),
			(txtLcyAmountDetail, "Lcy Amount", FormHelper.ValidType.Double)
		}))
		{
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
		dr["ItemName"] = value;
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
		dr["ItemCode"] = value2;
		dr["TransactionCurrencyId"] = Conversion.ToInt(CmbTransactionCurrencyDetail.Value);
		dr["TransactionCurrency"] = Conversion.ToString((object)((Control)(object)CmbTransactionCurrencyDetail).Text);
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
		dr["TcyAmount"] = Conversion.ToDecimal((object)txtTcyAmount.Text);
		dr["LcyAmount"] = Conversion.ToDecimal((object)txtLcyAmountDetail.Text);
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
				updateDetailIndex = row.RowIndex;
				((Control)(object)CmbMasterItem).Text = "";
				ItemDtsFillFromGlobal();
				ItemNameBind();
				txtPartyRefDocNo.Text = Conversion.ToString(row.Cells["PartyRefDocNo"].Value);
				txtPartyRefDocDate.Value = Conversion.ToDateTime(row.Cells["PartyRefDocDate"].Value);
				CmbItemName.Value = row.Cells["ItemId"].Value;
				CmbTransactionCurrencyDetail.Value = row.Cells["TransactionCurrencyId"].Value;
				txtServiceRate.Text = Conversion.ToString(row.Cells["ServiceRate"].Value);
				CmbRateBaseType.Value = row.Cells["RateBaseId"].Value;
				txtTcyAmount.Text = Conversion.ToString(row.Cells["TcyAmount"].Value);
				txtLcyAmountDetail.Text = Conversion.ToString(row.Cells["LcyAmount"].Value);
				txtRemarksDetail.Text = Conversion.ToString(row.Cells["Remarks"].Value);
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
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "TransactionCurrencyId", "RateBaseId" };
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
						"Remarks",
						(EditType)1
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["PartyRefDocNo"].Width = 120;
			gridEX.RootTable.Columns["PartyRefDocDate"].Width = 100;
			gridEX.RootTable.Columns["ItemName"].Width = 160;
			gridEX.RootTable.Columns["ItemCode"].Width = 100;
			gridEX.RootTable.Columns["TransactionCurrency"].Width = 80;
			gridEX.RootTable.Columns["ServiceRate"].Width = 80;
			gridEX.RootTable.Columns["RateBase"].Width = 120;
			gridEX.RootTable.Columns["RateBaseEquivalent"].Width = 90;
			gridEX.RootTable.Columns["TcyAmount"].Width = 90;
			gridEX.RootTable.Columns["LcyAmount"].Width = 90;
			gridEX.RootTable.Columns["Remarks"].Width = 180;
			if (num)
			{
				GridEX_Helper.AddButton(grdCustomerDetail, "Delete", "X", 20, 0);
				GridEX_Helper.AddButton(grdCustomerDetail, "Edit", "Edit", 40, 1);
				grdCustomerDetail.FrozenColumns = 3;
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
				grdCustomerDetail.UpdateData();
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
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						AgreementDetail d = new AgreementDetail();
						d.agreementDetailId = Conversion.ToInt(r.Cells["Id"].Value);
						FillDetailListCommonForInsertAndDelete(d, r);
						d.ActionTypeId = 3;
						lstRemoveRecordcustomerDetail.Add(d);
						dtCustomerDetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					dtCustomerDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				return;
			}
			throw new Exception("Please Reset the Detail first..");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			txtRemarksHeader.Text = string.Empty;
			ResetCustomerDetail();
			dtCustomerDetail.Rows.Clear();
			grdCustomerDetail.DataSource = null;
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
			txtPartyRefDocNo.Text = string.Empty;
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
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AllSerivesItems", "MultiCurrency" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			AllLookUpComboBind(LookUps.LookUpAllServices(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
			CurrencyBindFromGlobal();
			SupplierDtFillFromGlobal();
			BrokerAgentOrServiceProviderBind(CmbBrokerAgentName, dtSupplier, "Broker Agent");
			BrokerAgentOrServiceProviderBind(CmbServiceProvider, dtServiceProvider, "Service Provider");
			ItemCategoryOrMasterItemBind();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			GetConfigurationsFromGlobalandBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmLogisticAgreement_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		try
		{
			if (grdCustomerDetail.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbServiceType, "Service Type", FormHelper.ValidType.INT),
				((Control)(object)CmbBrokerAgentName, "Bill To Party", FormHelper.ValidType.INT),
				((Control)(object)CmbServiceProvider, "Broker Agent Or Service Provider", FormHelper.ValidType.INT),
				((Control)(object)CmbTransactionCurrency, "Transaction Currency", FormHelper.ValidType.INT),
				(txtTransactionExchangeRate, "Tcy Exchange Rate", FormHelper.ValidType.Double),
				((Control)(object)CmbFcyCurrency, "Foreign Currency", FormHelper.ValidType.INT),
				(txtFcyExchangeRate, "Fcy Rate", FormHelper.ValidType.Double)
			}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			AgreementHeader Or = new AgreementHeader();
			Or.AgreementHeaderId = RecId;
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchesId = UserAccount.BranchesId;
			Or.ProjectsId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Or.DocumentTypeId = DocumentTypeId;
			Or.DocumentDate = Conversion.ToDateTime((object)txtdocdate.Value);
			Or.DocumentNo = Conversion.ToInt((object)txtdocno.Text);
			Or.serviceTypeId = Conversion.ToInt(CmbServiceType.Value);
			Or.BrokerAgentId = Conversion.ToInt(CmbBrokerAgentName.Value);
			Or.serviceProviderId = Conversion.ToInt(CmbServiceProvider.Value);
			Or.fcyCurrencyId = Conversion.ToInt(CmbFcyCurrency.Value);
			Or.fcyExchangeRate = Conversion.ToDouble((object)txtFcyExchangeRate.Text);
			Or.transactionCurrencyId = Conversion.ToInt(CmbTransactionCurrency.Value);
			Or.transactionExchangeRate = Conversion.ToDouble((object)txtTransactionExchangeRate.Text);
			Or.effectiveDateFrom = Conversion.ToDateTime((object)txtEffectiveDateFrom.Value);
			Or.effectiveDateTo = Conversion.ToDateTime((object)txtEffectiveDateTo.Value);
			Or.isActive = chkIsActive.Checked;
			Or.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ApprovedUserId = UserAccount.ID;
			Or.ApprovedDate = DateTime.Now;
			Or.IsApproved = false;
			Or.AgreementDetailList = new List<AgreementDetail>();
			if (lstRemoveRecordcustomerDetail.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecordcustomerDetail.Count; i++)
				{
					Or.AgreementDetailList.Add(lstRemoveRecordcustomerDetail[i]);
				}
			}
			GridEXRow[] rows = grdCustomerDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				AgreementDetail vd = new AgreementDetail();
				vd.agreementDetailId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.agreementDetailId <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r);
				FormHelper.ValidateField(vd.partyRefDocNo, "Party Ref DocNo", r.RowIndex);
				FormHelper.ValidateField(vd.serviceItemId, "Service Item", r.RowIndex);
				FormHelper.ValidateField(vd.transactionCurrencyId, "Transaction Currency", r.RowIndex);
				FormHelper.ValidateField(vd.serviceRate, "Service Rate", r.RowIndex);
				FormHelper.ValidateField(vd.serviceRateBaseUomId, "Rate Base", r.RowIndex);
				FormHelper.ValidateField(vd.tcyAmount, "Tcy amount", r.RowIndex);
				FormHelper.ValidateField(vd.lcyAmount, "Lcy amount", r.RowIndex);
				if (string.IsNullOrWhiteSpace(Or.RemarksHeader))
				{
					Or.RemarksHeader = vd.Remarks;
				}
				Or.AgreementDetailList.Add(vd);
			}
			Or.AttachmentsList = new List<DMSAttachments>();
			Or.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<AgreementHeader>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Or.AttachmentsValues = AttachmentsValues;
			Or.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = AgreementHeader.Save(Or);
			Or.AgreementHeaderId = success;
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
				AgreementHeader_Slip(Or.AgreementHeaderId);
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
			AgreementHeader Or = AgreementHeader.ReadById(RecId);
			if (Or == null || Or.AgreementDetailList == null)
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
			txtdocdate.Value = Or.DocumentDate;
			txtdocno.Text = Conversion.ToString((object)Or.DocumentNo);
			CmbServiceType.Value = Conversion.ToInt((object)Or.serviceTypeId);
			CmbBrokerAgentName.Value = Conversion.ToInt((object)Or.BrokerAgentId);
			CmbServiceProvider.Value = Conversion.ToInt((object)Or.serviceProviderId);
			CmbTransactionCurrency.Value = Conversion.ToInt((object)Or.transactionCurrencyId);
			txtTransactionExchangeRate.Text = Or.transactionExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			CmbFcyCurrency.Value = Conversion.ToInt((object)Or.fcyCurrencyId);
			txtFcyExchangeRate.Text = Or.fcyExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			if (!Conversion.CheckDateTimeNull(Or.effectiveDateFrom))
			{
				txtEffectiveDateFrom.Value = Or.effectiveDateFrom;
			}
			if (!Conversion.CheckDateTimeNull(Or.effectiveDateTo))
			{
				txtEffectiveDateTo.Value = Or.effectiveDateTo;
			}
			chkIsActive.Checked = Or.isActive;
			txtRemarksHeader.Text = Conversion.ToString((object)Or.RemarksHeader);
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			dtCustomerDetail.Rows.Clear();
			FillDetailFromListCommonForReadById(Or, dtCustomerDetail);
			grdCustomerDetail.DataSource = dtCustomerDetail;
			grdCustomerDetail.RetrieveStructure();
			grdCustomerDetailSetting(grdCustomerDetail);
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
					AgreementHeader.DeleteByID(UserAccount.ID, RecId);
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

	private void FillDetailListCommonForInsertAndDelete(AgreementDetail vd, GridEXRow r)
	{
		vd.partyRefDocNo = Conversion.ToString(r.Cells["PartyRefDocNo"].Value);
		vd.partyRefDocDate = Conversion.ToDateTime(r.Cells["PartyRefDocDate"].Value);
		vd.serviceItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.transactionCurrencyId = Conversion.ToInt(r.Cells["TransactionCurrencyId"].Value);
		vd.TransactionCurrencyCode = Conversion.ToString(r.Cells["TransactionCurrency"].Value);
		vd.transactionExchangeRate = Conversion.ToDouble((object)txtTransactionExchangeRate.Text);
		vd.serviceRate = Conversion.ToDouble(r.Cells["ServiceRate"].Value);
		vd.serviceRateBaseUomId = Conversion.ToInt(r.Cells["RateBaseId"].Value);
		vd.RateBaseUom = Conversion.ToString(r.Cells["RateBase"].Value);
		vd.RateBaseUomEquivalent = Conversion.ToDouble(r.Cells["RateBaseEquivalent"].Value);
		vd.tcyAmount = Conversion.ToDouble(r.Cells["TcyAmount"].Value);
		vd.lcyAmount = Conversion.ToDouble(r.Cells["LcyAmount"].Value);
		double fcyExchangeRate = Conversion.ToDouble((object)txtFcyExchangeRate.Text);
		vd.fcyAmount = ((fcyExchangeRate > 0.0) ? (vd.lcyAmount / fcyExchangeRate) : 0.0);
		vd.effectiveDateFrom = Conversion.ToDateTime((object)txtEffectiveDateFrom.Value);
		vd.effectiveDateTo = Conversion.ToDateTime((object)txtEffectiveDateTo.Value);
		vd.isActive = chkIsActive.Checked;
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void FillDetailFromListCommonForReadById(AgreementHeader invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.AgreementDetailList : null) == null || invGrn.AgreementDetailList.Count == 0)
		{
			return;
		}
		foreach (AgreementDetail detail in invGrn.AgreementDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.agreementDetailId;
			dr["PartyRefDocNo"] = detail.partyRefDocNo;
			dr["PartyRefDocDate"] = detail.partyRefDocDate;
			dr["ItemId"] = detail.serviceItemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.ItemName;
			dr["TransactionCurrencyId"] = detail.transactionCurrencyId;
			dr["TransactionCurrency"] = detail.TransactionCurrencyCode;
			dr["ServiceRate"] = detail.serviceRate;
			dr["RateBaseId"] = detail.serviceRateBaseUomId;
			dr["RateBase"] = detail.RateBaseUom;
			dr["RateBaseEquivalent"] = detail.RateBaseUomEquivalent;
			dr["TcyAmount"] = detail.tcyAmount;
			dr["LcyAmount"] = detail.lcyAmount;
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			AgreementHeader_Slip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AgreementHeader_Slip(int PrintId)
	{
		try
		{
			CrystalReportPrint_Helper.AgreementHeader_Slip_1301(PrintId);
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
			return AgreementHeader.GetDataForDropDownFromAgreementHeader(new ReportsParameters
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
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ServiceProvider")
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
			DataTable dtHistory = AgreementHeader.FormHistory(obj);
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
					dtHistoryGrid.Columns.Add("BillToPartyId", typeof(int));
					dtHistoryGrid.Columns.Add("BillToPartyName", typeof(string));
					dtHistoryGrid.Columns.Add("ServiceProviderId", typeof(int));
					dtHistoryGrid.Columns.Add("BrokerAgentOrServiceProviderName", typeof(string));
					dtHistoryGrid.Columns.Add("TransactionCurrencyId", typeof(int));
					dtHistoryGrid.Columns.Add("TcyCode", typeof(string));
					dtHistoryGrid.Columns.Add("TransactionExchangeRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("FcyCurrencyId", typeof(int));
					dtHistoryGrid.Columns.Add("FcyCode", typeof(string));
					dtHistoryGrid.Columns.Add("FcyExchangeRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("EffectiveDateFrom", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EffectiveDateTo", typeof(DateTime));
					dtHistoryGrid.Columns.Add("IsActive", typeof(bool));
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
					dtHistoryGrid.Rows.Add(row["AgreementHeaderId"], row["DocumentTypeId"], row["DocumentNo"], row["DocumentDate"], row["ServiceTypeId"], row["ServiceType"], row["BrokerAgentId"], row["BrokerAgentName"], row["ServiceProviderId"], row["ServiceProviderName"], row["TransactionCurrencyId"], row["TcyCode"], row["TransactionExchangeRate"], row["FcyCurrencyId"], row["FcyCode"], row["FcyExchangeRate"], row["EffectiveDateFrom"], row["EffectiveDateTo"], row["IsActive"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovalUserName"], row["NoOfAttachments"]);
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "ServiceTypeId", "ServiceProviderId", "BillToPartyId", "DealStatusId", "TransactionCurrencyId", "FcyCurrencyId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocumentDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["EffectiveDateFrom"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["EffectiveDateTo"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["DocumentNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			grdHistory.RootTable.Columns["DocumentDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ServiceType"].Width = 150;
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
				AgreementHeader_Slip(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
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
			AgreementHeader Obj = AgreementHeader.ReadById(Id);
			if (Obj != null && Obj.AgreementDetailList.Count == 0)
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
					AgreementHeader_Slip(CurrId);
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
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Expected O, but got Unknown
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Expected O, but got Unknown
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_0d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Expected O, but got Unknown
		//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Expected O, but got Unknown
		//IL_546c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5476: Expected O, but got Unknown
		//IL_549a: Unknown result type (might be due to invalid IL or missing references)
		//IL_54a4: Expected O, but got Unknown
		GridEXLayout grdCustomerDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
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
		Appearance appearance187 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance238 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
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
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.lgstcm.frmLogisticAgreement));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdCustomerDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtPartyRefDocNo = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtPartyRefDocDate = new System.Windows.Forms.DateTimePicker();
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
		this.BtnCancelCustomer = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.BtnUpdateCustmer = new System.Windows.Forms.Button();
		this.label26 = new System.Windows.Forms.Label();
		this.txtTcyAmount = new System.Windows.Forms.TextBox();
		this.txtLcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbRateBaseType = new UltraCombo();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbServiceProvider = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.chkIsActive = new System.Windows.Forms.CheckBox();
		this.txtEffectiveDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtFcyExchangeRate = new System.Windows.Forms.TextBox();
		this.txtTransactionExchangeRate = new System.Windows.Forms.TextBox();
		this.CmbFcyCurrency = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbTransactionCurrency = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.CmbServiceType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
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
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
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
		this.label8 = new System.Windows.Forms.Label();
		this.CmbBrokerAgentName = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.txtEffectiveDateTo = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel34.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrencyDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateBaseType).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceType).BeginInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgentName).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(984, 611);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(984, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerDetail);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(976, 582);
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
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Location = new System.Drawing.Point(3, 322);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Name = "grdCustomerDetail";
		((ScrollableBase)this.grdCustomerDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Size = new System.Drawing.Size(970, 257);
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
		this.panel14.Location = new System.Drawing.Point(3, 301);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(970, 21);
		this.panel14.TabIndex = 131;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(924, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdCustomerDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 54);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(970, 247);
		this.panel5.TabIndex = 130;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtPartyRefDocNo);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.txtPartyRefDocDate);
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
		this.groupBox3.Controls.Add(this.BtnCancelCustomer);
		this.groupBox3.Controls.Add(this.txtRemarksDetail);
		this.groupBox3.Controls.Add(this.BtnUpdateCustmer);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.txtTcyAmount);
		this.groupBox3.Controls.Add(this.txtLcyAmountDetail);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateBaseType);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(4, 131);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(964, 112);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail Information";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(164, 13);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(77, 15);
		this.label2.TabIndex = 638;
		this.label2.Text = "Ref Doc Date";
		this.txtPartyRefDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyRefDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyRefDocNo.Location = new System.Drawing.Point(2, 34);
		this.txtPartyRefDocNo.Name = "txtPartyRefDocNo";
		this.txtPartyRefDocNo.Size = new System.Drawing.Size(160, 23);
		this.txtPartyRefDocNo.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(2, 13);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(98, 15);
		this.label12.TabIndex = 637;
		this.label12.Text = "Party Ref Doc No";
		this.txtPartyRefDocDate.CustomFormat = "dd-MMM-yy";
		this.txtPartyRefDocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtPartyRefDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPartyRefDocDate.Location = new System.Drawing.Point(164, 34);
		this.txtPartyRefDocDate.Name = "txtPartyRefDocDate";
		this.txtPartyRefDocDate.Size = new System.Drawing.Size(120, 22);
		this.txtPartyRefDocDate.TabIndex = 1;
		this.panel34.Controls.Add(this.RadCategory);
		this.panel34.Controls.Add(this.RadMasterItem);
		this.panel34.Location = new System.Drawing.Point(286, 9);
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
		this.txtServiceRate.Location = new System.Drawing.Point(781, 34);
		this.txtServiceRate.Name = "txtServiceRate";
		this.txtServiceRate.Size = new System.Drawing.Size(70, 23);
		this.txtServiceRate.TabIndex = 5;
		this.txtServiceRate.Tag = "double";
		this.txtServiceRate.TextChanged += new System.EventHandler(txtServiceRate_TextChanged);
		this.CmbMasterItem.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Location = new System.Drawing.Point(286, 32);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Name = "CmbMasterItem";
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Leave += new System.EventHandler(CmbMasterItem_Leave);
		this.RadItemCodePackListDetail.AutoSize = true;
		this.RadItemCodePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodePackListDetail.Location = new System.Drawing.Point(577, 11);
		this.RadItemCodePackListDetail.Name = "RadItemCodePackListDetail";
		this.RadItemCodePackListDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodePackListDetail.TabIndex = 633;
		this.RadItemCodePackListDetail.Text = "Code";
		this.RadItemCodePackListDetail.UseVisualStyleBackColor = true;
		this.CmbTransactionCurrencyDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbTransactionCurrencyDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransactionCurrencyDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransactionCurrencyDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance165;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance170).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransactionCurrencyDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransactionCurrencyDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Location = new System.Drawing.Point(659, 32);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Name = "CmbTransactionCurrencyDetail";
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrencyDetail).TabIndex = 4;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(781, 13);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 158;
		this.label20.Text = "Service Rate";
		this.RadItemNamePackListDetail.AutoSize = true;
		this.RadItemNamePackListDetail.Checked = true;
		this.RadItemNamePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNamePackListDetail.Location = new System.Drawing.Point(521, 11);
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
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance105).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(448, 32);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 3;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(659, 13);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(118, 15);
		this.label18.TabIndex = 159;
		this.label18.Text = "Transaction Currency";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(448, 13);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(73, 15);
		this.label16.TabIndex = 631;
		this.label16.Text = "Service Item";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(2, 61);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(72, 15);
		this.label23.TabIndex = 158;
		this.label23.Text = "Tcy Amount";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(93, 61);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(72, 15);
		this.label25.TabIndex = 158;
		this.label25.Text = "Lcy Amount";
		this.BtnAddCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnAddCustomer.FlatAppearance.BorderSize = 0;
		this.BtnAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnAddCustomer.Location = new System.Drawing.Point(782, 82);
		this.BtnAddCustomer.Name = "BtnAddCustomer";
		this.BtnAddCustomer.Size = new System.Drawing.Size(39, 23);
		this.BtnAddCustomer.TabIndex = 10;
		this.BtnAddCustomer.Text = "+";
		this.BtnAddCustomer.UseVisualStyleBackColor = false;
		this.BtnAddCustomer.Click += new System.EventHandler(BtnAddCustomer_Click);
		this.BtnCancelCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelCustomer.FlatAppearance.BorderSize = 0;
		this.BtnCancelCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnCancelCustomer.Location = new System.Drawing.Point(847, 82);
		this.BtnCancelCustomer.Name = "BtnCancelCustomer";
		this.BtnCancelCustomer.Size = new System.Drawing.Size(64, 23);
		this.BtnCancelCustomer.TabIndex = 12;
		this.BtnCancelCustomer.Text = "Cancel";
		this.BtnCancelCustomer.UseVisualStyleBackColor = false;
		this.BtnCancelCustomer.Visible = false;
		this.BtnCancelCustomer.Click += new System.EventHandler(BtnCancelCustomer_Click);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(194, 82);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(585, 23);
		this.txtRemarksDetail.TabIndex = 9;
		this.BtnUpdateCustmer.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateCustmer.FlatAppearance.BorderSize = 0;
		this.BtnUpdateCustmer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateCustmer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateCustmer.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateCustmer.Location = new System.Drawing.Point(782, 82);
		this.BtnUpdateCustmer.Name = "BtnUpdateCustmer";
		this.BtnUpdateCustmer.Size = new System.Drawing.Size(64, 23);
		this.BtnUpdateCustmer.TabIndex = 11;
		this.BtnUpdateCustmer.Text = "Update";
		this.BtnUpdateCustmer.UseVisualStyleBackColor = false;
		this.BtnUpdateCustmer.Visible = false;
		this.BtnUpdateCustmer.Click += new System.EventHandler(BtnUpdateCustmer_Click);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(194, 59);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(52, 15);
		this.label26.TabIndex = 131;
		this.label26.Text = "Remarks";
		this.txtTcyAmount.Enabled = false;
		this.txtTcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTcyAmount.Location = new System.Drawing.Point(2, 82);
		this.txtTcyAmount.Name = "txtTcyAmount";
		this.txtTcyAmount.Size = new System.Drawing.Size(90, 23);
		this.txtTcyAmount.TabIndex = 7;
		this.txtLcyAmountDetail.Enabled = false;
		this.txtLcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLcyAmountDetail.Location = new System.Drawing.Point(93, 82);
		this.txtLcyAmountDetail.Name = "txtLcyAmountDetail";
		this.txtLcyAmountDetail.Size = new System.Drawing.Size(100, 23);
		this.txtLcyAmountDetail.TabIndex = 8;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(852, 13);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(57, 15);
		this.label33.TabIndex = 159;
		this.label33.Text = "Rate Base";
		this.CmbRateBaseType.AllowNull = (DefaultableBoolean)1;
		this.CmbRateBaseType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateBaseType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateBaseType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateBaseType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateBaseType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Location = new System.Drawing.Point(852, 32);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Name = "CmbRateBaseType";
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Size = new System.Drawing.Size(106, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).TabIndex = 6;
		this.CmbRateBaseType.ValueChanged += new System.EventHandler(CmbRateBaseType_Leave);
		((System.Windows.Forms.Control)(object)this.CmbRateBaseType).Leave += new System.EventHandler(CmbRateBaseType_Leave);
		this.groupBox2.Controls.Add(this.chkIsActive);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceProvider);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceType);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtEffectiveDateTo);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtEffectiveDateFrom);
		this.groupBox2.Controls.Add(this.txtFcyExchangeRate);
		this.groupBox2.Controls.Add(this.txtTransactionExchangeRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCurrency);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(964, 128);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.CmbServiceProvider.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceProvider.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceProvider.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceProvider.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceProvider.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Location = new System.Drawing.Point(421, 14);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Name = "CmbServiceProvider";
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Size = new System.Drawing.Size(301, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Leave += new System.EventHandler(CmbCustomerName_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(726, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(111, 15);
		this.label4.TabIndex = 162;
		this.label4.Text = "Effective Date From";
		this.chkIsActive.AutoSize = true;
		this.chkIsActive.BackColor = System.Drawing.Color.White;
		this.chkIsActive.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.chkIsActive.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
		this.chkIsActive.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.chkIsActive.ForeColor = System.Drawing.Color.Black;
		this.chkIsActive.Location = new System.Drawing.Point(840, 73);
		this.chkIsActive.Name = "chkIsActive";
		this.chkIsActive.Padding = new System.Windows.Forms.Padding(2, 1, 1, 1);
		this.chkIsActive.Size = new System.Drawing.Size(67, 20);
		this.chkIsActive.TabIndex = 12;
		this.chkIsActive.Text = "Active";
		this.chkIsActive.UseVisualStyleBackColor = false;
		this.txtEffectiveDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtEffectiveDateFrom.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEffectiveDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectiveDateFrom.Location = new System.Drawing.Point(840, 16);
		this.txtEffectiveDateFrom.Name = "txtEffectiveDateFrom";
		this.txtEffectiveDateFrom.Size = new System.Drawing.Size(120, 22);
		this.txtEffectiveDateFrom.TabIndex = 10;
		this.txtFcyExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyExchangeRate.Location = new System.Drawing.Point(652, 72);
		this.txtFcyExchangeRate.Name = "txtFcyExchangeRate";
		this.txtFcyExchangeRate.Size = new System.Drawing.Size(70, 23);
		this.txtFcyExchangeRate.TabIndex = 9;
		this.txtFcyExchangeRate.Tag = "double";
		this.txtTransactionExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTransactionExchangeRate.Location = new System.Drawing.Point(652, 44);
		this.txtTransactionExchangeRate.Name = "txtTransactionExchangeRate";
		this.txtTransactionExchangeRate.Size = new System.Drawing.Size(70, 23);
		this.txtTransactionExchangeRate.TabIndex = 7;
		this.txtTransactionExchangeRate.Tag = "double";
		this.CmbFcyCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance201;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance215).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance216).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbFcyCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Location = new System.Drawing.Point(421, 70);
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Name = "CmbFcyCurrency";
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbFcyCurrency).TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(593, 76);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(51, 15);
		this.label10.TabIndex = 158;
		this.label10.Text = "Fcy Rate";
		this.CmbTransactionCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbTransactionCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransactionCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransactionCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransactionCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransactionCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Location = new System.Drawing.Point(421, 42);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Name = "CmbTransactionCurrency";
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).TabIndex = 6;
		this.CmbTransactionCurrency.ValueChanged += new System.EventHandler(CmbTransactionCurrency_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbTransactionCurrency).Leave += new System.EventHandler(CmbTransactionCurrency_ValueChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(593, 40);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(58, 30);
		this.label29.TabIndex = 158;
		this.label29.Text = "Exchange\r\nRate";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(327, 76);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(75, 15);
		this.label9.TabIndex = 159;
		this.label9.Text = "Fcy Currency";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(327, 40);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(68, 30);
		this.label30.TabIndex = 159;
		this.label30.Text = "Transaction\r\nCurrency";
		this.CmbServiceType.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Location = new System.Drawing.Point(93, 42);
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
		this.txtRemarksHeader.Location = new System.Drawing.Point(93, 100);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(867, 23);
		this.txtRemarksHeader.TabIndex = 4;
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(198, 16);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(125, 22);
		this.txtdocdate.TabIndex = 1;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(93, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(104, 23);
		this.txtdocno.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 108);
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
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(970, 51);
		this.panel3.TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.Yellow;
		this.label22.Location = new System.Drawing.Point(192, 31);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(625, 14);
		this.label22.TabIndex = 127;
		this.label22.Text = "A centralized record for creating, approving, and managing logistics and transportation agreements.";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(4, 27);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(186, 19);
		this.label21.TabIndex = 128;
		this.label21.Text = "Logistics Agreement :";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(970, 27);
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
		this.btnPrint.Text = "1301-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(889, 29);
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
		this.tabPage3.Size = new System.Drawing.Size(976, 582);
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 151);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(976, 214);
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
		this.panel9.Size = new System.Drawing.Size(976, 24);
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
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(976, 193);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 128);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(976, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(929, 0);
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
		this.panel13.Size = new System.Drawing.Size(976, 76);
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
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance242;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance225).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance226).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceProviderHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
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
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance204;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance160).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
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
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance237;
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
		this.panel38.Size = new System.Drawing.Size(976, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(976, 27);
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
		this.label32.Size = new System.Drawing.Size(240, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Logistic Agreement History  ";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(327, 12);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(92, 30);
		this.label8.TabIndex = 512;
		this.label8.Text = "Broker Agent / \r\nService Provider";
		this.CmbBrokerAgentName.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokerAgentName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokerAgentName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokerAgentName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokerAgentName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Location = new System.Drawing.Point(93, 70);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Name = "CmbBrokerAgentName";
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).TabIndex = 3;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(2, 74);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(84, 19);
		this.label14.TabIndex = 511;
		this.label14.Text = "Bill To Party";
		this.txtEffectiveDateTo.CustomFormat = "dd-MMM-yy";
		this.txtEffectiveDateTo.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEffectiveDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectiveDateTo.Location = new System.Drawing.Point(840, 44);
		this.txtEffectiveDateTo.Name = "txtEffectiveDateTo";
		this.txtEffectiveDateTo.Size = new System.Drawing.Size(120, 22);
		this.txtEffectiveDateTo.TabIndex = 11;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(726, 48);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(96, 15);
		this.label7.TabIndex = 162;
		this.label7.Text = "Effective Date To";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 611);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmLogisticAgreement";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmLogisticAgreement_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrencyDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateBaseType).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceType).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgentName).EndInit();
		base.ResumeLayout(false);
	}
}
