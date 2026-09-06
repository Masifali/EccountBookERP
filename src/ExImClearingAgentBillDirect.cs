using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Service;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Service;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Export;
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
namespace Architecture.WinApp.Service
{

public class ExImClearingAgentBillDirect : Form
{
	public UserAccount UserAccount = new UserAccount();

	private Rightsobjects formRights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private LoadExportInvoice LoadExportInvoice;

	private DataTable table = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtPartiesAlreadyUsedbyInvoice = new DataTable();

	private string ScreenName = "";

	private int DocumentTypeId = 155;

	private int RecId;

	private int VoucherHeadId;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool DebitAccountConfig;

	private bool ItemSearchWithNameOrCode;

	private int DefaultDaysToLessFromHistoryFromDate;

	private IContainer components;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grddetailadd;

	private TabPage tabHistory;

	internal GridEX grdhistory;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel1;

	private Panel panel7;

	private DateTimePicker txtdocdate;

	private Label label5;

	private TextBox txtdocno;

	private Label label9;

	internal UltraCombo CmbContractNoInvoiceData;

	private Label label8;

	internal UltraCombo CmbInvoiceNo;

	internal UltraCombo CmbPartyName;

	private Label label16;

	private TextBox txtMtonInvoiceData;

	private Label label15;

	private TextBox txtNoOfContainerInvoiceData;

	private Label label14;

	private Label label12;

	private Label label3;

	private TextBox txtrefbillno;

	private Label Forwarder;

	private Label label25;

	private Label label27;

	private TextBox txtduedays;

	private DateTimePicker txtduedate;

	private TextBox txtLcyAmountDetail;

	private Label label2;

	private Label label1;

	internal UltraCombo CmbFcyCodeDetail;

	private TextBox txtChargesRateFcyDetail;

	private Label label19;

	private Label label20;

	internal UltraCombo CmbUomDetail;

	private TextBox txtExchangeRateDetail;

	private Label label29;

	private TextBox txtdescription;

	private Label label30;

	private TextBox txtQtyDetail;

	private Label label31;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Label label32;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbDestinationPortInvoiceData;

	internal UltraCombo CmbLoadingPortInvoiceData;

	private ToolStrip btn;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnVoucher;

	private GroupBox groupBox1;

	private CtrlGrdBar ctrlGrdBar2;

	private Label lbldebitac;

	internal UltraCombo CmbDebitAc;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton1;

	private Label label4;

	private TextBox txtBookingRateInvoiceData;

	private GroupBox groupBox3;

	private GroupBox groupBox2;

	private Panel panel8;

	private Label label7;

	private ToolStripButton btnShortCutKey;

	private ToolStripButton btnDefineServices;

	private TextBox txtForwarderNameInvoiceData;

	private TextBox txtCustomerNameInvoiceData;

	private Label label10;

	private Label label6;

	private TextBox txtOtherChargesFcyDetail;

	private TextBox txtFcyAmountDetail;

	private Label label13;

	private Label label11;

	private TextBox txtTotalLcyAmountDetail;

	private Label label17;

	private Label label33;

	private Label label18;

	private Label label21;

	internal UltraCombo CmbDeliveredAtInvoiceData;

	internal UltraCombo CmbContainerDispatchedAtPortInvoiceData;

	internal UltraCombo CmbContainerReceivedFromPortInvoiceData;

	private Label label22;

	private TextBox txtGdNoInvoiceData;

	private Label label23;

	private TextBox txtIncoTermInvoiceData;

	private Label label48;

	private TextBox txtVesselNameInvoiceData;

	private TextBox txtBlNoInvoiceData;

	private Label label24;

	private RadioButton RadItemCodePackListDetail;

	private RadioButton RadItemNamePackListDetail;

	private ToolStrip toolStrip3;

	private ToolStripButton btnResetHistory;

	private ToolStripSeparator toolStripSeparator2;

	private Panel panel15;

	private Label label92;

	private Panel panel3;

	private GroupBox groupBox4;

	private Button btnShowHistory;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label89;

	private Label label91;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private RadioButton rdmodifydate;

	private Panel panel16;

	private Label label69;

	private Panel panel2;

	private Label label59;

	internal GridEX gridDetailHistory;

	internal UltraCombo CmbMasterItem;

	private Panel panel34;

	private RadioButton RadCategory;

	private RadioButton RadMasterItem;

	private CheckBox chkVoucher;

	private CheckBox ChkPreview;

	private ToolStripButton btnDelete;

