// // using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
using UserAccount = global::Architecture.Model.UserAccount;
using VoucherHead = global::Architecture.Model.Accounts.VoucherHead;
using DMSAttachments = global::Architecture.BLL.DMSAttachments;
using tblUserRights = global::Architecture.Model.tblUserRights;
using CheqBookHeader = global::Architecture.BLL.Accounts.CheqBookHeader;
using TaxScheduleMain = global::Architecture.BLL.Inventory.TaxScheduleMain;
using PdcInventory = global::Architecture.Model.Accounts.PdcInventory;
using MultiCurrency = global::Architecture.BLL.MultiCurrency;
using BankBalanceManual = global::Architecture.BLL.BankBalanceManual;
using BranchesAllocationToUser = global::Architecture.BLL.BranchesAllocationToUser;
using SupCustBankDetail = global::Architecture.Model.Inventory.SupCustBankDetail;
using Projects = global::Architecture.BLL.Projects;
using UsersWithCostCenter = global::Architecture.BLL.Inventory.UsersWithCostCenter;
using AccountsPrematurePaymentsReceipts = global::Architecture.BLL.Accounts.AccountsPrematurePaymentsReceipts;
using AccountsReconciliationHeader = global::Architecture.BLL.Accounts.AccountsReconciliationHeader;
using BankReconciliation = global::Architecture.BLL.Accounts.BankReconciliation;
using City = global::Architecture.Model.City;
using Country = global::Architecture.Model.Country;
using ChartofAccount = global::Architecture.BLL.ChartofAccountBLL;
using pdcTransaction = global::Architecture.Model.BankSchema.pdcTransaction;
using LicenseContext = OfficeOpenXml.LicenseContext;
using SortOrder = Janus.Windows.GridEX.SortOrder;
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using SreenRightsUserWise = global::Architecture.Model.SreenRightsUserWise;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;

namespace Architecture.WinApp.Account_Reports
{

public class SelectedTrialBalance : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtTrialSelected = new DataTable();

	public static DateTime FromDate;

	public static DateTime ToDate;

	public static int GroupId;

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private DataTable dtDocType = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel1;

	private Panel panel2;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel6;

	private ToolStrip tsSelectedTrialBalance;

	private ToolStripButton btnRefreshSelectedTrial;

	private ToolStripDropDownButton tsDropDownSelectedTrial;

	private ToolStripButton btnReportGroupWise;

	private CheckBox ChkZkipZeroSelected;

	private Label label6;

	internal UltraCombo CmbCustomeGroups;

	internal UltraCombo CmbLanguageSelected;

	private Label lblLangSelected;

	internal UltraCombo CmbCityName;

	private Label label9;

	private CheckBox ChkBoxPostSelectedTrial;

	private Button btnShowSelectedTrial;

	private DateTimePicker datToDateST;

	private DateTimePicker datFromDateSt;

	private Label label3;

	private Label label4;

	private Label label5;

	internal GridEX grdSelected;

	private ToolStripDropDownButton tsDropDownLevelWise;

	internal UltraCombo cmbDocumentTypeIds;

	private GroupBox groupBox1;

	private Label label1;

	private RadioButton RdSixColumns;

	private RadioButton RdFourColumns;

	internal UltraCombo CmbBranches;

	private Label Branches;

	public UltraCombo cmbAccountTitle;

	private CheckBox chkSkipCgs;

	private TextBox txtClosingDebit;

	private TextBox txtClosingCredit;

	private Label label2;

	private Label label7;

