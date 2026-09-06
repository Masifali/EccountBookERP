using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Common;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
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
namespace Architecture.WinApp.Account_Definition
{

public class frmExportInvoiceVoucher : BaseForm
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private List<ExImInvoiceSaleManCommission> lstRemoveRecord = new List<ExImInvoiceSaleManCommission>();

	private DataTable table = new DataTable();

	private DataTable dtCommission = new DataTable();

	private DataTable dtPendingFAc = new DataTable();

	private DataTable dtPaymnentDetail = new DataTable();

	private DataTable dtOtherChargesDetail = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccount = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private DataTable dtcoalst = new DataTable();

	public bool UpdateMode;

	private int RecId;

	private int updateDetailIndexCommissionInfo = -1;

	private int ExImInvoiceId;

	private int CustomerGlAcId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private double ToleranceWeightForDifferenceInDispatchedAndInvoice;

	private bool CGSFlag;

	private bool SaleCostingJobOrderWise;

	private bool FIFOCGS;

	private bool Approved;

	private bool FormInitialized;

	private bool SaleCommissionTabFeatureWise;

	public bool ForSpecialApproval;

	private IContainer components;

	private ToolStrip miniToolStrip;

	private ToolStripButton btnNew;

	private ToolStripButton btnEdit;

	private ToolStripButton btnS;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnDte;

	private ToolStripButton btnCopy;

	private ToolStripButton btnPaste;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip1;

	private Panel panel1;

	private TabPage Form;

	private Panel panel2;

	private Panel panel4;

	private TabPage History;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private new ToolStripButton Update;

	private ToolStripButton Save;

	private ToolStripButton Print;

	private ToolStripButton toolStripButton10;

	private Label ExpenseVoucherLable;

	private Label label15;

	private DateTimePicker datVoucherDate;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar1;

	private TextBox txtVoucherCode;

	private CtrlGrdBar ctrlGrdBar3;

	private BackgroundWorker backgroundWorker1;

	private Timer timer1;

	internal GridEX DataGridHistory;

	private Panel panel9;

	internal GridEX grdDetail;

	private ToolStripButton btnRefresh;

	private Label label4;

	private TextBox txtRsAmount;

	private Label label3;

	private TextBox txtExchangeRate;

	private Label label18;

	private CheckBox ChkBox;

	private TextBox txtFcyAmount;

	private Label label1;

	internal UltraCombo CmbRefAccountId;

	private Label label2;

	private Label label6;

	private Label label7;

	private TextBox txtInvoiceNo;

	internal UltraCombo CmbCurrencyCode;

	private Label label8;

	private Panel panel17;

	private Panel panel16;

	private CtrlGrdBar ctrlGrdBar4;

	internal UltraCombo CmbCreditAccount;

	private Label label9;

	private DateTimePicker datInvoiceDate;

	private Label label11;

	private Label label12;

	private TextBox txtTotalWeight;

	private DateTimePicker txtDueDate;

	private Label label14;

	private Label label13;

	private TextBox txtDueDays;

	private TextBox txtRemarks;

	private Panel panel6;

	internal GridEX grd;

	internal GridEX grdPending;

	private Panel panel15;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator2;

	private Label label16;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNewCpvHistory;

	private ToolStripButton btnRefreshCpvHistory;

	private Label label25;

	private GroupBox groupBox1;

	private Label label26;

	internal UltraCombo cmbAccountTitleCpv;

	private Button btnShowCpv;

	private Label label28;

	private Label label29;

	private Label label30;

	private Label label31;

	private Label label32;

	internal UltraCombo cmbApprovedStatusCpv;

	private TextBox txtToDocNoCpv;

	private TextBox txtFromDocNoCpv;

	private DateTimePicker ToDateCpvHistory;

	private DateTimePicker FromDateCpvHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private GroupBox VouchersInfoBoxCash;

	private Label label42;

	private Label lblUnApprovedVouchers;

	private Label lblTotalVouchers;

	private Label label45;

	private Label lblApprovedVouchers;

	private Label label47;

	private ToolStripButton btnShortCutKey;

	private TextBox txtAddLessAmount;

	private TextBox txtFcyAmountWithoutAddLess;

	private Label label5;

	private Label label10;

	private Label label17;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private TabPage tabPaymentDetail;

	internal GridEX grdPaymentTermDetail;

	private Panel panel20;

	private CtrlGrdBar ctrlgrdPaymentTermDetail;

	private TabPage tabChargesDetail;

	internal GridEX grdOtherChargesDetail;

	private Panel panel3;

	private CtrlGrdBar ctrlgrdOtherChargesDetail;

	private ToolStripButton BtnDelete;

	private TabPage tabCommission;

	private Panel panel13;

	private CtrlGrdBar ctrlGrdBar5;

	private Label label108;

	private Panel panel25;

	private Label label116;

	private TextBox txtCommissionRemarks;

	private Button btnAddCommission;

	private Button btnCancelCommission;

	private TextBox txtCommRateUom;

	private TextBox txtCommissionLcyAmount;

	private Label label115;

	private Label label114;

	private TextBox txtCommissionExchangeRate;

	private TextBox txtCommRate;

	private TextBox txtCommissionAmount;

	internal UltraCombo cmbCommissionType;

	private Label label110;

	private Label label111;

	private Label label112;

	private Label label113;

	private Label label109;

	internal UltraCombo CmbSalePerson;

	private Button btnUpdateCommision;

	internal GridEX grdCommissionInfo;

	internal UltraCombo CmbCommissionExpenseAccount;

	private Label label19;

	internal TabControl tabControl1;

	private Panel panel5;

	private Button btnRecordsUpdate;

