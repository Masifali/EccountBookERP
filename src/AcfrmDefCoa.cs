// using Attachment = global::Architecture.Model.Attachment;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
namespace Architecture.WinApp
{
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using Bank = global::Architecture.Model.Bank;
using City = global::Architecture.Model.City;
using CheqBookHeader = global::Architecture.Model.Accounts.CheqBookHeader;
using CheqBookDetail = global::Architecture.Model.Accounts.CheqBookDetail;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;
using ChartOfAccountMultiLingo = global::Architecture.Model.Accounts.ChartOfAccountMultiLingo;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using SupplierCustomerShipToAddress = global::Architecture.Model.Inventory.SupplierCustomerShipToAddress;
using AccountsOpeningBalances = global::Architecture.BLL.Accounts.AccountsOpeningBalances;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using ShortCutKeyPopUp = global::Architecture.WinApp.Common.ShortCutKeyPopUp;
using Constants = global::Architecture.WinApp.Common.Constants;
using GridEX_Helper = global::Architecture.WinApp.Common.GridEX_Helper;
using InfragisticsHelper = global::Architecture.WinApp.Common.InfragisticsHelper;
using HierarchicalMode = Janus.Windows.GridEX.HierarchicalMode;
using SelfReferencingAggregationMode = Janus.Windows.GridEX.SelfReferencingAggregationMode;

using Architecture.WinApp.Common;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using FilterMode = Janus.Windows.GridEX.FilterMode;
using FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle;
using EditType = Janus.Windows.GridEX.EditType;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using VisualStyle = Janus.Windows.GridEX.VisualStyle;
using ScrollableBase = Janus.Windows.GridEX.ScrollableBase;
using ConditionOperator = Janus.Windows.GridEX.ConditionOperator;
using MultiCurrency = global::Architecture.Model.MultiCurrency;
using AcLookUps = global::Architecture.Model.Accounts.AcLookUps;
using AccountNotes = global::Architecture.Model.Accounts.AccountNotes;
using CustomerGroup = global::Architecture.Model.Inventory.CustomerGroup;
using AccountTypes = global::Architecture.Model.Accounts.AccountTypes;
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;

using UserAccount = global::Architecture.Model.UserAccount;
using FinancialYear = global::Architecture.Model.FinancialYear;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.BLL.Accounts;
using global::Architecture.BLL.Inventory;
using global::Architecture.BLL.Reports.Accounts;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.Accounts;
using global::Architecture.Model.Inventory;
// // // using Architecture.Reports;
// // using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using grid.User_Conrtols;

public class AcfrmDefCoa : Form
{
	private int ACCOUNTID;

	private bool FormInitialized;

	private bool MultiCurrencyFeature;

	private bool IsCustomGroupCompulsory;

	private bool FlagForHistoryCallAfterSave;

	private bool FlagForHistoryCallFromHistoryButtonClick;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formRights = new Rightsobjects();

	private HashSet<int> modifiedRowIds = new HashSet<int>();

	public UserAccount UserAccount = new UserAccount();

	private ChartofAccount coa = new ChartofAccount();

	private DataTable table = new DataTable();

	private DataTable secondlevelac = new DataTable();

	private DataTable thirdlevelac = new DataTable();

	private DataTable fourthlevelac = new DataTable();

	private DataTable fivelevelac = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtForGridLayout = new DataTable();

	private DataTable dtHistoryForUpdateAccounts = new DataTable();

	private IContainer components;

	private PrintDialog printDialog1;

	private ContextMenuStrip contextMenuStrip1;

	private Panel panel5;

	private Label label1;

	private TextBox txtacclass;

	private Label label8;

	private Label label9;

	private Label label6;

	private Label label2;

	private TextBox txtactitle;

	private Label label4;

	private Label label11;

	private TextBox txtaccode;

	private TextBox txtaclevel;

	private Label label5;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private ToolStripButton toolStripButton7;

	private Panel panel1;

	private Panel panel2;

	private Label label12;

	private Label label10;

	internal UltraCombo cmbparentac;

	internal UltraCombo cmbactype;

	internal UltraCombo cmbbsnote;

	internal UltraCombo cmbplnote;

	private CtrlGrdBar ctrlGrdBar1;

	private Button button1;

	internal GridEX grdlevel4;

	private Label label13;

	private TextBox txtPhoneNo;

	internal UltraCombo CmbCity;

	private Label label3;

	private TextBox txtOpeningBalance;

	private Label label14;

	internal UltraCombo cmbgroupdetail;

	internal GridEX grdParentCode;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbAccountTitle;

	private Label label15;

	private Label label16;

	internal UltraCombo CmbAccountTypeFilter;

	private Label label17;

	private Label label21;

	private Label label20;

	private Label label19;

	private Label label18;

	private ToolStripButton btnCityDefine;

	internal GridEX grdAllocation;

	private TextBox txtOtherCode;

	private Label label22;

	internal UltraCombo CmbCustomerGroup;

	private Label label23;

	private TabControl TabCOA;

	private TabPage TabForm;

	private Panel ParentPanel;

	private TabPage TabHistory;

	private GroupBox DetailFilterGroup;

	private Panel panel15;

	internal GridEX grdFormHistory;

	private Panel panel16;

	private Label label24;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo cmbcustomgroup;

	private Label label25;

	private ToolStripButton btnUploadExcelSheet;

	private Panel panel8;

	private Label label27;

	private GroupBox groupBox5;

	private Panel panel3;

	private Panel panel12;

	private Label label28;

	private Panel panel13;

	private Label label29;

	private Panel panel4;

	internal GridEX grdlevel3;

	private Panel panel14;

	private Label label32;

	internal GridEX grdlevel2;

	private Panel panel7;

	private Label label31;

	internal GridEX grdfrm;

	private Panel panel6;

	private Label label30;

	private Panel panel18;

	private Label label33;

	private ToolStrip toolStrip2;

	private ToolStripButton btnUpdateHistory;

	private ToolStripButton btnUpdateAccountsExcelSheet;

	private Label label7;

	private Panel panel11;

	private GroupBox groupBox6;

	private GroupBox groupBox7;

	private RadioButton rd5thLevel;

	private RadioButton rd4thLevel;

	private RadioButton rd3rdLevel;

	private RadioButton rd2ndLevel;

	private RadioButton rd1stLevel;

	private RadioButton rdGroupeExpand;

	private RadioButton rdGroupCollapsed;

	private GroupBox groupBox8;

	private RadioButton RadActiveOnly;

	private RadioButton RadAll;

	private RadioButton RadInActive;

	private Button btnshow;

	internal UltraCombo CmbCustomGroupHistory;

	private Label label26;

	internal UltraCombo CmbAccount;

	private Label label34;

	internal UltraCombo CmbCurrencyId;

	private Label lblCurrency;

	public AcfrmDefCoa(UserAccount puserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Refresh();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		((Control)(object)cmbparentac).Text = string.Empty;
		grdParentCode.DataSource = null;
	}