	public ExImClearingAgentBillDirect(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 800);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("MasterItemId", typeof(int));
			dtItem.Columns.Add("MasterItem");
			dtItem.Columns.Add("ItemCategoryId", typeof(int));
			dtItem.Columns.Add("ItemCategory");
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode");
			table.Columns.Add("ItemName");
			table.Columns.Add("Qty", typeof(double));
			table.Columns.Add("UomId", typeof(int));
			table.Columns.Add("Uom");
			table.Columns.Add("ChargesRate", typeof(double));
			table.Columns.Add("CurrencyId", typeof(int));
			table.Columns.Add("Currency");
			table.Columns.Add("FcyAmount", typeof(double));
			table.Columns.Add("ExchangeRate", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("OtherChargesAmount", typeof(double));
			table.Columns.Add("TotalLcyAmount", typeof(double));
			table.Columns.Add("Description");
			table.Columns.Add("DebitAcId", typeof(int));
			table.Columns.Add("DebitAc");
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
			ScreenName = base.Name;
			formRights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			string documentCode = GetDocumentCode();
			DataTable portData = PortsDbCall();
			GetConfigurationsFromGlobal();
			DataTable exportInvoicePendingAndAll = getExportInvoicePendingAndAll();
			return new
			{
				documentNoData = documentCode,
				PortData = portData,
				InvoiceNoData = exportInvoicePendingAndAll
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
				btnSave.Enabled = formRights.DoHaveSaveRight;
				btnDelete.Enabled = formRights.DoHaveCanDelete;
				btnUpdate.Enabled = formRights.DoHaveUpdateRights;
				btnprint.Enabled = formRights.DoHavePrintRights;
				btnVoucher.Enabled = formRights.DoHavePrintRights;
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
				if (ItemSearchWithNameOrCode)
				{
					RadItemNamePackListDetail.Checked = true;
				}
				else
				{
					RadItemNamePackListDetail.Checked = true;
				}
				lbldebitac.Visible = false;
				((Control)(object)CmbDebitAc).Visible = false;
				txtdescription.Width = 1010;
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				InvoiceNoBind(result.InvoiceNoData);
				PortsBind(result.PortData);
				SupplierDtFillFromGlobal();
				BindSupplierName();
				DeliveredAtBind();
				ItemCategoryOrMasterItemBind();
				ItemDtsFillFromGlobal();
				ItemNameBind();
				CurrencyBindFromGlobal();
				if (DebitAccountConfig)
				{
					txtdescription.Width = 716;
					lbldebitac.Visible = true;
					((Control)(object)CmbDebitAc).Visible = true;
					AccountlstDtFillFromGlobal();
				}
				BindGrids();
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
			DebitAccountConfig = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ServicesBillDebitAmountChargetoSelectedAc"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchWithNameOrCode"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private string GetDocumentCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)ExImClearingAgentBill.GenerateCode(new ExImClearingAgentBill
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable getExportInvoicePendingAndAll()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		DataTable dtInvoice = new DataTable();
		try
		{
			dtInvoice = ExImInvoice.getExportInvoicePendingAndAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dtInvoice;
	}

	private void InvoiceNoBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNo, dt, "Id", "InvoiceNo", "InvoiceNo", AllColumns: true, Conversion.ToInt(CmbInvoiceNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			if (dtPartiesAlreadyUsedbyInvoice != null && dtPartiesAlreadyUsedbyInvoice.Rows.Count > 0)
			{
				List<int> usedIds = (from row in dtPartiesAlreadyUsedbyInvoice.AsEnumerable()
					select row.Field<int>("SupplierCustomerId")).ToList();
				{
					foreach (getGlobalAllSupplierCustomer item in clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 10 && !usedIds.Contains(r.Id)))
					{
						dtSupplier.Rows.Add(Conversion.ToInt((object)item.Id), Conversion.ToString((object)item.CompanyName), Conversion.ToString((object)item.PartyCode), Conversion.ToString((object)item.CityName), Conversion.ToString((object)item.MobilePersonal));
					}
					return;
				}
			}
			foreach (getGlobalAllSupplierCustomer item2 in clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 10))
			{
				dtSupplier.Rows.Add(Conversion.ToInt((object)item2.Id), Conversion.ToString((object)item2.CompanyName), Conversion.ToString((object)item2.PartyCode), Conversion.ToString((object)item2.CityName), Conversion.ToString((object)item2.MobilePersonal));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbPartyName, dtSupplier, "Id", "CompanyName", "PartyName", AllColumns: true, Conversion.ToInt(CmbPartyName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			InfragisticsHelper.BindAndRetainSelection(CmbLoadingPortInvoiceData, dt, "Id", "PortName", "Loading Port", AllColumns: false, Conversion.ToInt(CmbLoadingPortInvoiceData.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDestinationPortInvoiceData, dt, "Id", "PortName", "Destination Port", AllColumns: false, Conversion.ToInt(CmbDestinationPortInvoiceData.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbContainerDispatchedAtPortInvoiceData, dt, "Id", "PortName", "Container Dispatched At Port", AllColumns: false, Conversion.ToInt(CmbContainerDispatchedAtPortInvoiceData.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbContainerReceivedFromPortInvoiceData, dt, "Id", "PortName", "Container Received From Port", AllColumns: false, Conversion.ToInt(CmbContainerReceivedFromPortInvoiceData.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveredAtBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dt.Rows.Add(1, UserAccount.CompName);
			dt.Rows.Add(2, "Other");
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveredAtInvoiceData, dt, "Id", "Name", "DELIVERED AT", AllColumns: false, Conversion.ToInt(CmbDeliveredAtInvoiceData.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbeximinvoiceno_Leave(object sender, EventArgs e)
	{
		try
		{
			dtPartiesAlreadyUsedbyInvoice.Clear();
			int InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			BindInvoiceData(DataByInvoiceDbCall(InvoiceId));
			SupplierDtFillFromGlobal();
			BindSupplierName();
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

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		try
		{
			int InvoiceId = Conversion.ToInt(CmbItemName.Value);
			ItemUomFromGlobalBind(InvoiceId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemUomFromGlobalBind(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)CmbUomDetail).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			InfragisticsHelper.BindAndRetainSelection(CmbUomDetail, dtUOM, "Id", "UOMCode", "Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CurrencyBindFromGlobal()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CurrencyCode", typeof(string));
			foreach (MultiCurrency dd in clsGlobalVariables.globalMultiCurrency)
			{
				dt.Rows.Add(dd.Id, dd.CurrencyCode);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbFcyCodeDetail, dt, "Id", "CurrencyCode", "Fcy Code", AllColumns: true, Conversion.ToInt(CmbFcyCodeDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountlstDtFillFromGlobal()
	{
		try
		{
			DataTable dtAccount = new DataTable();
			dtAccount.Columns.Add("Id", typeof(int));
			dtAccount.Columns.Add("AccountTitle", typeof(string));
			dtAccount.Columns.Add("AccountCode", typeof(string));
			foreach (getGlobalAllAccountsWithCustomGroup account in from r in clsGlobalVariables.AllAccountsWithCustomGroupId
				where r.AccountTypeId == 6 || r.AccountTypeId == 8 || r.AccountTypeId == 10
				group r by r.ChartOfAccountId into g
				select g.First())
			{
				int id = Conversion.ToInt((object)account.ChartOfAccountId);
				dtAccount.Rows.Add(id, account.AccountTitle, account.AccountCode);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbDebitAc, dtAccount, "Id", "AccountTitle", "Debit Title", AllColumns: true, Conversion.ToInt(CmbDebitAc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private bool FormValidationDetail()
	{
		List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbItemName, "Item Name", FormHelper.ValidType.INT),
			(txtQtyDetail, "Quantity", FormHelper.ValidType.Double),
			((Control)(object)CmbUomDetail, "Rate UOM", FormHelper.ValidType.INT),
			(txtChargesRateFcyDetail, "Charges Rate", FormHelper.ValidType.Double),
			((Control)(object)CmbFcyCodeDetail, "Currency", FormHelper.ValidType.INT),
			(txtFcyAmountDetail, "Fcy Amount", FormHelper.ValidType.Double),
			(txtLcyAmountDetail, "Lcy Amount", FormHelper.ValidType.Double),
			(txtTotalLcyAmountDetail, "Total Lcy Amount", FormHelper.ValidType.Double),
			(txtdescription, "Description", FormHelper.ValidType.String)
		};
		if (DebitAccountConfig)
		{
			controlsToValidate.Add(((Control)(object)CmbDebitAc, "Debit Account", FormHelper.ValidType.INT));
		}
		return FormHelper.ValidateControls(controlsToValidate);
	}

	private void BindGrids()
	{
		try
		{
			if (table != null && table.Columns.Count > 0)
			{
				grddetailadd.DataSource = table;
				grddetailadd.RetrieveStructure();
				gridsetting(grddetailadd);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows.Add(0, Conversion.ToInt(CmbItemName.Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), Conversion.ToDouble((object)txtQtyDetail.Text), Conversion.ToInt(CmbUomDetail.Value), Conversion.ToString((object)((Control)(object)CmbUomDetail).Text), Conversion.ToDouble((object)txtChargesRateFcyDetail.Text), Conversion.ToInt(CmbFcyCodeDetail.Value), Conversion.ToString((object)((Control)(object)CmbFcyCodeDetail).Text), Conversion.ToDouble((object)txtFcyAmountDetail.Text), Conversion.ToDouble((object)txtExchangeRateDetail.Text), Conversion.ToDouble((object)txtLcyAmountDetail.Text), Conversion.ToDouble((object)txtOtherChargesFcyDetail.Text), Conversion.ToDouble((object)txtTotalLcyAmountDetail.Text), Conversion.ToString((object)txtdescription.Text), Conversion.ToInt(CmbDebitAc.Value), Conversion.ToString((object)((Control)(object)CmbDebitAc).Text));
				BindGrids();
				resetdetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailadd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grddetailadd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)CmbMasterItem).Text = "";
				ItemDtsFillFromGlobal();
				ItemNameBind();
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				cmbitem_Leave(null, null);
				txtQtyDetail.Text = Conversion.ToDouble(item.Cells["Qty"].Value).ToString("#,##0.##");
				CmbUomDetail.Value = item.Cells["UomId"].Value.ToString();
				txtChargesRateFcyDetail.Text = Conversion.ToDouble(item.Cells["ChargesRate"].Value).ToString("#,##0.####");
				CmbFcyCodeDetail.Value = item.Cells["CurrencyId"].Value.ToString();
				txtFcyAmountDetail.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString("#,##0.####");
				txtExchangeRateDetail.Text = Conversion.ToDouble(item.Cells["ExchangeRate"].Value).ToString("#,##0.####");
				txtLcyAmountDetail.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString("#,##0.####");
				txtOtherChargesFcyDetail.Text = Conversion.ToDouble(item.Cells["OtherChargesAmount"].Value).ToString("#,##0.####");
				txtTotalLcyAmountDetail.Text = Conversion.ToDouble(item.Cells["TotalLcyAmount"].Value).ToString("#,##0.####");
				txtdescription.Text = item.Cells["Description"].Value.ToString();
				CmbDebitAc.Value = item.Cells["DebitAcId"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItemName.Focus();
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
			if (FormValidationDetail())
			{
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItemName.Value);
				table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
				table.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
				table.Rows[updateDetailIndex]["Qty"] = Conversion.ToDouble((object)txtQtyDetail.Text);
				table.Rows[updateDetailIndex]["UomId"] = Conversion.ToInt(CmbUomDetail.Value);
				table.Rows[updateDetailIndex]["Uom"] = Conversion.ToString((object)((Control)(object)CmbUomDetail).Text);
				table.Rows[updateDetailIndex]["ChargesRate"] = Conversion.ToDouble((object)txtChargesRateFcyDetail.Text);
				table.Rows[updateDetailIndex]["CurrencyId"] = Conversion.ToInt(CmbFcyCodeDetail.Value);
				table.Rows[updateDetailIndex]["Currency"] = Conversion.ToString((object)((Control)(object)CmbFcyCodeDetail).Text);
				table.Rows[updateDetailIndex]["FcyAmount"] = Conversion.ToDouble((object)txtFcyAmountDetail.Text);
				table.Rows[updateDetailIndex]["ExchangeRate"] = Conversion.ToDouble((object)txtExchangeRateDetail.Text);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtLcyAmountDetail.Text);
				table.Rows[updateDetailIndex]["OtherChargesAmount"] = Conversion.ToDouble((object)txtOtherChargesFcyDetail.Text);
				table.Rows[updateDetailIndex]["TotalLcyAmount"] = Conversion.ToDouble((object)txtTotalLcyAmountDetail.Text);
				table.Rows[updateDetailIndex]["Description"] = Conversion.ToString((object)txtdescription.Text);
				table.Rows[updateDetailIndex]["DebitAcId"] = Conversion.ToInt(CmbDebitAc.Value);
				table.Rows[updateDetailIndex]["DebitAc"] = Conversion.ToString((object)((Control)(object)CmbDebitAc).Text);
				BindGrids();
				resetdetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			resetdetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "UomId", "ItemId", "CurrencyId", "DebitAcId", "CurrencyId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns);
			grid.RootTable.Columns["DebitAc"].Visible = DebitAccountConfig;
			if (((Control)(object)grid).Name == "grddetailadd")
			{
				GridEX_Helper.AddButton(grid, "Delete", "X", 20, 1);
				grid.FrozenColumns = 1;
				ctrlGrdBar3_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailadd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grddetailadd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grddetailadd.UpdateData();
				return;
			}
			throw new Exception("Reset Detail First");
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
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		try
		{
			if (grddetailadd.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.String),
				((Control)(object)CmbInvoiceNo, "Invoice No", FormHelper.ValidType.INT),
				((Control)(object)CmbPartyName, "Party Name", FormHelper.ValidType.INT),
				((Control)(object)CmbContractNoInvoiceData, "Contract No", FormHelper.ValidType.INT),
				((Control)(object)CmbLoadingPortInvoiceData, "Loading Place", FormHelper.ValidType.INT),
				((Control)(object)CmbDestinationPortInvoiceData, "Destination Place", FormHelper.ValidType.INT),
				(txtNoOfContainerInvoiceData, "No. of Containers", FormHelper.ValidType.Double),
				(txtMtonInvoiceData, "M.Tons", FormHelper.ValidType.Double)
			}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			ExImClearingAgentBill obj = new ExImClearingAgentBill();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.ProjectId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocTypeId = DocumentTypeId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.IsApproved = false;
			obj.Id = RecId;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			obj.DueDays = Conversion.ToInt((object)txtduedays.Text);
			obj.DueDate = Conversion.ToDateTime((object)txtduedate.Value);
			obj.RefBillNo = Conversion.ToString((object)txtrefbillno.Text);
			obj.ExportInvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			obj.LcContractId = Conversion.ToInt(CmbContractNoInvoiceData.Value);
			obj.NoOfContainer = Conversion.ToInt((object)txtNoOfContainerInvoiceData.Text);
			obj.NetBillWeight = Conversion.ToDouble((object)txtMtonInvoiceData.Text);
			obj.LoadingPortId = Conversion.ToInt(CmbLoadingPortInvoiceData.Value);
			obj.DestinationPortId = Conversion.ToInt(CmbDestinationPortInvoiceData.Value);
			obj.BookingRate = Conversion.ToDecimal((object)txtBookingRateInvoiceData.Text);
			obj.USDRate = 0.0;
			obj.SupCustId = Conversion.ToInt(CmbPartyName.Value);
			obj.BillDetail = new List<ExImClearingAgentBillDetail>();
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			double TotalAmount = 0.0;
			GridEXRow[] rows = grddetailadd.GetRows();
			foreach (GridEXRow r in rows)
			{
				ExImClearingAgentBillDetail labdetail = new ExImClearingAgentBillDetail();
				labdetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				labdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				labdetail.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
				labdetail.UomId = Conversion.ToInt(r.Cells["UomId"].Value);
				labdetail.ChargesRate = Conversion.ToDouble((object)r.Cells["ChargesRate"].Text);
				labdetail.ItemRate = Conversion.ToDouble((object)r.Cells["ChargesRate"].Text);
				labdetail.FcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
				labdetail.CurrencyId = Conversion.ToInt(r.Cells["CurrencyId"].Value);
				labdetail.ExhangeRate = Conversion.ToDouble(r.Cells["ExchangeRate"].Value);
				labdetail.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
				labdetail.OtherChargesAmount = Conversion.ToDouble(r.Cells["OtherChargesAmount"].Value);
				labdetail.TotalAmount = Conversion.ToDouble(r.Cells["TotalLcyAmount"].Value);
				labdetail.Description = Conversion.ToString((object)r.Cells["Description"].Text);
				labdetail.DebitAccountId = Conversion.ToInt(r.Cells["DebitAcId"].Value);
				TotalAmount += labdetail.TotalAmount;
				labdetail.DocDate = DateTime.Now;
				labdetail.EffectedDateD = DateTime.Now;
				labdetail.ValidUpTo = DateTime.Now;
				obj.BillDetail.Add(labdetail);
			}
			obj.TotalAmount = TotalAmount;
			obj.GrandTotal = TotalAmount;
			FormHelper.UpdateAttachmentsForObject<ExImClearingAgentBill>(obj, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			ExImClearingAgentBill.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, ScreenName, DocumentTypeId, 0);
				}
			}
			reset();
			if (ChkPreview.Checked && formRights.DoHavePrintRights)
			{
				Slip420(obj.Id);
			}
			if (chkVoucher.Checked && formRights.DoHavePrintRights)
			{
				CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(obj.Id, DocumentTypeId), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "ErrorMessage", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "ErrorMessage", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record not update because RecId not found");
			}
			Insert();
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
			reset();
			RecId = ID;
			ExImClearingAgentBill billHeader = ExImClearingAgentBill.GetByID(RecId);
			if (billHeader == null)
			{
				throw new Exception("Record not found");
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocno.Text = Conversion.ToString((object)billHeader.DocNo);
			txtdocdate.Value = Conversion.ToDateTime((object)billHeader.DocDate);
			txtduedays.Text = billHeader.DueDays.ToString();
			txtduedate.Value = Conversion.ToDateTime((object)billHeader.DueDate);
			txtrefbillno.Text = Conversion.ToString((object)billHeader.RefBillNo);
			CmbInvoiceNo.Value = billHeader.ExportInvoiceId.ToString();
			cmbeximinvoiceno_Leave(null, null);
			CmbPartyName.Value = billHeader.SupCustId.ToString();
			CustomAttachmentsValues = billHeader.CustomAttachmentsValues;
			AttachmentsValues = billHeader.AttachmentsValues;
			table.Rows.Clear();
			FillDetailFromListCommonForReadById(billHeader, table);
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(ExImClearingAgentBill invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.BillDetail : null) == null || invGrn.BillDetail.Count == 0)
		{
			return;
		}
		foreach (ExImClearingAgentBillDetail detail in invGrn.BillDetail)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.ItemId, detail.ItemCode, detail.ItemName, detail.Qty, detail.UomId, detail.UomName, detail.ChargesRate, detail.CurrencyId, detail.CurrencyName,
				detail.FcyAmount, detail.ExhangeRate, detail.Amount, detail.OtherChargesAmount, detail.TotalAmount, detail.Description, detail.DebitAccountId, detail.AccountTitle
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			txtdocno.Text = string.Empty;
			txtrefbillno.Text = string.Empty;
			txtduedate.Value = DateTime.Now;
			txtduedays.Text = string.Empty;
			txtdocdate.Value = DateTime.Now;
			((Control)(object)CmbPartyName).Text = string.Empty;
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			cmbeximinvoiceno_Leave(null, null);
			resetdetail();
			table.Clear();
			BindGrids();
			UpdateDocumentNoUI(GetDocumentCode());
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetdetail()
	{
		try
		{
			((Control)(object)CmbItemName).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			((UltraGridBase)CmbUomDetail).DataSource = null;
			((Control)(object)CmbUomDetail).Text = string.Empty;
			txtChargesRateFcyDetail.Text = string.Empty;
			((Control)(object)CmbFcyCodeDetail).Text = string.Empty;
			txtFcyAmountDetail.Text = string.Empty;
			txtExchangeRateDetail.Text = string.Empty;
			txtLcyAmountDetail.Text = string.Empty;
			txtOtherChargesFcyDetail.Text = string.Empty;
			txtTotalLcyAmountDetail.Text = string.Empty;
			txtdescription.Text = string.Empty;
			((Control)(object)CmbDebitAc).Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			updateDetailIndex = -1;
			CmbItemName.Focus();
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
			reset();
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
			List<string> allServices = new List<string> { "UomSchedule", "SupplierCustomerLists", "AllSerivesItems", "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			InvoiceNoBind(getExportInvoicePendingAndAll());
			cmbeximinvoiceno_Leave(null, null);
			PortsBind(PortsDbCall());
			SupplierDtFillFromGlobal();
			BindSupplierName();
			DeliveredAtBind();
			ItemCategoryOrMasterItemBind();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			CurrencyBindFromGlobal();
			if (DebitAccountConfig)
			{
				txtdescription.Width = 716;
				lbldebitac.Visible = true;
				((Control)(object)CmbDebitAc).Visible = true;
				AccountlstDtFillFromGlobal();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImClearingAgentBillDirect_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar3")
			{
				CmbItemName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar3")
			{
				CmbItemName.Focus();
			}
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
				FromDateHistory.Focus();
			}
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
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			grdhistory.DataSource = null;
			gridDetailHistory.DataSource = null;
			FromDateHistory.Focus();
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
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (!formRights.DoHaveCanViewAllRecordRights)
			{
				obj.EntryUser = UserAccount.ID;
			}
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
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			DataTable dt = ExImClearingAgentBill.FormHistoryDirect(obj);
			if (dt.Rows.Count > 0)
			{
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id");
					dtHistoryGrid.Columns.Add("VoucherHeadId", typeof(int));
					dtHistoryGrid.Columns.Add("DocNo", typeof(int));
					dtHistoryGrid.Columns.Add("DocDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("PartyName");
					dtHistoryGrid.Columns.Add("InvoiceNo");
					dtHistoryGrid.Columns.Add("ContractNo");
					dtHistoryGrid.Columns.Add("LoadingPort");
					dtHistoryGrid.Columns.Add("DestinationPort");
					dtHistoryGrid.Columns.Add("NoOfContainer", typeof(double));
					dtHistoryGrid.Columns.Add("NetBillWeight", typeof(double));
					dtHistoryGrid.Columns.Add("TotalAmount", typeof(double));
					dtHistoryGrid.Columns.Add("EntryUser");
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUser");
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistoryGrid.Clear();
				foreach (DataRow row in dt.Rows)
				{
					dtHistoryGrid.Rows.Add(row["Id"], row["VoucherHeadId"], row["DocNo"], Conversion.ToDateTime(row["DocDate"]).ToString("dd-MMM-yyyy"), row["PartyName"], row["InvoiceNo"], row["ContractNo"], row["LoadingPort"], row["DestinationPort"], row["NoOfContainer"], row["NetBillWeight"], row["TotalAmount"], row["EntryUserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["NoOfAttachments"]);
				}
				grdhistory.DataSource = dtHistoryGrid;
				grdhistory.RetrieveStructure();
				GridHistorySetting();
			}
			else
			{
				grdhistory.ClearStructure();
				gridDetailHistory.ClearStructure();
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
			List<string> hiddenColumns = new List<string> { "Id", "VoucherHeadId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, hiddenColumns);
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			grdhistory.RootTable.Columns["PartyName"].Width = Constants.ExportConstants.StringConstant;
			grdhistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdhistory.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.StringConstant;
			grdhistory.RootTable.Columns["LoadingPort"].Width = Constants.ExportConstants.PortConstant;
			grdhistory.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			grdhistory.RootTable.Columns["NoOfContainer"].Width = Constants.ExportConstants.NoOfContainers;
			grdhistory.RootTable.Columns["NetBillWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdhistory.RootTable.Columns["TotalAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants - 12;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM + 10;
			ctrlGrdBar2_Load(null, null);
			if (formRights.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(grdhistory, "Slip", "Slip", 50, grdhistory.RootTable.Columns["DocNo"].Position - 1);
			}
			if (formRights.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(grdhistory, "Voucher", "Voucher", 70, grdhistory.RootTable.Columns["DocNo"].Position - 1);
			}
			GridEX_Helper.AddButton(grdhistory, "AddAttachment", "Add Attachment", 110);
			grdhistory.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				int currId = Conversion.ToInt(row.Cells["Id"].Value);
				VoucherHeadId = Conversion.ToInt(row.Cells["VoucherHeadId"].Value);
				ReadById(currId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int ReportId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				Slip420(ReportId);
			}
			else if (e.Column.Key == "Voucher")
			{
				CommonServices.VoucherReport_118(Conversion.ToInt(grdhistory.CurrentRow.Cells["VoucherHeadId"].Value));
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, ReportId, DocumentTypeId, ScreenName);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
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
			ExImClearingAgentBill Obj = ExImClearingAgentBill.GetByID(Id);
			if (Obj == null || Obj.BillDetail.Count == 0)
			{
				gridDetailHistory.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = table.Clone();
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
			gridDetailHistory.DataSource = dtDetailHistory;
			gridDetailHistory.RetrieveStructure();
			gridsetting(gridDetailHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void totalamount()
	{
		try
		{
			double chargesRate = Conversion.ToDouble((object)txtChargesRateFcyDetail.Text);
			double num = Conversion.ToDouble((object)txtExchangeRateDetail.Text);
			double qty = Conversion.ToDouble((object)txtQtyDetail.Text);
			double otherChargesAmount = Conversion.ToDouble((object)txtOtherChargesFcyDetail.Text);
			double fcyAmount = chargesRate * qty;
			double lcyAmount = num * fcyAmount;
			double totalLcyAmount = lcyAmount + otherChargesAmount;
			txtFcyAmountDetail.Text = fcyAmount.ToString("#,##0.####");
			txtLcyAmountDetail.Text = lcyAmount.ToString("#,##0.####");
			txtTotalLcyAmountDetail.Text = totalLcyAmount.ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show("Calculation error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtchargesrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtexrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherChargesFcyDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			totalamount();
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grddetailadd, table);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdhistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
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

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			Slip420(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Slip420(int ReportId)
	{
		try
		{
			CommonServices.ServicesBillSlip540(ReportId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				int HeaderId = 0;
				HeaderId = ((VoucherHeadId <= 0) ? CommonServices.VoucherHeadIdGet(RecId, DocumentTypeId) : VoucherHeadId);
				CommonServices.VoucherReport_118(HeaderId, DocumentTypeId);
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDaysCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDaysCalculate()
	{
		try
		{
			if (Conversion.ToInt((object)txtduedays.Text.Trim()) > 0)
			{
				txtduedate.Value = txtdocdate.Value.AddDays(Conversion.ToInt((object)txtduedays.Text.Trim()));
			}
			else
			{
				txtduedate.Value = txtdocdate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DueDaysCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		LoadExportInvoice = new LoadExportInvoice(UserAccount);
		LoadExportInvoice.ShowDialog();
		LoadExportInvoice.BringToFront();
		int InvoiceId = 0;
		if (LoadExportInvoice.dtSupply.Rows.Count > 0)
		{
			InvoiceId = Conversion.ToInt(LoadExportInvoice.dtSupply.Rows[0]["InvoiceId"]);
		}
		CmbInvoiceNo.Value = InvoiceId;
		cmbeximinvoiceno_Leave(null, null);
	}

	private DataTable DataByInvoiceDbCall(int InvoiceId)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			if (InvoiceId > 0)
			{
				dt = ExImInvoice.GetInvoiceInformationByInvoiceIdForServices(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = InvoiceId
				});
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindInvoiceData(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				txtCustomerNameInvoiceData.Text = Conversion.ToString(dt.Rows[0]["CustomerName"]);
				txtForwarderNameInvoiceData.Text = Conversion.ToString(dt.Rows[0]["ForwarderName"]);
				DDL.BindDDLNew(dt, CmbContractNoInvoiceData, "LcOrderNoId", "LcOrderNo", "ContractNo", false);
				((UltraGridBase)CmbContractNoInvoiceData).Rows[0].Activate();
				txtBlNoInvoiceData.Text = Conversion.ToString(dt.Rows[0]["BLNumber"]);
				txtGdNoInvoiceData.Text = Conversion.ToString(dt.Rows[0]["GdNo"]);
				CmbLoadingPortInvoiceData.Value = Conversion.ToInt(dt.Rows[0]["LoadingPortId"]);
				CmbDestinationPortInvoiceData.Value = Conversion.ToInt(dt.Rows[0]["DestinationPortId"]);
				CmbContainerDispatchedAtPortInvoiceData.Value = Conversion.ToInt(dt.Rows[0]["ContainerDispatchedAtPortId"]);
				CmbContainerReceivedFromPortInvoiceData.Value = Conversion.ToInt(dt.Rows[0]["ContainerReceivedFromPortId"]);
				CmbDeliveredAtInvoiceData.Value = Conversion.ToInt(dt.Rows[0]["DeliveredAtId"]);
				txtVesselNameInvoiceData.Text = Conversion.ToString(dt.Rows[0]["VesselName"]);
				txtIncoTermInvoiceData.Text = Conversion.ToString(dt.Rows[0]["DeliveryTerm"]);
				txtNoOfContainerInvoiceData.Text = Conversion.ToString(dt.Rows[0]["NoOfContainers"]);
				double MTon = Conversion.ToDouble(dt.Rows[0]["NetWeight"]) / 1000.0;
				txtMtonInvoiceData.Text = MTon.ToString("#,##0.###");
				txtBookingRateInvoiceData.Text = Conversion.ToDouble(dt.Rows[0]["BookingRate"]).ToString("#,##0.###");
				if (Conversion.ToInt(dt.Rows[0]["CreditAccountId"]) > 0)
				{
					CmbDebitAc.Value = Conversion.ToInt(dt.Rows[0]["CreditAccountId"]);
				}
			}
			else
			{
				txtCustomerNameInvoiceData.Text = string.Empty;
				txtForwarderNameInvoiceData.Text = string.Empty;
				((Control)(object)CmbContractNoInvoiceData).Text = string.Empty;
				txtBlNoInvoiceData.Text = string.Empty;
				txtGdNoInvoiceData.Text = string.Empty;
				((Control)(object)CmbLoadingPortInvoiceData).Text = string.Empty;
				((Control)(object)CmbDestinationPortInvoiceData).Text = string.Empty;
				((Control)(object)CmbContainerDispatchedAtPortInvoiceData).Text = string.Empty;
				((Control)(object)CmbContainerReceivedFromPortInvoiceData).Text = string.Empty;
				((Control)(object)CmbDeliveredAtInvoiceData).Text = string.Empty;
				txtVesselNameInvoiceData.Text = string.Empty;
				txtIncoTermInvoiceData.Text = string.Empty;
				txtNoOfContainerInvoiceData.Text = string.Empty;
				txtMtonInvoiceData.Text = string.Empty;
				txtBookingRateInvoiceData.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartiesAlreadyUsedbyInvoiceDBCall(int InvoiceId)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			if (InvoiceId > 0)
			{
				ExImInvoice obj = new ExImInvoice();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.Id = InvoiceId;
				obj.ActionId = RecId;
				dtPartiesAlreadyUsedbyInvoice = ExImInvoice.GetSupplierCustomerIdsFromServicesBillbyInvoice(obj);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtchargesrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtexrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void btnDefineServices_Click(object sender, EventArgs e)
	{
		try
		{
			EximServicesDefine eximServicesDefine = new EximServicesDefine(UserAccount);
			eximServicesDefine.WindowState = FormWindowState.Normal;
			eximServicesDefine.StartPosition = FormStartPosition.CenterScreen;
			eximServicesDefine.ShowDialog();
			eximServicesDefine.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImClearingAgentBillDirect_KeyDown(object sender, KeyEventArgs e)
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
					txtdocdate.Focus();
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
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && formRights.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
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
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (((Control)(object)grdhistory).Focused)
				{
					((Control)(object)gridDetailHistory).Focus();
				}
				else
				{
					((Control)(object)grdhistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdhistory).Focused)
				{
					((Control)(object)gridDetailHistory).Focus();
				}
				else
				{
					((Control)(object)grdhistory).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+L", "When In History Tab For Load All Records");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
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

	private void grddetailadd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEX grid = grddetailadd;
			GridEXRow r = grid.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grddetailadd_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEX grid = grdhistory;
			GridEXRow row = grid.CurrentRow;
			if (row == null || (int)row.RowType != 0)
			{
				return;
			}
			int currId = Conversion.ToInt(row.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formRights.DoHaveUpdateRights)
				{
					throw new Exception("You don't have the right to update.");
				}
				grdhistory_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn != null)
			{
				if (grid.CurrentColumn.Key == "NoOfAttachment")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(currId, DocumentTypeId);
				}
				else if (grid.CurrentColumn.Key == "Slip")
				{
					Slip420(currId);
				}
				else if (grid.CurrentColumn.Key == "Voucher")
				{
					CommonServices.VoucherReport_118(Conversion.ToInt(row.Cells["VoucherHeadId"].Value));
				}
			}
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
					ExImClearingAgentBill.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					reset();
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
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Expected O, but got Unknown
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Expected O, but got Unknown
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Expected O, but got Unknown
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Expected O, but got Unknown
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Expected O, but got Unknown
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Expected O, but got Unknown
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Expected O, but got Unknown
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c50: Expected O, but got Unknown
		//IL_0e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e91: Expected O, but got Unknown
		//IL_0eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebf: Expected O, but got Unknown
		GridEXLayout grddetailadd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout gridDetailHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
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
		Appearance appearance80 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
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
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.ExImClearingAgentBillDirect));
		this.grddetailadd = new GridEX();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.gridDetailHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.label89 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.panel34 = new System.Windows.Forms.Panel();
		this.RadCategory = new System.Windows.Forms.RadioButton();
		this.RadMasterItem = new System.Windows.Forms.RadioButton();
		this.CmbMasterItem = new UltraCombo();
		this.RadItemCodePackListDetail = new System.Windows.Forms.RadioButton();
		this.RadItemNamePackListDetail = new System.Windows.Forms.RadioButton();
		this.btnplus = new System.Windows.Forms.Button();
		this.CmbItemName = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.txtdescription = new System.Windows.Forms.TextBox();
		this.CmbDebitAc = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtQtyDetail = new System.Windows.Forms.TextBox();
		this.CmbFcyCodeDetail = new UltraCombo();
		this.txtChargesRateFcyDetail = new System.Windows.Forms.TextBox();
		this.lbldebitac = new System.Windows.Forms.Label();
		this.txtTotalLcyAmountDetail = new System.Windows.Forms.TextBox();
		this.txtLcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbUomDetail = new UltraCombo();
		this.txtOtherChargesFcyDetail = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtExchangeRateDetail = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.txtduedate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtrefbillno = new System.Windows.Forms.TextBox();
		this.CmbPartyName = new UltraCombo();
		this.CmbInvoiceNo = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.Forwarder = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.chkVoucher = new System.Windows.Forms.CheckBox();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtNoOfContainerInvoiceData = new System.Windows.Forms.TextBox();
		this.CmbDestinationPortInvoiceData = new UltraCombo();
		this.CmbLoadingPortInvoiceData = new UltraCombo();
		this.CmbDeliveredAtInvoiceData = new UltraCombo();
		this.CmbContainerDispatchedAtPortInvoiceData = new UltraCombo();
		this.CmbContainerReceivedFromPortInvoiceData = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.txtVesselNameInvoiceData = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtBlNoInvoiceData = new System.Windows.Forms.TextBox();
		this.txtMtonInvoiceData = new System.Windows.Forms.TextBox();
		this.txtForwarderNameInvoiceData = new System.Windows.Forms.TextBox();
		this.txtBookingRateInvoiceData = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbContractNoInvoiceData = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.txtCustomerNameInvoiceData = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtIncoTermInvoiceData = new System.Windows.Forms.TextBox();
		this.txtGdNoInvoiceData = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnVoucher = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnDefineServices = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).BeginInit();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel34.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCodeDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomDetail).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPortInvoiceData).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPortInvoiceData).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveredAtInvoiceData).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerDispatchedAtPortInvoiceData).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerReceivedFromPortInvoiceData).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNoInvoiceData).BeginInit();
		this.btn.SuspendLayout();
		base.SuspendLayout();
		this.grddetailadd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetailadd).BackColor = System.Drawing.Color.White;
		grddetailadd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailadd.DesignTimeLayout = grddetailadd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailadd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailadd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Location = new System.Drawing.Point(3, 352);
		((System.Windows.Forms.Control)(object)this.grddetailadd).Name = "grddetailadd";
		((ScrollableBase)this.grddetailadd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Size = new System.Drawing.Size(1170, 273);
		((System.Windows.Forms.Control)(object)this.grddetailadd).TabIndex = 0;
		this.grddetailadd.TotalRow = (InheritableBoolean)1;
		this.grddetailadd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailadd).VisualStyle = (VisualStyle)4;
		this.grddetailadd.ColumnButtonClick += new ColumnActionEventHandler(grddetailadd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grddetailadd).DoubleClick += new System.EventHandler(grddetailadd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grddetailadd).KeyDown += new System.Windows.Forms.KeyEventHandler(grddetailadd_KeyDown);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabHistory.Controls.Add(this.panel2);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailHistory);
		this.tabHistory.Controls.Add(this.panel15);
		this.tabHistory.Controls.Add(this.panel3);
		this.tabHistory.Controls.Add(this.panel16);
		this.tabHistory.Controls.Add(this.toolStrip3);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1176, 628);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(3, 132);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1170, 295);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label59);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(3, 427);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1170, 25);
		this.panel2.TabIndex = 348;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(5, 5);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(228, 17);
		this.label59.TabIndex = 562;
		this.label59.Text = "Detail of the selected above row";
		this.gridDetailHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailHistory.DesignTimeLayout = gridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.gridDetailHistory.DynamicFiltering = true;
		this.gridDetailHistory.FilterMode = (FilterMode)2;
		this.gridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Location = new System.Drawing.Point(3, 452);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Name = "gridDetailHistory";
		((ScrollableBase)this.gridDetailHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Size = new System.Drawing.Size(1170, 173);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).TabIndex = 1;
		this.gridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label92);
		this.panel15.Controls.Add(this.ctrlGrdBar2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 108);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1170, 24);
		this.panel15.TabIndex = 56;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(2, 2);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 1;
		this.label92.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1123, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar2.TabIndex = 40;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel3.Controls.Add(this.groupBox4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 58);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1170, 50);
		this.panel3.TabIndex = 55;
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.btnShowHistory);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.label89);
		this.groupBox4.Controls.Add(this.label91);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(6, 0);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1159, 47);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(807, 18);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(891, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 14);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(56, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(729, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(384, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(76, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From,To Doc";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(79, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From,To Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(564, 16);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(463, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(232, 16);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(148, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(83, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(148, 22);
		this.FromDateHistory.TabIndex = 0;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label69);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 30);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1170, 28);
		this.panel16.TabIndex = 54;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(2, 4);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(216, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Service Bill Direct History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.toolStripSeparator2 });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip3.TabIndex = 53;
		this.toolStrip3.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 654);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailadd);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.btn);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1176, 628);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 331);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1170, 21);
		this.panel1.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1128, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grddetailadd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 39;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox3);
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Controls.Add(this.label25);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1170, 301);
		this.panel7.TabIndex = 0;
		this.groupBox3.Controls.Add(this.panel34);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterItem);
		this.groupBox3.Controls.Add(this.RadItemCodePackListDetail);
		this.groupBox3.Controls.Add(this.RadItemNamePackListDetail);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.label32);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.txtdescription);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAc);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add(this.txtQtyDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail);
		this.groupBox3.Controls.Add(this.txtChargesRateFcyDetail);
		this.groupBox3.Controls.Add(this.lbldebitac);
		this.groupBox3.Controls.Add(this.txtTotalLcyAmountDetail);
		this.groupBox3.Controls.Add(this.txtLcyAmountDetail);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUomDetail);
		this.groupBox3.Controls.Add(this.txtOtherChargesFcyDetail);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtFcyAmountDetail);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.txtExchangeRateDetail);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(6, 188);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1157, 111);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail Information";
		this.panel34.Controls.Add(this.RadCategory);
		this.panel34.Controls.Add(this.RadMasterItem);
		this.panel34.Location = new System.Drawing.Point(0, 12);
		this.panel34.Name = "panel34";
		this.panel34.Size = new System.Drawing.Size(162, 22);
		this.panel34.TabIndex = 628;
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
		this.CmbMasterItem.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Location = new System.Drawing.Point(0, 37);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Name = "CmbMasterItem";
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Leave += new System.EventHandler(CmbMasterItem_Leave);
		this.RadItemCodePackListDetail.AutoSize = true;
		this.RadItemCodePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodePackListDetail.Location = new System.Drawing.Point(248, 14);
		this.RadItemCodePackListDetail.Name = "RadItemCodePackListDetail";
		this.RadItemCodePackListDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodePackListDetail.TabIndex = 625;
		this.RadItemCodePackListDetail.Text = "Code";
		this.RadItemCodePackListDetail.UseVisualStyleBackColor = true;
		this.RadItemNamePackListDetail.AutoSize = true;
		this.RadItemNamePackListDetail.Checked = true;
		this.RadItemNamePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNamePackListDetail.Location = new System.Drawing.Point(192, 14);
		this.RadItemNamePackListDetail.Name = "RadItemNamePackListDetail";
		this.RadItemNamePackListDetail.Size = new System.Drawing.Size(57, 19);
		this.RadItemNamePackListDetail.TabIndex = 624;
		this.RadItemNamePackListDetail.TabStop = true;
		this.RadItemNamePackListDetail.Text = "Name";
		this.RadItemNamePackListDetail.UseVisualStyleBackColor = true;
		this.RadItemNamePackListDetail.CheckedChanged += new System.EventHandler(RadItemNamePackListDetail_CheckedChanged);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(1014, 79);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(44, 26);
		this.btnplus.TabIndex = 13;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
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
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(162, 37);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(cmbitem_Leave);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(162, 16);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(67, 15);
		this.label32.TabIndex = 108;
		this.label32.Text = "Item Name";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1076, 79);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(57, 26);
		this.btnCancelUpdateDetial.TabIndex = 15;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnCancelUpdateDetial_Leave);
		this.txtdescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdescription.Location = new System.Drawing.Point(3, 81);
		this.txtdescription.Name = "txtdescription";
		this.txtdescription.Size = new System.Drawing.Size(1010, 23);
		this.txtdescription.TabIndex = 11;
		this.CmbDebitAc.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Location = new System.Drawing.Point(720, 79);
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Name = "CmbDebitAc";
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Size = new System.Drawing.Size(292, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Leave += new System.EventHandler(cmbitem_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(344, 16);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(29, 15);
		this.label31.TabIndex = 109;
		this.label31.Text = "QTY";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(599, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(55, 15);
		this.label1.TabIndex = 116;
		this.label1.Text = "Fcy Code";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1014, 79);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(61, 26);
		this.btnUpdateDetail.TabIndex = 14;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtQtyDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyDetail.Location = new System.Drawing.Point(344, 39);
		this.txtQtyDetail.Name = "txtQtyDetail";
		this.txtQtyDetail.Size = new System.Drawing.Size(80, 23);
		this.txtQtyDetail.TabIndex = 2;
		this.txtQtyDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyDetail.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtQtyDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.CmbFcyCodeDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCodeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCodeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCodeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance3).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFcyCodeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).Location = new System.Drawing.Point(598, 37);
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).Name = "CmbFcyCodeDetail";
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).TabIndex = 5;
		this.txtChargesRateFcyDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChargesRateFcyDetail.Location = new System.Drawing.Point(516, 39);
		this.txtChargesRateFcyDetail.Name = "txtChargesRateFcyDetail";
		this.txtChargesRateFcyDetail.Size = new System.Drawing.Size(80, 23);
		this.txtChargesRateFcyDetail.TabIndex = 4;
		this.txtChargesRateFcyDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtChargesRateFcyDetail.TextChanged += new System.EventHandler(txtchargesrate_TextChanged);
		this.txtChargesRateFcyDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtchargesrate_KeyPress);
		this.lbldebitac.AutoSize = true;
		this.lbldebitac.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbldebitac.Location = new System.Drawing.Point(720, 63);
		this.lbldebitac.Name = "lbldebitac";
		this.lbldebitac.Size = new System.Drawing.Size(53, 15);
		this.lbldebitac.TabIndex = 108;
		this.lbldebitac.Text = "Debit Ac";
		this.txtTotalLcyAmountDetail.Enabled = false;
		this.txtTotalLcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalLcyAmountDetail.Location = new System.Drawing.Point(1044, 39);
		this.txtTotalLcyAmountDetail.Name = "txtTotalLcyAmountDetail";
		this.txtTotalLcyAmountDetail.ReadOnly = true;
		this.txtTotalLcyAmountDetail.Size = new System.Drawing.Size(110, 23);
		this.txtTotalLcyAmountDetail.TabIndex = 10;
		this.txtTotalLcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLcyAmountDetail.Enabled = false;
		this.txtLcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLcyAmountDetail.Location = new System.Drawing.Point(852, 39);
		this.txtLcyAmountDetail.Name = "txtLcyAmountDetail";
		this.txtLcyAmountDetail.ReadOnly = true;
		this.txtLcyAmountDetail.Size = new System.Drawing.Size(110, 23);
		this.txtLcyAmountDetail.TabIndex = 8;
		this.txtLcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(516, 8);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(75, 30);
		this.label19.TabIndex = 113;
		this.label19.Text = "Charges Rate\r\n(Fcy)";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(3, 63);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(68, 15);
		this.label30.TabIndex = 110;
		this.label30.Text = "Description";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(428, 16);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(35, 15);
		this.label20.TabIndex = 112;
		this.label20.Text = "UOM";
		this.CmbUomDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbUomDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance129).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUomDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).Location = new System.Drawing.Point(425, 37);
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).Name = "CmbUomDetail";
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).TextChanged += new System.EventHandler(cmbuom_TextChanged);
		this.txtOtherChargesFcyDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherChargesFcyDetail.Location = new System.Drawing.Point(963, 39);
		this.txtOtherChargesFcyDetail.Name = "txtOtherChargesFcyDetail";
		this.txtOtherChargesFcyDetail.Size = new System.Drawing.Size(80, 23);
		this.txtOtherChargesFcyDetail.TabIndex = 9;
		this.txtOtherChargesFcyDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherChargesFcyDetail.TextChanged += new System.EventHandler(txtOtherChargesFcyDetail_TextChanged);
		this.txtOtherChargesFcyDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtexrate_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(852, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 15);
		this.label2.TabIndex = 117;
		this.label2.Text = "Lcy Amount";
		this.txtFcyAmountDetail.Enabled = false;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(680, 39);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.ReadOnly = true;
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(100, 23);
		this.txtFcyAmountDetail.TabIndex = 6;
		this.txtFcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(1044, 16);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(101, 15);
		this.label17.TabIndex = 117;
		this.label17.Text = "Lcy Total Amount";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(963, 8);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(85, 30);
		this.label13.TabIndex = 111;
		this.label13.Text = "Other Charges \r\n(Lcy)";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(782, 16);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(45, 15);
		this.label29.TabIndex = 111;
		this.label29.Text = "Ex Rate";
		this.txtExchangeRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRateDetail.Location = new System.Drawing.Point(781, 39);
		this.txtExchangeRateDetail.Name = "txtExchangeRateDetail";
		this.txtExchangeRateDetail.Size = new System.Drawing.Size(70, 23);
		this.txtExchangeRateDetail.TabIndex = 7;
		this.txtExchangeRateDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRateDetail.TextChanged += new System.EventHandler(txtexrate_TextChanged);
		this.txtExchangeRateDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtexrate_KeyPress);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(681, 16);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 111;
		this.label11.Text = "FcyAmount";
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add(this.txtduedate);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtrefbillno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.Forwarder);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 28);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(298, 159);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(186, 17);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(108, 22);
		this.txtdocdate.TabIndex = 1;
		this.txtdocdate.ValueChanged += new System.EventHandler(txtdocdate_ValueChanged);
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(112, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(73, 23);
		this.txtdocno.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 37;
		this.label5.Text = "Doc No && Date";
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtduedays.Location = new System.Drawing.Point(112, 45);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(73, 23);
		this.txtduedays.TabIndex = 2;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.txtduedate.CustomFormat = "dd-MMM-yy";
		this.txtduedate.Enabled = false;
		this.txtduedate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtduedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtduedate.Location = new System.Drawing.Point(186, 45);
		this.txtduedate.Name = "txtduedate";
		this.txtduedate.Size = new System.Drawing.Size(108, 22);
		this.txtduedate.TabIndex = 3;
		this.txtduedate.TabStop = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 49);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(98, 15);
		this.label27.TabIndex = 94;
		this.label27.Text = "Due Days && Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 77);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(97, 15);
		this.label3.TabIndex = 78;
		this.label3.Text = "Reference Bill No";
		this.txtrefbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrefbillno.Location = new System.Drawing.Point(112, 73);
		this.txtrefbillno.Name = "txtrefbillno";
		this.txtrefbillno.Size = new System.Drawing.Size(182, 23);
		this.txtrefbillno.TabIndex = 4;
		this.CmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(112, 127);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(182, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 6;
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(112, 99);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(182, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Leave += new System.EventHandler(cmbeximinvoiceno_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 105);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(65, 15);
		this.label8.TabIndex = 43;
		this.label8.Text = "Invoice No";
		this.Forwarder.AutoSize = true;
		this.Forwarder.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Forwarder.Location = new System.Drawing.Point(5, 133);
		this.Forwarder.Name = "Forwarder";
		this.Forwarder.Size = new System.Drawing.Size(74, 15);
		this.Forwarder.TabIndex = 80;
		this.Forwarder.Text = "Bill To /Party";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.chkVoucher);
		this.panel8.Controls.Add(this.ChkPreview);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1170, 27);
		this.panel8.TabIndex = 574;
		this.chkVoucher.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkVoucher.AutoSize = true;
		this.chkVoucher.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkVoucher.ForeColor = System.Drawing.Color.White;
		this.chkVoucher.Location = new System.Drawing.Point(1060, 3);
		this.chkVoucher.Name = "chkVoucher";
		this.chkVoucher.Size = new System.Drawing.Size(106, 20);
		this.chkVoucher.TabIndex = 575;
		this.chkVoucher.Text = "Preview 118";
		this.chkVoucher.UseVisualStyleBackColor = true;
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(950, 3);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(106, 20);
		this.ChkPreview.TabIndex = 576;
		this.ChkPreview.Text = "Preview 540";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(6, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(153, 19);
		this.label7.TabIndex = 574;
		this.label7.Text = "Service Bill Direct";
		this.groupBox1.Controls.Add(this.txtNoOfContainerInvoiceData);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPortInvoiceData);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingPortInvoiceData);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveredAtInvoiceData);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPortInvoiceData);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPortInvoiceData);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtVesselNameInvoiceData);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.txtBlNoInvoiceData);
		this.groupBox1.Controls.Add(this.txtMtonInvoiceData);
		this.groupBox1.Controls.Add(this.txtForwarderNameInvoiceData);
		this.groupBox1.Controls.Add(this.txtBookingRateInvoiceData);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractNoInvoiceData);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtCustomerNameInvoiceData);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtIncoTermInvoiceData);
		this.groupBox1.Controls.Add(this.txtGdNoInvoiceData);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(304, 28);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(859, 159);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Invoice Information";
		this.txtNoOfContainerInvoiceData.Enabled = false;
		this.txtNoOfContainerInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainerInvoiceData.Location = new System.Drawing.Point(708, 73);
		this.txtNoOfContainerInvoiceData.Name = "txtNoOfContainerInvoiceData";
		this.txtNoOfContainerInvoiceData.Size = new System.Drawing.Size(147, 23);
		this.txtNoOfContainerInvoiceData.TabIndex = 12;
		this.txtNoOfContainerInvoiceData.TabStop = false;
		this.txtNoOfContainerInvoiceData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbDestinationPortInvoiceData.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPortInvoiceData.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPortInvoiceData.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPortInvoiceData.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPortInvoiceData).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortInvoiceData).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortInvoiceData).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPortInvoiceData.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortInvoiceData).Location = new System.Drawing.Point(428, 43);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortInvoiceData).Name = "CmbDestinationPortInvoiceData";
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortInvoiceData).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortInvoiceData).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortInvoiceData).TabStop = false;
		this.CmbLoadingPortInvoiceData.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingPortInvoiceData.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingPortInvoiceData.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingPortInvoiceData.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingPortInvoiceData).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPortInvoiceData).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPortInvoiceData).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingPortInvoiceData.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPortInvoiceData).Location = new System.Drawing.Point(428, 15);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPortInvoiceData).Name = "CmbLoadingPortInvoiceData";
		((System.Windows.Forms.Control)(object)this.CmbLoadingPortInvoiceData).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPortInvoiceData).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPortInvoiceData).TabStop = false;
		this.CmbDeliveredAtInvoiceData.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveredAtInvoiceData.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveredAtInvoiceData.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveredAtInvoiceData.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance117).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveredAtInvoiceData).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveredAtInvoiceData).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbDeliveredAtInvoiceData).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveredAtInvoiceData.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveredAtInvoiceData).Location = new System.Drawing.Point(428, 127);
		((System.Windows.Forms.Control)(object)this.CmbDeliveredAtInvoiceData).Name = "CmbDeliveredAtInvoiceData";
		((System.Windows.Forms.Control)(object)this.CmbDeliveredAtInvoiceData).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveredAtInvoiceData).TabIndex = 9;
		this.CmbContainerDispatchedAtPortInvoiceData.AllowNull = (DefaultableBoolean)1;
		this.CmbContainerDispatchedAtPortInvoiceData.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContainerDispatchedAtPortInvoiceData.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContainerDispatchedAtPortInvoiceData.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContainerDispatchedAtPortInvoiceData).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPortInvoiceData).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPortInvoiceData).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContainerDispatchedAtPortInvoiceData.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPortInvoiceData).Location = new System.Drawing.Point(428, 99);
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPortInvoiceData).Name = "CmbContainerDispatchedAtPortInvoiceData";
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPortInvoiceData).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPortInvoiceData).TabIndex = 8;
		this.CmbContainerReceivedFromPortInvoiceData.AllowNull = (DefaultableBoolean)1;
		this.CmbContainerReceivedFromPortInvoiceData.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContainerReceivedFromPortInvoiceData.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContainerReceivedFromPortInvoiceData.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContainerReceivedFromPortInvoiceData).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPortInvoiceData).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPortInvoiceData).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContainerReceivedFromPortInvoiceData.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPortInvoiceData).Location = new System.Drawing.Point(428, 71);
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPortInvoiceData).Name = "CmbContainerReceivedFromPortInvoiceData";
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPortInvoiceData).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPortInvoiceData).TabIndex = 7;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(611, 21);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(74, 15);
		this.label48.TabIndex = 581;
		this.label48.Text = "Vessel Name";
		this.txtVesselNameInvoiceData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVesselNameInvoiceData.Enabled = false;
		this.txtVesselNameInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVesselNameInvoiceData.Location = new System.Drawing.Point(708, 17);
		this.txtVesselNameInvoiceData.Margin = new System.Windows.Forms.Padding(4);
		this.txtVesselNameInvoiceData.Name = "txtVesselNameInvoiceData";
		this.txtVesselNameInvoiceData.Size = new System.Drawing.Size(147, 23);
		this.txtVesselNameInvoiceData.TabIndex = 10;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(290, 133);
		this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(72, 15);
		this.label33.TabIndex = 575;
		this.label33.Text = "Delivered At";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(290, 105);
		this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(136, 15);
		this.label18.TabIndex = 576;
		this.label18.Text = "Container Dispatched At";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(290, 77);
		this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(139, 15);
		this.label21.TabIndex = 577;
		this.label21.Text = "Container Received From";
		this.txtBlNoInvoiceData.Enabled = false;
		this.txtBlNoInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBlNoInvoiceData.Location = new System.Drawing.Point(103, 101);
		this.txtBlNoInvoiceData.Name = "txtBlNoInvoiceData";
		this.txtBlNoInvoiceData.Size = new System.Drawing.Size(184, 23);
		this.txtBlNoInvoiceData.TabIndex = 3;
		this.txtBlNoInvoiceData.TabStop = false;
		this.txtBlNoInvoiceData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMtonInvoiceData.Enabled = false;
		this.txtMtonInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMtonInvoiceData.Location = new System.Drawing.Point(708, 101);
		this.txtMtonInvoiceData.Name = "txtMtonInvoiceData";
		this.txtMtonInvoiceData.Size = new System.Drawing.Size(147, 23);
		this.txtMtonInvoiceData.TabIndex = 13;
		this.txtMtonInvoiceData.TabStop = false;
		this.txtMtonInvoiceData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtForwarderNameInvoiceData.Enabled = false;
		this.txtForwarderNameInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtForwarderNameInvoiceData.Location = new System.Drawing.Point(103, 45);
		this.txtForwarderNameInvoiceData.Name = "txtForwarderNameInvoiceData";
		this.txtForwarderNameInvoiceData.Size = new System.Drawing.Size(184, 23);
		this.txtForwarderNameInvoiceData.TabIndex = 1;
		this.txtBookingRateInvoiceData.Enabled = false;
		this.txtBookingRateInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBookingRateInvoiceData.Location = new System.Drawing.Point(708, 129);
		this.txtBookingRateInvoiceData.Name = "txtBookingRateInvoiceData";
		this.txtBookingRateInvoiceData.Size = new System.Drawing.Size(147, 23);
		this.txtBookingRateInvoiceData.TabIndex = 14;
		this.txtBookingRateInvoiceData.TabStop = false;
		this.txtBookingRateInvoiceData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(611, 77);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(98, 15);
		this.label24.TabIndex = 57;
		this.label24.Text = "No Of Containers";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(611, 105);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(41, 15);
		this.label16.TabIndex = 57;
		this.label16.Text = "M.Ton";
		this.CmbContractNoInvoiceData.AllowNull = (DefaultableBoolean)1;
		this.CmbContractNoInvoiceData.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractNoInvoiceData.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractNoInvoiceData.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractNoInvoiceData).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractNoInvoiceData).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbContractNoInvoiceData).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractNoInvoiceData.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNoInvoiceData).Location = new System.Drawing.Point(103, 71);
		((System.Windows.Forms.Control)(object)this.CmbContractNoInvoiceData).Name = "CmbContractNoInvoiceData";
		((System.Windows.Forms.Control)(object)this.CmbContractNoInvoiceData).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractNoInvoiceData).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbContractNoInvoiceData).TabStop = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(611, 133);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(56, 15);
		this.label4.TabIndex = 57;
		this.label4.Text = "CRO Rate";
		this.txtCustomerNameInvoiceData.Enabled = false;
		this.txtCustomerNameInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerNameInvoiceData.Location = new System.Drawing.Point(103, 17);
		this.txtCustomerNameInvoiceData.Name = "txtCustomerNameInvoiceData";
		this.txtCustomerNameInvoiceData.Size = new System.Drawing.Size(184, 23);
		this.txtCustomerNameInvoiceData.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(611, 49);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(61, 15);
		this.label23.TabIndex = 55;
		this.label23.Text = "Inco Term";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(5, 133);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(43, 15);
		this.label22.TabIndex = 55;
		this.label22.Text = "GD No";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 105);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(39, 15);
		this.label15.TabIndex = 55;
		this.label15.Text = "BL No";
		this.txtIncoTermInvoiceData.Enabled = false;
		this.txtIncoTermInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIncoTermInvoiceData.Location = new System.Drawing.Point(708, 45);
		this.txtIncoTermInvoiceData.Name = "txtIncoTermInvoiceData";
		this.txtIncoTermInvoiceData.Size = new System.Drawing.Size(147, 23);
		this.txtIncoTermInvoiceData.TabIndex = 11;
		this.txtIncoTermInvoiceData.TabStop = false;
		this.txtIncoTermInvoiceData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGdNoInvoiceData.Enabled = false;
		this.txtGdNoInvoiceData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdNoInvoiceData.Location = new System.Drawing.Point(103, 129);
		this.txtGdNoInvoiceData.Name = "txtGdNoInvoiceData";
		this.txtGdNoInvoiceData.Size = new System.Drawing.Size(184, 23);
		this.txtGdNoInvoiceData.TabIndex = 4;
		this.txtGdNoInvoiceData.TabStop = false;
		this.txtGdNoInvoiceData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 77);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(75, 15);
		this.label9.TabIndex = 45;
		this.label9.Text = "Lc / Contract";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(290, 21);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(81, 15);
		this.label12.TabIndex = 49;
		this.label12.Text = "Loading Place";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 49);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(95, 15);
		this.label10.TabIndex = 78;
		this.label10.Text = "Forwarder Name";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 21);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(93, 15);
		this.label6.TabIndex = 78;
		this.label6.Text = "Customer Name";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(290, 49);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(99, 15);
		this.label14.TabIndex = 53;
		this.label14.Text = "Destination Place";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label25.Location = new System.Drawing.Point(1321, 310);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(30, 17);
		this.label25.TabIndex = 85;
		this.label25.Text = "TAX";
		this.btn.BackColor = System.Drawing.Color.White;
		this.btn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnDelete, this.btnattachment, this.btnVoucher, this.btnprint, this.toolStripSeparator1, this.toolStripButton1,
			this.btnDefineServices, this.btnShortCutKey
		});
		this.btn.Location = new System.Drawing.Point(3, 3);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1170, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucher.Name = "btnVoucher";
		this.btnVoucher.Size = new System.Drawing.Size(115, 24);
		this.btnVoucher.Text = "Voucher-118";
		this.btnVoucher.Click += new System.EventHandler(btnVoucher_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "&Print-540";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(95, 24);
		this.toolStripButton1.Text = "Load Invoice";
		this.toolStripButton1.Visible = false;
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnDefineServices.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefineServices.Image = (System.Drawing.Image)resources.GetObject("btnDefineServices.Image");
		this.btnDefineServices.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineServices.Name = "btnDefineServices";
		this.btnDefineServices.Size = new System.Drawing.Size(102, 24);
		this.btnDefineServices.Text = "Services Item";
		this.btnDefineServices.Click += new System.EventHandler(btnDefineServices_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 654);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "ExImClearingAgentBillDirect";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExImClearingAgentBillDirect_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExImClearingAgentBillDirect_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).EndInit();
		this.tabHistory.ResumeLayout(false);
		this.tabHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCodeDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomDetail).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPortInvoiceData).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPortInvoiceData).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveredAtInvoiceData).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerDispatchedAtPortInvoiceData).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerReceivedFromPortInvoiceData).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNoInvoiceData).EndInit();
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
