using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.FixedAssets;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.FixedAssets;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.AssetSchema
{

public class frmFixedAssetsCategory : Form
{
	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int RecId;

	private DataTable dtAllCoaAccounts = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnPrint;

	private Label label24;

	private Label label8;

	private TextBox txtCategoryCode;

	private Label label1;

	private TextBox txtCategoryDescription;

	private TextBox txtserialTo;

	private Label label3;

	private TextBox txtserialFrom;

	private Label label2;

	private TextBox txtOtherDescription;

	internal UltraCombo cmbAccumolativeACC;

	private Label label7;

	internal UltraCombo cmbDepriciationACC;

	private Label label6;

	internal UltraCombo cmbGlAcc;

	private Label label5;

	internal GridEX DatagridHistory;

	private ToolStripButton BtnRefresh;

	private ToolStripButton BtnShortCutkeys;

	internal UltraCombo CmbExpenseMaintenanceAccount;

	private Label label4;

	public frmFixedAssetsCategory(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmFixedAssetsCategory_Load_1(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(980, 500);
			AllAccountsWithTypeIdDbCall();
			GlAndAccomolativeAccountBind();
			DepriciationAccountFill();
			ExpenseMaintenanceAccountFill();
			BindHistory();
			txtCategoryCode.Focus();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllAccountsWithTypeIdDbCall()
	{
		try
		{
			dtAllCoaAccounts.Clear();
			dtAllCoaAccounts = CommonServices.Accounts_GetAccountTitleByAccountTypeIds();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable GetAccountByTypeIdFromAllAccountsWithTypeId(string accountTypeIds)
	{
		try
		{
			int result;
			HashSet<int> accountTypeIdSet = new HashSet<int>((from id in accountTypeIds.Split(',')
				select id.Trim() into id
				where int.TryParse(id, out result)
				select id).Select(int.Parse));
			DataTable dt = new DataTable();
			dt.Columns.AddRange(new DataColumn[5]
			{
				new DataColumn("Id", typeof(int)),
				new DataColumn("AccountTitle", typeof(string)),
				new DataColumn("AccountCode", typeof(string)),
				new DataColumn("ParentAccountTitle", typeof(string)),
				new DataColumn("AccountClass", typeof(string))
			});
			foreach (DataRow dr in dtAllCoaAccounts.Rows)
			{
				int accountTypeId = Conversion.ToInt(dr["AccountTypeId"]);
				if (accountTypeIdSet.Contains(accountTypeId))
				{
					dt.Rows.Add(dr["Id"], dr["AccountTitle"], dr["AccountCode"], dr["ParentAccountTitle"], dr["AccountClass"]);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception("Error while filtering accounts by type ID: " + ex.Message, ex);
		}
	}

	private void DepriciationAccountFill()
	{
		try
		{
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("14");
			InfragisticsHelper.BindAndRetainSelection(cmbDepriciationACC, dt, "Id", "AccountTitle", "Depreciation Account", AllColumns: true, Conversion.ToInt(cmbDepriciationACC.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbDepriciationACC).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GlAndAccomolativeAccountBind()
	{
		try
		{
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("1");
			InfragisticsHelper.BindAndRetainSelection(cmbAccumolativeACC, dt, "Id", "AccountTitle", "Accumulative Account", AllColumns: true, Conversion.ToInt(cmbAccumolativeACC.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbGlAcc, dt, "Id", "AccountTitle", "GL Account", AllColumns: true, Conversion.ToInt(cmbGlAcc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbAccumolativeACC).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbGlAcc).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseMaintenanceAccountFill()
	{
		try
		{
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("23");
			InfragisticsHelper.BindAndRetainSelection(CmbExpenseMaintenanceAccount, dt, "Id", "AccountTitle", "Expense Maintenance Account", AllColumns: true, Conversion.ToInt(CmbExpenseMaintenanceAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbExpenseMaintenanceAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtCategoryCode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Category Code. Field Required");
			txtCategoryCode.Focus();
			return false;
		}
		if (txtCategoryDescription.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Category Description Field Required");
			txtCategoryDescription.Focus();
			return false;
		}
		if (txtserialFrom.Text.Trim() == string.Empty || txtserialFrom.Text.Trim() == "0")
		{
			MessageBox.Show("Serial From Field Required");
			txtserialFrom.Focus();
			return false;
		}
		if (txtserialTo.Text.Trim() == string.Empty || txtserialTo.Text.Trim() == "0")
		{
			MessageBox.Show("Serial To Field Required");
			txtserialTo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbGlAcc).ActiveRow == null || Conversion.ToInt(cmbGlAcc.Value) == 0)
		{
			MessageBox.Show("GL Account Field Required");
			cmbGlAcc.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDepriciationACC).ActiveRow == null || Conversion.ToInt(cmbDepriciationACC.Value) == 0)
		{
			MessageBox.Show("Depreciation Account Field Required");
			cmbDepriciationACC.Focus();
			return false;
		}
		if (((UltraGridBase)cmbAccumolativeACC).ActiveRow == null || Conversion.ToInt(cmbAccumolativeACC.Value) == 0)
		{
			MessageBox.Show("Accumulative Account Field Required");
			cmbAccumolativeACC.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			FixedAssetsCategory fixedAssets = new FixedAssetsCategory();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				fixedAssets.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			fixedAssets.AssetsCategoryCode = txtCategoryCode.Text;
			fixedAssets.AssetsCategoryDesc = txtCategoryDescription.Text;
			fixedAssets.OtherDescription = txtOtherDescription.Text;
			fixedAssets.SerialNoFrom = Conversion.ToInt((object)txtserialFrom.Text);
			fixedAssets.SerialNoTo = Conversion.ToInt((object)txtserialTo.Text);
			fixedAssets.AssetsCateAccountId = Conversion.ToInt(cmbGlAcc.Value);
			fixedAssets.AssetsDeprAccountId = Conversion.ToInt(cmbDepriciationACC.Value);
			fixedAssets.AssetsAcmtDeprAccountId = Conversion.ToInt(cmbAccumolativeACC.Value);
			fixedAssets.ExpenseMaintenanceAccountId = Conversion.ToInt(CmbExpenseMaintenanceAccount.Value);
			FixedAssetsCategory.Save(fixedAssets);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + fixedAssets.AssetsCategoryCode);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + fixedAssets.AssetsCategoryCode);
			}
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			RecId = Id;
			FixedAssetsCategory SC = FixedAssetsCategory.GetById(RecId);
			if (SC != null)
			{
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				UpdateMode = true;
				txtCategoryCode.Text = Conversion.ToString((object)SC.AssetsCategoryCode);
				txtCategoryDescription.Text = Conversion.ToString((object)SC.AssetsCategoryDesc);
				txtOtherDescription.Text = Conversion.ToString((object)SC.OtherDescription);
				txtserialFrom.Text = Conversion.ToString((object)SC.SerialNoFrom);
				txtserialTo.Text = Conversion.ToString((object)SC.SerialNoTo);
				cmbGlAcc.Value = Conversion.ToInt((object)SC.AssetsCateAccountId);
				cmbDepriciationACC.Value = Conversion.ToInt((object)SC.AssetsDeprAccountId);
				cmbAccumolativeACC.Value = Conversion.ToInt((object)SC.AssetsAcmtDeprAccountId);
				CmbExpenseMaintenanceAccount.Value = Conversion.ToInt((object)SC.ExpenseMaintenanceAccount);
			}
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
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FormReset()
	{
		try
		{
			RecId = 0;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			txtCategoryCode.Text = string.Empty;
			txtCategoryDescription.Text = string.Empty;
			txtOtherDescription.Text = string.Empty;
			txtserialFrom.Text = string.Empty;
			txtserialTo.Text = string.Empty;
			((Control)(object)cmbAccumolativeACC).Text = string.Empty;
			((Control)(object)cmbDepriciationACC).Text = string.Empty;
			((Control)(object)cmbGlAcc).Text = string.Empty;
			BindHistory();
			txtCategoryCode.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			AllAccountsWithTypeIdDbCall();
			GlAndAccomolativeAccountBind();
			DepriciationAccountFill();
			ExpenseMaintenanceAccountFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistory()
	{
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable lst = FixedAssetsCategory.GetAll();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CategoryCode");
			dt.Columns.Add("CategoryDescription");
			dt.Columns.Add("OtherDescription");
			dt.Columns.Add("SerialFrom");
			dt.Columns.Add("SerialTo");
			dt.Columns.Add("GLAcc");
			dt.Columns.Add("DepreciationAcc");
			dt.Columns.Add("AccumulativeAcc");
			dt.Columns.Add("ExpenseMaintenanceAccount");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["AssetsCategoryCode"], lst.Rows[i]["AssetsCategoryDesc"], lst.Rows[i]["OtherDescription"], lst.Rows[i]["SerialNoFrom"], lst.Rows[i]["SerialNoTo"], lst.Rows[i]["AssetsCateAccount"], lst.Rows[i]["AssetsDeprAccount"], lst.Rows[i]["AssetsAcmtDeprAccount"], lst.Rows[i]["ExpenseMaintenanceAccount"]);
			}
			DatagridHistory.DataSource = dt;
			DatagridHistory.RetrieveStructure();
			DatagridHistory.AllowEdit = (InheritableBoolean)2;
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DatagridHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			DatagridHistory.RootTable.Columns["CategoryCode"].Width = Constants.InventoryConstants.CategoryCode;
			DatagridHistory.RootTable.Columns["CategoryDescription"].Width = Constants.InventoryConstants.CategoryDescription;
			DatagridHistory.RootTable.Columns["OtherDescription"].Width = Constants.InventoryConstants.OtherDescription;
			DatagridHistory.RootTable.Columns["SerialFrom"].Width = Constants.InventoryConstants.SealNo;
			DatagridHistory.RootTable.Columns["SerialTo"].Width = Constants.InventoryConstants.SealNo;
			DatagridHistory.RootTable.Columns["GLAcc"].Width = Constants.InventoryConstants.AccountTitle;
			DatagridHistory.RootTable.Columns["DepreciationAcc"].Width = Constants.InventoryConstants.AccountTitle;
			DatagridHistory.RootTable.Columns["AccumulativeAcc"].Width = Constants.InventoryConstants.AccountTitle;
			DatagridHistory.RootTable.Columns["ExpenseMaintenanceAccount"].Width = Constants.InventoryConstants.AccountTitle;
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
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmFixedAssetsCategory_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
			{
				btnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				BtnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtCategoryCode.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DatagridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtCategoryCode.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Code");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Code");
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

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DatagridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Control && e.KeyCode == Keys.Return)
				{
					FormReset();
					ReadById(CuurID);
				}
				else if (e.Control && e.KeyCode == Keys.Space && DatagridHistory.CurrentColumn != null && DatagridHistory.CurrentColumn.Key == "Edit")
				{
					FormReset();
					ReadById(CuurID);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmFixedAssetsCategory_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AssetSchema.frmFixedAssetsCategory));
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
		this.DatagridHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtCategoryDescription = new System.Windows.Forms.TextBox();
		this.cmbAccumolativeACC = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbDepriciationACC = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbGlAcc = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtserialTo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtserialFrom = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtOtherDescription = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtCategoryCode = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.CmbExpenseMaintenanceAccount = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccumolativeACC).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepriciationACC).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGlAcc).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseMaintenanceAccount).BeginInit();
		base.SuspendLayout();
		this.DatagridHistory.AllowDelete = (InheritableBoolean)1;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 201);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(800, 460);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 1;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.txtCategoryDescription);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepriciationACC);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGlAcc);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtserialTo);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtserialFrom);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtOtherDescription);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtCategoryCode);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(800, 116);
		this.panel4.TabIndex = 0;
		this.txtCategoryDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCategoryDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCategoryDescription.Location = new System.Drawing.Point(122, 32);
		this.txtCategoryDescription.Name = "txtCategoryDescription";
		this.txtCategoryDescription.Size = new System.Drawing.Size(229, 23);
		this.txtCategoryDescription.TabIndex = 1;
		this.cmbAccumolativeACC.AllowNull = (DefaultableBoolean)1;
		this.cmbAccumolativeACC.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccumolativeACC.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccumolativeACC.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccumolativeACC).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbAccumolativeACC.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).Location = new System.Drawing.Point(489, 57);
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).Name = "cmbAccumolativeACC";
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).Size = new System.Drawing.Size(289, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccumolativeACC).TabIndex = 7;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(354, 63);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(128, 15);
		this.label7.TabIndex = 94;
		this.label7.Text = "Accumolative Account";
		this.cmbDepriciationACC.AllowNull = (DefaultableBoolean)1;
		this.cmbDepriciationACC.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepriciationACC.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepriciationACC.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepriciationACC).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationACC).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDepriciationACC.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationACC).Location = new System.Drawing.Point(489, 30);
		((System.Windows.Forms.Control)(object)this.cmbDepriciationACC).Name = "cmbDepriciationACC";
		((System.Windows.Forms.Control)(object)this.cmbDepriciationACC).Size = new System.Drawing.Size(289, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepriciationACC).TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(354, 36);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(120, 15);
		this.label6.TabIndex = 92;
		this.label6.Text = "Depriciation Account";
		this.cmbGlAcc.AllowNull = (DefaultableBoolean)1;
		this.cmbGlAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGlAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGlAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGlAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbGlAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGlAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGlAcc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbGlAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGlAcc).Location = new System.Drawing.Point(489, 3);
		((System.Windows.Forms.Control)(object)this.cmbGlAcc).Name = "cmbGlAcc";
		((System.Windows.Forms.Control)(object)this.cmbGlAcc).Size = new System.Drawing.Size(289, 26);
		((System.Windows.Forms.Control)(object)this.cmbGlAcc).TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(354, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 90;
		this.label5.Text = "GL Account";
		this.txtserialTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtserialTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtserialTo.Location = new System.Drawing.Point(237, 86);
		this.txtserialTo.Name = "txtserialTo";
		this.txtserialTo.Size = new System.Drawing.Size(114, 23);
		this.txtserialTo.TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 90);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(114, 15);
		this.label3.TabIndex = 86;
		this.label3.Text = "Serial No From && To";
		this.txtserialFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtserialFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtserialFrom.Location = new System.Drawing.Point(122, 86);
		this.txtserialFrom.Name = "txtserialFrom";
		this.txtserialFrom.Size = new System.Drawing.Size(114, 23);
		this.txtserialFrom.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 63);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(101, 15);
		this.label2.TabIndex = 84;
		this.label2.Text = "Other Description";
		this.txtOtherDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherDescription.Location = new System.Drawing.Point(122, 59);
		this.txtOtherDescription.Name = "txtOtherDescription";
		this.txtOtherDescription.Size = new System.Drawing.Size(229, 23);
		this.txtOtherDescription.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 36);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(118, 15);
		this.label1.TabIndex = 82;
		this.label1.Text = "Category Description";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 9);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(84, 15);
		this.label8.TabIndex = 80;
		this.label8.Text = "Category Code";
		this.txtCategoryCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCategoryCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCategoryCode.Location = new System.Drawing.Point(122, 5);
		this.txtCategoryCode.Name = "txtCategoryCode";
		this.txtCategoryCode.Size = new System.Drawing.Size(229, 23);
		this.txtCategoryCode.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 170);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(800, 31);
		this.panel3.TabIndex = 1;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.Teal;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label24.Location = new System.Drawing.Point(3, 30);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(190, 19);
		this.label24.TabIndex = 5;
		this.label24.Text = "Fixed Assets Category";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label24);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(800, 54);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.BtnRefresh, this.btnSave, this.btnUpdate, this.toolStripSeparator2, this.btnPrint, this.BtnShortCutkeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(800, 27);
		this.toolStrip3.TabIndex = 60;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.CmbExpenseMaintenanceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbExpenseMaintenanceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExpenseMaintenanceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExpenseMaintenanceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbExpenseMaintenanceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Location = new System.Drawing.Point(489, 84);
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Name = "CmbExpenseMaintenanceAccount";
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Size = new System.Drawing.Size(289, 26);
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(354, 82);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(122, 30);
		this.label4.TabIndex = 96;
		this.label4.Text = "Expense Maintenance\r\nAccount";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 661);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel2);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmFixedAssetsCategory";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmFixedAssetsCategory_FormClosing);
		base.Load += new System.EventHandler(frmFixedAssetsCategory_Load_1);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFixedAssetsCategory_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccumolativeACC).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepriciationACC).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGlAcc).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseMaintenanceAccount).EndInit();
		base.ResumeLayout(false);
	}
}

}
