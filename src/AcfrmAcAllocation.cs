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
using AccountsOpeningBalances = global::Architecture.BLL.Accounts.AccountsOpeningBalances;
using ChartofAccount = global::Architecture.BLL.Accounts.ChartofAccount;
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
using System.Linq;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.BLL.Accounts;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.Accounts;
// // using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;

public class AcfrmAcAllocation : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ContextMenuStrip contextMenuStrip1;

	private PrintDialog printDialog1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private Panel panel1;

	internal GridEX GridUnAllocatedAccounts;

	private Panel panel7;

	private TextBox txtorganization;

	private Label label16;

	private Label lbltocompany;

	private Label label17;

	private Panel panel6;

	private Label label15;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	internal GridEX GridAllocatedAccounts;

	internal UltraCombo cmbToCompany;

	internal UltraCombo cmbFromCompany;

	private Panel panel3;

	private Label label4;

	private Button BtnAllocateAccounts;

	private Panel panel5;

	private Panel panel4;

	private Label label1;

	private Panel panel8;

	private Button btnDeAllocate;

	internal UltraCombo CmbFinancialYear;

	private Label label2;

	internal UltraCombo CmbThirdLevelAccounts;

	private Label label3;

	internal UltraCombo CmbAccountType;

	private Label label5;

	private Button btnShow;

	public AcfrmAcAllocation(UserAccount puserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void AcfrmAcAllocation_Load(object sender, EventArgs e)
	{
		try
		{
			companytofill();
			AccountTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void companytofill()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbToCompany).Enabled = true;
			List<global::Architecture.Model.Company> lst = Conversion.ConvertDataTableToList<global::Architecture.Model.Company>(global::Architecture.BLL.Company.GetAlldt(new global::Architecture.Model.Company
			{
				OrgCompanyTypeId = UserAccount.OrganizationId
			}));
			if (lst.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CompanyName");
			for (int i = 0; i < lst.Count; i++)
			{
				dt.Rows.Add(lst[i].Id, lst[i].CompName);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbToCompany, "Id", "CompanyName", "Company Name", ZeroIndex: false);
			}
			List<global::Architecture.Model.Company> lstComp = lst.Where((global::Architecture.Model.Company x) => x.Id == UserAccount.CompanyId).ToList();
			if (lstComp.Count > 0 && lstComp[0].CompType == "Branch")
			{
				((Control)(object)cmbToCompany).Enabled = false;
				cmbToCompany.Value = UserAccount.CompanyId;
				if (GridAllocatedAccounts.GetDataRows().Count() == 0 && GridUnAllocatedAccounts.GetDataRows().Count() == 0)
				{
					cmbToCompany_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ThirdLevelAccountsFill()
	{
		try
		{
			ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Account_Level = 3
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountTypeFill()
	{
		try
		{
			DataTable dttype = global::Architecture.BLL.Accounts.AccountTypes.GetAll(new global::Architecture.Model.Accounts.AccountTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountType");
			for (int i = 0; i < dttype.Rows.Count; i++)
			{
				dt.Rows.Add(dttype.Rows[i]["Id"], dttype.Rows[i]["AccountType"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbAccountType, "Id", "AccountType", "Account Types", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbToCompany_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbToCompany).ActiveRow != null)
			{
				Un_AllocatedAccountsGridFill();
				AllocatedAccountsGridFill();
				BindFinancialYear();
				return;
			}
			((Control)(object)CmbFinancialYear).Text = string.Empty;
			((UltraGridBase)CmbFinancialYear).DataSource = null;
			GridAllocatedAccounts.DataSource = null;
			GridUnAllocatedAccounts.DataSource = null;
			throw new Exception("Select Company First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindFinancialYear()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbFinancialYear).Text = string.Empty;
			List<FinancialYear> lst = global::Architecture.BLL.FinancialYear.GetFinancialYearlist(new FinancialYear
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = Conversion.ToInt(cmbToCompany.Value)
			});
			if (lst != null && lst.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Period");
				for (int i = 0; i < lst.Count; i++)
				{
					dt.Rows.Add(lst[i].Id, lst[i].FinancialYearCode);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbFinancialYear, "Id", "Period", "Financial Year", ZeroIndex: false);
					((UltraGridBase)CmbFinancialYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				else
				{
					((Control)(object)CmbFinancialYear).Text = string.Empty;
					((UltraGridBase)CmbFinancialYear).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbFinancialYear).Text = string.Empty;
				((UltraGridBase)CmbFinancialYear).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Un_AllocatedAccountsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			DataTable dt = global::Architecture.BLL.Accounts.COAAllocation.GetAllocatedAndUnAllocatedAccountsToCompany(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = Conversion.ToInt(cmbToCompany.Value),
				AccountTypeIds = Conversion.ToString(CmbAccountType.Value),
				ActionId = 1
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtUnAllocate = new DataTable();
				dtUnAllocate.Columns.Add("Id");
				dtUnAllocate.Columns.Add("AccountCode");
				dtUnAllocate.Columns.Add("AccountTitle");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUnAllocate.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTitle"]);
				}
				GridUnAllocatedAccounts.DataSource = dtUnAllocate;
				GridUnAllocatedAccounts.RetrieveStructure();
				GridUnAllocatedAccountsSetting();
			}
			else
			{
				GridUnAllocatedAccounts.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridUnAllocatedAccountsSetting()
	{
		try
		{
			GridUnAllocatedAccounts.RootTable.Columns["Id"].Visible = false;
			GridUnAllocatedAccounts.RootTable.Columns["AccountCode"].Width = 90;
			GridUnAllocatedAccounts.RootTable.Columns["AccountTitle"].Width = 340;
			GridUnAllocatedAccounts.RootTable.Columns.Add("Select");
			GridUnAllocatedAccounts.RootTable.Columns["Select"].ActAsSelector = true;
			GridUnAllocatedAccounts.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridUnAllocatedAccounts.RootTable.Columns["Select"].Width = 60;
			GridUnAllocatedAccounts.RootTable.Columns["Select"].Position = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllocatedAccountsGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			DataTable dt = global::Architecture.BLL.Accounts.COAAllocation.GetAllocatedAndUnAllocatedAccountsToCompany(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = Conversion.ToInt(cmbToCompany.Value),
				AccountTypeIds = Conversion.ToString(CmbAccountType.Value),
				ActionId = 2
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtAllocate = new DataTable();
				dtAllocate.Columns.Add("Id");
				dtAllocate.Columns.Add("AccountCode");
				dtAllocate.Columns.Add("AccountTitle");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtAllocate.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTitle"]);
				}
				GridAllocatedAccounts.DataSource = dtAllocate;
				GridAllocatedAccounts.RetrieveStructure();
				GridAllocatedAccountsSetting();
			}
			else
			{
				GridAllocatedAccounts.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridAllocatedAccountsSetting()
	{
		try
		{
			GridAllocatedAccounts.RootTable.Columns["Id"].Visible = false;
			GridAllocatedAccounts.RootTable.Columns["AccountCode"].Width = 90;
			GridAllocatedAccounts.RootTable.Columns["AccountTitle"].Width = 355;
			GridAllocatedAccounts.RootTable.Columns.Add("Select");
			GridAllocatedAccounts.RootTable.Columns["Select"].ActAsSelector = true;
			GridAllocatedAccounts.RootTable.Columns["Select"].UseHeaderSelector = true;
			GridAllocatedAccounts.RootTable.Columns["Select"].Width = 60;
			GridAllocatedAccounts.RootTable.Columns["Select"].Position = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAllocateAccounts_Click(object sender, EventArgs e)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbToCompany).Text.Trim() == string.Empty)
			{
				MessageBox.Show("Select Company First");
				cmbToCompany.Focus();
				return;
			}
			if (((Control)(object)CmbFinancialYear).Text.Trim() == string.Empty)
			{
				MessageBox.Show("Select Financial Year First");
				CmbFinancialYear.Focus();
				return;
			}
			if (GridUnAllocatedAccounts.GetCheckedRows().Length > 0)
			{
				COAAllocation coaah = new COAAllocation();
				coaah.COAAllocationlist = new List<COAAllocation>();
				GridEXRow[] checkedRows = GridUnAllocatedAccounts.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					COAAllocation coaa = new COAAllocation();
					coaa.OrganizationId = UserAccount.OrganizationId;
					coaa.CompanyId = Conversion.ToInt(cmbToCompany.Value);
					coaa.BranchId = UserAccount.BranchesId;
					coaa.FinancialYearId = Conversion.ToInt(CmbFinancialYear.Value);
					coaa.EntryUserId = UserAccount.ID;
					coaa.ChartofAccountId = Conversion.ToInt(r.Cells["Id"].Value);
					coaa.AccountTitle = Conversion.ToString(r.Cells["AccountTitle"].Value);
					coaa.GLPageNo = "";
					coaa.IsActive = true;
					coaah.COAAllocationlist.Add(coaa);
				}
				global::Architecture.BLL.Accounts.COAAllocation.Save(coaah);
				MessageBox.Show("Accounts Allocated Successfully");
				AllocatedAccountsGridFill();
				Un_AllocatedAccountsGridFill();
				return;
			}
			throw new Exception("Select Row's First To Allocate");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDeAllocate_Click(object sender, EventArgs e)
	{
		try
		{
			if (GridAllocatedAccounts.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked Row's first To Un-Allocate Accounts");
			}
			if (Conversion.ToInt(cmbToCompany.Value) > 0)
			{
				if (MessageBox.Show("Are you sure to UnAllocate Accounts?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					string ids = null;
					GridEXRow[] checkedRows = GridAllocatedAccounts.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ids = ids + Conversion.ToString(item.Cells["Id"].Value) + ",";
					}
					global::Architecture.BLL.Accounts.COAAllocation.DeleteById(Conversion.ToInt(cmbToCompany.Value), ids);
					MessageBox.Show("Record's UnAllocated Successfully");
					Un_AllocatedAccountsGridFill();
					AllocatedAccountsGridFill();
				}
				return;
			}
			throw new Exception("Select Company First To Un-Allocate Accounts");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			companytofill();
			AccountTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmAcAllocation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnRefresh_Click(null, null);
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			else if (e.Control && e.KeyCode == Keys.Left)
			{
				((Control)(object)GridUnAllocatedAccounts).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				((Control)(object)GridAllocatedAccounts).Focus();
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

	private void AcfrmAcAllocation_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbToCompany).ActiveRow != null)
			{
				Un_AllocatedAccountsGridFill();
				AllocatedAccountsGridFill();
				return;
			}
			((Control)(object)CmbFinancialYear).Text = string.Empty;
			((UltraGridBase)CmbFinancialYear).DataSource = null;
			GridAllocatedAccounts.DataSource = null;
			GridUnAllocatedAccounts.DataSource = null;
			throw new Exception("Select Company First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
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
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
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
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.GridAllocatedAccounts = new GridEX();
		this.btnDeAllocate = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GridUnAllocatedAccounts = new GridEX();
		this.BtnAllocateAccounts = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.CmbFinancialYear = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbToCompany = new UltraCombo();
		this.cmbFromCompany = new UltraCombo();
		this.txtorganization = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.CmbThirdLevelAccounts = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbAccountType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedAccounts).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedAccounts).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinancialYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbToCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFromCompany).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbThirdLevelAccounts).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountType).BeginInit();
		base.SuspendLayout();
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.printDialog1.UseEXDialog = true;
		this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.dataGridViewTextBoxColumn8.HeaderText = "✓";
		this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1015, 723);
		this.panel1.TabIndex = 37;
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Controls.Add(this.btnDeAllocate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(506, 121);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(506, 602);
		this.panel4.TabIndex = 64;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocatedAccounts);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 31);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(506, 571);
		this.panel8.TabIndex = 4;
		this.GridAllocatedAccounts.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.GridAllocatedAccounts).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocatedAccounts.FilterMode = (FilterMode)2;
		this.GridAllocatedAccounts.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocatedAccounts).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocatedAccounts.GroupByBoxVisible = false;
		this.GridAllocatedAccounts.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedAccounts).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridAllocatedAccounts).Name = "GridAllocatedAccounts";
		((ScrollableBase)this.GridAllocatedAccounts).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocatedAccounts).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocatedAccounts).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocatedAccounts).Size = new System.Drawing.Size(506, 571);
		((System.Windows.Forms.Control)(object)this.GridAllocatedAccounts).TabIndex = 63;
		this.GridAllocatedAccounts.TotalRow = (InheritableBoolean)1;
		this.GridAllocatedAccounts.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocatedAccounts).VisualStyle = (VisualStyle)4;
		this.btnDeAllocate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeAllocate.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeAllocate.Location = new System.Drawing.Point(478, 3);
		this.btnDeAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeAllocate.Name = "btnDeAllocate";
		this.btnDeAllocate.Size = new System.Drawing.Size(25, 25);
		this.btnDeAllocate.TabIndex = 3;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDeAllocate_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(506, 31);
		this.label1.TabIndex = 2;
		this.label1.Text = "Allocated Accounts";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.BtnAllocateAccounts);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 121);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(506, 602);
		this.panel3.TabIndex = 63;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedAccounts);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 31);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(506, 571);
		this.panel5.TabIndex = 65;
		this.GridUnAllocatedAccounts.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedAccounts).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridUnAllocatedAccounts.FilterMode = (FilterMode)2;
		this.GridUnAllocatedAccounts.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedAccounts).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocatedAccounts.GroupByBoxVisible = false;
		this.GridUnAllocatedAccounts.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedAccounts).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedAccounts).Name = "GridUnAllocatedAccounts";
		((ScrollableBase)this.GridUnAllocatedAccounts).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocatedAccounts).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocatedAccounts).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedAccounts).Size = new System.Drawing.Size(506, 571);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedAccounts).TabIndex = 2;
		this.GridUnAllocatedAccounts.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocatedAccounts.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocatedAccounts).VisualStyle = (VisualStyle)4;
		this.BtnAllocateAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAllocateAccounts.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnAllocateAccounts.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocateAccounts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnAllocateAccounts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnAllocateAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocateAccounts.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnAllocateAccounts.Location = new System.Drawing.Point(478, 3);
		this.BtnAllocateAccounts.Margin = new System.Windows.Forms.Padding(0);
		this.BtnAllocateAccounts.Name = "BtnAllocateAccounts";
		this.BtnAllocateAccounts.Size = new System.Drawing.Size(25, 25);
		this.BtnAllocateAccounts.TabIndex = 64;
		this.BtnAllocateAccounts.UseVisualStyleBackColor = true;
		this.BtnAllocateAccounts.Click += new System.EventHandler(BtnAllocateAccounts_Click);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(506, 31);
		this.label4.TabIndex = 1;
		this.label4.Text = "Pending Accounts For Allocation";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.btnShow);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountType);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinancialYear);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbToCompany);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFromCompany);
		this.panel7.Controls.Add(this.txtorganization);
		this.panel7.Controls.Add(this.label16);
		this.panel7.Controls.Add(this.lbltocompany);
		this.panel7.Controls.Add(this.label17);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 64);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1015, 57);
		this.panel7.TabIndex = 58;
		this.CmbFinancialYear.AllowNull = (DefaultableBoolean)1;
		this.CmbFinancialYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinancialYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinancialYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinancialYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinancialYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinancialYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFinancialYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).Location = new System.Drawing.Point(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).Name = "CmbFinancialYear";
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbFinancialYear).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(240, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(90, 17);
		this.label2.TabIndex = 35;
		this.label2.Text = "Financial Year";
		this.cmbToCompany.AllowNull = (DefaultableBoolean)1;
		this.cmbToCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbToCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbToCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbToCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbToCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbToCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbToCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Location = new System.Drawing.Point(12, 26);
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Name = "cmbToCompany";
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.cmbToCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbToCompany).Leave += new System.EventHandler(cmbToCompany_Leave);
		this.cmbFromCompany.AllowNull = (DefaultableBoolean)1;
		this.cmbFromCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFromCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFromCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFromCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbFromCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFromCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFromCompany).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFromCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFromCompany).Location = new System.Drawing.Point(459, 104);
		((System.Windows.Forms.Control)(object)this.cmbFromCompany).Name = "cmbFromCompany";
		((System.Windows.Forms.Control)(object)this.cmbFromCompany).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.cmbFromCompany).TabIndex = 32;
		((System.Windows.Forms.Control)(object)this.cmbFromCompany).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbFromCompany).Visible = false;
		this.txtorganization.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtorganization.Enabled = false;
		this.txtorganization.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtorganization.Location = new System.Drawing.Point(105, 105);
		this.txtorganization.Name = "txtorganization";
		this.txtorganization.Size = new System.Drawing.Size(220, 25);
		this.txtorganization.TabIndex = 30;
		this.txtorganization.TabStop = false;
		this.txtorganization.Visible = false;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(11, 107);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(89, 17);
		this.label16.TabIndex = 8;
		this.label16.Text = "Organization";
		this.label16.Visible = false;
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(12, 5);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(84, 17);
		this.lbltocompany.TabIndex = 16;
		this.lbltocompany.Text = "To Company";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(350, 107);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(103, 17);
		this.label17.TabIndex = 16;
		this.label17.Text = "From Company";
		this.label17.Visible = false;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 29);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1015, 35);
		this.panel6.TabIndex = 44;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(13, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(176, 24);
		this.label15.TabIndex = 0;
		this.label15.Text = "Accounts Allocation";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1015, 29);
		this.panel2.TabIndex = 45;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1015, 27);
		this.toolStrip1.TabIndex = 55;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.CmbThirdLevelAccounts.AllowNull = (DefaultableBoolean)1;
		this.CmbThirdLevelAccounts.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbThirdLevelAccounts.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbThirdLevelAccounts.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbThirdLevelAccounts).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbThirdLevelAccounts.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Location = new System.Drawing.Point(686, 109);
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Name = "CmbThirdLevelAccounts";
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).Size = new System.Drawing.Size(169, 26);
		((System.Windows.Forms.Control)(object)this.CmbThirdLevelAccounts).TabIndex = 36;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(683, 89);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(122, 17);
		this.label3.TabIndex = 37;
		this.label3.Text = "3rd Level Accounts";
		this.CmbAccountType.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Location = new System.Drawing.Point(375, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Name = "CmbAccountType";
		((System.Windows.Forms.Control)(object)this.CmbAccountType).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountType).TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(375, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(90, 17);
		this.label5.TabIndex = 39;
		this.label5.Text = "Account Type";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(572, 26);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 26);
		this.btnShow.TabIndex = 3;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1015, 723);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "AcfrmAcAllocation";
		this.Text = "Accounts Allocation";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(AcfrmAcAllocation_FormClosed);
		base.Load += new System.EventHandler(AcfrmAcAllocation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmAcAllocation_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocatedAccounts).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedAccounts).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinancialYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbToCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFromCompany).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbThirdLevelAccounts).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountType).EndInit();
		base.ResumeLayout(false);
	}
}

}