	public frmExportInvoiceVoucher(UserAccount puserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void AcfrmPaymentVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			Save.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			Update.Enabled = formright.DoHaveUpdateRights;
			BtnDelete.Enabled = formright.DoHaveCanDelete;
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar3, formright);
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar4, formright);
			FIFOCGS = CommonServices.GetERPFeatureById(5);
			SaleCommissionTabFeatureWise = CommonServices.GetERPFeatureById(15);
			CGSFlag = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CGSEntryAllow"));
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			ToleranceWeightForDifferenceInDispatchedAndInvoice = Math.Abs(Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ToleranceWeightForDifferenceInDispatchedAndInvoice")));
			FormInitialized = true;
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			if (table.Columns.Count == 0)
			{
				table.Columns.Add("EntryTypeId", typeof(int));
				table.Columns.Add("Id", typeof(int));
				table.Columns.Add("ExImInvoiceDetailId", typeof(int));
				table.Columns.Add("ExImInvoiceId", typeof(int));
				table.Columns.Add("ContractId", typeof(int));
				table.Columns.Add("ContractNo", typeof(string));
				table.Columns.Add("InvoiceNo", typeof(string));
				table.Columns.Add("ItemId", typeof(int));
				table.Columns.Add("ItemName", typeof(string));
				table.Columns.Add("WarehouseId", typeof(int));
				table.Columns.Add("CropYearId", typeof(int));
				table.Columns.Add("CropYear", typeof(string));
				table.Columns.Add("LotJobId", typeof(int));
				table.Columns.Add("JobLotCode", typeof(string));
				table.Columns.Add("PackingMaterialId", typeof(int));
				table.Columns.Add("PackMaterilaType", typeof(string));
				table.Columns.Add("Qty", typeof(double));
				table.Columns.Add("OuterQtyUomId", typeof(int));
				table.Columns.Add("ItemUOM", typeof(string));
				table.Columns.Add("NetWeight", typeof(double));
				table.Columns.Add("RatePrice", typeof(double));
				table.Columns.Add("RateUomId", typeof(int));
				table.Columns.Add("RateUOM", typeof(string));
				table.Columns.Add("FcAmount", typeof(double));
				table.Columns.Add("StockWeight", typeof(double));
				table.Columns.Add("CostRate", typeof(double));
				table.Columns.Add("StockValue", typeof(double));
				table.Columns.Add("AddLessAmount", typeof(double));
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtPaymnentDetail.Columns.Add("Id");
			dtPaymnentDetail.Columns.Add("PaymentTermId");
			dtPaymnentDetail.Columns.Add("PaymentTerm");
			dtPaymnentDetail.Columns.Add("DocumentTypeId");
			dtPaymnentDetail.Columns.Add("ExImEFormRegistrationId");
			dtPaymnentDetail.Columns.Add("FinancialInstrumentNo");
			dtPaymnentDetail.Columns.Add("%OfTotal", typeof(double));
			dtPaymnentDetail.Columns.Add("FcyAmount", typeof(double));
			dtPaymnentDetail.Columns.Add("DueDays");
			dtPaymnentDetail.Columns.Add("Remarks");
			dtOtherChargesDetail.Columns.Add("Id", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItemId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItem");
			dtOtherChargesDetail.Columns.Add("AddAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("LessAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("ChargesAccountId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesAccount", typeof(string));
			dtOtherChargesDetail.Columns.Add("Remarks");
			dtCommission.Columns.Add("Id", typeof(int));
			dtCommission.Columns.Add("SalesPersonId", typeof(int));
			dtCommission.Columns.Add("SalesPerson", typeof(string));
			dtCommission.Columns.Add("CommissionTypeId", typeof(int));
			dtCommission.Columns.Add("CommissionType", typeof(string));
			dtCommission.Columns.Add("Rate", typeof(double));
			dtCommission.Columns.Add("RateUom", typeof(string));
			dtCommission.Columns.Add("FcyAmount", typeof(double));
			dtCommission.Columns.Add("ExchangeRate", typeof(double));
			dtCommission.Columns.Add("LcyAmount", typeof(double));
			dtCommission.Columns.Add("Remarks", typeof(string));
			if (dtAccount.Columns.Count == 0)
			{
				dtAccount.Columns.Add("Id");
				dtAccount.Columns.Add("AccountTitle");
			}
			if (dtJobLot.Columns.Count == 0)
			{
				dtJobLot.Columns.Add("Id");
				dtJobLot.Columns.Add("JobLotDescription");
			}
			UpdateDocumentNoUI(Conversion.ToString((object)CommonServices.GenerateVoucherCode(27)));
			SupplierDtFillFromGlobal();
			BindSupplierName();
			CreditGlAccountBindFromGlobal();
			ExpenseCommissionAccountBindFromGlobal();
			MultiCurrencyBind(MultiCurrency.GetMultiCurrencywithExchangeRate(UserAccount.CompanyId));
			if (SaleCommissionTabFeatureWise)
			{
				salesPersonFill();
				CommissionTypeFill();
			}
			else
			{
				((Control)(object)CmbCommissionExpenseAccount).Visible = false;
				label19.Visible = false;
				tabControl2.TabPages.Remove(tabCommission);
			}
			PendingForAccountsGLEntry();
			txtVoucherCode.Focus();
			ChkBox.Checked = true;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			StatusFillForHistory();
			ComboBindForHistory();
			if (!ForSpecialApproval)
			{
				FromDateCpvHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			}
			else
			{
				((Control)(object)CmbCreditAccount).Enabled = false;
				datVoucherDate.Enabled = false;
				((Control)(object)CmbCommissionExpenseAccount).Enabled = false;
				txtExchangeRate.Enabled = false;
				txtDueDays.Enabled = false;
				if (SaleCommissionTabFeatureWise)
				{
					tabControl2.TabPages[3].Enabled = false;
					tabControl2.Dock = DockStyle.Fill;
				}
				FromDateCpvHistory.Checked = false;
				panel5.Visible = false;
				Update.Text = "Special Approve";
				HistoryFill();
			}
			ToDateCpvHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtVoucherCode.Text = documentData;
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			foreach (getGlobalAllSupplierCustomer item in clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 7))
			{
				dtSupplier.Rows.Add(Conversion.ToInt((object)item.Id), Conversion.ToString((object)item.CompanyName), Conversion.ToString((object)item.PartyCode), Conversion.ToInt((object)item.GlAccountId));
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
			InfragisticsHelper.BindAndRetainSelection(CmbRefAccountId, dtSupplier, "Id", "CompanyName", "Buyer Name", AllColumns: true, Conversion.ToInt(CmbRefAccountId.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbRefAccountId).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CreditGlAccountBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[4] { 3, 6, 8, 10 });
			InfragisticsHelper.BindAndRetainSelection(CmbCreditAccount, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbCreditAccount.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseCommissionAccountBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[3] { 11, 20, 21 });
			InfragisticsHelper.BindAndRetainSelection(CmbCommissionExpenseAccount, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbCommissionExpenseAccount.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCurrencyCode, dt, "Id", "CurrencyCode", "Fcy Code", AllColumns: true, Conversion.ToInt(CmbCurrencyCode.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void salesPersonFill()
	{
		try
		{
			int id = Conversion.ToInt(CmbSalePerson.Value);
			DataTable dtSalePerson = CommonServices.GetSupplierustomerByCustomerGroupId("9");
			DDL.BindDDL(dtSalePerson, CmbSalePerson, "Id", "CompanyName", "Sales Person", ZeroIndex: true);
			((UltraGridBase)CmbSalePerson).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (dtSalePerson.Rows.Count > 0 && id > 0)
			{
				CommonServices.SetComboValue(CmbSalePerson, "Id", id, dtSalePerson, ActivateRow: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Fixed Commission Amount");
			dt.Rows.Add(2, "Commission % of Value");
			dt.Rows.Add(3, "Commission By Weight Kg");
			DDL.BindDDL(dt, cmbCommissionType, "Id", "CommissionType", "Commission Type", ZeroIndex: true);
			((UltraGridBase)cmbCommissionType).Rows[2].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbRefAccountId).ActiveRow == null || Conversion.ToInt(CmbRefAccountId.Value) == 0)
		{
			MessageBox.Show("Account Field is Required");
			CmbRefAccountId.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCurrencyCode).ActiveRow == null || Conversion.ToInt(CmbCurrencyCode.Value) == 0)
		{
			MessageBox.Show("Currency Code Field is Required");
			CmbCurrencyCode.Focus();
			return false;
		}
		if (txtInvoiceNo.Text == string.Empty || txtInvoiceNo.Text == "0")
		{
			MessageBox.Show("Invoice No Field is Required");
			txtInvoiceNo.Focus();
			return false;
		}
		if (txtExchangeRate.Text == string.Empty || txtExchangeRate.Text == "0")
		{
			MessageBox.Show("ExchangeRate Field is Required");
			txtExchangeRate.Focus();
			return false;
		}
		if (txtFcyAmount.Text == string.Empty || txtFcyAmount.Text == "0")
		{
			MessageBox.Show("FcyAmount Field is Required");
			txtFcyAmount.Focus();
			return false;
		}
		if (txtRsAmount.Text == string.Empty || txtRsAmount.Text == "0")
		{
			MessageBox.Show("RsAmount Field is Required");
			txtRsAmount.Focus();
			return false;
		}
		return true;
	}

	private void BindGrids()
	{
		try
		{
			if (table != null && table.Columns.Count > 0)
			{
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
			}
			if (dtPaymnentDetail != null && dtPaymnentDetail.Columns.Count > 0)
			{
				grdPaymentTermDetail.DataSource = dtPaymnentDetail;
				grdPaymentTermDetail.RetrieveStructure();
				grdSettingsPaymentTerm(grdPaymentTermDetail);
			}
			if (dtOtherChargesDetail != null && dtOtherChargesDetail.Columns.Count > 0)
			{
				grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
				grdOtherChargesDetail.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetail);
			}
			if (dtCommission != null && dtCommission.Columns.Count > 0)
			{
				grdCommissionInfo.DataSource = dtCommission;
				grdCommissionInfo.RetrieveStructure();
				grdSettingsCommissionInfo();
			}
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
			List<string> hiddenColumns = new List<string>
			{
				"Id", "EntryTypeId", "ExImInvoiceId", "InvoiceNo", "ExImInvoiceDetailId", "ContractId", "ItemId", "RateUomId", "OuterQtyUomId", "CropYearId",
				"LotJobId", "WarehouseId", "PackingMaterialId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns);
			grd.RootTable.Columns["PackMaterilaType"].Caption = "Packing Type";
			grd.RootTable.Columns["JobLotCode"].Caption = "Job Lot";
			grd.RootTable.Columns["RatePrice"].Caption = "Rate";
			if (!CGSFlag && !FIFOCGS)
			{
				grd.RootTable.Columns["CostRate"].EditType = (EditType)1;
			}
			else
			{
				grd.RootTable.Columns["CostRate"].EditType = (EditType)0;
			}
			if (FIFOCGS)
			{
				grd.RootTable.Columns["CostRate"].Visible = false;
				grd.RootTable.Columns["StockValue"].Visible = false;
			}
			double FcYTotal = GridEX_Helper.GetColumnSum(grd, "FcAmount");
			txtFcyAmountWithoutAddLess.Text = FcYTotal.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			grd.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grd.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grd.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
			grd.RootTable.Columns["CropYear"].Width = Constants.ExportConstants.CropYearConstant;
			grd.RootTable.Columns["JobLotCode"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["PackMaterilaType"].Width = Constants.ExportConstants.PackingTypeConstant;
			grd.RootTable.Columns["ItemUOM"].Width = Constants.ExportConstants.UomConstant;
			grd.RootTable.Columns["RateUOM"].Width = Constants.ExportConstants.UomConstant;
			grd.RootTable.Columns["Qty"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["RatePrice"].Width = Constants.ExportConstants.ExRateConstant;
			grd.RootTable.Columns["FcAmount"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["AddLessAmount"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["StockWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["CostRate"].Width = Constants.ExportConstants.ExRateConstant;
			grd.RootTable.Columns["StockValue"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["AddLessAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLessAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLessAmount"].FormatString = "#,##0.####";
			grd.RootTable.Columns["AddLessAmount"].TotalFormatString = "#,##0.####";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "CostRate")
			{
				double StockWeight = Conversion.ToDouble(item.Cells["StockWeight"].Value);
				double CostRate = Conversion.ToDouble(item.Cells["CostRate"].Value);
				if (StockWeight > 0.0 && CostRate > 0.0)
				{
					item.Cells["StockValue"].Value = StockWeight * CostRate;
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdSettingsPaymentTerm(GridEX grid)
	{
		try
		{
			grid.RootTable.Columns["Id"].Visible = false;
			grid.RootTable.Columns["PaymentTermId"].Visible = false;
			grid.RootTable.Columns["ExImEFormRegistrationId"].Visible = false;
			grid.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3);
			grid.RootTable.Columns["FinancialInstrumentNo"].Width = 350;
			grid.RootTable.Columns["PaymentTerm"].Width = 350;
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
			GridEXRow r = grdPaymentTermDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeletePTDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePTDetailRow(GridEXRow r)
	{
	}

	private void grdSettingsOtherChargesDetail(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ChargesItemId", "ChargesAccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns);
			grid.RootTable.Columns["ChargesItem"].Width = 350;
			grid.RootTable.Columns["ChargesAccount"].Width = 200;
			grid.RootTable.Columns["Remarks"].Width = 350;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherChargesDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOtherChargesDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeleteOtherChargesDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOtherChargesDetailRow(GridEXRow r)
	{
	}

	private void PendingForAccountsGLEntry()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtPendingFAc = VoucherHead.PendingForAccountsGLEntry(obj);
			if (dtPendingFAc.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("Customer", typeof(string));
				dt.Columns.Add("Consignee", typeof(string));
				dt.Columns.Add("ContractNo", typeof(string));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("InvoiceStatusId", typeof(int));
				dt.Columns.Add("InvoiceDispatchStatus", typeof(string));
				dt.Columns.Add("FcurrencyId", typeof(int));
				dt.Columns.Add("FcyCode", typeof(string));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("NoOfContainers", typeof(int));
				dt.Columns.Add("InvoiceNetWeight", typeof(double));
				dt.Columns.Add("DispatchWeight", typeof(double));
				dt.Columns.Add("DoNetWeight", typeof(double));
				dt.Columns.Add("WbGrossWeight", typeof(double));
				dt.Columns.Add("ChartOfAccountId", typeof(int));
				dt.Columns.Add("CreditAccountId", typeof(int));
				dt.Columns.Add("FI_Number", typeof(string));
				dt.Columns.Add("FI_Date", typeof(DateTime));
				dt.Columns.Add("SpecialApprovedStatus", typeof(string));
				dt.Rows.Clear();
				foreach (DataRow dr in dtPendingFAc.Rows)
				{
					dt.Rows.Add(dr["Id"], dr["SupplierCustomerId"], dr["BuyerName"], dr["Consignee"], dr["ContractNo"], dr["InvoiceNo"], dr["DocDate"], dr["InvoiceStatusId"], dr["InvoiceStatus"], dr["FcurrencyId"], dr["FcyCode"], dr["ExchangeRate"], dr["FCurrencyAmount"], dr["DeliveryTermCode"], dr["NoOfContainers"], dr["NetWeight"], dr["DispatchWeight"], dr["DoWeight"], dr["WbGrossWeight"], dr["ChartOfAccountId"], dr["CreditAccountId"], dr["FI_Number"], dr["FI_Date"], dr["SpecialApprovedStatus"]);
				}
				grdPending.DataSource = dt;
				grdPending.RetrieveStructure();
				grdPendingSetting();
			}
			else
			{
				grdPending.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "FcurrencyId", "SupplierCustomerId", "ChartOfAccountId", "CreditAccountId", "InvoiceStatusId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPending, 3, 3, hiddenColumns);
			grdPending.RootTable.Columns["SpecialApprovedStatus"].Visible = ToleranceWeightForDifferenceInDispatchedAndInvoice > 0.0;
			grdPending.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName;
			grdPending.RootTable.Columns["Consignee"].Width = Constants.InventoryConstants.SupplierName;
			grdPending.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grdPending.RootTable.Columns["FI_Number"].Width = Constants.ExportConstants.BLNumberConstant;
			grdPending.RootTable.Columns["FI_Date"].Width = Constants.ExportConstants.DateConstant;
			grdPending.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdPending.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant;
			grdPending.RootTable.Columns["FcyCode"].Width = 60;
			grdPending.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdPending.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdPending.RootTable.Columns["DeliveryTerm"].Width = Constants.ExportConstants.TermConstant;
			grdPending.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			grdPending.RootTable.Columns["InvoiceNetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdPending.RootTable.Columns["WbGrossWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdPending.RootTable.Columns["DoNetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdPending.RootTable.Columns["InvoiceDispatchStatus"].Width = Constants.ExportConstants.StatusConstant;
			GridEX_Helper.AddButton(grdPending, "Edit", "Edit", 40, 1);
			grdPending.FrozenColumns = 1;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingRecordbind(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && Save.Visible && Save.Enabled)
			{
				int Id = Conversion.ToInt(r.Cells[0].Value);
				if (Conversion.ToInt(r.Cells["InvoiceStatusId"].Value) == 0)
				{
					throw new Exception(((ToleranceWeightForDifferenceInDispatchedAndInvoice == 0.0) ? "The invoice status is pending because the dispatch has not been completed against the invoice" : "The invoice status is pending because the dispatch has not been completed against the invoice or Special Approval is Pending") + ". Please check");
				}
				CmbRefAccountId.Value = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				CustomerGlAcId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
				txtExchangeRate.Text = Conversion.ToString(r.Cells["ExchangeRate"].Value);
				txtInvoiceNo.Text = Conversion.ToString(r.Cells["InvoiceNo"].Value);
				CmbCurrencyCode.Value = Conversion.ToInt(r.Cells["FcurrencyId"].Value);
				ExImInvoiceId = Conversion.ToInt(r.Cells["Id"].Value);
				datInvoiceDate.Value = Conversion.ToDateTime(r.Cells["InvoiceDate"].Value);
				txtTotalWeight.Text = Conversion.ToString(r.Cells["InvoiceNetWeight"].Value);
				if (Conversion.ToInt(r.Cells["CreditAccountId"].Value) > 0)
				{
					CmbCreditAccount.Value = Conversion.ToInt(r.Cells["CreditAccountId"].Value);
				}
				else
				{
					((Control)(object)CmbCreditAccount).Text = string.Empty;
				}
				DataTable dt = ExImInvoice.GetExImInvoicePackingDetailForVouchers(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Id,
					DocDate = datVoucherDate.Value
				});
				if (dt.Rows.Count > 0)
				{
					table.Rows.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						table.Rows.Add(dt.Rows[i]["EntryTypeId"], dt.Rows[i]["Id"], dt.Rows[i]["ExImInvoiceDetailId"], dt.Rows[i]["ExImInvoiceId"], dt.Rows[i]["ContractId"], dt.Rows[i]["ContractNo"], dt.Rows[i]["InvoiceNo"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["WarehouseId"], dt.Rows[i]["CropYearId"], dt.Rows[i]["CropYear"], dt.Rows[i]["LotJobId"], dt.Rows[i]["JobLotCode"], dt.Rows[i]["PackingMaterialId"], dt.Rows[i]["PackMaterilaType"], dt.Rows[i]["Qty"], dt.Rows[i]["OuterQtyUomId"], dt.Rows[i]["ItemUOM"], dt.Rows[i]["NetWeight"], dt.Rows[i]["RatePrice"], dt.Rows[i]["RateUomId"], dt.Rows[i]["RateUOM"], dt.Rows[i]["FcAmount"], dt.Rows[i]["StockWeight"], dt.Rows[i]["CostRate"], dt.Rows[i]["StockValue"], 0);
					}
				}
				else
				{
					grd.ClearStructure();
				}
				dtPaymnentDetail.Clear();
				FillPaymentDetailFromInvoice(ExImInvoiceId, dtPaymnentDetail);
				dtOtherChargesDetail.Rows.Clear();
				FillOtherChargesDetailFromInvoice(ExImInvoiceId, dtOtherChargesDetail);
				dtCommission.Rows.Clear();
				FillCommissionDetailFromInvoice(ExImInvoiceId, dtCommission);
				BindGrids();
				txtFcyAmountWithoutAddLess.Text = GridEX_Helper.GetColumnSum(grd, "FcAmount").ToString(clsGlobalVariables.stringFormatsingleForFcy);
				UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtAddLessAmount);
				ProportionateAddLessAmount();
				return;
			}
			throw new Exception("Record Not bind in grid");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPending_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPending.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Edit")
			{
				PendingRecordbind(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool CommissionDetailFormValidation()
	{
		if (Conversion.ToInt(CmbSalePerson.Value) == 0)
		{
			MessageBox.Show("Sales Person field is required");
			CmbSalePerson.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbCommissionType.Value) == 0)
		{
			MessageBox.Show("Commission Type field is required");
			cmbCommissionType.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommRate.Text) == 0.0)
		{
			MessageBox.Show("Commission Rate Field is Required");
			txtCommRate.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbCommissionType.Value) == 3 && txtCommRateUom.Text == "")
		{
			MessageBox.Show("Commission Rate Uom Field is Required");
			txtCommRateUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommissionAmount.Text.Trim()) == 0.0 || txtCommissionAmount.Text == "")
		{
			MessageBox.Show("Commission Fcy Amount Field is Required");
			txtCommissionAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommissionExchangeRate.Text.Trim()) == 0.0 || txtCommissionExchangeRate.Text == "")
		{
			MessageBox.Show("Exchange Rate Field is Required");
			txtCommissionExchangeRate.Focus();
			return false;
		}
		return true;
	}

	private void btnAddCommission_Click(object sender, EventArgs e)
	{
		try
		{
			if (!CommissionDetailFormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdCommissionInfo.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(CmbSalePerson.Value) > 0 && Conversion.ToInt(item.Cells["SalesPersonId"].Value) == Conversion.ToInt(CmbSalePerson.Value))
				{
					throw new Exception("Sale person already add in Grid Please select another sale person!");
				}
			}
			dtCommission.Rows.Add(0, CmbSalePerson.Value, ((Control)(object)CmbSalePerson).Text, Conversion.ToInt(cmbCommissionType.Value), ((Control)(object)cmbCommissionType).Text, Conversion.ToDouble((object)txtCommRate.Text.Trim()), txtCommRateUom.Text.Trim(), Conversion.ToDouble((object)txtCommissionAmount.Text.Trim()), Conversion.ToDouble((object)txtCommissionExchangeRate.Text.Trim()), Conversion.ToDouble((object)txtCommissionLcyAmount.Text.Trim()), Conversion.ToString((object)txtCommissionRemarks.Text.Trim()));
			grdCommissionInfo.DataSource = dtCommission;
			grdCommissionInfo.RetrieveStructure();
			grdSettingsCommissionInfo();
			ResetCommissionDetail();
			CmbSalePerson.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void ResetCommissionDetail()
	{
		try
		{
			updateDetailIndexCommissionInfo = -1;
			CmbSalePerson.Value = 0;
			cmbCommissionType.Value = 0;
			txtCommRate.Text = Conversion.ToInt((object)0).ToString();
			txtCommRateUom.Text = Conversion.ToInt((object)0).ToString();
			txtCommRateUom.Tag = "";
			txtCommissionAmount.Text = Conversion.ToInt((object)0).ToString();
			txtCommissionLcyAmount.Text = Conversion.ToInt((object)0).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdateCommision_Click(object sender, EventArgs e)
	{
		try
		{
			if (!CommissionDetailFormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdCommissionInfo.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (((UltraGridBase)CmbSalePerson).ActiveRow != null && Conversion.ToInt(CmbSalePerson.Value) > 0 && updateDetailIndexCommissionInfo != Conversion.ToInt((object)item.RowIndex) && Conversion.ToInt(item.Cells["SalesPersonId"].Value) == Conversion.ToInt(CmbSalePerson.Value))
				{
					throw new Exception("Sales person already add in Grid Please selected another SalesPerson!");
				}
			}
			dtCommission.Rows[updateDetailIndexCommissionInfo]["SalesPersonId"] = Conversion.ToInt(CmbSalePerson.Value);
			dtCommission.Rows[updateDetailIndexCommissionInfo]["SalesPerson"] = ((Control)(object)CmbSalePerson).Text;
			dtCommission.Rows[updateDetailIndexCommissionInfo]["CommissionTypeId"] = Conversion.ToInt(cmbCommissionType.Value);
			dtCommission.Rows[updateDetailIndexCommissionInfo]["CommissionType"] = ((Control)(object)cmbCommissionType).Text;
			dtCommission.Rows[updateDetailIndexCommissionInfo]["Rate"] = Conversion.ToDouble((object)txtCommRate.Text);
			dtCommission.Rows[updateDetailIndexCommissionInfo]["RateUom"] = txtCommRateUom.Text.Trim();
			dtCommission.Rows[updateDetailIndexCommissionInfo]["FcyAmount"] = txtCommissionAmount.Text.Trim();
			dtCommission.Rows[updateDetailIndexCommissionInfo]["ExchangeRate"] = Conversion.ToDouble((object)txtCommissionExchangeRate.Text.Trim());
			dtCommission.Rows[updateDetailIndexCommissionInfo]["LcyAmount"] = txtCommissionLcyAmount.Text.Trim();
			dtCommission.Rows[updateDetailIndexCommissionInfo]["Remarks"] = txtCommissionRemarks.Text;
			grdCommissionInfo.DataSource = dtCommission;
			btnAddCommission.Visible = true;
			btnUpdateCommision.Visible = false;
			btnCancelCommission.Visible = false;
			ResetCommissionDetail();
			CmbSalePerson.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelCommission_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddCommission.Visible = true;
			btnUpdateCommision.Visible = false;
			btnCancelCommission.Visible = false;
			ResetCommissionDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsCommissionInfo()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SalesPersonId", "CommissionTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdCommissionInfo, 2, 3, hiddenColumns);
			grdCommissionInfo.RootTable.Columns["SalesPerson"].Width = Constants.ExportConstants.PartyName + 20;
			grdCommissionInfo.RootTable.Columns["CommissionType"].Width = 150;
			grdCommissionInfo.RootTable.Columns["Rate"].Width = Constants.ExportConstants.ExRateConstant + 10;
			grdCommissionInfo.RootTable.Columns["RateUom"].Width = Constants.ExportConstants.UomConstant + 20;
			grdCommissionInfo.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdCommissionInfo.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant + 20;
			grdCommissionInfo.RootTable.Columns["LcyAmount"].Width = Constants.ExportConstants.LcyAmountConstant;
			grdCommissionInfo.RootTable.Columns["Remarks"].Width = Constants.ExportConstants.RemarksConstants;
			grdCommissionInfo.RootTable.Columns.Add("Delete");
			grdCommissionInfo.RootTable.Columns["Delete"].Caption = "X";
			grdCommissionInfo.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdCommissionInfo.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdCommissionInfo.RootTable.Columns["Delete"].ButtonText = "X";
			grdCommissionInfo.RootTable.Columns["Delete"].Width = 20;
			grdCommissionInfo.RootTable.Columns["Delete"].Position = 0;
			grdCommissionInfo.FrozenColumns = 1;
			((Control)(object)grdCommissionInfo).Refresh();
			ctrlGrdBar5_Load(null, null);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdCommissionInfo, dtCommission);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdCommissionInfo_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdCommissionInfo.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				if (updateDetailIndexCommissionInfo != -1)
				{
					throw new Exception("Reset Detail First...");
				}
				deleteSaleCommissionRecord(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void deleteSaleCommissionRecord(GridEXRow r)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ExImInvoiceSaleManCommission d = new ExImInvoiceSaleManCommission();
					d.exImInvoiceSaleManCommissionId = Id;
					d.SalesPersonId = Conversion.ToInt(r.Cells["SalesPersonId"].Value);
					d.commissionTypeId = Conversion.ToInt(r.Cells["CommissionTypeId"].Value);
					d.commissionRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					d.rateUom = Conversion.ToString((object)r.Cells["RateUom"].Text);
					d.fcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					d.exchangeRate = Conversion.ToDouble(r.Cells["ExchangeRate"].Value);
					d.lcyAmount = Conversion.ToDouble(r.Cells["LcyAmount"].Value);
					d.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					d.ActionTypeId = 3;
					lstRemoveRecord.Add(d);
					dtCommission.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			else
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdCommissionInfo_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdCommissionInfo.CurrentRow;
			if (item != null)
			{
				updateDetailIndexCommissionInfo = item.RowIndex;
				CmbSalePerson.Value = Conversion.ToInt(item.Cells["SalesPersonId"].Value);
				cmbCommissionType.Value = Conversion.ToInt(item.Cells["CommissionTypeId"].Value);
				txtCommRate.Text = Conversion.ToDouble(item.Cells["Rate"].Value).ToString();
				txtCommRateUom.Text = item.Cells["RateUom"].Value.ToString();
				txtCommissionAmount.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString("#,##0.###");
				txtCommissionExchangeRate.Text = Conversion.ToDouble(item.Cells["ExchangeRate"].Value).ToString("#,##0.###");
				txtCommissionLcyAmount.Text = Conversion.ToDouble(item.Cells["LcyAmount"].Value).ToString("#,##0.###");
				txtCommissionRemarks.Text = item.Cells["Remarks"].Value.ToString();
				btnAddCommission.Visible = false;
				btnUpdateCommision.Visible = true;
				btnCancelCommission.Visible = true;
				CmbSalePerson.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateCommissionAmount()
	{
		try
		{
			if (Conversion.ToInt(cmbCommissionType.Value) > 0 && Conversion.ToDouble((object)txtCommRate.Text.Trim()) > 0.0 && Conversion.ToDouble((object)txtFcyAmount.Text.Trim()) > 0.0)
			{
				double GridAmount = 0.0;
				double GridRate = 0.0;
				double CommRate = Conversion.ToDouble((object)txtCommRate.Text.Trim());
				double FcyAmount = Conversion.ToDouble((object)txtFcyAmount.Text.Trim());
				double NetWeightKgs = Conversion.ToDouble((object)txtTotalWeight.Text.Trim());
				double CommAmt = 0.0;
				GridEXRow[] rows = grdCommissionInfo.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (updateDetailIndexCommissionInfo != item.RowIndex)
					{
						GridAmount += Conversion.ToDouble(item.Cells["FcyAmount"].Value);
						GridRate += Conversion.ToDouble(item.Cells["Rate"].Value);
					}
				}
				if (Conversion.ToInt(cmbCommissionType.Value) == 1)
				{
					if (GridAmount + CommRate > FcyAmount)
					{
						double rem = FcyAmount - GridAmount;
						CommRate = ((rem >= 0.0) ? rem : 0.0);
					}
					CommAmt = CommRate;
				}
				else if (Conversion.ToInt(cmbCommissionType.Value) == 2)
				{
					if (CommRate + GridRate > 99.0)
					{
						CommAmt = FcyAmount - GridAmount;
						CommRate = CommAmt * 100.0 / FcyAmount;
						txtCommRate.Text = CommRate.ToString("#,##0.####");
					}
					CommAmt = FcyAmount * CommRate / 100.0;
					if (CommAmt + GridAmount > FcyAmount)
					{
						double rem2 = FcyAmount - GridAmount;
						CommAmt = ((!(rem2 > 0.0)) ? 0.0 : (rem2 * CommRate / 100.0));
					}
				}
				else if (Conversion.ToInt(cmbCommissionType.Value) == 3)
				{
					if (NetWeightKgs > 0.0)
					{
						double UOM = Conversion.ToDouble((object)txtCommRateUom.Text.Trim());
						CommAmt = NetWeightKgs / UOM * CommRate;
						if (CommAmt + GridAmount > FcyAmount)
						{
							double rem3 = FcyAmount - GridAmount;
							if (rem3 > 0.0)
							{
								CommAmt = rem3;
								CommRate = CommAmt * 100.0 / FcyAmount;
								txtCommRate.Text = CommRate.ToString("#,##0.####");
								CommAmt = NetWeightKgs / UOM * CommRate;
							}
							else
							{
								CommAmt = 0.0;
							}
						}
					}
					else
					{
						CommAmt = 0.0;
					}
				}
				txtCommissionAmount.Text = CommAmt.ToString("#,##0.####");
				double ExchangeRate = Conversion.ToDouble((object)txtCommissionExchangeRate.Text);
				double LcyAmount = ((ExchangeRate > 0.0) ? (ExchangeRate * CommAmt) : 0.0);
				txtCommissionLcyAmount.Text = LcyAmount.ToString("#,##0.####");
			}
			else
			{
				txtCommissionAmount.Text = "0";
				txtCommissionLcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbCommissionType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(cmbCommissionType.Value);
			if (Id > 0 && Id == 1)
			{
				txtCommRateUom.Text = "Lump Sum";
				txtCommRateUom.Enabled = false;
			}
			else if (Id > 0 && Id == 2)
			{
				txtCommRateUom.Text = "%";
				txtCommRateUom.Enabled = false;
			}
			else if (Id > 0 && Id == 3)
			{
				if (txtCommRateUom.Tag == "")
				{
					txtCommRateUom.Text = "1000";
				}
				txtCommRateUom.Enabled = true;
			}
			else
			{
				txtCommRateUom.Text = "";
				txtCommRateUom.Enabled = true;
			}
			CalculateCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommRateUom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtCommRateUom.Tag = txtCommRateUom.Text;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommissionAmount_TextChanged(object sender, EventArgs e)
	{
	}

	private void RecalculateCommissionGridLocalAmount()
	{
		try
		{
			GridEXRow[] rows = grdCommissionInfo.GetRows();
			foreach (GridEXRow item in rows)
			{
				double fcyAmount = Conversion.ToDouble(item.Cells["FcyAmount"].Value);
				double ExchangeRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				item.BeginEdit();
				if (fcyAmount > 0.0 && ExchangeRate > 0.0)
				{
					double LocalAmount = fcyAmount * ExchangeRate;
					item.Cells["ExchangeRate"].Value = ExchangeRate;
					item.Cells["LcyAmount"].Value = LocalAmount;
				}
				else
				{
					item.Cells["ExchangeRate"].Value = 0;
					item.Cells["LcyAmount"].Value = 0;
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			UpdateMode = false;
			CustomerGlAcId = 0;
			RecId = 0;
			ExImInvoiceId = 0;
			txtInvoiceNo.Text = string.Empty;
			datInvoiceDate.Value = DateTime.Now;
			txtTotalWeight.Text = string.Empty;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			((Control)(object)CmbCurrencyCode).Text = string.Empty;
			txtFcyAmountWithoutAddLess.Text = string.Empty;
			txtAddLessAmount.Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtRsAmount.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			table.Clear();
			dtPaymnentDetail.Clear();
			dtOtherChargesDetail.Clear();
			dtCommission.Rows.Clear();
			ResetCommissionDetail();
			BindGrids();
			Save.Visible = true;
			Update.Visible = false;
			BtnDelete.Visible = false;
			UpdateDocumentNoUI(Conversion.ToString((object)CommonServices.GenerateVoucherCode(27)));
			PendingForAccountsGLEntry();
			txtVoucherCode.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
		((Control)(object)CmbRefAccountId).Text = string.Empty;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "SupplierCustomerLists", "VendorsAndCustomersForTransporters" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			CGSFlag = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CGSEntryAllow"));
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			ToleranceWeightForDifferenceInDispatchedAndInvoice = Math.Abs(Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ToleranceWeightForDifferenceInDispatchedAndInvoice")));
			SupplierDtFillFromGlobal();
			BindSupplierName();
			CreditGlAccountBindFromGlobal();
			ExpenseCommissionAccountBindFromGlobal();
			MultiCurrencyBind(MultiCurrency.GetMultiCurrencywithExchangeRate(UserAccount.CompanyId));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExportInvoiceVoucher_FormClosing(object sender, FormClosingEventArgs e)
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

	private bool Insert(bool IsAutoUpdate = false, int RecUpdateId = 0)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Expected O, but got Unknown
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Expected O, but got Unknown
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return false;
			}
			ExportVoucher obj = new ExportVoucher();
			if (IsAutoUpdate)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return false;
				}
				obj.Id = RecUpdateId;
				obj.AutoUpdate = 1;
			}
			else if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return false;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return false;
			}
			if (Conversion.ToInt(CmbCommissionExpenseAccount.Value) == 0 && grdCommissionInfo.GetRows().Count() > 0)
			{
				CmbCommissionExpenseAccount.Focus();
				throw new Exception("Commission Expense Account required because commission is given in commission info tab!");
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.DocumentTypeId = 27;
			obj.ExImInvoiceId = ExImInvoiceId;
			obj.InvoiceNo = txtInvoiceNo.Text.Trim();
			obj.DocNo = Conversion.ToInt((object)txtVoucherCode.Text.Trim());
			obj.DocDate = datVoucherDate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(CmbRefAccountId.Value);
			obj.CreditAccounId = Conversion.ToInt(CmbCreditAccount.Value);
			obj.CommissionExpenseAccountId = Conversion.ToInt(CmbCommissionExpenseAccount.Value);
			obj.CurrencyId = Conversion.ToInt(CmbCurrencyCode.Value);
			obj.FcyAmountWithoutAddLess = Conversion.ToDouble((object)txtFcyAmountWithoutAddLess.Text);
			obj.AddLessAmount = Conversion.ToDouble((object)txtAddLessAmount.Text);
			obj.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			obj.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			obj.LcyAmount = Conversion.ToDecimal((object)txtRsAmount.Text);
			if (ForSpecialApproval)
			{
				obj.IsSpecialApproved = true;
				obj.IsSpecialApprovalRequired = 2;
				obj.SpecialApprovalUserId = UserAccount.ID;
				obj.SpecialApprovalDate = DateTime.Now;
			}
			obj.DueDate = Conversion.ToDateTime((object)txtDueDate.Value);
			obj.DueDays = Conversion.ToInt((object)txtDueDays.Text);
			obj.Remarks = txtRemarks.Text.Trim();
			obj.ExportVoucherDetailslist = new List<ExportVoucherDetail>();
			decimal DetailAddLessAmount = default(decimal);
			if (grd.RowCount <= 0)
			{
				MessageBox.Show("Grid Fields Required");
				return false;
			}
			double TotalNetWeight = 0.0;
			double CompareTotalWeight = Conversion.ToDouble((object)txtTotalWeight.Text.Trim());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				ExportVoucherDetail item2 = new ExportVoucherDetail();
				obj.ExImInvoiceId = Conversion.ToInt(r4.Cells["ExImInvoiceId"].Value);
				item2.EntryTypeId = Conversion.ToInt(r4.Cells["EntryTypeId"].Value);
				item2.Id = Conversion.ToInt(r4.Cells["Id"].Value);
				item2.ExImInvoiceDetailId = Conversion.ToInt(r4.Cells["ExImInvoiceDetailId"].Value);
				item2.ContractId = Conversion.ToInt(r4.Cells["ContractId"].Value);
				item2.CropYearId = Conversion.ToInt(r4.Cells["CropYearId"].Value);
				item2.PackingTypeId = Conversion.ToInt(r4.Cells["PackingMaterialId"].Value);
				item2.ItemUomId = Conversion.ToInt(r4.Cells["OuterQtyUomId"].Value);
				item2.CostRate = Conversion.ToDecimal(r4.Cells["CostRate"].Value);
				if (CGSFlag && item2.CostRate <= 0m)
				{
					throw new Exception("CostRate Not Found Please Check");
				}
				item2.Qty = Conversion.ToDecimal(r4.Cells["Qty"].Value);
				item2.StockWeight = Conversion.ToDecimal(r4.Cells["StockWeight"].Value);
				item2.NetWeight = Conversion.ToDecimal(r4.Cells["NetWeight"].Value);
				item2.StockValue = item2.CostRate * item2.StockWeight;
				item2.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
				item2.LotJobId = Conversion.ToInt(r4.Cells["LotJobId"].Value);
				item2.WarehouseId = Conversion.ToInt(r4.Cells["WarehouseId"].Value);
				item2.Mton = Conversion.ToDecimal((object)item2.NetWeight) / 1000m;
				item2.RatePrice = Conversion.ToDecimal(r4.Cells["RatePrice"].Value);
				item2.RateUomId = Conversion.ToInt(r4.Cells["RateUomId"].Value);
				item2.CurrencyId = Conversion.ToInt(CmbCurrencyCode.Value);
				item2.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text.Trim());
				item2.FcyAmount = Conversion.ToDecimal(r4.Cells["FcAmount"].Value);
				item2.AddLessAmount = Conversion.ToDecimal(r4.Cells["AddLessAmount"].Value);
				item2.LcyAmount = item2.FcyAmount * item2.ExchangeRate;
				if (item2.EntryTypeId != 1)
				{
					item2.Remarks = Conversion.ToString((object)((Control)(object)CmbRefAccountId).Text) + " : " + Conversion.ToString(r4.Cells["ItemName"].Value) + " : " + Conversion.ToString((object)item2.Mton) + "/MTon : @" + ((Control)(object)CmbCurrencyCode).Text + "  " + Conversion.ToString((object)item2.RatePrice) + " : Invoice No: " + txtInvoiceNo.Text + " : Contract No: " + r4.Cells["ContractNo"].Value.ToString();
				}
				else
				{
					item2.Remarks = Conversion.ToString((object)((Control)(object)CmbRefAccountId).Text) + " : " + Conversion.ToString(r4.Cells["ItemName"].Value) + " @ " + ((Control)(object)CmbCurrencyCode).Text + "  " + Conversion.ToDecimal((object)item2.RatePrice) + " Invoice No: " + txtInvoiceNo.Text;
				}
				obj.ExportVoucherDetailslist.Add(item2);
				DetailAddLessAmount += item2.AddLessAmount;
				TotalNetWeight += Conversion.ToDouble(r4.Cells["NetWeight"].Value);
			}
			if (ToleranceWeightForDifferenceInDispatchedAndInvoice > 0.0)
			{
				if (Math.Floor(CompareTotalWeight) > Math.Floor(TotalNetWeight) + ToleranceWeightForDifferenceInDispatchedAndInvoice)
				{
					throw new Exception("TotalInvoiceWeight should be equal to SumOfGridNetWeight please Check");
				}
			}
			else if (Math.Abs(Math.Floor(CompareTotalWeight) - Math.Floor(TotalNetWeight)) > 0.5)
			{
				throw new Exception("TotalInvoiceWeight not equal to SumOfGridNetWeight please Check");
			}
			obj.NetWeight = Conversion.ToDecimal((object)TotalNetWeight);
			if (Math.Abs(Conversion.ToDecimal((object)obj.AddLessAmount) - DetailAddLessAmount) > 0.3m)
			{
				throw new Exception($"Header AddLess:{obj.AddLessAmount} not equal to detail AddLess:{DetailAddLessAmount}. Please Check!");
			}
			rows = grdPaymentTermDetail.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				ExImInvoicePaymentTermsDetail vdp2 = new ExImInvoicePaymentTermsDetail();
				vdp2.DocumentTypeId = Conversion.ToInt(r3.Cells["DocumentTypeId"].Value);
				vdp2.PaymentTermId = Conversion.ToInt(r3.Cells["PaymentTermId"].Value);
				if (vdp2.PaymentTermId == 1)
				{
					vdp2.ExImEFormRegistrationId = Conversion.ToInt(r3.Cells["ExImEFormRegistrationId"].Value);
				}
				vdp2.FinancialInstrumentNo = Conversion.ToString((object)r3.Cells["FinancialInstrumentNo"].Text);
				vdp2.PrcntOfTotal = Conversion.ToDouble(r3.Cells["%OfTotal"].Value);
				vdp2.DueDays = Conversion.ToInt(r3.Cells["DueDays"].Value);
				vdp2.FcyAmount = Conversion.ToDouble(r3.Cells["FcyAmount"].Value);
				vdp2.PaymentRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
				vdp2.SortNo = r3.RowIndex + 1;
				obj.ExImInvoicePaymentTermsDetailList.Add(vdp2);
			}
			rows = grdOtherChargesDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ExImInvoiceOtherChargesDetail vd = new ExImInvoiceOtherChargesDetail();
				vd.ChargesItemId = Conversion.ToInt(r2.Cells["ChargesItemId"].Value);
				vd.AddAmount = Conversion.ToDouble(r2.Cells["AddAmount"].Value);
				vd.LessAmount = Conversion.ToDouble(r2.Cells["LessAmount"].Value);
				vd.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
				vd.ChargesAccountId = Conversion.ToInt(r2.Cells["ChargesAccountId"].Value);
				vd.ChargesAccount = Conversion.ToString(r2.Cells["ChargesAccount"].Value);
				obj.ExImInvoiceOtherChargesDetailList.Add(vd);
			}
			if (grdCommissionInfo.RowCount > 0)
			{
				rows = grdCommissionInfo.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImInvoiceSaleManCommission vdp = new ExImInvoiceSaleManCommission();
					vdp.exImInvoiceSaleManCommissionId = Conversion.ToInt(r.Cells["Id"].Value);
					vdp.ActionTypeId = ((vdp.exImInvoiceSaleManCommissionId <= 0) ? 1 : 2);
					vdp.SalesPersonId = Conversion.ToInt(r.Cells["SalesPersonId"].Value);
					vdp.commissionTypeId = Conversion.ToInt(r.Cells["CommissionTypeId"].Value);
					vdp.commissionRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					vdp.rateUom = Conversion.ToString((object)r.Cells["RateUom"].Text);
					vdp.fcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					vdp.exchangeRate = Conversion.ToDouble(r.Cells["ExchangeRate"].Value);
					vdp.lcyAmount = Conversion.ToDouble(r.Cells["LcyAmount"].Value);
					vdp.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					obj.ExImInvoiceSaleManCommissionList.Add(vdp);
				}
			}
			if (lstRemoveRecord.Count > 0 && RecId > 0)
			{
				foreach (ExImInvoiceSaleManCommission item in lstRemoveRecord)
				{
					obj.ExImInvoiceSaleManCommissionList.Add(item);
				}
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExportVoucher>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = ExportVoucher.Save(obj);
			if (IsAutoUpdate)
			{
				Reset();
			}
			else
			{
				if (RecId == 0)
				{
					MessageBox.Show("Voucher Save Successfully...[" + obj.DocNo + "]");
				}
				else
				{
					MessageBox.Show("Voucher Update Successfully...[" + obj.DocNo + "]");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 27, 0);
					}
				}
				Reset();
				grd.DataSource = null;
				if (ChkBox.Checked)
				{
					CommonServices.ExportInvoiceVoucher_Slip(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return true;
	}

	private void Save_Click(object sender, EventArgs e)
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

	private void Update_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Rec Id Not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtvoucherno_TextChanged(object sender, EventArgs e)
	{
	}

	public void ReadById(int ID)
	{
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		try
		{
			Reset();
			RecId = ID;
			ExportVoucher vh = ExportVoucher.GetByID(ID);
			if (vh == null)
			{
				return;
			}
			Save.Visible = false;
			Update.Visible = true;
			if (!ForSpecialApproval)
			{
				BtnDelete.Visible = true;
			}
			tabControl1.SelectedIndex = 0;
			datVoucherDate.Value = vh.DocDate;
			txtVoucherCode.Text = Conversion.ToString((object)vh.DocNo);
			CmbRefAccountId.Value = vh.SupplierCustomerId;
			if (vh.CreditAccounId > 0)
			{
				CmbCreditAccount.Value = vh.CreditAccounId;
			}
			if (vh.CommissionExpenseAccountId > 0)
			{
				CmbCommissionExpenseAccount.Value = vh.CommissionExpenseAccountId;
			}
			CmbCurrencyCode.Value = vh.CurrencyId;
			ExImInvoiceId = vh.ExImInvoiceId;
			txtInvoiceNo.Text = Conversion.ToString((object)vh.InvoiceNo);
			if (vh.FcyAmountWithoutAddLess > 0.0)
			{
				txtFcyAmountWithoutAddLess.Text = vh.FcyAmountWithoutAddLess.ToString("#,#0.###");
			}
			else
			{
				txtFcyAmountWithoutAddLess.Text = vh.FcyAmount.ToString("#,#0.###");
			}
			txtAddLessAmount.Text = vh.AddLessAmount.ToString("#,#0.###");
			txtFcyAmount.Text = vh.FcyAmount.ToString("#,#0.###");
			txtExchangeRate.Text = vh.ExchangeRate.ToString("#,#0.###");
			txtRsAmount.Text = Conversion.ToString((object)vh.LcyAmount);
			txtDueDays.Text = Conversion.ToString((object)vh.DueDays);
			txtDueDate.Value = Conversion.ToDateTime((object)vh.DueDate);
			txtRemarks.Text = Conversion.ToString((object)vh.Remarks);
			Approved = vh.IsApproved;
			DataTable dtInv = ExImInvoice.GetInvoiceDate(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = vh.ExImInvoiceId
			});
			if (dtInv.Rows.Count > 0)
			{
				datInvoiceDate.Value = Conversion.ToDateTime(dtInv.Rows[0]["InvoiceDate"]);
				txtTotalWeight.Text = Conversion.ToString(dtInv.Rows[0]["NetWeight"]);
			}
			table.Rows.Clear();
			if (vh.ExportVoucherDetailslist.Count > 0)
			{
				for (int i = 0; i < vh.ExportVoucherDetailslist.Count; i++)
				{
					table.Rows.Add(vh.ExportVoucherDetailslist[i].EntryTypeId, vh.ExportVoucherDetailslist[i].Id, vh.ExportVoucherDetailslist[i].ExImInvoiceDetailId, vh.ExImInvoiceId, vh.ExportVoucherDetailslist[i].ContractId, vh.ExportVoucherDetailslist[i].ContractNo, vh.InvoiceNo, vh.ExportVoucherDetailslist[i].ItemId, vh.ExportVoucherDetailslist[i].ItemName, vh.ExportVoucherDetailslist[i].WarehouseId, vh.ExportVoucherDetailslist[i].CropYearId, vh.ExportVoucherDetailslist[i].CropYear, vh.ExportVoucherDetailslist[i].LotJobId, vh.ExportVoucherDetailslist[i].JobLotDescription, vh.ExportVoucherDetailslist[i].PackingTypeId, vh.ExportVoucherDetailslist[i].PackTypeDesc, vh.ExportVoucherDetailslist[i].Qty, vh.ExportVoucherDetailslist[i].ItemUomId, vh.ExportVoucherDetailslist[i].PackUom, vh.ExportVoucherDetailslist[i].NetWeight, vh.ExportVoucherDetailslist[i].RatePrice, vh.ExportVoucherDetailslist[i].RateUomId, vh.ExportVoucherDetailslist[i].RateUom, vh.ExportVoucherDetailslist[i].FcyAmount, vh.ExportVoucherDetailslist[i].StockWeight, vh.ExportVoucherDetailslist[i].CostRate, vh.ExportVoucherDetailslist[i].StockValue, vh.ExportVoucherDetailslist[i].AddLessAmount);
				}
			}
			dtPaymnentDetail.Clear();
			FillPaymentDetailFromInvoice(ExImInvoiceId, dtPaymnentDetail);
			dtOtherChargesDetail.Rows.Clear();
			FillOtherChargesDetailFromInvoice(ExImInvoiceId, dtOtherChargesDetail);
			dtCommission.Rows.Clear();
			FillCommissionDetailFromInvoice(ExImInvoiceId, dtCommission);
			BindGrids();
			UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtAddLessAmount);
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateAddLessAmountInHeaderFromChargesGrid(GridEX grid, TextBox AddLessText)
	{
		try
		{
			double columnSum = GridEX_Helper.GetColumnSum(grid, "AddAmount");
			double LessAmount = GridEX_Helper.GetColumnSum(grid, "LessAmount");
			AddLessText.Text = (columnSum - LessAmount).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillPaymentDetailFromInvoice(int InvoiceId, DataTable dt)
	{
		DataTable dtRecord = ExImInvoice.InvoicePaymentTermsDetailByInvoiceId(InvoiceId);
		if (dtRecord == null || dtRecord.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataRow r in dtRecord.Rows)
		{
			dt.Rows.Add(r["Id"], r["PaymentTermId"], r["PaymentTerm"], r["DocumentTypeId"], r["ExImEFormRegistrationId"], r["FinancialInstrumentNo"], r["PrcntOfTotal"], r["FcyAmount"], r["DueDays"], r["PaymentRemarks"]);
		}
	}

	private void FillOtherChargesDetailFromInvoice(int InvoiceId, DataTable dt)
	{
		DataTable dtRecord = ExImInvoice.ExImInvoiceOtherChargesDetail_ReadByHeaderId(InvoiceId);
		if (dtRecord == null || dtRecord.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataRow item in dtRecord.Rows)
		{
			dt.Rows.Add(item["Id"], item["ChargesItemId"], item["ChargesItemName"], item["AddAmount"], item["LessAmount"], item["ChargesAccountId"], item["ChargesAccount"], item["Remarks"]);
		}
	}

	private void FillCommissionDetailFromInvoice(int InvoiceId, DataTable dt)
	{
		DataTable dtRecord = ExImInvoice.InvoiceCommissionDetailByInvoiceId(InvoiceId);
		if (dtRecord == null || dtRecord.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataRow r in dtRecord.Rows)
		{
			dt.Rows.Add(r["exImInvoiceSaleManCommissionId"], r["SalesPersonId"], r["SalesPerson"], r["commissionTypeId"], r["CommissionType"], r["commissionRate"], r["rateUom"], r["fcyAmount"], r["exchangeRate"], r["lcyAmount"], r["Remarks"]);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record cannot be Delete, because ots approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 27,
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPending).Name);
			if (fs != null)
			{
				grdPending.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick_1(object sender, EventArgs e)
	{
		try
		{
			if (DataGridHistory.CurrentRow.RowIndex != -1)
			{
				int Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.BranchesId = UserAccount.BranchesId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.DocumentTypeId = 27;
			vh.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!vh.CanViewAllRecord)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateCpvHistory.Checked)
				{
					vh.FromDate = FromDateCpvHistory.Value;
				}
				if (ToDateCpvHistory.Checked)
				{
					vh.ToDate = ToDateCpvHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateCpvHistory.Checked)
				{
					vh.EntryDateFrom = FromDateCpvHistory.Value;
				}
				if (ToDateCpvHistory.Checked)
				{
					vh.EntryDateTo = ToDateCpvHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateCpvHistory.Checked)
				{
					vh.ModifyDateFrom = FromDateCpvHistory.Value;
				}
				if (ToDateCpvHistory.Checked)
				{
					vh.ModifyDateTo = ToDateCpvHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateCpvHistory.Checked)
				{
					vh.ApprovedDateFrom = FromDateCpvHistory.Value;
				}
				if (ToDateCpvHistory.Checked)
				{
					vh.ApprovedDateTo = ToDateCpvHistory.Value;
				}
			}
			vh.FromDocNo = Conversion.ToInt((object)txtFromDocNoCpv.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtToDocNoCpv.Text);
			vh.AccountId = Conversion.ToInt(cmbAccountTitleCpv.Value);
			if (((Control)(object)cmbApprovedStatusCpv).Text == "Approved")
			{
				vh.IsApproved = true;
			}
			if (((Control)(object)cmbApprovedStatusCpv).Text == "Not Approved")
			{
				vh.IsApproved = false;
			}
			if (((Control)(object)cmbApprovedStatusCpv).Text == "All")
			{
				vh.ApprovedFilter = "All";
			}
			if (ForSpecialApproval)
			{
				vh.IsSpecialApprovalRequired = 1;
			}
			DataTable lst = ExportVoucher.FormHistoryNew(vh);
			if (lst.Rows.Count > 0)
			{
				DataGridHistory.DataSource = null;
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("VoucherHeadId", typeof(int));
				dtHistory.Columns.Add("DocumentTypeId", typeof(int));
				dtHistory.Columns.Add("VoucherCode", typeof(int));
				dtHistory.Columns.Add("VoucherDate", typeof(DateTime));
				dtHistory.Columns.Add("InvoiceNo");
				dtHistory.Columns.Add("CustomerName");
				dtHistory.Columns.Add("Remarks");
				dtHistory.Columns.Add("NoOfContainers", typeof(int));
				dtHistory.Columns.Add("NetWeight", typeof(double));
				dtHistory.Columns.Add("CurrencyCode");
				dtHistory.Columns.Add("ExchangeRate", typeof(double));
				dtHistory.Columns.Add("FcyAmount", typeof(double));
				dtHistory.Columns.Add("VoucherAmount", typeof(double));
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("EntryUser");
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser");
				dtHistory.Columns.Add("ApprovalStatus");
				dtHistory.Columns.Add("NoOfAttachments");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["VoucherHeadId"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocNo"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["InvoiceNo"], lst.Rows[i]["CustomerName"], lst.Rows[i]["Remarks"], lst.Rows[i]["NoOfContainers"], lst.Rows[i]["NetWeight"], lst.Rows[i]["CurrencyCode"], lst.Rows[i]["ExchangeRate"], lst.Rows[i]["FcyAmount"], lst.Rows[i]["LcyAmount"], lst.Rows[i]["EntryDate"], lst.Rows[i]["EntryUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ApprovalStatus"], lst.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistory;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdDetail.ClearStructure();
				DataGridHistory.ClearStructure();
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["VoucherCode"].Width = Constants.ExportConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["VoucherDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant + 30;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.PartyName;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.ExportConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant + 20;
			DataGridHistory.RootTable.Columns["CurrencyCode"].Width = Constants.ExportConstants.FcyConstant;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant + 20;
			DataGridHistory.RootTable.Columns["VoucherAmount"].Width = Constants.ExportConstants.AmountConstant + 20;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["ApprovalStatus"].Width = Constants.ExportConstants.StatusConstant + 20;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm tt";
			DataGridHistory.RootTable.Columns["Remarks"].Width = 350;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns["Edit"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			((Control)(object)DataGridHistory).Refresh();
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Edit")
				{
					int Id = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(Id);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.ExportInvoiceVoucher_Slip(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDeailByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DataGridHistory.CurrentRow == null || (int)DataGridHistory.CurrentRow.RowType != 0 || DataGridHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			string obj = item.Cells["Id"].Value.ToString();
			DataTable table = new DataTable
			{
				Columns = 
				{
					{
						"ContractNo",
						typeof(string)
					},
					{
						"InvoiceNo",
						typeof(string)
					},
					{
						"ItemName",
						typeof(string)
					},
					{
						"CropYear",
						typeof(string)
					},
					{
						"JobLotCode",
						typeof(string)
					},
					{
						"PackMaterilaType",
						typeof(string)
					},
					{
						"Qty",
						typeof(double)
					},
					{
						"ItemUOM",
						typeof(string)
					},
					{
						"NetWeight",
						typeof(double)
					},
					{
						"RatePrice",
						typeof(double)
					},
					{
						"RateUOM",
						typeof(string)
					},
					{
						"FcAmount",
						typeof(double)
					},
					{
						"StockWeight",
						typeof(double)
					},
					{
						"CostRate",
						typeof(double)
					},
					{
						"StockValue",
						typeof(double)
					},
					{
						"AddLessAmount",
						typeof(double)
					}
				}
			};
			ExportVoucher vh = ExportVoucher.GetByID(Conversion.ToInt((object)obj));
			if (vh.ExportVoucherDetailslist.Count > 0)
			{
				for (int i = 0; i < vh.ExportVoucherDetailslist.Count; i++)
				{
					table.Rows.Add(vh.ExportVoucherDetailslist[i].ContractNo, vh.InvoiceNo, vh.ExportVoucherDetailslist[i].ItemName, vh.ExportVoucherDetailslist[i].CropYear, vh.ExportVoucherDetailslist[i].JobLotDescription, vh.ExportVoucherDetailslist[i].PackTypeDesc, vh.ExportVoucherDetailslist[i].Qty, vh.ExportVoucherDetailslist[i].PackUom, vh.ExportVoucherDetailslist[i].NetWeight, vh.ExportVoucherDetailslist[i].RatePrice, vh.ExportVoucherDetailslist[i].RateUom, vh.ExportVoucherDetailslist[i].FcyAmount, vh.ExportVoucherDetailslist[i].StockWeight, vh.ExportVoucherDetailslist[i].CostRate, vh.ExportVoucherDetailslist[i].StockValue, vh.ExportVoucherDetailslist[i].AddLessAmount);
				}
			}
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			if (FIFOCGS)
			{
				grdDetail.RootTable.Columns["CostRate"].Visible = false;
				grdDetail.RootTable.Columns["StockValue"].Visible = false;
			}
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockValue"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLessAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["StockWeight"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["RatePrice"].FormatString = "#,#0.####";
			grdDetail.RootTable.Columns["FcAmount"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["StockValue"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["CostRate"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["AddLessAmount"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,#.####";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,#.####";
			grdDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,#.####";
			grdDetail.RootTable.Columns["FcAmount"].TotalFormatString = "#,#.####";
			grdDetail.RootTable.Columns["StockValue"].TotalFormatString = "#,#.####";
			grdDetail.RootTable.Columns["AddLessAmount"].TotalFormatString = "#,#.####";
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RatePrice"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["StockValue"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CostRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLessAmount"].TextAlignment = (TextAlignment)3;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
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
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
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
		try
		{
			GetDeailByHeaderId();
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
			FromDateCpvHistory.Focus();
		}
		else
		{
			CmbCreditAccount.Focus();
		}
	}

	private void btnNewCpvHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			FromDateCpvHistory.Value = DateTime.Now;
			ToDateCpvHistory.Value = DateTime.Now;
			txtFromDocNoCpv.Text = string.Empty;
			txtToDocNoCpv.Text = string.Empty;
			((Control)(object)cmbAccountTitleCpv).Text = string.Empty;
			((UltraGridBase)cmbApprovedStatusCpv).Rows[0].Activate();
			VouchersInfoBoxCash.Visible = false;
			DataGridHistory.ClearStructure();
			grdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshCpvHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFillForHistory()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Approved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApprovedStatusCpv, "Id", "Status", "Status", ZeroIndex: false);
			if (!ForSpecialApproval)
			{
				((UltraGridBase)cmbApprovedStatusCpv).Rows[0].Activate();
				return;
			}
			((UltraGridBase)cmbApprovedStatusCpv).Rows[2].Activate();
			((Control)(object)cmbApprovedStatusCpv).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExportVoucher.GetDataForDropDownFromExportInvoiceVoucher(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.OrganizationId
			});
			DataTable dtAccount = new DataTable();
			dtAccount.Columns.Add("Id", typeof(int));
			dtAccount.Columns.Add("Customer");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Customer")
					{
						dtAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtAccount, cmbAccountTitleCpv, "Id", "Customer", "Customer Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowCpv_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton10_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		try
		{
			CommonServices.ExportInvoiceVoucher_Slip(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDebitAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtDueDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromDocNoCpv_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoCpv_KeyPress(object sender, KeyPressEventArgs e)
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

	private void AmountCalculation()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtFcyAmountWithoutAddLess.Text.Trim());
			double AddLessAmount = Conversion.ToDouble((object)txtAddLessAmount.Text.Trim());
			double TotalFcyAmount = num + AddLessAmount;
			txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatbothForFcy);
			double ExchangeRate = Conversion.ToDouble((object)txtExchangeRate.Text.Trim());
			double RsAmount = TotalFcyAmount * ExchangeRate;
			txtRsAmount.Text = RsAmount.ToString(clsGlobalVariables.stringFormatsingle);
			ProportionateAddLessAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProportionateAddLessAmount()
	{
		try
		{
			double TotalAddLessAmount = Conversion.ToDouble((object)txtAddLessAmount.Text);
			double TotalFcyAmount = GridEX_Helper.GetColumnSum(grd, "FcAmount");
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				double FcAmount = Conversion.ToDouble(obj.Cells["FcAmount"].Value);
				double addLess = TotalAddLessAmount / TotalFcyAmount * FcAmount;
				addLess = Math.Round(addLess, 4);
				obj.BeginEdit();
				obj.Cells["AddLessAmount"].Value = addLess;
				obj.EndEdit();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtFcyAmountWithoutAddLess_TextChanged(object sender, EventArgs e)
	{
		AmountCalculation();
	}

	private void txtAddLessAmount_TextChanged(object sender, EventArgs e)
	{
		AmountCalculation();
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtCommissionExchangeRate.Text = txtExchangeRate.Text;
			AmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmount_TextChanged(object sender, EventArgs e)
	{
		AmountCalculation();
	}

	private void datVoucherDate_Leave(object sender, EventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			grd.UpdateData();
			if (grd.GetRows().Count() > 0)
			{
				double AvgRate = 0.0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					if (SaleCostingJobOrderWise)
					{
						ReportsParameters obj = new ReportsParameters();
						obj.OrganizationId = UserAccount.OrganizationId;
						obj.CompanyId = UserAccount.CompanyId;
						obj.DocDate = Conversion.ToDateTime((object)datVoucherDate.Value);
						obj.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						obj.JobLotId = Conversion.ToInt(item.Cells["LotJobId"].Value);
						if (RecId > 0)
						{
							obj.DocumentTypeId = 27;
							obj.RecId = RecId;
						}
						AvgRate = CommonServies.getAvgRateByJobOrder(obj);
					}
					else
					{
						AvgRate = CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(item.Cells["ItemId"].Value), datVoucherDate.Value, 27, RecId, Conversion.ToInt(item.Cells["LotJobId"].Value), Conversion.ToInt(item.Cells["CropYearId"].Value), null, Conversion.ToInt(item.Cells["WarehouseId"].Value));
					}
					item.Cells["CostRate"].Value = AvgRate.ToString();
					item.Cells["StockValue"].Value = AvgRate * Conversion.ToDouble(item.Cells["StockWeight"].Value);
					item.EndEdit();
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				txtDueDate.Value = DateTime.Now.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				txtDueDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ExpenseVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			btnRecordsUpdate.Visible = false;
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (Save.Visible && Save.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				Save_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
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
			if (e.Control && e.Shift && e.KeyCode == Keys.F)
			{
				btnRecordsUpdate.Visible = true;
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (Update.Visible && Update.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				Update_Click(null, null);
			}
			if (BtnDelete.Visible && BtnDelete.Enabled && e.Control && e.Shift && e.KeyCode == Keys.Delete)
			{
				BtnDelete_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				AT.Show();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				GenerateReport(RecId);
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
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+P", "For Print ");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Debit Account in Detail Box");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				_ = item.RowType;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int ID = 0;
			int documentTypeId = 0;
			if (DataGridHistory.GetRows().Count() > 0)
			{
				ID = Conversion.ToInt(item.Cells["Id"].Value);
				documentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(ID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(ID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ExportInvoiceVoucher_Slip(ID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(ID, documentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPending_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPending.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.KeyData == Keys.Return)
				{
					SendKeys.Send("{TAB}");
				}
				if (e.Control && e.KeyCode == Keys.Space && grdPending.CurrentColumn.Key == "Edit")
				{
					PendingRecordbind(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommissionExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommissionExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = ExportVoucher.GetExportVoucherIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, 27);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				bool shouldContinue = true;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int recId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(recId);
					shouldContinue = Insert(IsAutoUpdate: true, recId);
					if (!shouldContinue)
					{
						break;
					}
				}
				if (shouldContinue)
				{
					MessageBox.Show("Records updated successfully.");
				}
				else
				{
					MessageBox.Show("Update stopped before all records were processed.");
				}
				btnRecordsUpdate.Visible = false;
				return;
			}
			btnRecordsUpdate.Visible = false;
			throw new Exception("Ids Not Found For Update");
		}
		catch (Exception ex)
		{
			btnRecordsUpdate.Visible = false;
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Expected O, but got Unknown
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Expected O, but got Unknown
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Expected O, but got Unknown
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_1450: Unknown result type (might be due to invalid IL or missing references)
		//IL_145a: Expected O, but got Unknown
		//IL_187a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1884: Expected O, but got Unknown
		//IL_213c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2146: Expected O, but got Unknown
		//IL_64a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_64ad: Expected O, but got Unknown
		//IL_64d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_64db: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmExportInvoiceVoucher));
		GridEXLayout grdPending_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTermDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherChargesDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdCommissionInfo_DesignTimeLayout = new GridEXLayout();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
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
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnEdit = new System.Windows.Forms.ToolStripButton();
		this.btnS = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnDte = new System.Windows.Forms.ToolStripButton();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.btnPaste = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdPending = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label16 = new System.Windows.Forms.Label();
		this.tabPaymentDetail = new System.Windows.Forms.TabPage();
		this.grdPaymentTermDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlgrdPaymentTermDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.tabChargesDetail = new System.Windows.Forms.TabPage();
		this.grdOtherChargesDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlgrdOtherChargesDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.tabCommission = new System.Windows.Forms.TabPage();
		this.grdCommissionInfo = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label116 = new System.Windows.Forms.Label();
		this.txtCommissionRemarks = new System.Windows.Forms.TextBox();
		this.btnAddCommission = new System.Windows.Forms.Button();
		this.btnCancelCommission = new System.Windows.Forms.Button();
		this.txtCommRateUom = new System.Windows.Forms.TextBox();
		this.txtCommissionLcyAmount = new System.Windows.Forms.TextBox();
		this.label115 = new System.Windows.Forms.Label();
		this.label114 = new System.Windows.Forms.Label();
		this.txtCommissionExchangeRate = new System.Windows.Forms.TextBox();
		this.txtCommRate = new System.Windows.Forms.TextBox();
		this.txtCommissionAmount = new System.Windows.Forms.TextBox();
		this.cmbCommissionType = new UltraCombo();
		this.label110 = new System.Windows.Forms.Label();
		this.label111 = new System.Windows.Forms.Label();
		this.label112 = new System.Windows.Forms.Label();
		this.label113 = new System.Windows.Forms.Label();
		this.label109 = new System.Windows.Forms.Label();
		this.CmbSalePerson = new UltraCombo();
		this.btnUpdateCommision = new System.Windows.Forms.Button();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label108 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.CmbCommissionExpenseAccount = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtAddLessAmount = new System.Windows.Forms.TextBox();
		this.txtVoucherCode = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtFcyAmountWithoutAddLess = new System.Windows.Forms.TextBox();
		this.CmbRefAccountId = new UltraCombo();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDueDate = new System.Windows.Forms.DateTimePicker();
		this.label18 = new System.Windows.Forms.Label();
		this.datInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtTotalWeight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbCreditAccount = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.datVoucherDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbCurrencyCode = new UltraCombo();
		this.txtRsAmount = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtInvoiceNo = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ExpenseVoucherLable = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.Update = new System.Windows.Forms.ToolStripButton();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.History = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.VouchersInfoBoxCash = new System.Windows.Forms.GroupBox();
		this.label42 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchers = new System.Windows.Forms.Label();
		this.lblTotalVouchers = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.lblApprovedVouchers = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtToDocNoCpv = new System.Windows.Forms.TextBox();
		this.txtFromDocNoCpv = new System.Windows.Forms.TextBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbAccountTitleCpv = new UltraCombo();
		this.btnShowCpv = new System.Windows.Forms.Button();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.cmbApprovedStatusCpv = new UltraCombo();
		this.ToDateCpvHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateCpvHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNewCpvHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshCpvHistory = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPending).BeginInit();
		this.panel15.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.tabPaymentDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermDetail).BeginInit();
		this.panel20.SuspendLayout();
		this.tabChargesDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).BeginInit();
		this.panel3.SuspendLayout();
		this.tabCommission.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCommissionInfo).BeginInit();
		this.panel25.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).BeginInit();
		this.panel13.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionExpenseAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefAccountId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyCode).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.History.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel16.SuspendLayout();
		this.panel9.SuspendLayout();
		this.VouchersInfoBoxCash.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitleCpv).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusCpv).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(718, 1);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1830, 27);
		this.miniToolStrip.TabIndex = 46;
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnNew, this.btnEdit, this.btnS, this.btnPrint, this.toolStripSeparator, this.btnDte, this.btnCopy, this.btnPaste, this.toolStripSeparator1, this.toolStripButton1,
			this.toolStripButton2, this.helpToolStripButton
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1830, 27);
		this.toolStrip1.TabIndex = 46;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnEdit.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.Size = new System.Drawing.Size(51, 24);
		this.btnEdit.Text = "&Edit";
		this.btnS.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnS.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnS.Name = "btnS";
		this.btnS.Size = new System.Drawing.Size(55, 24);
		this.btnS.Text = "&Save";
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "&Print";
		this.btnDte.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDte.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDte.Name = "btnDte";
		this.btnDte.Size = new System.Drawing.Size(64, 24);
		this.btnDte.Text = "&Delete";
		this.btnCopy.Image = Architecture.WinApp.Properties.Resources.icons8_copy_16;
		this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.Size = new System.Drawing.Size(59, 24);
		this.btnCopy.Text = "&Copy";
		this.btnPaste.Image = (System.Drawing.Image)resources.GetObject("btnPaste.Image");
		this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPaste.Name = "btnPaste";
		this.btnPaste.Size = new System.Drawing.Size(59, 24);
		this.btnPaste.Text = "&Paste";
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton1.Text = "&Attachment";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton2.Text = "&SMS";
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(56, 24);
		this.helpToolStripButton.Text = "He&lp";
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1448, 656);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1448, 656);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel5);
		this.Form.Controls.Add(this.tabControl2);
		this.Form.Controls.Add(this.panel2);
		this.Form.Controls.Add(this.panel4);
		this.Form.Controls.Add(this.toolStrip2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1440, 627);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdPending);
		this.panel5.Controls.Add(this.panel15);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 452);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1436, 173);
		this.panel5.TabIndex = 7;
		this.grdPending.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPending).BackColor = System.Drawing.Color.White;
		grdPending_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPending.DesignTimeLayout = grdPending_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPending).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPending.FilterMode = (FilterMode)2;
		this.grdPending.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPending).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPending.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPending).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdPending).Name = "grdPending";
		((ScrollableBase)this.grdPending).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPending).Size = new System.Drawing.Size(1436, 149);
		((System.Windows.Forms.Control)(object)this.grdPending).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdPending).TabStop = false;
		this.grdPending.TotalRow = (InheritableBoolean)1;
		this.grdPending.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPending).VisualStyle = (VisualStyle)4;
		this.grdPending.ColumnButtonClick += new ColumnActionEventHandler(grdPending_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPending).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPending_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar4);
		this.panel15.Controls.Add(this.label6);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1436, 24);
		this.panel15.TabIndex = 5;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1388, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdPending;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar4.TabIndex = 20;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(6, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(205, 16);
		this.label6.TabIndex = 0;
		this.label6.Text = "Pending For Accounts GL Entry";
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabPaymentDetail);
		this.tabControl2.Controls.Add(this.tabChargesDetail);
		this.tabControl2.Controls.Add(this.tabCommission);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(2, 193);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1436, 259);
		this.tabControl2.TabIndex = 6;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.panel6);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Size = new System.Drawing.Size(1428, 230);
		this.tabDetail.TabIndex = 4;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1428, 206);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1428, 24);
		this.panel6.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1379, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 4);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(44, 16);
		this.label16.TabIndex = 0;
		this.label16.Text = "Detail";
		this.tabPaymentDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail);
		this.tabPaymentDetail.Controls.Add(this.panel20);
		this.tabPaymentDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetail.Name = "tabPaymentDetail";
		this.tabPaymentDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentDetail.Size = new System.Drawing.Size(1428, 230);
		this.tabPaymentDetail.TabIndex = 2;
		this.tabPaymentDetail.Text = "Payment Detail";
		this.tabPaymentDetail.UseVisualStyleBackColor = true;
		this.grdPaymentTermDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentTermDetail).BackColor = System.Drawing.Color.White;
		grdPaymentTermDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTermDetail.DesignTimeLayout = grdPaymentTermDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTermDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Name = "grdPaymentTermDetail";
		((ScrollableBase)this.grdPaymentTermDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdPaymentTermDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdPaymentTermDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Size = new System.Drawing.Size(1422, 201);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).TabIndex = 1;
		this.grdPaymentTermDetail.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTermDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTermDetail).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlgrdPaymentTermDetail);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 3);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1422, 23);
		this.panel20.TabIndex = 0;
		this.ctrlgrdPaymentTermDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdPaymentTermDetail.FormName = null;
		this.ctrlgrdPaymentTermDetail.Location = new System.Drawing.Point(1379, 1);
		this.ctrlgrdPaymentTermDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdPaymentTermDetail.MyGrid = this.grdPaymentTermDetail;
		this.ctrlgrdPaymentTermDetail.Name = "ctrlgrdPaymentTermDetail";
		this.ctrlgrdPaymentTermDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdPaymentTermDetail.TabIndex = 27;
		this.tabChargesDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail);
		this.tabChargesDetail.Controls.Add(this.panel3);
		this.tabChargesDetail.Location = new System.Drawing.Point(4, 25);
		this.tabChargesDetail.Name = "tabChargesDetail";
		this.tabChargesDetail.Size = new System.Drawing.Size(1428, 230);
		this.tabChargesDetail.TabIndex = 3;
		this.tabChargesDetail.Text = "Other Charges Detail";
		this.tabChargesDetail.UseVisualStyleBackColor = true;
		this.grdOtherChargesDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOtherChargesDetail).BackColor = System.Drawing.Color.White;
		grdOtherChargesDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherChargesDetail.DesignTimeLayout = grdOtherChargesDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherChargesDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Name = "grdOtherChargesDetail";
		((ScrollableBase)this.grdOtherChargesDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdOtherChargesDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdOtherChargesDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Size = new System.Drawing.Size(1428, 207);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).TabIndex = 1;
		this.grdOtherChargesDetail.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesDetail).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlgrdOtherChargesDetail);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1428, 23);
		this.panel3.TabIndex = 2;
		this.ctrlgrdOtherChargesDetail.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdOtherChargesDetail.FormName = null;
		this.ctrlgrdOtherChargesDetail.Location = new System.Drawing.Point(1386, 0);
		this.ctrlgrdOtherChargesDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdOtherChargesDetail.MyGrid = this.grdOtherChargesDetail;
		this.ctrlgrdOtherChargesDetail.Name = "ctrlgrdOtherChargesDetail";
		this.ctrlgrdOtherChargesDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdOtherChargesDetail.TabIndex = 27;
		this.tabCommission.Controls.Add((System.Windows.Forms.Control)(object)this.grdCommissionInfo);
		this.tabCommission.Controls.Add(this.panel25);
		this.tabCommission.Controls.Add(this.panel13);
		this.tabCommission.Location = new System.Drawing.Point(4, 25);
		this.tabCommission.Name = "tabCommission";
		this.tabCommission.Size = new System.Drawing.Size(1428, 230);
		this.tabCommission.TabIndex = 5;
		this.tabCommission.Text = "Commission Info";
		this.tabCommission.UseVisualStyleBackColor = true;
		this.grdCommissionInfo.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdCommissionInfo).BackColor = System.Drawing.Color.White;
		grdCommissionInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCommissionInfo.DesignTimeLayout = grdCommissionInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCommissionInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Location = new System.Drawing.Point(0, 73);
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Name = "grdCommissionInfo";
		((ScrollableBase)this.grdCommissionInfo).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdCommissionInfo).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdCommissionInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Size = new System.Drawing.Size(1428, 157);
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).TabIndex = 4;
		this.grdCommissionInfo.TotalRow = (InheritableBoolean)1;
		this.grdCommissionInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCommissionInfo).VisualStyle = (VisualStyle)4;
		this.grdCommissionInfo.ColumnButtonClick += new ColumnActionEventHandler(grdCommissionInfo_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).DoubleClick += new System.EventHandler(grdCommissionInfo_DoubleClick);
		this.panel25.Controls.Add(this.label116);
		this.panel25.Controls.Add(this.txtCommissionRemarks);
		this.panel25.Controls.Add(this.btnAddCommission);
		this.panel25.Controls.Add(this.btnCancelCommission);
		this.panel25.Controls.Add(this.txtCommRateUom);
		this.panel25.Controls.Add(this.txtCommissionLcyAmount);
		this.panel25.Controls.Add(this.label115);
		this.panel25.Controls.Add(this.label114);
		this.panel25.Controls.Add(this.txtCommissionExchangeRate);
		this.panel25.Controls.Add(this.txtCommRate);
		this.panel25.Controls.Add(this.txtCommissionAmount);
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommissionType);
		this.panel25.Controls.Add(this.label110);
		this.panel25.Controls.Add(this.label111);
		this.panel25.Controls.Add(this.label112);
		this.panel25.Controls.Add(this.label113);
		this.panel25.Controls.Add(this.label109);
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalePerson);
		this.panel25.Controls.Add(this.btnUpdateCommision);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 21);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1428, 52);
		this.panel25.TabIndex = 3;
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label116.Location = new System.Drawing.Point(789, 5);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(52, 15);
		this.label116.TabIndex = 604;
		this.label116.Text = "Remarks";
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(792, 25);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(250, 23);
		this.txtCommissionRemarks.TabIndex = 7;
		this.btnAddCommission.BackColor = System.Drawing.Color.Teal;
		this.btnAddCommission.FlatAppearance.BorderSize = 0;
		this.btnAddCommission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddCommission.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddCommission.ForeColor = System.Drawing.Color.White;
		this.btnAddCommission.Location = new System.Drawing.Point(1045, 25);
		this.btnAddCommission.Name = "btnAddCommission";
		this.btnAddCommission.Size = new System.Drawing.Size(31, 22);
		this.btnAddCommission.TabIndex = 8;
		this.btnAddCommission.Text = "+";
		this.btnAddCommission.UseVisualStyleBackColor = false;
		this.btnAddCommission.Click += new System.EventHandler(btnAddCommission_Click);
		this.btnCancelCommission.BackColor = System.Drawing.Color.Teal;
		this.btnCancelCommission.FlatAppearance.BorderSize = 0;
		this.btnCancelCommission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelCommission.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelCommission.ForeColor = System.Drawing.Color.White;
		this.btnCancelCommission.Location = new System.Drawing.Point(1104, 25);
		this.btnCancelCommission.Name = "btnCancelCommission";
		this.btnCancelCommission.Size = new System.Drawing.Size(55, 22);
		this.btnCancelCommission.TabIndex = 10;
		this.btnCancelCommission.Text = "Cancel";
		this.btnCancelCommission.UseVisualStyleBackColor = false;
		this.btnCancelCommission.Visible = false;
		this.btnCancelCommission.Click += new System.EventHandler(btnCancelCommission_Click);
		this.txtCommRateUom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRateUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommRateUom.Location = new System.Drawing.Point(390, 25);
		this.txtCommRateUom.Name = "txtCommRateUom";
		this.txtCommRateUom.Size = new System.Drawing.Size(87, 23);
		this.txtCommRateUom.TabIndex = 3;
		this.txtCommRateUom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRateUom.TextChanged += new System.EventHandler(txtCommRateUom_TextChanged);
		this.txtCommRateUom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommRateUom_KeyPress);
		this.txtCommissionLcyAmount.BackColor = System.Drawing.Color.White;
		this.txtCommissionLcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionLcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionLcyAmount.Location = new System.Drawing.Point(684, 25);
		this.txtCommissionLcyAmount.Name = "txtCommissionLcyAmount";
		this.txtCommissionLcyAmount.ReadOnly = true;
		this.txtCommissionLcyAmount.Size = new System.Drawing.Size(107, 23);
		this.txtCommissionLcyAmount.TabIndex = 6;
		this.txtCommissionLcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label115.AutoSize = true;
		this.label115.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label115.Location = new System.Drawing.Point(684, 5);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(72, 15);
		this.label115.TabIndex = 598;
		this.label115.Text = "Lcy Amount";
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label114.Location = new System.Drawing.Point(580, 5);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(84, 15);
		this.label114.TabIndex = 597;
		this.label114.Text = "Exchange Rate";
		this.txtCommissionExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionExchangeRate.Location = new System.Drawing.Point(580, 25);
		this.txtCommissionExchangeRate.Name = "txtCommissionExchangeRate";
		this.txtCommissionExchangeRate.Size = new System.Drawing.Size(103, 23);
		this.txtCommissionExchangeRate.TabIndex = 5;
		this.txtCommissionExchangeRate.Text = "0";
		this.txtCommissionExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommissionExchangeRate.TextChanged += new System.EventHandler(txtCommissionExchangeRate_TextChanged);
		this.txtCommissionExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommissionExchangeRate_KeyPress);
		this.txtCommRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommRate.Location = new System.Drawing.Point(312, 25);
		this.txtCommRate.Name = "txtCommRate";
		this.txtCommRate.Size = new System.Drawing.Size(77, 23);
		this.txtCommRate.TabIndex = 2;
		this.txtCommRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRate.TextChanged += new System.EventHandler(txtCommRate_TextChanged);
		this.txtCommRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommRate_KeyPress);
		this.txtCommissionAmount.BackColor = System.Drawing.Color.White;
		this.txtCommissionAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionAmount.Location = new System.Drawing.Point(478, 25);
		this.txtCommissionAmount.Name = "txtCommissionAmount";
		this.txtCommissionAmount.ReadOnly = true;
		this.txtCommissionAmount.Size = new System.Drawing.Size(101, 23);
		this.txtCommissionAmount.TabIndex = 4;
		this.txtCommissionAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommissionAmount.TextChanged += new System.EventHandler(txtCommissionAmount_TextChanged);
		this.cmbCommissionType.AllowNull = (DefaultableBoolean)1;
		this.cmbCommissionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommissionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommissionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCommissionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).Location = new System.Drawing.Point(189, 23);
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).Name = "cmbCommissionType";
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).TextChanged += new System.EventHandler(cmbCommissionType_TextChanged);
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label110.Location = new System.Drawing.Point(387, 5);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(61, 15);
		this.label110.TabIndex = 475;
		this.label110.Text = "Rate UOM";
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label111.Location = new System.Drawing.Point(189, 5);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(101, 15);
		this.label111.TabIndex = 473;
		this.label111.Text = "Commission Type";
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label112.Location = new System.Drawing.Point(478, 5);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(72, 15);
		this.label112.TabIndex = 468;
		this.label112.Text = "Fcy Amount";
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label113.Location = new System.Drawing.Point(313, 5);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(30, 15);
		this.label113.TabIndex = 469;
		this.label113.Text = "Rate";
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label109.Location = new System.Drawing.Point(3, 5);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(73, 15);
		this.label109.TabIndex = 467;
		this.label109.Text = "Sales Person";
		this.CmbSalePerson.AllowNull = (DefaultableBoolean)1;
		this.CmbSalePerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalePerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalePerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalePerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Location = new System.Drawing.Point(3, 23);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Name = "CmbSalePerson";
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).TabIndex = 0;
		this.btnUpdateCommision.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateCommision.FlatAppearance.BorderSize = 0;
		this.btnUpdateCommision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateCommision.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateCommision.ForeColor = System.Drawing.Color.White;
		this.btnUpdateCommision.Location = new System.Drawing.Point(1043, 25);
		this.btnUpdateCommision.Name = "btnUpdateCommision";
		this.btnUpdateCommision.Size = new System.Drawing.Size(60, 22);
		this.btnUpdateCommision.TabIndex = 9;
		this.btnUpdateCommision.Text = "Update";
		this.btnUpdateCommision.UseVisualStyleBackColor = false;
		this.btnUpdateCommision.Visible = false;
		this.btnUpdateCommision.Click += new System.EventHandler(btnUpdateCommision_Click);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar5);
		this.panel13.Controls.Add(this.label108);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1428, 21);
		this.panel13.TabIndex = 1;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1388, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar5.MyGrid = this.grdCommissionInfo;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar5.TabIndex = 28;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.ForeColor = System.Drawing.Color.White;
		this.label108.Location = new System.Drawing.Point(3, 3);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(304, 16);
		this.label108.TabIndex = 27;
		this.label108.Text = "Commission Agent / Sales Person Information";
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionExpenseAccount);
		this.panel2.Controls.Add(this.label19);
		this.panel2.Controls.Add(this.txtAddLessAmount);
		this.panel2.Controls.Add(this.txtVoucherCode);
		this.panel2.Controls.Add(this.txtExchangeRate);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.txtFcyAmountWithoutAddLess);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefAccountId);
		this.panel2.Controls.Add(this.txtFcyAmount);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.txtDueDate);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.datInvoiceDate);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.label14);
		this.panel2.Controls.Add(this.txtDueDays);
		this.panel2.Controls.Add(this.txtRemarks);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add(this.txtTotalWeight);
		this.panel2.Controls.Add(this.label15);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.datVoucherDate);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.label12);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrencyCode);
		this.panel2.Controls.Add(this.txtRsAmount);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.txtInvoiceNo);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 51);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1436, 142);
		this.panel2.TabIndex = 0;
		this.CmbCommissionExpenseAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionExpenseAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionExpenseAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionExpenseAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionExpenseAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionExpenseAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommissionExpenseAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionExpenseAccount).Location = new System.Drawing.Point(104, 110);
		((System.Windows.Forms.Control)(object)this.CmbCommissionExpenseAccount).Name = "CmbCommissionExpenseAccount";
		((System.Windows.Forms.Control)(object)this.CmbCommissionExpenseAccount).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionExpenseAccount).TabIndex = 5;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(7, 108);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(73, 30);
		this.label19.TabIndex = 44;
		this.label19.Text = "Commission\r\nExpense Ac";
		this.txtAddLessAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessAmount.Enabled = false;
		this.txtAddLessAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLessAmount.Location = new System.Drawing.Point(432, 30);
		this.txtAddLessAmount.Name = "txtAddLessAmount";
		this.txtAddLessAmount.Size = new System.Drawing.Size(140, 23);
		this.txtAddLessAmount.TabIndex = 7;
		this.txtAddLessAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessAmount.TextChanged += new System.EventHandler(txtAddLessAmount_TextChanged);
		this.txtVoucherCode.BackColor = System.Drawing.Color.White;
		this.txtVoucherCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoucherCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVoucherCode.Location = new System.Drawing.Point(104, 3);
		this.txtVoucherCode.Name = "txtVoucherCode";
		this.txtVoucherCode.ReadOnly = true;
		this.txtVoucherCode.Size = new System.Drawing.Size(99, 23);
		this.txtVoucherCode.TabIndex = 0;
		this.txtVoucherCode.Leave += new System.EventHandler(txtvoucherno_TextChanged);
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(432, 85);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(140, 23);
		this.txtExchangeRate.TabIndex = 9;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(7, 34);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Buyer Name";
		this.txtFcyAmountWithoutAddLess.BackColor = System.Drawing.Color.White;
		this.txtFcyAmountWithoutAddLess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountWithoutAddLess.Enabled = false;
		this.txtFcyAmountWithoutAddLess.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmountWithoutAddLess.Location = new System.Drawing.Point(432, 3);
		this.txtFcyAmountWithoutAddLess.Name = "txtFcyAmountWithoutAddLess";
		this.txtFcyAmountWithoutAddLess.ReadOnly = true;
		this.txtFcyAmountWithoutAddLess.Size = new System.Drawing.Size(140, 23);
		this.txtFcyAmountWithoutAddLess.TabIndex = 6;
		this.txtFcyAmountWithoutAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountWithoutAddLess.TextChanged += new System.EventHandler(txtFcyAmountWithoutAddLess_TextChanged);
		this.CmbRefAccountId.AllowNull = (DefaultableBoolean)1;
		this.CmbRefAccountId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefAccountId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefAccountId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRefAccountId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Location = new System.Drawing.Point(104, 28);
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Name = "CmbRefAccountId";
		this.CmbRefAccountId.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).TabIndex = 2;
		this.txtFcyAmount.BackColor = System.Drawing.Color.White;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(432, 58);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ReadOnly = true;
		this.txtFcyAmount.Size = new System.Drawing.Size(140, 23);
		this.txtFcyAmount.TabIndex = 8;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.TextChanged += new System.EventHandler(txtFcyAmount_TextChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(576, 89);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Remarks";
		this.txtDueDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDate.CustomFormat = "dd-MMM-yy";
		this.txtDueDate.Enabled = false;
		this.txtDueDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDueDate.Location = new System.Drawing.Point(647, 58);
		this.txtDueDate.Name = "txtDueDate";
		this.txtDueDate.Size = new System.Drawing.Size(146, 23);
		this.txtDueDate.TabIndex = 12;
		this.txtDueDate.TabStop = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(329, 7);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(72, 15);
		this.label18.TabIndex = 35;
		this.label18.Text = "Fcy Amount";
		this.datInvoiceDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datInvoiceDate.CustomFormat = "dd-MMM-yy";
		this.datInvoiceDate.Enabled = false;
		this.datInvoiceDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.datInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datInvoiceDate.Location = new System.Drawing.Point(876, 30);
		this.datInvoiceDate.Name = "datInvoiceDate";
		this.datInvoiceDate.Size = new System.Drawing.Size(141, 23);
		this.datInvoiceDate.TabIndex = 15;
		this.datInvoiceDate.TabStop = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(329, 62);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(101, 15);
		this.label10.TabIndex = 35;
		this.label10.Text = "Total Fcy Amount";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(576, 62);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(60, 15);
		this.label14.TabIndex = 5;
		this.label14.Text = "Due  Date";
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDueDays.Location = new System.Drawing.Point(647, 30);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(146, 23);
		this.txtDueDays.TabIndex = 11;
		this.txtDueDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.txtDueDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDays_KeyPress);
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(647, 84);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(370, 53);
		this.txtRemarks.TabIndex = 13;
		this.txtRemarks.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(798, 34);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(74, 15);
		this.label11.TabIndex = 5;
		this.label11.Text = "Invoice Date";
		this.txtTotalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalWeight.Location = new System.Drawing.Point(876, 58);
		this.txtTotalWeight.Name = "txtTotalWeight";
		this.txtTotalWeight.ReadOnly = true;
		this.txtTotalWeight.Size = new System.Drawing.Size(141, 23);
		this.txtTotalWeight.TabIndex = 16;
		this.txtTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalWeight.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(7, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(98, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Voucher No,Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(329, 89);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 37;
		this.label3.Text = "Exchange Rate";
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(104, 56);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(329, 34);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(98, 15);
		this.label5.TabIndex = 37;
		this.label5.Text = "AddLess Amount";
		this.datVoucherDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datVoucherDate.CustomFormat = "dd-MMM-yy";
		this.datVoucherDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.datVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datVoucherDate.Location = new System.Drawing.Point(204, 3);
		this.datVoucherDate.Name = "datVoucherDate";
		this.datVoucherDate.Size = new System.Drawing.Size(122, 23);
		this.datVoucherDate.TabIndex = 1;
		this.datVoucherDate.Leave += new System.EventHandler(datVoucherDate_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(576, 34);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(58, 15);
		this.label13.TabIndex = 37;
		this.label13.Text = "Due Days";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(7, 62);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(78, 15);
		this.label9.TabIndex = 6;
		this.label9.Text = "Account Title";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(798, 62);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(75, 15);
		this.label12.TabIndex = 37;
		this.label12.Text = "Total Weight";
		this.CmbCurrencyCode.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrencyCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrencyCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrencyCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)Appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Appearance = (AppearanceBase)(object)Appearance1;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)Appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)Appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)Appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)Appearance5;
		((AppearanceBase)Appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)Appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)Appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)Appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)Appearance7;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)Appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)Appearance8;
		((AppearanceBase)Appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)Appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)Appearance9;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)Appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)Appearance10;
		((AppearanceBase)Appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)Appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)Appearance11;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)Appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)Appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)Appearance12;
		((AppearanceBase)Appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)Appearance2;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)Appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)Appearance3;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)Appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)Appearance4;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCurrencyCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Location = new System.Drawing.Point(104, 83);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Name = "CmbCurrencyCode";
		this.CmbCurrencyCode.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).TabStop = false;
		this.txtRsAmount.BackColor = System.Drawing.Color.White;
		this.txtRsAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRsAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRsAmount.Location = new System.Drawing.Point(647, 3);
		this.txtRsAmount.Name = "txtRsAmount";
		this.txtRsAmount.ReadOnly = true;
		this.txtRsAmount.Size = new System.Drawing.Size(146, 23);
		this.txtRsAmount.TabIndex = 10;
		this.txtRsAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(7, 89);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(84, 15);
		this.label8.TabIndex = 42;
		this.label8.Text = "Currency Code";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(576, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(69, 15);
		this.label4.TabIndex = 39;
		this.label4.Text = "Rs. Amount";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(798, 7);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(62, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "InvoiceNo";
		this.txtInvoiceNo.BackColor = System.Drawing.Color.White;
		this.txtInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInvoiceNo.Location = new System.Drawing.Point(876, 3);
		this.txtInvoiceNo.Name = "txtInvoiceNo";
		this.txtInvoiceNo.ReadOnly = true;
		this.txtInvoiceNo.Size = new System.Drawing.Size(141, 23);
		this.txtInvoiceNo.TabIndex = 14;
		this.txtInvoiceNo.TabStop = false;
		this.txtInvoiceNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.btnRecordsUpdate);
		this.panel4.Controls.Add(this.ExpenseVoucherLable);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 29);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1436, 22);
		this.panel4.TabIndex = 2;
		this.ExpenseVoucherLable.AutoSize = true;
		this.ExpenseVoucherLable.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpenseVoucherLable.ForeColor = System.Drawing.Color.White;
		this.ExpenseVoucherLable.Location = new System.Drawing.Point(3, 2);
		this.ExpenseVoucherLable.Name = "ExpenseVoucherLable";
		this.ExpenseVoucherLable.Size = new System.Drawing.Size(236, 19);
		this.ExpenseVoucherLable.TabIndex = 0;
		this.ExpenseVoucherLable.Text = "EXPORT INVOICE VOUCHER";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBox.Location = new System.Drawing.Point(1313, 1);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.TabStop = false;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.New, this.btnRefresh, this.toolStripSeparator3, this.Update, this.BtnDelete, this.Save, this.toolStripSeparator4, this.toolStripButton10, this.toolStripSeparator5, this.Print,
			this.toolStripSeparator2, this.btnShortCutKey
		});
		this.toolStrip2.Location = new System.Drawing.Point(2, 2);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1436, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(59, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.Update.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.Update.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Update.Name = "Update";
		this.Update.Size = new System.Drawing.Size(78, 24);
		this.Update.Text = "&Update";
		this.Update.Visible = false;
		this.Update.Click += new System.EventHandler(Update_Click);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(73, 24);
		this.BtnDelete.Text = "&Delete";
		this.BtnDelete.Visible = false;
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton10.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton10.Name = "toolStripButton10";
		this.toolStripButton10.Size = new System.Drawing.Size(109, 24);
		this.toolStripButton10.Text = "&Attachment";
		this.toolStripButton10.Click += new System.EventHandler(toolStripButton10_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "102-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.History.Controls.Add(this.panel17);
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.History.Controls.Add(this.panel16);
		this.History.Controls.Add(this.panel9);
		this.History.Controls.Add(this.panel8);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Margin = new System.Windows.Forms.Padding(2);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(2);
		this.History.Size = new System.Drawing.Size(1440, 627);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 155);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1436, 282);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick_1);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(2, 437);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1436, 21);
		this.panel17.TabIndex = 46;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(2, 458);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1436, 167);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label17);
		this.panel16.Controls.Add(this.ctrlGrdBar3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(2, 129);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1436, 26);
		this.panel16.TabIndex = 45;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(3, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(142, 19);
		this.label17.TabIndex = 53;
		this.label17.Text = "Filtered Records";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1384, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(46, 23);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.TabStop = false;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel9.Controls.Add(this.VouchersInfoBoxCash);
		this.panel9.Controls.Add(this.groupBox1);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(2, 54);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1436, 75);
		this.panel9.TabIndex = 46;
		this.VouchersInfoBoxCash.Controls.Add(this.label42);
		this.VouchersInfoBoxCash.Controls.Add(this.lblUnApprovedVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.lblTotalVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.label45);
		this.VouchersInfoBoxCash.Controls.Add(this.lblApprovedVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.label47);
		this.VouchersInfoBoxCash.Location = new System.Drawing.Point(836, 1);
		this.VouchersInfoBoxCash.Name = "VouchersInfoBoxCash";
		this.VouchersInfoBoxCash.Size = new System.Drawing.Size(212, 71);
		this.VouchersInfoBoxCash.TabIndex = 1;
		this.VouchersInfoBoxCash.TabStop = false;
		this.VouchersInfoBoxCash.Visible = false;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(4, 11);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(96, 17);
		this.label42.TabIndex = 1;
		this.label42.Text = "Total Vouchers";
		this.lblUnApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUnApprovedVouchers.Location = new System.Drawing.Point(156, 47);
		this.lblUnApprovedVouchers.Name = "lblUnApprovedVouchers";
		this.lblUnApprovedVouchers.Size = new System.Drawing.Size(52, 17);
		this.lblUnApprovedVouchers.TabIndex = 6;
		this.lblUnApprovedVouchers.Text = "0";
		this.lblUnApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalVouchers.Location = new System.Drawing.Point(156, 11);
		this.lblTotalVouchers.Name = "lblTotalVouchers";
		this.lblTotalVouchers.Size = new System.Drawing.Size(52, 17);
		this.lblTotalVouchers.TabIndex = 4;
		this.lblTotalVouchers.Text = "0";
		this.lblTotalVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(4, 47);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(144, 17);
		this.label45.TabIndex = 3;
		this.label45.Text = "UnApproved Vouchers";
		this.lblApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedVouchers.Location = new System.Drawing.Point(156, 28);
		this.lblApprovedVouchers.Name = "lblApprovedVouchers";
		this.lblApprovedVouchers.Size = new System.Drawing.Size(52, 17);
		this.lblApprovedVouchers.TabIndex = 5;
		this.lblApprovedVouchers.Text = "0";
		this.lblApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(4, 28);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(127, 17);
		this.label47.TabIndex = 2;
		this.label47.Text = "Approved Vouchers";
		this.groupBox1.Controls.Add(this.txtToDocNoCpv);
		this.groupBox1.Controls.Add(this.txtFromDocNoCpv);
		this.groupBox1.Controls.Add(this.rdapproveddate);
		this.groupBox1.Controls.Add(this.rdmodifydate);
		this.groupBox1.Controls.Add(this.drdocdate);
		this.groupBox1.Controls.Add(this.rdentrydate);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv);
		this.groupBox1.Controls.Add(this.btnShowCpv);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv);
		this.groupBox1.Controls.Add(this.ToDateCpvHistory);
		this.groupBox1.Controls.Add(this.FromDateCpvHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(1, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(836, 71);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.txtToDocNoCpv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoCpv.Location = new System.Drawing.Point(266, 42);
		this.txtToDocNoCpv.Name = "txtToDocNoCpv";
		this.txtToDocNoCpv.ShortcutsEnabled = false;
		this.txtToDocNoCpv.Size = new System.Drawing.Size(77, 23);
		this.txtToDocNoCpv.TabIndex = 3;
		this.txtToDocNoCpv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoCpv_KeyPress);
		this.txtFromDocNoCpv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoCpv.Location = new System.Drawing.Point(266, 14);
		this.txtFromDocNoCpv.Name = "txtFromDocNoCpv";
		this.txtFromDocNoCpv.ShortcutsEnabled = false;
		this.txtFromDocNoCpv.Size = new System.Drawing.Size(77, 23);
		this.txtFromDocNoCpv.TabIndex = 2;
		this.txtFromDocNoCpv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoCpv_KeyPress);
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(727, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(727, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(647, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(647, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(348, 18);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(85, 15);
		this.label26.TabIndex = 147;
		this.label26.Text = "Buyer Account";
		this.cmbAccountTitleCpv.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitleCpv.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitleCpv.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitleCpv.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitleCpv.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).Location = new System.Drawing.Point(445, 12);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).Name = "cmbAccountTitleCpv";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).TabIndex = 4;
		this.btnShowCpv.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowCpv.FlatAppearance.BorderSize = 0;
		this.btnShowCpv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowCpv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowCpv.ForeColor = System.Drawing.Color.White;
		this.btnShowCpv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowCpv.Location = new System.Drawing.Point(578, 40);
		this.btnShowCpv.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowCpv.Name = "btnShowCpv";
		this.btnShowCpv.Size = new System.Drawing.Size(60, 26);
		this.btnShowCpv.TabIndex = 6;
		this.btnShowCpv.Text = "Show";
		this.btnShowCpv.UseVisualStyleBackColor = false;
		this.btnShowCpv.Click += new System.EventHandler(btnShowCpv_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(348, 46);
		this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(95, 15);
		this.label28.TabIndex = 142;
		this.label28.Text = "Approved Status";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(207, 46);
		this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(45, 15);
		this.label29.TabIndex = 141;
		this.label29.Text = "To Doc";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(207, 18);
		this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(60, 15);
		this.label30.TabIndex = 140;
		this.label30.Text = "From Doc";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(5, 46);
		this.label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(48, 15);
		this.label31.TabIndex = 139;
		this.label31.Text = "To Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(5, 18);
		this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(63, 15);
		this.label32.TabIndex = 13;
		this.label32.Text = "From Date";
		this.cmbApprovedStatusCpv.AllowNull = (DefaultableBoolean)1;
		this.cmbApprovedStatusCpv.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApprovedStatusCpv.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApprovedStatusCpv.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApprovedStatusCpv.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).Location = new System.Drawing.Point(445, 40);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).Name = "cmbApprovedStatusCpv";
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).TabIndex = 5;
		this.ToDateCpvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateCpvHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateCpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateCpvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateCpvHistory.Location = new System.Drawing.Point(69, 42);
		this.ToDateCpvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateCpvHistory.Name = "ToDateCpvHistory";
		this.ToDateCpvHistory.ShowCheckBox = true;
		this.ToDateCpvHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateCpvHistory.TabIndex = 1;
		this.FromDateCpvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateCpvHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateCpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateCpvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateCpvHistory.Location = new System.Drawing.Point(69, 14);
		this.FromDateCpvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateCpvHistory.Name = "FromDateCpvHistory";
		this.FromDateCpvHistory.ShowCheckBox = true;
		this.FromDateCpvHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDateCpvHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label25);
		this.panel8.Controls.Add(this.toolStrip4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1436, 52);
		this.panel8.TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(5, 30);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(261, 19);
		this.label25.TabIndex = 53;
		this.label25.Text = "Export Invoice Voucher History";
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewCpvHistory, this.btnRefreshCpvHistory });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1436, 27);
		this.toolStrip4.TabIndex = 52;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNewCpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewCpvHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewCpvHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewCpvHistory.Name = "btnNewCpvHistory";
		this.btnNewCpvHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewCpvHistory.Text = "&Reset";
		this.btnNewCpvHistory.Click += new System.EventHandler(btnNewCpvHistory_Click);
		this.btnRefreshCpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshCpvHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshCpvHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshCpvHistory.Name = "btnRefreshCpvHistory";
		this.btnRefreshCpvHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshCpvHistory.Text = "Refresh";
		this.btnRefreshCpvHistory.Click += new System.EventHandler(btnRefreshCpvHistory_Click);
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(245, 0);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(75, 22);
		this.btnRecordsUpdate.TabIndex = 515;
		this.btnRecordsUpdate.Text = "Records Update";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1448, 656);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmExportInvoiceVoucher";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmExportInvoiceVoucher_FormClosing);
		base.Load += new System.EventHandler(AcfrmPaymentVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExpenseVoucher_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.Form.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPending).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabPaymentDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.tabChargesDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.tabCommission.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCommissionInfo).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionExpenseAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefAccountId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyCode).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.History.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.VouchersInfoBoxCash.ResumeLayout(false);
		this.VouchersInfoBoxCash.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitleCpv).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusCpv).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
