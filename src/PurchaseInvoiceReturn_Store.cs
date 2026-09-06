using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
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
namespace Architecture.WinApp.StoreManagement
{

public class PurchaseInvoiceReturn_Store : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlstForFreight = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtPartiesAndItem = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtCondition = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private DataTable dtTaxType = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private bool FormInitialized;

	private bool WagesStatus;

	private bool GdnExist;

	private bool RefDocumentExist;

	private int BaseDocumentTypeId;

	private double SupplierGLCreditAmount;

	private double SupplierGLDebitAmount;

	private double SupplierCommissionAmount;

	private int updateDetailIndex;

	private int VoucherHeadId;

	private int RecId;

	private bool Approved;

	private int BaseCurrency;

	private double BaseRate;

	private bool HasMultiCurrencyFeature;

	private bool FreightDebitToExpenses;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool GetItemagainstwarehouseId;

	private bool TaxPercntEditableOrNot;

	private bool ItemSearchWithNameOrCode;

	private int OutwardFreightAccountId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private frmLoadGDN_Store LoadTransactions;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	private GroupBox groupBox1;

	private TextBox txtAmount;

	private Label label14;

	private Label label42;

	private TextBox txtRate;

	private Label label40;

	private TextBox txtQty;

	private Label label38;

	private Label label35;

	private Label label32;

	private Label label31;

	private Label label28;

	private DateTimePicker txtgpdate;

	internal UltraCombo CmbPackUOM;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbWarehouseName;

	private Label label47;

	private Label label48;

	private TextBox txtvehicleno;

	private Button btnAdd;

	private TextBox txtgatepassno;

	private TextBox txtSupplierGLId;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnItemSlip;

	private CheckBox ChkBok;

	private TextBox txtBillAmount;

	private TextBox txtremarks;

	private Label label24;

	private Label label4;

	private TextBox txtduedays;

	private Label label10;

	private DateTimePicker duedate;

	private Label label29;

	internal UltraCombo CmbSupplierName;

	private Label label7;

	private TextBox txtSupplierReference;

	private Label label6;

	private Label label12;

	private TextBox txtbillno;

	private Label label5;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbPaymentTerm;

	internal UltraCombo combdeliverytrm;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdDetail;

	private CheckBox ChkPrintslip;

	private CheckBox ChkResetOnSave;

	private Label lblBalance;

	private ToolStripButton btnLoadGdn;

	private ToolStripButton btnPartySlip;

	internal UltraCombo CmbCityName;

	private Label label8;

	private Panel panel11;

	private GroupBox groupBox2;

	private Label label17;

	private Panel panel16;

	private Label label23;

	private Label label54;

	private CtrlGrdBar ctrlGrdBar3;

	private TextBox txtInvoiceQty;

	private Label label63;

	internal UltraCombo cmbCurrency;

	private TextBox txtFcyAmount;

	private TextBox txtExchangeRate;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label56;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo CmbTaxType;

	private TextBox txtTaxPercent;

	private Label lblTaxPercent;

	private TextBox txtTaxAmount;

	private Label lblTaxTypes;

	private Label lblTaxAmount;

	private Label lblTotalAmount;

	private TextBox txtTotalAmount;

	private Label label1;

	internal UltraCombo CmbTaxAccount;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private TabPage tabFrieght;

	private TabPage tabOtherItems;

	private TabPage tabPartyAddLess;

	private TextBox txtRemarksDetail;

	private Label label13;

	internal GridEX grd;

	internal GridEX grdInvExp;

	private Panel panel12;

	private Label label37;

	internal GridEX grdGLedger;

	private Panel panel4;

	private Label label2;

	internal GridEX grdFreight;

	private Label label16;

	private Panel panel7;

	private Label label18;

	private Panel panel5;

	private Label label15;

	private Panel panel6;

	private GroupBox groupBox3;

	private Button btnShowHistory;

	private Label label53;

	private Label label3;

	private Label label9;

	private Label label11;

	private Label label57;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnNewHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	internal UltraCombo CmbRackName;

	private Label label19;

	internal UltraCombo CmbItemCondition;

	private Label label20;

	internal UltraCombo CmbJobLot;

	private Label label21;

	private ToolStripButton BtnLoadInvoice;

	private ToolStripButton btnDefineReason;

