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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
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
namespace Architecture.WinApp.Export
{

public class frmGdBreakUpByInvoiceNew : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtInvoices = new DataTable();

	private DataTable dtGdBreakUp = new DataTable();

	private DataTable dtAdvanceUtilize = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtHistoryGridGdBreakUp = new DataTable();

	private bool FormInitialized;

	private int DocumentTypeId = 214;

	private string ScreenName = "frmGdBreakUpByInvoice";

	private int GdBreakUpRecId;

	private bool UpdateMode;

	private int updateDetailIndexGdBreakUp = -1;

	private int updateDetailIndexAdvanceUtilize = -1;

	private string AdvanceUtilizeIds = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool AllowOneRowPerInvoiceOnGD;

	private List<ExImInvoiceBankGDBreakUp> lstRemoveRecordGdBreakUpDetail = new List<ExImInvoiceBankGDBreakUp>();

	private IContainer components;

	private Panel panel5;

	private TabControl tabControl1;

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

	private TabPage tabGdBreakUpForm;

	private ToolStrip toolStripGDBreakUpForm;

	private ToolStripButton BtnNewGdBreakUp;

	private ToolStripButton BtnSaveGdBreakUp;

	private ToolStripButton BtnUpdateGDBreakUp;

	private ToolStripButton BtnRefreshGdBreakUp;

	internal GridEX grdGdBreakUp;

	private DateTimePicker datInvoiceDateGdBreakUp;

	private DateTimePicker datGDDateGdBreakUp;

	private Label label100;

	private TextBox txtBankInvoiceNoGDBreakUp;

	private Label label102;

	private TextBox txtGDNoGdBreakUp;

	private Button btnCancelGDBreakUpDetail;

	private Button btnUpdateGDBreakUpDetail;

	private Button btnAddGDBreakUpDetail;

	private Label label107;

	private TextBox txtGDValueGdBreakUp;

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

	private Button btnAddAdvanceAgaintGdDetail;

	private Button btnUpdateAdvanceAgaintGdDetail;

	private Label label111;

	private Button btnCancelAdvanceAgaintGdDetail;

	private TextBox txtFIBalanceAdvanceUtlize;

	private Label label112;

	private TextBox txtAdvanceUtlize;

	internal UltraCombo CmbGdNoAdvanceUtilize;

	private Label label115;

	private Panel panel33;

	private CtrlGrdBar ctrlGrdBar6;

	private ToolStrip toolStripAdvanceAgaintGD;

	private ToolStripButton BtnNewAdvanceAgaintGD;

	private ToolStripButton BtnSaveAdvanceAgaintGD;

	private ToolStripButton BtnUpdateAdvanceAgaintGD;

	private ToolStripButton BtnRefreshAdvanceAgaintGD;

	private Panel panel30;

	private Label label113;

	private Label label114;

	internal UltraCombo CmbInvoiceAdvanceUtilize;

	private Label label103;

	private TabPage TabGdBreakUpHistory;

	internal GridEX grdGDhistory;

	private Panel panel34;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label104;

	internal UltraCombo CmbBankNameGdBreakUp;

	private Label label105;

	private TextBox txtFreightGdBreakUp;

	private Label label110;

	private TextBox txtDueDaysGdBreakUp;

	private Label label106;

	private TextBox txtFobValueGdBreakUp;

	private Label label116;

	private TextBox txtExchangeRateGdBreakUp;

	private Label label119;

	private TextBox txtNetAmountGdBreakUp;

	private Label label118;

	private TextBox txtFTTPrcntGdBreakUp;

	private Label label117;

	private TextBox txtFttAmountGdBreakUp;

	private Label label120;

	private TextBox txtRemarksGdBreakUp;

	private Label label121;

	private ToolStrip toolStripGdBreakupHistory;

	private ToolStripButton btnResetGdBreakupHistory;

	private ToolStripButton btnRefreshGdBreakupHistory;

	private Panel panel42;

	private Button btnShowGdBreakUpHistory;

	internal UltraCombo CmbBankGdBankHistory;

	private Label label143;

	private DateTimePicker ToDateGdBreakupHistory;

	private Label label144;

	private DateTimePicker FromDateGdBreakupHistory;

	private Label label147;

	private Panel panel43;

	private Panel panel44;

	private TabControl tabControl3;

	private TabPage tabPage6;

	private TabPage tabPage8;

	internal GridEX grdAdvanceHistory;

	private TabControl tabControlMain;

	private TabPage tabGdBreakUp;

	private TabPage tabAdvanceByGd;

	private Panel panel1;

	private Label label1;

	private TextBox txtInvoiceBalanceGdBreakUp;

	private ToolStripButton btnCustomInvoice;

	internal GridEX grdpaymentdetail;

	internal UltraCombo CmbPaymentTermGdUtilize;

	private Label label76;

	private ToolStripButton FiOpening;

	private ToolStripButton btnFIOpening;

	private Panel panel3;

	private Panel panel2;

	private Panel panel4;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label2;

	private Panel panel6;

	private Button btnShowAdvanceHistory;

	internal UltraCombo CmbInvoiceInvoiceAdvanceHistory;

	private DateTimePicker datFromDateAdvanceHistory;

	private Label label3;

	private Label label4;

	private DateTimePicker datToDateAdvanceHistory;

	private Label label5;

