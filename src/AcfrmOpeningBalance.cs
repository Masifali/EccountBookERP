using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
namespace Architecture.WinApp
{
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using Bank = global::Architecture.Model.Bank;
using City = global::Architecture.BLL.City;
using CheqBookHeader = global::Architecture.Model.Accounts.CheqBookHeader;
using CheqBookDetail = global::Architecture.Model.Accounts.CheqBookDetail;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;
using ChartOfAccountMultiLingo = global::Architecture.Model.Accounts.ChartOfAccountMultiLingo;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using SupplierCustomerShipToAddress = global::Architecture.Model.Inventory.SupplierCustomerShipToAddress;
using AccountsOpeningBalances = global::Architecture.Model.Accounts.AccountsOpeningBalances;
using ShortCutKeyPopUp = global::Architecture.WinApp.Common.ShortCutKeyPopUp;
using Constants = global::Architecture.WinApp.Common.Constants;
using GridEX_Helper = global::Architecture.WinApp.Common.GridEX_Helper;
using InfragisticsHelper = global::Architecture.WinApp.Common.InfragisticsHelper;
using HierarchicalMode = Janus.Windows.GridEX.HierarchicalMode;
using SelfReferencingAggregationMode = Janus.Windows.GridEX.SelfReferencingAggregationMode;

using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;

using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;

using UserAccount = global::Architecture.Model.UserAccount;
using FinancialYear = global::Architecture.Model.FinancialYear;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using global::Architecture.BLL.Accounts;
using global::Architecture.BLL.Reports.Accounts;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.Accounts;
using global::Architecture.Model.Main;
// // // using Architecture.Reports;
// // using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using grid.User_Conrtols;

public class AcfrmOpeningBalance : Form
{
	private int RId;

	private bool BranchFeature;

	private DataTable getlist = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	public List<AccountsOpeningBalances> accOpeningBal = new List<AccountsOpeningBalances>();

	private IContainer components;

	private Panel panel1;

	private Label label10;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNew;

	private ToolStripButton BtnEdit;

	private ToolStripButton BtnPrint;

	private GroupBox groupBox1;

	private TextBox txtCredit;

	private TextBox txtDebit;

	private Label label9;

	private Label label6;

	private Label label8;

	private Label label7;

	private Label label4;

	private Label label1;

	internal UltraCombo cmbAccountTitle;

	private TextBox txtAcId;

	private Panel panel2;

	private Panel panel3;

	internal GridEX dgvOpeningBalance;

	private CtrlGrdBar ctrlGrdBar1;

	private Button button1;

	private ToolStripButton toolStripButton1;

	private Label label2;

	private ToolStripButton btnShortCutKey;

	private Label lblBranch;

	internal UltraCombo cmbBranchName;

	public AcfrmOpeningBalance(UserAccount pUserAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		FindAccountTitle();
		BtnEdit.Text = "Update";
	}

