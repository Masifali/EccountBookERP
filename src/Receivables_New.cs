// using Architecture.Reports;
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
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
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
namespace Architecture.WinApp.Account_Reports
{

public class Receivables_New : Form
{
	private WaitForm wf = new WaitForm();

	private DataTable dtGrid = new DataTable();

	private DataTable dtOverDue = new DataTable();

	private DataTable dtNotYetDue = new DataTable();

	private List<string> SortingList = new List<string>();

	private int AccouuntClassId;

	private string ScreenName = "";

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker EndDate;

	private Label label27;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel6;

	private Panel panel5;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private TextBox txtAgingDays;

	private Button btnshow;

	private Panel panel7;

	private Label label3;

	private ToolStripButton btnshortcutkey;

	private GroupBox groupBox1;

	private Panel PanelIntervalsMain;

	private Panel PanelInterval01;

	private Label labelInterval01;

	private TextBox txtInverval01Value;

	private Panel PanelInterval04;

	private Label labelInterval04;

	private TextBox txtInterval04Value;

	private Panel PanelInterval03;

	private Label labelInterval03;

	private TextBox txtInterval03Value;

	private Panel PanelInterval02;

	private Label labelInterval02;

	private TextBox txtInterval02Value;

	private GroupBox GroupBoxPeriod;

	private DateTimePicker txttoDate;

	private DateTimePicker txtFromDate;

	private Label label5;

	private Label label4;

	private Panel PanelTotals;

	private Label label7;

	private Label label6;

	private Label labelRecOrPayOfTotals;

	private TextBox txttotalClosing;

	private TextBox txtOverDue;

	private RadioButton RadOverDue;

	private RadioButton RadNotYetDue;

	private RadioButton RadNet;

	internal UltraCombo CmbCustomeGroups;

	internal UltraCombo cmbAccount;

	private Label label2;

	private Label label8;

	private ToolStripButton btnRefresh;

	private Label label9;

	private TextBox txtNotYetDue;

	internal UltraCombo CmbBranches;

	private Label Branches;

	internal UltraCombo CmbCostCenter;

	private Label label1;

	internal UltraCombo CmbAccountTitle;

	private Label label10;

	private CheckBox chkCurrentBalanceSorting;

	private RadioButton rdDescending;

	private RadioButton rdAscending;

	private Button btnApplySorting;

	private CheckBox chkAccountTitleSorting;

	private CheckBox chkAccountCodeSorting;

	private Label lblSortingCriteria;

