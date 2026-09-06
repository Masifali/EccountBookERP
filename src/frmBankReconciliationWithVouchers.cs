// // using Architecture.Reports;
// using Architecture.WinApp.Helper;
// using Attachment = global::Architecture.Model.Attachment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
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
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using GlobalVariables_Helper = global::Architecture.WinApp.Helper.GlobalVariables_Helper;
using DatatableHelper = global::Architecture.WinApp.Helper.DatatableHelper;
using FormHelper = global::Architecture.WinApp.Helper.FormHelper;
using BankReconciliationBLL = global::Architecture.WinApp.Common.BankReconciliationBLL;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Definition.AdjustmentVouchers
{

public class frmBankReconciliationWithVouchers : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private List<BankReconciliation> lstRemoveDetailRecord = new List<BankReconciliation>();

	private DataTable dtGrid = new DataTable();

	private DataTable dtPendingVouchers = new DataTable();

	private DataTable dtGridHistory = new DataTable();

	private DataTable dtVoucherNo = new DataTable();

	private int RecId;

	private int DocumentTypeId = 920;

	private bool Approved;

	private bool FormInitialized;

	private string ScreenName = "frmBankReconciliationWithVouchers";

	private int DefaultDaysToLessFromHistoryFromDate;

	private double BankReconcilationAdjustmentMaxAmount;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnSlip;

	internal UltraCombo CmbSupplier;

	private Label label7;

	private ToolStripButton btnDelete;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdHistory;

	private CheckBox ChkPrintslip;

	private Panel panel11;

	private GroupBox groupBox2;

	private Label label17;

	private Panel panel16;

	private Label label23;

	private Label label54;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnshortcutkeys;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel6;

	private Label label58;

	private Panel panel5;

	private Panel panel7;

	private Label label78;

	private GroupBox groupBox4;

	private Button btnshow;

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label77;

	private TextBox txtFromNoHistory;

	private ToolStripButton BtnRefreshHistory;

	internal GridEX grdPendingVouchers;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label2;

	private Label label8;

	private Label label3;

	private DateTimePicker txtVoucherDate;

	internal UltraCombo CmbVoucher;

	private TextBox txtVoucherAmountCr;

	private Label label5;

	private Button btnShowRecords;

	private RadioButton rdapproveddate;

	private TextBox txtVoucherAmountDr;

	private Label label4;

	private Label label1;

	private Label label6;

	private ToolStripButton BtnUploadData;

	private CtrlGrdBar ctrlGrdBar3;

	public frmBankReconciliationWithVouchers(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1030, 700);
			dtVoucherNo.Columns.Add("Id", typeof(int));
			dtVoucherNo.Columns.Add("VoucherNo", typeof(int));
			dtVoucherNo.Columns.Add("DetailId", typeof(int));
			if (dtGrid.Columns.Count == 0)
			{
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtGrid.Columns.Add("BankAccountId", typeof(int));
				dtGrid.Columns.Add("AccountTitle", typeof(string));
				dtGrid.Columns.Add("AccountCode", typeof(string));
				dtGrid.Columns.Add("TransactionDate", typeof(DateTime));
				dtGrid.Columns.Add("ChequeNo", typeof(string));
				dtGrid.Columns.Add("Particulars", typeof(string));
				dtGrid.Columns.Add("Debit", typeof(decimal));
				dtGrid.Columns.Add("Credit", typeof(decimal));
				dtGrid.Columns.Add("RunningBalance", typeof(decimal));
				dtGrid.Columns.Add("Remarks", typeof(string));
			}
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
			ScreenName = Conversion.ToString((object)base.Name);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			return new
			{
				HcomboData = BankReconciliationBLL.GetDataForDropDownFromBankReconciliation(UserAccount.OrganizationId, UserAccount.CompanyId, 0, "BankAccount")
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				GetConfigrationsFromGlobal();
				var result = t.Result;
				btnSave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnSlip.Enabled = formright.DoHavePrintRights;
				ChkPrintslip.Checked = formright.DoHavePrintRights;
				_ = RecId;
				BankGlAccountBindFromGlobal();
				ParameterFill();
				HistoryComboBind(result.HcomboData);
				txtFromdateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				txtToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				btnSave.Visible = true;
				btnUpdate.Visible = false;
				btnDelete.Visible = false;
				CmbSupplier.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigrationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			BankReconcilationAdjustmentMaxAmount = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BankReconcilationAdjustmentMaxAmount"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankGlAccountBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 15 });
			InfragisticsHelper.BindAndRetainSelection(CmbSupplier, dt, "Id", "AccountTitle", "AccountTitle", AllColumns: true, Conversion.ToInt(CmbSupplier.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowRecords_Click(object sender, EventArgs e)
	{
		try
		{
			if (!btnSave.Visible || !btnSave.Enabled)
			{
				throw new Exception("Please Reset the Form First...");
			}
			int Id = Conversion.ToInt(CmbSupplier.Value);
			if (Id <= 0)
			{
				MessageBox.Show("Please Select a Bank Account to Show Records");
				CmbSupplier.Focus();
			}
			else if ((dtGrid.Rows.Count <= 0 && dtPendingVouchers.Rows.Count <= 0) || FormHelper.ConfirmAction("Record Already Exists,Do you want to Refresh Previous And Load Again"))
			{
				Reset();
				FillGrdPendingVouchers(Id);
				FillGrdReconcilation(Id);
				BindGrids();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids()
	{
		try
		{
			if (dtGrid != null && dtGrid.Columns.Count > 0)
			{
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSetting();
			}
			if (dtPendingVouchers != null && dtPendingVouchers.Columns.Count > 0)
			{
				grdPendingVouchers.DataSource = dtPendingVouchers;
				grdPendingVouchers.RetrieveStructure();
				grdPendingVouchersSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillGrdPendingVouchers(int AccountId)
	{
		try
		{
			DataTable accountLedgerForBankReconciliation = BankReconciliationBLL.GetAccountLedgerForBankReconciliation(UserAccount.OrganizationId, UserAccount.CompanyId, AccountId);
			dtPendingVouchers.Rows.Clear();
			grdPendingVouchers.DataSource = null;
			if (dtPendingVouchers.Columns.Count == 0)
			{
				dtPendingVouchers.Columns.Add("RecNo", typeof(int));
				dtPendingVouchers.Columns.Add("Id", typeof(int));
				dtPendingVouchers.Columns.Add("DetailId", typeof(int));
				dtPendingVouchers.Columns.Add("DocumentTypeId", typeof(int));
				dtPendingVouchers.Columns.Add("DocumentType", typeof(string));
				dtPendingVouchers.Columns.Add("VoucherDate", typeof(DateTime));
				dtPendingVouchers.Columns.Add("VoucherCode", typeof(int));
				dtPendingVouchers.Columns.Add("AccountId", typeof(int));
				dtPendingVouchers.Columns.Add("AccountCode", typeof(string));
				dtPendingVouchers.Columns.Add("AccountTitle", typeof(string));
				dtPendingVouchers.Columns.Add("ChequeNo", typeof(string));
				dtPendingVouchers.Columns.Add("ChequeDate", typeof(DateTime));
				dtPendingVouchers.Columns.Add("Remarks", typeof(string));
				dtPendingVouchers.Columns.Add("DebitAmount", typeof(decimal));
				dtPendingVouchers.Columns.Add("CreditAmount", typeof(decimal));
				dtPendingVouchers.Columns.Add("RunningBalance", typeof(decimal));
			}
			foreach (DataRow row in accountLedgerForBankReconciliation.Rows)
			{
				decimal CreditAmount = Math.Abs(Conversion.ToDecimal(row["CreditAmount"]));
				dtPendingVouchers.Rows.Add(row["RecNo"], row["Id"], row["DetailId"], row["DocumentTypeId"], row["DocumentTypeDescription"], row["VoucherDate"], row["VoucherCode"], row["AccountId"], row["AccountCode"], row["AccountTitle"], row["ChequeNo"], row["ChequeDate"], row["Remarks"], row["DebitAmount"], CreditAmount, row["RunningBalance"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while loading pending vouchers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingVouchersSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DetailId", "RecNo", "AccountId", "DocumentTypeId", "AccountTitle", "AccountCode", "RunningBalance" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPendingVouchers, 2, 0, hiddenColumns);
			grdPendingVouchers.RootTable.Columns["DocumentType"].Caption = "V.Type";
			grdPendingVouchers.RootTable.Columns["RecNo"].Width = 60;
			grdPendingVouchers.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType;
			grdPendingVouchers.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant;
			grdPendingVouchers.RootTable.Columns["VoucherCode"].Width = Constants.InventoryConstants.DocNoConstant;
			grdPendingVouchers.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdPendingVouchers.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
			grdPendingVouchers.RootTable.Columns["DebitAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingVouchers.RootTable.Columns["CreditAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingVouchers.RootTable.Columns["RunningBalance"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingVouchers.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.BLNumberConstant;
			grdPendingVouchers.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant;
			grdPendingVouchers.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 30;
			GridEX_Helper.AddButton(grdPendingVouchers, "Load", "Load", 50, 0);
			grdPendingVouchers.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingOrders_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPendingVouchers.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Load")
			{
				grdPendingOrders_LoadClick(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingOrders_LoadClick(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (!btnSave.Visible || !btnSave.Enabled)
				{
					throw new Exception("Please Reset the Form First to Load New Data");
				}
				if (Conversion.ToInt(r.Cells["DocumentTypeId"].Value) == 0)
				{
					throw new Exception("Please Select Valid row to Load Data");
				}
				if (Conversion.ToInt(CmbVoucher.Value) <= 0 || FormHelper.ConfirmAction("Voucher Is Already Selected,Do you Want To change??"))
				{
					double CreditAmount = Conversion.ToDouble(r.Cells["CreditAmount"].Value);
					double DebitAmount = Conversion.ToDouble(r.Cells["DebitAmount"].Value);
					dtVoucherNo.Clear();
					dtVoucherNo.Rows.Add(r.Cells["Id"].Value, r.Cells["VoucherCode"].Value, r.Cells["DetailId"].Value);
					InfragisticsHelper.BindAndRetainSelection(CmbVoucher, dtVoucherNo, "Id", "VoucherNo", "VoucherNo", AllColumns: true, Conversion.ToInt(CmbVoucher.Value), insertDefaultRow: false, null, null, ActivateRow: false);
					((UltraGridBase)CmbVoucher).Rows[0].Activate();
					txtVoucherDate.Value = Conversion.ToDateTime(r.Cells["VoucherDate"].Value);
					txtVoucherAmountCr.Text = Math.Abs(CreditAmount).ToString("#,##0.###");
					txtVoucherAmountDr.Text = Math.Abs(DebitAmount).ToString("#,##0.###");
					((Control)(object)grd).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillGrdReconcilation(int accountId)
	{
		try
		{
			DataTable dataTable = BankReconciliationBLL.BankReconciliation_GetDataForReconciliationWithvouchers(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, accountId);
			grd.DataSource = null;
			dtGrid.Clear();
			foreach (DataRow row in dataTable.Rows)
			{
				dtGrid.Rows.Add(row["Id"], row["DocumentTypeId"], row["BankAccountId"], row["AccountTitle"], row["AccountCode"], row["TransactionDate"], row["ChequeNo"], row["Particulars"], row["Debit"], row["Credit"], row["RunningBalance"], row["Remarks"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "BankAccountId", "AccountTitle", "AccountCode", "RunningBalance" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 0, hiddenColumns);
			grd.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
			grd.RootTable.Columns["TransactionDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grd.RootTable.Columns["ChequeNo"].Width = 120;
			grd.RootTable.Columns["Particulars"].Width = Constants.InventoryConstants.RemarksConstants * 2;
			grd.RootTable.Columns["Debit"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Credit"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["RunningBalance"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			ctrlGrdBar1_Load(null, null);
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
			grd.FrozenColumns = 2;
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
			if (item != null && (int)item.RowType == 0)
			{
				_ = item.Cells.Count;
				_ = 0;
			}
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
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0 && r.IsChecked)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				BankReconciliation detail = new BankReconciliation();
				detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				FillDetailListCommonForInsertAndDelete(detail, r);
				detail.ActionId = 3;
				lstRemoveDetailRecord.Add(detail);
				dtGrid.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				dtGrid.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			grd.UpdateData();
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
			Approved = false;
			lstRemoveDetailRecord.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			((Control)(object)CmbSupplier).Enabled = true;
			dtVoucherNo.Clear();
			((Control)(object)CmbVoucher).Text = string.Empty;
			((UltraGridBase)CmbVoucher).DataSource = null;
			txtVoucherAmountCr.Text = string.Empty;
			txtVoucherAmountDr.Text = string.Empty;
			dtGrid.Rows.Clear();
			dtPendingVouchers.Rows.Clear();
			BindGrids();
			CmbSupplier.Focus();
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
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigrationsFromGlobal();
			BankGlAccountBindFromGlobal();
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

	private void BtnSave_Click(object sender, EventArgs e)
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
			if (RecId == 0)
			{
				throw new Exception("Record Not found");
			}
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please check Any Record");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbSupplier, "Bank Account", FormHelper.ValidType.INT),
				((Control)(object)CmbVoucher, "Voucher No", FormHelper.ValidType.INT)
			}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			double voucherAmountDr = Conversion.ToDouble((object)txtVoucherAmountDr.Text);
			double voucherAmountCr = Conversion.ToDouble((object)txtVoucherAmountCr.Text);
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			double sumCredit = checkedRows.Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["Credit"].Value));
			double sumBalance = checkedRows.Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["Debit"].Value)) - sumCredit;
			double differenceAmount = 0.0;
			if (voucherAmountDr > 0.0)
			{
				if (!(sumBalance < 0.0))
				{
					throw new Exception($"Reconciliation failed: Debit amount is {voucherAmountDr}. Please select credit-side entries totaling this amount. Currently selected amount is {sumBalance}.");
				}
				differenceAmount = voucherAmountDr - Math.Abs(sumBalance);
				if (Math.Abs(differenceAmount) > BankReconcilationAdjustmentMaxAmount)
				{
					throw new Exception($"Difference Amount {Math.Abs(differenceAmount)} exceeds the maximum allowed of {BankReconcilationAdjustmentMaxAmount}.");
				}
			}
			else if (voucherAmountCr > 0.0)
			{
				if (!(sumBalance > 0.0))
				{
					throw new Exception($"Reconciliation failed: Credit amount is {voucherAmountCr}. Please select debit-side entries totaling this amount. Currently selected amount is {sumBalance}.");
				}
				differenceAmount = voucherAmountCr - Math.Abs(sumBalance);
				if (Math.Abs(differenceAmount) > BankReconcilationAdjustmentMaxAmount)
				{
					throw new Exception($"Difference Amount {Math.Abs(differenceAmount)} exceeds the maximum allowed of {BankReconcilationAdjustmentMaxAmount}.");
				}
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.EntryUser = UserAccount.ID;
			obj.AccountId = Conversion.ToInt(CmbSupplier.Value);
			obj.VoucherHeadId = Conversion.ToInt(CmbVoucher.Value);
			obj.VoucherDetailId = Conversion.ToInt(((UltraDropDownBase)CmbVoucher).SelectedRow.Cells["DetailId"].Value);
			obj.Amount = differenceAmount;
			obj.StatusId = 1;
			string Ids = string.Join(",", checkedRows.Select((GridEXRow r) => Conversion.ToInt(r.Cells["Id"].Value)));
			obj.Ids = Ids;
			BankReconciliationBLL.BankReconciliation_UpdateStatusWithVoucher(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Record Saved Successfully [" + Conversion.ToString((object)((Control)(object)CmbVoucher).Text) + "] ");
			}
			else
			{
				MessageBox.Show("Record Update Successfully [" + Conversion.ToString((object)((Control)(object)CmbVoucher).Text) + "] ");
			}
			Reset();
			if (ChkPrintslip.Checked)
			{
				GenerateSlip(obj.AccountId, obj.VoucherHeadId);
			}
			btnShowRecords_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int BankAccountId, int VoucherHeadId)
	{
		try
		{
			Reset();
			RecId = VoucherHeadId;
			DataTable invPI = BankReconciliationBLL.ReadByIdAccountIdAndVoucherHeadId(BankAccountId, VoucherHeadId);
			if (invPI == null || invPI.Rows.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			DataRow dr = invPI.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt((object)r.Field<int>("VoucherHeadId")) == VoucherHeadId);
			((Control)(object)CmbSupplier).Enabled = false;
			CmbSupplier.Value = Conversion.ToInt(dr["BankAccountId"]);
			Approved = Conversion.ToBool(dr["IsApproved"]);
			dtVoucherNo.Clear();
			dtVoucherNo.Rows.Add(dr["VoucherHeadId"], dr["VoucherCode"], dr["VoucherDetailId"]);
			InfragisticsHelper.BindAndRetainSelection(CmbVoucher, dtVoucherNo, "Id", "VoucherNo", "VoucherNo");
			((UltraGridBase)CmbVoucher).Rows[0].Activate();
			txtVoucherDate.Value = Conversion.ToDateTime(dr["VoucherDate"]);
			txtVoucherAmountCr.Text = Conversion.ToString(dr["VoucherAmountCr"]);
			txtVoucherAmountDr.Text = Conversion.ToString(dr["VoucherAmountDr"]);
			foreach (DataRow dataRow in invPI.Rows)
			{
				dtGrid.Rows.Add(dataRow["Id"], dataRow["DocumentTypeId"], dataRow["BankAccountId"], dataRow["AccountTitle"], dataRow["AccountCode"], dataRow["TransactionDate"], dataRow["ChequeNo"], dataRow["Particulars"], dataRow["Debit"], dataRow["Credit"], dataRow["RunningBalance"], dataRow["Remarks"]);
			}
			BindGrids();
			((Control)(object)grd).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Id not found for deletion...");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ReportsParameters val = new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = DocumentTypeId,
					EntryUser = UserAccount.ID,
					AccountId = Conversion.ToInt(CmbSupplier.Value),
					VoucherHeadId = Conversion.ToInt(CmbVoucher.Value),
					VoucherDetailId = Conversion.ToInt(((UltraDropDownBase)CmbVoucher).SelectedRow.Cells["DetailId"].Value),
					Amount = 0.0,
					StatusId = 2
				};
				string Ids = string.Join(",", from r in grd.GetDataRows()
					select Conversion.ToInt(r.Cells["Id"].Value));
				val.Ids = Ids;
				BankReconciliationBLL.BankReconciliation_UpdateStatusWithVoucher(val);
				MessageBox.Show("Deleted Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			HashSet<int> gridIds = (from row in grd.GetDataRows()
				where Conversion.ToInt(row.Cells["Id"].Value) > 0
				select Conversion.ToInt(row.Cells["Id"].Value)).ToHashSet();
			lstRemoveDetailRecord.RemoveAll((BankReconciliation item) => gridIds.Contains(item.Id));
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(BankReconciliation vd, GridEXRow r)
	{
		vd.OrganizationId = UserAccount.OrganizationId;
		vd.CompanyId = UserAccount.CompanyId;
		vd.BranchesId = UserAccount.BranchesId;
		vd.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
		vd.DocumentTypeId = DocumentTypeId;
		vd.EntryDate = DateTime.Now;
		vd.ModifyDate = DateTime.Now;
		vd.ApprovedDate = DateTime.Now;
		vd.VoucherDate = DateTime.Now;
		vd.EntryUserId = UserAccount.ID;
		vd.ModifyUserId = UserAccount.ID;
		vd.ApprovedUserId = UserAccount.ID;
		vd.VoucherHeadId = 0;
		vd.VoucherDetailId = 0;
		vd.VoucherCode = 0;
		vd.VoucherDate = Conversion.ToDateTime((object)txtVoucherDate.Value);
		vd.BankAccountId = Conversion.ToInt(CmbSupplier.Value);
		vd.BankAccountTitle = Conversion.ToString(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["AccountTitle"].Value);
		vd.BankAccountCode = Conversion.ToString(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["AccountCode"].Value);
		vd.TransactionDate = Conversion.ToDateTime(r.Cells["TransactionDate"].Value);
		vd.ChequeNo = Conversion.ToString(r.Cells["ChequeNo"].Value);
		vd.Particulars = Conversion.ToString(r.Cells["Particulars"].Value);
		vd.Debit = Conversion.ToDecimal(r.Cells["Debit"].Value);
		vd.Credit = Conversion.ToDecimal(r.Cells["Credit"].Value);
		vd.IsReconciled = false;
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				cmbDateTypeHistory.Focus();
			}
			else
			{
				((Control)(object)grd).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbDateTypeHistory, 0);
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
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtSupplier)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtSupplier, "Id", "name", "Bank Account", AllColumns: false, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Resethistory()
	{
		try
		{
			cmbDateTypeHistory.Value = 3;
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(BankReconciliationBLL.GetDataForDropDownFromBankReconciliation(UserAccount.OrganizationId, UserAccount.CompanyId, 0, "BankAccount"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			FillHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = DocumentTypeId;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.FromDate = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ToDate = txtToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.EntryDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ModifyDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ApprovedDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ApprovedDateTo = txtToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			PI.AccountId = Conversion.ToInt(CmbCustomerHistory.Value);
			PI.SkipZero = 1;
			DataTable invPI = BankReconciliationBLL.FormHistory(PI);
			if (invPI != null && invPI.Rows.Count > 0)
			{
				if (dtGridHistory.Columns.Count == 0)
				{
					dtGridHistory.Columns.Add("Id", typeof(int));
					dtGridHistory.Columns.Add("DocumentTypeId", typeof(int));
					dtGridHistory.Columns.Add("VoucherHeadId", typeof(int));
					dtGridHistory.Columns.Add("VoucherType", typeof(string));
					dtGridHistory.Columns.Add("VoucherNo", typeof(int));
					dtGridHistory.Columns.Add("VoucherDate", typeof(DateTime));
					dtGridHistory.Columns.Add("BankAccountId", typeof(int));
					dtGridHistory.Columns.Add("AccountTitle", typeof(string));
					dtGridHistory.Columns.Add("AccountCode", typeof(string));
					dtGridHistory.Columns.Add("TransactionDate", typeof(DateTime));
					dtGridHistory.Columns.Add("ChequeNo", typeof(string));
					dtGridHistory.Columns.Add("Particulars", typeof(string));
					dtGridHistory.Columns.Add("Debit", typeof(decimal));
					dtGridHistory.Columns.Add("Credit", typeof(decimal));
					dtGridHistory.Columns.Add("IsReconciled", typeof(bool));
					dtGridHistory.Columns.Add("Remarks", typeof(string));
					dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtGridHistory.Columns.Add("EntryUser", typeof(string));
					dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtGridHistory.Columns.Add("ModifyUser", typeof(string));
					dtGridHistory.Columns.Add("IsApproved", typeof(bool));
					dtGridHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtGridHistory.Columns.Add("ApprovedUser", typeof(string));
					dtGridHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtGridHistory.Clear();
				foreach (DataRow dataRow in invPI.Rows)
				{
					dtGridHistory.Rows.Add(dataRow["Id"], dataRow["DocumentTypeId"], dataRow["VoucherHeadId"], dataRow["VoucherDocumentType"], dataRow["VoucherNo"], dataRow["VoucherDate"], dataRow["BankAccountId"], dataRow["AccountTitle"], dataRow["AccountCode"], dataRow["TransactionDate"], dataRow["ChequeNo"], dataRow["Particulars"], dataRow["Debit"], dataRow["Credit"], dataRow["IsReconciled"], dataRow["Remarks"], dataRow["EntryDate"], dataRow["EntryUser"], dataRow["ModifyDate"], dataRow["ModifyUser"], dataRow["IsApproved"], dataRow["ApprovedDate"], dataRow["ApprovedUser"], dataRow["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtGridHistory;
				grdHistory.RetrieveStructure();
				grdHistorySetting();
			}
			else
			{
				grdHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistorySetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "VoucherHeadId", "BankAccountId", "NoOfAttachments" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns);
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["VoucherNo"].Width = Constants.InventoryConstants.VoucherNo;
			grdHistory.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdHistory.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
			grdHistory.RootTable.Columns["TransactionDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ChequeNo"].Width = 120;
			grdHistory.RootTable.Columns["Particulars"].Width = 180;
			grdHistory.RootTable.Columns["Debit"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["Credit"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["IsReconciled"].Width = 80;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["IsApproved"].Width = 80;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = 80;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 40, 1);
			grdHistory.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int VoucherHeadId = Conversion.ToInt((object)r.Cells["VoucherHeadId"].Value.ToString());
				int BankAccountId = Conversion.ToInt((object)r.Cells["BankAccountId"].Value.ToString());
				ReadById(BankAccountId, VoucherHeadId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int VoucherHeadId = Conversion.ToInt((object)r.Cells["VoucherHeadId"].Value.ToString());
				int BankAccountId = Conversion.ToInt((object)r.Cells["BankAccountId"].Value.ToString());
				if (e.Column.Key == "Edit")
				{
					ReadById(BankAccountId, VoucherHeadId);
				}
				if (e.Column.Key == "Print")
				{
					GenerateSlip(BankAccountId, VoucherHeadId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			int VoucherHeadId = Conversion.ToInt(CmbVoucher.Value);
			int BankAccountId = Conversion.ToInt(CmbSupplier.Value);
			GenerateSlip(BankAccountId, VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int BankAccountId, int VoucherHeadId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = BankReconciliationBLL.BankReconciliation_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				AccountId = BankAccountId,
				VoucherHeadId = VoucherHeadId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "920_BankReconciliation_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dtGridHistory);
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

	private void txtFromNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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
					((Control)(object)grd).Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					cmbDateTypeHistory.Focus();
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
					BtnSave_Click(null, null);
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
					CmbSupplier.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdPendingVouchers).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbSupplier.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnSlip_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlip_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistory).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+P", "For Print Slip 920");
			dt.Rows.Add("Alt+1", "For Print Slip 920");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				((Control)(object)grd).Focus();
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
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			int VoucherHeadId = Conversion.ToInt((object)r.Cells["VoucherHeadId"].Value.ToString());
			int BankAccountId = Conversion.ToInt((object)r.Cells["BankAccountId"].Value.ToString());
			Conversion.ToInt((object)r.Cells["PartyGlId"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					ReadById(BankAccountId, VoucherHeadId);
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					GenerateSlip(BankAccountId, VoucherHeadId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				ReadById(BankAccountId, VoucherHeadId);
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				GenerateSlip(BankAccountId, VoucherHeadId);
			}
			((Control)(object)grdHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUploadData_Click(object sender, EventArgs e)
	{
		try
		{
			new frmBankReconciliationUploadExcelSheet(UserAccount).ShowDialog();
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdPendingVouchers, dtPendingVouchers);
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
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Expected O, but got Unknown
		//IL_0c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Expected O, but got Unknown
		//IL_0c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Expected O, but got Unknown
		//IL_2726: Unknown result type (might be due to invalid IL or missing references)
		//IL_2730: Expected O, but got Unknown
		GridEXLayout grdPendingVouchers_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.AdjustmentVouchers.frmBankReconciliationWithVouchers));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdPendingVouchers = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnShowRecords = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtVoucherAmountDr = new System.Windows.Forms.TextBox();
		this.txtVoucherAmountCr = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtVoucherDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbVoucher = new UltraCombo();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.CmbSupplier = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.BtnUploadData = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label78 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label73 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.CmbCustomerHistory = new UltraCombo();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingVouchers).BeginInit();
		this.panel6.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVoucher).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
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
		this.tabControl1.Size = new System.Drawing.Size(1034, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1026, 582);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingVouchers);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1020, 576);
		this.panel1.TabIndex = 7;
		this.grdPendingVouchers.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingVouchers).BackColor = System.Drawing.Color.White;
		this.grdPendingVouchers.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPendingVouchers_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingVouchers.DesignTimeLayout = grdPendingVouchers_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingVouchers.DynamicFiltering = true;
		this.grdPendingVouchers.FilterMode = (FilterMode)2;
		this.grdPendingVouchers.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingVouchers.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Location = new System.Drawing.Point(0, 488);
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Name = "grdPendingVouchers";
		((ScrollableBase)this.grdPendingVouchers).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Size = new System.Drawing.Size(1020, 88);
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).TabIndex = 1;
		this.grdPendingVouchers.TotalRow = (InheritableBoolean)1;
		this.grdPendingVouchers.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingVouchers).VisualStyle = (VisualStyle)4;
		this.grdPendingVouchers.ColumnButtonClick += new ColumnActionEventHandler(grdPendingOrders_ColumnButtonClick);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Controls.Add(this.label58);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 464);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1020, 24);
		this.panel6.TabIndex = 16;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(3, 4);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(275, 16);
		this.label58.TabIndex = 126;
		this.label58.Text = "Pending Vouchers For Bank Reconciliation";
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Location = new System.Drawing.Point(0, 124);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1020, 340);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel16);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1012, 311);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1006, 284);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1006, 21);
		this.panel16.TabIndex = 33;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(966, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.btnShowRecords);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1020, 99);
		this.panel3.TabIndex = 0;
		this.btnShowRecords.BackColor = System.Drawing.Color.Teal;
		this.btnShowRecords.FlatAppearance.BorderSize = 0;
		this.btnShowRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowRecords.Font = new System.Drawing.Font("Verdana", 9.2f, System.Drawing.FontStyle.Bold);
		this.btnShowRecords.ForeColor = System.Drawing.Color.White;
		this.btnShowRecords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowRecords.Location = new System.Drawing.Point(294, 68);
		this.btnShowRecords.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowRecords.Name = "btnShowRecords";
		this.btnShowRecords.Size = new System.Drawing.Size(57, 25);
		this.btnShowRecords.TabIndex = 1;
		this.btnShowRecords.Text = "Show";
		this.btnShowRecords.UseVisualStyleBackColor = false;
		this.btnShowRecords.Click += new System.EventHandler(btnShowRecords_Click);
		this.groupBox2.Controls.Add(this.txtVoucherAmountDr);
		this.groupBox2.Controls.Add(this.txtVoucherAmountCr);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtVoucherDate);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVoucher);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(356, 30);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(465, 68);
		this.groupBox2.TabIndex = 2;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Voucher Information";
		this.txtVoucherAmountDr.Enabled = false;
		this.txtVoucherAmountDr.Location = new System.Drawing.Point(319, 42);
		this.txtVoucherAmountDr.Name = "txtVoucherAmountDr";
		this.txtVoucherAmountDr.Size = new System.Drawing.Size(139, 20);
		this.txtVoucherAmountDr.TabIndex = 3;
		this.txtVoucherAmountCr.Enabled = false;
		this.txtVoucherAmountCr.Location = new System.Drawing.Point(319, 16);
		this.txtVoucherAmountCr.Name = "txtVoucherAmountCr";
		this.txtVoucherAmountCr.Size = new System.Drawing.Size(139, 20);
		this.txtVoucherAmountCr.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(204, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(114, 15);
		this.label4.TabIndex = 66;
		this.label4.Text = "Voucher Amount Dr";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(204, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(112, 15);
		this.label5.TabIndex = 66;
		this.label5.Text = "Voucher Amount Cr";
		this.txtVoucherDate.CustomFormat = "dd-MMM-yy";
		this.txtVoucherDate.Enabled = false;
		this.txtVoucherDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtVoucherDate.Location = new System.Drawing.Point(84, 42);
		this.txtVoucherDate.Name = "txtVoucherDate";
		this.txtVoucherDate.Size = new System.Drawing.Size(116, 22);
		this.txtVoucherDate.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(6, 46);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(79, 15);
		this.label1.TabIndex = 65;
		this.label1.Text = "Voucher Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(6, 20);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(70, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Voucher No";
		this.CmbVoucher.AllowNull = (DefaultableBoolean)1;
		this.CmbVoucher.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVoucher.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVoucher.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVoucher).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbVoucher).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVoucher.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Location = new System.Drawing.Point(84, 14);
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Name = "CmbVoucher";
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.CmbVoucher).TabIndex = 0;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1020, 28);
		this.panel11.TabIndex = 110;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(292, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "Bank Reconciliation With Vouchers";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(930, 4);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(88, 20);
		this.ChkPrintslip.TabIndex = 1;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(92, 39);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(259, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(5, 45);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(81, 15);
		this.label6.TabIndex = 65;
		this.label6.Text = "Bank Account";
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.btnSlip,
			this.BtnUploadData, this.btnshortcutkeys, this.toolStripSeparator5
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1020, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
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
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(BtnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Visible = false;
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(88, 22);
		this.btnSlip.Text = "920-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.BtnUploadData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUploadData.Image = Architecture.WinApp.Properties.Resources.icons8_bill_32;
		this.BtnUploadData.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUploadData.Name = "BtnUploadData";
		this.BtnUploadData.Size = new System.Drawing.Size(202, 22);
		this.BtnUploadData.Text = "Upload Reconciliation Data";
		this.BtnUploadData.Click += new System.EventHandler(BtnUploadData_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1026, 582);
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
		this.grdHistory.GroupByBoxVisible = false;
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 155);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1020, 424);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label78);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 128);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1020, 27);
		this.panel7.TabIndex = 48;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(967, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(49, 25);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(5, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(142, 19);
		this.label78.TabIndex = 35;
		this.label78.Text = "Filtered Records";
		this.panel5.Controls.Add(this.groupBox4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 57);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1020, 71);
		this.panel5.TabIndex = 0;
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label8);
		this.groupBox4.Controls.Add(this.label2);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(1, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1016, 67);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(814, 44);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(138, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Voucher Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdapproveddate.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(937, 11);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(71, 50);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Voucher Approve Date";
		this.rdapproveddate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(814, 28);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(127, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Voucher Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(756, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(54, 25);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(555, 18);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(81, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Bank Account";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(814, 12);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(97, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Voucher Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(555, 35);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(4, 18);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(4, 35);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(262, 37);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(125, 22);
		this.txtToDateHistory.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(262, 18);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 15);
		this.label8.TabIndex = 171;
		this.label8.Text = "Date To";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(133, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 171;
		this.label2.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(471, 37);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(82, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(136, 37);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(125, 22);
		this.txtFromdateHistory.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(471, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 173;
		this.label3.Text = "Voucher To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(388, 18);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(82, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Voucher From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(388, 37);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(82, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1020, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(360, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Bank Reconciliation With Vouchers History ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1020, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1020, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(980, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.ctrlGrdBar3.MyGrid = this.grdPendingVouchers;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 611);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmBankReconciliationWithVouchers";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingVouchers).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVoucher).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
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