	public SelectedTrialBalance(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void SelectedTrialBalance_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.P)
		{
			btnReportGroupWise_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.L)
		{
			lblLangSelected.Visible = true;
			((Control)(object)CmbLanguageSelected).Visible = true;
		}
	}

	private void ReportsLoad()
	{
		try
		{
			string[] filesSelected = CommonServices.DynamicReportsLoad("SelectedTrialBalance");
			if (filesSelected != null && filesSelected.Length != 0)
			{
				for (int i = 0; i < filesSelected.Length; i++)
				{
					tsDropDownSelectedTrial.DropDownItems.Add(Path.GetFileNameWithoutExtension(filesSelected[i]));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ReportsLoadForLevelWise()
	{
		try
		{
			string[] filesSelected = CommonServices.DynamicReportsLoad("Trial Balance Level Wise");
			if (filesSelected != null && filesSelected.Length != 0)
			{
				for (int i = 0; i < filesSelected.Length; i++)
				{
					tsDropDownLevelWise.DropDownItems.Add(Path.GetFileNameWithoutExtension(filesSelected[i]));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			DataTable dt = global::Architecture.BLL.ChartofAccountBLL.ReadAllAccountgroup(new ReportsParameters
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
				DDL.BindDDL(dtAcc, cmbAccountTitle, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
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
					((UltraGridBase)CmbBranches).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				((Control)(object)CmbBranches).Text = UserAccount.BranchName;
			}
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
				DDL.BindDDLNew(dt, CmbLanguageSelected, "Id", "LanguageDescription", "LanguageName", true);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CityNameFill()
	{
		try
		{
			DataTable dt = CommonServices.CityGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCityName, "Id", "CityName", "City Name", false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
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

	private void CustomeGroupsDefine()
	{
		try
		{
			DataTable dtCustome = CommonServices.CustomeGroupsDefine(1);
			if (dtCustome.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustome, CmbCustomeGroups, "Id", "AcLookUpsDescription", "Custom Group", false);
				((UltraGridBase)CmbCustomeGroups).DisplayLayout.Bands[0].Columns[1].Width = 336;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SelectedTrialBalance_Load(object sender, EventArgs e)
	{
		try
		{
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
			AccountTitleFill();
			CityNameFill();
			CustomeGroupsDefine();
			VouchertypeFill();
			ChkZkipZeroSelected.Checked = true;
			datFromDateSt.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				ChkBoxPostSelectedTrial.Checked = true;
				datFromDateSt.Value = Conversion.ToDateTime((object)FromDate);
				datToDateST.Value = Conversion.ToDateTime((object)ToDate);
				if (GroupId > 0)
				{
					cmbAccountTitle.Value = GroupId;
				}
				btnShowSelectedTrial_Click(null, null);
			}
			cmbAccountTitle.Focus();
			ChkBoxPostSelectedTrial.Checked = true;
			ReportsLoad();
			ReportsLoadForLevelWise();
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
			((Control)(object)CmbCityName).Text = string.Empty;
			((Control)(object)cmbAccountTitle).Text = string.Empty;
			ChkBoxPostSelectedTrial.Checked = true;
			cmbAccountTitle.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void grdSelected_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdSelected.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			int branchesId = Conversion.ToInt(currentRow.Cells["BranchesId"].Value);
			CommonServices.GoToGeneralLedgerFromLinkedEvent(Id, datFromDateSt.Value, datToDateST.Value, 0, branchesId);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void GridSettingsSelected()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdSelected.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (System.Collections.IEnumerable)grdSelected.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdSelected.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdSelected.RootTable.Columns["BranchesId"].Visible = false;
			grdSelected.RootTable.Columns["AccountId"].Visible = false;
			grdSelected.RootTable.Columns["AcLevel"].Visible = false;
			grdSelected.RootTable.Columns["ClassName"].Visible = false;
			grdSelected.RootTable.Groups.Add("ParentTitle");
			grdSelected.HideColumnsWhenGrouped = (InheritableBoolean)1;
			if (RdFourColumns.Checked)
			{
				grdSelected.RootTable.Columns["Opening"].Visible = true;
				grdSelected.RootTable.Columns["Closing"].Visible = true;
				grdSelected.RootTable.Columns["OpeningDr"].Visible = false;
				grdSelected.RootTable.Columns["OpeningCr"].Visible = false;
				grdSelected.RootTable.Columns["ClosingDr"].Visible = false;
				grdSelected.RootTable.Columns["ClosingCr"].Visible = false;
			}
			else
			{
				grdSelected.RootTable.Columns["Opening"].Visible = false;
				grdSelected.RootTable.Columns["Closing"].Visible = false;
				grdSelected.RootTable.Columns["OpeningDr"].Visible = true;
				grdSelected.RootTable.Columns["OpeningCr"].Visible = true;
				grdSelected.RootTable.Columns["ClosingDr"].Visible = true;
				grdSelected.RootTable.Columns["ClosingCr"].Visible = true;
			}
			grdSelected.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdSelected.RootTable.Columns["ClassName"].Width = 100;
			grdSelected.RootTable.Columns["AcLevel"].Width = 55;
			grdSelected.RootTable.Columns["AccountType"].Width = 100;
			grdSelected.RootTable.Columns["AccountCode"].Width = 80;
			grdSelected.RootTable.Columns["AccountTitle"].Width = 160;
			grdSelected.RootTable.Columns["Opening"].Width = 120;
			grdSelected.RootTable.Columns["OpeningDr"].Width = 120;
			grdSelected.RootTable.Columns["OpeningCr"].Width = 120;
			grdSelected.RootTable.Columns["CurrDebit"].Width = 120;
			grdSelected.RootTable.Columns["CurrCredit"].Width = 120;
			grdSelected.RootTable.Columns["Closing"].Width = 120;
			grdSelected.RootTable.Columns["ClosingDr"].Width = 120;
			grdSelected.RootTable.Columns["ClosingCr"].Width = 120;
			grdSelected.RootTable.Columns["CurrDebit"].Caption = "Debit";
			grdSelected.RootTable.Columns["CurrCredit"].Caption = "Credit";
			grdSelected.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grdSelected.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["OpeningDr"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["OpeningDr"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["OpeningDr"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["OpeningDr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["OpeningCr"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["OpeningCr"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["OpeningCr"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["OpeningCr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["CurrDebit"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["CurrDebit"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["CurrDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["CurrDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["CurrCredit"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["CurrCredit"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["CurrCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["CurrCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["ClosingDr"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["ClosingDr"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["ClosingDr"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["ClosingDr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["ClosingCr"].AggregateFunction = (AggregateFunction)2;
			grdSelected.RootTable.Columns["ClosingCr"].TextAlignment = (TextAlignment)3;
			grdSelected.RootTable.Columns["ClosingCr"].FormatString = clsGlobalVariables.stringFormatboth;
			grdSelected.RootTable.Columns["ClosingCr"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSelected).Name);
			if (fs != null)
			{
				grdSelected.LoadLayoutFile((Stream)fs);
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

	private void tsDropDownSelectedTrial_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtTrialSelected.Rows.Count > 0 && dtTrialSelected != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtTrialSelected, e.ClickedItem.Text + ".rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReportGroupWise_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtTrialSelected.Rows.Count > 0 && dtTrialSelected != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtTrialSelected, "113A-AcRptTrialBalanceSelectedGroupAc.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshSelectedTrial_Click(object sender, EventArgs e)
	{
		try
		{
			ResetSelectedTrial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetSelectedTrial()
	{
		try
		{
			ChkZkipZeroSelected.Checked = true;
			((Control)(object)CmbCityName).Text = string.Empty;
			((Control)(object)CmbCustomeGroups).Text = string.Empty;
			((Control)(object)cmbAccountTitle).Text = string.Empty;
			grdSelected.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void btnShowSelectedTrial_Click(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		try
		{
			wf.Show();
			string DocumentTypeIds = "";
			string BranchIds = "";
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.Debit = Conversion.ToDouble((object)txtClosingDebit.Text.Trim());
			vh.Credit = Conversion.ToDouble((object)txtClosingCredit.Text.Trim());
			vh.GroupAccountId = Conversion.ToInt(cmbAccountTitle.Value);
			vh.FromDate = datFromDateSt.Value;
			vh.ToDate = datToDateST.Value;
			vh.CityId = Conversion.ToInt(CmbCityName.Value);
			if (ChkBoxPostSelectedTrial.Checked)
			{
				vh.ApprovedFilter = "All";
			}
			else
			{
				vh.IsApproved = true;
			}
			vh.LanguageId = Conversion.ToInt(CmbLanguageSelected.Value);
			vh.CustomerGroupId = Conversion.ToInt(CmbCustomeGroups.Value);
			if (ChkZkipZeroSelected.Checked)
			{
				vh.ZeroBalanceType = 1;
			}
			if (chkSkipCgs.Checked)
			{
				vh.SkipCgsAccounts = 1;
			}
			if (Conversion.ToString((object)((Control)(object)cmbDocumentTypeIds).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbDocumentTypeIds).Text) + ",").Split(',');
				foreach (string author2 in array)
				{
					DataRow[] dr2 = dtDocType.Select("DocumentTypeDescription='" + Conversion.ToString((object)author2) + "'");
					if (dr2.Length != 0)
					{
						DocumentTypeIds = DocumentTypeIds + "," + Conversion.ToString(dr2[0].ItemArray[0]);
					}
				}
				vh.DocumentTypeIds = DocumentTypeIds;
			}
			if (BranchFeature && BranchFeatureConsolidated)
			{
				if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
				{
					DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
					string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
					foreach (string author in array)
					{
						DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
						if (dr.Length != 0)
						{
							BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
						}
					}
				}
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
			DataSet ds = VoucherReports.SelectedTrialBalanceNew(vh);
			dtTrialSelected = ds.Tables[0].Copy();
			if (dtTrialSelected.Rows.Count > 0)
			{
				if (dtTrialSelected.Columns.Contains("CompLogoImage"))
				{
					dtTrialSelected.Columns["CompLogoImage"].ColumnName = "CompLogoImage2";
				}
				dtTrialSelected.Columns.Add("CompLogoImage", typeof(byte[]));
				foreach (DataRow row2 in dtTrialSelected.Rows)
				{
					row2["CompLogoImage"] = ((ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows[0][0] : "");
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("BranchesId", typeof(int));
				dt.Columns.Add("BranchName");
				dt.Columns.Add("ClassName");
				dt.Columns.Add("AcLevel");
				dt.Columns.Add("ParentTitle");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("AccountTitle");
				dt.Columns.Add("AccountType");
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("OpeningDr", typeof(double));
				dt.Columns.Add("OpeningCr", typeof(double));
				dt.Columns.Add("CurrDebit", typeof(double));
				dt.Columns.Add("CurrCredit", typeof(double));
				dt.Columns.Add("Closing", typeof(double));
				dt.Columns.Add("ClosingDr", typeof(double));
				dt.Columns.Add("ClosingCr", typeof(double));
				dt.BeginLoadData();
				foreach (DataRow row in dtTrialSelected.Rows)
				{
					dt.Rows.Add(row["AccountId"], row["BranchesId"], row["BranchName"], row["ClassName"], row["AcLevel"], row["ParentAccountTitle"], row["AccountCode"], row["AccountTitle"], row["AccountType"], row["Opening"], row["OpeningDr"], row["OpeningCr"], row["CurrDebit"], row["CurrCredit"], row["Closing"], row["ClosingDr"], row["ClosingCr"]);
				}
				dt.EndLoadData();
				grdSelected.DataSource = dt;
				grdSelected.RetrieveStructure();
				GridSettingsSelected();
			}
			else
			{
				grdSelected.ClearStructure();
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

	private void tsDropDownLevelWise_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.FromDate = datFromDateSt.Value;
			vh.ToDate = datToDateST.Value;
			vh.LanguageId = Conversion.ToInt(CmbLanguageSelected.Value);
			if (ChkZkipZeroSelected.Checked)
			{
				vh.ZeroBalanceType = 1;
			}
			DataTable dt = VoucherReports.TrialBalanceAllLevels(vh);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, e.ClickedItem.Text + ".rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SelectedTrialBalance_FormClosing(object sender, FormClosingEventArgs e)
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

	private void RdFourColumns_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtTrialSelected.Rows.Count > 0)
			{
				if (RdFourColumns.Checked)
				{
					grdSelected.RootTable.Columns["Opening"].Visible = true;
					grdSelected.RootTable.Columns["Closing"].Visible = true;
					grdSelected.RootTable.Columns["OpeningDr"].Visible = false;
					grdSelected.RootTable.Columns["OpeningCr"].Visible = false;
					grdSelected.RootTable.Columns["ClosingDr"].Visible = false;
					grdSelected.RootTable.Columns["ClosingCr"].Visible = false;
				}
				else
				{
					grdSelected.RootTable.Columns["Opening"].Visible = false;
					grdSelected.RootTable.Columns["Closing"].Visible = false;
					grdSelected.RootTable.Columns["OpeningDr"].Visible = true;
					grdSelected.RootTable.Columns["OpeningCr"].Visible = true;
					grdSelected.RootTable.Columns["ClosingDr"].Visible = true;
					grdSelected.RootTable.Columns["ClosingCr"].Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtClosingDebit_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void txtClosingCredit_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.SelectedTrialBalance));
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdSelected = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtClosingDebit = new System.Windows.Forms.TextBox();
		this.txtClosingCredit = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.RdSixColumns = new System.Windows.Forms.RadioButton();
		this.label1 = new System.Windows.Forms.Label();
		this.RdFourColumns = new System.Windows.Forms.RadioButton();
		this.CmbCustomeGroups = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbDocumentTypeIds = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.chkSkipCgs = new System.Windows.Forms.CheckBox();
		this.ChkZkipZeroSelected = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.datFromDateSt = new System.Windows.Forms.DateTimePicker();
		this.datToDateST = new System.Windows.Forms.DateTimePicker();
		this.CmbLanguageSelected = new UltraCombo();
		this.cmbAccountTitle = new UltraCombo();
		this.lblLangSelected = new System.Windows.Forms.Label();
		this.btnShowSelectedTrial = new System.Windows.Forms.Button();
		this.CmbCityName = new UltraCombo();
		this.ChkBoxPostSelectedTrial = new System.Windows.Forms.CheckBox();
		this.label9 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsSelectedTrialBalance = new System.Windows.Forms.ToolStrip();
		this.btnRefreshSelectedTrial = new System.Windows.Forms.ToolStripButton();
		this.tsDropDownSelectedTrial = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsDropDownLevelWise = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnReportGroupWise = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSelected).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomeGroups).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentTypeIds).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguageSelected).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsSelectedTrialBalance.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(969, 27);
		this.panel3.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(928, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdSelected;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdSelected.AllowEdit = (InheritableBoolean)2;
		this.grdSelected.AlternatingColors = true;
		((System.Windows.Forms.Control)(object)this.grdSelected).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSelected.DynamicFiltering = true;
		this.grdSelected.FilterMode = (FilterMode)2;
		this.grdSelected.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSelected).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSelected.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSelected).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSelected).Name = "grdSelected";
		((ScrollableBase)this.grdSelected).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSelected).Size = new System.Drawing.Size(969, 401);
		((System.Windows.Forms.Control)(object)this.grdSelected).TabIndex = 0;
		this.grdSelected.TotalRow = (InheritableBoolean)1;
		this.grdSelected.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSelected).VisualStyle = (VisualStyle)4;
		this.grdSelected.LinkClicked += new ColumnActionEventHandler(grdSelected_LinkClicked);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(199, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Trial Balances Selected";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(969, 561);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdSelected);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 160);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(969, 401);
		this.panel6.TabIndex = 108;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(969, 160);
		this.panel5.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtClosingDebit);
		this.groupBox1.Controls.Add(this.txtClosingCredit);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.groupBox1.Controls.Add(this.Branches);
		this.groupBox1.Controls.Add(this.RdSixColumns);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.RdFourColumns);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomeGroups);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.chkSkipCgs);
		this.groupBox1.Controls.Add(this.ChkZkipZeroSelected);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.datFromDateSt);
		this.groupBox1.Controls.Add(this.datToDateST);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguageSelected);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitle);
		this.groupBox1.Controls.Add(this.lblLangSelected);
		this.groupBox1.Controls.Add(this.btnShowSelectedTrial);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.groupBox1.Controls.Add(this.ChkBoxPostSelectedTrial);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(962, 157);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.txtClosingDebit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtClosingDebit.Location = new System.Drawing.Point(305, 14);
		this.txtClosingDebit.Name = "txtClosingDebit";
		this.txtClosingDebit.Size = new System.Drawing.Size(102, 23);
		this.txtClosingDebit.TabIndex = 2;
		this.txtClosingDebit.Text = "0";
		this.txtClosingDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtClosingDebit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtClosingDebit_KeyPress);
		this.txtClosingCredit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtClosingCredit.Location = new System.Drawing.Point(305, 42);
		this.txtClosingCredit.Name = "txtClosingCredit";
		this.txtClosingCredit.Size = new System.Drawing.Size(102, 23);
		this.txtClosingCredit.TabIndex = 3;
		this.txtClosingCredit.Text = "0";
		this.txtClosingCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtClosingCredit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtClosingCredit_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(246, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 15);
		this.label2.TabIndex = 157;
		this.label2.Text = "Cl_Debit";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(246, 46);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(53, 15);
		this.label7.TabIndex = 158;
		this.label7.Text = "Cl_Credit";
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
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
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
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(102, 124);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(305, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 6;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(6, 130);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 154;
		this.Branches.Text = "Branches";
		this.RdSixColumns.AutoSize = true;
		this.RdSixColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdSixColumns.Location = new System.Drawing.Point(808, 96);
		this.RdSixColumns.Name = "RdSixColumns";
		this.RdSixColumns.Size = new System.Drawing.Size(91, 19);
		this.RdSixColumns.TabIndex = 15;
		this.RdSixColumns.Text = "Six Columns";
		this.RdSixColumns.UseVisualStyleBackColor = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 66);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(89, 30);
		this.label1.TabIndex = 150;
		this.label1.Text = "Documents Not To Include";
		this.RdFourColumns.AutoSize = true;
		this.RdFourColumns.Checked = true;
		this.RdFourColumns.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdFourColumns.Location = new System.Drawing.Point(808, 73);
		this.RdFourColumns.Name = "RdFourColumns";
		this.RdFourColumns.Size = new System.Drawing.Size(99, 19);
		this.RdFourColumns.TabIndex = 14;
		this.RdFourColumns.TabStop = true;
		this.RdFourColumns.Text = "Four Columns";
		this.RdFourColumns.UseVisualStyleBackColor = true;
		this.RdFourColumns.CheckedChanged += new System.EventHandler(RdFourColumns_CheckedChanged);
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
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Location = new System.Drawing.Point(508, 12);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Name = "CmbCustomeGroups";
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Size = new System.Drawing.Size(294, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).TabIndex = 7;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(63, 15);
		this.label5.TabIndex = 141;
		this.label5.Text = "From Date";
		this.cmbDocumentTypeIds.AllowNull = (DefaultableBoolean)1;
		this.cmbDocumentTypeIds.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocumentTypeIds.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocumentTypeIds.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocumentTypeIds).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).Location = new System.Drawing.Point(102, 68);
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).Name = "cmbDocumentTypeIds";
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).Size = new System.Drawing.Size(305, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocumentTypeIds).TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 140;
		this.label4.Text = "To Date";
		this.chkSkipCgs.AutoSize = true;
		this.chkSkipCgs.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipCgs.Location = new System.Drawing.Point(808, 54);
		this.chkSkipCgs.Margin = new System.Windows.Forms.Padding(2);
		this.chkSkipCgs.Name = "chkSkipCgs";
		this.chkSkipCgs.Size = new System.Drawing.Size(74, 19);
		this.chkSkipCgs.TabIndex = 13;
		this.chkSkipCgs.Text = "Skip CGS";
		this.chkSkipCgs.UseVisualStyleBackColor = true;
		this.ChkZkipZeroSelected.AutoSize = true;
		this.ChkZkipZeroSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkZkipZeroSelected.Location = new System.Drawing.Point(808, 35);
		this.ChkZkipZeroSelected.Margin = new System.Windows.Forms.Padding(2);
		this.ChkZkipZeroSelected.Name = "ChkZkipZeroSelected";
		this.ChkZkipZeroSelected.Size = new System.Drawing.Size(76, 19);
		this.ChkZkipZeroSelected.TabIndex = 12;
		this.ChkZkipZeroSelected.Text = "Skip Zero";
		this.ChkZkipZeroSelected.UseVisualStyleBackColor = true;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 102);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(88, 15);
		this.label3.TabIndex = 142;
		this.label3.Text = "Group Account";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(412, 18);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(89, 15);
		this.label6.TabIndex = 147;
		this.label6.Text = "Custom Groups";
		this.datFromDateSt.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDateSt.CustomFormat = "dd-MMM-yy";
		this.datFromDateSt.Font = new System.Drawing.Font("Verdana", 9f);
		this.datFromDateSt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDateSt.Location = new System.Drawing.Point(102, 14);
		this.datFromDateSt.Name = "datFromDateSt";
		this.datFromDateSt.Size = new System.Drawing.Size(140, 22);
		this.datFromDateSt.TabIndex = 0;
		this.datToDateST.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateST.CustomFormat = "dd-MMM-yy";
		this.datToDateST.Font = new System.Drawing.Font("Verdana", 9f);
		this.datToDateST.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDateST.Location = new System.Drawing.Point(102, 42);
		this.datToDateST.Name = "datToDateST";
		this.datToDateST.Size = new System.Drawing.Size(140, 22);
		this.datToDateST.TabIndex = 1;
		this.CmbLanguageSelected.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguageSelected.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguageSelected.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguageSelected.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguageSelected).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguageSelected).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguageSelected.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguageSelected).Location = new System.Drawing.Point(508, 68);
		((System.Windows.Forms.Control)(object)this.CmbLanguageSelected).Name = "CmbLanguageSelected";
		((System.Windows.Forms.Control)(object)this.CmbLanguageSelected).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguageSelected).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbLanguageSelected).Visible = false;
		this.cmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Location = new System.Drawing.Point(102, 96);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Name = "cmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Size = new System.Drawing.Size(305, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).TabIndex = 5;
		this.lblLangSelected.AutoSize = true;
		this.lblLangSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLangSelected.Location = new System.Drawing.Point(412, 74);
		this.lblLangSelected.Name = "lblLangSelected";
		this.lblLangSelected.Size = new System.Drawing.Size(59, 15);
		this.lblLangSelected.TabIndex = 146;
		this.lblLangSelected.Text = "Language";
		this.lblLangSelected.Visible = false;
		this.btnShowSelectedTrial.BackColor = System.Drawing.Color.Teal;
		this.btnShowSelectedTrial.FlatAppearance.BorderSize = 0;
		this.btnShowSelectedTrial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowSelectedTrial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowSelectedTrial.ForeColor = System.Drawing.Color.White;
		this.btnShowSelectedTrial.Location = new System.Drawing.Point(746, 69);
		this.btnShowSelectedTrial.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowSelectedTrial.Name = "btnShowSelectedTrial";
		this.btnShowSelectedTrial.Size = new System.Drawing.Size(56, 24);
		this.btnShowSelectedTrial.TabIndex = 10;
		this.btnShowSelectedTrial.Text = "Show";
		this.btnShowSelectedTrial.UseVisualStyleBackColor = false;
		this.btnShowSelectedTrial.Click += new System.EventHandler(btnShowSelectedTrial_Click);
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(508, 40);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(294, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 8;
		this.ChkBoxPostSelectedTrial.AutoSize = true;
		this.ChkBoxPostSelectedTrial.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPostSelectedTrial.Location = new System.Drawing.Point(808, 15);
		this.ChkBoxPostSelectedTrial.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxPostSelectedTrial.Name = "ChkBoxPostSelectedTrial";
		this.ChkBoxPostSelectedTrial.Size = new System.Drawing.Size(154, 19);
		this.ChkBoxPostSelectedTrial.TabIndex = 11;
		this.ChkBoxPostSelectedTrial.Text = "Include UnPost Voucher";
		this.ChkBoxPostSelectedTrial.UseVisualStyleBackColor = true;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(412, 46);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(62, 15);
		this.label9.TabIndex = 144;
		this.label9.Text = "City Name";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(969, 615);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.tsSelectedTrialBalance);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(969, 27);
		this.panel2.TabIndex = 3;
		this.tsSelectedTrialBalance.AutoSize = false;
		this.tsSelectedTrialBalance.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tsSelectedTrialBalance.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsSelectedTrialBalance.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnRefreshSelectedTrial, this.tsDropDownSelectedTrial, this.tsDropDownLevelWise, this.btnReportGroupWise });
		this.tsSelectedTrialBalance.Location = new System.Drawing.Point(0, 0);
		this.tsSelectedTrialBalance.Name = "tsSelectedTrialBalance";
		this.tsSelectedTrialBalance.Size = new System.Drawing.Size(969, 27);
		this.tsSelectedTrialBalance.TabIndex = 8;
		this.tsSelectedTrialBalance.Text = "toolStrip1";
		this.btnRefreshSelectedTrial.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnRefreshSelectedTrial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSelectedTrial.Name = "btnRefreshSelectedTrial";
		this.btnRefreshSelectedTrial.Size = new System.Drawing.Size(59, 24);
		this.btnRefreshSelectedTrial.Text = "&Reset";
		this.btnRefreshSelectedTrial.Click += new System.EventHandler(btnRefreshSelectedTrial_Click);
		this.tsDropDownSelectedTrial.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDownSelectedTrial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDownSelectedTrial.Name = "tsDropDownSelectedTrial";
		this.tsDropDownSelectedTrial.Size = new System.Drawing.Size(65, 24);
		this.tsDropDownSelectedTrial.Text = "Print";
		this.tsDropDownSelectedTrial.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDownSelectedTrial_DropDownItemClicked);
		this.tsDropDownLevelWise.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDownLevelWise.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDownLevelWise.Name = "tsDropDownLevelWise";
		this.tsDropDownLevelWise.Size = new System.Drawing.Size(95, 24);
		this.tsDropDownLevelWise.Text = "Level Wise";
		this.tsDropDownLevelWise.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDownLevelWise_DropDownItemClicked);
		this.btnReportGroupWise.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnReportGroupWise.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReportGroupWise.Name = "btnReportGroupWise";
		this.btnReportGroupWise.Size = new System.Drawing.Size(87, 24);
		this.btnReportGroupWise.Text = "113A-Print";
		this.btnReportGroupWise.Click += new System.EventHandler(btnReportGroupWise_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(969, 615);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "SelectedTrialBalance";
		this.Text = "Trial Balance Selected";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SelectedTrialBalance_FormClosing);
		base.Load += new System.EventHandler(SelectedTrialBalance_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectedTrialBalance_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSelected).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomeGroups).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentTypeIds).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguageSelected).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.tsSelectedTrialBalance.ResumeLayout(false);
		this.tsSelectedTrialBalance.PerformLayout();
		base.ResumeLayout(false);
	}
}

}