	public Receivables_New(UserAccount user)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Receivables_New_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1120, 600);
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
			ScreenName = "Receivables_New";
			if (ScreenName == "PayablesAging")
			{
				AccouuntClassId = 3;
				label23.Text = "Accounts Payables Aging";
			}
			else if (ScreenName == "Receivables_New")
			{
				AccouuntClassId = 2;
				label23.Text = "Accounts Receivables Aging";
			}
			txtAgingDays.Focus();
			GroupBoxPeriod.Visible = false;
			PanelIntervalsMain.Visible = false;
			PanelTotals.Visible = false;
			txtFromDate.Enabled = false;
			txttoDate.Enabled = false;
			txtAgingDays.Text = "30";
			AccountFill3rdLevel();
			CustomeGroupsDefine();
			CostCenterFill();
			AccountFillFromGlobal(Conversion.ToInt(cmbAccount.Value));
			if (UserAccount.AppId == 5 && ((DataTable)((UltraGridBase)CmbCostCenter).DataSource).Rows.Count > 0)
			{
				((UltraGridBase)CmbCostCenter).Rows[0].Activate();
				((Control)(object)CmbCostCenter).Enabled = false;
			}
			chkAccountTitleSorting.Checked = true;
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
			DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, "", 0);
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

	private void AccountFill3rdLevel()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle");
			int Id = Conversion.ToInt(cmbAccount.Value);
			DataTable dtAccount = ChartofAccount.ReadAll3rdLevelAccountsForPayablesandReceeivablesAging(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtAccount.Rows.Count > 0)
			{
				for (int i = 0; i < dtAccount.Rows.Count; i++)
				{
					if (AccouuntClassId == 3 && Conversion.ToInt(dtAccount.Rows[i]["TypeNo"]) == 1)
					{
						dt.Rows.Add(dtAccount.Rows[i]["id"], dtAccount.Rows[i]["AccountTitle"]);
					}
					else if (AccouuntClassId == 2 && Conversion.ToInt(dtAccount.Rows[i]["TypeNo"]) == 2)
					{
						dt.Rows.Add(dtAccount.Rows[i]["id"], dtAccount.Rows[i]["AccountTitle"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbAccount, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Id, insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomeGroupsDefine()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCustomeGroups.Value);
			DataTable dtCustome = CommonServices.CustomeGroupsDefine(1);
			InfragisticsHelper.BindAndRetainSelection(CmbCustomeGroups, dtCustome, "Id", "AcLookUpsDescription", "Custom Group", AllColumns: false, Id, insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtCustome.Rows.Count > 0)
			{
				((UltraGridBase)CmbCustomeGroups).DisplayLayout.Bands[0].Columns[1].Width = 336;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CostCenterFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCostCenter.Value);
			DataTable dt = Projects.GetSubCostCenters(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, UserAccount.AppId, 0);
			InfragisticsHelper.BindAndRetainSelection(CmbCostCenter, dt, "Id", "CostCenterName", "Cost Center", AllColumns: true, Id, insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountFillFromGlobal(int ParentAccountId)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("AccountCode", typeof(string));
			dt.Columns.Add("ParentAccountId", typeof(int));
			dt.Columns.Add("ParentAccountTitle", typeof(string));
			IEnumerable<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => r.AccountTypeId == 3 && r.AccountClass == AccouuntClassId);
			if (ParentAccountId != 0)
			{
				allAccounts = allAccounts.Where((getGlobalAllAccountsWithCustomGroup r) => r.ParentAccountId == ParentAccountId);
			}
			HashSet<int> seenIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup account in allAccounts)
			{
				int id = Conversion.ToInt((object)account.ChartOfAccountId);
				if (seenIds.Add(id))
				{
					dt.Rows.Add(id, account.AccountTitle, account.AccountCode, account.ParentAccountId, account.ParentAccountTitle);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbAccountTitle, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbAccountTitle.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbAccountTitle).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			AccountFillFromGlobal(Conversion.ToInt(cmbAccount.Value));
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
			txtAgingDays.Clear();
			txtAgingDays.Focus();
			GroupBoxPeriod.Visible = false;
			PanelIntervalsMain.Visible = false;
			PanelTotals.Visible = false;
			GridFill();
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

	private void Receivables_New_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click_1(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			CostCenterFill();
			AccountFill3rdLevel();
			CustomeGroupsDefine();
			AccountFillFromGlobal(Conversion.ToInt(cmbAccount.Value));
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
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			if (UserAccount.AppId == 5 && Conversion.ToInt(CmbCostCenter.Value) == 0)
			{
				throw new Exception("Cost Center Not Found");
			}
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			DataSet ds = new DataSet();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.ToDate = EndDate.Value;
			obj.AgingDays = Conversion.ToInt((object)txtAgingDays.Text);
			obj.AccouuntClassId = AccouuntClassId;
			obj.CustomGroupId = Conversion.ToInt(CmbCustomeGroups.Value);
			obj.ParentId = Conversion.ToInt(cmbAccount.Value);
			obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
			obj.AccountId = Conversion.ToInt(CmbAccountTitle.Value);
			obj.Activity = "Detail";
			obj.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			if (!RadNet.Checked)
			{
				if (RadOverDue.Checked)
				{
					obj.ReportTypeId = 1;
				}
				else if (RadNotYetDue.Checked)
				{
					obj.ReportTypeId = 2;
				}
			}
			ds = VoucherReports.ReceivablesAging_New(obj);
			dtGrid.Rows.Clear();
			dtOverDue.Rows.Clear();
			dtNotYetDue.Rows.Clear();
			if (ds.Tables.Count > 0)
			{
				GroupBoxPeriod.Visible = true;
				PanelIntervalsMain.Visible = true;
				PanelTotals.Visible = true;
				dtGrid = ds.Tables?[0];
				dtOverDue = ds.Tables?[3];
				dtNotYetDue = ds.Tables?[4];
				if (dtGrid.Rows.Count > 0)
				{
					DatatableHelper.ReplaceCompLogoColumn(dtGrid, (ds.Tables[2].Rows.Count > 0) ? ds.Tables[2] : null);
					txtFromDate.Value = Conversion.ToDateTime(dtGrid.Rows[0]["StartDate"]);
					txttoDate.Value = Conversion.ToDateTime(dtGrid.Rows[0]["AsOnDate"]);
					string sortExpression = string.Join(", ", SortingList.Select((string col) => col + (rdAscending.Checked ? " ASC" : " DESC")));
					lblSortingCriteria.Text = "(" + sortExpression + ")";
					DataView view = dtGrid.DefaultView;
					view.Sort = sortExpression;
					dtGrid = view.ToTable();
					labelInterval01.Text = Conversion.ToString(dtGrid.Rows[0]["FirstIntervalCaption"]);
					labelInterval02.Text = Conversion.ToString(dtGrid.Rows[0]["SecondIntervalCaption"]);
					labelInterval03.Text = Conversion.ToString(dtGrid.Rows[0]["ThirdIntervalCaption"]);
					labelInterval04.Text = Conversion.ToString(dtGrid.Rows[0]["AboveIntervalCaption"]);
					txtInverval01Value.Text = dtGrid.AsEnumerable().Sum((DataRow row) => row.Field<double>("IstIntervale")).ToString(clsGlobalVariables.stringFormatboth);
					txtInterval02Value.Text = dtGrid.AsEnumerable().Sum((DataRow row) => row.Field<double>("ScnInterval")).ToString(clsGlobalVariables.stringFormatboth);
					txtInterval03Value.Text = dtGrid.AsEnumerable().Sum((DataRow row) => row.Field<double>("TrdIntarval")).ToString(clsGlobalVariables.stringFormatboth);
					txtInterval04Value.Text = dtGrid.AsEnumerable().Sum((DataRow row) => row.Field<double>("Above")).ToString(clsGlobalVariables.stringFormatboth);
					txttotalClosing.Text = dtGrid.AsEnumerable().Sum((DataRow row) => row.Field<double>("CurrentBalance")).ToString(clsGlobalVariables.stringFormatboth);
					txtOverDue.Text = dtGrid.AsEnumerable().Sum((DataRow row) => row.Field<double>("OverDue")).ToString(clsGlobalVariables.stringFormatboth);
					txtNotYetDue.Text = dtGrid.AsEnumerable().Sum((DataRow row) => row.Field<double>("NotYetDue")).ToString(clsGlobalVariables.stringFormatboth);
					txtOverDue.Visible = true;
					txtNotYetDue.Visible = true;
					label6.Visible = true;
					label9.Visible = true;
					DataTable dt = new DataTable();
					dt.BeginLoadData();
					dt.Columns.Add("RNo");
					dt.Columns.Add("BranchesId", typeof(int));
					dt.Columns.Add("BranchName");
					dt.Columns.Add("AccountId");
					dt.Columns.Add("ParentAccount");
					dt.Columns.Add("AccountCode");
					dt.Columns.Add("AccountTitle");
					dt.Columns.Add("AccountType");
					dt.Columns.Add("CurrentBalance", typeof(double));
					dt.Columns.Add("OverDue", typeof(double));
					dt.Columns.Add("NotYetDue", typeof(double));
					dt.Columns.Add(Conversion.ToString(dtGrid.Rows[0]["FirstIntervalCaption"]), typeof(double));
					dt.Columns.Add(Conversion.ToString(dtGrid.Rows[0]["SecondIntervalCaption"]), typeof(double));
					dt.Columns.Add(Conversion.ToString(dtGrid.Rows[0]["ThirdIntervalCaption"]), typeof(double));
					dt.Columns.Add(Conversion.ToString(dtGrid.Rows[0]["AboveIntervalCaption"]), typeof(double));
					dt.Columns.Add("Closing", typeof(double));
					int lenthofcolumnsofnewdt = (from DataColumn x in dt.Columns
						select x.ColumnName).ToArray().Length - 7;
					foreach (DataRow row2 in dtGrid.Rows)
					{
						dt.Rows.Add(row2["RNo"], row2["BranchesId"], row2["BranchName"], row2["AccountId"], row2["ParentAccount"], row2["AccountCode"], row2["AccountTitle"], row2["AccountType"], row2["CurrentBalance"], row2["OverDue"], row2["NotYetDue"], row2["IstIntervale"], row2["ScnInterval"], row2["TrdIntarval"], row2["Above"], row2["Closing"]);
					}
					dt.EndLoadData();
					grd.DataSource = dt;
					grd.RetrieveStructure();
					GridSettings(lenthofcolumnsofnewdt);
				}
				else
				{
					GroupBoxPeriod.Visible = false;
					PanelIntervalsMain.Visible = false;
					PanelTotals.Visible = false;
					grd.DataSource = null;
				}
			}
			else
			{
				GroupBoxPeriod.Visible = false;
				PanelIntervalsMain.Visible = false;
				PanelTotals.Visible = false;
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings(int lenthofcolumnsofnewdt)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Groups.Remove(grd.RootTable.Groups["ParentAccount"]);
			grd.RootTable.Groups.Add("ParentAccount");
			grd.RootTable.Columns["ParentAccount"].HideWhenGrouped = (InheritableBoolean)1;
			grd.RootTable.Columns["RNo"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grd.RootTable.Columns["BranchesId"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["OverDue"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["NotYetDue"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["Closing"].Visible = false;
			grd.RootTable.Columns["RNo"].Width = 60;
			grd.RootTable.Columns["BranchName"].Width = 120;
			grd.RootTable.Columns["AccountType"].Width = 65;
			grd.RootTable.Columns["AccountCode"].Width = 95;
			grd.RootTable.Columns["ParentAccount"].Width = 150;
			grd.RootTable.Columns["AccountTitle"].Width = 250;
			grd.RootTable.Columns["CurrentBalance"].Width = 110;
			grd.RootTable.Columns["OverDue"].Width = 110;
			grd.RootTable.Columns["NotYetDue"].Width = 110;
			grd.RootTable.Columns[labelInterval01.Text].Width = 110;
			grd.RootTable.Columns[labelInterval02.Text].Width = 110;
			grd.RootTable.Columns[labelInterval03.Text].Width = 110;
			grd.RootTable.Columns[labelInterval04.Text].Width = 110;
			grd.RootTable.Columns["Closing"].Width = 110;
			if (RadOverDue.Checked)
			{
				grd.RootTable.Columns["Closing"].Caption = "Over Due Total";
			}
			else if (RadNotYetDue.Checked)
			{
				grd.RootTable.Columns["Closing"].Caption = "Not Yet Due Total";
			}
			for (int i = 0; i < lenthofcolumnsofnewdt; i++)
			{
				grd.RootTable.Columns[i + 7].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns[i + 7].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns[i + 7].FormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns[i + 7].TotalFormatString = clsGlobalVariables.stringFormatboth;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(item.Cells["AccountId"].Value);
			int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
			if (e.Column.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(Id, Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period), EndDate.Value, 0, branchesId);
			}
			else if (e.Column.Key == "OverDue")
			{
				EnumerableRowCollection<DataRow> rows = from r in dtOverDue.AsEnumerable()
					where Conversion.ToInt(r["AccountId"]) == Id
					select r;
				DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtOverDue.Clone());
				if (dt.Rows.Count > 0)
				{
					PayablesReceivablesAgingBreakPopUp payablesReceivablesAgingBreakPopUp = new PayablesReceivablesAgingBreakPopUp(UserAccount, dt);
					payablesReceivablesAgingBreakPopUp.lblFor.Text = "Receivable Aging of Over Due";
					payablesReceivablesAgingBreakPopUp.ReportType_OverDue = true;
					payablesReceivablesAgingBreakPopUp.Show();
				}
			}
			else if (e.Column.Key == "NotYetDue")
			{
				EnumerableRowCollection<DataRow> rows2 = from r in dtNotYetDue.AsEnumerable()
					where Conversion.ToInt(r["AccountId"]) == Id
					select r;
				DataTable dt2 = (rows2.Any() ? rows2.CopyToDataTable() : dtNotYetDue.Clone());
				if (dt2.Rows.Count > 0)
				{
					PayablesReceivablesAgingBreakPopUp payablesReceivablesAgingBreakPopUp2 = new PayablesReceivablesAgingBreakPopUp(UserAccount, dt2);
					payablesReceivablesAgingBreakPopUp2.lblFor.Text = "Receivable Aging of Net Yet Due";
					payablesReceivablesAgingBreakPopUp2.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count > 0 && dtGrid != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dtGrid, "123_Receivables_New.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
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
			wf.Close();
			MessageBox.Show(ex.ToString());
		}
	}

	private void Receivables_New_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				Print_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click_1(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				EndDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				EndDate.Focus();
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

	private void btnshortcutkey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl + P", "For print");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grd.CurrentColumn != null)
			{
				int id = Conversion.ToInt(item.Cells["AccountId"].Value);
				int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
				if (grd.CurrentColumn.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(id, Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period), EndDate.Value, 0, branchesId);
				}
			}
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
			if (chkCurrentBalanceSorting.Checked)
			{
				if (!SortingList.Contains("CurrentBalance"))
				{
					SortingList.Add("CurrentBalance");
				}
			}
			else
			{
				SortingList.Remove("CurrentBalance");
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

	private void chkCurrentBalanceSorting_CheckedChanged(object sender, EventArgs e)
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
			if (dtGrid.Rows.Count > 0)
			{
				string sortExpression = string.Join(", ", SortingList.Select((string col) => col + (rdAscending.Checked ? " ASC" : " DESC")));
				lblSortingCriteria.Text = "(" + sortExpression + ")";
				DataView view = dtGrid.DefaultView;
				view.Sort = sortExpression;
				dtGrid = view.ToTable();
				DataTable dt = new DataTable();
				dt.BeginLoadData();
				dt.Columns.Add("RNo");
				dt.Columns.Add("BranchesId", typeof(int));
				dt.Columns.Add("BranchName");
				dt.Columns.Add("AccountId");
				dt.Columns.Add("ParentAccount");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("AccountTitle");
				dt.Columns.Add("AccountType");
				dt.Columns.Add("CurrentBalance", typeof(double));
				dt.Columns.Add("OverDue", typeof(double));
				dt.Columns.Add("NotYetDue", typeof(double));
				dt.Columns.Add(Conversion.ToString(dtGrid.Rows[0]["FirstIntervalCaption"]), typeof(double));
				dt.Columns.Add(Conversion.ToString(dtGrid.Rows[0]["SecondIntervalCaption"]), typeof(double));
				dt.Columns.Add(Conversion.ToString(dtGrid.Rows[0]["ThirdIntervalCaption"]), typeof(double));
				dt.Columns.Add(Conversion.ToString(dtGrid.Rows[0]["AboveIntervalCaption"]), typeof(double));
				dt.Columns.Add("Closing", typeof(double));
				int lenthofcolumnsofnewdt = (from DataColumn x in dt.Columns
					select x.ColumnName).ToArray().Length - 7;
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["RNo"], row["BranchesId"], row["BranchName"], row["AccountId"], row["ParentAccount"], row["AccountCode"], row["AccountTitle"], row["AccountType"], row["CurrentBalance"], row["OverDue"], row["NotYetDue"], row["IstIntervale"], row["ScnInterval"], row["TrdIntarval"], row["Above"], row["Closing"]);
				}
				dt.EndLoadData();
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSettings(lenthofcolumnsofnewdt);
			}
			else
			{
				GroupBoxPeriod.Visible = false;
				PanelIntervalsMain.Visible = false;
				PanelTotals.Visible = false;
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
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Expected O, but got Unknown
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1a: Expected O, but got Unknown
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
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.Receivables_New));
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.chkCurrentBalanceSorting = new System.Windows.Forms.CheckBox();
		this.rdDescending = new System.Windows.Forms.RadioButton();
		this.rdAscending = new System.Windows.Forms.RadioButton();
		this.btnApplySorting = new System.Windows.Forms.Button();
		this.chkAccountTitleSorting = new System.Windows.Forms.CheckBox();
		this.chkAccountCodeSorting = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.RadOverDue = new System.Windows.Forms.RadioButton();
		this.RadNotYetDue = new System.Windows.Forms.RadioButton();
		this.RadNet = new System.Windows.Forms.RadioButton();
		this.PanelTotals = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.labelRecOrPayOfTotals = new System.Windows.Forms.Label();
		this.txttotalClosing = new System.Windows.Forms.TextBox();
		this.txtNotYetDue = new System.Windows.Forms.TextBox();
		this.txtOverDue = new System.Windows.Forms.TextBox();
		this.PanelIntervalsMain = new System.Windows.Forms.Panel();
		this.PanelInterval04 = new System.Windows.Forms.Panel();
		this.labelInterval04 = new System.Windows.Forms.Label();
		this.txtInterval04Value = new System.Windows.Forms.TextBox();
		this.PanelInterval03 = new System.Windows.Forms.Panel();
		this.labelInterval03 = new System.Windows.Forms.Label();
		this.txtInterval03Value = new System.Windows.Forms.TextBox();
		this.PanelInterval02 = new System.Windows.Forms.Panel();
		this.labelInterval02 = new System.Windows.Forms.Label();
		this.txtInterval02Value = new System.Windows.Forms.TextBox();
		this.PanelInterval01 = new System.Windows.Forms.Panel();
		this.labelInterval01 = new System.Windows.Forms.Label();
		this.txtInverval01Value = new System.Windows.Forms.TextBox();
		this.GroupBoxPeriod = new System.Windows.Forms.GroupBox();
		this.txttoDate = new System.Windows.Forms.DateTimePicker();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.CmbCustomeGroups = new UltraCombo();
		this.CmbAccountTitle = new UltraCombo();
		this.cmbAccount = new UltraCombo();
		this.EndDate = new System.Windows.Forms.DateTimePicker();
		this.txtAgingDays = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkey = new System.Windows.Forms.ToolStripButton();
		this.lblSortingCriteria = new System.Windows.Forms.Label();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.PanelTotals.SuspendLayout();
		this.PanelIntervalsMain.SuspendLayout();
		this.PanelInterval04.SuspendLayout();
		this.PanelInterval03.SuspendLayout();
		this.PanelInterval02.SuspendLayout();
		this.PanelInterval01.SuspendLayout();
		this.GroupBoxPeriod.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomeGroups).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1104, 27);
		this.panel3.TabIndex = 4;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(455, 0);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 5;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 1);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(134, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Accounts Aging";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(369, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 14);
		this.label1.TabIndex = 104;
		this.label1.Text = "Cost Center";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1060, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1104, 278);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1104, 457);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 151);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1104, 306);
		this.panel6.TabIndex = 110;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.lblSortingCriteria);
		this.panel7.Controls.Add(this.chkCurrentBalanceSorting);
		this.panel7.Controls.Add(this.rdDescending);
		this.panel7.Controls.Add(this.rdAscending);
		this.panel7.Controls.Add(this.btnApplySorting);
		this.panel7.Controls.Add(this.chkAccountTitleSorting);
		this.panel7.Controls.Add(this.chkAccountCodeSorting);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1104, 28);
		this.panel7.TabIndex = 5;
		this.chkCurrentBalanceSorting.AutoSize = true;
		this.chkCurrentBalanceSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCurrentBalanceSorting.ForeColor = System.Drawing.Color.White;
		this.chkCurrentBalanceSorting.Location = new System.Drawing.Point(372, 5);
		this.chkCurrentBalanceSorting.Name = "chkCurrentBalanceSorting";
		this.chkCurrentBalanceSorting.Size = new System.Drawing.Size(115, 19);
		this.chkCurrentBalanceSorting.TabIndex = 177;
		this.chkCurrentBalanceSorting.Text = "Current Balance";
		this.chkCurrentBalanceSorting.UseVisualStyleBackColor = true;
		this.chkCurrentBalanceSorting.CheckedChanged += new System.EventHandler(chkCurrentBalanceSorting_CheckedChanged);
		this.rdDescending.AutoSize = true;
		this.rdDescending.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDescending.ForeColor = System.Drawing.Color.White;
		this.rdDescending.Location = new System.Drawing.Point(673, 5);
		this.rdDescending.Name = "rdDescending";
		this.rdDescending.Size = new System.Drawing.Size(90, 19);
		this.rdDescending.TabIndex = 176;
		this.rdDescending.Text = "Descending";
		this.rdDescending.UseVisualStyleBackColor = true;
		this.rdAscending.AutoSize = true;
		this.rdAscending.Checked = true;
		this.rdAscending.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAscending.ForeColor = System.Drawing.Color.White;
		this.rdAscending.Location = new System.Drawing.Point(589, 5);
		this.rdAscending.Name = "rdAscending";
		this.rdAscending.Size = new System.Drawing.Size(82, 19);
		this.rdAscending.TabIndex = 175;
		this.rdAscending.TabStop = true;
		this.rdAscending.Text = "Ascending";
		this.rdAscending.UseVisualStyleBackColor = true;
		this.btnApplySorting.BackColor = System.Drawing.Color.White;
		this.btnApplySorting.FlatAppearance.BorderSize = 0;
		this.btnApplySorting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApplySorting.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApplySorting.ForeColor = System.Drawing.Color.Teal;
		this.btnApplySorting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApplySorting.Location = new System.Drawing.Point(489, 3);
		this.btnApplySorting.Margin = new System.Windows.Forms.Padding(2);
		this.btnApplySorting.Name = "btnApplySorting";
		this.btnApplySorting.Size = new System.Drawing.Size(93, 23);
		this.btnApplySorting.TabIndex = 174;
		this.btnApplySorting.Text = "Apply Sorting";
		this.btnApplySorting.UseVisualStyleBackColor = false;
		this.btnApplySorting.Click += new System.EventHandler(btnApplySorting_Click);
		this.chkAccountTitleSorting.AutoSize = true;
		this.chkAccountTitleSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAccountTitleSorting.ForeColor = System.Drawing.Color.White;
		this.chkAccountTitleSorting.Location = new System.Drawing.Point(271, 5);
		this.chkAccountTitleSorting.Name = "chkAccountTitleSorting";
		this.chkAccountTitleSorting.Size = new System.Drawing.Size(100, 19);
		this.chkAccountTitleSorting.TabIndex = 173;
		this.chkAccountTitleSorting.Text = "Account Title";
		this.chkAccountTitleSorting.UseVisualStyleBackColor = true;
		this.chkAccountTitleSorting.CheckedChanged += new System.EventHandler(chkAccountTitleSorting_CheckedChanged);
		this.chkAccountCodeSorting.AutoSize = true;
		this.chkAccountCodeSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAccountCodeSorting.ForeColor = System.Drawing.Color.White;
		this.chkAccountCodeSorting.Location = new System.Drawing.Point(162, 5);
		this.chkAccountCodeSorting.Name = "chkAccountCodeSorting";
		this.chkAccountCodeSorting.Size = new System.Drawing.Size(103, 19);
		this.chkAccountCodeSorting.TabIndex = 172;
		this.chkAccountCodeSorting.Text = "Account Code";
		this.chkAccountCodeSorting.UseVisualStyleBackColor = true;
		this.chkAccountCodeSorting.CheckedChanged += new System.EventHandler(chkAccountCodeSorting_CheckedChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(142, 19);
		this.label3.TabIndex = 1;
		this.label3.Text = "Filtered Records";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.RadOverDue);
		this.panel5.Controls.Add(this.RadNotYetDue);
		this.panel5.Controls.Add(this.RadNet);
		this.panel5.Controls.Add(this.PanelTotals);
		this.panel5.Controls.Add(this.PanelIntervalsMain);
		this.panel5.Controls.Add(this.GroupBoxPeriod);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1104, 151);
		this.panel5.TabIndex = 0;
		this.RadOverDue.AutoSize = true;
		this.RadOverDue.Checked = true;
		this.RadOverDue.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadOverDue.Location = new System.Drawing.Point(319, 37);
		this.RadOverDue.Name = "RadOverDue";
		this.RadOverDue.Size = new System.Drawing.Size(85, 17);
		this.RadOverDue.TabIndex = 2;
		this.RadOverDue.TabStop = true;
		this.RadOverDue.Text = "Over Due";
		this.RadOverDue.UseVisualStyleBackColor = true;
		this.RadNotYetDue.AutoSize = true;
		this.RadNotYetDue.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNotYetDue.Location = new System.Drawing.Point(319, 21);
		this.RadNotYetDue.Name = "RadNotYetDue";
		this.RadNotYetDue.Size = new System.Drawing.Size(101, 17);
		this.RadNotYetDue.TabIndex = 1;
		this.RadNotYetDue.Text = "Not Yet Due";
		this.RadNotYetDue.UseVisualStyleBackColor = true;
		this.RadNet.AutoSize = true;
		this.RadNet.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNet.Location = new System.Drawing.Point(319, 4);
		this.RadNet.Name = "RadNet";
		this.RadNet.Size = new System.Drawing.Size(122, 17);
		this.RadNet.TabIndex = 0;
		this.RadNet.Text = "Net Receivable";
		this.RadNet.UseVisualStyleBackColor = true;
		this.PanelTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelTotals.Controls.Add(this.label7);
		this.PanelTotals.Controls.Add(this.label9);
		this.PanelTotals.Controls.Add(this.label6);
		this.PanelTotals.Controls.Add(this.labelRecOrPayOfTotals);
		this.PanelTotals.Controls.Add(this.txttotalClosing);
		this.PanelTotals.Controls.Add(this.txtNotYetDue);
		this.PanelTotals.Controls.Add(this.txtOverDue);
		this.PanelTotals.Location = new System.Drawing.Point(842, 3);
		this.PanelTotals.Name = "PanelTotals";
		this.PanelTotals.Size = new System.Drawing.Size(259, 137);
		this.PanelTotals.TabIndex = 2;
		this.PanelTotals.Visible = false;
		this.label7.BackColor = System.Drawing.Color.DarkTurquoise;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(3, 33);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 30);
		this.label7.TabIndex = 109;
		this.label7.Text = "Curr Balance";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label9.BackColor = System.Drawing.Color.DarkTurquoise;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Black;
		this.label9.Location = new System.Drawing.Point(3, 99);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(79, 29);
		this.label9.TabIndex = 109;
		this.label9.Text = "Not Yet Due";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label6.BackColor = System.Drawing.Color.DarkTurquoise;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(3, 66);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 29);
		this.label6.TabIndex = 109;
		this.label6.Text = "Over Due";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.labelRecOrPayOfTotals.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.labelRecOrPayOfTotals.Dock = System.Windows.Forms.DockStyle.Top;
		this.labelRecOrPayOfTotals.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelRecOrPayOfTotals.Location = new System.Drawing.Point(0, 0);
		this.labelRecOrPayOfTotals.Name = "labelRecOrPayOfTotals";
		this.labelRecOrPayOfTotals.Size = new System.Drawing.Size(257, 31);
		this.labelRecOrPayOfTotals.TabIndex = 107;
		this.labelRecOrPayOfTotals.Text = "Totals of Receivables";
		this.labelRecOrPayOfTotals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txttotalClosing.Enabled = false;
		this.txttotalClosing.Font = new System.Drawing.Font("Verdana", 13f);
		this.txttotalClosing.Location = new System.Drawing.Point(86, 34);
		this.txttotalClosing.Margin = new System.Windows.Forms.Padding(2);
		this.txttotalClosing.Name = "txttotalClosing";
		this.txttotalClosing.Size = new System.Drawing.Size(166, 29);
		this.txttotalClosing.TabIndex = 1;
		this.txtNotYetDue.Enabled = false;
		this.txtNotYetDue.Font = new System.Drawing.Font("Verdana", 13f);
		this.txtNotYetDue.Location = new System.Drawing.Point(86, 99);
		this.txtNotYetDue.Margin = new System.Windows.Forms.Padding(2);
		this.txtNotYetDue.Name = "txtNotYetDue";
		this.txtNotYetDue.Size = new System.Drawing.Size(166, 29);
		this.txtNotYetDue.TabIndex = 1;
		this.txtOverDue.Enabled = false;
		this.txtOverDue.Font = new System.Drawing.Font("Verdana", 13f);
		this.txtOverDue.Location = new System.Drawing.Point(86, 66);
		this.txtOverDue.Margin = new System.Windows.Forms.Padding(2);
		this.txtOverDue.Name = "txtOverDue";
		this.txtOverDue.Size = new System.Drawing.Size(166, 29);
		this.txtOverDue.TabIndex = 1;
		this.PanelIntervalsMain.Controls.Add(this.PanelInterval04);
		this.PanelIntervalsMain.Controls.Add(this.PanelInterval03);
		this.PanelIntervalsMain.Controls.Add(this.PanelInterval02);
		this.PanelIntervalsMain.Controls.Add(this.PanelInterval01);
		this.PanelIntervalsMain.Location = new System.Drawing.Point(453, 3);
		this.PanelIntervalsMain.Name = "PanelIntervalsMain";
		this.PanelIntervalsMain.Size = new System.Drawing.Size(386, 137);
		this.PanelIntervalsMain.TabIndex = 107;
		this.PanelIntervalsMain.Visible = false;
		this.PanelInterval04.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelInterval04.Controls.Add(this.labelInterval04);
		this.PanelInterval04.Controls.Add(this.txtInterval04Value);
		this.PanelInterval04.Location = new System.Drawing.Point(192, 68);
		this.PanelInterval04.Name = "PanelInterval04";
		this.PanelInterval04.Size = new System.Drawing.Size(187, 66);
		this.PanelInterval04.TabIndex = 109;
		this.labelInterval04.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.labelInterval04.Dock = System.Windows.Forms.DockStyle.Top;
		this.labelInterval04.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelInterval04.Location = new System.Drawing.Point(0, 0);
		this.labelInterval04.Name = "labelInterval04";
		this.labelInterval04.Size = new System.Drawing.Size(185, 31);
		this.labelInterval04.TabIndex = 107;
		this.labelInterval04.Text = "Interval 04";
		this.labelInterval04.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtInterval04Value.Enabled = false;
		this.txtInterval04Value.Font = new System.Drawing.Font("Verdana", 14f);
		this.txtInterval04Value.Location = new System.Drawing.Point(3, 33);
		this.txtInterval04Value.Margin = new System.Windows.Forms.Padding(2);
		this.txtInterval04Value.Name = "txtInterval04Value";
		this.txtInterval04Value.Size = new System.Drawing.Size(192, 30);
		this.txtInterval04Value.TabIndex = 1;
		this.PanelInterval03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelInterval03.Controls.Add(this.labelInterval03);
		this.PanelInterval03.Controls.Add(this.txtInterval03Value);
		this.PanelInterval03.Location = new System.Drawing.Point(0, 68);
		this.PanelInterval03.Name = "PanelInterval03";
		this.PanelInterval03.Size = new System.Drawing.Size(187, 67);
		this.PanelInterval03.TabIndex = 108;
		this.labelInterval03.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.labelInterval03.Dock = System.Windows.Forms.DockStyle.Top;
		this.labelInterval03.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelInterval03.Location = new System.Drawing.Point(0, 0);
		this.labelInterval03.Name = "labelInterval03";
		this.labelInterval03.Size = new System.Drawing.Size(185, 31);
		this.labelInterval03.TabIndex = 107;
		this.labelInterval03.Text = "Interval 03";
		this.labelInterval03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtInterval03Value.Enabled = false;
		this.txtInterval03Value.Font = new System.Drawing.Font("Verdana", 14f);
		this.txtInterval03Value.Location = new System.Drawing.Point(1, 33);
		this.txtInterval03Value.Margin = new System.Windows.Forms.Padding(2);
		this.txtInterval03Value.Name = "txtInterval03Value";
		this.txtInterval03Value.Size = new System.Drawing.Size(192, 30);
		this.txtInterval03Value.TabIndex = 1;
		this.PanelInterval02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelInterval02.Controls.Add(this.labelInterval02);
		this.PanelInterval02.Controls.Add(this.txtInterval02Value);
		this.PanelInterval02.Location = new System.Drawing.Point(192, 0);
		this.PanelInterval02.Name = "PanelInterval02";
		this.PanelInterval02.Size = new System.Drawing.Size(187, 66);
		this.PanelInterval02.TabIndex = 107;
		this.labelInterval02.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.labelInterval02.Dock = System.Windows.Forms.DockStyle.Top;
		this.labelInterval02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelInterval02.Location = new System.Drawing.Point(0, 0);
		this.labelInterval02.Name = "labelInterval02";
		this.labelInterval02.Size = new System.Drawing.Size(185, 31);
		this.labelInterval02.TabIndex = 107;
		this.labelInterval02.Text = "Interval 02";
		this.labelInterval02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtInterval02Value.Enabled = false;
		this.txtInterval02Value.Font = new System.Drawing.Font("Verdana", 14f);
		this.txtInterval02Value.Location = new System.Drawing.Point(3, 33);
		this.txtInterval02Value.Margin = new System.Windows.Forms.Padding(2);
		this.txtInterval02Value.Name = "txtInterval02Value";
		this.txtInterval02Value.Size = new System.Drawing.Size(192, 30);
		this.txtInterval02Value.TabIndex = 1;
		this.PanelInterval01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelInterval01.Controls.Add(this.labelInterval01);
		this.PanelInterval01.Controls.Add(this.txtInverval01Value);
		this.PanelInterval01.Location = new System.Drawing.Point(0, 0);
		this.PanelInterval01.Name = "PanelInterval01";
		this.PanelInterval01.Size = new System.Drawing.Size(187, 67);
		this.PanelInterval01.TabIndex = 106;
		this.labelInterval01.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.labelInterval01.Dock = System.Windows.Forms.DockStyle.Top;
		this.labelInterval01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelInterval01.Location = new System.Drawing.Point(0, 0);
		this.labelInterval01.Name = "labelInterval01";
		this.labelInterval01.Size = new System.Drawing.Size(185, 31);
		this.labelInterval01.TabIndex = 107;
		this.labelInterval01.Text = "Interval 01";
		this.labelInterval01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtInverval01Value.Enabled = false;
		this.txtInverval01Value.Font = new System.Drawing.Font("Verdana", 14f);
		this.txtInverval01Value.Location = new System.Drawing.Point(1, 33);
		this.txtInverval01Value.Margin = new System.Windows.Forms.Padding(2);
		this.txtInverval01Value.Name = "txtInverval01Value";
		this.txtInverval01Value.Size = new System.Drawing.Size(192, 30);
		this.txtInverval01Value.TabIndex = 1;
		this.GroupBoxPeriod.Controls.Add(this.txttoDate);
		this.GroupBoxPeriod.Controls.Add(this.txtFromDate);
		this.GroupBoxPeriod.Controls.Add(this.label5);
		this.GroupBoxPeriod.Controls.Add(this.label4);
		this.GroupBoxPeriod.Font = new System.Drawing.Font("Tahoma", 7f, System.Drawing.FontStyle.Bold);
		this.GroupBoxPeriod.Location = new System.Drawing.Point(311, 54);
		this.GroupBoxPeriod.Name = "GroupBoxPeriod";
		this.GroupBoxPeriod.Size = new System.Drawing.Size(138, 98);
		this.GroupBoxPeriod.TabIndex = 3;
		this.GroupBoxPeriod.TabStop = false;
		this.GroupBoxPeriod.Text = "Period Filtered Records";
		this.GroupBoxPeriod.Visible = false;
		this.txttoDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttoDate.CustomFormat = "dd-MMM-yy";
		this.txttoDate.Enabled = false;
		this.txttoDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttoDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txttoDate.Location = new System.Drawing.Point(4, 71);
		this.txttoDate.Name = "txttoDate";
		this.txttoDate.Size = new System.Drawing.Size(130, 23);
		this.txttoDate.TabIndex = 0;
		this.txtFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.CustomFormat = "dd-MMM-yy";
		this.txtFromDate.Enabled = false;
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(4, 31);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(130, 23);
		this.txtFromDate.TabIndex = 0;
		this.label5.BackColor = System.Drawing.Color.DarkTurquoise;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(4, 55);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(130, 15);
		this.label5.TabIndex = 108;
		this.label5.Text = "To Date";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label4.BackColor = System.Drawing.Color.DarkTurquoise;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(4, 15);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(130, 15);
		this.label4.TabIndex = 108;
		this.label4.Text = "From Date";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.groupBox1.Controls.Add(this.Branches);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomeGroups);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccount);
		this.groupBox1.Controls.Add(this.EndDate);
		this.groupBox1.Controls.Add(this.txtAgingDays);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.25f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(308, 151);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
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
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
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
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(102, 122);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 5;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(6, 128);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 165;
		this.Branches.Text = "Branches";
		this.CmbCustomeGroups.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomeGroups.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomeGroups.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomeGroups.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Appearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomeGroups).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Location = new System.Drawing.Point(102, 95);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Name = "CmbCustomeGroups";
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomeGroups).TabIndex = 4;
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(102, 67);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 3;
		this.cmbAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Location = new System.Drawing.Point(102, 39);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Name = "cmbAccount";
		((System.Windows.Forms.Control)(object)this.cmbAccount).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccount).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Leave += new System.EventHandler(cmbAccount_Leave);
		this.EndDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.EndDate.CustomFormat = "dd-MMM-yy";
		this.EndDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.EndDate.Location = new System.Drawing.Point(102, 15);
		this.EndDate.Name = "EndDate";
		this.EndDate.Size = new System.Drawing.Size(113, 23);
		this.EndDate.TabIndex = 0;
		this.txtAgingDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAgingDays.Location = new System.Drawing.Point(216, 15);
		this.txtAgingDays.Margin = new System.Windows.Forms.Padding(2);
		this.txtAgingDays.Name = "txtAgingDays";
		this.txtAgingDays.Size = new System.Drawing.Size(86, 23);
		this.txtAgingDays.TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 73);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 132;
		this.label10.Text = "Account Title";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 101);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 15);
		this.label2.TabIndex = 134;
		this.label2.Text = "Custom Group";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 45);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(94, 15);
		this.label8.TabIndex = 132;
		this.label8.Text = "Control Account";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(247, 124);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 23);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 15);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(93, 30);
		this.label27.TabIndex = 103;
		this.label27.Text = "End Date && Aging Days";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1104, 511);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1104, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.print, this.btnshortcutkey });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1104, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click_1);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "123-Print";
		this.print.Click += new System.EventHandler(Print_Click);
		this.btnshortcutkey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkey.Name = "btnshortcutkey";
		this.btnshortcutkey.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkey.Text = "ShortCut Keys";
		this.btnshortcutkey.Click += new System.EventHandler(btnshortcutkey_Click);
		this.lblSortingCriteria.AutoSize = true;
		this.lblSortingCriteria.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSortingCriteria.ForeColor = System.Drawing.Color.White;
		this.lblSortingCriteria.Location = new System.Drawing.Point(764, 7);
		this.lblSortingCriteria.Name = "lblSortingCriteria";
		this.lblSortingCriteria.Size = new System.Drawing.Size(86, 15);
		this.lblSortingCriteria.TabIndex = 186;
		this.lblSortingCriteria.Text = "SortingCritera";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1104, 511);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "Receivables_New";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Receivables_New_FormClosing);
		base.Load += new System.EventHandler(Receivables_New_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Receivables_New_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.PanelTotals.ResumeLayout(false);
		this.PanelTotals.PerformLayout();
		this.PanelIntervalsMain.ResumeLayout(false);
		this.PanelInterval04.ResumeLayout(false);
		this.PanelInterval04.PerformLayout();
		this.PanelInterval03.ResumeLayout(false);
		this.PanelInterval03.PerformLayout();
		this.PanelInterval02.ResumeLayout(false);
		this.PanelInterval02.PerformLayout();
		this.PanelInterval01.ResumeLayout(false);
		this.PanelInterval01.PerformLayout();
		this.GroupBoxPeriod.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomeGroups).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
