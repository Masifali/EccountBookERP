using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class FreightVoucherPaymentBreakUp : Form
{
	private bool ChequeBookEnableStatus;

	private double TotalPaidAmount;

	private int _VoucherId;

	private int UpdateDetailIndex = -1;

	private DataTable dtPaymentBreakUp = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	internal GridEX grd;

	private Panel panel30;

	private TextBox txtRemarks;

	private Label label121;

	private TextBox txtAmount;

	private Label label118;

	internal UltraCombo CmbTransactionType;

	private Label label105;

	internal UltraCombo CmbAccountTitle;

	private Label label2;

	internal UltraCombo CmbInstrumentType;

	private Label label1;

	private TextBox txtPayeeTitle;

	private Label label4;

	private TextBox txtChequeNo;

	internal UltraCombo CmbChequeNo;

	private Label label3;

	private GroupBox groupBox1;

	private Label label5;

	private DateTimePicker datChequeDate;

	private Button btnAdd;

	private Button btnUpdate;

	private Button btnCancel;

	private Label BalanceValue;

	private ToolStripButton btnRefresh;

	public FreightVoucherPaymentBreakUp(UserAccount PUser, DataTable _dt, double totalPaidAmount, int VoucherId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = PUser;
		dtPaymentBreakUp = _dt;
		TotalPaidAmount = totalPaidAmount;
		_VoucherId = VoucherId;
	}

	private void GdBreakUpManual_Load(object sender, EventArgs e)
	{
		try
		{
			grd.DataSource = dtPaymentBreakUp;
			grd.RetrieveStructure();
			grdSetting();
			ChequeBookEnableStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheqBook Enabled"));
			if (ChequeBookEnableStatus)
			{
				((Control)(object)CmbChequeNo).Visible = true;
			}
			else
			{
				((Control)(object)CmbChequeNo).Visible = false;
			}
			TransactionTypeBind();
			GetInstrumentTypes();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "TransactionTypeId", "InstrumentTypeId", "AccountTitleId", "ChequeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns);
			grd.RootTable.Columns["TransactionType"].Width = Constants.InventoryConstants.DiscountType;
			grd.RootTable.Columns["InstrumentType"].Width = Constants.InventoryConstants.DiscountType;
			grd.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 50;
			grd.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.CellNo;
			grd.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["PayeeTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
			grd.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TransactionTypeBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dt.Rows.Add(1, "Cash");
			dt.Rows.Add(2, "Bank");
			dt.Rows.Add(3, "Other");
			InfragisticsHelper.BindAndRetainSelection(CmbTransactionType, dt, "Id", "Name", "Transaction Type", AllColumns: true, Conversion.ToInt(CmbTransactionType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			((UltraGridBase)CmbTransactionType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetInstrumentTypes()
	{
		try
		{
			DataTable dt = VoucherHead.GetInstrumentTypes();
			if (dt.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbInstrumentType, dt, "Id", "InstrumentType", "Instrument Type", AllColumns: true, Conversion.ToInt(CmbInstrumentType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbInstrumentType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			dtPaymentBreakUp.Rows.Clear();
			grd.DataSource = dtPaymentBreakUp;
			grd.RetrieveStructure();
			grdSetting();
			((Control)(object)CmbTransactionType).Text = string.Empty;
			((Control)(object)CmbInstrumentType).Text = string.Empty;
			((Control)(object)CmbAccountTitle).Text = string.Empty;
			((UltraGridBase)CmbAccountTitle).DataSource = null;
			BalanceValue.Text = "0";
			((Control)(object)CmbChequeNo).Text = string.Empty;
			((UltraGridBase)CmbChequeNo).DataSource = null;
			txtChequeNo.Text = string.Empty;
			datChequeDate.Value = DateTime.Now;
			txtAmount.Clear();
			txtPayeeTitle.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			CmbTransactionType.Focus();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private bool FormValidation()
	{
		if (((Control)(object)CmbTransactionType).Text == null || ((Control)(object)CmbTransactionType).Text == string.Empty)
		{
			MessageBox.Show("Transaction Type field is required");
			CmbTransactionType.Focus();
			return false;
		}
		if (((Control)(object)CmbTransactionType).Text == "Bank" && Conversion.ToInt(CmbInstrumentType.Value) == 0)
		{
			MessageBox.Show("Instrument Type field is required");
			CmbInstrumentType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAccountTitle).ActiveRow == null || Conversion.ToInt(CmbAccountTitle.Value) == 0)
		{
			MessageBox.Show("Account Title field is required");
			CmbAccountTitle.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbTransactionType.Value) == 2 && Conversion.ToInt(CmbInstrumentType.Value) == 1)
		{
			if (ChequeBookEnableStatus)
			{
				if (((UltraGridBase)CmbChequeNo).ActiveRow == null)
				{
					MessageBox.Show("Cheque No Field is Required");
					CmbChequeNo.Focus();
					return false;
				}
			}
			else if (txtChequeNo.Text == null || txtChequeNo.Text == string.Empty)
			{
				MessageBox.Show("Cheque No field is required");
				txtChequeNo.Focus();
				return false;
			}
		}
		if (Conversion.ToDouble((object)txtAmount.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Net Amount field is Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void grdGdBreakUp_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				UpdateDetailIndex = Conversion.ToInt((object)item.RowIndex);
				CmbTransactionType.Value = Conversion.ToInt(item.Cells["TransactionTypeId"].Value);
				CmbTransactionType_Leave(null, null);
				if (Conversion.ToInt(item.Cells["InstrumentTypeId"].Value) > 0)
				{
					CmbInstrumentType.Value = Conversion.ToInt(item.Cells["InstrumentTypeId"].Value);
				}
				CmbAccountTitle.Value = Conversion.ToInt(item.Cells["AccountTitleId"].Value);
				CmbAccountTitle_Leave(null, null);
				if (ChequeBookEnableStatus)
				{
					CmbChequeNo.Value = Conversion.ToInt(item.Cells["ChequeId"].Value);
				}
				else
				{
					txtChequeNo.Text = Conversion.ToString((object)item.Cells["ChequeNo"].Text);
				}
				datChequeDate.Value = Conversion.ToDateTime(item.Cells["ChequeDate"].Value);
				txtAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString("#,##0.###");
				txtPayeeTitle.Text = Conversion.ToString((object)item.Cells["PayeeTitle"].Text);
				txtRemarks.Text = Conversion.ToString((object)item.Cells["Remarks"].Text);
				CmbTransactionType.Focus();
				btnAdd.Visible = false;
				btnUpdate.Visible = true;
				btnCancel.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void GdBreakUpManual_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 83;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				Reset();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTransactionType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbTransactionType.Value) == 2 && ((DataTable)((UltraGridBase)CmbInstrumentType).DataSource).Rows.Count > 0)
			{
				((UltraGridBase)CmbInstrumentType).Rows[0].Activate();
			}
			BindAccounts();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindAccounts()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTransactionType.Value);
			if (Id == 0)
			{
				((Control)(object)CmbAccountTitle).Text = string.Empty;
				((UltraGridBase)CmbAccountTitle).DataSource = null;
				((Control)(object)CmbChequeNo).Text = string.Empty;
				((UltraGridBase)CmbChequeNo).DataSource = null;
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("AccountCode", typeof(string));
			dt.Columns.Add("AccountClass", typeof(string));
			dt.Columns.Add("AccountType", typeof(string));
			dt.Columns.Add("ParentAccountTitle", typeof(string));
			List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
			switch (Id)
			{
			case 1:
				allAccounts = allAccounts.Where((getGlobalAllAccountsWithCustomGroup r) => r.AccountTypeId == 2).ToList();
				break;
			case 2:
				allAccounts = allAccounts.Where((getGlobalAllAccountsWithCustomGroup r) => r.AccountTypeId == 15).ToList();
				break;
			case 3:
				allAccounts = allAccounts.Where((getGlobalAllAccountsWithCustomGroup r) => r.AccountTypeId == 3 || r.AccountTypeId == 6 || r.AccountTypeId == 8).ToList();
				break;
			}
			HashSet<int> seenIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in allAccounts)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (seenIds.Add(id))
				{
					dt.Rows.Add(id, acc.AccountTitle, acc.AccountCode, acc.AccountClassName, acc.AccountType, acc.ParentAccountTitle);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbAccountTitle, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbAccountTitle.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbAccountTitle_Leave(object sender, EventArgs e)
	{
		try
		{
			AccountCurrentBalance();
			CheqNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountCurrentBalance()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DateTime.Now,
				RefAccountId = Conversion.ToInt(CmbAccountTitle.Value)
			});
			if (detail.Rows.Count > 0)
			{
				double ClosingBalance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
				BalanceValue.Text = ClosingBalance.ToString("#,#;(#,#);0") + ((ClosingBalance > 0.0) ? " Dr" : ((ClosingBalance < 0.0) ? " Cr" : ((ClosingBalance == 0.0) ? " Nill" : "")));
			}
			else
			{
				BalanceValue.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheqNoFill()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbTransactionType.Value) == 2 && ChequeBookEnableStatus)
			{
				((Control)(object)CmbChequeNo).Text = string.Empty;
				DataTable dtCheq = CheqBookHeader.OutstandingCheqNo(new CheqBookHeader
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BankId = Conversion.ToInt(CmbAccountTitle.Value),
					Id = _VoucherId
				});
				if (dtCheq.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("CheqNo");
					dt.Clear();
					for (int i = 0; i < dtCheq.Rows.Count; i++)
					{
						dt.Rows.Add(dtCheq.Rows[i]["Id"], dtCheq.Rows[i]["CheqNo"]);
					}
					if (dt.Rows.Count > 0)
					{
						InfragisticsHelper.BindAndRetainSelection(CmbChequeNo, dtCheq, "Id", "CheqNo", "Cheque No", AllColumns: true, Conversion.ToInt(CmbChequeNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
					}
				}
				else
				{
					((Control)(object)CmbChequeNo).Text = string.Empty;
					((UltraGridBase)CmbChequeNo).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbChequeNo).Text = string.Empty;
				((UltraGridBase)CmbChequeNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidation())
			{
				double Amount = 0.0;
				if (dtPaymentBreakUp.Rows.Count > 0)
				{
					Amount = GridEX_Helper.GetColumnSum(grd, "Amount");
				}
				if (Amount + Conversion.ToDouble((object)txtAmount.Text) > TotalPaidAmount)
				{
					throw new Exception("Amount is greater than total paid amount");
				}
				dtPaymentBreakUp.Rows.Add(0, CmbTransactionType.Value, ((Control)(object)CmbTransactionType).Text.Trim(), CmbInstrumentType.Value, ((Control)(object)CmbInstrumentType).Text, CmbAccountTitle.Value, ((Control)(object)CmbAccountTitle).Text, ChequeBookEnableStatus ? CmbChequeNo.Value : ((object)0), ChequeBookEnableStatus ? ((Control)(object)CmbChequeNo).Text : txtChequeNo.Text, datChequeDate.Value, Conversion.ToDouble((object)txtAmount.Text), txtPayeeTitle.Text, txtRemarks.Text);
				grd.DataSource = dtPaymentBreakUp;
				grd.RetrieveStructure();
				grdSetting();
				ResetDetails();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetails()
	{
		try
		{
			((Control)(object)CmbInstrumentType).Text = string.Empty;
			((Control)(object)CmbAccountTitle).Text = string.Empty;
			((UltraGridBase)CmbAccountTitle).DataSource = null;
			BalanceValue.Text = "0";
			((Control)(object)CmbChequeNo).Text = string.Empty;
			txtChequeNo.Text = string.Empty;
			datChequeDate.Value = DateTime.Now;
			txtAmount.Clear();
			txtPayeeTitle.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			CmbTransactionType.Focus();
			btnAdd.Visible = true;
			btnUpdate.Visible = false;
			btnCancel.Visible = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidation())
			{
				double Amount = 0.0;
				if (dtPaymentBreakUp.Rows.Count > 0)
				{
					double rowAmount = Conversion.ToDouble(dtPaymentBreakUp.Rows[UpdateDetailIndex]["Amount"]);
					Amount = GridEX_Helper.GetColumnSum(grd, "Amount");
					Amount -= rowAmount;
				}
				if (Amount + Conversion.ToDouble((object)txtAmount.Text) > TotalPaidAmount)
				{
					throw new Exception("Amount is greater than total paid amount");
				}
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["TransactionTypeId"] = Conversion.ToInt(CmbTransactionType.Value);
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["TransactionType"] = ((Control)(object)CmbTransactionType).Text;
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["InstrumentTypeId"] = Conversion.ToInt(CmbInstrumentType.Value);
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["InstrumentType"] = ((Control)(object)CmbInstrumentType).Text;
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["AccountTitleId"] = Conversion.ToInt(CmbAccountTitle.Value);
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["AccountTitle"] = ((Control)(object)CmbAccountTitle).Text;
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["ChequeId"] = (ChequeBookEnableStatus ? Conversion.ToInt(CmbChequeNo.Value) : 0);
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["ChequeNo"] = (ChequeBookEnableStatus ? ((Control)(object)CmbChequeNo).Text : txtChequeNo.Text);
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["ChequeDate"] = Conversion.ToDateTime((object)datChequeDate.Value);
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmount.Text.Trim());
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["PayeeTitle"] = Conversion.ToString((object)txtPayeeTitle.Text.Trim());
				dtPaymentBreakUp.Rows[UpdateDetailIndex]["Remarks"] = Conversion.ToString((object)txtRemarks.Text.Trim());
				grd.DataSource = dtPaymentBreakUp;
				ResetDetails();
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
			ResetDetails();
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
			if (r != null && (int)r.RowType == 0)
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
			}
			grd.UpdateData();
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
			GetInstrumentTypes();
			BindAccounts();
			CheqNoFill();
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
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		this.grd = new GridEX();
		this.panel30 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.BalanceValue = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnUpdate = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label121 = new System.Windows.Forms.Label();
		this.txtPayeeTitle = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbAccountTitle = new UltraCombo();
		this.datChequeDate = new System.Windows.Forms.DateTimePicker();
		this.label105 = new System.Windows.Forms.Label();
		this.CmbTransactionType = new UltraCombo();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label118 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbInstrumentType = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtChequeNo = new System.Windows.Forms.TextBox();
		this.CmbChequeNo = new UltraCombo();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel30.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInstrumentType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		base.SuspendLayout();
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 167);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(926, 240);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 605;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grdGdBreakUp_DoubleClick);
		this.panel30.Controls.Add(this.groupBox1);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 55);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(926, 112);
		this.panel30.TabIndex = 606;
		this.groupBox1.Controls.Add(this.BalanceValue);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.btnUpdate);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label121);
		this.groupBox1.Controls.Add(this.txtPayeeTitle);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.groupBox1.Controls.Add(this.datChequeDate);
		this.groupBox1.Controls.Add(this.label105);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransactionType);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label118);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInstrumentType);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeNo);
		this.groupBox1.Controls.Add(this.txtChequeNo);
		this.groupBox1.Location = new System.Drawing.Point(5, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(766, 108);
		this.groupBox1.TabIndex = 624;
		this.groupBox1.TabStop = false;
		this.BalanceValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BalanceValue.Location = new System.Drawing.Point(350, 9);
		this.BalanceValue.Name = "BalanceValue";
		this.BalanceValue.Size = new System.Drawing.Size(151, 18);
		this.BalanceValue.TabIndex = 630;
		this.BalanceValue.Text = "0";
		this.BalanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatAppearance.BorderSize = 0;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(629, 78);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(31, 23);
		this.btnAdd.TabIndex = 8;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnUpdate.FlatAppearance.BorderSize = 0;
		this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.ForeColor = System.Drawing.Color.White;
		this.btnUpdate.Location = new System.Drawing.Point(629, 78);
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(65, 23);
		this.btnUpdate.TabIndex = 9;
		this.btnUpdate.Text = "Update";
		this.btnUpdate.UseVisualStyleBackColor = false;
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderSize = 0;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(696, 78);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(65, 23);
		this.btnCancel.TabIndex = 10;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(629, 11);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 15);
		this.label5.TabIndex = 626;
		this.label5.Text = "Cheque Date";
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(355, 78);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(273, 23);
		this.txtRemarks.TabIndex = 7;
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label121.Location = new System.Drawing.Point(355, 60);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(52, 15);
		this.label121.TabIndex = 614;
		this.label121.Text = "Remarks";
		this.txtPayeeTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPayeeTitle.Location = new System.Drawing.Point(127, 78);
		this.txtPayeeTitle.Name = "txtPayeeTitle";
		this.txtPayeeTitle.Size = new System.Drawing.Size(227, 23);
		this.txtPayeeTitle.TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(127, 58);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(61, 15);
		this.label4.TabIndex = 623;
		this.label4.Text = "PayeeTitle";
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(252, 29);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(251, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Leave += new System.EventHandler(CmbAccountTitle_Leave);
		this.datChequeDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datChequeDate.CustomFormat = "dd-MMM-yyyy";
		this.datChequeDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datChequeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datChequeDate.Location = new System.Drawing.Point(629, 31);
		this.datChequeDate.Name = "datChequeDate";
		this.datChequeDate.Size = new System.Drawing.Size(132, 23);
		this.datChequeDate.TabIndex = 4;
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label105.Location = new System.Drawing.Point(2, 11);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(96, 15);
		this.label105.TabIndex = 598;
		this.label105.Text = "Transaction Type";
		this.CmbTransactionType.AllowNull = (DefaultableBoolean)1;
		this.CmbTransactionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransactionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransactionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransactionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Location = new System.Drawing.Point(2, 29);
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Name = "CmbTransactionType";
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Leave += new System.EventHandler(CmbTransactionType_Leave);
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(2, 78);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(124, 23);
		this.txtAmount.TabIndex = 5;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetAmount_KeyPress);
		this.label118.AutoSize = true;
		this.label118.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label118.Location = new System.Drawing.Point(3, 59);
		this.label118.Name = "label118";
		this.label118.Size = new System.Drawing.Size(51, 15);
		this.label118.TabIndex = 608;
		this.label118.Text = "Amount";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(127, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 15);
		this.label1.TabIndex = 616;
		this.label1.Text = "Instrument Type";
		this.CmbInstrumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbInstrumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInstrumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInstrumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInstrumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbInstrumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInstrumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).Location = new System.Drawing.Point(127, 29);
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).Name = "CmbInstrumentType";
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(252, 11);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(97, 15);
		this.label2.TabIndex = 618;
		this.label2.Text = "Account Title(Cr)";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(505, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(66, 15);
		this.label3.TabIndex = 620;
		this.label3.Text = "Cheque No";
		this.txtChequeNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChequeNo.Location = new System.Drawing.Point(504, 31);
		this.txtChequeNo.Name = "txtChequeNo";
		this.txtChequeNo.Size = new System.Drawing.Size(124, 23);
		this.txtChequeNo.TabIndex = 3;
		this.CmbChequeNo.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Location = new System.Drawing.Point(504, 29);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Name = "CmbChequeNo";
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).TabIndex = 3;
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(926, 28);
		this.panel1.TabIndex = 6;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(222, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Freight Payment Breakup ";
		this.toolStrip4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnRefresh });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(926, 27);
		this.toolStrip4.TabIndex = 5;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(884, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar1.TabIndex = 66;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(926, 407);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel30);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip4);
		base.KeyPreview = true;
		base.Name = "FreightVoucherPaymentBreakUp";
		this.Text = "Freight Payment Breakup";
		base.Load += new System.EventHandler(GdBreakUpManual_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GdBreakUpManual_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel30.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInstrumentType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