	public frmGdBreakUpByInvoiceNew(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 800);
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
			dtPaymentTerm.Columns.Add("Id", typeof(int));
			dtPaymentTerm.Columns.Add("DocumentTypeId", typeof(int));
			dtPaymentTerm.Columns.Add("PaymentTermId", typeof(int));
			dtPaymentTerm.Columns.Add("PaymentTerm");
			dtPaymentTerm.Columns.Add("ExImEFormRegistrationId", typeof(int));
			dtPaymentTerm.Columns.Add("FinancialInstrumentNo");
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(double));
			dtPaymentTerm.Columns.Add("FcyAmount", typeof(double));
			dtPaymentTerm.Columns.Add("DueDays", typeof(int));
			grdpaymentdetail.DataSource = dtPaymentTerm;
			grdpaymentdetail.RetrieveStructure();
			grdSettingsPaymentTerm();
			dtAdvanceUtilize.Columns.Add("Id", typeof(int));
			dtAdvanceUtilize.Columns.Add("InvoiceId", typeof(int));
			dtAdvanceUtilize.Columns.Add("InvoiceNo");
			dtAdvanceUtilize.Columns.Add("GDId", typeof(int));
			dtAdvanceUtilize.Columns.Add("GDNo");
			dtAdvanceUtilize.Columns.Add("PaymentTermId", typeof(int));
			dtAdvanceUtilize.Columns.Add("PaymentTerm", typeof(string));
			dtAdvanceUtilize.Columns.Add("DocumentTypeId", typeof(int));
			dtAdvanceUtilize.Columns.Add("FIId", typeof(int));
			dtAdvanceUtilize.Columns.Add("FINo");
			dtAdvanceUtilize.Columns.Add("FIDocumentTypeId", typeof(int));
			dtAdvanceUtilize.Columns.Add("UtilizeAmount", typeof(double));
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
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			dtInvoices = ExImInvoice.GetCustomInvoicesForGdMapping(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			DataTable bankInvoicesForGdBreakUpNew = ExImInvoice.GetBankInvoicesForGdBreakUpNew(UserAccount.OrganizationId, UserAccount.CompanyId, GdBreakUpRecId);
			List<Bank> bankData = BanksDbCall();
			DataTable dataForDropDownFromExportGdBreakup = ExImInvoice.GetDataForDropDownFromExportGdBreakup(UserAccount.OrganizationId, UserAccount.CompanyId, "Bank");
			DataTable dataForDropDownFromExportAdvanceUtilizeAgainstGd = ExImInvoice.GetDataForDropDownFromExportAdvanceUtilizeAgainstGd(UserAccount.OrganizationId, UserAccount.CompanyId, "Invoice");
			return new
			{
				InvoiceDataGdBreakUp = bankInvoicesForGdBreakUpNew,
				BankData = bankData,
				HistoryComboData = dataForDropDownFromExportGdBreakup,
				AdvanceHistoryComboData = dataForDropDownFromExportAdvanceUtilizeAgainstGd
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
				BtnSaveGdBreakUp.Enabled = formrights.DoHaveSaveRight;
				BtnUpdateGDBreakUp.Enabled = formrights.DoHaveUpdateRights;
				BtnSaveAdvanceAgaintGD.Enabled = formrights.DoHaveSaveRight;
				BtnUpdateAdvanceAgaintGD.Enabled = formrights.DoHaveUpdateRights;
				GetConfigurationsFromGlobal();
				_ = GdBreakUpRecId;
				InvoicesNoBindGdBreakUp(result.InvoiceDataGdBreakUp);
				BankBind(result.BankData);
				InvoicesNoBindAdvanceUtilize();
				BindGrids();
				HistoryComboBindGdBreakup(result.HistoryComboData);
				HistoryComboBindAdvanceUtilize(result.AdvanceHistoryComboData);
				FromDateGdBreakupHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateGdBreakupHistory.Value = DateTime.Now;
				FormInitialized = true;
				CmbInvoiceGdBreakUp.Focus();
				tabControlMain.SelectedIndex = 0;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void BindGrids()
	{
		try
		{
			if (dtGdBreakUp != null && dtGdBreakUp.Columns.Count > 0)
			{
				grdGdBreakUp.DataSource = dtGdBreakUp;
				grdGdBreakUp.RetrieveStructure();
				grdSettingsGdBreakUp();
			}
			if (dtAdvanceUtilize != null && dtAdvanceUtilize.Columns.Count > 0)
			{
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

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			AllowOneRowPerInvoiceOnGD = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowOneRowPerInvoiceOnGD"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoicesNoBindGdBreakUp(DataTable dtInvoice)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceGdBreakUp, dtInvoice, "Id", "InvoiceNo", "Invoice No", AllColumns: true, Conversion.ToInt(CmbInvoiceGdBreakUp.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private List<Bank> BanksDbCall()
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

	private void BankBind(List<Bank> lst)
	{
		try
		{
			DataTable dtHome = new DataTable();
			dtHome.Columns.Add("Id", typeof(int));
			dtHome.Columns.Add("BranchName");
			dtHome.Columns.Add("BankIBANNo");
			List<Bank> FilterLst = lst.Where((Bank r) => r.IsHomeland == "Home Country").ToList();
			if (FilterLst.Count > 0)
			{
				for (int i = 0; i < FilterLst.Count; i++)
				{
					dtHome.Rows.Add(FilterLst[i].Id, FilterLst[i].BranchName, FilterLst[i].BankIBANNo);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBankNameGdBreakUp, dtHome, "Id", "BranchName", "Bank Name", AllColumns: true, Conversion.ToInt(CmbBankNameGdBreakUp.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbInvoiceGdBreakUp_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbInvoiceGdBreakUp).ActiveRow != null)
			{
				txtInvoiceBalanceGdBreakUp.Text = Conversion.ToString(((UltraDropDownBase)CmbInvoiceGdBreakUp).SelectedRow.Cells[2].Value);
			}
			else
			{
				txtInvoiceBalanceGdBreakUp.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool GdBreakUpFormValidation()
	{
		return FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbInvoiceGdBreakUp, "Invoice No", FormHelper.ValidType.INT),
			(txtGDNoGdBreakUp, "GD No", FormHelper.ValidType.String),
			(txtBankInvoiceNoGDBreakUp, "Bank Invoice No", FormHelper.ValidType.String),
			(txtDueDaysGdBreakUp, "Due Days", FormHelper.ValidType.INT),
			(txtGDValueGdBreakUp, "GD Value", FormHelper.ValidType.Double),
			(txtFobValueGdBreakUp, "FOB Value", FormHelper.ValidType.Double),
			(txtNetAmountGdBreakUp, "Net Amount", FormHelper.ValidType.Double)
		});
	}

	private void btnAddGDBreakUpDetail_Click(object sender, EventArgs e)
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
				if (AllowOneRowPerInvoiceOnGD)
				{
					throw new Exception("You can only one Row Per invoice in the grid");
				}
			}
			if (AllowOneRowPerInvoiceOnGD && Conversion.ToDouble((object)txtGDValueGdBreakUp.Text) != Conversion.ToDouble((object)txtInvoiceBalanceGdBreakUp.Text))
			{
				throw new Exception("GD Value Should be Equal To Invoice Balance");
			}
			dtGdBreakUp.Rows.Add(0, CmbInvoiceGdBreakUp.Value, ((Control)(object)CmbInvoiceGdBreakUp).Text.Trim(), txtGDNoGdBreakUp.Text.Trim(), Conversion.ToDateTime((object)datGDDateGdBreakUp.Value), txtBankInvoiceNoGDBreakUp.Text.Trim(), Conversion.ToDateTime((object)datInvoiceDateGdBreakUp.Value), txtInvoiceBalanceGdBreakUp.Text.Trim(), Conversion.ToInt(CmbBankNameGdBreakUp.Value), ((Control)(object)CmbBankNameGdBreakUp).Text.Trim(), Conversion.ToInt((object)txtDueDaysGdBreakUp.Text), Conversion.ToDouble((object)txtExchangeRateGdBreakUp.Text), Conversion.ToDouble((object)txtGDValueGdBreakUp.Text.Trim()), Conversion.ToDouble((object)txtFreightGdBreakUp.Text), Conversion.ToDouble((object)txtFobValueGdBreakUp.Text), Conversion.ToDouble((object)txtFTTPrcntGdBreakUp.Text), Conversion.ToDouble((object)txtFttAmountGdBreakUp.Text), Conversion.ToDouble((object)txtNetAmountGdBreakUp.Text), Conversion.ToString((object)txtRemarksGdBreakUp.Text));
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

	private void grdGdBreakUp_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdGdBreakUp.CurrentRow;
			if (item != null)
			{
				updateDetailIndexGdBreakUp = item.RowIndex;
				InvoicesNoBindGdBreakUp(ExImInvoice.GetBankInvoicesForGdBreakUpNew(UserAccount.OrganizationId, UserAccount.CompanyId, GdBreakUpRecId));
				CmbInvoiceGdBreakUp.Value = Conversion.ToInt(item.Cells["InvoiceId"].Value);
				((Control)(object)CmbInvoiceGdBreakUp).Text = Conversion.ToString(item.Cells["PartyInvoiceNo"].Value);
				txtGDNoGdBreakUp.Text = Conversion.ToString(item.Cells["GdNo"].Value);
				datGDDateGdBreakUp.Value = Conversion.ToDateTime(item.Cells["GdDate"].Value);
				txtBankInvoiceNoGDBreakUp.Text = Conversion.ToString(item.Cells["InvoiceNo"].Value);
				datInvoiceDateGdBreakUp.Value = Conversion.ToDateTime(item.Cells["InvoiceDate"].Value);
				CmbBankNameGdBreakUp.Value = Conversion.ToInt(item.Cells["BankId"].Value);
				txtRemarksGdBreakUp.Text = Conversion.ToString(item.Cells["Remarks"].Value);
				txtDueDaysGdBreakUp.Text = Conversion.ToString(item.Cells["DueDays"].Value);
				txtExchangeRateGdBreakUp.Text = Conversion.ToString(item.Cells["ExchangeRate"].Value);
				txtGDValueGdBreakUp.Text = Conversion.ToDouble(item.Cells["GDValue"].Value).ToString("#,##0.###");
				txtFreightGdBreakUp.Text = Conversion.ToDouble(item.Cells["Freight"].Value).ToString("#,##0.###");
				txtFobValueGdBreakUp.Text = Conversion.ToDouble(item.Cells["FOBValue"].Value).ToString("#,##0.###");
				txtFTTPrcntGdBreakUp.Text = ((Conversion.ToDouble(item.Cells["FTT%"].Value) > 0.0) ? Conversion.ToDouble(item.Cells["FTT%"].Value).ToString("#,##0.###") : "10");
				txtFttAmountGdBreakUp.Text = Conversion.ToDouble(item.Cells["FttAmount"].Value).ToString("#,##0.###");
				txtNetAmountGdBreakUp.Text = Conversion.ToDouble(item.Cells["NetAmount"].Value).ToString("#,##0.###");
				btnAddGDBreakUpDetail.Visible = false;
				btnUpdateGDBreakUpDetail.Visible = true;
				btnCancelGDBreakUpDetail.Visible = true;
				CmbInvoiceGdBreakUp.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateGDBreakUpDetail_Click(object sender, EventArgs e)
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
					if (i != updateDetailIndexGdBreakUp && Conversion.ToInt(dtGdBreakUp.Rows[i]["InvoiceId"]) != Conversion.ToInt(CmbInvoiceGdBreakUp.Value))
					{
						throw new Exception("You can only add data against one invoice in the grid");
					}
				}
			}
			if (AllowOneRowPerInvoiceOnGD && Conversion.ToDouble((object)txtGDValueGdBreakUp.Text) != Conversion.ToDouble((object)txtInvoiceBalanceGdBreakUp.Text))
			{
				throw new Exception("GD Value Should be Equal To Invoice Balance");
			}
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["BankId"] = CmbBankNameGdBreakUp.Value;
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["BankName"] = ((Control)(object)CmbBankNameGdBreakUp).Text;
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["InvoiceId"] = CmbInvoiceGdBreakUp.Value;
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["PartyInvoiceNo"] = ((Control)(object)CmbInvoiceGdBreakUp).Text;
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["GdNo"] = txtGDNoGdBreakUp.Text;
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["GdDate"] = datGDDateGdBreakUp.Value;
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["InvoiceNo"] = txtBankInvoiceNoGDBreakUp.Text;
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["InvoiceDate"] = datInvoiceDateGdBreakUp.Value;
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["OtherAmount"] = Conversion.ToDouble((object)txtInvoiceBalanceGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["DueDays"] = Conversion.ToInt((object)txtDueDaysGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["ExchangeRate"] = Conversion.ToDouble((object)txtExchangeRateGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["GDValue"] = Conversion.ToDouble((object)txtGDValueGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["Freight"] = Conversion.ToDouble((object)txtFreightGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["FOBValue"] = Conversion.ToDouble((object)txtFobValueGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["FTT%"] = Conversion.ToDouble((object)txtFTTPrcntGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["FttAmount"] = Conversion.ToDouble((object)txtFttAmountGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["NetAmount"] = Conversion.ToDouble((object)txtNetAmountGdBreakUp.Text.Trim());
			dtGdBreakUp.Rows[updateDetailIndexGdBreakUp]["Remarks"] = Conversion.ToString((object)txtRemarksGdBreakUp.Text.Trim());
			grdGdBreakUp.DataSource = dtGdBreakUp;
			ResetGdBreakUpDetails();
			CmbInvoiceGdBreakUp.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelGDBreakUpDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetGdBreakUpDetails();
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
			List<string> hiddenColumns = new List<string> { "Id", "InvoiceId", "OtherAmount", "BankId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdGdBreakUp, 2, 3, hiddenColumns);
			GridEX_Helper.AddButton(grdGdBreakUp, "Delete", "X", 20, 0);
			grdGdBreakUp.FrozenColumns = 1;
			ctrlGrdBar5_Load(null, null);
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
			grdpaymentdetail.RootTable.HeaderLines = 2;
			grdpaymentdetail.AllowEdit = (InheritableBoolean)2;
			grdpaymentdetail.RootTable.Columns["Id"].Visible = false;
			grdpaymentdetail.RootTable.Columns["ExImEFormRegistrationId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["PaymentTermId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdpaymentdetail.RootTable.Columns["ExImEFormRegistrationId"].EditType = (EditType)0;
			grdpaymentdetail.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grdpaymentdetail.RootTable.Columns["PaymentTermId"].EditType = (EditType)0;
			grdpaymentdetail.RootTable.Columns["PaymentTerm"].EditType = (EditType)0;
			grdpaymentdetail.RootTable.Columns["FinancialInstrumentNo"].EditType = (EditType)0;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].EditType = (EditType)1;
			grdpaymentdetail.RootTable.Columns["%OfTotal"].EditType = (EditType)0;
			grdpaymentdetail.RootTable.Columns["PaymentTerm"].Width = 70;
			grdpaymentdetail.RootTable.Columns["PaymentTerm"].HeaderAlignment = (TextAlignment)2;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].Width = 70;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].FormatString = "#,#.###";
			grdpaymentdetail.RootTable.Columns["FcyAmount"].TotalFormatString = "#,#.###";
			grdpaymentdetail.RootTable.Columns["%OfTotal"].AggregateFunction = (AggregateFunction)2;
			grdpaymentdetail.RootTable.Columns["%OfTotal"].TextAlignment = (TextAlignment)3;
			grdpaymentdetail.RootTable.Columns["%OfTotal"].FormatString = "#,#.##";
			grdpaymentdetail.RootTable.Columns["%OfTotal"].TotalFormatString = "#,#.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGdBreakUp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGdBreakUp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRowGdBreakUp(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRowGdBreakUp(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
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
					FillDetailListCommonForInsertAndDelete(vd, r);
					vd.ActionTypeId = 3;
					lstRemoveRecordGdBreakUpDetail.Add(vd);
					dtGdBreakUp.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
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

	private void FillDetailListCommonForInsertAndDelete(ExImInvoiceBankGDBreakUp vd, GridEXRow r)
	{
		vd.DocumentTypeId = 1;
		vd.FormDocumentTypeId = DocumentTypeId;
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
		vd.OtherAmount = Conversion.ToDecimal(r.Cells["OtherAmount"].Value);
	}

	private void Insert()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		try
		{
			if (grdGdBreakUp.GetRows().Count() == 0)
			{
				tabControl1.SelectedIndex = 0;
				throw new Exception("GDBreakUp Detail Record Not Found");
			}
			if (!FormHelper.ConfirmAction("Are you sure to Save"))
			{
				return;
			}
			ExImInvoiceBankGDBreakUp obj = new ExImInvoiceBankGDBreakUp();
			obj.FormDocumentTypeId = DocumentTypeId;
			obj.ExImInvoiceBankGDBreakUpslist = new List<ExImInvoiceBankGDBreakUp>();
			if (lstRemoveRecordGdBreakUpDetail.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecordGdBreakUpDetail.Count; i++)
				{
					obj.ExImInvoiceBankGDBreakUpslist.Add(lstRemoveRecordGdBreakUpDetail[i]);
				}
			}
			decimal BankInvoiceAmount = default(decimal);
			decimal GDValue = default(decimal);
			GridEXRow[] rows = grdGdBreakUp.GetRows();
			foreach (GridEXRow r in rows)
			{
				ExImInvoiceBankGDBreakUp vd = new ExImInvoiceBankGDBreakUp();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r);
				FormHelper.ValidateField(vd.ExImInvoiceId, "Invoice No", r.RowIndex);
				FormHelper.ValidateField(vd.GDNO, "GD No", r.RowIndex);
				FormHelper.ValidateField(vd.BankInvoiceNo, "Bank Invoice No", r.RowIndex);
				FormHelper.ValidateField(vd.DueDays, "Due Days", r.RowIndex);
				FormHelper.ValidateField(vd.GDValue, "GD Value", r.RowIndex);
				FormHelper.ValidateField(vd.FobValue, "FOB Value", r.RowIndex);
				FormHelper.ValidateField(vd.NetToBeRealized, "Net Amount", r.RowIndex);
				GDValue += Conversion.ToDecimal((object)vd.GDValue);
				BankInvoiceAmount = Conversion.ToDecimal(r.Cells["OtherAmount"].Value);
				obj.ExImInvoiceBankGDBreakUpslist.Add(vd);
			}
			if (BankInvoiceAmount != GDValue)
			{
				throw new Exception("Bank InvoiceAmount must be equal to Total GDValue Please Check");
			}
			ExImInvoice.SaveGdBreakUp(obj);
			MessageBox.Show("Record Save Successfully");
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveGdBreakUp_Click(object sender, EventArgs e)
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

	private void ReadyByIdGdBreak(int InvoiceId)
	{
		try
		{
			GdBreakUpRecId = InvoiceId;
			ExImInvoiceBankGDBreakUp obj = ExImInvoice.GetGdBreakUpByInvoiceId(InvoiceId);
			if (obj == null)
			{
				throw new Exception("Record not found");
			}
			tabControl1.SelectedIndex = 0;
			InvoicesNoBindGdBreakUp(ExImInvoice.GetBankInvoicesForGdBreakUpNew(UserAccount.OrganizationId, UserAccount.CompanyId, GdBreakUpRecId));
			dtGdBreakUp.Rows.Clear();
			FillOtherItemDetailFromListCommonForReadById(obj, dtGdBreakUp);
			grdGdBreakUp.DataSource = dtGdBreakUp;
			grdGdBreakUp.RetrieveStructure();
			grdSettingsGdBreakUp();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillOtherItemDetailFromListCommonForReadById(ExImInvoiceBankGDBreakUp invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ExImInvoiceBankGDBreakUpslist : null) == null || invGrn.ExImInvoiceBankGDBreakUpslist.Count == 0)
		{
			return;
		}
		foreach (ExImInvoiceBankGDBreakUp detail in invGrn.ExImInvoiceBankGDBreakUpslist)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.ExImInvoiceId, detail.InvoiceNo, detail.GDNO, detail.GDDate, detail.BankInvoiceNo, detail.BankInvoiceDate, detail.OtherAmount, detail.BankId, detail.BankName,
				detail.DueDays, detail.ExchangeRate, detail.GDValue, detail.Freight, detail.FobValue, detail.CommPercent, detail.CommAmount, detail.NetToBeRealized, detail.Remarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void txtGDValue_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValue();
			CalculateNetAmount();
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
			CalculateNetAmount();
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
			double Fob = Conversion.ToDouble((object)txtGDValueGdBreakUp.Text) - Conversion.ToDouble((object)txtFreightGdBreakUp.Text);
			txtFobValueGdBreakUp.Text = Fob.ToString("#,##0.###");
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
			if (Conversion.ToDouble((object)txtFTTPrcntGdBreakUp.Text) > 100.0)
			{
				txtFTTPrcntGdBreakUp.Text = "10";
				MessageBox.Show("Percent Cant be Greater Than 100", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			double FTTAmount = Conversion.ToDouble((object)txtFobValueGdBreakUp.Text) * Conversion.ToDouble((object)txtFTTPrcntGdBreakUp.Text) / 100.0;
			txtFttAmountGdBreakUp.Text = FTTAmount.ToString("#,##0.###");
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
			if (Conversion.ToDouble((object)txtGDValueGdBreakUp.Text) > 0.0)
			{
				double NetAmount = Conversion.ToDouble((object)txtGDValueGdBreakUp.Text) - Conversion.ToDouble((object)txtFttAmountGdBreakUp.Text);
				txtNetAmountGdBreakUp.Text = NetAmount.ToString("#,##0.###");
			}
			else
			{
				txtNetAmountGdBreakUp.Text = 0.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnNewGdBreakUp_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshGdBreakUp_Click(object sender, EventArgs e)
	{
		try
		{
			GetConfigurationsFromGlobal();
			InvoicesNoBindGdBreakUp(ExImInvoice.GetBankInvoicesForGdBreakUpNew(UserAccount.OrganizationId, UserAccount.CompanyId, GdBreakUpRecId));
			BankBind(BanksDbCall());
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
			updateDetailIndexGdBreakUp = -1;
			txtGDNoGdBreakUp.Text = string.Empty;
			txtBankInvoiceNoGDBreakUp.Text = string.Empty;
			((Control)(object)CmbBankNameGdBreakUp).Text = string.Empty;
			txtRemarksGdBreakUp.Clear();
			txtDueDaysGdBreakUp.Clear();
			txtFreightGdBreakUp.Clear();
			txtGDValueGdBreakUp.Text = string.Empty;
			txtFobValueGdBreakUp.Clear();
			txtFTTPrcntGdBreakUp.Text = "0";
			txtFttAmountGdBreakUp.Clear();
			txtNetAmountGdBreakUp.Clear();
			btnAddGDBreakUpDetail.Visible = true;
			btnUpdateGDBreakUpDetail.Visible = false;
			btnCancelGDBreakUpDetail.Visible = false;
			txtGDNoGdBreakUp.Focus();
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
			ResetGdBreakUpDetails();
			dtGdBreakUp.Rows.Clear();
			grdGdBreakUp.ClearStructure();
			((Control)(object)CmbInvoiceGdBreakUp).Enabled = true;
			CmbInvoiceGdBreakUp.ReadOnly = false;
			GdBreakUpRecId = 0;
			InvoicesNoBindGdBreakUp(ExImInvoice.GetBankInvoicesForGdBreakUpNew(UserAccount.OrganizationId, UserAccount.CompanyId, GdBreakUpRecId));
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
			if (tabControl1.SelectedIndex == 0)
			{
				CmbInvoiceGdBreakUp.Focus();
			}
			else
			{
				FromDateGdBreakupHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			HistoryComboBindGdBreakup(ExImInvoice.GetDataForDropDownFromExportGdBreakup(UserAccount.OrganizationId, UserAccount.CompanyId, "Bank"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBindGdBreakup(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbBankGdBankHistory, dt, "Id", "name", "Bank Name", AllColumns: false, Conversion.ToInt(CmbBankGdBankHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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

	private void FillGDFormHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (FromDateGdBreakupHistory.Checked)
			{
				obj.FromDate = FromDateGdBreakupHistory.Value;
			}
			if (ToDateGdBreakupHistory.Checked)
			{
				obj.ToDate = ToDateGdBreakupHistory.Value;
			}
			obj.AccountId = Conversion.ToInt(CmbBankGdBankHistory.Value);
			DataTable dt = ExImInvoice.ExImInvoiceBankGDBreakUp_History(obj);
			if (dt.Rows.Count > 0)
			{
				if (dtHistoryGridGdBreakUp.Columns.Count == 0)
				{
					dtHistoryGridGdBreakUp.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryGridGdBreakUp.Columns.Add("Id", typeof(int));
					dtHistoryGridGdBreakUp.Columns.Add("ExImInvoiceId", typeof(int));
					dtHistoryGridGdBreakUp.Columns.Add("InvoiceNo", typeof(string));
					dtHistoryGridGdBreakUp.Columns.Add("GDNO", typeof(string));
					dtHistoryGridGdBreakUp.Columns.Add("GDDate", typeof(DateTime));
					dtHistoryGridGdBreakUp.Columns.Add("BankInvoiceNo", typeof(string));
					dtHistoryGridGdBreakUp.Columns.Add("BankInvoiceDate", typeof(DateTime));
					dtHistoryGridGdBreakUp.Columns.Add("OtherAmount", typeof(decimal));
					dtHistoryGridGdBreakUp.Columns.Add("BankName", typeof(string));
					dtHistoryGridGdBreakUp.Columns.Add("DueDays", typeof(int));
					dtHistoryGridGdBreakUp.Columns.Add("ExchangeRate", typeof(decimal));
					dtHistoryGridGdBreakUp.Columns.Add("GDValue", typeof(decimal));
					dtHistoryGridGdBreakUp.Columns.Add("Freight", typeof(decimal));
					dtHistoryGridGdBreakUp.Columns.Add("FobValue", typeof(decimal));
					dtHistoryGridGdBreakUp.Columns.Add("FTT%", typeof(decimal));
					dtHistoryGridGdBreakUp.Columns.Add("FTTAmount", typeof(decimal));
					dtHistoryGridGdBreakUp.Columns.Add("NetAmount", typeof(decimal));
					dtHistoryGridGdBreakUp.Columns.Add("Remarks", typeof(string));
				}
				dtHistoryGridGdBreakUp.Rows.Clear();
				foreach (DataRow row in dt.Rows)
				{
					dtHistoryGridGdBreakUp.Rows.Add(row["DocumentTypeId"], row["Id"], row["ExImInvoiceId"], row["InvoiceNo"], row["GDNO"], Conversion.ToDateTime(row["GDDate"]).ToShortDateString(), row["BankInvoiceNo"], row["BankInvoiceDate"], row["OtherAmount"], row["BranchName"], row["DueDays"], row["ExchangeRate"], row["GDValue"], row["Freight"], row["FobValue"], row["CommPercent"], row["CommAmount"], row["NetToBeRealized"], row["Remarks"]);
				}
				grdGDhistory.DataSource = dtHistoryGridGdBreakUp;
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "OtherAmount", "ExImInvoiceId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdGDhistory, 2, 3, hiddenColumns);
			grdGDhistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdGDhistory.RootTable.Columns["GDNO"].Width = Constants.ExportConstants.DocNoConstant;
			grdGDhistory.RootTable.Columns["GDDate"].Width = Constants.ExportConstants.DateConstant;
			grdGDhistory.RootTable.Columns["BankInvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdGDhistory.RootTable.Columns["BankInvoiceDate"].Width = Constants.ExportConstants.DateConstant;
			grdGDhistory.RootTable.Columns["OtherAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdGDhistory.RootTable.Columns["BankName"].Width = Constants.ExportConstants.ItemNameConstants;
			grdGDhistory.RootTable.Columns["DueDays"].Width = Constants.ExportConstants.DueDays;
			grdGDhistory.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdGDhistory.RootTable.Columns["GDValue"].Width = Constants.ExportConstants.AmountConstant;
			grdGDhistory.RootTable.Columns["Freight"].Width = Constants.ExportConstants.AmountConstant;
			grdGDhistory.RootTable.Columns["FobValue"].Width = Constants.ExportConstants.AmountConstant;
			grdGDhistory.RootTable.Columns["FTT%"].Width = Constants.ExportConstants.PercentOfTotal;
			grdGDhistory.RootTable.Columns["FTTAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdGDhistory.RootTable.Columns["NetAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdGDhistory.RootTable.Columns["Remarks"].Width = Constants.ExportConstants.RemarksConstants;
			ctrlGrdBar7_Load(null, null);
			GridEX_Helper.AddButton(grdGDhistory, "ContainerBreakUp", "BreakUp", 80);
			if (formrights.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(grdGDhistory, "Edit", "Edit", 50, grdGDhistory.RootTable.Columns["InvoiceNo"].Position - 1);
			}
			grdGDhistory.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGDhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGDhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				ReadyByIdGdBreak(Conversion.ToInt(item.Cells["ExImInvoiceId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGDhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGDhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Edit")
				{
					grdGDhistory_DoubleClick(null, null);
				}
				else if (e.Column.Key == "ContainerBreakUp")
				{
					GdContainerBreakUp gdContainerBreakUp = new GdContainerBreakUp(UserAccount);
					gdContainerBreakUp.GdIdFromBreakUp = Conversion.ToInt(r.Cells["Id"].Value);
					gdContainerBreakUp.Show();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGDhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdGDhistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdGDhistory.CurrentColumn.Key == "Edit")
				{
					grdGDhistory_DoubleClick(null, null);
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

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdGdBreakUp, dtGdBreakUp);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdGDhistory, dtHistoryGridGdBreakUp);
			}
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

	private void InvoicesNoBindAdvanceUtilize()
	{
		try
		{
			DataTable dt = HelperMethods.GetDistinctDataTable(dtInvoices, "Id", "InvoiceNo");
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceAdvanceUtilize, dt, "Id", "InvoiceNo", "Invoice No", AllColumns: true, Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GDsNoBindAdvanceUtilize()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("GdId", typeof(int));
			dt.Columns.Add("GDNO", typeof(string));
			dt.Columns.Add("GDValue", typeof(double));
			dt.Columns.Add("DocumentTypeId", typeof(int));
			dt.Columns.Add("GdDocTypeId", typeof(int));
			int invoiceId = Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value);
			if (invoiceId > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtInvoices.AsEnumerable()
					where r.Field<int?>("Id") == invoiceId
					select r;
				if (!rows.Any())
				{
					return;
				}
				foreach (DataRow row in rows)
				{
					dt.Rows.Add(row.Field<int?>("GdId").GetValueOrDefault(), row.Field<string>("GDNO") ?? string.Empty, Convert.ToDouble((row["GDValue"] == DBNull.Value) ? ((object)0) : row["GDValue"]), row.Field<int?>("DocumentTypeId").GetValueOrDefault(), row.Field<int?>("GdDocTypeId").GetValueOrDefault());
				}
				InfragisticsHelper.BindAndRetainSelection(CmbGdNoAdvanceUtilize, dt, "GdId", "GDNO", "GD No", AllColumns: true, Conversion.ToInt(CmbGdNoAdvanceUtilize.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			}
			else
			{
				((Control)(object)CmbGdNoAdvanceUtilize).Text = string.Empty;
				((UltraGridBase)CmbGdNoAdvanceUtilize).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FinancialInstrumentBindAdvanceUtilize(int PaymentTermId)
	{
		try
		{
			EnumerableRowCollection<DataRow> rows = from r in dtPaymentTerm.AsEnumerable()
				where r.Field<int?>("PaymentTermId") == PaymentTermId
				select r;
			DataTable obj = (rows.Any() ? rows.CopyToDataTable() : dtPaymentTerm.Clone());
			DataTable dt = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"FINo",
						typeof(string)
					},
					{
						"FcyAmount",
						typeof(double)
					},
					{
						"DocumentTypeId",
						typeof(int)
					}
				}
			};
			foreach (DataRow dr in obj.Rows)
			{
				dt.Rows.Add(dr["ExImEFormRegistrationId"], dr["FinancialInstrumentNo"], dr["FcyAmount"], dr["DocumentTypeId"]);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbFIAdvanceUtlize, dt, "Id", "FINo", "FI No", AllColumns: true, Conversion.ToInt(CmbFIAdvanceUtlize.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceAdvanceUtilize_Leave(object sender, EventArgs e)
	{
		try
		{
			int InvoiceId = Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value);
			GDsNoBindAdvanceUtilize();
			if (((UltraGridBase)CmbInvoiceAdvanceUtilize).ActiveRow != null && InvoiceId > 0)
			{
				getPaymentTermDetailByInvoiceId(InvoiceId);
				return;
			}
			dtPaymentTerm.Rows.Clear();
			grdpaymentdetail.ClearStructure();
			((Control)(object)CmbPaymentTermGdUtilize).Text = string.Empty;
			((UltraGridBase)CmbPaymentTermGdUtilize).DataSource = null;
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
			dtPaymentTerm.Rows.Clear();
			DataTable dt = ExImInvoice.getPaymentTermDetailByInvoiceId(InvoiceId, 0);
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow item in dt.Rows)
				{
					dtPaymentTerm.Rows.Add(item["Id"], item["DocumentTypeId"], item["PaymentTermId"], item["PaymentTerm"], item["ExImEFormRegistrationId"], item["FinancialInstrumentNo"], item["PrcntOfTotal"], item["FcyAmount"], item["DueDays"]);
				}
				grdpaymentdetail.DataSource = dtPaymentTerm;
				grdpaymentdetail.RetrieveStructure();
				grdSettingsPaymentTerm();
			}
			else
			{
				grdpaymentdetail.DataSource = null;
			}
			PaymentTermGdUtilizeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermGdUtilizeFill()
	{
		try
		{
			DataTable dt = HelperMethods.GetDistinctDataTable(dtPaymentTerm, "PaymentTermId", "PaymentTerm");
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTermGdUtilize, dt, "PaymentTermId", "PaymentTerm", "Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTermGdUtilize.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBindAdvanceUtilize(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceInvoiceAdvanceHistory, dt, "Id", "name", "Invoice No", AllColumns: false, Conversion.ToInt(CmbInvoiceInvoiceAdvanceHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool AdvanceUtilizeFormValidation()
	{
		return FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbInvoiceAdvanceUtilize, "Invoice No", FormHelper.ValidType.INT),
			((Control)(object)CmbGdNoAdvanceUtilize, "GD No", FormHelper.ValidType.INT),
			((Control)(object)CmbPaymentTermGdUtilize, "Payment Term", FormHelper.ValidType.INT),
			(txtAdvanceUtlize, "Utilize Amount", FormHelper.ValidType.Double)
		});
	}

	private void AdvanceUtilizeGridBindByInvoiceWithDbCall(int InvoiceId, int InvoiceDocumentTypeId)
	{
		try
		{
			if (InvoiceId == 0)
			{
				return;
			}
			if (grdAdvanceUtlize.GetRows().Length == 0)
			{
				dtAdvanceUtilize.Rows.Clear();
				AdvanceUtilizeagainstGDs obj = ExImInvoice.GetAdvanceUtilizeByInvoiceId(InvoiceId, InvoiceDocumentTypeId);
				for (int i = 0; i < obj.AdvanceUtilizeagainstGDslist.Count; i++)
				{
					dtAdvanceUtilize.Rows.Add(obj.AdvanceUtilizeagainstGDslist[i].Id, obj.AdvanceUtilizeagainstGDslist[i].EximInvoiceId, obj.AdvanceUtilizeagainstGDslist[i].InvoiceNo, obj.AdvanceUtilizeagainstGDslist[i].GDId, obj.AdvanceUtilizeagainstGDslist[i].GDNO, obj.AdvanceUtilizeagainstGDslist[i].RefDocumentTypeId, obj.AdvanceUtilizeagainstGDslist[i].FIId, obj.AdvanceUtilizeagainstGDslist[i].FINo, obj.AdvanceUtilizeagainstGDslist[i].DocumentTypeId, obj.AdvanceUtilizeagainstGDslist[i].UtilizeAmount);
				}
			}
			grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
			grdAdvanceUtlize.RetrieveStructure();
			grdSettingsAdvanceUtilize();
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
			if (!AdvanceUtilizeFormValidation())
			{
				return;
			}
			if (Conversion.ToInt(CmbPaymentTermGdUtilize.Value) == 1 && (((UltraGridBase)CmbFIAdvanceUtlize).ActiveRow == null || Conversion.ToInt(CmbFIAdvanceUtlize.Value) == 0))
			{
				MessageBox.Show("Financial Instrument field is required...");
				CmbFIAdvanceUtlize.Focus();
				return;
			}
			if (dtAdvanceUtilize != null || dtAdvanceUtilize.Rows.Count > 0)
			{
				GridEXRow[] rows = grdAdvanceUtlize.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					if (Conversion.ToInt(rows[i].Cells["InvoiceId"].Value) != Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value))
					{
						MessageBox.Show("Record cannot be add against multi invoice");
						return;
					}
				}
			}
			double utilizeAmount = Conversion.ToDouble((object)txtAdvanceUtlize.Text.Trim());
			double gdBalance = Conversion.ToDouble((object)txtGdValueAdvanceUtlize.Text.Trim());
			Conversion.ToInt(CmbGdNoAdvanceUtilize.Value);
			if (utilizeAmount > gdBalance)
			{
				MessageBox.Show($"Utilize Amount ({utilizeAmount:N2}) cannot exceed GD Balance ({gdBalance:N2}) for the selected GD.", "Invalid Utilize Amount", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (Conversion.ToInt(CmbPaymentTermGdUtilize.Value) == 1)
			{
				double fiBalance = Conversion.ToDouble((object)txtFIBalanceAdvanceUtlize.Text.Trim());
				Conversion.ToInt(CmbFIAdvanceUtlize.Value);
				if (utilizeAmount > fiBalance)
				{
					MessageBox.Show($"Utilize Amount ({utilizeAmount:N2}) cannot exceed FI Balance ({fiBalance:N2}) for the selected FI.", "Invalid Utilize Amount", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			dtAdvanceUtilize.Rows.Add(0, CmbInvoiceAdvanceUtilize.Value, ((Control)(object)CmbInvoiceAdvanceUtilize).Text.Trim(), CmbGdNoAdvanceUtilize.Value, ((Control)(object)CmbGdNoAdvanceUtilize).Text.Trim(), CmbPaymentTermGdUtilize.Value, ((Control)(object)CmbPaymentTermGdUtilize).Text.Trim(), Conversion.ToInt(((UltraDropDownBase)CmbGdNoAdvanceUtilize).SelectedRow.Cells["GdDocTypeId"].Value), CmbFIAdvanceUtlize.Value, ((Control)(object)CmbFIAdvanceUtlize).Text.Trim(), Conversion.ToInt(((UltraDropDownBase)CmbFIAdvanceUtlize).SelectedRow.Cells["DocumentTypeId"].Value), Conversion.ToDouble((object)txtAdvanceUtlize.Text.Trim()));
			grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
			grdAdvanceUtlize.RetrieveStructure();
			grdSettingsAdvanceUtilize();
			ResetGdAdvanceUtilizeDetails();
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
				CmbPaymentTermGdUtilize.Value = Conversion.ToInt(item.Cells["PaymentTermId"].Value);
				CmbFIAdvanceUtlize.Value = Conversion.ToInt(item.Cells["FIId"].Value);
				txtAdvanceUtlize.Text = Conversion.ToString(item.Cells["UtilizeAmount"].Value);
				btnAddAdvanceAgaintGdDetail.Visible = false;
				btnUpdateAdvanceAgaintGdDetail.Visible = true;
				btnCancelAdvanceAgaintGdDetail.Visible = true;
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
			if (!AdvanceUtilizeFormValidation())
			{
				return;
			}
			if (Conversion.ToInt(CmbPaymentTermGdUtilize.Value) == 1 && (((UltraGridBase)CmbFIAdvanceUtlize).ActiveRow == null || Conversion.ToInt(CmbFIAdvanceUtlize.Value) == 0))
			{
				MessageBox.Show("Financial Instrument field is required...");
				CmbFIAdvanceUtlize.Focus();
				return;
			}
			if (dtAdvanceUtilize != null || dtAdvanceUtilize.Rows.Count > 0)
			{
				GridEXRow[] rows = grdAdvanceUtlize.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					if (Conversion.ToInt(rows[i].Cells["InvoiceId"].Value) != Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value))
					{
						MessageBox.Show("Record cannot be add against multi invoice");
						return;
					}
				}
			}
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["InvoiceId"] = Conversion.ToInt(CmbInvoiceAdvanceUtilize.Value);
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["InvoiceNo"] = ((Control)(object)CmbInvoiceAdvanceUtilize).Text;
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["GDId"] = Conversion.ToInt(CmbGdNoAdvanceUtilize.Value);
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["GDNo"] = ((Control)(object)CmbGdNoAdvanceUtilize).Text;
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["PaymentTermId"] = Conversion.ToInt(CmbPaymentTermGdUtilize.Value);
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["PaymentTerm"] = ((Control)(object)CmbPaymentTermGdUtilize).Text;
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["FIId"] = Conversion.ToInt(CmbFIAdvanceUtlize.Value);
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["FINo"] = ((Control)(object)CmbFIAdvanceUtlize).Text;
			dtAdvanceUtilize.Rows[updateDetailIndexAdvanceUtilize]["UtilizeAmount"] = txtAdvanceUtlize.Text.Trim();
			grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
			btnAddAdvanceAgaintGdDetail.Visible = true;
			btnUpdateAdvanceAgaintGdDetail.Visible = false;
			btnCancelAdvanceAgaintGdDetail.Visible = false;
			ResetGdAdvanceUtilizeDetails();
			CmbInvoiceAdvanceUtilize.Focus();
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
			btnAddAdvanceAgaintGdDetail.Visible = true;
			btnUpdateAdvanceAgaintGdDetail.Visible = false;
			btnCancelAdvanceAgaintGdDetail.Visible = false;
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
			grdAdvanceUtlize.RootTable.HeaderLines = 2;
			grdAdvanceUtlize.AllowEdit = (InheritableBoolean)2;
			grdAdvanceUtlize.RootTable.Columns["Id"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["GDId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["InvoiceId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["FIId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["PaymentTermId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["FIDocumentTypeId"].Visible = false;
			grdAdvanceUtlize.RootTable.Columns["FINo"].Caption = "Financial Instrument No";
			grdAdvanceUtlize.RootTable.Columns["FINo"].Width = 180;
			grdAdvanceUtlize.RootTable.Columns["InvoiceNo"].Width = 145;
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

	private void BtnSaveAdvanceAgaintGD_Click(object sender, EventArgs e)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		try
		{
			if (grdAdvanceUtlize.GetRows().Count() <= 0)
			{
				throw new Exception("Advance Utilize Grid Record not found");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			DataTable dt = (DataTable)((UltraGridBase)CmbGdNoAdvanceUtilize).DataSource;
			if (ValidateByKey(dt, dtAdvanceUtilize, "GdId", "GDValue", "UtilizeAmount", "GDNO") && ValidateByKey(dtAdvanceUtilize, dtPaymentTerm, "PaymentTermId", "UtilizeAmount", "FcyAmount", "Payment Term") && grdAdvanceUtlize.RowCount > 0)
			{
				AdvanceUtilizeagainstGDs obj = new AdvanceUtilizeagainstGDs();
				obj.AdvanceUtilizeagainstGDslist = new List<AdvanceUtilizeagainstGDs>();
				obj.Ids = AdvanceUtilizeIds;
				GridEXRow[] rows = grdAdvanceUtlize.GetRows();
				foreach (GridEXRow r in rows)
				{
					AdvanceUtilizeagainstGDs vd = new AdvanceUtilizeagainstGDs();
					vd.EximInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.GDId = Conversion.ToInt(r.Cells["GDId"].Value);
					vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					vd.FIId = Conversion.ToInt(r.Cells["FIId"].Value);
					vd.FINo = Conversion.ToString(r.Cells["FINo"].Value);
					vd.UtilizeAmount = Conversion.ToDecimal(r.Cells["UtilizeAmount"].Value);
					vd.InvoiceDocTypeId = 214;
					vd.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
					vd.DocumentTypeId = Conversion.ToInt(r.Cells["FIDocumentTypeId"].Value);
					vd.EntryUserId = UserAccount.ID;
					vd.ModifyUserId = UserAccount.ID;
					vd.OrganizationId = UserAccount.OrganizationId;
					vd.CompanyId = UserAccount.CompanyId;
					obj.AdvanceUtilizeagainstGDslist.Add(vd);
				}
				if ((from x in obj.AdvanceUtilizeagainstGDslist
					where x.EximInvoiceId > 0
					select x.EximInvoiceId).Distinct().ToList().Count > 1)
				{
					throw new Exception("Record cannot be inserted against multiple invoices");
				}
				ExImInvoice.SaveAdvanceUtilize(obj);
				MessageBox.Show("Record Save Successfully");
				dtAdvanceUtilize.Rows.Clear();
				grdAdvanceUtlize.ClearStructure();
				AdvanceUtilizeIds = "";
				BtnNewAdvanceAgaintGD_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool ValidateByKey(DataTable gridA, DataTable gridB, string keyColumn, string amountColumnA, string amountColumnB, string keyCaption)
	{
		foreach (object key in (from x in (from r in gridA.AsEnumerable()
				select r[keyColumn]).Union(from r in gridB.AsEnumerable()
				select r[keyColumn])
			where x != DBNull.Value
			select x).Distinct())
		{
			double sumA = SafeSum(gridA, (DataRow r) => object.Equals(r[keyColumn], key), amountColumnA);
			double sumB = SafeSum(gridB, (DataRow r) => object.Equals(r[keyColumn], key), amountColumnB);
			if (Math.Round(sumA, 2) != Math.Round(sumB, 2))
			{
				MessageBox.Show(keyCaption + " wise mismatch.\n\n" + $"{keyCaption}: {key}\n" + $"Advance Total: {sumA:N2}\n" + $"Schedule Total: {sumB:N2}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
		}
		return true;
	}

	private double SafeSum(DataTable dt, Func<DataRow, bool> predicate, string amountColumn)
	{
		if (dt == null || dt.Rows.Count == 0)
		{
			return 0.0;
		}
		return dt.AsEnumerable().Where(predicate).Sum((DataRow r) => Convert.ToDouble((r[amountColumn] == DBNull.Value) ? ((object)0) : r[amountColumn]));
	}

	private void BtnRefreshAdvanceAgaintGD_Click(object sender, EventArgs e)
	{
	}

	private void ResetGdAdvanceUtilizeDetails()
	{
		try
		{
			txtAdvanceUtlize.Text = string.Empty;
			CmbGdNoAdvanceUtilize.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewAdvanceAgaintGD_Click(object sender, EventArgs e)
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
			btnAddAdvanceAgaintGdDetail.Visible = true;
			btnUpdateAdvanceAgaintGdDetail.Visible = false;
			btnCancelAdvanceAgaintGdDetail.Visible = false;
			ResetGdAdvanceUtilizeDetails();
			dtAdvanceUtilize.Rows.Clear();
			grdAdvanceUtlize.ClearStructure();
			dtInvoices.Rows.Clear();
			dtInvoices = ExImInvoice.GetCustomInvoicesForGdMapping(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			InvoicesNoBindAdvanceUtilize();
			dtPaymentTerm.Rows.Clear();
			grdpaymentdetail.DataSource = null;
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
				double num = Conversion.ToDouble(((UltraDropDownBase)CmbFIAdvanceUtlize).SelectedRow.Cells["FcyAmount"].Value);
				int fiId = Conversion.ToInt(CmbFIAdvanceUtlize.Value);
				double utilizedInGrid = ((dtAdvanceUtilize == null || dtAdvanceUtilize.Rows.Count == 0) ? 0.0 : (from r in dtAdvanceUtilize.AsEnumerable()
					where r.Field<int?>("FIId") == fiId
					select r).Sum((DataRow r) => r.Field<double?>("UtilizeAmount").GetValueOrDefault()));
				double FIBalance = num - utilizedInGrid;
				txtFIBalanceAdvanceUtlize.Text = Conversion.ToString((object)FIBalance);
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

	private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void grdAdvanceHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdAdvanceHistory.CurrentRow;
			if (item == null)
			{
				return;
			}
			tabControl3.SelectedIndex = 0;
			dtAdvanceUtilize.Rows.Clear();
			int invoiceid = Conversion.ToInt(item.Cells["InvoiceId"].Value);
			DataTable dt = (DataTable)grdAdvanceHistory.DataSource;
			dtInvoices.Rows.Clear();
			dtInvoices = ExImInvoice.GetCustomInvoicesForGdMapping(UserAccount.OrganizationId, UserAccount.CompanyId, invoiceid);
			InvoicesNoBindAdvanceUtilize();
			if (invoiceid > 0)
			{
				CmbInvoiceAdvanceUtilize.Value = invoiceid;
				CmbInvoiceAdvanceUtilize_Leave(null, null);
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<int?>("InvoiceId") == invoiceid
				select r;
			foreach (DataRow dr in (rows.Any() ? rows.CopyToDataTable() : dt.Clone()).Rows)
			{
				dtAdvanceUtilize.Rows.Add(dr["Id"], dr["InvoiceId"], dr["InvoiceNo"], dr["GDId"], dr["GDNo"], dr["PaymentTermId"], dr["PaymentTerm"], dr["DocumentTypeId"], dr["FIId"], dr["FINo"], dr["FIDocumentTypeId"], dr["UtilizeAmount"]);
			}
			grdAdvanceUtlize.DataSource = dtAdvanceUtilize;
			grdAdvanceUtlize.RetrieveStructure();
			grdSettingsAdvanceUtilize();
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

	private void frmGdBreakUpByInvoice_FormClosing(object sender, FormClosingEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex != 0 && tabControl1.SelectedIndex == 3)
			{
				btnResetGdBreakupHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshGdBreakupHistory_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 83;
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
				else if (tabControl1.SelectedIndex == 4)
				{
					tabControl1.SelectedIndex = 0;
				}
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex != 0 && tabControl1.SelectedIndex == 3)
			{
				((Control)(object)grdGDhistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex != 0 && tabControl1.SelectedIndex == 3)
			{
				FromDateGdBreakupHistory.Focus();
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 116;
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCustomInvoice_Click(object sender, EventArgs e)
	{
		new frmCustomInvoice(UserAccount).Show();
	}

	private void FiOpening_Click(object sender, EventArgs e)
	{
		try
		{
			new frmFIOpening(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFIOpening_Click(object sender, EventArgs e)
	{
		try
		{
			new frmFIOpening(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTermGdUtilize_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbFIAdvanceUtlize).Text = string.Empty;
			((UltraGridBase)CmbFIAdvanceUtlize).DataSource = null;
			if (((UltraGridBase)CmbPaymentTermGdUtilize).ActiveRow != null)
			{
				int paymenttermid = Conversion.ToInt(CmbPaymentTermGdUtilize.Value);
				if (paymenttermid == 1)
				{
					FinancialInstrumentBindAdvanceUtilize(paymenttermid);
				}
				double num = ((dtPaymentTerm == null || dtPaymentTerm.Rows.Count == 0) ? 0.0 : (from r in dtPaymentTerm.AsEnumerable()
					where r.Field<int?>("PaymentTermId") == paymenttermid
					select r).Sum((DataRow r) => Conversion.ToDouble((object)r.Field<double>("FcyAmount"))));
				double UtilizeAmount = ((dtAdvanceUtilize == null || dtAdvanceUtilize.Rows.Count == 0) ? 0.0 : (from r in dtAdvanceUtilize.AsEnumerable()
					where r.Field<int?>("PaymentTermId") == paymenttermid
					select r).Sum((DataRow r) => Conversion.ToDouble((object)r.Field<double>("UtilizeAmount"))));
				txtAdvanceUtlize.Text = (num - UtilizeAmount).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbGdNoAdvanceUtilize_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbGdNoAdvanceUtilize).ActiveRow != null)
			{
				double num = Conversion.ToDouble(((UltraDropDownBase)CmbGdNoAdvanceUtilize).SelectedRow.Cells["GDValue"].Value);
				int gdId = Conversion.ToInt(CmbGdNoAdvanceUtilize.Value);
				double utilizedInGrid = ((dtAdvanceUtilize == null || dtAdvanceUtilize.Rows.Count == 0) ? 0.0 : (from r in dtAdvanceUtilize.AsEnumerable()
					where r.Field<int?>("GDId") == gdId
					select r).Sum((DataRow r) => r.Field<double?>("UtilizeAmount").GetValueOrDefault()));
				double GdBalance = num - utilizedInGrid;
				txtGdValueAdvanceUtlize.Text = Conversion.ToString((object)GdBalance);
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

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 214;
			obj.InvoiceId = Conversion.ToInt(CmbInvoiceInvoiceAdvanceHistory.Value);
			if (datFromDateAdvanceHistory.Checked)
			{
				obj.FromDate = datFromDateAdvanceHistory.Value;
			}
			if (datToDateAdvanceHistory.Checked)
			{
				obj.ToDate = datToDateAdvanceHistory.Value;
			}
			DataTable dt = ExImInvoice.AdvanceUtilizeagainstCustomInvoicesAndGDs_FormHistory(obj);
			if (dt != null && dt.Rows.Count > 0)
			{
				DataTable dthis = new DataTable();
				dthis.Columns.Add("Id", typeof(int));
				dthis.Columns.Add("InvoiceId", typeof(int));
				dthis.Columns.Add("InvoiceNo");
				dthis.Columns.Add("GDId", typeof(int));
				dthis.Columns.Add("GDNo");
				dthis.Columns.Add("PaymentTermId", typeof(int));
				dthis.Columns.Add("PaymentTerm", typeof(string));
				dthis.Columns.Add("DocumentTypeId", typeof(int));
				dthis.Columns.Add("FIId", typeof(int));
				dthis.Columns.Add("FINo");
				dthis.Columns.Add("UtilizeAmount", typeof(double));
				dthis.Columns.Add("EntryDate", typeof(DateTime));
				dthis.Columns.Add("EntryUser", typeof(string));
				dthis.Columns.Add("ModifyDate", typeof(DateTime));
				dthis.Columns.Add("ModifyUser", typeof(string));
				dthis.Columns.Add("FIDocumentTypeId", typeof(int));
				foreach (DataRow dr in dt.Rows)
				{
					dthis.Rows.Add(dr["Id"], dr["ExImInvoiceId"], dr["InvoiceNo"], dr["GDId"], dr["GDNO"], dr["PaymentTermId"], dr["PaymentTerm"], dr["RefDocumentTypeId"], dr["FIId"], dr["FINo"], dr["UtilizeAmount"], dr["EntryDate"], dr["EntryUser"], dr["ModifyDate"], dr["ModifyUser"], dr["DocumentTypeId"]);
				}
				grdAdvanceHistory.DataSource = dthis;
				grdAdvanceHistory.RetrieveStructure();
				grdAdvanceHistory.RootTable.HeaderLines = 2;
				grdAdvanceHistory.RootTable.Columns["Id"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["GDId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["InvoiceId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["FIId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["PaymentTermId"].Visible = false;
				grdAdvanceHistory.RootTable.Columns["FIDocumentTypeId"].Visible = false;
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

	private void frmGdBreakUpByInvoiceNew_Load(object sender, EventArgs e)
	{
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
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Expected O, but got Unknown
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Expected O, but got Unknown
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a17: Expected O, but got Unknown
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a22: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1c: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Expected O, but got Unknown
		//IL_119c: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a6: Expected O, but got Unknown
		//IL_370f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3719: Expected O, but got Unknown
		//IL_46cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_46d7: Expected O, but got Unknown
		GridEXLayout grdGdBreakUp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance14 = new Appearance();
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
		Appearance appearance37 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		GridEXLayout grdGDhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		GridEXLayout grdpaymentdetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdAdvanceUtlize_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance224 = new Appearance();
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
		Appearance appearance235 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		GridEXLayout grdAdvanceHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
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
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
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
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
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
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance205 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmGdBreakUpByInvoiceNew));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabGdBreakUpForm = new System.Windows.Forms.TabPage();
		this.grdGdBreakUp = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel30 = new System.Windows.Forms.Panel();
		this.txtFttAmountGdBreakUp = new System.Windows.Forms.TextBox();
		this.txtNetAmountGdBreakUp = new System.Windows.Forms.TextBox();
		this.txtFTTPrcntGdBreakUp = new System.Windows.Forms.TextBox();
		this.txtFobValueGdBreakUp = new System.Windows.Forms.TextBox();
		this.txtExchangeRateGdBreakUp = new System.Windows.Forms.TextBox();
		this.txtFreightGdBreakUp = new System.Windows.Forms.TextBox();
		this.txtDueDaysGdBreakUp = new System.Windows.Forms.TextBox();
		this.txtGDValueGdBreakUp = new System.Windows.Forms.TextBox();
		this.txtRemarksGdBreakUp = new System.Windows.Forms.TextBox();
		this.label121 = new System.Windows.Forms.Label();
		this.label120 = new System.Windows.Forms.Label();
		this.label119 = new System.Windows.Forms.Label();
		this.txtInvoiceBalanceGdBreakUp = new System.Windows.Forms.TextBox();
		this.label118 = new System.Windows.Forms.Label();
		this.label117 = new System.Windows.Forms.Label();
		this.label116 = new System.Windows.Forms.Label();
		this.label110 = new System.Windows.Forms.Label();
		this.label106 = new System.Windows.Forms.Label();
		this.CmbBankNameGdBreakUp = new UltraCombo();
		this.label105 = new System.Windows.Forms.Label();
		this.label107 = new System.Windows.Forms.Label();
		this.btnAddGDBreakUpDetail = new System.Windows.Forms.Button();
		this.btnUpdateGDBreakUpDetail = new System.Windows.Forms.Button();
		this.btnCancelGDBreakUpDetail = new System.Windows.Forms.Button();
		this.txtGDNoGdBreakUp = new System.Windows.Forms.TextBox();
		this.label102 = new System.Windows.Forms.Label();
		this.txtBankInvoiceNoGDBreakUp = new System.Windows.Forms.TextBox();
		this.CmbInvoiceGdBreakUp = new UltraCombo();
		this.label100 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.datGDDateGdBreakUp = new System.Windows.Forms.DateTimePicker();
		this.datInvoiceDateGdBreakUp = new System.Windows.Forms.DateTimePicker();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label114 = new System.Windows.Forms.Label();
		this.toolStripGDBreakUpForm = new System.Windows.Forms.ToolStrip();
		this.BtnNewGdBreakUp = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveGdBreakUp = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdateGDBreakUp = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshGdBreakUp = new System.Windows.Forms.ToolStripButton();
		this.btnCustomInvoice = new System.Windows.Forms.ToolStripButton();
		this.btnFIOpening = new System.Windows.Forms.ToolStripButton();
		this.TabGdBreakUpHistory = new System.Windows.Forms.TabPage();
		this.panel43 = new System.Windows.Forms.Panel();
		this.panel44 = new System.Windows.Forms.Panel();
		this.grdGDhistory = new GridEX();
		this.panel34 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label104 = new System.Windows.Forms.Label();
		this.panel42 = new System.Windows.Forms.Panel();
		this.btnShowGdBreakUpHistory = new System.Windows.Forms.Button();
		this.CmbBankGdBankHistory = new UltraCombo();
		this.FromDateGdBreakupHistory = new System.Windows.Forms.DateTimePicker();
		this.label143 = new System.Windows.Forms.Label();
		this.label147 = new System.Windows.Forms.Label();
		this.ToDateGdBreakupHistory = new System.Windows.Forms.DateTimePicker();
		this.label144 = new System.Windows.Forms.Label();
		this.toolStripGdBreakupHistory = new System.Windows.Forms.ToolStrip();
		this.btnResetGdBreakupHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshGdBreakupHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdpaymentdetail = new GridEX();
		this.grdAdvanceUtlize = new GridEX();
		this.panel33 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel32 = new System.Windows.Forms.Panel();
		this.CmbPaymentTermGdUtilize = new UltraCombo();
		this.label76 = new System.Windows.Forms.Label();
		this.CmbInvoiceAdvanceUtilize = new UltraCombo();
		this.label103 = new System.Windows.Forms.Label();
		this.CmbFIAdvanceUtlize = new UltraCombo();
		this.label108 = new System.Windows.Forms.Label();
		this.txtGdValueAdvanceUtlize = new System.Windows.Forms.TextBox();
		this.label109 = new System.Windows.Forms.Label();
		this.btnAddAdvanceAgaintGdDetail = new System.Windows.Forms.Button();
		this.btnUpdateAdvanceAgaintGdDetail = new System.Windows.Forms.Button();
		this.label111 = new System.Windows.Forms.Label();
		this.btnCancelAdvanceAgaintGdDetail = new System.Windows.Forms.Button();
		this.txtFIBalanceAdvanceUtlize = new System.Windows.Forms.TextBox();
		this.label112 = new System.Windows.Forms.Label();
		this.txtAdvanceUtlize = new System.Windows.Forms.TextBox();
		this.CmbGdNoAdvanceUtilize = new UltraCombo();
		this.label115 = new System.Windows.Forms.Label();
		this.panel29 = new System.Windows.Forms.Panel();
		this.label113 = new System.Windows.Forms.Label();
		this.toolStripAdvanceAgaintGD = new System.Windows.Forms.ToolStrip();
		this.BtnNewAdvanceAgaintGD = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshAdvanceAgaintGD = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAdvanceAgaintGD = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdateAdvanceAgaintGD = new System.Windows.Forms.ToolStripButton();
		this.FiOpening = new System.Windows.Forms.ToolStripButton();
		this.tabPage8 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdAdvanceHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.btnShowAdvanceHistory = new System.Windows.Forms.Button();
		this.CmbInvoiceInvoiceAdvanceHistory = new UltraCombo();
		this.datFromDateAdvanceHistory = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.datToDateAdvanceHistory = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
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
		this.tabControlMain = new System.Windows.Forms.TabControl();
		this.tabGdBreakUp = new System.Windows.Forms.TabPage();
		this.tabAdvanceByGd = new System.Windows.Forms.TabPage();
		this.tabControl1.SuspendLayout();
		this.tabGdBreakUpForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGdBreakUp).BeginInit();
		this.panel1.SuspendLayout();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankNameGdBreakUp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceGdBreakUp).BeginInit();
		this.panel25.SuspendLayout();
		this.toolStripGDBreakUpForm.SuspendLayout();
		this.TabGdBreakUpHistory.SuspendLayout();
		this.panel43.SuspendLayout();
		this.panel44.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGDhistory).BeginInit();
		this.panel34.SuspendLayout();
		this.panel42.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankGdBankHistory).BeginInit();
		this.toolStripGdBreakupHistory.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdAdvanceUtlize).BeginInit();
		this.panel33.SuspendLayout();
		this.panel32.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermGdUtilize).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceAdvanceUtilize).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFIAdvanceUtlize).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGdNoAdvanceUtilize).BeginInit();
		this.panel29.SuspendLayout();
		this.toolStripAdvanceAgaintGD.SuspendLayout();
		this.tabPage8.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAdvanceHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceInvoiceAdvanceHistory).BeginInit();
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
		this.tabControlMain.SuspendLayout();
		this.tabGdBreakUp.SuspendLayout();
		this.tabAdvanceByGd.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabGdBreakUpForm);
		this.tabControl1.Controls.Add(this.TabGdBreakUpHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(3, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1092, 529);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabGdBreakUpForm.Controls.Add((System.Windows.Forms.Control)(object)this.grdGdBreakUp);
		this.tabGdBreakUpForm.Controls.Add(this.panel1);
		this.tabGdBreakUpForm.Controls.Add(this.panel30);
		this.tabGdBreakUpForm.Controls.Add(this.panel25);
		this.tabGdBreakUpForm.Controls.Add(this.toolStripGDBreakUpForm);
		this.tabGdBreakUpForm.Location = new System.Drawing.Point(4, 4);
		this.tabGdBreakUpForm.Name = "tabGdBreakUpForm";
		this.tabGdBreakUpForm.Size = new System.Drawing.Size(1084, 500);
		this.tabGdBreakUpForm.TabIndex = 2;
		this.tabGdBreakUpForm.Text = "Form";
		this.tabGdBreakUpForm.UseVisualStyleBackColor = true;
		this.grdGdBreakUp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGdBreakUp).BackColor = System.Drawing.Color.White;
		grdGdBreakUp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGdBreakUp.DesignTimeLayout = grdGdBreakUp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGdBreakUp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Location = new System.Drawing.Point(0, 184);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Name = "grdGdBreakUp";
		((ScrollableBase)this.grdGdBreakUp).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdGdBreakUp).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdGdBreakUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Size = new System.Drawing.Size(1084, 316);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).TabIndex = 0;
		this.grdGdBreakUp.TotalRow = (InheritableBoolean)1;
		this.grdGdBreakUp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGdBreakUp).VisualStyle = (VisualStyle)4;
		this.grdGdBreakUp.ColumnButtonClick += new ColumnActionEventHandler(grdGdBreakUp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).DoubleClick += new System.EventHandler(grdGdBreakUp_DoubleClick);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.ctrlGrdBar5);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 161);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1084, 23);
		this.panel1.TabIndex = 605;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(53, 18);
		this.label1.TabIndex = 29;
		this.label1.Text = "Detail";
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1032, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.grdGdBreakUp;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(48, 23);
		this.ctrlGrdBar5.TabIndex = 27;
		this.panel30.Controls.Add(this.txtFttAmountGdBreakUp);
		this.panel30.Controls.Add(this.txtNetAmountGdBreakUp);
		this.panel30.Controls.Add(this.txtFTTPrcntGdBreakUp);
		this.panel30.Controls.Add(this.txtFobValueGdBreakUp);
		this.panel30.Controls.Add(this.txtExchangeRateGdBreakUp);
		this.panel30.Controls.Add(this.txtFreightGdBreakUp);
		this.panel30.Controls.Add(this.txtDueDaysGdBreakUp);
		this.panel30.Controls.Add(this.txtGDValueGdBreakUp);
		this.panel30.Controls.Add(this.txtRemarksGdBreakUp);
		this.panel30.Controls.Add(this.label121);
		this.panel30.Controls.Add(this.label120);
		this.panel30.Controls.Add(this.label119);
		this.panel30.Controls.Add(this.txtInvoiceBalanceGdBreakUp);
		this.panel30.Controls.Add(this.label118);
		this.panel30.Controls.Add(this.label117);
		this.panel30.Controls.Add(this.label116);
		this.panel30.Controls.Add(this.label110);
		this.panel30.Controls.Add(this.label106);
		this.panel30.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankNameGdBreakUp);
		this.panel30.Controls.Add(this.label105);
		this.panel30.Controls.Add(this.label107);
		this.panel30.Controls.Add(this.btnAddGDBreakUpDetail);
		this.panel30.Controls.Add(this.btnUpdateGDBreakUpDetail);
		this.panel30.Controls.Add(this.btnCancelGDBreakUpDetail);
		this.panel30.Controls.Add(this.txtGDNoGdBreakUp);
		this.panel30.Controls.Add(this.label102);
		this.panel30.Controls.Add(this.txtBankInvoiceNoGDBreakUp);
		this.panel30.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp);
		this.panel30.Controls.Add(this.label100);
		this.panel30.Controls.Add(this.label98);
		this.panel30.Controls.Add(this.datGDDateGdBreakUp);
		this.panel30.Controls.Add(this.datInvoiceDateGdBreakUp);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 50);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1084, 111);
		this.panel30.TabIndex = 0;
		this.txtFttAmountGdBreakUp.Enabled = false;
		this.txtFttAmountGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFttAmountGdBreakUp.Location = new System.Drawing.Point(831, 59);
		this.txtFttAmountGdBreakUp.Name = "txtFttAmountGdBreakUp";
		this.txtFttAmountGdBreakUp.Size = new System.Drawing.Size(120, 23);
		this.txtFttAmountGdBreakUp.TabIndex = 14;
		this.txtFttAmountGdBreakUp.Tag = "double";
		this.txtFttAmountGdBreakUp.Text = "0";
		this.txtFttAmountGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFttAmountGdBreakUp.TextChanged += new System.EventHandler(txtFttAmount_TextChanged);
		this.txtNetAmountGdBreakUp.Enabled = false;
		this.txtNetAmountGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmountGdBreakUp.Location = new System.Drawing.Point(831, 84);
		this.txtNetAmountGdBreakUp.Name = "txtNetAmountGdBreakUp";
		this.txtNetAmountGdBreakUp.Size = new System.Drawing.Size(120, 23);
		this.txtNetAmountGdBreakUp.TabIndex = 15;
		this.txtNetAmountGdBreakUp.Tag = "double";
		this.txtNetAmountGdBreakUp.Text = "0";
		this.txtNetAmountGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFTTPrcntGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFTTPrcntGdBreakUp.Location = new System.Drawing.Point(831, 34);
		this.txtFTTPrcntGdBreakUp.Name = "txtFTTPrcntGdBreakUp";
		this.txtFTTPrcntGdBreakUp.Size = new System.Drawing.Size(120, 23);
		this.txtFTTPrcntGdBreakUp.TabIndex = 13;
		this.txtFTTPrcntGdBreakUp.Tag = "double";
		this.txtFTTPrcntGdBreakUp.Text = "0";
		this.txtFTTPrcntGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFTTPrcntGdBreakUp.TextChanged += new System.EventHandler(txtFTTPrcnt_TextChanged);
		this.txtFobValueGdBreakUp.Enabled = false;
		this.txtFobValueGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFobValueGdBreakUp.Location = new System.Drawing.Point(831, 8);
		this.txtFobValueGdBreakUp.Name = "txtFobValueGdBreakUp";
		this.txtFobValueGdBreakUp.Size = new System.Drawing.Size(120, 23);
		this.txtFobValueGdBreakUp.TabIndex = 12;
		this.txtFobValueGdBreakUp.Tag = "double";
		this.txtFobValueGdBreakUp.Text = "0";
		this.txtFobValueGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFobValueGdBreakUp.TextChanged += new System.EventHandler(txtFobValue_TextChanged);
		this.txtExchangeRateGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRateGdBreakUp.Location = new System.Drawing.Point(499, 59);
		this.txtExchangeRateGdBreakUp.Name = "txtExchangeRateGdBreakUp";
		this.txtExchangeRateGdBreakUp.Size = new System.Drawing.Size(80, 23);
		this.txtExchangeRateGdBreakUp.TabIndex = 10;
		this.txtExchangeRateGdBreakUp.Tag = "double";
		this.txtExchangeRateGdBreakUp.Text = "0";
		this.txtExchangeRateGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightGdBreakUp.Location = new System.Drawing.Point(643, 59);
		this.txtFreightGdBreakUp.Name = "txtFreightGdBreakUp";
		this.txtFreightGdBreakUp.Size = new System.Drawing.Size(109, 23);
		this.txtFreightGdBreakUp.TabIndex = 11;
		this.txtFreightGdBreakUp.Tag = "double";
		this.txtFreightGdBreakUp.Text = "0";
		this.txtFreightGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightGdBreakUp.TextChanged += new System.EventHandler(txtFreight_TextChanged);
		this.txtDueDaysGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDaysGdBreakUp.Location = new System.Drawing.Point(499, 34);
		this.txtDueDaysGdBreakUp.Name = "txtDueDaysGdBreakUp";
		this.txtDueDaysGdBreakUp.Size = new System.Drawing.Size(80, 23);
		this.txtDueDaysGdBreakUp.TabIndex = 8;
		this.txtDueDaysGdBreakUp.Tag = "int";
		this.txtDueDaysGdBreakUp.Text = "0";
		this.txtDueDaysGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGDValueGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGDValueGdBreakUp.Location = new System.Drawing.Point(643, 34);
		this.txtGDValueGdBreakUp.Name = "txtGDValueGdBreakUp";
		this.txtGDValueGdBreakUp.Size = new System.Drawing.Size(109, 23);
		this.txtGDValueGdBreakUp.TabIndex = 9;
		this.txtGDValueGdBreakUp.Tag = "double";
		this.txtGDValueGdBreakUp.Text = "0";
		this.txtGDValueGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGDValueGdBreakUp.TextChanged += new System.EventHandler(txtGDValue_TextChanged);
		this.txtRemarksGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksGdBreakUp.Location = new System.Drawing.Point(80, 84);
		this.txtRemarksGdBreakUp.Name = "txtRemarksGdBreakUp";
		this.txtRemarksGdBreakUp.Size = new System.Drawing.Size(672, 23);
		this.txtRemarksGdBreakUp.TabIndex = 6;
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label121.Location = new System.Drawing.Point(4, 88);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(52, 15);
		this.label121.TabIndex = 614;
		this.label121.Text = "Remarks";
		this.label120.AutoSize = true;
		this.label120.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label120.Location = new System.Drawing.Point(755, 63);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(74, 15);
		this.label120.TabIndex = 612;
		this.label120.Text = "FTT Amount";
		this.label119.AutoSize = true;
		this.label119.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label119.Location = new System.Drawing.Point(422, 63);
		this.label119.Name = "label119";
		this.label119.Size = new System.Drawing.Size(58, 15);
		this.label119.TabIndex = 610;
		this.label119.Text = "Exch Rate";
		this.txtInvoiceBalanceGdBreakUp.Enabled = false;
		this.txtInvoiceBalanceGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceBalanceGdBreakUp.Location = new System.Drawing.Point(308, 8);
		this.txtInvoiceBalanceGdBreakUp.Name = "txtInvoiceBalanceGdBreakUp";
		this.txtInvoiceBalanceGdBreakUp.Size = new System.Drawing.Size(110, 23);
		this.txtInvoiceBalanceGdBreakUp.TabIndex = 1;
		this.txtInvoiceBalanceGdBreakUp.Text = "0";
		this.txtInvoiceBalanceGdBreakUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label118.AutoSize = true;
		this.label118.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label118.Location = new System.Drawing.Point(755, 88);
		this.label118.Name = "label118";
		this.label118.Size = new System.Drawing.Size(73, 15);
		this.label118.TabIndex = 608;
		this.label118.Text = "Net Amount";
		this.label117.AutoSize = true;
		this.label117.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label117.Location = new System.Drawing.Point(755, 38);
		this.label117.Name = "label117";
		this.label117.Size = new System.Drawing.Size(40, 15);
		this.label117.TabIndex = 606;
		this.label117.Text = "FTT %";
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label116.Location = new System.Drawing.Point(755, 12);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(61, 15);
		this.label116.TabIndex = 604;
		this.label116.Text = "FOB Value";
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label110.Location = new System.Drawing.Point(585, 63);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(44, 15);
		this.label110.TabIndex = 602;
		this.label110.Text = "Freight";
		this.label106.AutoSize = true;
		this.label106.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label106.Location = new System.Drawing.Point(422, 38);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(58, 15);
		this.label106.TabIndex = 600;
		this.label106.Text = "Due Days";
		this.CmbBankNameGdBreakUp.AllowNull = (DefaultableBoolean)1;
		this.CmbBankNameGdBreakUp.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankNameGdBreakUp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankNameGdBreakUp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankNameGdBreakUp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankNameGdBreakUp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankNameGdBreakUp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankNameGdBreakUp).Location = new System.Drawing.Point(499, 6);
		((System.Windows.Forms.Control)(object)this.CmbBankNameGdBreakUp).Name = "CmbBankNameGdBreakUp";
		((System.Windows.Forms.Control)(object)this.CmbBankNameGdBreakUp).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankNameGdBreakUp).TabIndex = 7;
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label105.Location = new System.Drawing.Point(422, 12);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(68, 15);
		this.label105.TabIndex = 598;
		this.label105.Text = "Bank Name";
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label107.Location = new System.Drawing.Point(585, 38);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(56, 15);
		this.label107.TabIndex = 513;
		this.label107.Text = "GD Value";
		this.btnAddGDBreakUpDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddGDBreakUpDetail.FlatAppearance.BorderSize = 0;
		this.btnAddGDBreakUpDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddGDBreakUpDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddGDBreakUpDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddGDBreakUpDetail.Location = new System.Drawing.Point(953, 84);
		this.btnAddGDBreakUpDetail.Name = "btnAddGDBreakUpDetail";
		this.btnAddGDBreakUpDetail.Size = new System.Drawing.Size(31, 23);
		this.btnAddGDBreakUpDetail.TabIndex = 17;
		this.btnAddGDBreakUpDetail.Text = "+";
		this.btnAddGDBreakUpDetail.UseVisualStyleBackColor = false;
		this.btnAddGDBreakUpDetail.Click += new System.EventHandler(btnAddGDBreakUpDetail_Click);
		this.btnUpdateGDBreakUpDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateGDBreakUpDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateGDBreakUpDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateGDBreakUpDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateGDBreakUpDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateGDBreakUpDetail.Location = new System.Drawing.Point(953, 84);
		this.btnUpdateGDBreakUpDetail.Name = "btnUpdateGDBreakUpDetail";
		this.btnUpdateGDBreakUpDetail.Size = new System.Drawing.Size(63, 23);
		this.btnUpdateGDBreakUpDetail.TabIndex = 25;
		this.btnUpdateGDBreakUpDetail.Text = "Update";
		this.btnUpdateGDBreakUpDetail.UseVisualStyleBackColor = false;
		this.btnUpdateGDBreakUpDetail.Visible = false;
		this.btnUpdateGDBreakUpDetail.Click += new System.EventHandler(btnUpdateGDBreakUpDetail_Click);
		this.btnCancelGDBreakUpDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelGDBreakUpDetail.FlatAppearance.BorderSize = 0;
		this.btnCancelGDBreakUpDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelGDBreakUpDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelGDBreakUpDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelGDBreakUpDetail.Location = new System.Drawing.Point(953, 59);
		this.btnCancelGDBreakUpDetail.Name = "btnCancelGDBreakUpDetail";
		this.btnCancelGDBreakUpDetail.Size = new System.Drawing.Size(63, 23);
		this.btnCancelGDBreakUpDetail.TabIndex = 16;
		this.btnCancelGDBreakUpDetail.Text = "Cancel";
		this.btnCancelGDBreakUpDetail.UseVisualStyleBackColor = false;
		this.btnCancelGDBreakUpDetail.Visible = false;
		this.btnCancelGDBreakUpDetail.Click += new System.EventHandler(btnCancelGDBreakUpDetail_Click);
		this.txtGDNoGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGDNoGdBreakUp.Location = new System.Drawing.Point(80, 34);
		this.txtGDNoGdBreakUp.Name = "txtGDNoGdBreakUp";
		this.txtGDNoGdBreakUp.Size = new System.Drawing.Size(227, 23);
		this.txtGDNoGdBreakUp.TabIndex = 2;
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.Location = new System.Drawing.Point(4, 38);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(71, 15);
		this.label102.TabIndex = 509;
		this.label102.Text = "GD No,Date";
		this.txtBankInvoiceNoGDBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBankInvoiceNoGDBreakUp.Location = new System.Drawing.Point(80, 59);
		this.txtBankInvoiceNoGDBreakUp.Name = "txtBankInvoiceNoGDBreakUp";
		this.txtBankInvoiceNoGDBreakUp.Size = new System.Drawing.Size(227, 23);
		this.txtBankInvoiceNoGDBreakUp.TabIndex = 4;
		this.CmbInvoiceGdBreakUp.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceGdBreakUp.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceGdBreakUp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceGdBreakUp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceGdBreakUp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceGdBreakUp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).Location = new System.Drawing.Point(80, 6);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).Name = "CmbInvoiceGdBreakUp";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceGdBreakUp).TextChanged += new System.EventHandler(CmbInvoiceGdBreakUp_TextChanged);
		this.label100.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.Location = new System.Drawing.Point(4, 55);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(76, 30);
		this.label100.TabIndex = 509;
		this.label100.Text = "Bank Invoice No && Date";
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label98.Location = new System.Drawing.Point(4, 6);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(65, 30);
		this.label98.TabIndex = 595;
		this.label98.Text = "Custom \r\nInvoice No";
		this.datGDDateGdBreakUp.CustomFormat = "dd-MMM-yy";
		this.datGDDateGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datGDDateGdBreakUp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datGDDateGdBreakUp.Location = new System.Drawing.Point(308, 34);
		this.datGDDateGdBreakUp.Name = "datGDDateGdBreakUp";
		this.datGDDateGdBreakUp.Size = new System.Drawing.Size(110, 23);
		this.datGDDateGdBreakUp.TabIndex = 3;
		this.datInvoiceDateGdBreakUp.CustomFormat = "dd-MMM-yy";
		this.datInvoiceDateGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datInvoiceDateGdBreakUp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datInvoiceDateGdBreakUp.Location = new System.Drawing.Point(308, 59);
		this.datInvoiceDateGdBreakUp.Name = "datInvoiceDateGdBreakUp";
		this.datInvoiceDateGdBreakUp.Size = new System.Drawing.Size(110, 23);
		this.datInvoiceDateGdBreakUp.TabIndex = 5;
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.label114);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 27);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1084, 23);
		this.panel25.TabIndex = 5;
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label114.ForeColor = System.Drawing.Color.White;
		this.label114.Location = new System.Drawing.Point(3, 2);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(396, 18);
		this.label114.TabIndex = 29;
		this.label114.Text = "Goods Declaration (GD) / Bank Invoice & GD Mapping";
		this.toolStripGDBreakUpForm.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripGDBreakUpForm.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStripGDBreakUpForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.BtnNewGdBreakUp, this.BtnSaveGdBreakUp, this.BtnUpdateGDBreakUp, this.BtnRefreshGdBreakUp, this.btnCustomInvoice, this.btnFIOpening });
		this.toolStripGDBreakUpForm.Location = new System.Drawing.Point(0, 0);
		this.toolStripGDBreakUpForm.Name = "toolStripGDBreakUpForm";
		this.toolStripGDBreakUpForm.Size = new System.Drawing.Size(1084, 27);
		this.toolStripGDBreakUpForm.TabIndex = 5;
		this.toolStripGDBreakUpForm.Text = "toolStrip1";
		this.BtnNewGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewGdBreakUp.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewGdBreakUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewGdBreakUp.Name = "BtnNewGdBreakUp";
		this.BtnNewGdBreakUp.Size = new System.Drawing.Size(59, 24);
		this.BtnNewGdBreakUp.Text = "&New";
		this.BtnNewGdBreakUp.Click += new System.EventHandler(BtnNewGdBreakUp_Click);
		this.BtnSaveGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveGdBreakUp.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveGdBreakUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveGdBreakUp.Name = "BtnSaveGdBreakUp";
		this.BtnSaveGdBreakUp.Size = new System.Drawing.Size(64, 24);
		this.BtnSaveGdBreakUp.Text = "&Save";
		this.BtnSaveGdBreakUp.Click += new System.EventHandler(BtnSaveGdBreakUp_Click);
		this.BtnUpdateGDBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateGDBreakUp.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateGDBreakUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdateGDBreakUp.Name = "BtnUpdateGDBreakUp";
		this.BtnUpdateGDBreakUp.Size = new System.Drawing.Size(78, 24);
		this.BtnUpdateGDBreakUp.Text = "&Update";
		this.BtnUpdateGDBreakUp.Visible = false;
		this.BtnRefreshGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshGdBreakUp.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshGdBreakUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshGdBreakUp.Name = "BtnRefreshGdBreakUp";
		this.BtnRefreshGdBreakUp.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshGdBreakUp.Text = "&Refresh";
		this.BtnRefreshGdBreakUp.Click += new System.EventHandler(BtnRefreshGdBreakUp_Click);
		this.btnCustomInvoice.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnCustomInvoice.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCustomInvoice.Name = "btnCustomInvoice";
		this.btnCustomInvoice.Size = new System.Drawing.Size(120, 24);
		this.btnCustomInvoice.Text = "Custom Invoice";
		this.btnCustomInvoice.Click += new System.EventHandler(btnCustomInvoice_Click);
		this.btnFIOpening.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnFIOpening.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFIOpening.Name = "btnFIOpening";
		this.btnFIOpening.Size = new System.Drawing.Size(311, 24);
		this.btnFIOpening.Text = "FI Opening Balances (Balance Advance Payment)";
		this.btnFIOpening.Click += new System.EventHandler(btnFIOpening_Click);
		this.TabGdBreakUpHistory.Controls.Add(this.panel43);
		this.TabGdBreakUpHistory.Controls.Add(this.panel42);
		this.TabGdBreakUpHistory.Controls.Add(this.toolStripGdBreakupHistory);
		this.TabGdBreakUpHistory.Location = new System.Drawing.Point(4, 4);
		this.TabGdBreakUpHistory.Name = "TabGdBreakUpHistory";
		this.TabGdBreakUpHistory.Size = new System.Drawing.Size(1181, 500);
		this.TabGdBreakUpHistory.TabIndex = 3;
		this.TabGdBreakUpHistory.Text = "History";
		this.TabGdBreakUpHistory.UseVisualStyleBackColor = true;
		this.panel43.Controls.Add(this.panel44);
		this.panel43.Controls.Add(this.panel34);
		this.panel43.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel43.Location = new System.Drawing.Point(0, 61);
		this.panel43.Name = "panel43";
		this.panel43.Size = new System.Drawing.Size(1181, 439);
		this.panel43.TabIndex = 49;
		this.panel44.Controls.Add((System.Windows.Forms.Control)(object)this.grdGDhistory);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel44.Location = new System.Drawing.Point(0, 24);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(1181, 415);
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
		((System.Windows.Forms.Control)(object)this.grdGDhistory).Size = new System.Drawing.Size(1181, 415);
		((System.Windows.Forms.Control)(object)this.grdGDhistory).TabIndex = 0;
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
		this.panel34.Size = new System.Drawing.Size(1181, 24);
		this.panel34.TabIndex = 45;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1132, 0);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdGDhistory;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar7.TabIndex = 45;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.ForeColor = System.Drawing.Color.White;
		this.label104.Location = new System.Drawing.Point(7, 3);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(496, 19);
		this.label104.TabIndex = 1;
		this.label104.Text = "Goods Declaration (GD) / Bank Invoice & GD Mapping History";
		this.panel42.Controls.Add(this.btnShowGdBreakUpHistory);
		this.panel42.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory);
		this.panel42.Controls.Add(this.FromDateGdBreakupHistory);
		this.panel42.Controls.Add(this.label143);
		this.panel42.Controls.Add(this.label147);
		this.panel42.Controls.Add(this.ToDateGdBreakupHistory);
		this.panel42.Controls.Add(this.label144);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 27);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(1181, 34);
		this.panel42.TabIndex = 0;
		this.btnShowGdBreakUpHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowGdBreakUpHistory.FlatAppearance.BorderSize = 0;
		this.btnShowGdBreakUpHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowGdBreakUpHistory.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnShowGdBreakUpHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowGdBreakUpHistory.Location = new System.Drawing.Point(801, 4);
		this.btnShowGdBreakUpHistory.Name = "btnShowGdBreakUpHistory";
		this.btnShowGdBreakUpHistory.Size = new System.Drawing.Size(57, 24);
		this.btnShowGdBreakUpHistory.TabIndex = 3;
		this.btnShowGdBreakUpHistory.Text = "Show";
		this.btnShowGdBreakUpHistory.UseVisualStyleBackColor = false;
		this.btnShowGdBreakUpHistory.Click += new System.EventHandler(btnShowGdBreakUpHistory_Click);
		this.CmbBankGdBankHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBankGdBankHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankGdBankHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankGdBankHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance170;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance207).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankGdBankHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankGdBankHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).Location = new System.Drawing.Point(498, 3);
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).Name = "CmbBankGdBankHistory";
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).Size = new System.Drawing.Size(300, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankGdBankHistory).TabIndex = 2;
		this.FromDateGdBreakupHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateGdBreakupHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateGdBreakupHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateGdBreakupHistory.Location = new System.Drawing.Point(68, 5);
		this.FromDateGdBreakupHistory.Name = "FromDateGdBreakupHistory";
		this.FromDateGdBreakupHistory.ShowCheckBox = true;
		this.FromDateGdBreakupHistory.Size = new System.Drawing.Size(150, 23);
		this.FromDateGdBreakupHistory.TabIndex = 0;
		this.label143.AutoSize = true;
		this.label143.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label143.Location = new System.Drawing.Point(426, 9);
		this.label143.Name = "label143";
		this.label143.Size = new System.Drawing.Size(68, 15);
		this.label143.TabIndex = 12;
		this.label143.Text = "Bank Name";
		this.label147.AutoSize = true;
		this.label147.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label147.Location = new System.Drawing.Point(5, 9);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(63, 15);
		this.label147.TabIndex = 5;
		this.label147.Text = "From Date";
		this.ToDateGdBreakupHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateGdBreakupHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateGdBreakupHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateGdBreakupHistory.Location = new System.Drawing.Point(272, 5);
		this.ToDateGdBreakupHistory.Name = "ToDateGdBreakupHistory";
		this.ToDateGdBreakupHistory.ShowCheckBox = true;
		this.ToDateGdBreakupHistory.Size = new System.Drawing.Size(150, 23);
		this.ToDateGdBreakupHistory.TabIndex = 1;
		this.label144.AutoSize = true;
		this.label144.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label144.Location = new System.Drawing.Point(221, 9);
		this.label144.Name = "label144";
		this.label144.Size = new System.Drawing.Size(48, 15);
		this.label144.TabIndex = 7;
		this.label144.Text = "To Date";
		this.toolStripGdBreakupHistory.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripGdBreakupHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStripGdBreakupHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetGdBreakupHistory, this.btnRefreshGdBreakupHistory });
		this.toolStripGdBreakupHistory.Location = new System.Drawing.Point(0, 0);
		this.toolStripGdBreakupHistory.Name = "toolStripGdBreakupHistory";
		this.toolStripGdBreakupHistory.Size = new System.Drawing.Size(1181, 27);
		this.toolStripGdBreakupHistory.TabIndex = 47;
		this.toolStripGdBreakupHistory.Text = "toolStrip7";
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
		this.tabControl3.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl3.Controls.Add(this.tabPage6);
		this.tabControl3.Controls.Add(this.tabPage8);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Location = new System.Drawing.Point(3, 3);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1189, 529);
		this.tabControl3.TabIndex = 9;
		this.tabControl3.SelectedIndexChanged += new System.EventHandler(tabControl3_SelectedIndexChanged);
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdpaymentdetail);
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize);
		this.tabPage6.Controls.Add(this.panel33);
		this.tabPage6.Controls.Add(this.panel32);
		this.tabPage6.Controls.Add(this.panel29);
		this.tabPage6.Location = new System.Drawing.Point(4, 4);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(1181, 503);
		this.tabPage6.TabIndex = 0;
		this.tabPage6.Text = "Form";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.grdpaymentdetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdpaymentdetail).BackColor = System.Drawing.Color.White;
		grdpaymentdetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdpaymentdetail.DesignTimeLayout = grdpaymentdetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdpaymentdetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Location = new System.Drawing.Point(652, 192);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Name = "grdpaymentdetail";
		((ScrollableBase)this.grdpaymentdetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdpaymentdetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdpaymentdetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Size = new System.Drawing.Size(526, 308);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).TabIndex = 606;
		this.grdpaymentdetail.TotalRow = (InheritableBoolean)1;
		this.grdpaymentdetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdpaymentdetail).VisualStyle = (VisualStyle)4;
		((ScrollableBase)this.grdAdvanceUtlize).BackColor = System.Drawing.Color.White;
		grdAdvanceUtlize_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAdvanceUtlize.DesignTimeLayout = grdAdvanceUtlize_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Dock = System.Windows.Forms.DockStyle.Left;
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAdvanceUtlize.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Location = new System.Drawing.Point(3, 192);
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Name = "grdAdvanceUtlize";
		((ScrollableBase)this.grdAdvanceUtlize).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAdvanceUtlize).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdAdvanceUtlize).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).Size = new System.Drawing.Size(649, 308);
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).TabIndex = 0;
		this.grdAdvanceUtlize.TotalRow = (InheritableBoolean)1;
		this.grdAdvanceUtlize.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAdvanceUtlize).VisualStyle = (VisualStyle)4;
		this.grdAdvanceUtlize.ColumnButtonClick += new ColumnActionEventHandler(grdAdvanceUtlize_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdAdvanceUtlize).DoubleClick += new System.EventHandler(grdAdvanceUtlize_DoubleClick);
		this.panel33.BackColor = System.Drawing.Color.Teal;
		this.panel33.Controls.Add(this.ctrlGrdBar6);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel33.Location = new System.Drawing.Point(3, 169);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(1175, 23);
		this.panel33.TabIndex = 605;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1135, 0);
		this.ctrlGrdBar6.MyGrid = this.grdAdvanceUtlize;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar6.TabIndex = 27;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermGdUtilize);
		this.panel32.Controls.Add(this.label76);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceAdvanceUtilize);
		this.panel32.Controls.Add(this.label103);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize);
		this.panel32.Controls.Add(this.label108);
		this.panel32.Controls.Add(this.txtGdValueAdvanceUtlize);
		this.panel32.Controls.Add(this.label109);
		this.panel32.Controls.Add(this.btnAddAdvanceAgaintGdDetail);
		this.panel32.Controls.Add(this.btnUpdateAdvanceAgaintGdDetail);
		this.panel32.Controls.Add(this.label111);
		this.panel32.Controls.Add(this.btnCancelAdvanceAgaintGdDetail);
		this.panel32.Controls.Add(this.txtFIBalanceAdvanceUtlize);
		this.panel32.Controls.Add(this.label112);
		this.panel32.Controls.Add(this.txtAdvanceUtlize);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize);
		this.panel32.Controls.Add(this.label115);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(3, 53);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1175, 116);
		this.panel32.TabIndex = 0;
		this.CmbPaymentTermGdUtilize.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermGdUtilize.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermGdUtilize.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermGdUtilize.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermGdUtilize).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermGdUtilize).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermGdUtilize).Location = new System.Drawing.Point(468, 28);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermGdUtilize).Name = "CmbPaymentTermGdUtilize";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermGdUtilize).Size = new System.Drawing.Size(193, 25);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermGdUtilize).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermGdUtilize).Leave += new System.EventHandler(CmbPaymentTermGdUtilize_Leave);
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(468, 7);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(84, 15);
		this.label76.TabIndex = 607;
		this.label76.Text = "Payment Term";
		this.CmbInvoiceAdvanceUtilize.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceAdvanceUtilize.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceAdvanceUtilize.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceAdvanceUtilize.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Appearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance239;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance244).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceAdvanceUtilize).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance248;
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
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Appearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance226;
		((SpecialBoxBase)((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance231).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance232).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFIAdvanceUtlize).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Location = new System.Drawing.Point(8, 84);
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Name = "CmbFIAdvanceUtlize";
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Size = new System.Drawing.Size(181, 25);
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).TextChanged += new System.EventHandler(CmbFIAdvanceUtlize_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbFIAdvanceUtlize).Leave += new System.EventHandler(CmbFIAdvanceUtlize_TextChanged);
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.Location = new System.Drawing.Point(9, 64);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(140, 15);
		this.label108.TabIndex = 600;
		this.label108.Text = "Financial Instruments No";
		this.txtGdValueAdvanceUtlize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdValueAdvanceUtlize.Location = new System.Drawing.Point(363, 31);
		this.txtGdValueAdvanceUtlize.Name = "txtGdValueAdvanceUtlize";
		this.txtGdValueAdvanceUtlize.ReadOnly = true;
		this.txtGdValueAdvanceUtlize.Size = new System.Drawing.Size(102, 22);
		this.txtGdValueAdvanceUtlize.TabIndex = 2;
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
		this.btnAddAdvanceAgaintGdDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddAdvanceAgaintGdDetail.FlatAppearance.BorderSize = 0;
		this.btnAddAdvanceAgaintGdDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddAdvanceAgaintGdDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddAdvanceAgaintGdDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddAdvanceAgaintGdDetail.Location = new System.Drawing.Point(398, 83);
		this.btnAddAdvanceAgaintGdDetail.Name = "btnAddAdvanceAgaintGdDetail";
		this.btnAddAdvanceAgaintGdDetail.Size = new System.Drawing.Size(31, 26);
		this.btnAddAdvanceAgaintGdDetail.TabIndex = 8;
		this.btnAddAdvanceAgaintGdDetail.Text = "+";
		this.btnAddAdvanceAgaintGdDetail.UseVisualStyleBackColor = false;
		this.btnAddAdvanceAgaintGdDetail.Click += new System.EventHandler(btnAddAdvanceUtlize_Click);
		this.btnUpdateAdvanceAgaintGdDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateAdvanceAgaintGdDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateAdvanceAgaintGdDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateAdvanceAgaintGdDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateAdvanceAgaintGdDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateAdvanceAgaintGdDetail.Location = new System.Drawing.Point(432, 83);
		this.btnUpdateAdvanceAgaintGdDetail.Name = "btnUpdateAdvanceAgaintGdDetail";
		this.btnUpdateAdvanceAgaintGdDetail.Size = new System.Drawing.Size(63, 26);
		this.btnUpdateAdvanceAgaintGdDetail.TabIndex = 9;
		this.btnUpdateAdvanceAgaintGdDetail.Text = "Update";
		this.btnUpdateAdvanceAgaintGdDetail.UseVisualStyleBackColor = false;
		this.btnUpdateAdvanceAgaintGdDetail.Visible = false;
		this.btnUpdateAdvanceAgaintGdDetail.Click += new System.EventHandler(btnUpdateAdvanceUtilize_Click);
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label111.Location = new System.Drawing.Point(192, 64);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(48, 15);
		this.label111.TabIndex = 602;
		this.label111.Text = "Balance";
		this.btnCancelAdvanceAgaintGdDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelAdvanceAgaintGdDetail.FlatAppearance.BorderSize = 0;
		this.btnCancelAdvanceAgaintGdDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelAdvanceAgaintGdDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelAdvanceAgaintGdDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelAdvanceAgaintGdDetail.Location = new System.Drawing.Point(498, 83);
		this.btnCancelAdvanceAgaintGdDetail.Name = "btnCancelAdvanceAgaintGdDetail";
		this.btnCancelAdvanceAgaintGdDetail.Size = new System.Drawing.Size(59, 26);
		this.btnCancelAdvanceAgaintGdDetail.TabIndex = 10;
		this.btnCancelAdvanceAgaintGdDetail.Text = "Cancel";
		this.btnCancelAdvanceAgaintGdDetail.UseVisualStyleBackColor = false;
		this.btnCancelAdvanceAgaintGdDetail.Visible = false;
		this.btnCancelAdvanceAgaintGdDetail.Click += new System.EventHandler(btnCancelAdvanceUtilize_Click);
		this.txtFIBalanceAdvanceUtlize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFIBalanceAdvanceUtlize.Location = new System.Drawing.Point(191, 85);
		this.txtFIBalanceAdvanceUtlize.Name = "txtFIBalanceAdvanceUtlize";
		this.txtFIBalanceAdvanceUtlize.ReadOnly = true;
		this.txtFIBalanceAdvanceUtlize.Size = new System.Drawing.Size(101, 22);
		this.txtFIBalanceAdvanceUtlize.TabIndex = 5;
		this.txtFIBalanceAdvanceUtlize.TabStop = false;
		this.txtFIBalanceAdvanceUtlize.Text = "0";
		this.txtFIBalanceAdvanceUtlize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label112.Location = new System.Drawing.Point(294, 64);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(87, 15);
		this.label112.TabIndex = 603;
		this.label112.Text = "Utilize Amount";
		this.txtAdvanceUtlize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdvanceUtlize.Location = new System.Drawing.Point(293, 85);
		this.txtAdvanceUtlize.Name = "txtAdvanceUtlize";
		this.txtAdvanceUtlize.Size = new System.Drawing.Size(101, 22);
		this.txtAdvanceUtlize.TabIndex = 6;
		this.txtAdvanceUtlize.Tag = "double";
		this.txtAdvanceUtlize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbGdNoAdvanceUtilize.AllowNull = (DefaultableBoolean)1;
		this.CmbGdNoAdvanceUtilize.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGdNoAdvanceUtilize.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGdNoAdvanceUtilize.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Appearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance211;
		((SpecialBoxBase)((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance216).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGdNoAdvanceUtilize).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbGdNoAdvanceUtilize.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Location = new System.Drawing.Point(203, 29);
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Name = "CmbGdNoAdvanceUtilize";
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Size = new System.Drawing.Size(158, 26);
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbGdNoAdvanceUtilize).Leave += new System.EventHandler(CmbGdNoAdvanceUtilize_Leave);
		this.label115.AutoSize = true;
		this.label115.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label115.Location = new System.Drawing.Point(204, 10);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(43, 15);
		this.label115.TabIndex = 595;
		this.label115.Text = "GD No";
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.label113);
		this.panel29.Controls.Add(this.toolStripAdvanceAgaintGD);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 3);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1175, 50);
		this.panel29.TabIndex = 8;
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label113.ForeColor = System.Drawing.Color.White;
		this.label113.Location = new System.Drawing.Point(3, 28);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(355, 18);
		this.label113.TabIndex = 28;
		this.label113.Text = "Bank Invoice Payment Schedule && GD Mapping";
		this.toolStripAdvanceAgaintGD.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripAdvanceAgaintGD.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStripAdvanceAgaintGD.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnNewAdvanceAgaintGD, this.BtnRefreshAdvanceAgaintGD, this.BtnSaveAdvanceAgaintGD, this.BtnUpdateAdvanceAgaintGD, this.FiOpening });
		this.toolStripAdvanceAgaintGD.Location = new System.Drawing.Point(0, 0);
		this.toolStripAdvanceAgaintGD.Name = "toolStripAdvanceAgaintGD";
		this.toolStripAdvanceAgaintGD.Size = new System.Drawing.Size(1175, 27);
		this.toolStripAdvanceAgaintGD.TabIndex = 606;
		this.toolStripAdvanceAgaintGD.Text = "toolStrip5";
		this.BtnNewAdvanceAgaintGD.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewAdvanceAgaintGD.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewAdvanceAgaintGD.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewAdvanceAgaintGD.Name = "BtnNewAdvanceAgaintGD";
		this.BtnNewAdvanceAgaintGD.Size = new System.Drawing.Size(59, 24);
		this.BtnNewAdvanceAgaintGD.Text = "&New";
		this.BtnNewAdvanceAgaintGD.Click += new System.EventHandler(BtnNewAdvanceAgaintGD_Click);
		this.BtnRefreshAdvanceAgaintGD.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshAdvanceAgaintGD.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshAdvanceAgaintGD.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshAdvanceAgaintGD.Name = "BtnRefreshAdvanceAgaintGD";
		this.BtnRefreshAdvanceAgaintGD.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshAdvanceAgaintGD.Text = "&Refresh";
		this.BtnRefreshAdvanceAgaintGD.Click += new System.EventHandler(BtnRefreshAdvanceAgaintGD_Click);
		this.BtnSaveAdvanceAgaintGD.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveAdvanceAgaintGD.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveAdvanceAgaintGD.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAdvanceAgaintGD.Name = "BtnSaveAdvanceAgaintGD";
		this.BtnSaveAdvanceAgaintGD.Size = new System.Drawing.Size(64, 24);
		this.BtnSaveAdvanceAgaintGD.Text = "&Save";
		this.BtnSaveAdvanceAgaintGD.Click += new System.EventHandler(BtnSaveAdvanceAgaintGD_Click);
		this.BtnUpdateAdvanceAgaintGD.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateAdvanceAgaintGD.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateAdvanceAgaintGD.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdateAdvanceAgaintGD.Name = "BtnUpdateAdvanceAgaintGD";
		this.BtnUpdateAdvanceAgaintGD.Size = new System.Drawing.Size(78, 24);
		this.BtnUpdateAdvanceAgaintGD.Text = "&Update";
		this.BtnUpdateAdvanceAgaintGD.Visible = false;
		this.FiOpening.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.FiOpening.Name = "FiOpening";
		this.FiOpening.Size = new System.Drawing.Size(75, 24);
		this.FiOpening.Text = "FI Opening";
		this.FiOpening.Click += new System.EventHandler(FiOpening_Click);
		this.tabPage8.Controls.Add(this.panel3);
		this.tabPage8.Controls.Add(this.panel2);
		this.tabPage8.Location = new System.Drawing.Point(4, 4);
		this.tabPage8.Name = "tabPage8";
		this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage8.Size = new System.Drawing.Size(1181, 503);
		this.tabPage8.TabIndex = 1;
		this.tabPage8.Text = "History";
		this.tabPage8.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdAdvanceHistory);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 65);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1175, 435);
		this.panel3.TabIndex = 610;
		((ScrollableBase)this.grdAdvanceHistory).BackColor = System.Drawing.Color.White;
		grdAdvanceHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAdvanceHistory.DesignTimeLayout = grdAdvanceHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAdvanceHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Name = "grdAdvanceHistory";
		((ScrollableBase)this.grdAdvanceHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAdvanceHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdAdvanceHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).Size = new System.Drawing.Size(1175, 435);
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).TabIndex = 608;
		this.grdAdvanceHistory.TotalRow = (InheritableBoolean)1;
		this.grdAdvanceHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAdvanceHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdAdvanceHistory).DoubleClick += new System.EventHandler(grdAdvanceHistory_DoubleClick);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1175, 62);
		this.panel2.TabIndex = 609;
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 34);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1175, 24);
		this.panel4.TabIndex = 47;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1126, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdAdvanceHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar1.TabIndex = 45;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(7, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(496, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "Goods Declaration (GD) / Bank Invoice & GD Mapping History";
		this.panel6.Controls.Add(this.btnShowAdvanceHistory);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceInvoiceAdvanceHistory);
		this.panel6.Controls.Add(this.datFromDateAdvanceHistory);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Controls.Add(this.datToDateAdvanceHistory);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1175, 34);
		this.panel6.TabIndex = 46;
		this.btnShowAdvanceHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowAdvanceHistory.FlatAppearance.BorderSize = 0;
		this.btnShowAdvanceHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowAdvanceHistory.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnShowAdvanceHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowAdvanceHistory.Location = new System.Drawing.Point(801, 5);
		this.btnShowAdvanceHistory.Name = "btnShowAdvanceHistory";
		this.btnShowAdvanceHistory.Size = new System.Drawing.Size(57, 24);
		this.btnShowAdvanceHistory.TabIndex = 3;
		this.btnShowAdvanceHistory.Text = "Show";
		this.btnShowAdvanceHistory.UseVisualStyleBackColor = false;
		this.btnShowAdvanceHistory.Click += new System.EventHandler(button1_Click);
		this.CmbInvoiceInvoiceAdvanceHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceInvoiceAdvanceHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceInvoiceAdvanceHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceInvoiceAdvanceHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance251).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance252).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceInvoiceAdvanceHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceInvoiceAdvanceHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceInvoiceAdvanceHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceInvoiceAdvanceHistory).Location = new System.Drawing.Point(498, 5);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceInvoiceAdvanceHistory).Name = "CmbInvoiceInvoiceAdvanceHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceInvoiceAdvanceHistory).Size = new System.Drawing.Size(300, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceInvoiceAdvanceHistory).TabIndex = 2;
		this.datFromDateAdvanceHistory.Checked = false;
		this.datFromDateAdvanceHistory.CustomFormat = "dd-MMM-yy";
		this.datFromDateAdvanceHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDateAdvanceHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDateAdvanceHistory.Location = new System.Drawing.Point(68, 6);
		this.datFromDateAdvanceHistory.Name = "datFromDateAdvanceHistory";
		this.datFromDateAdvanceHistory.ShowCheckBox = true;
		this.datFromDateAdvanceHistory.Size = new System.Drawing.Size(150, 23);
		this.datFromDateAdvanceHistory.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(426, 10);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "Invoice No";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 10);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 5;
		this.label4.Text = "From Date";
		this.datToDateAdvanceHistory.CustomFormat = "dd-MMM-yy";
		this.datToDateAdvanceHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateAdvanceHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDateAdvanceHistory.Location = new System.Drawing.Point(272, 6);
		this.datToDateAdvanceHistory.Name = "datToDateAdvanceHistory";
		this.datToDateAdvanceHistory.ShowCheckBox = true;
		this.datToDateAdvanceHistory.Size = new System.Drawing.Size(150, 23);
		this.datToDateAdvanceHistory.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(221, 10);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 7;
		this.label5.Text = "To Date";
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
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
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
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
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
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
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
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
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
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
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
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
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
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
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
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
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
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
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
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
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
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
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance187).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance191;
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
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance195;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(385, 6);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 8;
		this.tabControlMain.Controls.Add(this.tabGdBreakUp);
		this.tabControlMain.Controls.Add(this.tabAdvanceByGd);
		this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlMain.Location = new System.Drawing.Point(0, 0);
		this.tabControlMain.Name = "tabControlMain";
		this.tabControlMain.SelectedIndex = 0;
		this.tabControlMain.Size = new System.Drawing.Size(1106, 561);
		this.tabControlMain.TabIndex = 10;
		this.tabGdBreakUp.Controls.Add(this.tabControl1);
		this.tabGdBreakUp.Location = new System.Drawing.Point(4, 22);
		this.tabGdBreakUp.Name = "tabGdBreakUp";
		this.tabGdBreakUp.Padding = new System.Windows.Forms.Padding(3);
		this.tabGdBreakUp.Size = new System.Drawing.Size(1098, 535);
		this.tabGdBreakUp.TabIndex = 0;
		this.tabGdBreakUp.Text = "GD BreakUp";
		this.tabGdBreakUp.UseVisualStyleBackColor = true;
		this.tabAdvanceByGd.Controls.Add(this.tabControl3);
		this.tabAdvanceByGd.Location = new System.Drawing.Point(4, 22);
		this.tabAdvanceByGd.Name = "tabAdvanceByGd";
		this.tabAdvanceByGd.Padding = new System.Windows.Forms.Padding(3);
		this.tabAdvanceByGd.Size = new System.Drawing.Size(1195, 535);
		this.tabAdvanceByGd.TabIndex = 1;
		this.tabAdvanceByGd.Text = "Bank Invoice Payment Schedule && GD Mapping";
		this.tabAdvanceByGd.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1106, 561);
		base.Controls.Add(this.tabControlMain);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmGdBreakUpByInvoiceNew";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGdBreakUpByInvoice_FormClosing);
		base.Load += new System.EventHandler(frmGdBreakUpByInvoiceNew_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabGdBreakUpForm.ResumeLayout(false);
		this.tabGdBreakUpForm.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGdBreakUp).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankNameGdBreakUp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceGdBreakUp).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.toolStripGDBreakUpForm.ResumeLayout(false);
		this.toolStripGDBreakUpForm.PerformLayout();
		this.TabGdBreakUpHistory.ResumeLayout(false);
		this.TabGdBreakUpHistory.PerformLayout();
		this.panel43.ResumeLayout(false);
		this.panel44.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGDhistory).EndInit();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		this.panel42.ResumeLayout(false);
		this.panel42.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankGdBankHistory).EndInit();
		this.toolStripGdBreakupHistory.ResumeLayout(false);
		this.toolStripGdBreakupHistory.PerformLayout();
		this.tabControl3.ResumeLayout(false);
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdAdvanceUtlize).EndInit();
		this.panel33.ResumeLayout(false);
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermGdUtilize).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceAdvanceUtilize).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFIAdvanceUtlize).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGdNoAdvanceUtilize).EndInit();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.toolStripAdvanceAgaintGD.ResumeLayout(false);
		this.toolStripAdvanceAgaintGD.PerformLayout();
		this.tabPage8.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAdvanceHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceInvoiceAdvanceHistory).EndInit();
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
		this.tabControlMain.ResumeLayout(false);
		this.tabGdBreakUp.ResumeLayout(false);
		this.tabAdvanceByGd.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