	private void AcfrmOpeningBalance_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			BtnEdit.Enabled = formrights.DoHaveUpdateRights;
			button1.Enabled = formrights.DoHaveUpdateRights;
			BtnPrint.Enabled = formrights.DoHavePrintRights;
			FindAccountTitle();
			fillDataGrid();
			cmbAccountTitle.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (Conversion.ToInt(cmbAccountTitle.Value) == 0)
		{
			MessageBox.Show("Account Title Field is Required");
			cmbAccountTitle.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtDebit.Text.Trim()) > 0.0 && Conversion.ToDouble((object)txtCredit.Text.Trim()) > 0.0)
		{
			MessageBox.Show("Debit Amount and Credit Amount Both cannot be greater than zero");
			return false;
		}
		if (Conversion.ToDouble((object)txtDebit.Text.Trim()) == 0.0 && Conversion.ToDouble((object)txtCredit.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Credit Or Debit Field is Required");
			txtDebit.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			RId = 0;
			((Control)(object)cmbAccountTitle).Text = string.Empty;
			txtCredit.Clear();
			txtDebit.Clear();
			fillDataGrid();
			cmbAccountTitle.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void fillDataGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			AccountsOpeningBalances accounts = new AccountsOpeningBalances();
			accounts.OrganizationId = UserAccount.OrganizationId;
			accounts.CompanyId = UserAccount.CompanyId;
			accounts.EntryUser = UserAccount.ID;
			accounts.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			getlist = global::Architecture.BLL.Accounts.AccountsOpeningBalances.GetAll(accounts);
			if (getlist.Rows.Count > 0)
			{
				dgvOpeningBalance.DataSource = null;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ChartofAccountId", typeof(int));
				dt.Columns.Add("AccountType");
				dt.Columns.Add("ParentAccountTitle");
				dt.Columns.Add("AccountTitle");
				dt.Columns.Add("DebitBalance", typeof(double));
				dt.Columns.Add("CreditBalance", typeof(double));
				dgvOpeningBalance.DataSource = dt;
				for (int i = 0; i < getlist.Rows.Count; i++)
				{
					dt.Rows.Add(getlist.Rows[i]["Id"], getlist.Rows[i]["ChartOfAccountId"], getlist.Rows[i]["AccountType"], getlist.Rows[i]["ParentAccountTitle"], getlist.Rows[i]["ChartOfAccountTitle"], getlist.Rows[i]["YearObDebit"], getlist.Rows[i]["YearObCredit"]);
				}
				dgvOpeningBalance.DataSource = dt;
				dgvOpeningBalance.RetrieveStructure();
				GridSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FindAccountTitle()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbAccountTitle.Value);
			DataTable lst = global::Architecture.BLL.Accounts.AccountsOpeningBalances.GetAll(new AccountsOpeningBalances
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EntryUser = UserAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["ChartOfAccountTitle"]);
			}
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, cmbAccountTitle, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbAccountTitle.Value = id;
				}
				else
				{
					((Control)(object)cmbAccountTitle).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			List<getGlobalBranchesAllocateToUser> lst = clsGlobalVariables.globalBranchesAllocateToUser;
			if (dtBranch.Columns.Count == 0)
			{
				dtBranch.Columns.Add("Id", typeof(int));
				dtBranch.Columns.Add("BranchName", typeof(string));
			}
			if (lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtBranch.Rows.Add(lst[i].BranchId, lst[i].BranchName);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbBranchName, dtBranch, "Id", "BranchName", "Branch Name", AllColumns: true, Conversion.ToInt(cmbBranchName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtBranch.Rows.Count > 0)
			{
				cmbBranchName.Value = UserAccount.BranchesId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			AllRecordUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SingleRecordUpdate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			AccountsOpeningBalances accOpeningBal = new AccountsOpeningBalances();
			if (FormValidation())
			{
				if (RId == 0)
				{
					throw new Exception("ChartofAccount Id Not Found");
				}
				accOpeningBal.Id = RId;
				accOpeningBal.ChartOfAccountId = Conversion.ToInt((object)txtAcId.Text);
				accOpeningBal.ChartOfAccountTitle = ((Control)(object)cmbAccountTitle).Text;
				accOpeningBal.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				accOpeningBal.YearObCredit = Conversion.ToDouble((object)txtCredit.Text);
				accOpeningBal.YearObDebit = Conversion.ToDouble((object)txtDebit.Text);
				accOpeningBal.OrganizationId = UserAccount.OrganizationId;
				accOpeningBal.CompanyId = UserAccount.CompanyId;
				accOpeningBal.EntryUser = UserAccount.ID;
				accOpeningBal.ModifyUser = UserAccount.ID;
				accOpeningBal.EntryDate = DateTime.Now;
				accOpeningBal.ModifyDate = DateTime.Now;
				accOpeningBal.PostDate = DateTime.Now;
				accOpeningBal.PostState = false;
				accOpeningBal.PostUser = 0;
				global::Architecture.BLL.Accounts.AccountsOpeningBalances.Save(accOpeningBal);
				MessageBox.Show("Update recored Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllRecordUpdate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		try
		{
			AccountsOpeningBalances accOpeningBal = new AccountsOpeningBalances();
			accOpeningBal.OrganizationId = UserAccount.OrganizationId;
			accOpeningBal.CompanyId = UserAccount.CompanyId;
			accOpeningBal.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			accOpeningBal.OpeningBalanceList = new List<AccountsOpeningBalances>();
			GridEXRow[] rows = dgvOpeningBalance.GetRows();
			foreach (GridEXRow r in rows)
			{
				for (int i = 0; i < getlist.Rows.Count; i++)
				{
					if (Conversion.ToInt(getlist.Rows[i]["Id"]) == Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) && Conversion.ToDouble(getlist.Rows[i]["YearObDebit"]) != Convert.ToDouble(r.Cells["DebitBalance"].Value.ToString()))
					{
						AccountsOpeningBalances coaa2 = new AccountsOpeningBalances();
						coaa2.Id = int.Parse(r.Cells["Id"].Value.ToString());
						coaa2.ChartOfAccountId = int.Parse(r.Cells["ChartOfAccountId"].Value.ToString());
						coaa2.ChartOfAccountTitle = ((Control)(object)cmbAccountTitle).Text;
						coaa2.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						coaa2.YearObDebit = Convert.ToDouble(r.Cells["DebitBalance"].Value.ToString());
						coaa2.YearObCredit = Convert.ToDouble(0);
						coaa2.OrganizationId = UserAccount.OrganizationId;
						coaa2.CompanyId = UserAccount.CompanyId;
						coaa2.EntryUser = UserAccount.ID;
						coaa2.ModifyUser = UserAccount.ID;
						coaa2.EntryDate = DateTime.Now;
						coaa2.ModifyDate = DateTime.Now;
						coaa2.PostDate = DateTime.Now;
						coaa2.PostState = false;
						coaa2.PostUser = 0;
						accOpeningBal.OpeningBalanceList.Add(coaa2);
					}
					if (Conversion.ToInt(getlist.Rows[i]["Id"]) == Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) && Conversion.ToDouble(getlist.Rows[i]["YearObCredit"]) != Convert.ToDouble(r.Cells["CreditBalance"].Value.ToString()))
					{
						AccountsOpeningBalances coaa = new AccountsOpeningBalances();
						coaa.Id = int.Parse(r.Cells["Id"].Value.ToString());
						coaa.ChartOfAccountId = int.Parse(r.Cells["ChartOfAccountId"].Value.ToString());
						coaa.ChartOfAccountTitle = ((Control)(object)cmbAccountTitle).Text;
						coaa.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						coaa.YearObDebit = Convert.ToDouble(0);
						coaa.YearObCredit = Convert.ToDouble(r.Cells["CreditBalance"].Value.ToString());
						coaa.OrganizationId = UserAccount.OrganizationId;
						coaa.CompanyId = UserAccount.CompanyId;
						coaa.EntryUser = UserAccount.ID;
						coaa.ModifyUser = UserAccount.ID;
						coaa.EntryDate = DateTime.Now;
						coaa.ModifyDate = DateTime.Now;
						coaa.PostDate = DateTime.Now;
						coaa.PostState = false;
						coaa.PostUser = 0;
						accOpeningBal.OpeningBalanceList.Add(coaa);
					}
				}
			}
			global::Architecture.BLL.Accounts.AccountsOpeningBalances.SaveAllRecord(accOpeningBal);
			MessageBox.Show("Update recored Successfully");
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDebit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtCredit.Text) != 0.0)
			{
				if (Conversion.ToDouble((object)txtCredit.Text) > 0.0)
				{
					txtDebit.Text = "0";
				}
			}
			else
			{

			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCredit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtDebit.Text) != 0.0)
			{
				if (Conversion.ToDouble((object)txtDebit.Text) > 0.0)
				{
					txtCredit.Text = "0";
				}
			}
			else
			{

			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbAccountTitle_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			DataTable ob = global::Architecture.BLL.Accounts.AccountsOpeningBalances.GetById(new AccountsOpeningBalances
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = Conversion.ToInt(cmbAccountTitle.Value)
			});
			if (ob.Rows.Count > 0)
			{
				RId = Conversion.ToInt((object)ob.Rows[0]["Id"].ToString());
				txtAcId.Text = ob.Rows[0]["ChartOfAccountId"].ToString();
				txtCredit.Text = ob.Rows[0]["YearObCredit"].ToString();
				txtDebit.Text = ob.Rows[0]["YearObDebit"].ToString();
			}
			else
			{
				RId = 0;
				txtAcId.Text = "0";
				txtCredit.Text = "0";
				txtDebit.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dgvOpeningBalance_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		try
		{
			DataTable openingBalances = global::Architecture.BLL.Accounts.AccountsOpeningBalances.GetById(new AccountsOpeningBalances
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = Conversion.ToInt((object)dgvOpeningBalance.CurrentRow.Cells[0].Value.ToString())
			});
			if (openingBalances.Rows.Count > 0)
			{
				RId = Conversion.ToInt((object)openingBalances.Rows[0]["Id"].ToString());
				cmbAccountTitle.Value = openingBalances.Rows[0]["Id"].ToString();
				txtAcId.Text = openingBalances.Rows[0]["ChartOfAccountId"].ToString();
				txtDebit.Text = openingBalances.Rows[0]["YearObDebit"].ToString();
				txtCredit.Text = openingBalances.Rows[0]["YearObCredit"].ToString();
				cmbAccountTitle.Focus();
			}
			else
			{
				RId = 0;
				((Control)(object)cmbAccountTitle).Text = string.Empty;
				txtDebit.Text = string.Empty;
				txtCredit.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			dgvOpeningBalance.RootTable.Columns["Id"].Visible = false;
			dgvOpeningBalance.RootTable.Columns["ChartOfAccountId"].Visible = false;
			dgvOpeningBalance.RootTable.Columns["Id"].EditType = (EditType)0;
			dgvOpeningBalance.RootTable.Columns["ChartofAccountId"].EditType = (EditType)0;
			dgvOpeningBalance.RootTable.Columns["AccountType"].EditType = (EditType)0;
			dgvOpeningBalance.RootTable.Columns["ParentAccountTitle"].EditType = (EditType)0;
			dgvOpeningBalance.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			dgvOpeningBalance.RootTable.Columns["AccountType"].FilterEditType = (FilterEditType)1;
			dgvOpeningBalance.RootTable.Columns["ParentAccountTitle"].FilterEditType = (FilterEditType)1;
			dgvOpeningBalance.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			dgvOpeningBalance.RootTable.Columns["DebitBalance"].EditType = (EditType)1;
			dgvOpeningBalance.RootTable.Columns["CreditBalance"].EditType = (EditType)1;
			dgvOpeningBalance.RootTable.Columns["DebitBalance"].AggregateFunction = (AggregateFunction)2;
			dgvOpeningBalance.RootTable.Columns["DebitBalance"].TextAlignment = (TextAlignment)3;
			dgvOpeningBalance.RootTable.Columns["DebitBalance"].FormatString = clsGlobalVariables.stringFormatsingle;
			dgvOpeningBalance.RootTable.Columns["DebitBalance"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			dgvOpeningBalance.RootTable.Columns["CreditBalance"].AggregateFunction = (AggregateFunction)2;
			dgvOpeningBalance.RootTable.Columns["CreditBalance"].TextAlignment = (TextAlignment)3;
			dgvOpeningBalance.RootTable.Columns["CreditBalance"].FormatString = clsGlobalVariables.stringFormatsingle;
			dgvOpeningBalance.RootTable.Columns["CreditBalance"].TotalFormatString = clsGlobalVariables.stringFormatsingle;

			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmOpeningBalance_KeyPress(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			BtnNew_Click(null, null);
		}
		if (button1.Visible && button1.Enabled && e.Control && e.KeyCode == Keys.S)
		{
			button1_Click(null, null);
		}
		if (BtnEdit.Visible && BtnEdit.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			BtnEdit_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			toolStripButton1_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			cmbAccountTitle.Focus();
		}
		if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
		{
			BtnPrint_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			((Control)(object)dgvOpeningBalance).Focus();
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = null;
			if (fs != null)
			{
				dgvOpeningBalance.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			SingleRecordUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			FindAccountTitle();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dgvOpeningBalance_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			dgvOpeningBalance.UpdateData();
			GridEXRow item = dgvOpeningBalance.CurrentRow;
			if (e.Column.Key == "DebitBalance" && item != null && (int)item.RowType == 0 && item.Cells["DebitBalance"].Value.ToString() != string.Empty && Conversion.ToDouble(item.Cells["DebitBalance"].Value) > 0.0)
			{
				item.Cells["CreditBalance"].Value = 0;
			}
			if (e.Column.Key == "CreditBalance" && item != null && (int)item.RowType == 0 && item.Cells["CreditBalance"].Value.ToString() != string.Empty && Conversion.ToDouble(item.Cells["CreditBalance"].Value) > 0.0)
			{
				item.Cells["DebitBalance"].Value = 0;
			}
			dgvOpeningBalance.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherReports.OpeningBalanceReport(new AccountsOpeningBalances
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			// Reporting val = new Reporting();
			// val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			// val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			// val.ShowReportWithDataTable(dt, "129-OpeningBalanceRpt.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDebit_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtCredit_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void AcfrmOpeningBalance_FormClosing(object sender, FormClosingEventArgs e)
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

	private void dgvOpeningBalance_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = dgvOpeningBalance.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Return)
			{
				dgvOpeningBalance_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar1")
			{
				cmbAccountTitle.Focus();
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
			dt.Rows.Add("Ctrl+U", "For Update All");
			dt.Rows.Add("Ctrl+S", "For Update Single");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Account");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void dgvOpeningBalance_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "DebitBalance" || e.Column.Key == "CreditBalance") && !double.TryParse(Conversion.ToString(e.Value), out _))
			{
				throw new Exception("Please Type Only Numeric Value");
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
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_149c: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a6: Expected O, but got Unknown
		//IL_14b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bd: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AcfrmOpeningBalance));
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout dgvOpeningBalance_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.BtnEdit = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.button1 = new System.Windows.Forms.Button();
		this.cmbAccountTitle = new UltraCombo();
		this.txtCredit = new System.Windows.Forms.TextBox();
		this.txtDebit = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAcId = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.dgvOpeningBalance = new GridEX();
		this.cmbBranchName = new UltraCombo();
		this.lblBranch = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvOpeningBalance).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel1.Controls.Add(this.label10);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(788, 29);
		this.panel1.TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(5, 5);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(145, 19);
		this.label10.TabIndex = 0;
		this.label10.Text = "Opening Balance";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnNew, this.toolStripButton1, this.BtnEdit, this.BtnPrint, this.btnShortCutKey });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(788, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.BtnEdit.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnEdit.Name = "BtnEdit";
		this.BtnEdit.Size = new System.Drawing.Size(98, 24);
		this.BtnEdit.Text = "&Update All";
		this.BtnEdit.Click += new System.EventHandler(BtnEdit_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "129-Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.lblBranch);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitle);
		this.groupBox1.Controls.Add(this.txtCredit);
		this.groupBox1.Controls.Add(this.txtDebit);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(596, 95);
		this.groupBox1.TabIndex = 4;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Information";
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(468, 64);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(108, 26);
		this.button1.TabIndex = 3;
		this.button1.Text = "Update Single";
		this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button1.Leave += new System.EventHandler(button1_Leave);
		this.cmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Location = new System.Drawing.Point(111, 38);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Name = "cmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Size = new System.Drawing.Size(353, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Leave += new System.EventHandler(cmbAccountTitle_Leave);
		this.txtCredit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCredit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCredit.Location = new System.Drawing.Point(334, 66);
		this.txtCredit.Name = "txtCredit";
		this.txtCredit.Size = new System.Drawing.Size(130, 23);
		this.txtCredit.TabIndex = 2;
		this.txtCredit.TextChanged += new System.EventHandler(txtCredit_TextChanged);
		this.txtCredit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCredit_KeyPress);
		this.txtDebit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDebit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebit.Location = new System.Drawing.Point(111, 66);
		this.txtDebit.Name = "txtDebit";
		this.txtDebit.Size = new System.Drawing.Size(130, 23);
		this.txtDebit.TabIndex = 1;
		this.txtDebit.TextChanged += new System.EventHandler(txtDebit_TextChanged);
		this.txtDebit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDebit_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(245, 70);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(85, 15);
		this.label9.TabIndex = 1;
		this.label9.Text = "Credit Amount";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(28, 44);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 1;
		this.label6.Text = "Account_Title";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(28, 70);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(83, 15);
		this.label8.TabIndex = 1;
		this.label8.Text = "Debit Amount";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(17, 89);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(0, 17);
		this.label7.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(17, 55);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(0, 17);
		this.label4.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(17, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(0, 17);
		this.label1.TabIndex = 0;
		this.txtAcId.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAcId.Location = new System.Drawing.Point(374, 131);
		this.txtAcId.Name = "txtAcId";
		this.txtAcId.Size = new System.Drawing.Size(58, 22);
		this.txtAcId.TabIndex = 5;
		this.txtAcId.Visible = false;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Controls.Add(this.txtAcId);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 56);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(788, 103);
		this.panel2.TabIndex = 8;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 159);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(788, 29);
		this.panel3.TabIndex = 9;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 19);
		this.label2.TabIndex = 21;
		this.label2.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(747, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.dgvOpeningBalance;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.dgvOpeningBalance.DefaultFilterRowComparison = (FilterConditionOperator)7;
		dgvOpeningBalance_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.dgvOpeningBalance.DesignTimeLayout = dgvOpeningBalance_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.dgvOpeningBalance).Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvOpeningBalance.DynamicFiltering = true;
		this.dgvOpeningBalance.FilterMode = (FilterMode)2;
		this.dgvOpeningBalance.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.dgvOpeningBalance).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.dgvOpeningBalance.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.dgvOpeningBalance).Location = new System.Drawing.Point(0, 188);
		((System.Windows.Forms.Control)(object)this.dgvOpeningBalance).Name = "dgvOpeningBalance";
		((ScrollableBase)this.dgvOpeningBalance).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.dgvOpeningBalance).Size = new System.Drawing.Size(788, 419);
		((System.Windows.Forms.Control)(object)this.dgvOpeningBalance).TabIndex = 4;
		this.dgvOpeningBalance.TotalRow = (InheritableBoolean)1;
		this.dgvOpeningBalance.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.dgvOpeningBalance).VisualStyle = (VisualStyle)4;
		this.dgvOpeningBalance.CellUpdated += new ColumnActionEventHandler(dgvOpeningBalance_CellUpdated);
		this.dgvOpeningBalance.UpdatingCell += new UpdatingCellEventHandler(dgvOpeningBalance_UpdatingCell);
		((System.Windows.Forms.Control)(object)this.dgvOpeningBalance).DoubleClick += new System.EventHandler(dgvOpeningBalance_DoubleClick);
		((System.Windows.Forms.Control)(object)this.dgvOpeningBalance).KeyDown += new System.Windows.Forms.KeyEventHandler(dgvOpeningBalance_KeyDown);
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(111, 11);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(353, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 593;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Visible = false;
		this.lblBranch.AutoSize = true;
		this.lblBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranch.Location = new System.Drawing.Point(28, 17);
		this.lblBranch.Name = "lblBranch";
		this.lblBranch.Size = new System.Drawing.Size(79, 15);
		this.lblBranch.TabIndex = 594;
		this.lblBranch.Text = "Branch Name";
		this.lblBranch.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(788, 607);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.dgvOpeningBalance);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip2);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Name = "AcfrmOpeningBalance";
		this.Text = "Opening Balance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AcfrmOpeningBalance_FormClosing);
		base.Load += new System.EventHandler(AcfrmOpeningBalance_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmOpeningBalance_KeyPress);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvOpeningBalance).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}