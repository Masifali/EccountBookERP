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
using Architecture.Model.Common;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using ChartofAccount = global::Architecture.WinApp.Common.ChartofAccountBLL;
using AccountsReconciliationHeader = global::Architecture.Model.Accounts.AccountsReconciliationHeader;
using SupCustBankDetail = global::Architecture.Model.Inventory.SupCustBankDetail;
using Reporting = global::Architecture.Common.Reporting;
using MultiCurrencyBLL = global::Architecture.BLL.MultiCurrency;
using VoucherHead = Architecture.BLL.Accounts.VoucherHead;
using ProjectsBLL = global::Architecture.BLL.Projects;
using DMSAttachmentsBLL = global::Architecture.BLL.DMSAttachments;
using ImInvoiceBLL = global::Architecture.BLL.Import.ImInvoice;
using ExImFCBankPaymenstsBLL = global::Architecture.BLL.Import.ExImFCBankPaymensts;
using FcYBankChargesTypes = System.Object;
using SpecialSreenRightsUserWise = System.Object;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Reports
{

public class TrialBalance : Form
{
	public static DateTime FromDate;

	public static DateTime ToDate;

	public static int GroupId;

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	public bool RequestFromJournalVoucher;

	private List<string> SortingList = new List<string>();

	private DataTable dtTrial = new DataTable();

	private DataTable dtDocType = new DataTable();

	private DataTable dtTrialSelected = new DataTable();

	public DataTable dtForLoad = new DataTable();

	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private SelectedTrialBalance selectedTrialForm;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel4;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label33;

	private Label label27;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel6;

	internal GridEX grd;

	private CheckBox SkipZero;

	private Button btnshow;

	private CheckBox ChkBoxPost;

	private ToolStripButton btnTrialBalanceNew;

	private TextBox txtClosingDebit;

	private Label label2;

	private Label label1;

	private TextBox txtClosingCredit;

	private ToolStripButton btnTrialBalance135;

	private TabControl tabControl1;

	private TabPage tabTrialBalance;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Panel panel3;

	private ToolStripDropDownButton tsDrowdownTrial;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btntrialMultisubPrint;

	internal UltraCombo cmbDocumentTypeIds;

	private Label label3;

	private Panel panel7;

	private Label label6;

	private GroupBox groupBox1;

	private RadioButton rdSixColumns;

	private RadioButton rdFourColumns;

	private CheckBox chkGroupOnParent;

	internal UltraCombo CmbBranches;

	private Label Branches;

	private GroupBox groupBox2;

	private CheckBox ChkCGSGroupAccountShow;

	private CheckBox ChkSaleGroupAccountShow;

	private CheckBox chkStockGroupAccountShow;

	private ToolStripDropDownButton tsDropDownLevelWise;

	internal UltraCombo CmbCustomeGroups;

	private Label label4;

	private Label label5;

	public UltraCombo CmbGroupAccount;

	private CheckBox chkSkipCGSAc;

	private ToolStripButton btnLoadForVouchers;

	private Label lblSortingCriteria;

	private CheckBox chkClosingSorting;

	private RadioButton rdDescending;

	private RadioButton rdAscending;

	private Button btnApplySorting;

	private CheckBox chkAccountTitleSorting;

	private CheckBox chkAccountCodeSorting;

	private TabPage tabTrialbalanceselected;

	private Panel PanelOtherForms;

	public TrialBalance(UserAccount user)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VouchertypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			global::Architecture.Model.Accounts.VoucherHead obj = new global::Architecture.Model.Accounts.VoucherHead();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtDocType = global::Architecture.BLL.Accounts.VoucherHead.GetDocumentTypesFromVouchers(obj);
			if (dtDocType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDocType, cmbDocumentTypeIds, "Id", "DocumentTypeDescription", "Document Type", false);
				UltraGridColumn obj2 = ((UltraGridBase)cmbDocumentTypeIds).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbDocumentTypeIds.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbDocumentTypeIds.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbDocumentTypeIds.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbDocumentTypeIds.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbDocumentTypeIds).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TrialBalance_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			rdFourColumns.Checked = true;
			VouchertypeFill();
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				Branches.Visible = false;
				((Control)(object)CmbBranches).Visible = false;
			}
			else
			{
				BranchesFill();
			}
			MultiLanguagesGetAll();
			GroupAccountFill();
			CustomGroupsDefine();
			fromdate.Focus();
			SkipZero.Checked = true;
			ChkBoxPost.Checked = true;
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			todate.Value = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.End_Period)) ? DateTime.Now : ((Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.End_Period) < DateTime.Now) ? Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.End_Period) : DateTime.Now));
			if (RequestFromJournalVoucher)
			{
				rdSixColumns.Checked = true;
				btnLoadForVouchers.Visible = true;
			}
			ReportsLoad();
			ReportsLoadForLevelWise();
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

	private void ReportsLoadForLevelWise()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("Trial Balance Level Wise");
			if (files != null && files.Length != 0)
			{
				ToolStripMenuItem[] items = files.Select((string f) => new ToolStripMenuItem(Path.GetFileNameWithoutExtension(f))).ToArray();
				ToolStripItemCollection dropDownItems = tsDropDownLevelWise.DropDownItems;
				ToolStripItem[] toolStripItems = items;
				dropDownItems.AddRange(toolStripItems);
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Error loading reports.", ex);
		}
	}

	private void reset()
	{
		SkipZero.Checked = true;
		fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
	}

	private void BranchesFill()
	{
		try
		{
			DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, "", 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBranches, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				if (BranchFeature && BranchFeatureConsolidated)
				{
					UltraGridColumn obj = ((UltraGridBase)CmbBranches).DisplayLayout.Bands[0].Columns.Add();
					((KeyedSubObjectBase)obj).Key = "Selected";
					((HeaderBase)obj.Header).Caption = string.Empty;
					obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
					obj.DataType = typeof(bool);
					((HeaderBase)obj.Header).VisiblePosition = 0;
					CmbBranches.LimitToList = false;
					CmbBranches.CheckedListSettings.CheckStateMember = "Selected";
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ListSeparator = ",";
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				}
				((Control)(object)CmbBranches).Text = UserAccount.BranchName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GroupAccountFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
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
				DDL.BindDDL(dtAcc, CmbGroupAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomGroupsDefine()
	{
		try
		{
			DataTable dtCustom = CommonServices.CustomeGroupsDefine(1);
			if (dtCustom.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustom, CmbCustomeGroups, "Id", "AcLookUpsDescription", "Custom Group", false);
				((UltraGridBase)CmbCustomeGroups).DisplayLayout.Bands[0].Columns[1].Width = 336;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			wf.Show();
			string BranchIds = "";
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.FromDate = fromdate.Value;
			vh.ToDate = todate.Value;
			vh.ZeroBalanceType = Conversion.ToInt((object)SkipZero.Checked);
			vh.Debit = Conversion.ToDouble((object)txtClosingDebit.Text.Trim());
			vh.Credit = Conversion.ToDouble((object)txtClosingCredit.Text.Trim());
			if (!ChkBoxPost.Checked)
			{
				vh.IsApproved = true;
			}
			else
			{
				vh.ApprovedFilter = "All";
			}
			vh.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			if (Conversion.ToString((object)((Control)(object)cmbDocumentTypeIds).Text) != string.Empty)
			{
				vh.DocumentTypeIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbDocumentTypeIds).Text, dtDocType, "DocumentTypeDescription");
			}
			if (BranchFeature && BranchFeatureConsolidated)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				BranchIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)CmbBranches).Text, dtBranch, "BranchName");
			}
			else if (BranchFeature)
			{
				if (((UltraGridBase)CmbBranches).ActiveRow == null || Conversion.ToInt(CmbBranches.Value) == 0)
				{
					CmbBranches.Focus();
					wf.Close();
					throw new Exception("Please Select Branch first!");
				}
				BranchIds = Conversion.ToString(CmbBranches.Value);
			}
			vh.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			List<string> idsList = new List<string>();
			if (chkStockGroupAccountShow.Checked)
			{
				idsList.Add("4");
			}
			if (ChkSaleGroupAccountShow.Checked)
			{
				idsList.Add("10");
			}
			if (ChkCGSGroupAccountShow.Checked)
			{
				idsList.Add("12");
			}
			string Ids = (idsList.Any() ? string.Join(",", idsList) : "");
			vh.AccountTypeIds = Ids;
			vh.CustomGroupId = Conversion.ToInt(CmbCustomeGroups.Value);
			vh.GroupAccountId = Conversion.ToInt(CmbGroupAccount.Value);
			vh.SkipCgsAccounts = Conversion.ToInt((object)chkSkipCGSAc.Checked);
			dtTrial = VoucherReports.TrialBalanceReport(vh);
			if (dtTrial.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtTrial, clsGlobalVariables.dtCompLogo);
				string fixedSort = "ParentAccountTitle ASC";
				string dynamicSort = string.Join(", ", SortingList.Select((string col) => col + (rdAscending.Checked ? " ASC" : " DESC")));
				string sortExpression = (string.IsNullOrWhiteSpace(dynamicSort) ? fixedSort : (fixedSort + ", " + dynamicSort));
				lblSortingCriteria.Text = "(" + sortExpression + ")";
				DataView view = dtTrial.DefaultView;
				view.Sort = sortExpression;
				dtTrial = view.ToTable();
				DataTable dt = new DataTable();
				dt.BeginLoadData();
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("SubsidiaryAccountId", typeof(int));
				dt.Columns.Add("BranchesId", typeof(int));
				dt.Columns.Add("BranchName", typeof(string));
				dt.Columns.Add("ParentCode", typeof(string));
				dt.Columns.Add("ParentTitle", typeof(string));
				dt.Columns.Add("AccountCode", typeof(string));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("AccountType", typeof(string));
				dt.Columns.Add("AccountGroup", typeof(string));
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("OpeningDr", typeof(double));
				dt.Columns.Add("OpeningCr", typeof(double));
				dt.Columns.Add("Debit", typeof(double));
				dt.Columns.Add("Credit", typeof(double));
				dt.Columns.Add("Closing", typeof(double));
				dt.Columns.Add("ClosingDr", typeof(double));
				dt.Columns.Add("ClosingCr", typeof(double));
				foreach (DataRow row in dtTrial.Rows)
				{
					dt.Rows.Add(row["AccountId"], row["SubsidiaryAccoutId"], row["BranchesId"], row["BranchName"], row["ParentAccount"], row["ParentAccountTitle"], row["AccountCode"], row["AccountTitle"], row["AccountType"], row["AccountGroup"], row["Opening"], row["OpeningDr"], row["OpeningCr"], row["Debit"], row["Credit"], row["Closing"], row["ClosingDr"], row["ClosingCr"]);
				}
				dt.EndLoadData();
				grd.DataSource = dt;
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
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["BranchesId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grd.RootTable.Columns["ParentCode"].Visible = false;
			grd.RootTable.Columns["AccountGroup"].Visible = false;
			if (rdFourColumns.Checked)
			{
				grd.RootTable.Columns["OpeningDr"].Visible = false;
				grd.RootTable.Columns["OpeningCr"].Visible = false;
				grd.RootTable.Columns["ClosingDr"].Visible = false;
				grd.RootTable.Columns["ClosingCr"].Visible = false;
				grd.RootTable.Columns["Opening"].Visible = true;
				grd.RootTable.Columns["Closing"].Visible = true;
				grd.RootTable.Columns["Credit"].Visible = true;
				grd.RootTable.Columns["Debit"].Visible = true;
			}
			else
			{
				grd.RootTable.Columns["Closing"].Visible = false;
				grd.RootTable.Columns["Opening"].Visible = false;
				grd.RootTable.Columns["OpeningDr"].Visible = true;
				grd.RootTable.Columns["OpeningCr"].Visible = true;
				grd.RootTable.Columns["ClosingDr"].Visible = true;
				grd.RootTable.Columns["ClosingCr"].Visible = true;
				if (RequestFromJournalVoucher)
				{
					grd.RootTable.Columns["OpeningDr"].Visible = false;
					grd.RootTable.Columns["OpeningCr"].Visible = false;
					grd.RootTable.Columns["Debit"].Visible = false;
					grd.RootTable.Columns["Credit"].Visible = false;
					grd.RootTable.Columns["ClosingDr"].Visible = true;
					grd.RootTable.Columns["ClosingCr"].Visible = true;
				}
			}
			if (chkGroupOnParent.Checked)
			{
				grd.RootTable.Columns["ParentTitle"].Visible = false;
				grd.RootTable.Groups.Add("ParentTitle");
			}
			else
			{
				grd.RootTable.Columns["ParentTitle"].Visible = true;
			}
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["ParentCode"].Width = 100;
			grd.RootTable.Columns["ParentTitle"].Width = 185;
			grd.RootTable.Columns["AccountCode"].Width = 100;
			grd.RootTable.Columns["AccountTitle"].Width = 250;
			grd.RootTable.Columns["AccountType"].Width = 100;
			grd.RootTable.Columns["Opening"].Width = 120;
			grd.RootTable.Columns["OpeningDr"].Width = 120;
			grd.RootTable.Columns["OpeningCr"].Width = 120;
			grd.RootTable.Columns["Debit"].Width = 120;
			grd.RootTable.Columns["Credit"].Width = 120;
			grd.RootTable.Columns["Closing"].Width = 120;
			grd.RootTable.Columns["ClosingDr"].Width = 120;
			grd.RootTable.Columns["ClosingCr"].Width = 120;
			grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["OpeningDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OpeningDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OpeningDr"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["OpeningDr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["OpeningCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OpeningCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OpeningCr"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["OpeningCr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClosingDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClosingDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClosingDr"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClosingDr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClosingCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClosingCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClosingCr"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClosingCr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			if (RequestFromJournalVoucher)
			{
				grd.RootTable.Columns.Add("Select");
				grd.RootTable.Columns["Select"].ActAsSelector = true;
				grd.RootTable.Columns["Select"].UseHeaderSelector = true;
				grd.RootTable.Columns["Select"].Width = 30;
				grd.RootTable.Columns["Select"].Position = 0;
			}
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("TrialBalance");
			if (files != null && files.Length != 0)
			{
				ToolStripMenuItem[] items = files.Select((string f) => new ToolStripMenuItem(Path.GetFileNameWithoutExtension(f))).ToArray();
				ToolStripItemCollection dropDownItems = tsDrowdownTrial.DropDownItems;
				ToolStripItem[] toolStripItems = items;
				dropDownItems.AddRange(toolStripItems);
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Error loading reports.", ex);
		}
	}

	private void TrialBalance_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
		}
		if (tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.L)
		{
			lblLanguage.Visible = true;
			((Control)(object)CmbLanguage).Visible = true;
			((Control)(object)CmbCustomeGroups).Size = new Size(159, 26);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "AccountCode")
			{
				string AccountGroup = Conversion.ToString(item.Cells["AccountGroup"].Value);
				int AccountId = Conversion.ToInt(item.Cells["AccountId"].Value);
				int SubsidiaryAccountId = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
				if (AccountGroup == "Detail")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AccountId, fromdate.Value, todate.Value, SubsidiaryAccountId, branchesId);
					return;
				}
				selectedTrialForm.cmbAccountTitle.Value = AccountId;
				selectedTrialForm.btnShowSelectedTrial_Click(null, null);
				tabControl1.SelectedIndex = 1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTrialBalanceNew_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherReports.TrialBalanceNewReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = fromdate.Value,
				ToDate = todate.Value,
				ZeroBalanceType = 1
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = null;
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "131-AccountsTrialBalance.rpt");
				return;
			}
			throw new Exception("Record not found for Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTrialBalance135_Click(object sender, EventArgs e)
	{
	}

	private void tsDrowdownTrial_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtTrial.Rows.Count > 0 && dtTrial != null)
			{
				Reporting val = null;
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtTrial, e.ClickedItem.Text + ".rpt");
				return;
			}
			throw new Exception("Record not found for Display");
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
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbLanguage, "Id", "LanguageDescription", "LanguageName", true);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedTab.Name == "tabTrialBalance")
		{
			PanelOtherForms.Visible = false;
			tabControl1.Dock = DockStyle.Fill;
		}
		else if (tabControl1.SelectedTab.Name == "tabTrialbalanceselected")
		{
			PanelOtherForms.Visible = true;
			tabControl1.Dock = DockStyle.Top;
			tabControl1.Height = 27;
			FormHelper.OpenFormInPanel("SelectedTrialBalance", new SelectedTrialBalance(UserAccount), PanelOtherForms);
		}
	}

	private void btnRefreshSelectedTrial_Click(object sender, EventArgs e)
	{
	}

	private void btnReportGroupWise_Click(object sender, EventArgs e)
	{
	}

	private void btn131LevelWise_Click(object sender, EventArgs e)
	{
	}

	private void btntrialMultisubPrint_Click(object sender, EventArgs e)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtTrial != null && dtTrial.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtTrial.AsEnumerable()
					where r.Field<double>("ClosingDr") > 0.0
					select r;
				DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dtTrial.Clone());
				EnumerableRowCollection<DataRow> rowscr = from r in dtTrial.AsEnumerable()
					where r.Field<double>("ClosingCr") > 0.0
					select r;
				DataTable obj = (rowscr.Any() ? rowscr.CopyToDataTable() : dtTrial.Clone());
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject DayBookSubReport_DebitTransactions = new SubReportObject(dtDebit, "TrialBalanceDebitSubReport.rpt");
				SubReportObject DayBookSubReport_CreditTransactions = new SubReportObject(obj, "TrialBalanceCreditSubReport.rpt");
				lstSubReports.Add(DayBookSubReport_DebitTransactions);
				lstSubReports.Add(DayBookSubReport_CreditTransactions);
				Reporting val = null;
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dtTrial, "111A-AcRptTrialBalances.rpt", lstSubReports);
				return;
			}
			throw new Exception("Record not found for Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TrialBalance_FormClosing(object sender, FormClosingEventArgs e)
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

	private void rdFourColumns_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			if (rdFourColumns.Checked)
			{
				grd.RootTable.Columns["ClosingDr"].Visible = false;
				grd.RootTable.Columns["ClosingCr"].Visible = false;
				grd.RootTable.Columns["OpeningDr"].Visible = false;
				grd.RootTable.Columns["OpeningCr"].Visible = false;
				grd.RootTable.Columns["Opening"].Visible = true;
				grd.RootTable.Columns["Closing"].Visible = true;
				return;
			}
			grd.RootTable.Columns["ClosingDr"].Visible = true;
			grd.RootTable.Columns["ClosingCr"].Visible = true;
			grd.RootTable.Columns["OpeningDr"].Visible = true;
			grd.RootTable.Columns["OpeningCr"].Visible = true;
			grd.RootTable.Columns["Opening"].Visible = false;
			grd.RootTable.Columns["Closing"].Visible = false;
			if (RequestFromJournalVoucher)
			{
				grd.RootTable.Columns["OpeningDr"].Visible = false;
				grd.RootTable.Columns["OpeningCr"].Visible = false;
				grd.RootTable.Columns["CurrDebit"].Visible = false;
				grd.RootTable.Columns["CurrCredit"].Visible = false;
				grd.RootTable.Columns["ClosingDr"].Visible = true;
				grd.RootTable.Columns["ClosingCr"].Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtClosingDebit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtClosingCredit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void tsDropDownLevelWise_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtTrial.Rows.Count > 0 && dtTrial != null)
			{
				Reporting val = null;
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtTrial, e.ClickedItem.Text + ".rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadForVouchers_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetCheckedRows().Length != 0)
			{
				dtForLoad = dtTrial.Clone();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					DataRow dataRow = dtTrial.Rows[r.RowIndex];
					dtForLoad.Rows.Add(dataRow.ItemArray);
					Hide();
				}
				return;
			}
			throw new Exception("No rows Selected For Load");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateSortingList()
	{
		try
		{
			if (chkAccountCodeSorting.Checked)
			{
				if (!SortingList.Contains("AccountCode"))
				{
					SortingList.Add("AccountCode");
				}
			}
			else
			{
				SortingList.Remove("AccountCode");
			}
			if (chkAccountTitleSorting.Checked)
			{
				if (!SortingList.Contains("AccountTitle"))
				{
					SortingList.Add("AccountTitle");
				}
			}
			else
			{
				SortingList.Remove("AccountTitle");
			}
			if (chkClosingSorting.Checked)
			{
				if (!SortingList.Contains("Closing"))
				{
					SortingList.Add("Closing");
				}
			}
			else
			{
				SortingList.Remove("Closing");
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Error updating sorting list: " + ex.Message);
		}
	}

	private void chkAccountCodeSorting_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSortingList();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkAccountTitleSorting_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSortingList();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkClosingSorting_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateSortingList();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApplySorting_Click(object sender, EventArgs e)
	{
		try
		{
			if (dtTrial.Rows.Count <= 0)
			{
				return;
			}
			string fixedSort = "ParentAccountTitle ASC";
			string dynamicSort = string.Join(", ", SortingList.Select((string col) => col + (rdAscending.Checked ? " ASC" : " DESC")));
			string sortExpression = (string.IsNullOrWhiteSpace(dynamicSort) ? fixedSort : (fixedSort + ", " + dynamicSort));
			lblSortingCriteria.Text = "(" + sortExpression + ")";
			DataView view = dtTrial.DefaultView;
			view.Sort = sortExpression;
			dtTrial = view.ToTable();
			if (dtTrial.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.BeginLoadData();
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("SubsidiaryAccountId", typeof(int));
				dt.Columns.Add("BranchesId", typeof(int));
				dt.Columns.Add("BranchName", typeof(string));
				dt.Columns.Add("ParentCode", typeof(string));
				dt.Columns.Add("ParentTitle", typeof(string));
				dt.Columns.Add("AccountCode", typeof(string));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("AccountType", typeof(string));
				dt.Columns.Add("AccountGroup", typeof(string));
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("OpeningDr", typeof(double));
				dt.Columns.Add("OpeningCr", typeof(double));
				dt.Columns.Add("Debit", typeof(double));
				dt.Columns.Add("Credit", typeof(double));
				dt.Columns.Add("Closing", typeof(double));
				dt.Columns.Add("ClosingDr", typeof(double));
				dt.Columns.Add("ClosingCr", typeof(double));
				foreach (DataRow row in dtTrial.Rows)
				{
					dt.Rows.Add(row["AccountId"], row["SubsidiaryAccoutId"], row["BranchesId"], row["BranchName"], row["ParentAccount"], row["ParentAccountTitle"], row["AccountCode"], row["AccountTitle"], row["AccountType"], row["AccountGroup"], row["Opening"], row["OpeningDr"], row["OpeningCr"], row["Debit"], row["Credit"], row["Closing"], row["ClosingDr"], row["ClosingCr"]);
				}
				dt.EndLoadData();
				grd.DataSource = dt;
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
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0922: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Expected O, but got Unknown
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.TrialBalance));
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabTrialBalance = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.lblSortingCriteria = new System.Windows.Forms.Label();
		this.chkClosingSorting = new System.Windows.Forms.CheckBox();
		this.rdDescending = new System.Windows.Forms.RadioButton();
		this.rdAscending = new System.Windows.Forms.RadioButton();
		this.btnApplySorting = new System.Windows.Forms.Button();
		this.chkAccountTitleSorting = new System.Windows.Forms.CheckBox();
		this.chkAccountCodeSorting = new System.Windows.Forms.CheckBox();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.ChkCGSGroupAccountShow = new System.Windows.Forms.CheckBox();
		this.ChkSaleGroupAccountShow = new System.Windows.Forms.CheckBox();
		this.chkStockGroupAccountShow = new System.Windows.Forms.CheckBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.chkSkipCGSAc = new System.Windows.Forms.CheckBox();
		this.CmbCustomeGroups = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbBranches = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.CmbLanguage = new UltraCombo();
		this.CmbGroupAccount = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.rdFourColumns = new System.Windows.Forms.RadioButton();
		this.rdSixColumns = new System.Windows.Forms.RadioButton();
		this.chkGroupOnParent = new System.Windows.Forms.CheckBox();
		this.txtClosingDebit = new System.Windows.Forms.TextBox();
		this.cmbDocumentTypeIds = new UltraCombo();
		this.txtClosingCredit = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.SkipZero = new System.Windows.Forms.CheckBox();
		this.label33 = new System.Windows.Forms.Label();
		this.ChkBoxPost = new System.Windows.Forms.CheckBox();
		this.label27 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.tsDrowdownTrial = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnTrialBalanceNew = new System.Windows.Forms.ToolStripButton();
		this.btnTrialBalance135 = new System.Windows.Forms.ToolStripButton();
		this.btntrialMultisubPrint = new System.Windows.Forms.ToolStripButton();
		this.tsDropDownLevelWise = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnLoadForVouchers = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.PanelOtherForms = new System.Windows.Forms.Panel();
		this.tabTrialbalanceselected = new System.Windows.Forms.TabPage();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabTrialBalance.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomeGroups).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroupAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentTypeIds).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1027, 632);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Controls.Add(this.PanelOtherForms);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1027, 632);
		this.panel6.TabIndex = 108;
		this.tabControl1.Controls.Add(this.tabTrialBalance);
		this.tabControl1.Controls.Add(this.tabTrialbalanceselected);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1027, 593);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabTrialBalance.Controls.Add(this.panel5);
		this.tabTrialBalance.Controls.Add(this.panel3);
		this.tabTrialBalance.Controls.Add(this.panel2);
		this.tabTrialBalance.Location = new System.Drawing.Point(4, 25);
		this.tabTrialBalance.Name = "tabTrialBalance";
		this.tabTrialBalance.Padding = new System.Windows.Forms.Padding(3);
		this.tabTrialBalance.Size = new System.Drawing.Size(1019, 564);
		this.tabTrialBalance.TabIndex = 0;
		this.tabTrialBalance.Text = "Trial Balance";
		this.tabTrialBalance.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 218);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1013, 343);
		this.panel5.TabIndex = 111;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1013, 315);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.lblSortingCriteria);
		this.panel7.Controls.Add(this.chkClosingSorting);
		this.panel7.Controls.Add(this.rdDescending);
		this.panel7.Controls.Add(this.rdAscending);
		this.panel7.Controls.Add(this.btnApplySorting);
		this.panel7.Controls.Add(this.chkAccountTitleSorting);
		this.panel7.Controls.Add(this.chkAccountCodeSorting);
		this.panel7.Controls.Add(this.ctrlGrdBar3);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1013, 28);
		this.panel7.TabIndex = 111;
		this.lblSortingCriteria.AutoSize = true;
		this.lblSortingCriteria.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSortingCriteria.ForeColor = System.Drawing.Color.White;
		this.lblSortingCriteria.Location = new System.Drawing.Point(671, 7);
		this.lblSortingCriteria.Name = "lblSortingCriteria";
		this.lblSortingCriteria.Size = new System.Drawing.Size(86, 15);
		this.lblSortingCriteria.TabIndex = 191;
		this.lblSortingCriteria.Text = "SortingCritera";
		this.chkClosingSorting.AutoSize = true;
		this.chkClosingSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkClosingSorting.ForeColor = System.Drawing.Color.White;
		this.chkClosingSorting.Location = new System.Drawing.Point(333, 5);
		this.chkClosingSorting.Name = "chkClosingSorting";
		this.chkClosingSorting.Size = new System.Drawing.Size(65, 19);
		this.chkClosingSorting.TabIndex = 190;
		this.chkClosingSorting.Text = "Closing";
		this.chkClosingSorting.UseVisualStyleBackColor = true;
		this.chkClosingSorting.CheckedChanged += new System.EventHandler(chkClosingSorting_CheckedChanged);
		this.rdDescending.AutoSize = true;
		this.rdDescending.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDescending.ForeColor = System.Drawing.Color.White;
		this.rdDescending.Location = new System.Drawing.Point(580, 5);
		this.rdDescending.Name = "rdDescending";
		this.rdDescending.Size = new System.Drawing.Size(90, 19);
		this.rdDescending.TabIndex = 189;
		this.rdDescending.Text = "Descending";
		this.rdDescending.UseVisualStyleBackColor = true;
		this.rdAscending.AutoSize = true;
		this.rdAscending.Checked = true;
		this.rdAscending.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAscending.ForeColor = System.Drawing.Color.White;
		this.rdAscending.Location = new System.Drawing.Point(496, 5);
		this.rdAscending.Name = "rdAscending";
		this.rdAscending.Size = new System.Drawing.Size(82, 19);
		this.rdAscending.TabIndex = 188;
		this.rdAscending.TabStop = true;
		this.rdAscending.Text = "Ascending";
		this.rdAscending.UseVisualStyleBackColor = true;
		this.btnApplySorting.BackColor = System.Drawing.Color.White;
		this.btnApplySorting.FlatAppearance.BorderSize = 0;
		this.btnApplySorting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApplySorting.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApplySorting.ForeColor = System.Drawing.Color.Teal;
		this.btnApplySorting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApplySorting.Location = new System.Drawing.Point(399, 3);
		this.btnApplySorting.Margin = new System.Windows.Forms.Padding(2);
		this.btnApplySorting.Name = "btnApplySorting";
		this.btnApplySorting.Size = new System.Drawing.Size(93, 23);
		this.btnApplySorting.TabIndex = 187;
		this.btnApplySorting.Text = "Apply Sorting";
		this.btnApplySorting.UseVisualStyleBackColor = false;
		this.btnApplySorting.Click += new System.EventHandler(btnApplySorting_Click);
		this.chkAccountTitleSorting.AutoSize = true;
		this.chkAccountTitleSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAccountTitleSorting.ForeColor = System.Drawing.Color.White;
		this.chkAccountTitleSorting.Location = new System.Drawing.Point(233, 5);
		this.chkAccountTitleSorting.Name = "chkAccountTitleSorting";
		this.chkAccountTitleSorting.Size = new System.Drawing.Size(100, 19);
		this.chkAccountTitleSorting.TabIndex = 186;
		this.chkAccountTitleSorting.Text = "Account Title";
		this.chkAccountTitleSorting.UseVisualStyleBackColor = true;
		this.chkAccountTitleSorting.CheckedChanged += new System.EventHandler(chkAccountTitleSorting_CheckedChanged);
		this.chkAccountCodeSorting.AutoSize = true;
		this.chkAccountCodeSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAccountCodeSorting.ForeColor = System.Drawing.Color.White;
		this.chkAccountCodeSorting.Location = new System.Drawing.Point(128, 5);
		this.chkAccountCodeSorting.Name = "chkAccountCodeSorting";
		this.chkAccountCodeSorting.Size = new System.Drawing.Size(103, 19);
		this.chkAccountCodeSorting.TabIndex = 185;
		this.chkAccountCodeSorting.Text = "Account Code";
		this.chkAccountCodeSorting.UseVisualStyleBackColor = true;
		this.chkAccountCodeSorting.CheckedChanged += new System.EventHandler(chkAccountCodeSorting_CheckedChanged);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(972, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 1;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(4, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(116, 19);
		this.label6.TabIndex = 2;
		this.label6.Text = "Trial Balance";
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 33);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1013, 185);
		this.panel3.TabIndex = 0;
		this.groupBox2.Controls.Add(this.ChkCGSGroupAccountShow);
		this.groupBox2.Controls.Add(this.ChkSaleGroupAccountShow);
		this.groupBox2.Controls.Add(this.chkStockGroupAccountShow);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(592, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(83, 179);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Group Accounts Show";
		this.ChkCGSGroupAccountShow.AutoSize = true;
		this.ChkCGSGroupAccountShow.Checked = true;
		this.ChkCGSGroupAccountShow.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkCGSGroupAccountShow.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkCGSGroupAccountShow.Location = new System.Drawing.Point(8, 99);
		this.ChkCGSGroupAccountShow.Margin = new System.Windows.Forms.Padding(2);
		this.ChkCGSGroupAccountShow.Name = "ChkCGSGroupAccountShow";
		this.ChkCGSGroupAccountShow.Size = new System.Drawing.Size(48, 19);
		this.ChkCGSGroupAccountShow.TabIndex = 3;
		this.ChkCGSGroupAccountShow.Text = "CGS";
		this.ChkCGSGroupAccountShow.UseVisualStyleBackColor = true;
		this.ChkSaleGroupAccountShow.AutoSize = true;
		this.ChkSaleGroupAccountShow.Checked = true;
		this.ChkSaleGroupAccountShow.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkSaleGroupAccountShow.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSaleGroupAccountShow.Location = new System.Drawing.Point(8, 77);
		this.ChkSaleGroupAccountShow.Margin = new System.Windows.Forms.Padding(2);
		this.ChkSaleGroupAccountShow.Name = "ChkSaleGroupAccountShow";
		this.ChkSaleGroupAccountShow.Size = new System.Drawing.Size(48, 19);
		this.ChkSaleGroupAccountShow.TabIndex = 2;
		this.ChkSaleGroupAccountShow.Text = "Sale";
		this.ChkSaleGroupAccountShow.UseVisualStyleBackColor = true;
		this.chkStockGroupAccountShow.AutoSize = true;
		this.chkStockGroupAccountShow.Checked = true;
		this.chkStockGroupAccountShow.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkStockGroupAccountShow.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkStockGroupAccountShow.Location = new System.Drawing.Point(8, 56);
		this.chkStockGroupAccountShow.Margin = new System.Windows.Forms.Padding(2);
		this.chkStockGroupAccountShow.Name = "chkStockGroupAccountShow";
		this.chkStockGroupAccountShow.Size = new System.Drawing.Size(56, 19);
		this.chkStockGroupAccountShow.TabIndex = 1;
		this.chkStockGroupAccountShow.Text = "Stock";
		this.chkStockGroupAccountShow.UseVisualStyleBackColor = true;
		this.groupBox1.Controls.Add(this.chkSkipCGSAc);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomeGroups);
		this.groupBox1.Controls.Add(this.Branches);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.lblLanguage);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGroupAccount);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.rdFourColumns);
		this.groupBox1.Controls.Add(this.rdSixColumns);
		this.groupBox1.Controls.Add(this.chkGroupOnParent);
		this.groupBox1.Controls.Add(this.txtClosingDebit);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds);
		this.groupBox1.Controls.Add(this.txtClosingCredit);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.SkipZero);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.ChkBoxPost);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.todate);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(585, 179);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.chkSkipCGSAc.AutoSize = true;
		this.chkSkipCGSAc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipCGSAc.Location = new System.Drawing.Point(441, 71);
		this.chkSkipCGSAc.Margin = new System.Windows.Forms.Padding(2);
		this.chkSkipCGSAc.Name = "chkSkipCGSAc";
		this.chkSkipCGSAc.Size = new System.Drawing.Size(127, 19);
		this.chkSkipCGSAc.TabIndex = 11;
		this.chkSkipCGSAc.Text = "Skip CGS Accounts";
		this.chkSkipCGSAc.UseVisualStyleBackColor = true;
		this.CmbCustomeGroups.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomeGroups.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomeGroups.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomeGroups.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Location = new System.Drawing.Point(108, 120);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Name = "CmbCustomeGroups";
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Size = new System.Drawing.Size(326, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).TabIndex = 5;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.Branches.Location = new System.Drawing.Point(6, 153);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 117;
		this.Branches.Text = "Branches";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 99);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(88, 15);
		this.label4.TabIndex = 150;
		this.label4.Text = "Group Account";
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(108, 147);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(260, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 126);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(89, 15);
		this.label5.TabIndex = 151;
		this.label5.Text = "Custom Groups";
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblLanguage.Location = new System.Drawing.Point(269, 126);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(59, 15);
		this.lblLanguage.TabIndex = 110;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(332, 120);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(102, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.CmbGroupAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbGroupAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGroupAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGroupAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGroupAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbGroupAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGroupAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).Location = new System.Drawing.Point(108, 93);
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).Name = "CmbGroupAccount";
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).Size = new System.Drawing.Size(326, 26);
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).TabIndex = 4;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(370, 147);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(64, 26);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.rdFourColumns.AutoSize = true;
		this.rdFourColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdFourColumns.Location = new System.Drawing.Point(441, 111);
		this.rdFourColumns.Name = "rdFourColumns";
		this.rdFourColumns.Size = new System.Drawing.Size(99, 19);
		this.rdFourColumns.TabIndex = 13;
		this.rdFourColumns.TabStop = true;
		this.rdFourColumns.Text = "Four Columns";
		this.rdFourColumns.UseVisualStyleBackColor = true;
		this.rdFourColumns.CheckedChanged += new System.EventHandler(rdFourColumns_CheckedChanged);
		this.rdSixColumns.AutoSize = true;
		this.rdSixColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSixColumns.Location = new System.Drawing.Point(441, 130);
		this.rdSixColumns.Name = "rdSixColumns";
		this.rdSixColumns.Size = new System.Drawing.Size(91, 19);
		this.rdSixColumns.TabIndex = 14;
		this.rdSixColumns.TabStop = true;
		this.rdSixColumns.Text = "Six Columns";
		this.rdSixColumns.UseVisualStyleBackColor = true;
		this.chkGroupOnParent.AutoSize = true;
		this.chkGroupOnParent.Checked = true;
		this.chkGroupOnParent.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkGroupOnParent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkGroupOnParent.Location = new System.Drawing.Point(441, 92);
		this.chkGroupOnParent.Margin = new System.Windows.Forms.Padding(2);
		this.chkGroupOnParent.Name = "chkGroupOnParent";
		this.chkGroupOnParent.Size = new System.Drawing.Size(132, 19);
		this.chkGroupOnParent.TabIndex = 12;
		this.chkGroupOnParent.Text = "Group On Parent Ac";
		this.chkGroupOnParent.UseVisualStyleBackColor = true;
		this.txtClosingDebit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtClosingDebit.Location = new System.Drawing.Point(332, 16);
		this.txtClosingDebit.Name = "txtClosingDebit";
		this.txtClosingDebit.Size = new System.Drawing.Size(102, 23);
		this.txtClosingDebit.TabIndex = 6;
		this.txtClosingDebit.Text = "0";
		this.txtClosingDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtClosingDebit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtClosingDebit_KeyPress);
		this.cmbDocumentTypeIds.AllowNull = (DefaultableBoolean)1;
		this.cmbDocumentTypeIds.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocumentTypeIds.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocumentTypeIds.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).Location = new System.Drawing.Point(108, 66);
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).Name = "cmbDocumentTypeIds";
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).Size = new System.Drawing.Size(326, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).TabIndex = 2;
		this.txtClosingCredit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtClosingCredit.Location = new System.Drawing.Point(332, 41);
		this.txtClosingCredit.Name = "txtClosingCredit";
		this.txtClosingCredit.Size = new System.Drawing.Size(102, 23);
		this.txtClosingCredit.TabIndex = 7;
		this.txtClosingCredit.Text = "0";
		this.txtClosingCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtClosingCredit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtClosingCredit_KeyPress);
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 64);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(89, 30);
		this.label3.TabIndex = 112;
		this.label3.Text = "Documents Not To Include";
		this.SkipZero.AutoSize = true;
		this.SkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SkipZero.Location = new System.Drawing.Point(441, 51);
		this.SkipZero.Margin = new System.Windows.Forms.Padding(2);
		this.SkipZero.Name = "SkipZero";
		this.SkipZero.Size = new System.Drawing.Size(76, 19);
		this.SkipZero.TabIndex = 10;
		this.SkipZero.Text = "Skip Zero";
		this.SkipZero.UseVisualStyleBackColor = true;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(6, 45);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.ChkBoxPost.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPost.Location = new System.Drawing.Point(441, 18);
		this.ChkBoxPost.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxPost.Name = "ChkBoxPost";
		this.ChkBoxPost.Size = new System.Drawing.Size(132, 35);
		this.ChkBoxPost.TabIndex = 9;
		this.ChkBoxPost.Text = "Include UnPost Voucher";
		this.ChkBoxPost.UseVisualStyleBackColor = true;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 20);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(273, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(51, 15);
		this.label1.TabIndex = 108;
		this.label1.Text = "Cl_Debit";
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(108, 41);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(159, 23);
		this.todate.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(273, 45);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 15);
		this.label2.TabIndex = 108;
		this.label2.Text = "Cl_Credit";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(108, 16);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(159, 23);
		this.fromdate.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1013, 30);
		this.panel2.TabIndex = 109;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.tsDrowdownTrial, this.btnTrialBalanceNew, this.btnTrialBalance135, this.btntrialMultisubPrint, this.tsDropDownLevelWise, this.btnLoadForVouchers });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1013, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.tsDrowdownTrial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsDrowdownTrial.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDrowdownTrial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDrowdownTrial.Name = "tsDrowdownTrial";
		this.tsDrowdownTrial.Size = new System.Drawing.Size(70, 24);
		this.tsDrowdownTrial.Text = "Print";
		this.tsDrowdownTrial.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDrowdownTrial_DropDownItemClicked);
		this.btnTrialBalanceNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnTrialBalanceNew.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnTrialBalanceNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnTrialBalanceNew.Name = "btnTrialBalanceNew";
		this.btnTrialBalanceNew.Size = new System.Drawing.Size(139, 24);
		this.btnTrialBalanceNew.Text = "131-TrialBalance";
		this.btnTrialBalanceNew.Visible = false;
		this.btnTrialBalanceNew.Click += new System.EventHandler(btnTrialBalanceNew_Click);
		this.btnTrialBalance135.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnTrialBalance135.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnTrialBalance135.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnTrialBalance135.Name = "btnTrialBalance135";
		this.btnTrialBalance135.Size = new System.Drawing.Size(226, 24);
		this.btnTrialBalance135.Text = "135-TrialBalanceUpto5thLevel";
		this.btnTrialBalance135.Visible = false;
		this.btnTrialBalance135.Click += new System.EventHandler(btnTrialBalance135_Click);
		this.btntrialMultisubPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btntrialMultisubPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btntrialMultisubPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btntrialMultisubPrint.Name = "btntrialMultisubPrint";
		this.btntrialMultisubPrint.Size = new System.Drawing.Size(94, 24);
		this.btntrialMultisubPrint.Text = "111A&Print";
		this.btntrialMultisubPrint.Click += new System.EventHandler(btntrialMultisubPrint_Click);
		this.tsDropDownLevelWise.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDownLevelWise.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDownLevelWise.Name = "tsDropDownLevelWise";
		this.tsDropDownLevelWise.Size = new System.Drawing.Size(110, 24);
		this.tsDropDownLevelWise.Text = "Level Wise";
		this.tsDropDownLevelWise.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDownLevelWise_DropDownItemClicked);
		this.btnLoadForVouchers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnLoadForVouchers.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadForVouchers.Image = (System.Drawing.Image)resources.GetObject("btnLoadForVouchers.Image");
		this.btnLoadForVouchers.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadForVouchers.Name = "btnLoadForVouchers";
		this.btnLoadForVouchers.Size = new System.Drawing.Size(138, 24);
		this.btnLoadForVouchers.Text = "Load For Vouchers ";
		this.btnLoadForVouchers.Visible = false;
		this.btnLoadForVouchers.Click += new System.EventHandler(btnLoadForVouchers_Click);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1027, 632);
		this.panel1.TabIndex = 0;
		this.PanelOtherForms.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.PanelOtherForms.Location = new System.Drawing.Point(0, 593);
		this.PanelOtherForms.Name = "PanelOtherForms";
		this.PanelOtherForms.Size = new System.Drawing.Size(1027, 39);
		this.PanelOtherForms.TabIndex = 174;
		this.PanelOtherForms.Visible = false;
		this.tabTrialbalanceselected.Location = new System.Drawing.Point(4, 25);
		this.tabTrialbalanceselected.Name = "tabTrialbalanceselected";
		this.tabTrialbalanceselected.Size = new System.Drawing.Size(1019, 564);
		this.tabTrialbalanceselected.TabIndex = 1;
		this.tabTrialbalanceselected.Text = "Trial Balance Selected";
		this.tabTrialbalanceselected.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1027, 632);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "TrialBalance";
		this.Text = "Trial Balance & Selected Trial Balance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(TrialBalance_FormClosing);
		base.Load += new System.EventHandler(TrialBalance_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(TrialBalance_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabTrialBalance.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomeGroups).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroupAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentTypeIds).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