	private new void Refresh()
	{
		try
		{
			button1.Text = "Save";
			ACCOUNTID = 0;
			((Control)(object)cmbbsnote).Enabled = false;
			((Control)(object)cmbplnote).Enabled = false;
			((Control)(object)cmbactype).Enabled = false;
			txtactitle.Text = string.Empty;
			((Control)(object)cmbplnote).Text = string.Empty;
			((Control)(object)cmbbsnote).Text = string.Empty;
			txtacclass.Text = string.Empty;
			txtaclevel.Text = string.Empty;
			txtaccode.Text = string.Empty;
			((Control)(object)cmbgroupdetail).Text = string.Empty;
			txtPhoneNo.Text = string.Empty;
			((Control)(object)CmbCity).Text = string.Empty;
			((Control)(object)CmbCurrencyId).Text = string.Empty;
			((Control)(object)cmbactype).Text = string.Empty;
			((Control)(object)CmbAccountTypeFilter).Text = string.Empty;
			((Control)(object)CmbAccountTitle).Text = string.Empty;
			txtOpeningBalance.Text = string.Empty;
			txtOtherCode.Text = string.Empty;
			HistoryGridFill();
			cmbparentacfill();
			AccountTitleFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Expected O, but got Unknown
		try
		{
			ChartofAccount coa = new ChartofAccount();
			if (!formvalidation())
			{
				return;
			}
			coa.ParentAccountCode = Conversion.ToString(cmbparentac.Value);
			EnumAccountClass accountClassEnum;
			if (txtacclass.Text == "Capital")
			{
				coa.AccountClassEnum = (EnumAccountClass)1;
				coa.AccountClass = 1;
				TextBox textBox = txtacclass;
				accountClassEnum = coa.AccountClassEnum;
				textBox.Text = accountClassEnum.ToString();
			}
			if (txtacclass.Text == "Assets")
			{
				coa.AccountClassEnum = (EnumAccountClass)2;
				coa.AccountClass = 2;
				TextBox textBox2 = txtacclass;
				accountClassEnum = coa.AccountClassEnum;
				textBox2.Text = accountClassEnum.ToString();
			}
			if (txtacclass.Text == "Liabilities")
			{
				coa.AccountClassEnum = (EnumAccountClass)3;
				coa.AccountClass = 3;
				TextBox textBox3 = txtacclass;
				accountClassEnum = coa.AccountClassEnum;
				textBox3.Text = accountClassEnum.ToString();
			}
			if (txtacclass.Text == "Expenses")
			{
				coa.AccountClassEnum = (EnumAccountClass)4;
				coa.AccountClass = 4;
				TextBox textBox4 = txtacclass;
				accountClassEnum = coa.AccountClassEnum;
				textBox4.Text = accountClassEnum.ToString();
			}
			if (txtacclass.Text == "Revenue")
			{
				coa.AccountClassEnum = (EnumAccountClass)5;
				coa.AccountClass = 5;
				TextBox textBox5 = txtacclass;
				accountClassEnum = coa.AccountClassEnum;
				textBox5.Text = accountClassEnum.ToString();
			}
			coa.ParentCodeId = Conversion.ToInt(((UltraDropDownBase)cmbparentac).SelectedRow.Cells[2].Value);
			coa.Account_Level = Conversion.ToInt((object)txtaclevel.Text.Trim());
			coa.AccountCode = Conversion.ToString((object)txtaccode.Text.Trim());
			coa.AccountTitle = Conversion.ToString((object)txtactitle.Text.Trim());
			coa.AccountGroup = Conversion.ToString((object)((Control)(object)cmbgroupdetail).Text);
			coa.AccountTypeId = Conversion.ToInt(cmbactype.Value);
			coa.PLNoteId = Conversion.ToInt(cmbplnote.Value);
			coa.BSNoteId = Conversion.ToInt(cmbbsnote.Value);
			coa.CustomGroupId = Conversion.ToInt(cmbcustomgroup.Value);
			coa.CityId = Conversion.ToInt(CmbCity.Value);
			coa.PhoneNumber = txtPhoneNo.Text.Trim();
			coa.OtherErpCode = Conversion.ToString((object)txtOtherCode.Text.Trim());
			if (Conversion.ToDouble((object)txtOpeningBalance.Text.Trim()) > 0.0)
			{
				coa.YearObDebit = Conversion.ToDouble((object)txtOpeningBalance.Text.Trim());
			}
			if (Conversion.ToDouble((object)txtOpeningBalance.Text.Trim()) < 0.0)
			{
				coa.YearObCredit = Conversion.ToDouble((object)txtOpeningBalance.Text.Trim());
			}
			coa.OrganizationId = UserAccount.OrganizationId;
			coa.CompanyId = UserAccount.CompanyId;
			coa.BranchId = UserAccount.BranchesId;
			coa.EntryUser = UserAccount.ID;
			coa.IsActive = true;
			coa.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (Conversion.ToInt(cmbactype.Value) == 3 || Conversion.ToInt(cmbactype.Value) == 22)
			{
				coa.CustomerGroupId = Conversion.ToInt(CmbCustomerGroup.Value);
			}
			if (coa.AccountGroup == "Detail")
			{
				coa.ContactNo = txtPhoneNo.Text.Trim();
				coa.CurrencyId = Conversion.ToInt(CmbCurrencyId.Value);
				coa.COAAllocationList = new List<COAAllocation>();
				int countfalsevalue = 0;
				int count = grdAllocation.GetDataRows().Count();
				if (count == 0)
				{
					COAAllocation defaultAllocation = new COAAllocation();
					defaultAllocation.CompanyId = UserAccount.CompanyId > 0 ? UserAccount.CompanyId : 1;
					defaultAllocation.IsActive = true;
					defaultAllocation.BranchId = UserAccount.BranchesId;
					coa.COAAllocationList.Add(defaultAllocation);
				}
				GridEXRow[] dataRows = grdAllocation.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (count == 1)
					{
						COAAllocation allocation = new COAAllocation();
						allocation.CompanyId = Conversion.ToInt(r.Cells["Id"].Value);
						allocation.IsActive = true;
						allocation.BranchId = UserAccount.BranchesId;
						coa.COAAllocationList.Add(allocation);
					}
					else if (bool.Parse(r.Cells["Value"].Value.ToString()))
					{
						COAAllocation allocation2 = new COAAllocation();
						allocation2.CompanyId = Conversion.ToInt(r.Cells["Id"].Value);
						allocation2.IsActive = true;
						allocation2.BranchId = UserAccount.BranchesId;
						coa.COAAllocationList.Add(allocation2);
					}
					else
					{
						countfalsevalue++;
					}
				}
				if (count > 1 && count == countfalsevalue)
				{
					throw new Exception("Please select Branch first");
				}
			}
			Architecture.BLL.Accounts.ChartofAccount.Save(coa);
			MessageBox.Show(txtactitle.Text + " Save Successfully");
			FlagForHistoryCallAfterSave = true;
			Refresh();
			grdfrmfill();
			txtactitle.Focus();
			cmbparentac_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdfrmfill()
	{
		try
		{
			if (dtHistory.Rows.Count > 0)
			{
				table.Rows.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 1)
					{
						table.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["AccountTitle"]);
					}
				}
			}
			else
			{
				table.Clear();
			}
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			GridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["AccountCode"].Width = 100;
			grdfrm.RootTable.Columns["AccountTitle"].Width = 250;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["ParentAccountCode"].Visible = false;
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdfrm_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdfrm.SelectedItems.Count > 0)
			{
				if (dtHistory.Rows.Count > 0)
				{
					secondlevelac.Rows.Clear();
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 2 && Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == Conversion.ToString(grdfrm.CurrentRow.Cells[2].Value))
						{
							secondlevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
						}
					}
				}
				else
				{
					dtHistory.Rows.Clear();
				}
			}
			else
			{
				secondlevelac.Clear();
			}
			grdlevel2.DataSource = secondlevelac;
			grdlevel2.RetrieveStructure();
			GridSettings2ndlevel();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings2ndlevel()
	{
		try
		{
			grdlevel2.RootTable.Columns["Id"].EditType = (EditType)0;
			grdlevel2.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdlevel2.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdlevel2.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdlevel2.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdlevel2.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdlevel2.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdlevel2.RootTable.Columns["AccountCode"].Width = 100;
			grdlevel2.RootTable.Columns["AccountTitle"].Width = 250;
			grdlevel2.RootTable.Columns["Id"].Visible = false;
			grdlevel2.RootTable.Columns["ParentAccountCode"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdlevel2_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdlevel2.SelectedItems.Count > 0)
			{
				if (dtHistory.Rows.Count > 0)
				{
					thirdlevelac.Rows.Clear();
					string ParentAccountCode = "";
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						ParentAccountCode = Conversion.ToString(grdlevel2.CurrentRow.Cells[1].Value);
						if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 3 && Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == ParentAccountCode)
						{
							thirdlevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
						}
					}
				}
				else
				{
					dtHistory.Rows.Clear();
				}
				grdlevel3.DataSource = thirdlevelac;
				grdlevel3.RetrieveStructure();
				GridSettings3rdlevel();
			}
			else
			{
				thirdlevelac.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings3rdlevel()
	{
		try
		{
			grdlevel3.RootTable.Columns["Id"].EditType = (EditType)0;
			grdlevel3.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdlevel3.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdlevel3.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdlevel3.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdlevel3.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdlevel3.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdlevel3.RootTable.Columns["AccountCode"].Width = 100;
			grdlevel3.RootTable.Columns["AccountTitle"].Width = 250;
			grdlevel3.RootTable.Columns["Id"].Visible = false;
			grdlevel3.RootTable.Columns["ParentAccountCode"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdlevel3_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdlevel3.SelectedItems.Count > 0)
			{
				if (dtHistory.Rows.Count > 0)
				{
					fourthlevelac.Rows.Clear();
					string ParentAccountCode = "";
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						ParentAccountCode = Conversion.ToString(grdlevel3.CurrentRow.Cells[1].Value);
						if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 4 && Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == ParentAccountCode)
						{
							fourthlevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
						}
					}
					grdParentCode.ClearStructure();
				}
				else
				{
					dtHistory.Rows.Clear();
				}
				grdlevel4.DataSource = fourthlevelac;
				grdlevel4.RetrieveStructure();
				GridSettings4thlevel();
			}
			else
			{
				fourthlevelac.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings4thlevel()
	{
		try
		{
			grdlevel4.RootTable.Columns["Id"].EditType = (EditType)0;
			grdlevel4.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdlevel4.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdlevel4.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdlevel4.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdlevel4.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdlevel4.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdlevel4.RootTable.Columns["AccountCode"].Width = 100;
			grdlevel4.RootTable.Columns["AccountTitle"].Width = 300;
			grdlevel4.RootTable.Columns["Id"].Visible = false;
			grdlevel4.RootTable.Columns["ParentAccountCode"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings5thlevel()
	{
		try
		{
			grdParentCode.RootTable.Columns["Id"].EditType = (EditType)0;
			grdParentCode.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdParentCode.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdParentCode.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdParentCode.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdParentCode.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdParentCode.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdParentCode.RootTable.Columns["AccountCode"].Width = 100;
			grdParentCode.RootTable.Columns["AccountTitle"].Width = 300;
			grdParentCode.RootTable.Columns["Id"].Visible = false;
			grdParentCode.RootTable.Columns["ParentAccountCode"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool formvalidation()
	{
		if (txtactitle.Text.Trim() == "")
		{
			MessageBox.Show("Please Enter Account Title", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			txtactitle.Focus();
			return false;
		}
		if (((Control)(object)cmbparentac).Text.Trim() == "")
		{
			MessageBox.Show("Please Enter Parent Account", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			cmbparentac.Focus();
			return false;
		}
		if (txtaclevel.Text == "3" && ((Control)(object)cmbactype).Text.Trim() == "")
		{
			MessageBox.Show("Please Enter Account Type", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			cmbactype.Focus();
			return false;
		}
		if (((Control)(object)cmbplnote).Enabled && txtaclevel.Text == "3" && ((Control)(object)cmbplnote).Text.Trim() == "")
		{
			MessageBox.Show("Please Enter PL Notes", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			cmbplnote.Focus();
			return false;
		}
		if (((Control)(object)cmbbsnote).Enabled && txtaclevel.Text == "3" && ((Control)(object)cmbbsnote).Text.Trim() == "")
		{
			MessageBox.Show("Please Enter BS Notes", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			cmbbsnote.Focus();
			return false;
		}
		if (!CmbCustomerGroup.ReadOnly && txtaclevel.Text == "4" && ((Control)(object)CmbCustomerGroup).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Customer Group", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			CmbCustomerGroup.Focus();
			return false;
		}
		if (((UltraGridBase)cmbactype).ActiveRow != null && Conversion.ToInt(cmbactype.Value) == 3 && IsCustomGroupCompulsory && ((UltraGridBase)cmbcustomgroup).ActiveRow == null)
		{
			MessageBox.Show("Please Select Custom Group", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			cmbcustomgroup.Focus();
			return false;
		}
		if (MultiCurrencyFeature && ((Control)(object)cmbgroupdetail).Text == "Detail" && Conversion.ToInt(CmbCurrencyId.Value) == 0)
		{
			MessageBox.Show("Please Select Currency", "Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			CmbCurrencyId.Focus();
			return false;
		}
		return true;
	}

	private void cmbCityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtcity = Architecture.BLL.City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcity.Rows.Count > 0)
			{
				DataTable dtCity = new DataTable();
				dtCity.Columns.Add("Id");
				dtCity.Columns.Add("CityName");
				dtCity.Clear();
				for (int i = 0; i < dtcity.Rows.Count; i++)
				{
					dtCity.Rows.Add(dtcity.Rows[i]["Id"], dtcity.Rows[i]["CityName"]);
				}
				if (dtCity.Rows.Count > 0)
				{
					DDL.BindDDL(dtCity, CmbCity, "Id", "CityName", "Description", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CurrencyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Architecture.BLL.MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				// InfragisticsHelper.BindAndRetainSelection(CmbCurrencyId, dt, "Id", "CurrencyCode", "Currency", AllColumns: false, Conversion.ToInt(CmbCurrencyId.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbCurrencyId).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomGroupNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			AcLookUps val = new AcLookUps
			{
				AcLookUpTypesId = 1,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtLookUp = new DataTable();
			dtLookUp = Architecture.BLL.Accounts.AcLookUps.GetByProfileTypeId(val);
			DDL.BindDDLNew(dtLookUp, cmbcustomgroup, "Id", "AcLookUpsDescription", "Custom Group", false);
			((UltraGridBase)cmbcustomgroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AccountTitleFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbAccountTitle, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbparentacfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtcoaAccountgroup = new DataTable();
			dtcoaAccountgroup = Architecture.BLL.Accounts.ChartofAccount.ReadAllAccountgroup(val);
			DataTable dt = new DataTable();
			dt.Columns.Add("AccountCode");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("ParentCodeId");
			dt.Columns.Add("AccountClass");
			dt.Columns.Add("AccountLevel");
			for (int i = 0; i < dtcoaAccountgroup.Rows.Count; i++)
			{
				dt.Rows.Add(dtcoaAccountgroup.Rows[i]["AccountCode"], dtcoaAccountgroup.Rows[i]["AccountTitle"], dtcoaAccountgroup.Rows[i]["Id"], dtcoaAccountgroup.Rows[i]["ClassName"], dtcoaAccountgroup.Rows[i]["Account_Level"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbparentac, "AccountCode", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbparentac).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbbsnotefill()
	{
		try
		{
			DataTable dtaccnote = new DataTable();
			dtaccnote = Architecture.BLL.Accounts.AccountNotes.ReadByBSNote(0);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("NoteTitle");
			for (int i = 0; i < dtaccnote.Rows.Count; i++)
			{
				dt.Rows.Add(dtaccnote.Rows[i]["Id"], dtaccnote.Rows[i]["NoteTitle"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbbsnote, "Id", "NoteTitle", "Note Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbplnotefill()
	{
		try
		{
			DataTable dtplcnote = new DataTable();
			dtplcnote = Architecture.BLL.Accounts.AccountNotes.ReadByPLNote();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("NoteTitle");
			for (int i = 0; i < dtplcnote.Rows.Count; i++)
			{
				dt.Rows.Add(dtplcnote.Rows[i]["Id"], dtplcnote.Rows[i]["NoteTitle"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbplnote, "Id", "NoteTitle", "Note Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbsupgroupsfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCG = Architecture.BLL.Inventory.CustomerGroup.GetAll(new CustomerGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCG.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Description");
			for (int i = 0; i < dtCG.Rows.Count; i++)
			{
				if (Conversion.ToInt(cmbactype.Value) == 22)
				{
					if (Conversion.ToInt(dtCG.Rows[i]["Id"]) == 7 || Conversion.ToInt(dtCG.Rows[i]["Id"]) == 9 || Conversion.ToInt(dtCG.Rows[i]["Id"]) == 10)
					{
						dt.Rows.Add(dtCG.Rows[i]["Id"], dtCG.Rows[i]["Description"]);
					}
				}
				else if (Conversion.ToInt(cmbactype.Value) == 3)
				{
					if (Conversion.ToInt(dtCG.Rows[i]["Id"]) != 7 && Conversion.ToInt(dtCG.Rows[i]["Id"]) != 9 && Conversion.ToInt(dtCG.Rows[i]["Id"]) != 10)
					{
						dt.Rows.Add(dtCG.Rows[i]["Id"], dtCG.Rows[i]["Description"]);
					}
				}
				else
				{
					dt.Rows.Clear();
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCustomerGroup, "Id", "Description", "Customer Group", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AccountTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Group");
			dt.Rows.Add(2, "Detail");
			DDL.BindDDL(dt, cmbgroupdetail, "Id", "Name", "Account Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbAccountType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			AccountTypes val = new AccountTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dttype = new DataTable();
			dttype = Architecture.BLL.Accounts.AccountTypes.GetAll(val);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountType");
			for (int i = 0; i < dttype.Rows.Count; i++)
			{
				dt.Rows.Add(dttype.Rows[i]["Id"], dttype.Rows[i]["AccountType"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbactype, "Id", "AccountType", "Account Types", ZeroIndex: false);
				DDL.BindDDL(dt, CmbAccountTypeFilter, "Id", "AccountType", "Account Types", ZeroIndex: false);
				((Control)(object)cmbactype).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BranchesBindInGrid()
	{
		try
		{
			DataTable dt = CommonServices.CompanyServiceBind();
			if (dt.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Id");
				table.Columns.Add("Location");
				table.Columns.Add("Value");
				table.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					table.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompName"], true);
				}
				if (table.Rows.Count > 0)
				{
					grdAllocation.DataSource = table;
					grdAllocation.RetrieveStructure();
					AllocationGridSetting();
				}
				else
				{
					grdAllocation.ClearStructure();
				}
			}
			else
			{
				grdAllocation.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbparentac_Leave(object sender, EventArgs e)
	{
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		try
		{
			if (btnUpdate.Visible)
			{
				button1.Text = "Update";
			}
			if (btnsave.Visible)
			{
				button1.Text = "Save";
			}
			coa.OrganizationId = UserAccount.OrganizationId;
			coa.CompanyId = UserAccount.CompanyId;
			coa.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			coa.AccountCode = Convert.ToString(cmbparentac.Value);
			DataTable dtparentLevelAcc = new DataTable();
			dtparentLevelAcc = Architecture.BLL.Accounts.ChartofAccount.ReadNewCodebyParentCode(coa);
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "1")
			{
				txtacclass.Text = "Capital";
			}
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "2")
			{
				txtacclass.Text = "Assets";
			}
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "3")
			{
				txtacclass.Text = "Liabilities";
			}
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "4")
			{
				txtacclass.Text = "Expenses";
			}
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "5")
			{
				txtacclass.Text = "Revenue";
			}
			txtaclevel.Text = dtparentLevelAcc.Rows[0]["Account_Level"].ToString();
			txtaccode.Text = dtparentLevelAcc.Rows[0]["AccountCode"].ToString();
			CmbAccountType();
			if (Conversion.ToDouble((object)txtaclevel.Text.Trim()) >= 4.0)
			{
				((Control)(object)cmbactype).Text = dtparentLevelAcc.Rows[0]["AccountType"].ToString();
				((Control)(object)cmbactype).Enabled = false;
				((Control)(object)CmbCurrencyId).Enabled = true;
			}
			cmbsupgroupsfill();
			if (Conversion.ToDouble((object)txtaclevel.Text.Trim()) >= 4.0)
			{
				((Control)(object)CmbCustomerGroup).Text = dtparentLevelAcc.Rows[0]["CustomerGroupId"].ToString();
			}
			if (Conversion.ToDouble((object)txtaclevel.Text.Trim()) == 4.0)
			{
				CmbCustomerGroup.ReadOnly = false;
				if (Conversion.ToDouble((object)txtaclevel.Text.Trim()) > 3.0 && Conversion.ToDouble((object)txtaclevel.Text.Trim()) < 6.0)
				{
					CmbCustomerGroup.Value = string.Empty;
				}
			}
			else
			{
				CmbCustomerGroup.ReadOnly = true;
			}
			if (Conversion.ToInt((object)txtaclevel.Text) < 4)
			{
				((Control)(object)cmbgroupdetail).Text = "Group";
				((Control)(object)cmbgroupdetail).Enabled = false;
				((Control)(object)CmbCurrencyId).Enabled = false;
			}
			else
			{
				((Control)(object)cmbgroupdetail).Text = "Detail";
				((Control)(object)cmbgroupdetail).Enabled = false;
			}
			if (Conversion.ToInt((object)txtaclevel.Text) == 3)
			{
				((Control)(object)cmbactype).Enabled = true;
			}
			int classcode = Conversion.ToInt(dtparentLevelAcc.Rows[0]["AccountClass"]);
			if (Conversion.ToInt((object)txtaclevel.Text) == 3)
			{
				if (classcode == 1 || classcode == 2 || classcode == 3)
				{
					((Control)(object)cmbplnote).Enabled = false;
					((Control)(object)cmbbsnote).Enabled = true;
				}
				if (classcode == 4 || classcode == 5)
				{
					((Control)(object)cmbplnote).Enabled = true;
					((Control)(object)cmbbsnote).Enabled = false;
				}
			}
			if (Conversion.ToInt((object)txtaclevel.Text) != 3)
			{
				((Control)(object)cmbplnote).Enabled = false;
				((Control)(object)cmbbsnote).Enabled = false;
			}
			if (Conversion.ToInt((object)txtaclevel.Text) != 3)
			{
				((Control)(object)cmbactype).Enabled = false;
			}
			if (Conversion.ToInt((object)txtaclevel.Text) < 3)
			{
				((Control)(object)cmbactype).Enabled = false;
				((Control)(object)cmbactype).Text = string.Empty;
				((Control)(object)cmbbsnote).Text = string.Empty;
				((Control)(object)cmbplnote).Text = string.Empty;
			}
			if (Conversion.ToDouble((object)txtaclevel.Text.Trim()) >= 4.0)
			{
				((Control)(object)CmbCity).Enabled = true;
				txtPhoneNo.Enabled = true;
				txtOpeningBalance.Enabled = true;
			}
			else
			{
				((Control)(object)CmbCity).Enabled = false;
				txtPhoneNo.Enabled = false;
				txtOpeningBalance.Enabled = false;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountCode");
			dt.Columns.Add("AccountTitle");
			DataTable dtHistory = Architecture.BLL.Accounts.ChartofAccount.ReadAccountLevel(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtHistory.Rows.Count > 0)
			{
				string ParentAccountCode = "";
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					ParentAccountCode = Conversion.ToString(cmbparentac.Value);
					if (Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == ParentAccountCode)
					{
						dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["AccountTitle"]);
					}
				}
			}
			else
			{
				dt.Rows.Clear();
			}
			grdParentCode.DataSource = dt;
			grdParentCode.RetrieveStructure();
			GridParentcodeSetting();
			cmbgroupdetail_Leave(null, null);
			cmbactype_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbAccountTitle_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			int code = Architecture.BLL.Accounts.COAAllocation.GetParentCodeByChartofAccountId(new ChartofAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbAccountTitle.Value)
			});
			if (code > 0)
			{
				cmbparentac.Value = Conversion.ToString((object)code);
				cmbparentac_Leave(null, null);
				txtactitle.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCityDefine_Click_1(object sender, EventArgs e)
	{
		try
		{
			// new DefineCity(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtOpeningBalance_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-';
	}

	private void CmbAccountTypeFilter_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			DataTable dt = Architecture.BLL.Accounts.COAAllocation.Get3rdLevelGroupAccounts(new ChartofAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				AccountTypeId = Conversion.ToInt(CmbAccountTypeFilter.Value)
			});
			if (dt.Rows.Count > 0)
			{
				grdParentCode.DataSource = dt;
				grdParentCode.RetrieveStructure();
				grdParentCode.RootTable.Columns["Id"].Visible = false;
				grdParentCode.RootTable.Columns["ParentAccountCode"].Visible = false;
				grdParentCode.RootTable.Columns["AccountCode"].Width = 100;
				grdParentCode.RootTable.Columns["AccountTitle"].Width = 300;
			}
			else
			{
				grdParentCode.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbactype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbactype.Value) == 3 || Conversion.ToInt(cmbactype.Value) == 22)
			{
				CmbCustomerGroup.ReadOnly = false;
				cmbsupgroupsfill();
			}
			else
			{
				CmbCustomerGroup.Value = string.Empty;
				CmbCustomerGroup.ReadOnly = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridParentcodeSetting()
	{
		try
		{
			grdParentCode.RootTable.Columns["Id"].Visible = false;
			grdParentCode.RootTable.Columns["AccountCode"].Width = 100;
			grdParentCode.RootTable.Columns["AccountTitle"].Width = 350;
			grdParentCode.RootTable.Columns.Add("Edit");
			grdParentCode.RootTable.Columns["Edit"].Caption = "Edit";
			grdParentCode.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdParentCode.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdParentCode.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdParentCode.RootTable.Columns["Edit"].Width = 40;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Coa = new ReportsParameters();
			Coa.OrganizationId = UserAccount.OrganizationId;
			Coa.CompanyId = UserAccount.CompanyId;
			dtHistory = Architecture.BLL.Accounts.ChartofAccount.ReadAccountLevel(Coa);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AllocationGridSetting()
	{
		try
		{
			grdAllocation.RootTable.Columns["Id"].Visible = false;
			grdAllocation.RootTable.Columns["Location"].Width = 250;
			grdAllocation.RootTable.Columns["Value"].EditType = (EditType)8;
			grdAllocation.RootTable.Columns["Value"].FilterEditType = (FilterEditType)7;
			grdAllocation.RootTable.Columns["Value"].ColumnType = (ColumnType)3;
			grdAllocation.RootTable.Columns["Value"].CheckBoxTrueValue = "True";
			grdAllocation.RootTable.Columns["Value"].CheckBoxFalseValue = "False";
			grdAllocation.RootTable.Columns["Value"].UseHeaderSelector = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AcfrmDefCoa_Load(object sender, EventArgs e)
	{
		try
		{
			TabForm.AutoScroll = true;
			TabForm.AutoScrollMargin = new Size(20, 20);
			TabForm.AutoScrollMinSize = new Size(1120, 890);
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formRights.DoHaveSaveRight;
			btnUpdate.Enabled = formRights.DoHaveUpdateRights;
			btnUpdateAccountsExcelSheet.Visible = formRights.DoHaveUpdateRights;
			toolStripButton7.Enabled = formRights.DoHavePrintRights;
			MultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			IsCustomGroupCompulsory = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("CustomGroupCompulsoryOnChartofAccount"));
			table.Columns.Add("Id");
			table.Columns.Add("AccountCode");
			table.Columns.Add("ParentAccountCode");
			table.Columns.Add("AccountTitle");
			secondlevelac.Columns.Add("Id");
			secondlevelac.Columns.Add("AccountCode");
			secondlevelac.Columns.Add("ParentAccountCode");
			secondlevelac.Columns.Add("AccountTitle");
			thirdlevelac.Columns.Add("Id");
			thirdlevelac.Columns.Add("AccountCode");
			thirdlevelac.Columns.Add("ParentAccountCode");
			thirdlevelac.Columns.Add("AccountTitle");
			fourthlevelac.Columns.Add("Id");
			fourthlevelac.Columns.Add("AccountCode");
			fourthlevelac.Columns.Add("ParentAccountCode");
			fourthlevelac.Columns.Add("AccountTitle");
			fivelevelac.Columns.Add("Id");
			fivelevelac.Columns.Add("AccountCode");
			fivelevelac.Columns.Add("ParentAccountCode");
			fivelevelac.Columns.Add("AccountTitle");
			btnsave.Visible = true;
			AccountTitleFill();
			CurrencyFill();
			CustomGroupNameFill();
			AccountTypeFill();
			cmbparentacfill();
			cmbbsnotefill();
			cmbplnotefill();
			CmbAccountType();
			HistoryGridFill();
			BranchesBindInGrid();
			if (MultiCurrencyFeature)
			{
				lblCurrency.Visible = true;
				((Control)(object)CmbCurrencyId).Visible = true;
			}
			else
			{
				lblCurrency.Visible = false;
				((Control)(object)CmbCurrencyId).Visible = false;
			}
			((Control)(object)cmbgroupdetail).Enabled = false;
			((Control)(object)cmbplnote).Enabled = false;
			((Control)(object)cmbbsnote).Enabled = false;
			cmbparentac.Focus();
			cmbCityFill();
			grdfrmfill();
			((Control)(object)CmbCity).Enabled = false;
			txtPhoneNo.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AcfrmDefCoa_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				toolStripButton7_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F11)
			{
				btnUpdateHistory.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdfrm).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdfrm).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void toolStripButton7_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherReports.ChartofAccount(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsApproved = true
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				// Reporting val = new Reporting();
				// val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				// val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				// val.ShowReportWithDataTable(dt, "114-AcRptChartOfAccounts.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel3_DoubleClick(object sender, EventArgs e)
	{
		cmbparentac.Value = Conversion.ToString(grdlevel3.CurrentRow.Cells[1].Value);
		cmbparentac_Leave(null, null);
	}

	private void grdlevel2_DoubleClick(object sender, EventArgs e)
	{
		cmbparentac.Value = Conversion.ToString(grdlevel2.CurrentRow.Cells[1].Value);
		cmbparentac_Leave(null, null);
	}

	private void grdlevel4_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			ChartofAccount chart = new ChartofAccount();
			ACCOUNTID = Conversion.ToInt((object)grdlevel4.CurrentRow.Cells[0].Value.ToString());
			chart = Architecture.BLL.Accounts.ChartofAccount.GetByID(ACCOUNTID);
			cmbparentacfill();
			cmbparentac.Value = Conversion.ToString(grdlevel3.CurrentRow.Cells[1].Value);
			txtactitle.Text = Conversion.ToString(grdlevel4.CurrentRow.Cells[3].Value);
			txtOtherCode.Text = Conversion.ToString((object)chart.OtherErpCode);
			cmbparentac_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdParentCode_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void grdlevel4_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdlevel4.SelectedItems.Count > 0)
			{
				if (dtHistory.Rows.Count > 0)
				{
					fivelevelac.Rows.Clear();
					string ParentAccountCode = "";
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						ParentAccountCode = Conversion.ToString(grdlevel4.CurrentRow.Cells[1].Value);
						if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 5 && Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == ParentAccountCode)
						{
							fivelevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
						}
					}
				}
				else
				{
					dtHistory.Rows.Clear();
				}
				grdParentCode.DataSource = fivelevelac;
				grdParentCode.RetrieveStructure();
				GridSettings5thlevel();
			}
			else
			{
				fivelevelac.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel4_SelectionChanged(object sender, EventArgs e)
	{
	}

	public void ReadById(int ID)
	{
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			ACCOUNTID = ID;
			ChartofAccount chart = Architecture.BLL.Accounts.ChartofAccount.GetByID(ACCOUNTID);
			if (chart == null)
			{
				return;
			}
			((Control)(object)cmbparentac).Text = chart.ParentAccountCodeTitle;
			cmbparentac_Leave(null, null);
			if (chart.AccountGroup == "Detail")
			{
				if (chart.CurrencyId > 0)
				{
					CmbCurrencyId.Value = chart.CurrencyId;
				}
				txtPhoneNo.Text = Conversion.ToString((object)chart.ContactNo);
			}
			txtactitle.Text = Conversion.ToString((object)chart.AccountTitle);
			txtOtherCode.Text = Conversion.ToString((object)chart.OtherErpCode);
			if (btnUpdate.Visible = true)
			{
				button1.Text = "Update";
			}
			else
			{
				button1.Text = "Save";
			}
			txtactitle.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdParentCode_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt((object)grdParentCode.CurrentRow.Cells[0].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (ACCOUNTID > 0)
			{
				btnUpdate_Click(null, null);
			}
			else
			{
				btnsave_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		try
		{
			if (txtactitle.Text == string.Empty || txtactitle.Text == "0")
			{
				throw new Exception("AccountTitle Field Required");
			}
			if (MultiCurrencyFeature && ((Control)(object)cmbgroupdetail).Text == "Detail" && Conversion.ToInt(CmbCurrencyId.Value) == 0)
			{
				MessageBox.Show("Please Select Currency", "Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
				CmbCurrencyId.Focus();
				return;
			}
			Architecture.BLL.Accounts.ChartofAccount.DELETEANDUPDATECHARTOFACCOUNTBYID(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ChartOfAccountId = ACCOUNTID,
				AccountTitle = txtactitle.Text,
				OtherCode = Conversion.ToString((object)txtOtherCode.Text.Trim()),
				CurrencyId = Conversion.ToInt(CmbCurrencyId.Value),
				ContactNo = Conversion.ToString((object)txtPhoneNo.Text),
				IsApproved = true,
				ReqType = "Update",
				EntryUser = Conversion.ToInt((object)UserAccount.ID),
				ReportsParameterslist = new List<ReportsParameters>()
			});
			MessageBox.Show("Recored Update Successfully...;");
			Refresh();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			grdfrmfill();
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
			AccountTitleFill();
			CurrencyFill();
			cmbparentacfill();
			cmbCityFill();
			cmbsupgroupsfill();
			CustomGroupNameFill();
			if (grdFormHistory.RowCount > 0)
			{
				HistoryGridCombo();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TabCOA_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (TabCOA.SelectedIndex == 1 && FlagForHistoryCallAfterSave && FlagForHistoryCallFromHistoryButtonClick)
			{
				FormhistoryBind();
				FlagForHistoryCallAfterSave = false;
				FlagForHistoryCallFromHistoryButtonClick = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormhistoryBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AccountId = Conversion.ToInt(CmbAccount.Value);
			string CustomGroupIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbCustomGroupHistory).Text) != string.Empty)
			{
				DataTable dtType = (DataTable)((UltraGridBase)CmbCustomGroupHistory).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbCustomGroupHistory).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtType.Select("AcLookUpsDescription='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						CustomGroupIds = CustomGroupIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.CustomGroupIds = CustomGroupIds;
			}
			obj.ApprovedFilter = (RadAll.Checked ? "All" : null);
			obj.IsApproved = !RadInActive.Checked && RadActiveOnly.Checked;
			List<string> levels = new List<string>();
			if (rd1stLevel.Checked)
			{
				levels.Add("1");
			}
			if (rd2ndLevel.Checked)
			{
				levels.Add("1,2");
			}
			if (rd3rdLevel.Checked)
			{
				levels.Add("1,2,3");
			}
			if (rd4thLevel.Checked)
			{
				levels.Add("1,2,3,4");
			}
			if (rd5thLevel.Checked)
			{
				levels.Add("1,2,3,4,5");
			}
			if (levels.Count == 0)
			{
				throw new Exception("Please Check the Ac Levels First");
			}
			obj.AccountLevels = string.Join(",", levels);
			DataTable dt = (dtHistoryForUpdateAccounts = VoucherReports.ChartofAccount(obj));
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable filteredDt = new DataTable();
			filteredDt.Columns.Add("Id", typeof(int));
			filteredDt.Columns.Add("ParentAccountId", typeof(int));
			filteredDt.Columns.Add("AccountTitle");
			filteredDt.Columns.Add("AccountCode");
			filteredDt.Columns.Add("OtherErpCode");
			if (MultiCurrencyFeature)
			{
				filteredDt.Columns.Add("Currency");
			}
			filteredDt.Columns.Add("ParentAccountCode");
			filteredDt.Columns.Add("AccountGroup");
			filteredDt.Columns.Add("Account_Level");
			filteredDt.Columns.Add("IsActive");
			filteredDt.Columns.Add("AccountClass");
			filteredDt.Columns.Add("AccountType");
			filteredDt.Columns.Add("NoteTitle");
			dtForGridLayout = filteredDt.Clone();
			foreach (DataRow row in dt.Rows)
			{
				object Value = ((Conversion.ToInt(row["ParentCodeId"]) == 0) ? DBNull.Value : row["ParentCodeId"]);
				DataRow newRow = filteredDt.NewRow();
				newRow["Id"] = row["Id"];
				newRow["ParentAccountId"] = Value;
				newRow["AccountTitle"] = row["AccountTitle"];
				newRow["AccountCode"] = row["AccountCode"];
				newRow["OtherErpCode"] = row["OtherErpCode"];
				if (MultiCurrencyFeature)
				{
					newRow["Currency"] = row["CurrencyId"];
				}
				newRow["ParentAccountCode"] = row["ParentAccountCode"];
				newRow["AccountGroup"] = row["AccountGroup"];
				newRow["Account_Level"] = row["Account_Level"];
				newRow["IsActive"] = (Conversion.ToBool(row["IsActive"]) ? "True" : "False");
				newRow["AccountClass"] = row["AccountClass"];
				newRow["AccountType"] = row["AccountType"];
				newRow["NoteTitle"] = row["NoteTitle"];
				filteredDt.Rows.Add(newRow);
			}
			dtForGridLayout = filteredDt.Copy();
			grdFormHistory.DataSource = filteredDt;
			grdFormHistory.RetrieveStructure();
			FormHistoryGridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void EditNotEditableFields()
	{
		try
		{
			grdFormHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["ParentAccountId"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["AccountGroup"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["Account_Level"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["AccountClass"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["AccountType"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["NoteTitle"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["IsActive"].EditType = (EditType)4;
			grdFormHistory.RootTable.Columns["AccountTitle"].EditType = (EditType)1;
			grdFormHistory.RootTable.Columns["OtherErpCode"].EditType = (EditType)1;
			grdFormHistory.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["ParentAccountId"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["ParentAccountCode"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["AccountGroup"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["Account_Level"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["AccountClass"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["AccountType"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["NoteTitle"].FilterEditType = (FilterEditType)1;
			grdFormHistory.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridCombo()
	{
		try
		{
			DataTable dt = (DataTable)((UltraGridBase)CmbCurrencyId).DataSource;
			if (dt != null)
			{
				grdFormHistory.RootTable.Columns["Currency"].ColumnType = (ColumnType)0;
				grdFormHistory.RootTable.Columns["Currency"].HasValueList = true;
				grdFormHistory.RootTable.Columns["Currency"].LimitToList = true;
				grdFormHistory.RootTable.Columns["Currency"].EditType = (EditType)4;
				grdFormHistory.RootTable.Columns["Currency"].Caption = "Currency";
				grdFormHistory.RootTable.Columns["Currency"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "CurrencyCode");
				grdFormHistory.RootTable.Columns["Currency"].Width = 80;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormHistoryGridSettings()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		try
		{
			grdFormHistory.AllowEdit = (InheritableBoolean)1;
			grdFormHistory.RootTable.AllowEdit = (InheritableBoolean)1;
			grdFormHistory.GroupByBoxVisible = false;
			grdFormHistory.ColumnAutoResize = false;
			EditNotEditableFields();
			grdFormHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdFormHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdFormHistory.GridLines = (GridLines)1;
			grdFormHistory.AlternatingColors = true;
			grdFormHistory.HideSelection = (HideSelection)2;
			grdFormHistory.AllowEdit = (InheritableBoolean)2;
			grdFormHistory.Hierarchical = true;
			grdFormHistory.RootTable.HierarchicalMode = (HierarchicalMode)1;
			grdFormHistory.RootTable.SelfReferencingSettings.ParentDataMember = "Id";
			grdFormHistory.RootTable.SelfReferencingSettings.ChildDataMember = "ParentAccountId";
			grdFormHistory.RootTable.SelfReferencingSettings.ExpandColumn = grdFormHistory.RootTable.Columns["AccountTitle"];
			grdFormHistory.RootTable.SelfReferencingSettings.AggregationMode = (SelfReferencingAggregationMode)1;
			if (rdGroupCollapsed.Checked)
			{
				grdFormHistory.CollapseRecords();
			}
			else
			{
				grdFormHistory.ExpandRecords();
			}
			DataTable dttrue = new DataTable();
			dttrue.Columns.Add("Id");
			dttrue.Columns.Add("IsActive");
			dttrue.Rows.Add(0, "False");
			dttrue.Rows.Add(1, "True");
			grdFormHistory.RootTable.Columns["Id"].Visible = false;
			grdFormHistory.RootTable.Columns["ParentAccountId"].Visible = false;
			grdFormHistory.RootTable.Columns["ParentAccountCode"].Visible = false;
			if (MultiCurrencyFeature)
			{
				HistoryGridCombo();
			}
			grdFormHistory.RootTable.Columns["AccountTitle"].Width = 300;
			grdFormHistory.RootTable.Columns["AccountCode"].Width = 100; // Constants.Inventory// Constants.AccountCode;
			grdFormHistory.RootTable.Columns["OtherErpCode"].Width = 100; // Constants.Inventory// Constants.AccountCode;
			grdFormHistory.RootTable.Columns["AccountGroup"].Width = 100; // Constants.Inventory// Constants.StatusConstant;
			grdFormHistory.RootTable.Columns["Account_Level"].Width = 100; // Constants.Inventory// Constants.DocNoConstant + 10;
			grdFormHistory.RootTable.Columns["IsActive"].Width = 100; // Constants.Inventory// Constants.DocNoConstant;
			grdFormHistory.RootTable.Columns["AccountClass"].Width = 100; // Constants.Inventory// Constants.ItemClass;
			grdFormHistory.RootTable.Columns["AccountType"].Width = 100; // Constants.Inventory// Constants.DocumentType - 50;
			grdFormHistory.RootTable.Columns["NoteTitle"].Width = 100; // Constants.Inventory// Constants.AccountTitle;
			grdFormHistory.RootTable.Columns["AccountTitle"].EditType = (EditType)1;
			grdFormHistory.RootTable.Columns["OtherErpCode"].EditType = (EditType)1;
			grdFormHistory.RootTable.Columns["IsActive"].ColumnType = (ColumnType)0;
			grdFormHistory.RootTable.Columns["IsActive"].HasValueList = true;
			grdFormHistory.RootTable.Columns["IsActive"].LimitToList = true;
			grdFormHistory.RootTable.Columns["IsActive"].EditType = (EditType)4;
			grdFormHistory.RootTable.Columns["IsActive"].Caption = "IsActive";
			grdFormHistory.RootTable.Columns["IsActive"].ValueList.PopulateValueList((IEnumerable)dttrue.DefaultView, "Id", "IsActive");
			grdFormHistory.RootTable.Columns["IsActive"].Width = 60;
			grdFormHistory.RootTable.Columns["AccountTitle"].DefaultFilterRowComparison = (FilterConditionOperator)7;
			GridEXFormatCondition fc = new GridEXFormatCondition(grdFormHistory.RootTable.Columns["IsActive"], (ConditionOperator)0, (object)true);
			fc.TargetColumn = grdFormHistory.RootTable.Columns["IsActive"];
			fc.FormatStyle.ForeColor = Color.DarkGreen;
			grdFormHistory.RootTable.FormatConditions.Add(fc);
			GridEXFormatCondition fc2 = new GridEXFormatCondition(grdFormHistory.RootTable.Columns["IsActive"], (ConditionOperator)0, (object)false);
			fc2.TargetColumn = grdFormHistory.RootTable.Columns["IsActive"];
			fc2.FormatStyle.ForeColor = Color.Red;
			grdFormHistory.RootTable.FormatConditions.Add(fc2);
			grdFormHistory.RootTable.Columns.Add("Edit");
			grdFormHistory.RootTable.Columns["Edit"].Caption = "Update";
			grdFormHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFormHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdFormHistory.RootTable.Columns["Edit"].ButtonText = "Update";
			grdFormHistory.RootTable.Columns["Edit"].Width = 55;
			grdFormHistory.RootTable.Columns.Add("AddSibling");
			grdFormHistory.RootTable.Columns["AddSibling"].Caption = "Add Sibling";
			grdFormHistory.RootTable.Columns["AddSibling"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFormHistory.RootTable.Columns["AddSibling"].ButtonStyle = (ButtonStyle)5;
			grdFormHistory.RootTable.Columns["AddSibling"].ButtonText = "Add Sibling";
			grdFormHistory.RootTable.Columns["AddSibling"].Width = 85;
			grdFormHistory.RootTable.Columns.Add("AddChild");
			grdFormHistory.RootTable.Columns["AddChild"].Caption = "Add Child";
			grdFormHistory.RootTable.Columns["AddChild"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFormHistory.RootTable.Columns["AddChild"].ButtonStyle = (ButtonStyle)5;
			grdFormHistory.RootTable.Columns["AddChild"].ButtonText = "Add Child";
			grdFormHistory.RootTable.Columns["AddChild"].Width = 70;
			((Control)(object)grdFormHistory).Refresh();
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdFormHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		try
		{
			GridEXRow row = grdFormHistory.CurrentRow;
			if (row == null || (int)row.RowType != 0)
			{
				return;
			}
			int AccountLevel = Conversion.ToInt(row.Cells["Account_Level"].Value);
			if (AccountLevel == 1)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ReportsParameters coa = new ReportsParameters();
					coa.OrganizationId = UserAccount.OrganizationId;
					coa.CompanyId = UserAccount.CompanyId;
					coa.ChartOfAccountId = Conversion.ToInt(row.Cells["Id"].Value);
					coa.AccountTitle = Conversion.ToString(row.Cells["AccountTitle"].Value);
					coa.OtherCode = Conversion.ToString(row.Cells["OtherErpCode"].Value);
					if (MultiCurrencyFeature)
					{
						coa.CurrencyId = Conversion.ToInt(row.Cells["Currency"].Value);
					}
					coa.EntryUser = Conversion.ToInt((object)UserAccount.ID);
					if (Conversion.ToString((object)row.Cells["IsActive"].Text) == "True")
					{
						coa.IsApproved = true;
					}
					else
					{
						coa.IsApproved = false;
					}
					coa.ReqType = "Update";
					coa.ReportsParameterslist = new List<ReportsParameters>();
					Architecture.BLL.Accounts.ChartofAccount.DELETEANDUPDATECHARTOFACCOUNTBYID(coa);
					MessageBox.Show("Record Update Successfully...;");
					FormhistoryBind();
					if (btnUpdate.Visible)
					{
						button1.Text = "Update";
					}
					else
					{
						button1.Text = "Save";
					}
				}
			}
			else if (e.Column.Key == "AddSibling")
			{
				btnsave.Visible = true;
				cmbparentac.Value = Conversion.ToInt(row.Cells["ParentAccountCode"].Value);
				TabCOA.SelectedIndex = 0;
				FlagForHistoryCallFromHistoryButtonClick = true;
				cmbparentac_Leave(null, null);
				cmbactype_Leave(null, null);
				txtactitle.Focus();
			}
			else if (e.Column.Key == "AddChild" && AccountLevel != 4)
			{
				btnsave.Visible = true;
				cmbparentac.Value = Conversion.ToInt(row.Cells["AccountCode"].Value);
				TabCOA.SelectedIndex = 0;
				FlagForHistoryCallFromHistoryButtonClick = true;
				cmbparentac_Leave(null, null);
				cmbactype_Leave(null, null);
				txtactitle.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				// GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdFormHistory, dtForGridLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '-';
	}

	private void cmbgroupdetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbgroupdetail).Text == "Detail")
			{
				((Control)(object)cmbcustomgroup).Enabled = true;
				return;
			}
			((Control)(object)cmbcustomgroup).Enabled = false;
			((Control)(object)cmbcustomgroup).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateHistory_Click(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure you want to update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (grdFormHistory.RowCount > 0)
			{
				ReportsParameters coa = new ReportsParameters();
				coa.ReportsParameterslist = new List<ReportsParameters>();
				foreach (int id in modifiedRowIds)
				{
					GridEXRow row = grdFormHistory.GetDataRows().Cast<GridEXRow>().FirstOrDefault((GridEXRow r) => Conversion.ToInt(r.Cells["Id"].Value) == id);
					if (row == null)
					{
						continue;
					}
					DataRow dtRow = dtHistoryForUpdateAccounts.AsEnumerable().FirstOrDefault((DataRow x) => x.Field<int>("Id") == id);
					if (dtRow != null)
					{
						string oldTitle = dtRow.Field<string>("AccountTitle");
						string oldErp = dtRow.Field<string>("OtherErpCode");
						bool oldActive = dtRow.Field<bool>("IsActive");
						int oldCurrency = 0;
						if (MultiCurrencyFeature)
						{
							oldCurrency = dtRow.Field<int?>("CurrencyId").GetValueOrDefault();
						}
						string newTitle = Conversion.ToString(row.Cells["AccountTitle"].Value);
						string newErpCode = Conversion.ToString(row.Cells["OtherErpCode"].Value);
						bool newActive = Conversion.ToString((object)row.Cells["IsActive"].Text) == "True";
						int newCurrency = 0;
						if (MultiCurrencyFeature)
						{
							newCurrency = Conversion.ToInt(row.Cells["Currency"].Value);
						}
						bool num = !string.Equals(oldTitle, newTitle);
						bool erpChanged = !string.Equals(oldErp, newErpCode);
						bool activeChanged = !object.Equals(oldActive, newActive);
						bool currencyChange = oldCurrency != newCurrency;
						if (num || erpChanged || activeChanged || currencyChange)
						{
							ReportsParameters detail = new ReportsParameters
							{
								CompanyId = UserAccount.CompanyId,
								OrganizationId = UserAccount.OrganizationId,
								EntryUser = Convert.ToInt32(UserAccount.ID),
								ChartOfAccountId = id,
								OtherCode = newErpCode,
								AccountTitle = newTitle,
								IsApproved = newActive,
								CurrencyId = newCurrency,
								ReqType = "Update"
							};
							coa.ReportsParameterslist.Add(detail);
						}
					}
				}
				if (coa.ReportsParameterslist.Count > 0)
				{
					Architecture.BLL.Accounts.ChartofAccount.DELETEANDUPDATECHARTOFACCOUNTBYID(coa);
					MessageBox.Show("Record updated successfully.");
					FormhistoryBind();
					modifiedRowIds.Clear();
				}
				else
				{
					MessageBox.Show("No changes were found in the grid...");
				}
			}
			else
			{
				MessageBox.Show("Record Not found in the grid...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUploadExcelSheet_Click(object sender, EventArgs e)
	{
		try
		{
			// new frmExcelSheetUpload(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateAccountsExcelSheet_Click(object sender, EventArgs e)
	{
		try
		{
			// frmExcelSheetTitleUpdate // frmExcelSheetTitleUpdate = new // frmExcelSheetTitleUpdate(UserAccount);
			// frmExcelSheetTitleUpdate.ShowDialog();
			// frmExcelSheetTitleUpdate.BringToFront();
			FormhistoryBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AcfrmDefCoa_FormClosing(object sender, FormClosingEventArgs e)
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

	private void grdFormHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFormHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int num = Convert.ToInt32(r.Cells["Account_Level"].Value);
			bool allow = num != 1;
			grdFormHistory.AllowEdit = (InheritableBoolean)(allow ? 1 : 2);
			grdFormHistory.RootTable.AllowEdit = (InheritableBoolean)(allow ? 1 : 2);
			if (num >= 4)
			{
				grdFormHistory.RootTable.Columns["IsActive"].EditType = (EditType)4;
				if (MultiCurrencyFeature)
				{
					grdFormHistory.RootTable.Columns["Currency"].EditType = (EditType)4;
				}
			}
			else
			{
				grdFormHistory.RootTable.Columns["IsActive"].EditType = (EditType)0;
				if (MultiCurrencyFeature)
				{
					grdFormHistory.RootTable.Columns["Currency"].EditType = (EditType)0;
				}
			}
			if (allow)
			{
				string[] array = new string[9] { "Id", "ParentAccountId", "ParentAccountCode", "AccountCode", "AccountGroup", "Account_Level", "AccountClass", "AccountType", "NoteTitle" };
				foreach (string col in array)
				{
					grdFormHistory.RootTable.Columns[col].EditType = (EditType)0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFormHistory_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdFormHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				int id = Convert.ToInt32(row.Cells["Id"].Value);
				modifiedRowIds.Add(id);
			}
		}
		catch
		{
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			FormhistoryBind();
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
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
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
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Expected O, but got Unknown
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Expected O, but got Unknown
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Expected O, but got Unknown
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Expected O, but got Unknown
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Expected O, but got Unknown
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Expected O, but got Unknown
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Expected O, but got Unknown
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Expected O, but got Unknown
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Expected O, but got Unknown
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Expected O, but got Unknown
		//IL_0d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8e: Expected O, but got Unknown
		//IL_6bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c04: Expected O, but got Unknown
		//IL_6c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c1b: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
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
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
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
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AcfrmDefCoa));
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdParentCode = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.CmbCurrencyId = new UltraCombo();
		this.lblCurrency = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdAllocation = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbbsnote = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtaclevel = new System.Windows.Forms.TextBox();
		this.txtaccode = new System.Windows.Forms.TextBox();
		this.DetailFilterGroup = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbAccountTitle = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbAccountTypeFilter = new UltraCombo();
		this.cmbcustomgroup = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtactitle = new System.Windows.Forms.TextBox();
		this.txtOtherCode = new System.Windows.Forms.TextBox();
		this.CmbCustomerGroup = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtOpeningBalance = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtPhoneNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbCity = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.txtacclass = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbgroupdetail = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbparentac = new UltraCombo();
		this.cmbactype = new UltraCombo();
		this.cmbplnote = new UltraCombo();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
		this.btnCityDefine = new System.Windows.Forms.ToolStripButton();
		this.btnUploadExcelSheet = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.TabCOA = new System.Windows.Forms.TabControl();
		this.TabForm = new System.Windows.Forms.TabPage();
		this.ParentPanel = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdlevel4 = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label33 = new System.Windows.Forms.Label();
		this.grdlevel3 = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.grdlevel2 = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label31 = new System.Windows.Forms.Label();
		this.grdfrm = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.TabHistory = new System.Windows.Forms.TabPage();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdFormHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.rd5thLevel = new System.Windows.Forms.RadioButton();
		this.rd4thLevel = new System.Windows.Forms.RadioButton();
		this.rd3rdLevel = new System.Windows.Forms.RadioButton();
		this.rd2ndLevel = new System.Windows.Forms.RadioButton();
		this.rd1stLevel = new System.Windows.Forms.RadioButton();
		this.rdGroupeExpand = new System.Windows.Forms.RadioButton();
		this.rdGroupCollapsed = new System.Windows.Forms.RadioButton();
		this.groupBox8 = new System.Windows.Forms.GroupBox();
		this.RadActiveOnly = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.RadInActive = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbCustomGroupHistory = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbAccount = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnUpdateHistory = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateAccountsExcelSheet = new System.Windows.Forms.ToolStripButton();
		this.label24 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentCode).BeginInit();
		this.panel13.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyId).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbsnote).BeginInit();
		this.DetailFilterGroup.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypeFilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomgroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgroupdetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentac).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbactype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbplnote).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.TabCOA.SuspendLayout();
		this.TabForm.SuspendLayout();
		this.ParentPanel.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel4).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel3).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel2).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.TabHistory.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFormHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox6.SuspendLayout();
		this.groupBox7.SuspendLayout();
		this.groupBox8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroupHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).BeginInit();
		this.panel16.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.printDialog1.UseEXDialog = true;
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentCode);
		this.panel5.Controls.Add(this.panel13);
		this.panel5.Controls.Add(this.groupBox5);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 25);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(654, 690);
		this.panel5.TabIndex = 2;
		this.grdParentCode.AllowEdit = (InheritableBoolean)2;
		this.grdParentCode.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdParentCode).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdParentCode.FilterMode = (FilterMode)2;
		this.grdParentCode.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentCode).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentCode.GroupByBoxVisible = false;
		this.grdParentCode.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdParentCode).Location = new System.Drawing.Point(0, 403);
		((System.Windows.Forms.Control)(object)this.grdParentCode).Name = "grdParentCode";
		((ScrollableBase)this.grdParentCode).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdParentCode).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdParentCode).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentCode).Size = new System.Drawing.Size(654, 287);
		((System.Windows.Forms.Control)(object)this.grdParentCode).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdParentCode).TabStop = false;
		this.grdParentCode.TotalRow = (InheritableBoolean)1;
		this.grdParentCode.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentCode).VisualStyle = (VisualStyle)4;
		this.grdParentCode.ColumnButtonClick += new ColumnActionEventHandler(grdParentCode_ColumnButtonClick);
		this.grdParentCode.SelectionChanged += new System.EventHandler(grdParentCode_SelectionChanged);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label29);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 381);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(654, 22);
		this.panel13.TabIndex = 77;
		this.label29.AutoSize = true;
		this.label29.BackColor = System.Drawing.Color.Teal;
		this.label29.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(5, 2);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(174, 17);
		this.label29.TabIndex = 0;
		this.label29.Text = "Child Of Selected Parent";
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrencyId);
		this.groupBox5.Controls.Add(this.lblCurrency);
		this.groupBox5.Controls.Add(this.panel3);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbsnote);
		this.groupBox5.Controls.Add(this.label5);
		this.groupBox5.Controls.Add(this.txtaclevel);
		this.groupBox5.Controls.Add(this.txtaccode);
		this.groupBox5.Controls.Add(this.DetailFilterGroup);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcustomgroup);
		this.groupBox5.Controls.Add(this.label4);
		this.groupBox5.Controls.Add(this.label25);
		this.groupBox5.Controls.Add(this.txtactitle);
		this.groupBox5.Controls.Add(this.txtOtherCode);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerGroup);
		this.groupBox5.Controls.Add(this.label2);
		this.groupBox5.Controls.Add(this.label23);
		this.groupBox5.Controls.Add(this.label6);
		this.groupBox5.Controls.Add(this.txtOpeningBalance);
		this.groupBox5.Controls.Add(this.label21);
		this.groupBox5.Controls.Add(this.label14);
		this.groupBox5.Controls.Add(this.label9);
		this.groupBox5.Controls.Add(this.label13);
		this.groupBox5.Controls.Add(this.label20);
		this.groupBox5.Controls.Add(this.txtPhoneNo);
		this.groupBox5.Controls.Add(this.label8);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox5.Controls.Add(this.label3);
		this.groupBox5.Controls.Add(this.label19);
		this.groupBox5.Controls.Add(this.button1);
		this.groupBox5.Controls.Add(this.txtacclass);
		this.groupBox5.Controls.Add(this.label22);
		this.groupBox5.Controls.Add(this.label18);
		this.groupBox5.Controls.Add(this.label11);
		this.groupBox5.Controls.Add(this.label1);
		this.groupBox5.Controls.Add(this.label17);
		this.groupBox5.Controls.Add(this.label10);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgroupdetail);
		this.groupBox5.Controls.Add(this.label12);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparentac);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbactype);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbplnote);
		this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(0, 25);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(654, 356);
		this.groupBox5.TabIndex = 75;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Account Information";
		this.CmbCurrencyId.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrencyId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrencyId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrencyId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrencyId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCurrencyId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyId).Location = new System.Drawing.Point(108, 231);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyId).Name = "CmbCurrencyId";
		((System.Windows.Forms.Control)(object)this.CmbCurrencyId).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyId).TabIndex = 8;
		this.lblCurrency.AutoSize = true;
		this.lblCurrency.BackColor = System.Drawing.Color.White;
		this.lblCurrency.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCurrency.Location = new System.Drawing.Point(6, 237);
		this.lblCurrency.Name = "lblCurrency";
		this.lblCurrency.Size = new System.Drawing.Size(57, 15);
		this.lblCurrency.TabIndex = 75;
		this.lblCurrency.Text = "Currency ";
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocation);
		this.panel3.Controls.Add(this.panel12);
		this.panel3.Location = new System.Drawing.Point(383, 232);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(264, 120);
		this.panel3.TabIndex = 73;
		this.grdAllocation.AllowDelete = (InheritableBoolean)1;
		this.grdAllocation.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAllocation.GroupByBoxVisible = false;
		this.grdAllocation.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdAllocation).Name = "grdAllocation";
		((ScrollableBase)this.grdAllocation).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAllocation).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdAllocation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Size = new System.Drawing.Size(264, 99);
		((System.Windows.Forms.Control)(object)this.grdAllocation).TabIndex = 0;
		this.grdAllocation.TotalRow = (InheritableBoolean)1;
		this.grdAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocation).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label28);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(264, 21);
		this.panel12.TabIndex = 78;
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(7, 2);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(228, 17);
		this.label28.TabIndex = 0;
		this.label28.Text = "Account Allocation To Locations";
		this.cmbbsnote.AllowNull = (DefaultableBoolean)1;
		this.cmbbsnote.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbsnote.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbsnote.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbsnote).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbbsnote).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbsnote).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbsnote).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbsnote.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbsnote).Location = new System.Drawing.Point(108, 123);
		((System.Windows.Forms.Control)(object)this.cmbbsnote).Name = "cmbbsnote";
		((System.Windows.Forms.Control)(object)this.cmbbsnote).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbbsnote).TabIndex = 4;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 49);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(78, 15);
		this.label5.TabIndex = 2;
		this.label5.Text = "Account Title";
		this.txtaclevel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.txtaclevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaclevel.Enabled = false;
		this.txtaclevel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaclevel.Location = new System.Drawing.Point(504, 98);
		this.txtaclevel.Name = "txtaclevel";
		this.txtaclevel.ReadOnly = true;
		this.txtaclevel.Size = new System.Drawing.Size(141, 23);
		this.txtaclevel.TabIndex = 12;
		this.txtaclevel.TabStop = false;
		this.txtaccode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.txtaccode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaccode.Enabled = false;
		this.txtaccode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaccode.Location = new System.Drawing.Point(504, 45);
		this.txtaccode.Name = "txtaccode";
		this.txtaccode.ReadOnly = true;
		this.txtaccode.Size = new System.Drawing.Size(141, 23);
		this.txtaccode.TabIndex = 10;
		this.txtaccode.TabStop = false;
		this.txtaccode.Text = " ";
		this.DetailFilterGroup.Controls.Add(this.label16);
		this.DetailFilterGroup.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.DetailFilterGroup.Controls.Add(this.label15);
		this.DetailFilterGroup.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter);
		this.DetailFilterGroup.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.DetailFilterGroup.Location = new System.Drawing.Point(5, 265);
		this.DetailFilterGroup.Name = "DetailFilterGroup";
		this.DetailFilterGroup.Size = new System.Drawing.Size(367, 83);
		this.DetailFilterGroup.TabIndex = 70;
		this.DetailFilterGroup.TabStop = false;
		this.DetailFilterGroup.Text = "Filter Accounts In Child Grid";
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(6, 27);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(80, 15);
		this.label16.TabIndex = 31;
		this.label16.Text = "Account Type";
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance143;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(99, 49);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Leave += new System.EventHandler(CmbAccountTitle_Leave);
this.CmbAccountTitle.ValueChanged += new System.EventHandler(CmbAccountTitle_Leave);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.White;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(6, 55);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(78, 15);
		this.label15.TabIndex = 29;
		this.label15.Text = "Account Title";
		this.CmbAccountTypeFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTypeFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTypeFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTypeFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTypeFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Location = new System.Drawing.Point(99, 21);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Name = "CmbAccountTypeFilter";
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Leave += new System.EventHandler(CmbAccountTypeFilter_Leave);
this.CmbAccountTypeFilter.ValueChanged += new System.EventHandler(CmbAccountTypeFilter_Leave);
		this.cmbcustomgroup.AllowNull = (DefaultableBoolean)1;
		this.cmbcustomgroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcustomgroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcustomgroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomgroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomgroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcustomgroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcustomgroup).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbcustomgroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcustomgroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcustomgroup).Location = new System.Drawing.Point(108, 177);
		((System.Windows.Forms.Control)(object)this.cmbcustomgroup).Name = "cmbcustomgroup";
		((System.Windows.Forms.Control)(object)this.cmbcustomgroup).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbcustomgroup).TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(383, 102);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 9;
		this.label4.Text = "Account Level";
		this.label25.AutoSize = true;
		this.label25.BackColor = System.Drawing.Color.White;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(6, 183);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(84, 15);
		this.label25.TabIndex = 72;
		this.label25.Text = "Custom Group";
		this.txtactitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtactitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtactitle.Location = new System.Drawing.Point(108, 45);
		this.txtactitle.Name = "txtactitle";
		this.txtactitle.Size = new System.Drawing.Size(268, 23);
		this.txtactitle.TabIndex = 1;
		this.txtOtherCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherCode.Location = new System.Drawing.Point(504, 125);
		this.txtOtherCode.Name = "txtOtherCode";
		this.txtOtherCode.Size = new System.Drawing.Size(141, 23);
		this.txtOtherCode.TabIndex = 13;
		this.CmbCustomerGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).Location = new System.Drawing.Point(108, 150);
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).Name = "CmbCustomerGroup";
		this.CmbCustomerGroup.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerGroup).TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(383, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 15);
		this.label2.TabIndex = 2;
		this.label2.Text = "Account Class";
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.White;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(6, 156);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(92, 15);
		this.label23.TabIndex = 69;
		this.label23.Text = "Sup/Cust Group";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(383, 24);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 15);
		this.label6.TabIndex = 11;
		this.label6.Text = "Group/Detail";
		this.txtOpeningBalance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.txtOpeningBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOpeningBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOpeningBalance.Location = new System.Drawing.Point(504, 179);
		this.txtOpeningBalance.Name = "txtOpeningBalance";
		this.txtOpeningBalance.Size = new System.Drawing.Size(141, 23);
		this.txtOpeningBalance.TabIndex = 15;
		this.txtOpeningBalance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOpeningBalance_KeyPress);
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.White;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Red;
		this.label21.Location = new System.Drawing.Point(92, 128);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(14, 17);
		this.label21.TabIndex = 67;
		this.label21.Text = "*";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(383, 183);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(53, 15);
		this.label14.TabIndex = 23;
		this.label14.Text = "Opening";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 102);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(59, 15);
		this.label9.TabIndex = 12;
		this.label9.Text = "P/L Notes";
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(383, 156);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(60, 15);
		this.label13.TabIndex = 22;
		this.label13.Text = "Phone No";
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.White;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(92, 101);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 17);
		this.label20.TabIndex = 66;
		this.label20.Text = "*";
		this.txtPhoneNo.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.txtPhoneNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPhoneNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPhoneNo.Location = new System.Drawing.Point(504, 152);
		this.txtPhoneNo.Name = "txtPhoneNo";
		this.txtPhoneNo.Size = new System.Drawing.Size(141, 23);
		this.txtPhoneNo.TabIndex = 14;
		this.txtPhoneNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPhoneNo_KeyPress);
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 75);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 15);
		this.label8.TabIndex = 4;
		this.label8.Text = "Account Type";
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(108, 204);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 210);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(62, 15);
		this.label3.TabIndex = 19;
		this.label3.Text = "City Name";
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.White;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(490, 23);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 34;
		this.label19.Text = "*";
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.button1.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(588, 204);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(57, 26);
		this.button1.TabIndex = 16;
		this.button1.Text = "Save";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.txtacclass.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.txtacclass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtacclass.Enabled = false;
		this.txtacclass.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtacclass.Location = new System.Drawing.Point(504, 71);
		this.txtacclass.Name = "txtacclass";
		this.txtacclass.ReadOnly = true;
		this.txtacclass.Size = new System.Drawing.Size(141, 23);
		this.txtacclass.TabIndex = 11;
		this.txtacclass.TabStop = false;
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.White;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(383, 129);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(67, 15);
		this.label22.TabIndex = 2;
		this.label22.Text = "Other Code";
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.White;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(92, 74);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 33;
		this.label18.Text = "*";
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(6, 129);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 8;
		this.label11.Text = "B/S Notes";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 24);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(89, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Parent Account";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(383, 49);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 15);
		this.label17.TabIndex = 32;
		this.label17.Text = "Account Code";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Transparent;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Red;
		this.label10.Location = new System.Drawing.Point(92, 23);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(14, 17);
		this.label10.TabIndex = 17;
		this.label10.Text = "*";
		this.cmbgroupdetail.AllowNull = (DefaultableBoolean)1;
		this.cmbgroupdetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgroupdetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgroupdetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbgroupdetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Location = new System.Drawing.Point(504, 18);
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Name = "cmbgroupdetail";
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Leave += new System.EventHandler(cmbgroupdetail_Leave);
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Red;
		this.label12.Location = new System.Drawing.Point(92, 48);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(14, 17);
		this.label12.TabIndex = 18;
		this.label12.Text = "*";
		this.cmbparentac.AllowNull = (DefaultableBoolean)1;
		this.cmbparentac.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentac.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentac.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbparentac).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentac).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbparentac).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentac).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentac).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbparentac.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparentac).Location = new System.Drawing.Point(108, 18);
		((System.Windows.Forms.Control)(object)this.cmbparentac).Name = "cmbparentac";
		((System.Windows.Forms.Control)(object)this.cmbparentac).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbparentac).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbparentac).Leave += new System.EventHandler(cmbparentac_Leave);
		this.cmbactype.AllowNull = (DefaultableBoolean)1;
		this.cmbactype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbactype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbactype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbactype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbactype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbactype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbactype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbactype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.cmbactype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbactype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbactype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbactype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbactype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbactype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbactype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbactype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbactype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbactype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbactype).Location = new System.Drawing.Point(108, 69);
		((System.Windows.Forms.Control)(object)this.cmbactype).Name = "cmbactype";
		((System.Windows.Forms.Control)(object)this.cmbactype).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbactype).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbactype).Leave += new System.EventHandler(cmbactype_Leave);
		this.cmbplnote.AllowNull = (DefaultableBoolean)1;
		this.cmbplnote.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbplnote.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbplnote.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbplnote).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbplnote).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbplnote).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbplnote).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbplnote).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbplnote).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbplnote).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbplnote).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbplnote).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbplnote).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbplnote).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbplnote).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbplnote).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbplnote.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbplnote).Location = new System.Drawing.Point(108, 96);
		((System.Windows.Forms.Control)(object)this.cmbplnote).Name = "cmbplnote";
		((System.Windows.Forms.Control)(object)this.cmbplnote).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbplnote).TabIndex = 3;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label27);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(654, 25);
		this.panel8.TabIndex = 74;
		this.label27.AutoSize = true;
		this.label27.BackColor = System.Drawing.Color.Teal;
		this.label27.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(2, 2);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(230, 19);
		this.label27.TabIndex = 74;
		this.label27.Text = "Chart of Account Definition";
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.toolStripButton7, this.btnCityDefine, this.btnUploadExcelSheet });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1121, 25);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 22);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 22);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripButton7.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.Image = (System.Drawing.Image)resources.GetObject("toolStripButton7.Image");
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(92, 22);
		this.toolStripButton7.Text = "114-Print";
		this.toolStripButton7.Click += new System.EventHandler(toolStripButton7_Click);
		this.btnCityDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCityDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCityDefine.Name = "btnCityDefine";
		this.btnCityDefine.Size = new System.Drawing.Size(83, 22);
		this.btnCityDefine.Text = "Define City";
		this.btnCityDefine.Click += new System.EventHandler(btnCityDefine_Click_1);
		this.btnUploadExcelSheet.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUploadExcelSheet.Image = Architecture.WinApp.Properties.Resources.icons8_excel_48;
		this.btnUploadExcelSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUploadExcelSheet.Name = "btnUploadExcelSheet";
		this.btnUploadExcelSheet.Size = new System.Drawing.Size(158, 22);
		this.btnUploadExcelSheet.Text = "Upload Excel Sheet";
		this.btnUploadExcelSheet.Click += new System.EventHandler(btnUploadExcelSheet_Click);
		this.panel1.AutoScroll = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.TabCOA);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1135, 747);
		this.panel1.TabIndex = 1;
		this.TabCOA.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.TabCOA.Controls.Add(this.TabForm);
		this.TabCOA.Controls.Add(this.TabHistory);
		this.TabCOA.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TabCOA.Location = new System.Drawing.Point(0, 0);
		this.TabCOA.Name = "TabCOA";
		this.TabCOA.SelectedIndex = 0;
		this.TabCOA.Size = new System.Drawing.Size(1135, 747);
		this.TabCOA.TabIndex = 67;
		this.TabCOA.SelectedIndexChanged += new System.EventHandler(TabCOA_SelectedIndexChanged);
		this.TabForm.AutoScroll = true;
		this.TabForm.Controls.Add(this.ParentPanel);
		this.TabForm.Location = new System.Drawing.Point(4, 4);
		this.TabForm.Name = "TabForm";
		this.TabForm.Padding = new System.Windows.Forms.Padding(3);
		this.TabForm.Size = new System.Drawing.Size(1127, 721);
		this.TabForm.TabIndex = 1;
		this.TabForm.Text = "Form";
		this.TabForm.UseVisualStyleBackColor = true;
		this.ParentPanel.AutoScroll = true;
		this.ParentPanel.Controls.Add(this.panel4);
		this.ParentPanel.Controls.Add(this.panel5);
		this.ParentPanel.Controls.Add(this.panel2);
		this.ParentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ParentPanel.Location = new System.Drawing.Point(3, 3);
		this.ParentPanel.Name = "ParentPanel";
		this.ParentPanel.Size = new System.Drawing.Size(1121, 715);
		this.ParentPanel.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdlevel4);
		this.panel4.Controls.Add(this.panel18);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdlevel3);
		this.panel4.Controls.Add(this.panel14);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdlevel2);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(654, 25);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(425, 690);
		this.panel4.TabIndex = 67;
		this.grdlevel4.AllowDelete = (InheritableBoolean)1;
		this.grdlevel4.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdlevel4).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdlevel4.FilterMode = (FilterMode)2;
		this.grdlevel4.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdlevel4).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdlevel4.GroupByBoxVisible = false;
		this.grdlevel4.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdlevel4).Location = new System.Drawing.Point(0, 556);
		((System.Windows.Forms.Control)(object)this.grdlevel4).Name = "grdlevel4";
		((ScrollableBase)this.grdlevel4).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdlevel4).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdlevel4).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdlevel4).Size = new System.Drawing.Size(425, 134);
		((System.Windows.Forms.Control)(object)this.grdlevel4).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdlevel4).TabStop = false;
		this.grdlevel4.TotalRow = (InheritableBoolean)1;
		this.grdlevel4.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdlevel4).VisualStyle = (VisualStyle)4;
		this.grdlevel4.SelectionChanged += new System.EventHandler(grdlevel4_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdlevel4).Click += new System.EventHandler(grdlevel4_Click);
		((System.Windows.Forms.Control)(object)this.grdlevel4).DoubleClick += new System.EventHandler(grdlevel4_DoubleClick);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label33);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 534);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(425, 22);
		this.panel18.TabIndex = 86;
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.Teal;
		this.label33.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(6, 3);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(292, 17);
		this.label33.TabIndex = 0;
		this.label33.Text = "4th Level Account's Of Selected 3rd Level";
		this.grdlevel3.AllowColumnDrag = false;
		this.grdlevel3.AllowEdit = (InheritableBoolean)2;
		this.grdlevel3.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdlevel3).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdlevel3.FilterMode = (FilterMode)2;
		this.grdlevel3.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdlevel3).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdlevel3.GroupByBoxVisible = false;
		this.grdlevel3.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdlevel3).Location = new System.Drawing.Point(0, 374);
		((System.Windows.Forms.Control)(object)this.grdlevel3).Name = "grdlevel3";
		((ScrollableBase)this.grdlevel3).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdlevel3).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdlevel3).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdlevel3).Size = new System.Drawing.Size(425, 160);
		((System.Windows.Forms.Control)(object)this.grdlevel3).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdlevel3).TabStop = false;
		this.grdlevel3.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdlevel3).VisualStyle = (VisualStyle)4;
		this.grdlevel3.SelectionChanged += new System.EventHandler(grdlevel3_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdlevel3).DoubleClick += new System.EventHandler(grdlevel3_DoubleClick);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label32);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 352);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(425, 22);
		this.panel14.TabIndex = 85;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.Teal;
		this.label32.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(6, 3);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(295, 17);
		this.label32.TabIndex = 0;
		this.label32.Text = "3rd Level Account's Of Selected 2nd Level";
		this.grdlevel2.AllowCardSizing = false;
		this.grdlevel2.AllowColumnDrag = false;
		this.grdlevel2.AllowEdit = (InheritableBoolean)2;
		this.grdlevel2.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdlevel2).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdlevel2.FilterMode = (FilterMode)2;
		this.grdlevel2.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdlevel2).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdlevel2.GroupByBoxVisible = false;
		this.grdlevel2.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdlevel2).Location = new System.Drawing.Point(0, 192);
		((System.Windows.Forms.Control)(object)this.grdlevel2).Name = "grdlevel2";
		((ScrollableBase)this.grdlevel2).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdlevel2).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdlevel2).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdlevel2).Size = new System.Drawing.Size(425, 160);
		((System.Windows.Forms.Control)(object)this.grdlevel2).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdlevel2).TabStop = false;
		this.grdlevel2.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdlevel2).VisualStyle = (VisualStyle)4;
		this.grdlevel2.SelectionChanged += new System.EventHandler(grdlevel2_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdlevel2).DoubleClick += new System.EventHandler(grdlevel2_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label31);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 170);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(425, 22);
		this.panel7.TabIndex = 83;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.Teal;
		this.label31.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(6, 3);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(293, 17);
		this.label31.TabIndex = 0;
		this.label31.Text = "2nd Level Account's Of Selected 1st Level";
		this.grdfrm.AllowCardSizing = false;
		this.grdfrm.AllowColumnDrag = false;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		this.grdfrm.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(425, 145);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdfrm).TabStop = false;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.SelectionChanged += new System.EventHandler(grdfrm_SelectionChanged);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label30);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(425, 25);
		this.panel6.TabIndex = 81;
		this.label30.AutoSize = true;
		this.label30.BackColor = System.Drawing.Color.Teal;
		this.label30.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(6, 4);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(137, 17);
		this.label30.TabIndex = 0;
		this.label30.Text = "1st Level Accounts";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1121, 25);
		this.panel2.TabIndex = 0;
		this.TabHistory.AutoScroll = true;
		this.TabHistory.Controls.Add(this.panel15);
		this.TabHistory.Location = new System.Drawing.Point(4, 4);
		this.TabHistory.Name = "TabHistory";
		this.TabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.TabHistory.Size = new System.Drawing.Size(1127, 721);
		this.TabHistory.TabIndex = 0;
		this.TabHistory.Text = "History";
		this.TabHistory.UseVisualStyleBackColor = true;
		this.panel15.AutoScroll = true;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdFormHistory);
		this.panel15.Controls.Add(this.panel11);
		this.panel15.Controls.Add(this.panel16);
		this.panel15.Controls.Add(this.toolStrip2);
		this.panel15.Controls.Add(this.label24);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(3, 3);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1121, 715);
		this.panel15.TabIndex = 0;
		this.grdFormHistory.AllowCardSizing = false;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdFormHistory.FilterMode = (FilterMode)2;
		this.grdFormHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFormHistory.GroupTotals = (GroupTotals)2;
		this.grdFormHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Location = new System.Drawing.Point(0, 136);
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Name = "grdFormHistory";
		((ScrollableBase)this.grdFormHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdFormHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdFormHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Size = new System.Drawing.Size(1121, 579);
		((System.Windows.Forms.Control)(object)this.grdFormHistory).TabIndex = 65;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).TabStop = false;
		this.grdFormHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFormHistory).VisualStyle = (VisualStyle)4;
		this.grdFormHistory.CellValueChanged += new ColumnActionEventHandler(grdFormHistory_CellValueChanged);
		this.grdFormHistory.ColumnButtonClick += new ColumnActionEventHandler(grdFormHistory_ColumnButtonClick);
		this.grdFormHistory.SelectionChanged += new System.EventHandler(grdFormHistory_SelectionChanged);
		this.panel11.Controls.Add(this.groupBox6);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroupHistory);
		this.panel11.Controls.Add(this.label26);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccount);
		this.panel11.Controls.Add(this.label34);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 56);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1121, 80);
		this.panel11.TabIndex = 69;
		this.groupBox6.Controls.Add(this.groupBox7);
		this.groupBox6.Controls.Add(this.rdGroupeExpand);
		this.groupBox6.Controls.Add(this.rdGroupCollapsed);
		this.groupBox6.Controls.Add(this.groupBox8);
		this.groupBox6.Controls.Add(this.btnshow);
		this.groupBox6.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(0, 0);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(349, 80);
		this.groupBox6.TabIndex = 0;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Filters";
		this.groupBox7.Controls.Add(this.rd5thLevel);
		this.groupBox7.Controls.Add(this.rd4thLevel);
		this.groupBox7.Controls.Add(this.rd3rdLevel);
		this.groupBox7.Controls.Add(this.rd2ndLevel);
		this.groupBox7.Controls.Add(this.rd1stLevel);
		this.groupBox7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox7.Location = new System.Drawing.Point(2, 10);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(164, 67);
		this.groupBox7.TabIndex = 111;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Upto_Level";
		this.rd5thLevel.AutoSize = true;
		this.rd5thLevel.Location = new System.Drawing.Point(89, 32);
		this.rd5thLevel.Name = "rd5thLevel";
		this.rd5thLevel.Size = new System.Drawing.Size(73, 19);
		this.rd5thLevel.TabIndex = 4;
		this.rd5thLevel.Text = "5th Level";
		this.rd5thLevel.UseVisualStyleBackColor = true;
		this.rd4thLevel.AutoSize = true;
		this.rd4thLevel.Checked = true;
		this.rd4thLevel.Location = new System.Drawing.Point(89, 14);
		this.rd4thLevel.Name = "rd4thLevel";
		this.rd4thLevel.Size = new System.Drawing.Size(73, 19);
		this.rd4thLevel.TabIndex = 3;
		this.rd4thLevel.TabStop = true;
		this.rd4thLevel.Text = "4th Level";
		this.rd4thLevel.UseVisualStyleBackColor = true;
		this.rd3rdLevel.AutoSize = true;
		this.rd3rdLevel.Location = new System.Drawing.Point(7, 46);
		this.rd3rdLevel.Name = "rd3rdLevel";
		this.rd3rdLevel.Size = new System.Drawing.Size(73, 19);
		this.rd3rdLevel.TabIndex = 2;
		this.rd3rdLevel.Text = "3rd Level";
		this.rd3rdLevel.UseVisualStyleBackColor = true;
		this.rd2ndLevel.AutoSize = true;
		this.rd2ndLevel.Location = new System.Drawing.Point(7, 30);
		this.rd2ndLevel.Name = "rd2ndLevel";
		this.rd2ndLevel.Size = new System.Drawing.Size(76, 19);
		this.rd2ndLevel.TabIndex = 1;
		this.rd2ndLevel.Text = "2nd Level";
		this.rd2ndLevel.UseVisualStyleBackColor = true;
		this.rd1stLevel.AutoSize = true;
		this.rd1stLevel.Location = new System.Drawing.Point(7, 14);
		this.rd1stLevel.Name = "rd1stLevel";
		this.rd1stLevel.Size = new System.Drawing.Size(69, 19);
		this.rd1stLevel.TabIndex = 0;
		this.rd1stLevel.Text = "1st Level";
		this.rd1stLevel.UseVisualStyleBackColor = true;
		this.rdGroupeExpand.AutoSize = true;
		this.rdGroupeExpand.Checked = true;
		this.rdGroupeExpand.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupeExpand.Location = new System.Drawing.Point(263, 29);
		this.rdGroupeExpand.Name = "rdGroupeExpand";
		this.rdGroupeExpand.Size = new System.Drawing.Size(71, 21);
		this.rdGroupeExpand.TabIndex = 118;
		this.rdGroupeExpand.TabStop = true;
		this.rdGroupeExpand.Text = "Expand";
		this.rdGroupeExpand.UseVisualStyleBackColor = true;
		this.rdGroupCollapsed.AutoSize = true;
		this.rdGroupCollapsed.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupCollapsed.Location = new System.Drawing.Point(263, 12);
		this.rdGroupCollapsed.Name = "rdGroupCollapsed";
		this.rdGroupCollapsed.Size = new System.Drawing.Size(76, 21);
		this.rdGroupCollapsed.TabIndex = 117;
		this.rdGroupCollapsed.Text = "Collapse";
		this.rdGroupCollapsed.UseVisualStyleBackColor = true;
		this.groupBox8.Controls.Add(this.RadActiveOnly);
		this.groupBox8.Controls.Add(this.RadAll);
		this.groupBox8.Controls.Add(this.RadInActive);
		this.groupBox8.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox8.Location = new System.Drawing.Point(169, 10);
		this.groupBox8.Name = "groupBox8";
		this.groupBox8.Size = new System.Drawing.Size(88, 67);
		this.groupBox8.TabIndex = 113;
		this.groupBox8.TabStop = false;
		this.groupBox8.Text = "Status";
		this.RadActiveOnly.AutoSize = true;
		this.RadActiveOnly.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.RadActiveOnly.Location = new System.Drawing.Point(6, 29);
		this.RadActiveOnly.Name = "RadActiveOnly";
		this.RadActiveOnly.Size = new System.Drawing.Size(66, 17);
		this.RadActiveOnly.TabIndex = 8;
		this.RadActiveOnly.Text = "Active";
		this.RadActiveOnly.UseVisualStyleBackColor = true;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(6, 13);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(42, 17);
		this.RadAll.TabIndex = 7;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadInActive.AutoSize = true;
		this.RadInActive.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.RadInActive.Location = new System.Drawing.Point(6, 44);
		this.RadInActive.Name = "RadInActive";
		this.RadInActive.Size = new System.Drawing.Size(80, 17);
		this.RadInActive.TabIndex = 9;
		this.RadInActive.Text = "InActive";
		this.RadInActive.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(262, 50);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(63, 26);
		this.btnshow.TabIndex = 114;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbCustomGroupHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroupHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroupHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroupHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroupHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHistory).Location = new System.Drawing.Point(445, 126);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHistory).Name = "CmbCustomGroupHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHistory).TabIndex = 112;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(362, 104);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(78, 15);
		this.label26.TabIndex = 116;
		this.label26.Text = "Account Title";
		this.CmbAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Location = new System.Drawing.Point(445, 98);
		((System.Windows.Forms.Control)(object)this.CmbAccount).Name = "CmbAccount";
		((System.Windows.Forms.Control)(object)this.CmbAccount).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccount).TabIndex = 110;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(362, 132);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(84, 15);
		this.label34.TabIndex = 115;
		this.label34.Text = "Custom Group";
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label7);
		this.panel16.Controls.Add(this.ctrlGrdBar2);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 27);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1121, 29);
		this.panel16.TabIndex = 66;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(5, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(213, 19);
		this.label7.TabIndex = 23;
		this.label7.Text = "Chart OF Account History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1079, 4);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdFormHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnUpdateHistory, this.btnUpdateAccountsExcelSheet });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1121, 27);
		this.toolStrip2.TabIndex = 68;
		this.toolStrip2.Text = "toolStrip2";
		this.btnUpdateHistory.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateHistory.Name = "btnUpdateHistory";
		this.btnUpdateHistory.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateHistory.Text = "Update";
		this.btnUpdateHistory.Click += new System.EventHandler(btnUpdateHistory_Click);
		this.btnUpdateAccountsExcelSheet.Image = Architecture.WinApp.Properties.Resources.icons8_excel_48;
		this.btnUpdateAccountsExcelSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateAccountsExcelSheet.Name = "btnUpdateAccountsExcelSheet";
		this.btnUpdateAccountsExcelSheet.Size = new System.Drawing.Size(348, 24);
		this.btnUpdateAccountsExcelSheet.Text = "Update Chart Of Accounts Through Excel Sheet";
		this.btnUpdateAccountsExcelSheet.Click += new System.EventHandler(btnUpdateAccountsExcelSheet_Click);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(91, 114);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(213, 19);
		this.label24.TabIndex = 0;
		this.label24.Text = "Chart OF Account History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1116, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(46, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1135, 747);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "AcfrmDefCoa";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AcfrmDefCoa_FormClosing);
		base.Load += new System.EventHandler(AcfrmDefCoa_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmDefCoa_KeyDown);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdParentCode).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyId).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbsnote).EndInit();
		this.DetailFilterGroup.ResumeLayout(false);
		this.DetailFilterGroup.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypeFilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomgroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgroupdetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentac).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbactype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbplnote).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.TabCOA.ResumeLayout(false);
		this.TabForm.ResumeLayout(false);
		this.ParentPanel.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdlevel4).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel3).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel2).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.TabHistory.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFormHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		this.groupBox8.ResumeLayout(false);
		this.groupBox8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroupHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}