	public PurchaseInvoiceReturn_Store(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1300, 750);
			FormInitialized = true;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			string ScreenName = Conversion.ToString(base.Tag);
			if (ScreenName == "PurchaseInvoiceReturn_Store")
			{
				BaseDocumentTypeId = 1;
			}
			else if (ScreenName == "PurchaseInvoiceReturnPM")
			{
				BaseDocumentTypeId = 2;
				BtnLoadInvoice.Visible = false;
			}
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnItemSlip.Enabled = formright.DoHavePrintRights;
			btnPartySlip.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnLoadGdn.Visible = false;
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar3, formright);
			GetConfigurationsFromGlobal();
			CurrencyFill();
			MultiCurrencyFeature();
			dtSupplier.Columns.Add("SupplierCustomerId", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("SupplierCustomerId");
			dtAccountlst.Columns.Add("AccountTitle");
			dtAccountlstForFreight.Columns.Add("Id");
			dtAccountlstForFreight.Columns.Add("SupplierCustomerId");
			dtAccountlstForFreight.Columns.Add("AccountTitle");
			GridCombdtFill();
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("InvGdnId", typeof(int));
			dtGrid.Columns.Add("InvGdnDetailId", typeof(int));
			dtGrid.Columns.Add("GdnNo", typeof(int));
			dtGrid.Columns.Add("RefDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("RefDocId", typeof(int));
			dtGrid.Columns.Add("RefDocSubId", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemCode");
			dtGrid.Columns.Add("Item");
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse");
			dtGrid.Columns.Add("RackId", typeof(int));
			dtGrid.Columns.Add("RackName");
			dtGrid.Columns.Add("ItemConditionId", typeof(int));
			dtGrid.Columns.Add("ItemCondition");
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("JobLot");
			dtGrid.Columns.Add("PackUOMId", typeof(int));
			dtGrid.Columns.Add("PackUOM");
			dtGrid.Columns.Add("PackEquivalent", typeof(double));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("FcyAmount", typeof(decimal));
			dtGrid.Columns.Add("TaxTypeId", typeof(int));
			dtGrid.Columns.Add("Tax%", typeof(double));
			dtGrid.Columns.Add("TaxAmount", typeof(double));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("Freight", typeof(double));
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("CityId", typeof(int));
			dtGrid.Columns.Add("CityName");
			dtGrid.Columns.Add("Remarks");
			dtGrid.Columns.Add("ReasonId", typeof(int));
			dtGrid.Columns.Add("BalQty", typeof(double));
			BindGrid();
			dtInvExp.Columns.Add("Id", typeof(int));
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtGrdGL.Columns.Add("AccountId", typeof(int));
			dtGrdGL.Columns.Add("GlAccountId", typeof(int));
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(double));
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtFreight.Columns.Add("Id", typeof(int));
			dtFreight.Columns.Add("GdnId", typeof(int));
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("GlAccountId", typeof(int));
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			dtFreight.Columns.Add("Remarks");
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			DocumentNo();
			GetPartiesAndItem();
			suppliercustomer();
			PaymentTermBindFromGlobal();
			DeliveryTerm();
			BindTaxAccountFromGlobal();
			GetallTaxType();
			itemdtFillByParty();
			ItemNameBind();
			ItemConditionBindFromGlobal();
			JoblotBindFromGlobal();
			CityFillFromGlobal();
			GetConfigurationsFromGlobalandBind();
			DocDate.Focus();
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
			TaxPercntEditableOrNot = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("TaxPercentEditable"));
			WagesStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryOnSaleInvoiceDirect"));
			GetItemagainstwarehouseId = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("GetItemsagainstWarehouse"));
			OutwardFreightAccountId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FreightOutwardAc"));
			FreightDebitToExpenses = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DebitAmountChargetoExpenseAcFreightGridPurchase"));
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
			int warehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Warehouse"));
			if (warehouseId > 0)
			{
				CmbWarehouseName.Value = warehouseId;
			}
			int cityId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("City Area"));
			if (cityId > 0 && Conversion.ToInt(CmbCityName.Value) == 0)
			{
				CmbCityName.Value = cityId;
			}
			int jobId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (jobId > 0 && Conversion.ToInt(CmbJobLot.Value) == 0)
			{
				CmbJobLot.Value = jobId;
			}
			int baseCurrency = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			if (baseCurrency > 0)
			{
				BaseCurrency = baseCurrency;
				if (Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					cmbCurrency.Value = baseCurrency;
				}
			}
			double baseRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
			if (baseRate > 0.0)
			{
				BaseRate = baseRate;
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
				{
					txtExchangeRate.Text = baseRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int Code = InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 145
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

	private void suppliercustomer()
	{
		try
		{
			int Id = Conversion.ToInt(CmbSupplierName.Value);
			var distinctRows = (from r in dtPartiesAndItem.AsEnumerable()
				select new
				{
					SupplierCustomerId = r.Field<int>("SupplierCustomerId"),
					CompanyName = r.Field<string>("CompanyName"),
					GlAccountId = r.Field<int>("GlAccountId")
				}).Distinct().ToList();
			foreach (var r2 in distinctRows)
			{
				dtSupplier.Rows.Add(r2.SupplierCustomerId, r2.CompanyName, r2.GlAccountId);
			}
			if (!distinctRows.Any())
			{
				dtSupplier = dtPartiesAndItem.Clone();
			}
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierName, dtSupplier, "SupplierCustomerId", "CompanyName", "Party Name", AllColumns: true, Id, insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermBindFromGlobal()
	{
		try
		{
			DataTable dtTerm = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalPaymentTerm, (InvDueTerms x) => x.Id, (InvDueTerms x) => x.TermsDescription);
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTerm, dtTerm, "Id", "Description", "Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTerm.Value));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 145);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPartiesAndItem()
	{
		try
		{
			string DocumentTypeIds = ((Conversion.ToString(base.Tag) == "PurchaseInvoiceReturn_Store") ? "58,61,63,64,131,1604,1603" : ((Conversion.ToString(base.Tag) == "PurchaseInvoiceReturnPM") ? "245,702" : ""));
			dtPartiesAndItem = InvPurchaseInvoice.GetPartiesAndItemFromPurchaseInvoiceStoreOrPM(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeIds);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = CommonServices.GetVendorsAndCustomers(1);
				if (dt.Rows.Count > 0)
				{
					dtAccountlst.Clear();
					dtAccountlstForFreight.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtAccountlst.Rows.Add(Conversion.ToInt(dt.Rows[i]["GlAccountId"]), Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["CompanyName"]));
					}
					dtAccountlstForFreight = dtAccountlst;
				}
				return;
			}
			dtAccountlst.Rows.Clear();
			dtAccountlstForFreight.Rows.Clear();
			DataTable dt2 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
			if (dt2.Rows.Count > 0)
			{
				dtAccountlst.Clear();
				for (int k = 0; k < dt2.Rows.Count; k++)
				{
					dtAccountlst.Rows.Add(Conversion.ToInt(dt2.Rows[k]["Id"]), 0, Conversion.ToString(dt2.Rows[k]["AccountTitle"]));
				}
			}
			if (!FreightDebitToExpenses)
			{
				dtAccountlstForFreight.Clear();
				dtAccountlstForFreight = dtAccountlst;
				return;
			}
			DataTable dtfreight = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,22");
			if (dtfreight.Rows.Count > 0)
			{
				dtAccountlstForFreight.Clear();
				for (int j = 0; j < dtfreight.Rows.Count; j++)
				{
					dtAccountlstForFreight.Rows.Add(Conversion.ToInt(dtfreight.Rows[j]["Id"]), 0, Conversion.ToString(dtfreight.Rows[j]["AccountTitle"]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			itemdtFillByParty();
			ItemNameBind();
			BillAmount();
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtSupplier.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtSupplier.Rows[i]["SupplierCustomerId"]) == Conversion.ToInt(CmbSupplierName.Value))
				{
					txtSupplierGLId.Text = dtSupplier.Rows[i]["GlAccountId"].ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpttrm_Leave(object sender, EventArgs e)
	{
		try
		{
			txtduedays.Enabled = true;
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindTaxAccountFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.GetTaxAccountsFromGlobal();
			InfragisticsHelper.BindAndRetainSelection(CmbTaxAccount, dt, "Id", "AccountTitle", "Tax Account", AllColumns: true, Conversion.ToInt(CmbTaxAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void itemdtFillByParty()
	{
		try
		{
			dtitem.Rows.Clear();
			Conversion.ToInt(CmbItemName.Value);
			var distinctRows = (from r in dtPartiesAndItem.AsEnumerable()
				where r.Field<int>("SupplierCustomerId") == Conversion.ToInt(CmbSupplierName.Value)
				select new
				{
					ItemId = r.Field<int>("ItemId"),
					ItemName = r.Field<string>("ItemName"),
					ItemCodeNew = r.Field<string>("ItemCodeNew")
				}).Distinct().ToList();
			if (distinctRows.Count > 0)
			{
				for (int i = 0; i < distinctRows.Count; i++)
				{
					dtitem.Rows.Add(distinctRows[i].ItemId, distinctRows[i].ItemName, distinctRows[i].ItemCodeNew);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemName.Value);
			string ValueMember = (rdbtnItemName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdbtnItemName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtitem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtitem.Rows.Count > 0)
			{
				CmbItemName_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WareHouseBindFromGlobalRacksByItemId(int ItemId)
	{
		try
		{
			DataTable dtWareHouse = new DataTable();
			if (dtWareHouse.Columns.Count == 0)
			{
				dtWareHouse.Columns.Add("Id", typeof(int));
				dtWareHouse.Columns.Add("WareHouseName", typeof(string));
			}
			dtWareHouse.BeginLoadData();
			HashSet<int> Ids = new HashSet<int>();
			foreach (RacksWithWarehouseAndItem item in clsGlobalVariables.racksWithWarehouseAndItems.Where((RacksWithWarehouseAndItem rr) => rr.ItemId == ItemId))
			{
				if (Ids.Add(item.WarehouseId))
				{
					dtWareHouse.Rows.Add(item.WarehouseId, item.WareHouseName);
				}
			}
			dtWareHouse.EndLoadData();
			InfragisticsHelper.BindAndRetainSelection(CmbWarehouseName, dtWareHouse, "Id", "WareHouseName", "WareHouseName", AllColumns: true, Conversion.ToInt(CmbWarehouseName.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading WareHouse: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void RackBindFromGlobalRacksByItemId(int ItemId, int WareHouseId = 0)
	{
		try
		{
			DataTable dtRack = new DataTable();
			if (dtRack.Columns.Count == 0)
			{
				dtRack.Columns.Add("Id", typeof(int));
				dtRack.Columns.Add("RackName", typeof(string));
			}
			dtRack.BeginLoadData();
			HashSet<int> Ids = new HashSet<int>();
			foreach (RacksWithWarehouseAndItem item in clsGlobalVariables.racksWithWarehouseAndItems.Where((RacksWithWarehouseAndItem rr) => rr.ItemId == ItemId && (WareHouseId == 0 || rr.WarehouseId == WareHouseId)))
			{
				if (Ids.Add(item.Id))
				{
					dtRack.Rows.Add(item.Id, item.RackName);
				}
			}
			dtRack.EndLoadData();
			InfragisticsHelper.BindAndRetainSelection(CmbRackName, dtRack, "Id", "RackName", "RackName", AllColumns: true, Conversion.ToInt(CmbRackName.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading Rack: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemUomFromGlobalBind(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUOM).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			InfragisticsHelper.BindAndRetainSelection(CmbPackUOM, dtUOM, "Id", "UOMCode", "Pack Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemConditionBindFromGlobal()
	{
		try
		{
			List<GlobalItemCondition> lst = clsGlobalVariables.globalItemConditions;
			int Id = Conversion.ToInt(CmbItemCondition.Value);
			dtCondition = DatatableHelper.PopulateDataTableAndReturn(lst, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			InfragisticsHelper.BindAndRetainSelection(CmbItemCondition, dtCondition, "Id", "Description", "Item Condition", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JoblotBindFromGlobal()
	{
		try
		{
			List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
			dtJobLot = DatatableHelper.PopulateDataTableAndReturn(lst, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
			InfragisticsHelper.BindAndRetainSelection(CmbJobLot, dtJobLot, "Id", "Description", "Job Lot", AllColumns: true, Conversion.ToInt(CmbJobLot.Value));
		}
		catch (Exception)
		{
			MessageBox.Show("joblot Not Found");
		}
	}

	private void GridCombdtFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtOtherItem.Rows.Clear();
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
			AccountsFill();
			dtTaxType.Rows.Clear();
			dtTaxType = ItemTaxSchedule.GetAllTaxSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, DocDate.Value, 2);
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
			int Id = Conversion.ToInt(CmbTaxType.Value);
			if (dtTaxType.Rows.Count > 0)
			{
				DDL.BindDDL(dtTaxType, CmbTaxType, "TaxNameId", "TaxName", "Tax Type", ZeroIndex: false);
				((UltraGridBase)CmbTaxType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbTaxType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id > 0)
				{
					if (dtTaxType.AsEnumerable().Any((DataRow row) => row.Field<int>("TaxNameId") == Id))
					{
						CmbTaxType.Value = Id;
					}
					else
					{
						((Control)(object)CmbTaxType).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbTaxType).DataSource = null;
				((Control)(object)CmbTaxType).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFillFromGlobal()
	{
		try
		{
			dtCity = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalAllCities, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			InfragisticsHelper.BindAndRetainSelection(CmbCityName, dtCity, "Id", "Description", "City Name", AllColumns: false, Conversion.ToInt(CmbCityName.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		ItemNameBind();
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(CmbItemName.Value);
			int WareHouseId = Conversion.ToInt(CmbWarehouseName.Value);
			ItemUomFromGlobalBind(ItemId);
			WareHouseBindFromGlobalRacksByItemId(ItemId);
			RackBindFromGlobalRacksByItemId(ItemId, WareHouseId);
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWarehouseName_Leave(object sender, EventArgs e)
	{
		int ItemId = Conversion.ToInt(CmbItemName.Value);
		int WareHouseId = Conversion.ToInt(CmbWarehouseName.Value);
		RackBindFromGlobalRacksByItemId(ItemId, WareHouseId);
		AvailableStockGetByItem();
	}

	private void CmbRackName_Leave(object sender, EventArgs e)
	{
		try
		{
			int RackId = Conversion.ToInt(CmbRackName.Value);
			int WareHouseId = (from r in clsGlobalVariables.racksWithWarehouseAndItems
				where r.Id == RackId
				select r.WarehouseId).FirstOrDefault();
			if (WareHouseId > 0)
			{
				CmbWarehouseName.Value = WareHouseId;
			}
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemCondition_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void CmbJobLot_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		try
		{
			double CurrStock = InventoryStockEvalautionDetail.GetCurrentStockByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(CmbWarehouseName.Value),
				ItemId = Conversion.ToInt(CmbItemName.Value),
				JobLotId = Conversion.ToInt(CmbJobLot.Value),
				StoreRackId = Conversion.ToInt(CmbRackName.Value),
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				stockUOM = Conversion.ToInt(CmbPackUOM.Value)
			});
			if (CurrStock > 0.0)
			{
				lblBalance.Text = CurrStock.ToString();
			}
			else
			{
				lblBalance.Text = "0";
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
				label59.Visible = true;
				label60.Visible = true;
				label61.Visible = true;
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label59.Visible = false;
				label60.Visible = false;
				label61.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
					DocumentTypeIds = "145",
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

	private void BindGrid()
	{
		grd.DataSource = dtGrid;
		grd.RetrieveStructure();
		grdSettings(grd);
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && (GdnExist || RefDocumentExist))
			{
				throw new Exception("You cannot add a manual record because a " + (GdnExist ? "GDN" : "Purchase Invoice") + " record already exists in the grid.");
			}
			if (FormValidationDetila())
			{
				DataRow dr = dtGrid.NewRow();
				FillDetailRow(dr);
				dtGrid.Rows.Add(dr);
				BindGrid();
				ExpProportion();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
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
			if (FormValidationDetila() && updateDetailIndex >= 0 && updateDetailIndex < dtGrid.Rows.Count)
			{
				DataRow dr = dtGrid.Rows[updateDetailIndex];
				FillDetailRow(dr);
				BindGrid();
				ExpProportion();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailRow(DataRow dr)
	{
		dr["Id"] = 0;
		dr["InvGdnId"] = 0;
		dr["InvGdnDetailId"] = 0;
		dr["GdnNo"] = 0;
		dr["ItemId"] = Conversion.ToInt(CmbItemName.Value);
		dr["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
		dr["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
		dr["WarehouseId"] = Conversion.ToInt(CmbWarehouseName.Value);
		dr["Warehouse"] = Conversion.ToString((object)((Control)(object)CmbWarehouseName).Text);
		dr["RackId"] = Conversion.ToInt(CmbRackName.Value);
		dr["RackName"] = Conversion.ToString((object)((Control)(object)CmbRackName).Text);
		dr["ItemConditionId"] = Conversion.ToInt(CmbItemCondition.Value);
		dr["ItemCondition"] = Conversion.ToString((object)((Control)(object)CmbItemCondition).Text);
		dr["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
		dr["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
		dr["PackUOMId"] = Conversion.ToInt(CmbPackUOM.Value);
		dr["PackUOM"] = Conversion.ToString((object)((Control)(object)CmbPackUOM).Text);
		dr["PackEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUOM).SelectedRow.Cells[2].Value);
		double qty = Conversion.ToDouble((object)txtQty.Text);
		double rate = Conversion.ToDouble((object)txtRate.Text);
		double amount = Conversion.ToDouble((object)txtAmount.Text);
		dr["ItemQty"] = qty;
		dr["Rate"] = rate;
		dr["ItemAmount"] = amount;
		dr["FcyAmount"] = amount;
		int taxId = Conversion.ToInt(CmbTaxType.Value);
		double taxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
		double taxAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
		dr["TaxTypeId"] = taxId;
		dr["Tax%"] = taxPercent;
		dr["TaxAmount"] = taxAmount;
		dr["BillAmount"] = 0;
		dr["GpDate"] = Conversion.ToDateTime((object)txtgpdate.Value).ToShortDateString();
		dr["GpNo"] = Conversion.ToInt((object)txtgatepassno.Text);
		dr["VehicleNo"] = Conversion.ToString((object)txtvehicleno.Text);
		dr["CityId"] = Conversion.ToInt(CmbCityName.Value);
		dr["CityName"] = Conversion.ToString((object)((Control)(object)CmbCityName).Text);
		dr["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && !GdnExist && !RefDocumentExist)
			{
				updateDetailIndex = r.RowIndex;
				if (Conversion.ToInt(r.Cells["InvGdnId"].Value) > 0)
				{
					DisableFields();
				}
				CmbItemName.Value = r.Cells["ItemId"].Value.ToString();
				CmbItemName_Leave(null, null);
				CmbWarehouseName.Value = r.Cells["WareHouseId"].Value.ToString();
				CmbRackName.Value = r.Cells["RackId"].Value.ToString();
				CmbItemCondition.Value = r.Cells["ItemConditionId"].Value.ToString();
				CmbJobLot.Value = r.Cells["JobLotId"].Value.ToString();
				CmbPackUOM.Value = r.Cells["PackUOMId"].Value.ToString();
				txtQty.Text = Conversion.ToDouble(r.Cells["ItemQty"].Value).ToString("#,##0.##");
				txtRate.Text = Conversion.ToDouble(r.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtAmount.Text = Conversion.ToDouble(r.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				CmbTaxType.Value = r.Cells["TaxTypeId"].Value.ToString();
				txtTaxPercent.Text = r.Cells["Tax%"].Value.ToString();
				txtTaxAmount.Text = r.Cells["TaxAmount"].Value.ToString();
				txtgpdate.Text = r.Cells["GpDate"].Value.ToString();
				txtgatepassno.Text = r.Cells["GpNo"].Value.ToString();
				txtvehicleno.Text = r.Cells["VehicleNo"].Value.ToString();
				CmbCityName.Value = r.Cells["CityId"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnAdd.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grdSettings(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "InvGdnId", "InvGdnDetailId", "GdnNo", "CityId", "ItemId", "RefDocumentTypeId", "RefDocId", "RefDocSubId", "WarehouseId",
				"RackId", "ItemConditionId", "PackUOMId", "JobLotId", "JobLot", "PackEquivalent"
			};
			Dictionary<string, EditType> editColDirect = new Dictionary<string, EditType>
			{
				{
					"GpDate",
					(EditType)2
				},
				{
					"GpNo",
					(EditType)1
				},
				{
					"VehicleNo",
					(EditType)1
				}
			};
			Dictionary<string, EditType> EditColGdn = new Dictionary<string, EditType>
			{
				{
					"Rate",
					(EditType)1
				},
				{
					"TaxTypeId",
					(EditType)4
				},
				{
					"Tax%",
					(EditType)(TaxPercntEditableOrNot ? 1 : 0)
				}
			};
			Dictionary<string, EditType> EditColRefDocument = new Dictionary<string, EditType>
			{
				{
					"ItemQty",
					(EditType)1
				},
				{
					"TaxTypeId",
					(EditType)4
				},
				{
					"Tax%",
					(EditType)(TaxPercntEditableOrNot ? 1 : 0)
				},
				{
					"GpDate",
					(EditType)2
				},
				{
					"GpNo",
					(EditType)1
				},
				{
					"ReasonId",
					(EditType)4
				},
				{
					"VehicleNo",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, GdnExist ? EditColGdn : (RefDocumentExist ? EditColRefDocument : editColDirect));
			gridEX.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			gridEX.RootTable.Columns["PackUOM"].Caption = "UOM";
			gridEX.RootTable.Columns["ItemQty"].Caption = (RefDocumentExist ? "Return Qty" : "Item Qty");
			gridEX.RootTable.Columns["BalQty"].Visible = RefDocumentExist;
			gridEX.RootTable.Columns["ReasonId"].Visible = RefDocumentExist;
			gridEX.RootTable.Columns["BalQty"].Position = gridEX.RootTable.Columns["ItemQty"].Position + 1;
			gridEX.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			gridEX.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			gridEX.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			gridEX.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			gridEX.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			gridEX.RootTable.Columns["PackUOM"].Width = Constants.InventoryConstants.UomConstant;
			gridEX.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEX.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["TaxTypeId"].Width = Constants.InventoryConstants.DiscountType;
			gridEX.RootTable.Columns["Tax%"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["Journal"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.FreightConstant;
			gridEX.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			gridEX.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
			gridEX.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			gridEX.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			if (((Control)(object)gridEX).Name == "grd")
			{
				if (!GdnExist)
				{
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.FrozenColumns = 1;
				}
				DetailGridComboBind();
				ctrlGrdBar1_Load(null, null);
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
			GridEX_Helper.ComboBind(grd.RootTable.Columns["TaxTypeId"], dtTaxType, "TaxNameId", "TaxName", "Tax Type", Constants.InventoryConstants.CastingType);
			DataTable dt = Reasons.GetByRefDocumentType(145);
			if (dt.Rows.Count > 0)
			{
				GridEX_Helper.ComboBind(grd.RootTable.Columns["ReasonId"], dt, "ReasonId", "Reason", "Return Reason", Constants.InventoryConstants.StringConstant + 25);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_EditingCell(object sender, EditingCellEventArgs e)
	{
	}

	private void grd_RecordsDeleted(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
			FreightProportion();
			ExpProportion();
			BillAmount();
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Delete" && !GdnExist && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				item.Delete();
				((Control)(object)CmbSupplierName).Enabled = grd.RowCount <= 0;
				FreightProportion();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count == 0)
			{
				return;
			}
			string col = e.Column.Key;
			bool num = (GdnExist && (col == "ItemQty" || col == "Rate")) || (RefDocumentExist && col == "ItemQty");
			item.BeginEdit();
			if (num)
			{
				if (RefDocumentExist && col == "ItemQty")
				{
					ValidateQtyAgainstBalance(item);
				}
				item.Cells["ItemAmount"].Value = CalculateItemAmount(item);
			}
			if (GdnExist || RefDocumentExist)
			{
				if (col == "TaxTypeId")
				{
					item.Cells["Tax%"].Value = GetTaxPercentForSelectedType(item);
				}
				else if (col == "Tax%" && Conversion.ToDouble(item.Cells["Tax%"].Value) == 0.0)
				{
					item.Cells["Tax%"].Value = GetTaxPercentForSelectedType(item);
				}
				RecalculateTax(item);
			}
			if (col == "CityId" || col == "CityName")
			{
				item.Cells["CityId"].Value = Conversion.ToInt(item.Cells["CityName"].Value);
			}
			item.EndEdit();
			if (GdnExist || RefDocumentExist)
			{
				ExpProportion();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ValidateQtyAgainstBalance(GridEXRow item)
	{
		double num = Conversion.ToDouble(item.Cells["ItemQty"].Value);
		double balQty = Conversion.ToDouble(item.Cells["BalQty"].Value);
		if (num > balQty)
		{
			MessageBox.Show("Item Quantity can't be greater than Balance Quantity...");
			item.Cells["ItemQty"].Value = balQty;
		}
	}

	private double CalculateItemAmount(GridEXRow item)
	{
		double qty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
		double rate = Conversion.ToDouble(item.Cells["Rate"].Value);
		if (!(qty > 0.0) || !(rate > 0.0))
		{
			return 0.0;
		}
		return qty * rate;
	}

	private double GetTaxPercentForSelectedType(GridEXRow item)
	{
		int selectedTaxId = Conversion.ToInt(item.Cells["TaxTypeId"].Value);
		if (selectedTaxId == 0)
		{
			return 0.0;
		}
		DataRow[] matchingRows = dtTaxType.Select("TaxNameId = " + selectedTaxId);
		if (matchingRows.Length == 0)
		{
			return Conversion.ToDouble(item.Cells["Tax%"].Value);
		}
		return Conversion.ToDouble(matchingRows[0]["TaxPercent"]);
	}

	private void RecalculateTax(GridEXRow item)
	{
		int taxType = Conversion.ToInt(item.Cells["TaxTypeId"].Value);
		double taxPercent = Conversion.ToDouble(item.Cells["Tax%"].Value);
		double itemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
		if (taxPercent > 100.0)
		{
			MessageBox.Show("Tax % Can't be greater than 100...");
			taxPercent = 100.0;
			item.Cells["Tax%"].Value = taxPercent;
		}
		if (itemAmount > 0.0 && taxPercent > 0.0 && taxType > 0)
		{
			double taxAmount = itemAmount * taxPercent / 100.0;
			item.Cells["TaxAmount"].Value = Math.Round(taxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		}
		else
		{
			item.Cells["Tax%"].Value = 0;
			item.Cells["TaxAmount"].Value = 0;
		}
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExp.RootTable.Columns["Id"].Visible = false;
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdInvExp);
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "X";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "X";
			grdInvExp.RootTable.Columns["Delete"].Width = 30;
			grdInvExp.RootTable.Columns["Delete"].Position = 0;
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "+";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "+";
			grdInvExp.RootTable.Columns["Add"].Width = 30;
			grdInvExp.RootTable.Columns["Add"].Position = 1;
			grdInvExp.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtInvExp.Rows[item.RowIndex].Delete();
				item.Delete();
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
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					double Amount = num * Rate;
					item.Cells["Amount"].Value = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				if (e.Column.Key == "Amount")
				{
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
					item.Cells["Amount"].Value = Math.Round(Conversion.ToDouble(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
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
					if (Conversion.ToInt(r.Cells["GlAccountId"].Value) > 0 && Conversion.ToInt((object)txtSupplierGLId.Text) > 0 && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(r.Cells["GlAccountId"].Value))
					{
						MessageBox.Show("Selected Account Can not be Same As Supplier Account");
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = 0;
						r.Cells["AccountId"].Value = 0;
						r.EndEdit();
						return;
					}
				}
				else if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0 && Conversion.ToInt(CmbSupplierName.Value) > 0 && Conversion.ToInt(CmbSupplierName.Value) == Conversion.ToInt(r.Cells["AccountId"].Value))
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

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, 0, "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	private void gridGLSettings()
	{
		try
		{
			grdGLedger.RootTable.Columns["GlAccountId"].Visible = false;
			grdGLedger.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
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
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			CommonServices.GridAutoAdjustmentNew(grdGLedger);
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "X";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "X";
			grdGLedger.RootTable.Columns["Delete"].Width = 30;
			grdGLedger.RootTable.Columns["Delete"].Position = 0;
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "+";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "+";
			grdGLedger.RootTable.Columns["Add"].Width = 30;
			grdGLedger.RootTable.Columns["Add"].Position = 0;
			grdGLedger.FrozenColumns = 2;
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
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
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
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
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
					item.Cells["Debit"].Value = Math.Round(Math.Abs(TotlaPercentage), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
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
				SupCustIdUpdateforGLGrid();
			}
			grdGLedger.UpdateData();
			BillAmount();
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

	private void SupCustIdUpdateforFreightGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				string selectedAcId = Conversion.ToString((object)r.Cells["Transporter"].Text);
				if (selectedAcId != "0" && selectedAcId != string.Empty)
				{
					DataRow[] matchingRows = dtAccountlstForFreight.Select("AccountTitle='" + selectedAcId + "'");
					if (matchingRows.Length != 0)
					{
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
						r.EndEdit();
					}
				}
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0, 0, 0, 0, "");
		grdFreight.DataSource = dtFreight;
	}

	private void grdFreightSettings()
	{
		try
		{
			grdFreight.RootTable.Columns["Id"].Visible = false;
			if (!FreightDebitToExpenses)
			{
				grdFreight.RootTable.Columns["Debit"].Visible = false;
			}
			else
			{
				grdFreight.RootTable.Columns["Debit"].Visible = true;
			}
			grdFreight.RootTable.Columns["GdnId"].Visible = false;
			grdFreight.RootTable.Columns["GlAccountId"].Visible = false;
			grdFreight.RootTable.Columns["GdnId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "Id", "AccountTitle");
			}
			grdFreight.RootTable.Columns["Transporter"].Width = 250;
			grdFreight.RootTable.Columns["Freight"].Width = 50;
			grdFreight.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns.Add("Delete");
			grdFreight.RootTable.Columns["Delete"].Key = "Delete";
			grdFreight.RootTable.Columns["Delete"].Caption = "X";
			grdFreight.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreight.RootTable.Columns["Delete"].Width = 20;
			grdFreight.RootTable.Columns["Delete"].Width = 30;
			grdFreight.RootTable.Columns["Delete"].Position = 0;
			grdFreight.RootTable.Columns.Add("Add");
			grdFreight.RootTable.Columns["Add"].Key = "Add";
			grdFreight.RootTable.Columns["Add"].Caption = "+";
			grdFreight.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Add"].ButtonText = "+";
			grdFreight.RootTable.Columns["Add"].Width = 20;
			grdFreight.RootTable.Columns["Add"].Position = 0;
			grdFreight.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item2 = grdFreight.CurrentRow;
				dtFreight.Rows[item2.RowIndex].Delete();
				if (item2 != null && (int)item2.RowType == 0)
				{
					item2.Delete();
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				GridEXRow item = grdFreight.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					dtFreight.Rows.Add(0, item.Cells["Transporter"].Value, 0, item.Cells["Remarks"].Value);
					grdFreight.DataSource = dtFreight;
				}
			}
			grdFreight.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if (e.Column.Key == "Transporter")
			{
				SupCustIdUpdateforFreightGrid();
			}
			if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
			{
				double Freight = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Freight"].Value = Freight;
			}
			if (FreightDebitToExpenses)
			{
				if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
				{
					item.Cells["Freight"].Value = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
					{
						item.Cells["Freight"].Value = 0;
						MessageBox.Show("Debit Side is aleady added");
					}
				}
				if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
				{
					item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Freight"].Value.ToString()) > 0.0)
					{
						item.Cells["Debit"].Value = 0;
						MessageBox.Show("Credit Side is aleady added");
					}
				}
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

	private void grdFreight_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Freight" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbSupplierName).ActiveRow == null || Conversion.ToInt(CmbSupplierName.Value) == 0)
		{
			MessageBox.Show("CustomerName Field is Required");
			CmbSupplierName.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerm).ActiveRow == null || Conversion.ToInt(CmbPaymentTerm.Value) == 0)
		{
			MessageBox.Show("Payment Term is Required");
			CmbPaymentTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && Conversion.ToInt((object)txtduedays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null || Conversion.ToInt(combdeliverytrm.Value) == 0)
		{
			MessageBox.Show("Delivery Term is Required");
			combdeliverytrm.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetila()
	{
		if (((UltraGridBase)CmbWarehouseName).ActiveRow == null || Conversion.ToInt(CmbWarehouseName.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			CmbWarehouseName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItemName.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == "" || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUOM).ActiveRow == null || Conversion.ToInt(CmbPackUOM.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			CmbPackUOM.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0")
		{
			MessageBox.Show("Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (txtAmount.Text == string.Empty || txtAmount.Text == "0")
		{
			MessageBox.Show("Amount Field is Required");
			txtAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCityName).ActiveRow == null || Conversion.ToInt(CmbCityName.Value) == 0)
		{
			MessageBox.Show("city Field is Required");
			CmbCityName.Focus();
			return false;
		}
		return true;
	}

	private void DisableFields()
	{
		try
		{
			((Control)(object)CmbWarehouseName).Enabled = false;
			((Control)(object)CmbItemName).Enabled = false;
			((Control)(object)CmbPackUOM).Enabled = false;
			((Control)(object)CmbJobLot).Enabled = false;
			txtQty.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnablesFields()
	{
		try
		{
			((Control)(object)CmbWarehouseName).Enabled = true;
			((Control)(object)CmbItemName).Enabled = true;
			((Control)(object)CmbPackUOM).Enabled = true;
			((Control)(object)CmbJobLot).Enabled = true;
			txtQty.Enabled = true;
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
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			EnablesFields();
			GdnExist = false;
			RefDocumentExist = false;
			RecId = 0;
			VoucherHeadId = 0;
			Approved = false;
			AT = new Attachment();
			((Control)(object)CmbSupplierName).Text = string.Empty;
			((Control)(object)CmbSupplierName).Enabled = true;
			txtSupplierReference.Text = string.Empty;
			txtbillno.Text = string.Empty;
			txtInvoiceQty.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtduedays.Text = "0";
			((Control)(object)CmbTaxAccount).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			DocDate.Focus();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtGrid.Rows.Clear();
			dtFreight.Rows.Clear();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			DocumentNo();
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtBillAmount.Text = string.Empty;
			txtduedays.Enabled = true;
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
			GetConfigurationsFromGlobalandBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			((Control)(object)CmbPackUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			((Control)(object)CmbSupplierName).Enabled = grd.RowCount <= 0;
			CmbItemName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OptionResetFields()
	{
		((Control)(object)CmbItemName).Text = string.Empty;
		((Control)(object)CmbJobLot).Text = string.Empty;
		((Control)(object)CmbWarehouseName).Text = string.Empty;
		txtgatepassno.Text = string.Empty;
		txtvehicleno.Text = string.Empty;
		((Control)(object)CmbCityName).Text = string.Empty;
		CmbItemName.Focus();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "JobLot", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GridCombdtFill();
			GetPartiesAndItem();
			suppliercustomer();
			PaymentTermBindFromGlobal();
			DeliveryTerm();
			BindTaxAccountFromGlobal();
			GetallTaxType();
			CurrencyFill();
			itemdtFillByParty();
			ItemNameBind();
			ItemConditionBindFromGlobal();
			JoblotBindFromGlobal();
			CityFillFromGlobal();
			GetConfigurationsFromGlobalandBind();
			DetailGridComboBind();
			GridComboBind();
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

	private void InvfrmInvSaleInvoiceDirect_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnShowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name != null && base.ActiveControl.Name == "")
			{
				FromDateHistory.Focus();
			}
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
			if (Approved)
			{
				throw new Exception("Record Not Update beacause Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvSaleInvoiceDetail pd, GridEXRow r)
	{
		double qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
		pd.InvGdnId = Conversion.ToInt(r.Cells["InvGdnId"].Value);
		pd.InvGdnDetailId = Conversion.ToInt(r.Cells["InvGdnDetailId"].Value);
		pd.GdnNo = Conversion.ToInt(r.Cells["GdnNo"].Value);
		pd.RefRefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
		pd.RefRefDocIdNo = Conversion.ToInt(r.Cells["RefDocId"].Value);
		pd.RefDocSubId = Conversion.ToInt(r.Cells["RefDocSubId"].Value);
		pd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		pd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		pd.ItemName = Conversion.ToString(r.Cells["Item"].Value);
		pd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		pd.WareHouseName = Conversion.ToString(r.Cells["Warehouse"].Value);
		pd.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
		pd.rackName = Conversion.ToString(r.Cells["RackName"].Value);
		pd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		pd.ItemCondition = Conversion.ToString(r.Cells["ItemCondition"].Value);
		pd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		pd.JobLotDescription = Conversion.ToString(r.Cells["JobLot"].Value);
		pd.ItemUOMId = Conversion.ToInt(r.Cells["PackUOMId"].Value);
		pd.UOMCodeItem = Conversion.ToString(r.Cells["PackUOM"].Value);
		pd.ItemQty = qty;
		pd.GrossWeight = qty;
		pd.NetBillWeight = qty;
		pd.NetStockWeight = qty;
		pd.ItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
		pd.ItemRateWOExp = Conversion.ToDecimal(r.Cells["Rate"].Value);
		DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
		if (dt.Rows.Count > 0)
		{
			DataRow[] dr = dt.Select("Equivalent='" + 1 + "'");
			if (dr.Length == 0)
			{
				MessageBox.Show("RateUom (1) of the Item " + Conversion.ToString((object)r.Cells["ItemId"].Text) + " in row No " + (r.RowIndex + 1) + " is not define please check ");
				return;
			}
			pd.UomScheduleIdRate = Conversion.ToInt(dr[0].ItemArray[0]);
		}
		pd.ItemAmount = Math.Round(Conversion.ToDouble(r.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		pd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
		pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
		pd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
		pd.TaxNameId = Conversion.ToInt(r.Cells["TaxTypeId"].Value);
		pd.TaxDescriptions = Conversion.ToString((object)r.Cells["TaxTypeId"].Text);
		pd.TaxPercent = Conversion.ToDouble(r.Cells["Tax%"].Value);
		pd.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
		pd.BillAmount = Conversion.ToDouble(r.Cells["BillAmount"].Value);
		pd.ExpenseAmount = Conversion.ToDouble(r.Cells["Expense"].Value);
		pd.JournalAmount = Conversion.ToDouble(r.Cells["Journal"].Value);
		pd.FreightAmount = Conversion.ToDouble(r.Cells["Freight"].Value);
		pd.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
		pd.GpNo = Conversion.ToInt(r.Cells["GpNo"].Value);
		pd.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
		pd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
		pd.CityName = Conversion.ToString(r.Cells["CityName"].Value);
		pd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
		pd.ReasonId = Conversion.ToInt(r.Cells["ReasonId"].Value);
	}

	private void Insert()
	{
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Expected O, but got Unknown
		//IL_0891: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Expected O, but got Unknown
		//IL_0ba7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Expected O, but got Unknown
		//IL_0cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Expected O, but got Unknown
		//IL_0e5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbSupplierName, "Customer", FormHelper.ValidType.INT),
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbPaymentTerm, "Payment Term", FormHelper.ValidType.INT),
				((Control)(object)combdeliverytrm, "Delivery Term", FormHelper.ValidType.INT)
			}))
			{
				return;
			}
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && Conversion.ToInt((object)txtduedays.Text) == 0)
			{
				MessageBox.Show("Due Days Field is Required");
				txtduedays.Focus();
				return;
			}
			if (HasMultiCurrencyFeature)
			{
				if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					MessageBox.Show("Fcy Code Field is Required");
					cmbCurrency.Focus();
					return;
				}
				if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
				{
					MessageBox.Show("Exchange Rate Field is Required");
					txtExchangeRate.Focus();
					return;
				}
				if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0")
				{
					MessageBox.Show("Fcy Amount Rate Field is Required");
					txtFcyAmount.Focus();
					return;
				}
			}
			else
			{
				if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					MessageBox.Show("Please Configure Your Base Currency In configurations");
					cmbCurrency.Focus();
					return;
				}
				if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
				{
					MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
					txtExchangeRate.Focus();
					return;
				}
			}
			if (dtGrid.AsEnumerable().Any((DataRow x) => Conversion.ToInt(x["TaxTypeId"]) > 0) && !FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)> { ((Control)(object)CmbTaxAccount, "Tax Account", FormHelper.ValidType.INT) }))
			{
				return;
			}
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if (!(Conversion.ToDouble(r7.Cells["Credit"].Value) > 0.0) && Conversion.ToInt(r7.Cells["Debit"].Value) <= 0)
				{
					continue;
				}
				int num = Conversion.ToInt(r7.Cells["AccountId"].Value);
				int GlAccountId = Conversion.ToInt(r7.Cells["GlAccountId"].Value);
				if (num == 0 && GlAccountId == 0)
				{
					MessageBox.Show("Please Select an Account Against JL First");
					return;
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt(r7.Cells["GlAccountId"].Value) > 0 && Conversion.ToInt((object)txtSupplierGLId.Text) > 0 && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(r7.Cells["GlAccountId"].Value))
					{
						MessageBox.Show($"you cannot Select Account Same As Supplier Account in GL Grid Row no : {r7.RowIndex + 1}");
						return;
					}
				}
				else if (Conversion.ToInt(r7.Cells["AccountId"].Value) > 0 && Conversion.ToInt(CmbSupplierName.Value) > 0 && Conversion.ToInt(CmbSupplierName.Value) == Conversion.ToInt(r7.Cells["AccountId"].Value))
				{
					MessageBox.Show($"you cannot Select Account Same As Supplier Account in GL Grid Row no : {r7.RowIndex + 1}");
					return;
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Freight"].Value) > 0.0 || Conversion.ToInt(r6.Cells["Debit"].Value) > 0)
				{
					int num2 = Conversion.ToInt(r6.Cells["GlAccountId"].Value);
					int Transporter = Conversion.ToInt(r6.Cells["Transporter"].Value);
					if (num2 == 0 && Transporter == 0)
					{
						MessageBox.Show("Please Select an Account Against Freight First");
						return;
					}
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToDouble(r5.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r5.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			InvSaleInvoice obj = new InvSaleInvoice();
			obj.Id = RecId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 145;
			obj.BaseDocumentTypeId = BaseDocumentTypeId;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value);
			obj.ManualBillNo = txtbillno.Text.Trim();
			obj.SupplierReferenceNo = Conversion.ToString((object)txtSupplierReference.Text.Trim());
			obj.SupplierInvoiceDate = obj.DocDate;
			obj.PaymentTermId = Conversion.ToInt(CmbPaymentTerm.Value);
			obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			obj.DocDate = DocDate.Value;
			obj.DueDate = duedate.Value;
			obj.DueDays = Conversion.ToInt((object)txtduedays.Text);
			obj.SupplierInvoiceNo = Conversion.ToInt((object)txtduedays.Text);
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.OtherRemarks = obj.RemarksHeader;
			obj.TaxAccountId = Conversion.ToInt(CmbTaxAccount.Value);
			obj.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			obj.IsReserve = false;
			obj.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			obj.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			obj.InvoiceQty = Conversion.ToDecimal((object)txtInvoiceQty.Text);
			obj.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			obj.ScreenName = Conversion.ToString(base.Tag);
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			obj.SupplierInvoiceDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			obj.InvSaleInvoiceCommission = new List<InvSaleInvoiceCommission>();
			obj.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			obj.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			obj.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			rows = grd.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				InvSaleInvoiceDetail vd = new InvSaleInvoiceDetail();
				vd.LineId = r4.RowIndex + 1;
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r4.Cells["Id"].Value) : 0);
				double qty = Conversion.ToDouble(r4.Cells["ItemQty"].Value);
				if (!RefDocumentExist || !(qty <= 0.0))
				{
					FillDetailListCommonForInsertAndDelete(vd, r4);
					FormHelper.ValidateField(vd.ItemId, "Item", r4.RowIndex);
					FormHelper.ValidateField(vd.WarehouseId, "WareHouse", r4.RowIndex);
					FormHelper.ValidateField(vd.RackId, "Rack Name", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemConditionId, "Item Condition", r4.RowIndex);
					FormHelper.ValidateField(vd.JobLotId, "JobLot", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemUOMId, "Pack Uom", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemQty, "Qty", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemRate, "Rate", r4.RowIndex);
					FormHelper.ValidateField(vd.UomScheduleIdRate, "Rate Uom", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemAmount, "Amount", r4.RowIndex);
					if (RefDocumentExist && qty > 0.0)
					{
						FormHelper.ValidateField(vd.ReasonId, "Reason", r4.RowIndex);
					}
					if (vd.TaxNameId > 0 || vd.TaxPercent > 0.0 || vd.TaxAmount > 0.0)
					{
						FormHelper.ValidateField(vd.TaxNameId, "Tax Name", r4.RowIndex);
						FormHelper.ValidateField(vd.TaxPercent, "Tax Percent", r4.RowIndex);
						FormHelper.ValidateField(vd.TaxAmount, "Tax Amount", r4.RowIndex);
					}
					if (vd.BillAmount <= 0.0)
					{
						throw new Exception("Item Net Amount can't be less than equal to zero");
					}
					obj.InvSaleInvoiceDetaillist.Add(vd);
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					PE.InvRevExpItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r3.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					obj.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["AccountId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceJournal PJ = new InvSaleInvoiceJournal();
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
						PJ.TransporterSupCustId = Conversion.ToInt(r2.Cells["AccountId"].Value);
					}
					else
					{
						PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
						PJ.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PJ.JvRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					PJ.JvPrcnt = Conversion.ToDouble(r2.Cells["Percentage"].Value);
					PJ.JvQty = Conversion.ToDouble(r2.Cells["Qty"].Value);
					PJ.JvRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
					PJ.JvDebit = Conversion.ToDouble(r2.Cells["Debit"].Value);
					PJ.JvCredit = Conversion.ToDouble(r2.Cells["Credit"].Value);
					obj.InvSaleInvoiceJournalList.Add(PJ);
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["Transporter"].Value.ToString()) != 0)
				{
					InvSaleInvoiceFreight PF = new InvSaleInvoiceFreight();
					PF.InvGdnId = Conversion.ToInt(r.Cells["GdnId"].Value);
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PF.TansporterId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
						PF.TransporterSupCustId = Conversion.ToInt(r.Cells["Transporter"].Value);
					}
					else
					{
						PF.TansporterId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
						PF.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PF.FreightAmount = Conversion.ToInt(r.Cells["Freight"].Value);
					PF.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					obj.InvSaleInvoiceFreightList.Add(PF);
				}
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvSaleInvoice>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			int sucess = InvSaleInvoice.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Record Saved Successfully [" + obj.DocNo + "] ");
			}
			else
			{
				MessageBox.Show("Record Update Successfully [" + obj.DocNo + "] ");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 145, base.Name);
			}
			_ = WagesStatus;
			Reset();
			if (ChkBok.Checked)
			{
				RecId = sucess;
				VoucherHeadIdGet();
				RecId = 0;
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 145);
			}
			if (ChkPrintslip.Checked)
			{
				CommonServices.PurchaseInvoiceReturn_StorePartySlip145(sucess);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void saveToolStripButton_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			InvSaleInvoice invPI = InvSaleInvoice.GetByID(RecId);
			if (invPI != null)
			{
				tabControl1.SelectedIndex = 0;
				txtdocno.Text = invPI.DocNo.ToString();
				DocDate.Value = invPI.DocDate;
				CmbSupplierName.Value = invPI.SupplierCustomerId;
				((Control)(object)CmbSupplierName).Enabled = false;
				txtSupplierReference.Text = Conversion.ToString((object)invPI.SupplierReferenceNo);
				txtbillno.Text = invPI.ManualBillNo;
				CmbPaymentTerm.Value = invPI.PaymentTermId;
				txtduedays.Text = Conversion.ToString((object)invPI.SupplierInvoiceNo);
				duedate.Value = invPI.SupplierInvoiceDate;
				((Control)(object)combdeliverytrm).Text = invPI.DeliveryTerm;
				txtremarks.Text = Conversion.ToString((object)invPI.OtherRemarks);
				txtInvoiceQty.Text = invPI.InvoiceQty.ToString("#,##0.###");
				txtFcyAmount.Text = invPI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				CmbTaxAccount.Value = invPI.TaxAccountId;
				txtBillAmount.Text = invPI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
				Approved = invPI.IsApproved;
				cmbCurrency.Value = invPI.CurrencyId;
				AttachmentsValues = invPI.AttachmentsValues;
				CustomAttachmentsValues = invPI.CustomAttachmentsValues;
				if (invPI.CurrencyId > 0)
				{
					txtExchangeRate.Text = Conversion.ToString((object)invPI.ExchangeRate);
				}
				else if (invPI.CurrencyId == 0)
				{
					GetConfigurationsFromGlobalandBind();
				}
				VoucherHeadIdGet();
				dtGrid.Rows.Clear();
				FillDetailFromListCommonForReadById(invPI, dtGrid);
				dtInvExp.Rows.Clear();
				for (int k = 0; k < invPI.InvSaleInvoiceExpenseList.Count; k++)
				{
					dtInvExp.Rows.Add(invPI.InvSaleInvoiceExpenseList[k].Id, invPI.InvSaleInvoiceExpenseList[k].InvRevExpItemId, invPI.InvSaleInvoiceExpenseList[k].Qty, invPI.InvSaleInvoiceExpenseList[k].Rate, invPI.InvSaleInvoiceExpenseList[k].Amount, invPI.InvSaleInvoiceExpenseList[k].Remarks);
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.DataSource = dtInvExp;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
				dtGrdGL.Rows.Clear();
				for (int j = 0; j < invPI.InvSaleInvoiceJournalList.Count; j++)
				{
					dtGrdGL.Rows.Add(SubsidiaryAccountAllownOnVouchers ? invPI.InvSaleInvoiceJournalList[j].TransporterSupCustId : invPI.InvSaleInvoiceJournalList[j].ChartofAccountId, invPI.InvSaleInvoiceJournalList[j].ChartofAccountId, invPI.InvSaleInvoiceJournalList[j].JvRemarks, invPI.InvSaleInvoiceJournalList[j].JvPrcnt, invPI.InvSaleInvoiceJournalList[j].JvQty, invPI.InvSaleInvoiceJournalList[j].JvRate, invPI.InvSaleInvoiceJournalList[j].JvDebit, invPI.InvSaleInvoiceJournalList[j].JvCredit);
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdGLedger.DataSource = dtGrdGL;
				grdGLedger.RetrieveStructure();
				gridGLSettings();
				dtFreight.Rows.Clear();
				for (int i = 0; i < invPI.InvSaleInvoiceFreightList.Count; i++)
				{
					dtFreight.Rows.Add(invPI.InvSaleInvoiceFreightList[i].Id, invPI.InvSaleInvoiceFreightList[i].InvGdnId, SubsidiaryAccountAllownOnVouchers ? invPI.InvSaleInvoiceFreightList[i].TransporterSupCustId : invPI.InvSaleInvoiceFreightList[i].TansporterId, invPI.InvSaleInvoiceFreightList[i].TansporterId, invPI.InvSaleInvoiceFreightList[i].FreightAmount, invPI.InvSaleInvoiceFreightList[i].Debit, invPI.InvSaleInvoiceFreightList[i].Remarks);
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				grdFreight.DataSource = dtFreight;
				grdFreight.RetrieveStructure();
				grdFreightSettings();
				RefDocumentExist = invPI.InvSaleInvoiceDetaillist.Any((InvSaleInvoiceDetail r) => r.RefRefDocumentTypeId > 0 && r.RefRefDocIdNo > 0 && r.RefDocSubId > 0);
				BindGrid();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				ExpProportion();
				FreightProportion();
				BillAmount();
				txtExchangeRate_TextChanged(null, null);
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
			}
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
				throw new Exception("Record Not Delete beacause Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 145,
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

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void FillDetailFromListCommonForReadById(InvSaleInvoice inv, DataTable dt, bool ExtraColumns = false)
	{
		if (((inv != null) ? inv.InvSaleInvoiceDetaillist : null) == null || inv.InvSaleInvoiceDetaillist.Count == 0)
		{
			return;
		}
		foreach (InvSaleInvoiceDetail detail in inv.InvSaleInvoiceDetaillist)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.Id;
			dr["InvGdnId"] = detail.InvGdnId;
			dr["InvGdnDetailId"] = detail.InvGdnDetailId;
			dr["GdnNo"] = detail.GdnNo;
			dr["RefDocumentTypeId"] = detail.RefRefDocumentTypeId;
			dr["RefDocId"] = detail.RefRefDocIdNo;
			dr["RefDocSubId"] = detail.RefDocSubId;
			dr["ItemId"] = detail.ItemId;
			dr["Item"] = detail.ItemName;
			dr["ItemCode"] = detail.ItemCode;
			dr["WarehouseId"] = detail.WarehouseId;
			dr["Warehouse"] = detail.WareHouseName;
			dr["RackId"] = detail.RackId;
			dr["RackName"] = detail.rackName;
			dr["ItemConditionId"] = detail.ItemConditionId;
			dr["ItemCondition"] = detail.ItemCondition;
			dr["JobLotId"] = detail.JobLotId;
			dr["JobLot"] = detail.JobLotDescription;
			dr["PackUOMId"] = detail.ItemUOMId;
			dr["PackUOM"] = detail.UOMCodeItem;
			dr["PackEquivalent"] = detail.PackEquivalent;
			dr["ItemQty"] = detail.ItemQty;
			dr["BalQty"] = detail.PurchaseBalanceQty;
			dr["ReasonId"] = detail.ReasonId;
			dr["Rate"] = detail.ItemRate;
			dr["ItemAmount"] = detail.ItemAmount;
			dr["FcyAmount"] = detail.FcyAmount;
			dr["TaxTypeId"] = detail.TaxNameId;
			dr["Tax%"] = detail.TaxPercent;
			dr["TaxAmount"] = detail.TaxAmount;
			dr["BillAmount"] = detail.BillAmount;
			dr["Expense"] = detail.ExpenseAmount;
			dr["Journal"] = detail.JournalAmount;
			dr["Freight"] = detail.FreightAmount;
			dr["GpDate"] = detail.GpDate;
			dr["GpNo"] = detail.GpNo;
			dr["VehicleNo"] = detail.VehicleNo;
			dr["CityId"] = detail.CityId;
			dr["CityName"] = detail.CityName;
			if (ExtraColumns)
			{
				dr["TaxType"] = detail.TaxDescriptions;
			}
			dt.Rows.Add(dr);
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
			DocDate.Focus();
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ResetHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetHistory()
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

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = InvSaleInvoice.AllComboBindAgainstSaleInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = "145"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Customer", ZeroIndex: false);
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

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FillHistoryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistoryGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.DocumentTypeId = 145;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			PI.ScreenName = Conversion.ToString(base.Tag);
			DataTable dthistory = InvSaleInvoice.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("ManualBillNo");
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("PaymentTerm");
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierCustomerId"], dthistory.Rows[i]["CustomerName"], dthistory.Rows[i]["PaymentTerm"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), Conversion.ToDouble(dthistory.Rows[i]["BillAmount"]), dthistory.Rows[i]["UserName"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUserName"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["NoOfAttachments"]);
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
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 3);
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns.Add("ItemSlip");
			grdHistory.RootTable.Columns["ItemSlip"].Key = "ItemSlip";
			grdHistory.RootTable.Columns["ItemSlip"].Caption = "ItemSlip";
			grdHistory.RootTable.Columns["ItemSlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["ItemSlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["ItemSlip"].ButtonText = "ItemSlip";
			grdHistory.RootTable.Columns["ItemSlip"].Width = 70;
			grdHistory.RootTable.Columns.Add("PartySlip");
			grdHistory.RootTable.Columns["PartySlip"].Key = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].Caption = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["PartySlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["PartySlip"].ButtonText = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].Width = 75;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 70;
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["ItemSlip"].Position = 1;
			grdHistory.RootTable.Columns["PartySlip"].Position = 2;
			grdHistory.RootTable.Columns["Voucher"].Position = 3;
			grdHistory.RootTable.Columns["NoOfAttachments"].Position = 4;
			grdHistory.FrozenColumns = 5;
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 110);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
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
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 145);
			}
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
			int rId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = rId;
				ReadById(RecId);
			}
			else if (e.Column.Key == "ItemSlip")
			{
				CommonServices.PurchaseInvoiceReturn_StoreItemSip145A(rId);
			}
			else if (e.Column.Key == "PartySlip")
			{
				CommonServices.PurchaseInvoiceReturn_StorePartySlip145(rId);
			}
			else if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 145);
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, rId, 145, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			InvSaleInvoice Obj = InvSaleInvoice.GetByID(Id);
			if (Obj != null && Obj.InvSaleInvoiceDetaillist.Count == 0)
			{
				grdDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtGrid.Clone();
			dtDetailHistory.Columns.Add("TaxType");
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory, ExtraColumns: true);
			grdDetail.DataSource = dtDetailHistory;
			grdDetail.RetrieveStructure();
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
			grdSettings(grdDetail);
			grdDetail.RootTable.Columns["TaxTypeId"].Visible = false;
			grdDetail.RootTable.Columns["TaxType"].Position = grdDetail.RootTable.Columns["TaxTypeId"].Position + 1;
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
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 145);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseInvoiceReturn_StoreItemSip145A(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn294APrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseInvoiceReturn_StorePartySlip145(RecId);
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtGrid);
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

	public void BillAmount()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				double BillAmount = 0.0;
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double TaxAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["TaxAmount"], (AggregateFunction)2).ToString());
				Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				double FreightAmount = 0.0;
				double JournalDebit = 0.0;
				double JournalCredit = 0.0;
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt((object)r2.Cells["AccountId"].Value.ToString()) > 0)
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
					if (!SubsidiaryAccountAllownOnVouchers)
					{
						if (Conversion.ToInt(r.Cells["GlAccountId"].Value) > 0 && Conversion.ToInt((object)txtSupplierGLId.Text) > 0 && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(r.Cells["GlAccountId"].Value))
						{
							FreightAmount += Conversion.ToDouble(r.Cells["Freight"].Value);
						}
					}
					else if (Conversion.ToInt(r.Cells["Transporter"].Value) > 0 && Conversion.ToInt(CmbSupplierName.Value) > 0 && Conversion.ToInt(CmbSupplierName.Value) == Conversion.ToInt(r.Cells["Transporter"].Value))
					{
						FreightAmount += Conversion.ToDouble(r.Cells["Freight"].Value);
					}
				}
				BillAmount = ItemAmount + TaxAmount + ExpAmount - FreightAmount;
				BillAmount = BillAmount + JournalCredit - JournalDebit;
				txtBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtInvoiceQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.###");
				txtFcyAmount.Text = (BillAmount / Conversion.ToDouble((object)txtExchangeRate.Text)).ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtInvoiceQty.Text = "0";
				txtFcyAmount.Text = "0";
			}
			GridItemNetAmountCalculation();
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
			double TotalFreightAmount = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
			if (TotalFreightAmount > 0.0)
			{
				double ItemQty = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble(obj.Cells["ItemQty"].Value);
					obj.Cells["Freight"].Value = Math.Round(TotalFreightAmount / ItemQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				GridItemNetAmountCalculation();
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Freight"].Value = 0;
				}
			}
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
			double ItemQty = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double Qty = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
				obj.Cells["Expense"].Value = Math.Round(TotalAmount / ItemQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			GridItemNetAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridItemNetAmountCalculation()
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				double itemAmount = Math.Round(Conversion.ToDouble(r.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double Expense = Math.Round(Conversion.ToDouble(r.Cells["Expense"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				r.BeginEdit();
				List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
				if (lst.Count > 0)
				{
					if (!bool.Parse(lst[0].ConfigKey))
					{
						r.Cells["BillAmount"].Value = itemAmount + Conversion.ToDouble(r.Cells["TaxAmount"].Value);
					}
					else
					{
						r.Cells["BillAmount"].Value = Math.Round(itemAmount + Conversion.ToDouble(r.Cells["TaxAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
				}
				else
				{
					r.Cells["BillAmount"].Value = Math.Round(itemAmount + Conversion.ToDouble(r.Cells["TaxAmount"].Value) + Expense, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCaluculation()
	{
		try
		{
			double Qty = 0.0;
			double Rate = 0.0;
			double ItemAmount = 0.0;
			if (Conversion.ToDouble((object)txtRate.Text) != 0.0)
			{
				Rate = Conversion.ToDouble((object)txtRate.Text);
			}
			if (Conversion.ToDouble((object)txtQty.Text) != 0.0)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text);
			}
			if (Qty > 0.0 && Rate > 0.0)
			{
				ItemAmount = Qty * Rate;
				txtAmount.Text = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtAmount.Text = "0";
				txtTotalAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTaxAmountandTotalAmount()
	{
		try
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow != null && Conversion.ToInt(CmbTaxType.Value) > 0)
			{
				double TaxPercent = 0.0;
				if (!(Conversion.ToDouble((object)txtTaxPercent.Text) > 0.0))
				{
					txtTaxPercent.Text = Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value).ToString("#,##0.###");
					TaxPercent = Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value);
				}
				else
				{
					TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
				}
				if (TaxPercent > 100.0)
				{
					MessageBox.Show("Tax % Can't be greater than 100...");
					TaxPercent = 100.0;
					txtTaxPercent.Text = TaxPercent.ToString();
				}
				double num = Conversion.ToDouble((object)txtAmount.Text);
				double TaxAmt = num * TaxPercent / 100.0;
				txtTaxAmount.Text = TaxAmt.ToString(clsGlobalVariables.stringFormatsingle);
				double TotalAmount = num + TaxAmt;
				txtTotalAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
			}
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
			CommonServices.CommasApplyWhileTyping(sender, e);
			AmountCaluculation();
			CalculateTaxAmountandTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			AmountCaluculation();
			CalculateTaxAmountandTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comPackUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			AmountCaluculation();
			CalculateTaxAmountandTotalAmount();
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow != null && Conversion.ToInt(CmbTaxType.Value) > 0)
			{
				CalculateTaxAmountandTotalAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow != null && Conversion.ToInt(CmbTaxType.Value) > 0)
			{
				CalculateTaxAmountandTotalAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
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
			DueDateGenerate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDateGenerate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combdeliverytrm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_ = ((Control)(object)combdeliverytrm).Text == "Ponch";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateGenerate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEmptybagsUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtStockWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtratecut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcommrate_KeyPress(object sender, KeyPressEventArgs e)
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
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtFreightAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

	private void txtgatepassno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnLoadGdn_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && Conversion.ToInt(item.Cells["InvGdnId"].Value) == 0)
				{
					MessageBox.Show("You cannot be Load GDN because you add record exist without GDN");
					return;
				}
			}
			LoadTransactions = new frmLoadGDN_Store(UserAccount);
			LoadTransactions.DocumentTypeId = 144;
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadInGridDetail();
			txtExchangeRate_TextChanged(null, null);
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
			DataTable dtGdnData = LoadTransactions.dtLoader;
			if (dtGdnData.Rows.Count == 0)
			{
				return;
			}
			DataRow firstRow = dtGdnData.Rows[0];
			if (grd.RowCount > 0 && Conversion.ToInt(firstRow["SupplierCustomerId"]) != Conversion.ToInt(CmbSupplierName.Value))
			{
				throw new Exception("Data against another Party Already Exist in Detail");
			}
			CmbSupplierName.Value = Conversion.ToInt(firstRow["SupplierCustomerId"]);
			txtSupplierReference.Text = Conversion.ToString(firstRow["ReferenceNo"]);
			txtremarks.Text = Conversion.ToString(firstRow["RemarksHeader"]);
			((Control)(object)combdeliverytrm).Text = Conversion.ToString(firstRow["DeliveryTerm"]);
			((Control)(object)CmbSupplierName).Enabled = false;
			GdnExist = true;
			ClearDefaultFreightRow();
			Dictionary<int, List<RacksWithWarehouseAndItem>> rackLookup = (from x in clsGlobalVariables.racksWithWarehouseAndItems
				group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
				where (int)r.RowType == 0
				select Conversion.ToInt(r.Cells["InvGdnDetailId"].Value));
			HashSet<int> existingFreightIds = new HashSet<int>(from r in grdFreight.GetRows()
				where (int)r.RowType == 0
				select Conversion.ToInt(r.Cells["GdnId"].Value));
			StringBuilder orderIdsBuilder = new StringBuilder();
			foreach (DataRow dr in dtGdnData.Rows)
			{
				int gdnId = Conversion.ToInt(dr["Id"]);
				int detailId = Conversion.ToInt(dr["DetailId"]);
				int itemId = Conversion.ToInt(dr["ItemId"]);
				orderIdsBuilder.Append(",").Append(gdnId);
				if (!existingDetailIds.Contains(detailId))
				{
					FormHelper.ResolveWarehouseRack(dr, itemId, rackLookup, new FormHelper.WarehouseRackColumnMap
					{
						WarehouseId = "WarehouseId",
						WarehouseName = "WareHouseName",
						RackId = "RackId",
						RackName = "RackName"
					});
					dtGrid.Rows.Add(0, gdnId, detailId, dr["DocNo"], itemId, dr["ItemCode"], dr["ItemName"], dr["WarehouseId"], dr["WareHouseName"], dr["RackId"], dr["RackName"], dr["ItemConditionId"], dr["ItemCondition"], dr["JobLotId"], dr["JobLotDescription"], dr["ItemUomId"], dr["ItemUomCode"], dr["ItemUomEquivalent"], dr["ItemQty"], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dr["GpDate"], dr["GpNo"], dr["VehicleNo"], dr["CityId"], dr["CityName"]);
				}
				if (!existingFreightIds.Contains(gdnId) && Conversion.ToDouble(dr["FrieghtAmountHeader"]) > 0.0)
				{
					string remarks = BuildFreightRemarks(dr);
					if (SubsidiaryAccountAllownOnVouchers)
					{
						dtFreight.Rows.Add(0, gdnId, dr["TransporterSupCustId"], dr["TransporterId"], dr["FrieghtAmountHeader"], 0, remarks);
					}
					else
					{
						dtFreight.Rows.Add(0, gdnId, dr["TransporterId"], 0, dr["FrieghtAmountHeader"], 0, remarks);
					}
				}
			}
			grdFreight.DataSource = dtFreight;
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			LoadExpData(orderIdsBuilder.ToString());
			BindGrid();
			ExpProportion();
			FreightProportion();
			BillAmount();
			DisableFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ClearDefaultFreightRow()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		GridEXRow rr = grdFreight.GetRow(0);
		if (grdFreight.GetDataRows().Length != 0 && rr != null && (int)rr.RowType == 0 && Conversion.ToInt(rr.Cells["Transporter"].Value) == 0 && Conversion.ToDecimal(rr.Cells["Freight"].Value) == 0m && Conversion.ToDecimal(rr.Cells["Debit"].Value) == 0m)
		{
			dtFreight.Clear();
		}
	}

	private string BuildFreightRemarks(DataRow dr)
	{
		StringBuilder sb = new StringBuilder();
		if (Conversion.ToInt(dr["GpNo"]) > 0)
		{
			sb.Append("GpNo: ").Append(dr["GpNo"]);
			sb.Append(" GpDate: ").Append(Conversion.ToDateTime(dr["GpDate"]).ToShortDateString());
		}
		if (!string.IsNullOrEmpty(Conversion.ToString(dr["VehicleNo"])))
		{
			sb.Append(" VehicleNo: ").Append(dr["VehicleNo"]);
		}
		return sb.ToString();
	}

	private void LoadExpData(string ID)
	{
		DataTable dtExp = InvGdn.GdnExpensesByGdnIds(ID);
		if (dtExp.Rows.Count > 0)
		{
			for (int i = 0; i < dtExp.Rows.Count; i++)
			{
				dtInvExp.Rows.Add(0, dtExp.Rows[i]["Id"], dtExp.Rows[i]["InvGdnId"], dtExp.Rows[i]["ItemId"], dtExp.Rows[i]["Qty"], 0, 0, dtExp.Rows[i]["Remarks"]);
			}
		}
	}

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
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
					CmbSupplierName.Focus();
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
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					saveToolStripButton_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbSupplierName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdInvExp).Focus();
					}
					if (((Control)(object)grdInvExp).Focused)
					{
						((Control)(object)grdGLedger).Focus();
					}
					else
					{
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbWarehouseName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnSlip_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlip_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btn294APrint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
				{
					btnPrint_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
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
			if (e.Control && e.KeyCode == Keys.Up)
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip 294");
			dt.Rows.Add("Alt+2", "For Print Slip 294A");
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete" && item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
				int RackId = Conversion.ToInt(item.Cells["RackId"].Value);
				if (RefDocumentExist)
				{
					if (grd.CurrentColumn.Key == "WarehouseId" || grd.CurrentColumn.Key == "Warehouse")
					{
						List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
							where x.ItemId == ItemId
							group x by x.WarehouseId into g
							select g.First()).ToList();
						DataTable dt = new DataTable();
						dt.Columns.Add("Id", typeof(int));
						dt.Columns.Add("WareHouseName", typeof(string));
						dt.BeginLoadData();
						foreach (RacksWithWarehouseAndItem w in list)
						{
							dt.Rows.Add(w.WarehouseId, w.WareHouseName);
						}
						dt.EndLoadData();
						using (GrdPopUp grdPopUp = new GrdPopUp(dt, "Id", "WareHouseName"))
						{
							grdPopUp.StartPosition = FormStartPosition.CenterScreen;
							grdPopUp.ShowDialog();
							int selectedId3 = Conversion.ToInt((object)grdPopUp.ReturnId);
							string selectedName2 = Conversion.ToString((object)grdPopUp.ReturnName);
							item.Cells["WarehouseId"].Value = selectedId3;
							item.Cells["Warehouse"].Value = selectedName2;
							List<RacksWithWarehouseAndItem> racks = clsGlobalVariables.racksWithWarehouseAndItems.Where((RacksWithWarehouseAndItem x) => x.ItemId == ItemId && x.WarehouseId == selectedId3).ToList();
							if (RackId > 0)
							{
								if (!racks.Any((RacksWithWarehouseAndItem x) => x.Id == RackId))
								{
									if (racks.Count > 1)
									{
										item.Cells["RackId"].Value = 0;
										item.Cells["RackName"].Value = "";
									}
									else if (racks.Count == 1)
									{
										RacksWithWarehouseAndItem singleRack = racks.First();
										item.Cells["RackId"].Value = singleRack.Id;
										item.Cells["RackName"].Value = singleRack.RackName;
									}
									else
									{
										item.Cells["RackId"].Value = 0;
										item.Cells["RackName"].Value = "";
									}
								}
							}
							else if (racks.Count == 1)
							{
								RacksWithWarehouseAndItem singleRack2 = racks.First();
								item.Cells["RackId"].Value = singleRack2.Id;
								item.Cells["RackName"].Value = singleRack2.RackName;
							}
						}
						grd.UpdateData();
					}
					else if (grd.CurrentColumn.Key == "RackId" || grd.CurrentColumn.Key == "RackName")
					{
						List<RacksWithWarehouseAndItem> filteredRacks = (from rr in clsGlobalVariables.racksWithWarehouseAndItems
							where rr.ItemId == ItemId && (WarehouseId == 0 || rr.WarehouseId == WarehouseId)
							select rr into row
							group row by row.Id into g
							select g.First()).ToList();
						if (filteredRacks.Count == 0)
						{
							return;
						}
						DataTable dt2 = new DataTable();
						dt2.Columns.Add("Id", typeof(int));
						dt2.Columns.Add("RackName", typeof(string));
						dt2.Columns.Add("WarehouseId", typeof(int));
						dt2.Columns.Add("WarehouseName", typeof(string));
						dt2.BeginLoadData();
						foreach (RacksWithWarehouseAndItem itm in filteredRacks)
						{
							dt2.Rows.Add(itm.Id, itm.RackName, itm.WarehouseId, itm.WareHouseName);
						}
						dt2.EndLoadData();
						using (GrdPopUp grdPopUp2 = new GrdPopUp(dt2, "Id", "RackName", "WarehouseName"))
						{
							grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
							grdPopUp2.ShowDialog();
							int selectedId2 = Conversion.ToInt((object)grdPopUp2.ReturnId);
							string selectedName3 = Conversion.ToString((object)grdPopUp2.ReturnName);
							item.Cells["RackId"].Value = selectedId2;
							item.Cells["RackName"].Value = selectedName3;
							if (WarehouseId == 0)
							{
								RacksWithWarehouseAndItem selected = filteredRacks.FirstOrDefault((RacksWithWarehouseAndItem x) => x.Id == selectedId2);
								if (selected != null)
								{
									item.Cells["WarehouseId"].Value = selected.WarehouseId;
									item.Cells["Warehouse"].Value = selected.WareHouseName;
								}
							}
						}
						grd.UpdateData();
					}
					if (grd.CurrentColumn.Key == "CityId" || grd.CurrentColumn.Key == "CityName")
					{
						using (GrdPopUp obj = new GrdPopUp(dtCity, "Id", "Description"))
						{
							obj.StartPosition = FormStartPosition.CenterScreen;
							obj.ShowDialog();
							int selectedId = Conversion.ToInt((object)obj.ReturnId);
							string selectedName = Conversion.ToString((object)obj.ReturnName);
							item.Cells["CityId"].Value = selectedId;
							item.Cells["CityName"].Value = selectedName;
						}
						grd.UpdateData();
					}
				}
			}
			((Control)(object)grd).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
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
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
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
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
				txtExchangeRate_TextChanged(null, null);
			}
			((Control)(object)grdInvExp).Focus();
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
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					CmbSupplierName.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "ItemSlip")
				{
					CommonServices.PurchaseInvoiceReturn_StoreItemSip145A(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
				}
				if (grdHistory.CurrentColumn.Key == "PartySlip")
				{
					CommonServices.PurchaseInvoiceReturn_StorePartySlip145(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
				}
				if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 145);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value), 145);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				CmbSupplierName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.PurchaseInvoiceReturn_StorePartySlip145(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			((Control)(object)grdHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFreight.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdFreight.CurrentColumn.Key == "Delete")
				{
					dtFreight.Rows[r.RowIndex].Delete();
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					r.Delete();
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				else if (grdFreight.CurrentColumn.Key == "Add")
				{
					AddRowInFreightGrid();
				}
				FreightProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtFreight.Rows[r.RowIndex].Delete();
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				r.Delete();
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				FreightProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInFreightGrid();
				FreightProportion();
				BillAmount();
			}
			if (e.KeyCode == Keys.F1 && grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Transporter")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SupplierCustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["Transporter"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlstForFreight, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["Transporter"].Value = obj2.ReturnId;
				}
				SupCustIdUpdateforFreightGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SupplierCustomerId", "AccountTitle");
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

	private void BtnLoadSaleInvoice_Click(object sender, EventArgs e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				using (frmPendingPurchaseInvoiceForReturn loader = new frmPendingPurchaseInvoiceForReturn(UserAccount))
				{
					loader.ShowDialog();
					loader.BringToFront();
					LoadInGridDetail(loader.dtLoader);
					return;
				}
			}
			GridEXRow r = grd.GetRow(0);
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) == 0)
				{
					throw new Exception("You Can't Load Because direct Entry Already Exist");
				}
				if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) > 0)
				{
					throw new Exception("You Can't Load other Invoice.");
				}
			}
			using frmPendingPurchaseInvoiceForReturn frmPendingPurchaseInvoiceForReturn2 = new frmPendingPurchaseInvoiceForReturn(UserAccount);
			frmPendingPurchaseInvoiceForReturn2.ShowDialog();
			frmPendingPurchaseInvoiceForReturn2.BringToFront();
			LoadInGridDetail(frmPendingPurchaseInvoiceForReturn2.dtLoader);
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
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataRow firstRow = dt.Rows[0];
			CmbSupplierName.Value = Conversion.ToInt(firstRow["SupplierCustomerId"]);
			CmbPaymentTerm.Value = Conversion.ToInt(firstRow["PaymentTermId"]);
			txtduedays.Text = Conversion.ToInt(firstRow["DueDays"]).ToString();
			duedate.Value = Conversion.ToDateTime(firstRow["DueDate"]);
			((Control)(object)CmbSupplierName).Enabled = false;
			txtremarks.Text = Conversion.ToString(firstRow["RemarksHeader"]);
			RefDocumentExist = true;
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["RefDocSubId"].Value));
			Dictionary<int, List<RacksWithWarehouseAndItem>> rackLookup = (from x in clsGlobalVariables.racksWithWarehouseAndItems
				group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
			foreach (DataRow dr in dt.Rows)
			{
				int itemId = Conversion.ToInt(dr["ItemId"]);
				int DetailId = Conversion.ToInt(dr["DetailId"]);
				if (existingDetailIds.Add(DetailId))
				{
					FormHelper.ResolveWarehouseRack(dr, itemId, rackLookup, new FormHelper.WarehouseRackColumnMap
					{
						WarehouseId = "WarehouseId",
						WarehouseName = "WareHouseName",
						RackId = "RackId",
						RackName = "RackName"
					});
					DataRow newRow = dtGrid.NewRow();
					newRow["RefDocId"] = Conversion.ToInt(dr["Id"]);
					newRow["RefDocSubId"] = dr["DetailId"];
					newRow["RefDocumentTypeId"] = dr["DocumentTypeId"];
					newRow["ItemId"] = itemId;
					newRow["Item"] = ((itemId > 0) ? dr["ItemName"] : string.Empty);
					newRow["ItemCode"] = ((itemId > 0) ? dr["ItemCode"] : string.Empty);
					newRow["WareHouseId"] = dr["Warehouseid"];
					newRow["Warehouse"] = dr["WareHouseName"];
					newRow["RackId"] = dr["RackId"];
					newRow["RackName"] = dr["RackName"];
					newRow["ItemConditionId"] = dr["ItemConditionId"];
					newRow["ItemCondition"] = dr["ItemCondition"];
					newRow["JobLotId"] = dr["JobLotId"];
					newRow["JobLot"] = dr["JobLotCode"];
					newRow["PackUOMId"] = dr["ItemUOMId"];
					newRow["PackUOM"] = dr["PackUomCode"];
					newRow["PackEquivalent"] = dr["PackUomEquivalent"];
					newRow["ItemQty"] = dr["BalQty"];
					newRow["BalQty"] = dr["BalQty"];
					newRow["Rate"] = dr["ItemRate"];
					newRow["ItemAmount"] = Conversion.ToDouble(dr["BalQty"]) * Conversion.ToDouble(dr["ItemRate"]);
					newRow["TaxTypeId"] = dr["TaxNameId"];
					newRow["Tax%"] = dr["TaxPercent"];
					newRow["TaxAmount"] = Conversion.ToDouble(dr["BalQty"]) * Conversion.ToDouble(dr["ItemRate"]) * Conversion.ToDouble(dr["TaxPercent"]) / 100.0;
					newRow["CityId"] = dr["CityId"];
					newRow["CityName"] = dr["CityName"];
					newRow["GpNo"] = dr["GpNo"];
					newRow["VehicleNo"] = dr["VehicleNo"];
					dtGrid.Rows.Add(newRow);
				}
			}
			BindGrid();
			ExpProportion();
			FreightProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineReason_Click(object sender, EventArgs e)
	{
		try
		{
			frmDefineReasons frmDefineReasons = new frmDefineReasons(UserAccount);
			frmDefineReasons.StartPosition = FormStartPosition.CenterScreen;
			frmDefineReasons.RefDocumentTypeId = 145;
			frmDefineReasons.BringToFront();
			frmDefineReasons.Show();
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
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Expected O, but got Unknown
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Expected O, but got Unknown
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Expected O, but got Unknown
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Expected O, but got Unknown
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Expected O, but got Unknown
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Expected O, but got Unknown
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Expected O, but got Unknown
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Expected O, but got Unknown
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Expected O, but got Unknown
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Expected O, but got Unknown
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Expected O, but got Unknown
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Expected O, but got Unknown
		//IL_11a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ae: Expected O, but got Unknown
		//IL_11bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c5: Expected O, but got Unknown
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f3: Expected O, but got Unknown
		//IL_14ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c4: Expected O, but got Unknown
		//IL_14d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14db: Expected O, but got Unknown
		//IL_14e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f2: Expected O, but got Unknown
		//IL_1725: Unknown result type (might be due to invalid IL or missing references)
		//IL_172f: Expected O, but got Unknown
		//IL_173c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1746: Expected O, but got Unknown
		//IL_1753: Unknown result type (might be due to invalid IL or missing references)
		//IL_175d: Expected O, but got Unknown
		//IL_19e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f1: Expected O, but got Unknown
		//IL_19fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a08: Expected O, but got Unknown
		//IL_1a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1f: Expected O, but got Unknown
		//IL_8240: Unknown result type (might be due to invalid IL or missing references)
		//IL_824a: Expected O, but got Unknown
		//IL_826e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8278: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
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
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
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
		Appearance Appearance1 = new Appearance();
		Appearance Appearance5 = new Appearance();
		Appearance Appearance6 = new Appearance();
		Appearance Appearance7 = new Appearance();
		Appearance Appearance8 = new Appearance();
		Appearance Appearance9 = new Appearance();
		Appearance Appearance10 = new Appearance();
		Appearance Appearance11 = new Appearance();
		Appearance Appearance12 = new Appearance();
		Appearance Appearance2 = new Appearance();
		Appearance Appearance3 = new Appearance();
		Appearance Appearance4 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
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
		Appearance appearance26 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.PurchaseInvoiceReturn_Store));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.tabFrieght = new System.Windows.Forms.TabPage();
		this.grdFreight = new GridEX();
		this.label16 = new System.Windows.Forms.Label();
		this.tabOtherItems = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.tabPartyAddLess = new System.Windows.Forms.TabPage();
		this.grdGLedger = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.lblTotalAmount = new System.Windows.Forms.Label();
		this.CmbRackName = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.CmbCityName = new UltraCombo();
		this.CmbItemCondition = new UltraCombo();
		this.CmbTaxType = new UltraCombo();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.lblTaxPercent = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.lblTaxTypes = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbPackUOM = new UltraCombo();
		this.lblTaxAmount = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.CmbJobLot = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.label38 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.CmbItemName = new UltraCombo();
		this.lblBalance = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbWarehouseName = new UltraCombo();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.CmbTaxAccount = new UltraCombo();
		this.txtInvoiceQty = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.CmbPaymentTerm = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.combdeliverytrm = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtSupplierReference = new System.Windows.Forms.TextBox();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.CmbSupplierName = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.label15 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label53 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnItemSlip = new System.Windows.Forms.ToolStripButton();
		this.btnPartySlip = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnLoadGdn = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadInvoice = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnDefineReason = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.tabFrieght.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.tabOtherItems.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel12.SuspendLayout();
		this.tabPartyAddLess.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		this.panel11.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1264, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1256, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1250, 676);
		this.panel1.TabIndex = 7;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabFrieght);
		this.tabControl2.Controls.Add(this.tabOtherItems);
		this.tabControl2.Controls.Add(this.tabPartyAddLess);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 303);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1250, 373);
		this.tabControl2.TabIndex = 34;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.panel16);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1242, 344);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 23);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1236, 318);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.EditingCell += new EditingCellEventHandler(grd_EditingCell);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.RecordsDeleted += new System.EventHandler(grd_RecordsDeleted);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1236, 20);
		this.panel16.TabIndex = 33;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 1);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.tabFrieght.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.tabFrieght.Controls.Add(this.label16);
		this.tabFrieght.Location = new System.Drawing.Point(4, 25);
		this.tabFrieght.Name = "tabFrieght";
		this.tabFrieght.Padding = new System.Windows.Forms.Padding(3);
		this.tabFrieght.Size = new System.Drawing.Size(1242, 344);
		this.tabFrieght.TabIndex = 1;
		this.tabFrieght.Text = "Charge To Product";
		this.tabFrieght.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(3, 23);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(1236, 318);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.UpdatingCell += new UpdatingCellEventHandler(grdFreight_UpdatingCell);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.label16.BackColor = System.Drawing.Color.Teal;
		this.label16.Dock = System.Windows.Forms.DockStyle.Top;
		this.label16.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 3);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(1236, 20);
		this.label16.TabIndex = 2;
		this.label16.Text = "Freight_Charge To Product";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabOtherItems.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.tabOtherItems.Controls.Add(this.panel12);
		this.tabOtherItems.Location = new System.Drawing.Point(4, 25);
		this.tabOtherItems.Name = "tabOtherItems";
		this.tabOtherItems.Size = new System.Drawing.Size(1242, 344);
		this.tabOtherItems.TabIndex = 3;
		this.tabOtherItems.Text = "Other Items";
		this.tabOtherItems.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1242, 324);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 33;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label37);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1242, 20);
		this.panel12.TabIndex = 32;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(3, 2);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(98, 17);
		this.label37.TabIndex = 36;
		this.label37.Text = "Expense Grid";
		this.tabPartyAddLess.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.tabPartyAddLess.Controls.Add(this.panel4);
		this.tabPartyAddLess.Location = new System.Drawing.Point(4, 25);
		this.tabPartyAddLess.Name = "tabPartyAddLess";
		this.tabPartyAddLess.Size = new System.Drawing.Size(1242, 344);
		this.tabPartyAddLess.TabIndex = 2;
		this.tabPartyAddLess.Text = "PartyAddLess";
		this.tabPartyAddLess.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1242, 318);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 36;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1242, 26);
		this.panel4.TabIndex = 35;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(147, 17);
		this.label2.TabIndex = 36;
		this.label2.Text = "Party Add/Less Grid";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1250, 278);
		this.panel3.TabIndex = 29;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.txtRemarksDetail);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.lblTotalAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackName);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.txtTotalAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCondition);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.groupBox1.Controls.Add(this.txtTaxPercent);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.lblTaxPercent);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.txtTaxAmount);
		this.groupBox1.Controls.Add(this.txtgatepassno);
		this.groupBox1.Controls.Add(this.lblTaxTypes);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUOM);
		this.groupBox1.Controls.Add(this.lblTaxAmount);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseName);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(3, 164);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1092, 114);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(751, 84);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(272, 23);
		this.txtRemarksDetail.TabIndex = 17;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(751, 65);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(52, 15);
		this.label13.TabIndex = 145;
		this.label13.Text = "Remarks";
		this.lblTotalAmount.AutoSize = true;
		this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalAmount.Location = new System.Drawing.Point(231, 66);
		this.lblTotalAmount.Name = "lblTotalAmount";
		this.lblTotalAmount.Size = new System.Drawing.Size(80, 15);
		this.lblTotalAmount.TabIndex = 143;
		this.lblTotalAmount.Text = "Total Amount";
		this.CmbRackName.AllowNull = (DefaultableBoolean)1;
		this.CmbRackName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbRackName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Location = new System.Drawing.Point(395, 36);
		((System.Windows.Forms.Control)(object)this.CmbRackName).Name = "CmbRackName";
		((System.Windows.Forms.Control)(object)this.CmbRackName).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbRackName).Leave += new System.EventHandler(CmbRackName_Leave);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(395, 18);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(67, 15);
		this.label19.TabIndex = 251;
		this.label19.Text = "Rack Name";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(231, 84);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(100, 23);
		this.txtTotalAmount.TabIndex = 12;
		this.txtTotalAmount.Text = "0";
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance139).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(590, 82);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 16;
		this.CmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Location = new System.Drawing.Point(526, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Name = "CmbItemCondition";
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Leave += new System.EventHandler(CmbItemCondition_Leave);
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance129).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(3, 82);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(84, 84);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(55, 23);
		this.txtTaxPercent.TabIndex = 10;
		this.txtTaxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(590, 66);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 84;
		this.label8.Text = "City Name";
		this.lblTaxPercent.AutoSize = true;
		this.lblTaxPercent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxPercent.Location = new System.Drawing.Point(86, 66);
		this.lblTaxPercent.Name = "lblTaxPercent";
		this.lblTaxPercent.Size = new System.Drawing.Size(38, 15);
		this.lblTaxPercent.TabIndex = 140;
		this.lblTaxPercent.Text = "Tax %";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1025, 62);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(66, 23);
		this.btnCancelUpdateDetial.TabIndex = 20;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(526, 18);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(59, 15);
		this.label20.TabIndex = 250;
		this.label20.Text = "Condition";
		this.txtTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(140, 84);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(90, 23);
		this.txtTaxAmount.TabIndex = 11;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgatepassno.Location = new System.Drawing.Point(443, 84);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(55, 23);
		this.txtgatepassno.TabIndex = 14;
		this.txtgatepassno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgatepassno_KeyPress);
		this.lblTaxTypes.AutoSize = true;
		this.lblTaxTypes.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxTypes.Location = new System.Drawing.Point(3, 66);
		this.lblTaxTypes.Name = "lblTaxTypes";
		this.lblTaxTypes.Size = new System.Drawing.Size(53, 15);
		this.lblTaxTypes.TabIndex = 139;
		this.lblTaxTypes.Text = "Tax Type";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(1025, 84);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(36, 23);
		this.btnAdd.TabIndex = 18;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(1001, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.CmbPackUOM.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Location = new System.Drawing.Point(788, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Name = "CmbPackUOM";
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Size = new System.Drawing.Size(70, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Leave += new System.EventHandler(comPackUOM_Leave);
		this.lblTaxAmount.AutoSize = true;
		this.lblTaxAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxAmount.Location = new System.Drawing.Point(140, 66);
		this.lblTaxAmount.Name = "lblTaxAmount";
		this.lblTaxAmount.Size = new System.Drawing.Size(72, 15);
		this.lblTaxAmount.TabIndex = 141;
		this.lblTaxAmount.Text = "Tax Amount";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(1001, 38);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(90, 23);
		this.txtAmount.TabIndex = 8;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(657, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(CmbJobLot_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(932, 18);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(30, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Rate";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(499, 66);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(930, 38);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(70, 23);
		this.txtRate.TabIndex = 7;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(788, 18);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(35, 15);
		this.label28.TabIndex = 29;
		this.label28.Text = "UOM";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(499, 84);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(90, 23);
		this.txtvehicleno.TabIndex = 15;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(89, 16);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 127;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(861, 18);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(443, 66);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(32, 15);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP #";
		this.txtgpdate.CustomFormat = "dd-MM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(332, 84);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(110, 22);
		this.txtgpdate.TabIndex = 13;
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(859, 38);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(70, 23);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(657, 18);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(46, 15);
		this.label21.TabIndex = 249;
		this.label21.Text = "Job Lot";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 19;
		this.label40.Text = "Rate";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(332, 66);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(50, 15);
		this.label35.TabIndex = 45;
		this.label35.Text = "GP Date";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1025, 84);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(66, 23);
		this.btnUpdateDetail.TabIndex = 19;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(36, 16);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 0;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(3, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBalance.Location = new System.Drawing.Point(155, 18);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(14, 15);
		this.lblBalance.TabIndex = 80;
		this.lblBalance.Text = "0";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(3, 18);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(224, 18);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.CmbWarehouseName.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWarehouseName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Location = new System.Drawing.Point(224, 36);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Name = "CmbWarehouseName";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Leave += new System.EventHandler(CmbWarehouseName_Leave);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtSupplierGLId);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.groupBox2.Controls.Add(this.txtInvoiceQty);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.label59);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtbillno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtSupplierReference);
		this.groupBox2.Controls.Add(this.txtBillAmount);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.duedate);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 29);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1085, 132);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(581, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(73, 15);
		this.label1.TabIndex = 132;
		this.label1.Text = "Tax Account";
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(859, 103);
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 22);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance143;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(675, 16);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 10;
		this.txtInvoiceQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceQty.Enabled = false;
		this.txtInvoiceQty.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceQty.Location = new System.Drawing.Point(675, 75);
		this.txtInvoiceQty.Name = "txtInvoiceQty";
		this.txtInvoiceQty.ShortcutsEnabled = false;
		this.txtInvoiceQty.Size = new System.Drawing.Size(179, 26);
		this.txtInvoiceQty.TabIndex = 12;
		this.txtInvoiceQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(581, 81);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(81, 19);
		this.label63.TabIndex = 130;
		this.label63.Text = "Invoice Qty";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)Appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)Appearance1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)Appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)Appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)Appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)Appearance5;
		((AppearanceBase)Appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)Appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)Appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)Appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)Appearance7;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)Appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)Appearance8;
		((AppearanceBase)Appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)Appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)Appearance9;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)Appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)Appearance10;
		((AppearanceBase)Appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)Appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)Appearance11;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)Appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)Appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)Appearance12;
		((AppearanceBase)Appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)Appearance2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)Appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)Appearance3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)Appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)Appearance4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(944, 17);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(138, 24);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(944, 75);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ShortcutsEnabled = false;
		this.txtFcyAmount.Size = new System.Drawing.Size(138, 26);
		this.txtFcyAmount.TabIndex = 15;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(944, 47);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(138, 23);
		this.txtExchangeRate.TabIndex = 14;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(857, 81);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(72, 15);
		this.label59.TabIndex = 126;
		this.label59.Text = "Fcy Amount";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(857, 51);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(84, 15);
		this.label60.TabIndex = 127;
		this.label60.Text = "Exchange Rate";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(857, 22);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(55, 15);
		this.label61.TabIndex = 128;
		this.label61.Text = "Fcy Code";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(581, 50);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(90, 16);
		this.label56.TabIndex = 125;
		this.label56.Text = "Bill Amount";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(95, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(92, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(189, 18);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(115, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(3, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc No && Date";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(95, 77);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(209, 23);
		this.txtbillno.TabIndex = 3;
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(405, 16);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(174, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Leave += new System.EventHandler(combpttrm_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(3, 107);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(89, 15);
		this.label12.TabIndex = 63;
		this.label12.Text = "Party Reference";
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(405, 75);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(174, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(3, 81);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Manual Bill No";
		this.txtSupplierReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierReference.Location = new System.Drawing.Point(95, 103);
		this.txtSupplierReference.Name = "txtSupplierReference";
		this.txtSupplierReference.Size = new System.Drawing.Size(209, 23);
		this.txtSupplierReference.TabIndex = 4;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.txtBillAmount.Location = new System.Drawing.Point(675, 45);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(179, 27);
		this.txtBillAmount.TabIndex = 11;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(405, 103);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(449, 22);
		this.txtremarks.TabIndex = 9;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(308, 107);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(94, 15);
		this.label24.TabIndex = 102;
		this.label24.Text = "Invoice Remarks";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(3, 51);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Party Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(308, 22);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 100;
		this.label4.Text = "Payment Terms";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(405, 47);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(60, 23);
		this.txtduedays.TabIndex = 6;
		this.txtduedays.Text = "0";
		this.txtduedays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(95, 45);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 2;
		this.CmbSupplierName.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(308, 51);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(98, 15);
		this.label10.TabIndex = 98;
		this.label10.Text = "Due Days && Date";
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Enabled = false;
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(467, 47);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(112, 23);
		this.duedate.TabIndex = 7;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(308, 81);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(80, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Delivery Term";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Controls.Add(this.ChkBok);
		this.panel11.Controls.Add(this.ChkResetOnSave);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1250, 27);
		this.panel11.TabIndex = 110;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 5);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(248, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "PURCHASE INVOICE RETURN";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Checked = true;
		this.ChkPrintslip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintslip.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(1040, 5);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(87, 18);
		this.ChkPrintslip.TabIndex = 9;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1134, 5);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(116, 18);
		this.ChkBok.TabIndex = 10;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.ChkResetOnSave.ForeColor = System.Drawing.Color.White;
		this.ChkResetOnSave.Location = new System.Drawing.Point(850, 5);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(184, 18);
		this.ChkResetOnSave.TabIndex = 8;
		this.ChkResetOnSave.Text = "Reset On Add and  Save";
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[18]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnUpdate, this.btnSave, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.btnItemSlip,
			this.btnPartySlip, this.btnPrint, this.toolStripSeparator5, this.btnLoadGdn, this.BtnLoadInvoice, this.btnshortcutkeys, this.toolStripSeparator1, this.btnDefineReason
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1250, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1256, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 157);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1250, 343);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 45;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label18);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 133);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1250, 24);
		this.panel7.TabIndex = 52;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(5, 3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(142, 19);
		this.label18.TabIndex = 35;
		this.label18.Text = "Filtered Records";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel5.Location = new System.Drawing.Point(3, 500);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1250, 21);
		this.panel5.TabIndex = 51;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 521);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1250, 158);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(2, 2);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(297, 17);
		this.label15.TabIndex = 35;
		this.label15.Text = "Detail Information of Above Selected Row";
		this.panel6.Controls.Add(this.groupBox3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 57);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1250, 76);
		this.panel6.TabIndex = 50;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1084, 72);
		this.groupBox3.TabIndex = 4;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(873, 50);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(873, 28);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(792, 28);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(792, 50);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(723, 45);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 9;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.Leave += new System.EventHandler(btnShowHistory_Leave);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(489, 20);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(58, 15);
		this.label53.TabIndex = 87;
		this.label53.Text = "Customer";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(214, 45);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 86;
		this.label3.Text = "To Doc No";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(214, 20);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(79, 15);
		this.label9.TabIndex = 85;
		this.label9.Text = "From Doc No";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(3, 45);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(48, 15);
		this.label11.TabIndex = 84;
		this.label11.Text = "To Date";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(3, 20);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
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
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(553, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(299, 41);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(168, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(299, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(168, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(66, 41);
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
		this.FromDateHistory.Size = new System.Drawing.Size(139, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1250, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(258, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Purcha Invoice Return History ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1250, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1250, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1195, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 19);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1210, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1209, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 20);
		this.ctrlGrdBar3.TabIndex = 34;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(saveToolStripButton_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnItemSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnItemSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnItemSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnItemSlip.Name = "btnItemSlip";
		this.btnItemSlip.Size = new System.Drawing.Size(97, 22);
		this.btnItemSlip.Text = "145A-Print";
		this.btnItemSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btnPartySlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPartySlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPartySlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPartySlip.Name = "btnPartySlip";
		this.btnPartySlip.Size = new System.Drawing.Size(88, 22);
		this.btnPartySlip.Text = "145-Print";
		this.btnPartySlip.Click += new System.EventHandler(btn294APrint_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(111, 22);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnLoadGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadGdn.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadGdn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGdn.Name = "btnLoadGdn";
		this.btnLoadGdn.Size = new System.Drawing.Size(88, 22);
		this.btnLoadGdn.Text = "Load Gdn";
		this.btnLoadGdn.Visible = false;
		this.btnLoadGdn.Click += new System.EventHandler(btnLoadGdn_Click);
		this.BtnLoadInvoice.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadInvoice.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadInvoice.Name = "BtnLoadInvoice";
		this.BtnLoadInvoice.Size = new System.Drawing.Size(169, 22);
		this.BtnLoadInvoice.Text = "Load &Purchase Invoice";
		this.BtnLoadInvoice.Click += new System.EventHandler(BtnLoadSaleInvoice_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnDefineReason.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefineReason.Image = (System.Drawing.Image)resources.GetObject("btnDefineReason.Image");
		this.btnDefineReason.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineReason.Name = "btnDefineReason";
		this.btnDefineReason.Size = new System.Drawing.Size(109, 18);
		this.btnDefineReason.Text = "Define Reasons";
		this.btnDefineReason.Click += new System.EventHandler(btnDefineReason_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1264, 711);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "PurchaseInvoiceReturn_Store";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.tabFrieght.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.tabOtherItems.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPartyAddLess.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRackName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
