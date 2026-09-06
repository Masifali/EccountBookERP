// // using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
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
namespace Architecture.WinApp.Account_Reports
{
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
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
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;

public class CoaTitleChange : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dt = new DataTable();

	private DataTable dtType = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ContextMenuStrip contextMenuStrip1;

	internal UltraCombo cmbCoaTitle;

	internal UltraCombo cmbLanguage;

	private Button button1;

	private Label label3;

	private Label label2;

	private Label label1;

	private TextBox txtOtherLanguage;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private Panel panel9;

	private Label label4;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Label label5;

	private GroupBox groupBox2;

	internal UltraCombo CmbCustomGroup;

	private Label label7;

	internal UltraCombo CmbAccount;

	private Label label6;

	private RadioButton RadInActive;

	private RadioButton RadActiveOnly;

	private RadioButton RadAll;

	private GroupBox groupBox1;

	internal GridEX grd;

	private RadioButton rdGroupeExpand;

	private RadioButton rdGroupCollapsed;

	private GroupBox groupBox3;

	private RadioButton rd5thLevel;

	private RadioButton rd4thLevel;

	private RadioButton rd3rdLevel;

	private RadioButton rd2ndLevel;

	private RadioButton rd1stLevel;

	public CoaTitleChange(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ChartOfAccount_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			MultiLanguagesGetAll();
			AccountTitleFill();
			AccountFill();
			CustomeGroupsDefine();
			rdGroupeExpand.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiLanguagesGetAll()
	{
		try
		{
			int Id = Conversion.ToInt(cmbLanguage.Value);
			DataTable dt = new DataTable();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbLanguage, "Id", "LanguageDescription", "LanguageName", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbLanguage.Value = Id;
					}
					else
					{
						((Control)(object)cmbLanguage).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbLanguage).DataSource = null;
				((Control)(object)cmbLanguage).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbCoaTitle.Value);
			DataTable dt = new DataTable();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCoaTitle, "Id", "AccountTitle", "AccountTitle", true);
				DDL.BindDDLNew(dt, CmbAccount, "Id", "AccountTitle", "AccountTitle", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbCoaTitle.Value = Id;
					}
					else
					{
						((Control)(object)cmbCoaTitle).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbCoaTitle).DataSource = null;
				((Control)(object)cmbCoaTitle).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int Id2 = Conversion.ToInt(CmbAccount.Value);
			DataTable dt = ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtAcc = new DataTable();
				dtAcc.Columns.Add("Id", typeof(int));
				dtAcc.Columns.Add("AccountTitle");
				dtAcc.Columns.Add("Account_Level");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtAcc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["Account_Level"]);
				}
				DDL.BindDDL(dtAcc, CmbAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
				if (Id2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
					{
						CmbAccount.Value = Id2;
					}
					else
					{
						((Control)(object)CmbAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbAccount).DataSource = null;
				((Control)(object)CmbAccount).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomeGroupsDefine()
	{
		try
		{
			dtType = new DataTable();
			if (dtType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtType, CmbCustomGroup, "Id", "AcLookUpsDescription", "Custom Group", false);
				UltraGridColumn obj = ((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbCustomGroup.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbCustomGroup.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbCustomGroup.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbCustomGroup.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbCustomGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			wf.Show();
			dt.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AccountId = Conversion.ToInt(CmbAccount.Value);
			string CustomGroupIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbCustomGroup).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbCustomGroup).Text) + ",").Split(',');
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
				wf.Close();
				throw new Exception("Please Check the Ac Levels First");
			}
			obj.AccountLevels = string.Join(",", levels);
			dt = VoucherReports.ChartofAccount(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dataTable = new DataTable();
				dataTable.Columns.Add("Id", typeof(int));
				dataTable.Columns.Add("ParentAccountId", typeof(int));
				dataTable.Columns.Add("AccountTitle", typeof(string));
				dataTable.Columns.Add("AccountCode", typeof(string));
				dataTable.Columns.Add("AccountGroup", typeof(string));
				dataTable.Columns.Add("Account_Level", typeof(int));
				dataTable.Columns.Add("Status", typeof(string));
				dataTable.Columns.Add("AccountClass", typeof(string));
				dataTable.Columns.Add("AccountType", typeof(string));
				dataTable.Columns.Add("NoteTitle", typeof(string));
				dataTable.Columns.Add("LanguageId", typeof(int));
				dataTable.Columns.Add("LanguageName", typeof(string));
				dataTable.Columns.Add("AccountTitleOtherLingo", typeof(string));
				dataTable.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					object Value = ((Conversion.ToInt(dt.Rows[i]["ParentCodeId"]) == 0) ? DBNull.Value : dt.Rows[i]["ParentCodeId"]);
					dataTable.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Value, Conversion.ToString(dt.Rows[i]["AccountTitle"]), Conversion.ToString(dt.Rows[i]["AccountCode"]), Conversion.ToString(dt.Rows[i]["AccountGroup"]), Conversion.ToInt(dt.Rows[i]["Account_Level"]), Conversion.ToString(dt.Rows[i]["IsActive"]), Conversion.ToString(dt.Rows[i]["AccountClass"]), Conversion.ToString(dt.Rows[i]["AccountType"]), Conversion.ToString(dt.Rows[i]["NoteTitle"]), Conversion.ToInt(dt.Rows[i]["LanguageId"]), Conversion.ToString(dt.Rows[i]["LanguageName"]), Conversion.ToString(dt.Rows[i]["AccountTitleOtherLingo"]));
				}
				grd.DataSource = dataTable;
				grd.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void GridSettings()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.GridLines = (GridLines)1;
			grd.HideSelection = (HideSelection)2;
			grd.AllowEdit = (InheritableBoolean)2;
			grd.Hierarchical = true;
			grd.RootTable.HierarchicalMode = (HierarchicalMode)1;
			grd.RootTable.SelfReferencingSettings.ParentDataMember = "Id";
			grd.RootTable.SelfReferencingSettings.ChildDataMember = "ParentAccountId";
			grd.RootTable.SelfReferencingSettings.ExpandColumn = grd.RootTable.Columns["AccountTitle"];
			grd.RootTable.SelfReferencingSettings.AggregationMode = (SelfReferencingAggregationMode)1;
			if (rdGroupCollapsed.Checked)
			{
				grd.CollapseRecords();
			}
			else
			{
				grd.ExpandRecords();
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ParentAccountId"].Visible = false;
			grd.RootTable.Columns["LanguageId"].Visible = false;
			grd.RootTable.Columns["AccountCode"].Width = 80;
			grd.RootTable.Columns["AccountTitle"].Width = 180;
			grd.RootTable.Columns["AccountGroup"].Width = 70;
			grd.RootTable.Columns["Account_Level"].Width = 65;
			grd.RootTable.Columns["Status"].Width = 55;
			grd.RootTable.Columns["AccountClass"].Width = 90;
			grd.RootTable.Columns["AccountType"].Width = 130;
			grd.RootTable.Columns["NoteTitle"].Width = 100;
			grd.RootTable.Columns["LanguageName"].Width = 110;
			grd.RootTable.Columns["AccountTitleOtherLingo"].Width = 160;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				cmbCoaTitle.Value = Conversion.ToInt(r.Cells["Id"].Value);
				cmbLanguage.Value = Conversion.ToInt(r.Cells["LanguageId"].Value);
				txtOtherLanguage.Text = Conversion.ToString(r.Cells["AccountTitleOtherLingo"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbCoaTitle).ActiveRow == null)
			{
				cmbCoaTitle.Focus();
				throw new Exception("AccountTitle Field Required");
			}
			if (((UltraGridBase)cmbLanguage).ActiveRow == null)
			{
				cmbLanguage.Focus();
				throw new Exception("Language Field Required");
			}
			if (txtOtherLanguage.Text == string.Empty || txtOtherLanguage.Text == "0")
			{
				txtOtherLanguage.Focus();
				throw new Exception("Other Language AccountTitle Field Required");
			}
			ChartOfAccountMultiLingo val = new ChartOfAccountMultiLingo
			{
				ChartOfAccountMultiLingoList = new List<ChartOfAccountMultiLingo>()
			};
			ChartOfAccountMultiLingo obj = new ChartOfAccountMultiLingo();
			obj.ChartOfAccountId = Conversion.ToInt(cmbCoaTitle.Value);
			obj.MultiLanguagesId = Conversion.ToInt(cmbLanguage.Value);
			obj.AccountTitle = txtOtherLanguage.Text;
			val.ChartOfAccountMultiLingoList.Add(obj);
			global::Architecture.BLL.Accounts.ChartOfAccountMultiLingo.SaveList(val);
			MessageBox.Show("Record Update Successfully...;");
			reset();
			btnshow_Click(null, null);
			cmbCoaTitle.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowReport()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
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
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		try
		{
			ShowReport();
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
			FileStream fs = null;
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ChartOfAccount_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click_1(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					CmbCustomGroup.Focus();
				}
				else
				{
					cmbCoaTitle.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					CmbCustomGroup.Focus();
				}
				else
				{
					cmbCoaTitle.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl + P", "For print");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grd_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			((Control)(object)cmbCoaTitle).Text = string.Empty;
			txtOtherLanguage.Text = string.Empty;
			MultiLanguagesGetAll();
			AccountTitleFill();
			CmbCustomGroup.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CoaTitleChange_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
			wf.Show();
			MultiLanguagesGetAll();
			AccountTitleFill();
			CustomeGroupsDefine();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void rdGroupCollapsed_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rdGroupeExpand.Checked)
			{
				grd.ExpandRecords();
			}
			else
			{
				grd.CollapseRecords();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCoaTitle_Leave(object sender, EventArgs e)
	{
		try
		{
			OtherLanguagegetByAccountIdAndLanguageId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherLanguagegetByAccountIdAndLanguageId()
	{
		try
		{
			int LanguageId = Conversion.ToInt(cmbLanguage.Value);
			int AccountId = Conversion.ToInt(cmbCoaTitle.Value);
			EnumerableRowCollection<DataRow> query = from x in dt.AsEnumerable()
				where x.Field<int?>("LanguageId") == LanguageId && x.Field<int?>("Id") == AccountId
				select x;
			DataTable filteredDt = (query.Any() ? query.CopyToDataTable() : null);
			if (filteredDt != null && filteredDt.Rows.Count > 0)
			{
				txtOtherLanguage.Text = Conversion.ToString(filteredDt.Rows[0]["AccountTitleOtherLingo"]);
			}
			else
			{
				txtOtherLanguage.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLanguage_Leave(object sender, EventArgs e)
	{
		try
		{
			OtherLanguagegetByAccountIdAndLanguageId();
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
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.CoaTitleChange));
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
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.txtOtherLanguage = new System.Windows.Forms.TextBox();
		this.cmbCoaTitle = new UltraCombo();
		this.cmbLanguage = new UltraCombo();
		this.button1 = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.label5 = new System.Windows.Forms.Label();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rd5thLevel = new System.Windows.Forms.RadioButton();
		this.rd4thLevel = new System.Windows.Forms.RadioButton();
		this.rd3rdLevel = new System.Windows.Forms.RadioButton();
		this.rd2ndLevel = new System.Windows.Forms.RadioButton();
		this.rd1stLevel = new System.Windows.Forms.RadioButton();
		this.rdGroupeExpand = new System.Windows.Forms.RadioButton();
		this.rdGroupCollapsed = new System.Windows.Forms.RadioButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadActiveOnly = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.RadInActive = new System.Windows.Forms.RadioButton();
		this.CmbCustomGroup = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbAccount = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.grd = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCoaTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLanguage).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).BeginInit();
		this.tabPage2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar1);
		this.panel9.Controls.Add(this.label4);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 158);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(884, 27);
		this.panel9.TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(2, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(142, 19);
		this.label4.TabIndex = 1;
		this.label4.Text = "Filtered Records";
		this.txtOtherLanguage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherLanguage.Location = new System.Drawing.Point(377, 39);
		this.txtOtherLanguage.Name = "txtOtherLanguage";
		this.txtOtherLanguage.Size = new System.Drawing.Size(410, 23);
		this.txtOtherLanguage.TabIndex = 2;
		this.cmbCoaTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbCoaTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCoaTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCoaTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCoaTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbCoaTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCoaTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCoaTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCoaTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCoaTitle).Location = new System.Drawing.Point(3, 37);
		((System.Windows.Forms.Control)(object)this.cmbCoaTitle).Name = "cmbCoaTitle";
		((System.Windows.Forms.Control)(object)this.cmbCoaTitle).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbCoaTitle).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbCoaTitle).Leave += new System.EventHandler(cmbCoaTitle_Leave);
		this.cmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.cmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLanguage).Location = new System.Drawing.Point(239, 37);
		((System.Windows.Forms.Control)(object)this.cmbLanguage).Name = "cmbLanguage";
		((System.Windows.Forms.Control)(object)this.cmbLanguage).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.cmbLanguage).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbLanguage).Leave += new System.EventHandler(cmbLanguage_Leave);
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button1.Location = new System.Drawing.Point(792, 39);
		this.button1.Margin = new System.Windows.Forms.Padding(2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(67, 23);
		this.button1.TabIndex = 3;
		this.button1.Text = "Update";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(239, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 15);
		this.label3.TabIndex = 107;
		this.label3.Text = "Language";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 107;
		this.label2.Text = "COA Title";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(377, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(115, 15);
		this.label1.TabIndex = 107;
		this.label1.Text = "OtherLangauge Title";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(573, 41);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(63, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(884, 58);
		this.panel2.TabIndex = 3;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.print, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(884, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = (System.Drawing.Image)resources.GetObject("print.Image");
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "114-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 32);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(203, 19);
		this.label5.TabIndex = 1;
		this.label5.Text = "ChartOf Account Report";
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 58);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(884, 100);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.groupBox3);
		this.tabPage1.Controls.Add(this.rdGroupeExpand);
		this.tabPage1.Controls.Add(this.rdGroupCollapsed);
		this.tabPage1.Controls.Add(this.groupBox1);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.tabPage1.Controls.Add(this.label7);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccount);
		this.tabPage1.Controls.Add(this.label6);
		this.tabPage1.Controls.Add(this.btnshow);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(876, 71);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Report Filters";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.groupBox3.Controls.Add(this.rd5thLevel);
		this.groupBox3.Controls.Add(this.rd4thLevel);
		this.groupBox3.Controls.Add(this.rd3rdLevel);
		this.groupBox3.Controls.Add(this.rd2ndLevel);
		this.groupBox3.Controls.Add(this.rd1stLevel);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(313, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(164, 67);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Upto_Level";
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
		this.rdGroupeExpand.Location = new System.Drawing.Point(574, 20);
		this.rdGroupeExpand.Name = "rdGroupeExpand";
		this.rdGroupeExpand.Size = new System.Drawing.Size(71, 21);
		this.rdGroupeExpand.TabIndex = 109;
		this.rdGroupeExpand.TabStop = true;
		this.rdGroupeExpand.Text = "Expand";
		this.rdGroupeExpand.UseVisualStyleBackColor = true;
		this.rdGroupCollapsed.AutoSize = true;
		this.rdGroupCollapsed.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupCollapsed.Location = new System.Drawing.Point(574, 3);
		this.rdGroupCollapsed.Name = "rdGroupCollapsed";
		this.rdGroupCollapsed.Size = new System.Drawing.Size(76, 21);
		this.rdGroupCollapsed.TabIndex = 108;
		this.rdGroupCollapsed.Text = "Collapse";
		this.rdGroupCollapsed.UseVisualStyleBackColor = true;
		this.rdGroupCollapsed.CheckedChanged += new System.EventHandler(rdGroupCollapsed_CheckedChanged);
		this.groupBox1.Controls.Add(this.RadActiveOnly);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add(this.RadInActive);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(480, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(88, 67);
		this.groupBox1.TabIndex = 2;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Status";
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
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(89, 37);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 43);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(84, 15);
		this.label7.TabIndex = 106;
		this.label7.Text = "Custom Group";
		this.CmbAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Location = new System.Drawing.Point(89, 9);
		((System.Windows.Forms.Control)(object)this.CmbAccount).Name = "CmbAccount";
		((System.Windows.Forms.Control)(object)this.CmbAccount).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccount).TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 15);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 15);
		this.label6.TabIndex = 106;
		this.label6.Text = "Account Title";
		this.tabPage2.Controls.Add(this.groupBox2);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(876, 71);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Update Title";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.txtOtherLanguage);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCoaTitle);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLanguage);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.button1);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(870, 68);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Other Language Title";
		this.grd.AlternatingColors = true;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HeaderFormatStyle.LineAlignment = (TextAlignment)2;
		this.grd.HeaderFormatStyle.TextAlignment = (TextAlignment)2;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 185);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(884, 330);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 6;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(843, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(884, 515);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel9);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel2);
		this.DoubleBuffered = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "CoaTitleChange";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CoaTitleChange_FormClosing);
		base.Load += new System.EventHandler(ChartOfAccount_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ChartOfAccount_KeyDown);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCoaTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLanguage).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).EndInit();
		this.tabPage2.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}

}