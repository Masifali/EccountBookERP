using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.SMS;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.SMS;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Configurations;
using Architecture.WinApp.SMSSystem;
using GsmComm.GsmCommunication;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class Receivables : Form
{
	private bool UpdateMode;

	private int RecId;

	public DateTime FromDate;

	public DateTime ToDate;

	public int CompanyIdFromBreakUp;

	public GsmCommMain gsmComm;

	private DataTable dtGroups = new DataTable();

	private DataTable dtreceive = new DataTable();

	private DataTable dtAccount = new DataTable();

	private DataTable dtfollowUp = new DataTable();

	private DataTable dtForLayout = new DataTable();

	private DataTable dtBillDetail = new DataTable();

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private bool FormInitialized;

	public string BranchesIds = "";

	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private List<SupplierCustomer> lstsupp = new List<SupplierCustomer>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private Panel panel7;

	private Button btnshow;

	private DateTimePicker todate;

	private Label label29;

	private DateTimePicker fromdate;

	private Label label27;

	private Panel panel6;

	internal GridEX grd;

	internal GridEX grdBillDetail;

	internal UltraCombo cmbCompany;

	private Panel panel5;

	private Panel panel10;

	private Panel panel9;

	private Panel panel8;

	private ToolStrip toolStrip1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton btnFollowUpRefresh;

	private ToolStripButton btnsave;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnUpdate;

	private TextBox txtNextFollowupDays;

	private DateTimePicker txtPromiseDate;

	private Label label1;

	private Label label3;

	private Label label12;

	private TextBox txtCommentDetail;

	private DateTimePicker txtFollowupDate;

	private Label label2;

	internal UltraCombo cmbCOA;

	private Label label7;

	internal GridEX grdReceiveable;

	private ToolStripButton btnSMS;

	private TextBox txtBalanceTo;

	private TextBox txtBalanceFrom;

	private Label label5;

	private Label label4;

	private Label label8;

	internal UltraCombo cmbAccount;

	private TextBox txttest;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnSmSSave;

	internal UltraCombo CmbCityName;

	private Label label9;

	private Label label10;

	internal UltraCombo CmbCustomGroup;

	private CheckBox ChkShowOnlyTrade;

	internal UltraCombo cmbCustomerGroup;

	private Label label11;

	private Panel panel4;

	private Label label23;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private CheckBox SkipZero;

	internal UltraCombo CmbBranches;

	private Label Branches;

	private GroupBox groupBox1;

	private RadioButton rdAll;

	private RadioButton RdAssests;

	private RadioButton RdLiability;

	public Receivables(UserAccount user)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void reset()
	{
		GridSetting();
		bindBillHistoryDetail();
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		ShowReport();
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
		try
		{
			wf.Show();
			DataFill();
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

	private void DataFill()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			string account = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbAccount).Text, dtAccount, "AccountTitle");
			string GroupIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbCustomerGroup).Text, dtGroups, "Description");
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.FromDate = fromdate.Value;
			vh.ToDate = todate.Value;
			vh.AccountId = 3;
			vh.FromDocNo = Conversion.ToInt((object)txtBalanceFrom.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtBalanceTo.Text);
			vh.Status = account;
			vh.AccountTypeIds = Conversion.ToString((object)GroupIds);
			vh.CityId = Conversion.ToInt(CmbCityName.Value);
			vh.CustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			if (ChkShowOnlyTrade.Checked)
			{
				vh.ActionId = 1;
			}
			vh.SkipZero = Conversion.ToInt((object)SkipZero.Checked);
			vh.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			if (rdAll.Checked)
			{
				vh.ShowAssetLiability = 0;
			}
			else if (RdAssests.Checked)
			{
				vh.ShowAssetLiability = 2;
			}
			else if (RdLiability.Checked)
			{
				vh.ShowAssetLiability = 3;
			}
			dtreceive = VoucherReports.ReceivablesReport(vh);
			if (dtreceive.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("BranchesId", typeof(int));
				dt.Columns.Add("BranchName", typeof(string));
				dt.Columns.Add("Id", typeof(string));
				dt.Columns.Add("ParentAccount", typeof(string));
				dt.Columns.Add("AccountClass", typeof(string));
				dt.Columns.Add("AccountCode", typeof(string));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("AccountType", typeof(string));
				dt.Columns.Add("Opening", typeof(double));
				dt.Columns.Add("ObDebit", typeof(double));
				dt.Columns.Add("ObCredit", typeof(double));
				dt.Columns.Add("CurrDebit", typeof(double));
				dt.Columns.Add("CurrCredit", typeof(double));
				dt.Columns.Add("ClDebit", typeof(double));
				dt.Columns.Add("LastBillDate", typeof(DateTime));
				dt.Columns.Add("LastBillsAmount", typeof(double));
				dt.Columns.Add("BillDays", typeof(int));
				dt.Columns.Add("LastRcvdDate", typeof(DateTime));
				dt.Columns.Add("LastRcvdAmount", typeof(double));
				dt.Columns.Add("RcvdDays", typeof(int));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("MobilePersonal", typeof(string));
				dt.Columns.Add("Title", typeof(string));
				dt.Columns.Add("FollowUpDate", typeof(DateTime));
				dtForLayout = dt.Clone();
				foreach (DataRow row in dtreceive.Rows)
				{
					dt.Rows.Add(row["AccountId"], row["BranchesId"], row["BranchName"], row["Id"], row["ParentAccount"], row["ClassName"], row["AccountCode"], row["AccountTitle"], row["AccountType"], row["Opening"], row["ObDebit"], row["ObCredit"], row["CurrDebit"], row["CurrCredit"], row["ClDebit"], row["LastBillDate"], row["LastBillsAmount"], row["BillDays"], row["LastRcvdDate"], row["LastRcvdAmount"], row["RcvdDays"], row["CityName"], row["MobilePersonal"], row["Title"], row["FollowupDate"]);
				}
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
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings()
	{
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["BranchesId"].Visible = false;
			grd.RootTable.Columns["ObDebit"].Visible = false;
			grd.RootTable.Columns["ObCredit"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grd.RootTable.Columns["LastBillDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LastRcvdDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["FollowUpDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["Opening"].Caption = "Opening Balance";
			grd.RootTable.Columns["CurrDebit"].Caption = "Debit";
			grd.RootTable.Columns["CurrCredit"].Caption = "Credit";
			grd.RootTable.Columns["ClDebit"].Caption = "Closing Balance";
			grd.RootTable.Columns["LastRcvdDate"].Caption = "Received Date";
			grd.RootTable.Columns["LastRcvdAmount"].Caption = "Received Amount";
			grd.RootTable.Columns["RcvdDays"].Caption = "Received Days";
			grd.RootTable.Columns["BillDays"].TextAlignment = (TextAlignment)2;
			grd.RootTable.Columns["RcvdDays"].TextAlignment = (TextAlignment)2;
			grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CurrDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CurrDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CurrCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CurrCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastBillsAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LastBillsAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastBillsAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastBillsAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastRcvdAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LastRcvdAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastRcvdAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastRcvdAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchesId"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["ParentAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountClass"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountType"].EditType = (EditType)0;
			grd.RootTable.Columns["Opening"].EditType = (EditType)0;
			grd.RootTable.Columns["ObDebit"].EditType = (EditType)0;
			grd.RootTable.Columns["ObCredit"].EditType = (EditType)0;
			grd.RootTable.Columns["CurrDebit"].EditType = (EditType)0;
			grd.RootTable.Columns["CurrCredit"].EditType = (EditType)0;
			grd.RootTable.Columns["ClDebit"].EditType = (EditType)0;
			grd.RootTable.Columns["LastBillDate"].EditType = (EditType)0;
			grd.RootTable.Columns["LastBillsAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["LastRcvdDate"].EditType = (EditType)0;
			grd.RootTable.Columns["LastRcvdAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["BillDays"].EditType = (EditType)0;
			grd.RootTable.Columns["RcvdDays"].EditType = (EditType)0;
			grd.RootTable.Columns["CityName"].EditType = (EditType)0;
			grd.RootTable.Columns["Title"].EditType = (EditType)0;
			grd.RootTable.Columns["FollowUpDate"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ParentAccount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccountClass"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccountType"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Opening"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ObDebit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ObCredit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["CurrDebit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["CurrCredit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ClDebit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["LastBillDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["LastBillsAmount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["LastRcvdDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["LastRcvdAmount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BillDays"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RcvdDays"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["CityName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Title"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["FollowUpDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["AccountCode"].Width = 90;
			grd.RootTable.Columns["AccountTitle"].Width = 160;
			grd.RootTable.Columns["ParentAccount"].Width = 150;
			grd.RootTable.Columns["Opening"].Width = 100;
			grd.RootTable.Columns["CurrDebit"].Width = 100;
			grd.RootTable.Columns["CurrCredit"].Width = 100;
			grd.RootTable.Columns["ClDebit"].Width = 100;
			grd.RootTable.Columns["LastBillDate"].Width = 80;
			grd.RootTable.Columns["LastBillsAmount"].Width = 90;
			grd.RootTable.Columns["LastRcvdDate"].Width = 80;
			grd.RootTable.Columns["LastRcvdAmount"].Width = 90;
			grd.RootTable.Columns["BillDays"].Width = 60;
			grd.RootTable.Columns["RcvdDays"].Width = 65;
			grd.RootTable.Columns["CityName"].Width = 120;
			grd.RootTable.Columns["Title"].Width = 120;
			grd.RootTable.Columns["FollowUpDate"].Width = 85;
			grd.RootTable.Columns["AccountClass"].Width = 80;
			grd.RootTable.Columns["AccountType"].Width = 80;
			grd.RootTable.Columns.Add("Action");
			grd.RootTable.Columns["Action"].Caption = "Action";
			grd.RootTable.Columns["Action"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Action"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Action"].ButtonText = "CellNo_Save";
			grd.RootTable.Columns["Action"].Width = 60;
			grd.RootTable.SortKeys.Add(grd.RootTable.Columns["AccountTitle"], (SortOrder)1);
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Receivables_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			UserAccount.CompanyId = ((!UserAccount.IsHeadOffice) ? UserAccount.CompanyId : ((CompanyIdFromBreakUp > 0) ? CompanyIdFromBreakUp : UserAccount.CompanyId));
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
				if (BranchesIds != null && BranchesIds != string.Empty)
				{
					((Control)(object)CmbBranches).Text = string.Empty;
					if (BranchFeatureConsolidated)
					{
						string[] valuesToMatch = BranchesIds.Split(',');
						RowEnumerator enumerator = ((UltraGridBase)CmbBranches).Rows.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								UltraGridRow row = enumerator.Current;
								string[] array = valuesToMatch;
								for (int i = 0; i < array.Length; i++)
								{
									if (Conversion.ToInt((object)array[i]) == Conversion.ToInt(row.Cells["Id"].Value))
									{
										row.Cells["Selected"].Value = true;
										break;
									}
								}
							}
						}
						finally
						{
							if (enumerator is IDisposable disposable)
							{
								disposable.Dispose();
							}
						}
					}
					else
					{
						CmbBranches.Value = Conversion.ToInt((object)BranchesIds);
					}
				}
			}
			AccountFill3rdLevel();
			CityNameFill();
			SuppplierCustomer();
			CustomeGroupsDefine();
			CustomerGroupBind();
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				fromdate.Value = Conversion.ToDateTime((object)FromDate);
				todate.Value = Conversion.ToDateTime((object)ToDate);
			}
			bindBillHistoryDetail();
			AccountFill();
			GridFill();
			txtFollowupDate.Value = DateTime.Now;
			cmbAccountType_Leave(null, null);
			DataFill();
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

	private void Receivables_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 1 && !UpdateMode)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabControl1.SelectedIndex == 1)
			{
				btnNew_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 0)
			{
				btnNew_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.U && tabControl1.SelectedIndex == 1 && UpdateMode)
		{
			btnUpdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.P && tabControl1.SelectedIndex == 0)
		{
			print_Click_1(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				tabControl1.SelectedIndex = 1;
			}
			if (tabControl1.SelectedIndex == 1)
			{
				tabControl1.SelectedIndex = 0;
			}
		}
	}

	private void bindBillHistoryDetail()
	{
		try
		{
			dtBillDetail.Columns.Clear();
			dtBillDetail.Rows.Clear();
			dtBillDetail.Columns.Add("BNo");
			dtBillDetail.Columns.Add("BillDate", typeof(DateTime));
			dtBillDetail.Columns.Add("DueDate", typeof(DateTime));
			dtBillDetail.Columns.Add("TAmount", typeof(double));
			dtBillDetail.Columns.Add("RAmount", typeof(double));
			dtBillDetail.Columns.Add("BAmount", typeof(double));
			grdBillDetail.DataSource = dtBillDetail;
			grdBillDetail.RetrieveStructure();
			BillDetailGridSettings();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BillDetailGridSettings()
	{
		try
		{
			grdBillDetail.RootTable.Columns["BNo"].EditType = (EditType)0;
			grdBillDetail.RootTable.Columns["BillDate"].EditType = (EditType)0;
			grdBillDetail.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grdBillDetail.RootTable.Columns["TAmount"].EditType = (EditType)0;
			grdBillDetail.RootTable.Columns["RAmount"].EditType = (EditType)0;
			grdBillDetail.RootTable.Columns["BAmount"].EditType = (EditType)0;
			grdBillDetail.RootTable.Columns["BNo"].FilterEditType = (FilterEditType)1;
			grdBillDetail.RootTable.Columns["BillDate"].FilterEditType = (FilterEditType)1;
			grdBillDetail.RootTable.Columns["DueDate"].FilterEditType = (FilterEditType)1;
			grdBillDetail.RootTable.Columns["TAmount"].FilterEditType = (FilterEditType)1;
			grdBillDetail.RootTable.Columns["RAmount"].FilterEditType = (FilterEditType)1;
			grdBillDetail.RootTable.Columns["BAmount"].FilterEditType = (FilterEditType)1;
			grdBillDetail.RootTable.Columns["TAmount"].AggregateFunction = (AggregateFunction)2;
			grdBillDetail.RootTable.Columns["RAmount"].AggregateFunction = (AggregateFunction)2;
			grdBillDetail.RootTable.Columns["BAmount"].AggregateFunction = (AggregateFunction)2;
			grdBillDetail.RootTable.Columns["TAmount"].TextAlignment = (TextAlignment)3;
			grdBillDetail.RootTable.Columns["RAmount"].TextAlignment = (TextAlignment)3;
			grdBillDetail.RootTable.Columns["BAmount"].TextAlignment = (TextAlignment)3;
			grdBillDetail.RootTable.Columns["TAmount"].FormatString = "0,0";
			grdBillDetail.RootTable.Columns["RAmount"].FormatString = "0,0";
			grdBillDetail.RootTable.Columns["BAmount"].FormatString = "0,0";
			grdBillDetail.RootTable.Columns["TAmount"].TotalFormatString = "#,##0.##";
			grdBillDetail.RootTable.Columns["RAmount"].TotalFormatString = "#,##0.##";
			grdBillDetail.RootTable.Columns["BAmount"].TotalFormatString = "#,##0.##";
			grdBillDetail.RootTable.Columns["BNo"].Width = 40;
			grdBillDetail.RootTable.Columns["BillDate"].Width = 80;
			grdBillDetail.RootTable.Columns["DueDate"].Width = 80;
			grdBillDetail.RootTable.Columns["TAmount"].Width = 80;
			grdBillDetail.RootTable.Columns["RAmount"].Width = 80;
			grdBillDetail.RootTable.Columns["BAmount"].Width = 80;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ShowReport()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			if (fromdate.Value.ToString() != string.Empty && todate.Value.ToString() != string.Empty)
			{
				Reporting rpt = new Reporting();
				if (UserAccount.CompAddress.ToString() == string.Empty)
				{
					rpt.RptPerameter("CompanyAddress", (string)null);
				}
				else
				{
					rpt.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				}
				if (UserAccount.CompName.ToString() == string.Empty)
				{
					rpt.RptPerameter("CompanyName", (string)null);
				}
				else
				{
					rpt.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				}
				rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				rpt.RptPerameter("@FinancialYearId", clsGlobalVariables.ActiveYr.Id.ToString());
				rpt.RptPerameter("@VoucherDateF", fromdate.Value.ToString());
				rpt.RptPerameter("@VoucherDateT", todate.Value.ToString());
				rpt.ShowReport("AcRptReceivablesWithStatus.rpt");
			}
		}
		catch (Exception)
		{
			throw;
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
				((UltraGridBase)CmbBranches).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbBranches).Text = UserAccount.BranchName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void AccountFill3rdLevel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Account_Level = 3;
			obj.AccountTypeId = 3;
			obj.AccountClassIds = "2,3";
			dtAccount = ChartofAccount.ReadAllAccountgroup(obj);
			if (dtAccount.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("AccountTitle");
				for (int i = 0; i < dtAccount.Rows.Count; i++)
				{
					dt.Rows.Add(dtAccount.Rows[i]["Id"], dtAccount.Rows[i]["AccountTitle"]);
				}
				DDL.BindDDL(dt, cmbAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbAccount).DisplayLayout.Bands[0].Columns[1].Width = 336;
				UltraGridColumn obj2 = ((UltraGridBase)cmbAccount).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbAccount.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbAccount.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbAccount.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbAccount.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomerGroupBind()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtGroups.Rows.Clear();
			CustomerGroup obj = new CustomerGroup();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtGroups = CustomerGroup.GetSupplierCustomerGroupFromInventoryStockEvaluation(obj);
			if (dtGroups.Rows.Count > 0)
			{
				DDL.BindDDL(dtGroups, cmbCustomerGroup, "Id", "Description", "CustomerGroup", ZeroIndex: false);
				UltraGridColumn obj2 = ((UltraGridBase)cmbCustomerGroup).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbCustomerGroup.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbCustomerGroup.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbCustomerGroup.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbCustomerGroup.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				AccountsPayRecFollowUpHistory accountsPayRec = new AccountsPayRecFollowUpHistory();
				if (RecId > 0)
				{
					accountsPayRec.Id = RecId;
				}
				accountsPayRec.ChartOfAccountId = Conversion.ToInt(cmbCOA.Value);
				accountsPayRec.FollowupDate = txtFollowupDate.Value;
				accountsPayRec.CommentsDetail = txtCommentDetail.Text;
				accountsPayRec.NextFollowupDays = Conversion.ToInt((object)txtNextFollowupDays.Text);
				accountsPayRec.PromiseDate = txtPromiseDate.Value;
				accountsPayRec.CommentsDate = DateTime.Now;
				accountsPayRec.OrganizationId = UserAccount.OrganizationId;
				accountsPayRec.CompanyId = UserAccount.CompanyId;
				AccountsPayRecFollowUpHistory.Save(accountsPayRec);
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Save Successfully");
				}
				FormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountFill()
	{
		try
		{
			DataTable dtGlAcc = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("", "2,4,15,11,12,10,20,21");
			if (dtGlAcc.Rows.Count > 0)
			{
				DDL.BindDDL(dtGlAcc, cmbCOA, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbCOA).DisplayLayout.Bands[0].Columns[1].Width = 276;
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
				DDL.BindDDLNew(dtCustome, CmbCustomGroup, "Id", "AcLookUpsDescription", "Custome Group", false);
				((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns[1].Width = 336;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SuppplierCustomer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			lstsupp = SupplierCustomer.GetAllList(supplier);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)cmbCOA).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Account Field Required");
			cmbCOA.Focus();
			return false;
		}
		if (txtNextFollowupDays.Text.Trim() == "")
		{
			MessageBox.Show("Follow Up Days Field Required");
			txtNextFollowupDays.Focus();
			return false;
		}
		return true;
	}

	public void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			AccountsPayRecFollowUpHistory payRecFollowUpHistory = new AccountsPayRecFollowUpHistory();
			payRecFollowUpHistory.OrganizationId = UserAccount.OrganizationId;
			payRecFollowUpHistory.CompanyId = UserAccount.CompanyId;
			dtfollowUp = AccountsPayRecFollowUpHistory.GetAll(payRecFollowUpHistory);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountId");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("FollowUpDate");
			dt.Columns.Add("NF-Days");
			dt.Columns.Add("PromiseDate");
			dt.Columns.Add("CommentDetail");
			for (int i = 0; i < dtfollowUp.Rows.Count; i++)
			{
				dt.Rows.Add(dtfollowUp.Rows[i]["Id"], dtfollowUp.Rows[i]["ChartOfAccountId"], dtfollowUp.Rows[i]["AccountTitle"], Conversion.ToDateTime(dtfollowUp.Rows[i]["FollowupDate"]).ToString("dd-MMM-yyyy"), dtfollowUp.Rows[i]["NextFollowupDays"], Conversion.ToDateTime(dtfollowUp.Rows[i]["PromiseDate"]).ToString("dd-MMM-yyyy"), dtfollowUp.Rows[i]["CommentsDetail"]);
			}
			grdReceiveable.DataSource = dt;
			grdReceiveable.RetrieveStructure();
			GridSetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void GridSetting()
	{
		try
		{
			grdReceiveable.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdReceiveable.RootTable.Columns["FollowUpDate"].EditType = (EditType)0;
			grdReceiveable.RootTable.Columns["NF-Days"].EditType = (EditType)0;
			grdReceiveable.RootTable.Columns["PromiseDate"].EditType = (EditType)0;
			grdReceiveable.RootTable.Columns["CommentDetail"].EditType = (EditType)0;
			grdReceiveable.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdReceiveable.RootTable.Columns["FollowUpDate"].FilterEditType = (FilterEditType)0;
			grdReceiveable.RootTable.Columns["NF-Days"].FilterEditType = (FilterEditType)1;
			grdReceiveable.RootTable.Columns["PromiseDate"].FilterEditType = (FilterEditType)0;
			grdReceiveable.RootTable.Columns["CommentDetail"].FilterEditType = (FilterEditType)1;
			grdReceiveable.RootTable.Columns["AccountTitle"].Width = 300;
			grdReceiveable.RootTable.Columns["FollowUpDate"].Width = 100;
			grdReceiveable.RootTable.Columns["NF-Days"].Width = 80;
			grdReceiveable.RootTable.Columns["PromiseDate"].Width = 100;
			grdReceiveable.RootTable.Columns["CommentDetail"].Width = 750;
			grdReceiveable.RootTable.Columns["Id"].Visible = false;
			grdReceiveable.RootTable.Columns["AccountId"].Visible = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdReceiverList_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt((object)grdReceiveable.CurrentRow.Cells[0].Value.ToString());
			if (dtfollowUp.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtfollowUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtfollowUp.Rows[i]["Id"]) == RecId)
				{
					txtCommentDetail.Text = dtfollowUp.Rows[i]["CommentsDetail"].ToString();
					txtFollowupDate.Value = Conversion.ToDateTime(dtfollowUp.Rows[i]["FollowupDate"]);
					txtNextFollowupDays.Text = dtfollowUp.Rows[i]["NextFollowupDays"].ToString();
					txtPromiseDate.Value = Conversion.ToDateTime(dtfollowUp.Rows[i]["PromiseDate"]);
					cmbCOA.Value = dtfollowUp.Rows[i]["ChartOfAccountId"];
					UpdateMode = true;
					btnsave.Visible = false;
					btnUpdate.Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormReset()
	{
		try
		{
			RecId = 0;
			txtCommentDetail.Text = string.Empty;
			txtFollowupDate.Value = DateTime.Now;
			txtNextFollowupDays.Text = string.Empty;
			txtPromiseDate.Value = DateTime.Now;
			((Control)(object)cmbCOA).Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			GridFill();
			cmbCOA.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNextFollowupDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtPromiseDate.Value = DateTime.Parse(txtFollowupDate.Text).AddDays(Conversion.ToInt((object)txtNextFollowupDays.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCOA_TextChanged(object sender, EventArgs e)
	{
		if (!(((Control)(object)cmbCOA).Text != string.Empty))
		{
			return;
		}
		try
		{
			if (dtfollowUp.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("AccountId");
				dt.Columns.Add("AccountTitle");
				dt.Columns.Add("FollowUpDate");
				dt.Columns.Add("NF-Days");
				dt.Columns.Add("PromiseDate");
				dt.Columns.Add("CommentDetail");
				for (int i = 0; i < dtfollowUp.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtfollowUp.Rows[i]["ChartOfAccountId"]) == Conversion.ToInt(cmbCOA.Value))
					{
						dt.Rows.Add(dtfollowUp.Rows[i]["Id"], dtfollowUp.Rows[i]["ChartOfAccountId"], dtfollowUp.Rows[i]["AccountTitle"], Conversion.ToDateTime(dtfollowUp.Rows[i]["FollowupDate"]).ToString("dd-MMM-yyyy"), dtfollowUp.Rows[i]["NextFollowupDays"], Conversion.ToDateTime(dtfollowUp.Rows[i]["PromiseDate"]).ToString("dd-MMM-yyyy"), dtfollowUp.Rows[i]["CommentsDetail"]);
					}
				}
				grdReceiveable.DataSource = dt;
				grdReceiveable.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdReceiveable.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFollowUpRefresh_Click(object sender, EventArgs e)
	{
		FormReset();
	}

	private void btnSMS_Click(object sender, EventArgs e)
	{
		try
		{
			Architecture.WinApp.SMSSystem.CreateSMSTemplate createSMSTemplate = new Architecture.WinApp.SMSSystem.CreateSMSTemplate(UserAccount);
			createSMSTemplate.ScreenName = base.Name;
			createSMSTemplate.Show();
			createSMSTemplate.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveSMS_Click(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		try
		{
			if (grd.RowCount > 0)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					if (1 == 0)
					{
						continue;
					}
					string SMSBody = string.Empty;
					string CellNo = string.Empty;
					MessageBody dtGetSMSBody = MessageBody.GetMessageBody(new MessageBody
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ScreenName = base.Name
					});
					if (dtGetSMSBody == null)
					{
						continue;
					}
					SMSParameters objSMSParam = new SMSParameters();
					objSMSParam.AccountCode = "";
					objSMSParam.AccountTitle = Conversion.ToString((object)r.Cells["Title"].Text);
					objSMSParam.DocumentNo = "";
					objSMSParam.DocumentDate = todate.Value;
					objSMSParam.OtherDocNo = "";
					objSMSParam.Remarks = "";
					objSMSParam.Amount = 0.0;
					objSMSParam.Quantity = 0.0;
					objSMSParam.ChequeNo = "";
					objSMSParam.ChequeDate = DateTime.Now;
					objSMSParam.CompanyName = UserAccount.CompName;
					objSMSParam.CellNo = CellNo;
					objSMSParam.SCS = "Senergic Corporate Solutions";
					SMSBody = dtGetSMSBody.SMSBody.ToString();
					SMSBody = SMSBody.Replace("@AccountCode", objSMSParam.AccountCode).Replace("@AccountTitle", objSMSParam.AccountTitle).Replace("@DocumentNo", objSMSParam.DocumentNo)
						.Replace("@DocumentDate", objSMSParam.DocumentDate.ToShortDateString())
						.Replace("@OtherDocNo", objSMSParam.OtherDocNo)
						.Replace("@Remarks", objSMSParam.Remarks)
						.Replace("@Amount", objSMSParam.Amount.ToString())
						.Replace("@Quantity", objSMSParam.Quantity.ToString())
						.Replace("@ChequeNo", objSMSParam.ChequeNo)
						.Replace("@ChequeDate", objSMSParam.ChequeDate.ToShortDateString())
						.Replace("@CompanyName", objSMSParam.CompanyName)
						.Replace("@SCS", objSMSParam.SCS);
					SMSBody = SMSBody.Replace("'", "''");
					DataTable dtgetcellNo = SupplierCustomer.GetIdbyGlAccountId(new SupplierCustomer
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						GlAccountId = Conversion.ToInt(r.Cells["Id"].Value)
					});
					if (dtgetcellNo.Rows.Count > 0)
					{
						for (int j = 0; j < dtgetcellNo.Rows.Count; j++)
						{
							if (dtgetcellNo.Rows[j]["MobilePersonal"].ToString() != string.Empty)
							{
								SmsHistory val = new SmsHistory
								{
									OrganizationId = UserAccount.OrganizationId,
									CompanyId = UserAccount.OrganizationId,
									SmsCellNo = dtgetcellNo.Rows[j]["MobilePersonal"].ToString()
								};
								CellNo = val.SmsCellNo;
								val.SmsDate = DateTime.Now;
								val.SendStatus = false;
								val.EntryDate = DateTime.Now;
								val.SmsSourceDoc = "";
								val.SmsBody = SMSBody.ToString();
								val.SmsSourceId = 0;
								val.EntryUser = UserAccount.ID;
								val.ScreenName = base.Name;
								val.ToUserId = Conversion.ToInt(dtgetcellNo.Rows[j]["Id"]);
								val.ReceiverId = 0;
								val.SupplierCustomerId = Conversion.ToInt(dtgetcellNo.Rows[j]["Id"]);
								SmsHistory.Save(val);
							}
						}
					}
					DataTable dtmanualNo = new DataTable();
					dtmanualNo = SMSTemplateConfigurations.GetScreensActiveUsers(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ScreenName = base.Name
					});
					if (dtmanualNo.Rows.Count <= 0)
					{
						continue;
					}
					for (int i = 0; i < dtmanualNo.Rows.Count; i++)
					{
						if (dtmanualNo.Rows[i]["MobileNo"].ToString() != string.Empty)
						{
							SmsHistory val2 = new SmsHistory
							{
								OrganizationId = UserAccount.OrganizationId,
								CompanyId = UserAccount.OrganizationId,
								SmsCellNo = dtmanualNo.Rows[i]["MobileNo"].ToString()
							};
							CellNo = val2.SmsCellNo;
							val2.SmsDate = DateTime.Now;
							val2.SendStatus = false;
							val2.EntryDate = DateTime.Now;
							val2.SmsSourceDoc = "";
							val2.SmsBody = SMSBody.ToString();
							val2.SmsSourceId = 0;
							val2.EntryUser = UserAccount.ID;
							val2.ScreenName = base.Name;
							val2.ToUserId = Conversion.ToInt(dtmanualNo.Rows[i]["Id"]);
							val2.ReceiverId = 0;
							val2.SupplierCustomerId = Conversion.ToInt(dtmanualNo.Rows[i]["Id"]);
							SmsHistory.Save(val2);
						}
					}
				}
			}
			reset();
			btnshow_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		try
		{
			if (lstsupp.Count <= 0)
			{
				return;
			}
			List<SupplierCustomer> newList = lstsupp.Where((SupplierCustomer x) => x.GlAccountId == Conversion.ToInt(grd.CurrentRow.Cells[0].Value)).ToList();
			if (newList.Count <= 0)
			{
				return;
			}
			string PurchaseNumber = "";
			List<ConfigrationsAllocation> PurchaseList = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "ReceiptsByInvoiceSalesInvoice").ToList();
			if (PurchaseList.Count > 0)
			{
				PurchaseNumber = PurchaseList[0].ConfigKey;
			}
			DataTable dtbd = VoucherHead.GetReceivablesByInvoice(new ReportsParameters
			{
				SupplierCustomerId = Conversion.ToInt((object)newList[0].Id),
				SaleInvoiceDocumentTypeIds = Conversion.ToString((object)PurchaseNumber)
			});
			if (dtbd.Rows.Count > 0)
			{
				dtBillDetail.Rows.Clear();
				for (int i = 0; i < dtbd.Rows.Count; i++)
				{
					dtBillDetail.Rows.Add(dtbd.Rows[i]["ManualBillNo"], Conversion.ToDateTime(dtbd.Rows[i]["VoucherDate"]).ToString("dd-MMM-yy"), Conversion.ToDateTime(dtbd.Rows[i]["DueDate"]).ToString("dd-MMM-yy"), dtbd.Rows[i]["BillAmount"], dtbd.Rows[i]["ReceivedPaidTotal"], dtbd.Rows[i]["BalanceAmount"]);
				}
				grdBillDetail.DataSource = dtBillDetail;
				grdBillDetail.RetrieveStructure();
				BillDetailGridSettings();
			}
			else
			{
				grdBillDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtreceive.Rows.Count == 0 && dtreceive == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress);
			val.RptPerameter("CompanyName", UserAccount.CompName);
			val.ShowReportWithDataTable(dtreceive, "122-AcRptAccounts_ReceivablesWithStatus.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbAccountType_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbAccount).Text = string.Empty;
			((UltraGridBase)cmbAccount).DataSource = null;
			AccountFill3rdLevel();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click_1(object sender, EventArgs e)
	{
		try
		{
			fromdate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			todate.Value = Conversion.ToDateTime((object)DateTime.Now);
			txtBalanceFrom.Text = string.Empty;
			txtBalanceTo.Text = string.Empty;
			((Control)(object)cmbAccount).Text = string.Empty;
			((UltraGridBase)cmbAccount).DataSource = null;
			((Control)(object)cmbCustomerGroup).Text = string.Empty;
			((Control)(object)CmbCustomGroup).Text = string.Empty;
			grd.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			int branchesId = Conversion.ToInt(currentRow.Cells["BranchesId"].Value);
			CommonServices.GoToGeneralLedgerFromLinkedEvent(Id, fromdate.Value, todate.Value, 0, branchesId);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtForLayout);
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "Action")
			{
				if (Conversion.ToInt(item.Cells["Id"].Value) == 0)
				{
					throw new Exception("Customer Id Not Found");
				}
				if (Conversion.ToString(item.Cells["MobilePersonal"].Value) == string.Empty)
				{
					throw new Exception("Mobile Personal Field Required");
				}
				if (MessageBox.Show("Are you sure to Save CellNo?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					SupplierCustomer.SupplierCustomerDataUpdateById(new SupplierCustomer
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						MobilePersonal = Conversion.ToString(item.Cells["MobilePersonal"].Value)
					});
					MessageBox.Show("Record Update Successfully");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBalanceFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBalanceTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void Receivables_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
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
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Expected O, but got Unknown
		//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b63: Expected O, but got Unknown
		//IL_0b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.Receivables));
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
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
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		GridEXLayout grdBillDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		GridEXLayout grdReceiveable_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.RdAssests = new System.Windows.Forms.RadioButton();
		this.RdLiability = new System.Windows.Forms.RadioButton();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.SkipZero = new System.Windows.Forms.CheckBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.cmbCustomerGroup = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.ChkShowOnlyTrade = new System.Windows.Forms.CheckBox();
		this.CmbCustomGroup = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbCityName = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txttest = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbAccount = new UltraCombo();
		this.grdBillDetail = new GridEX();
		this.cmbCompany = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtBalanceTo = new System.Windows.Forms.TextBox();
		this.txtBalanceFrom = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnSMS = new System.Windows.Forms.ToolStripButton();
		this.btnSmSSave = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdReceiveable = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.txtNextFollowupDays = new System.Windows.Forms.TextBox();
		this.txtPromiseDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtCommentDetail = new System.Windows.Forms.TextBox();
		this.txtFollowupDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbCOA = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnFollowUpRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdBillDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCompany).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdReceiveable).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCOA).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1126, 625);
		this.panel1.TabIndex = 0;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1126, 625);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add(this.tabControl1);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1118, 599);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Receivables Report";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(3, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1112, 593);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1104, 567);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1098, 561);
		this.panel2.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 148);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1098, 413);
		this.panel6.TabIndex = 113;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1098, 413);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel7.Controls.Add(this.Branches);
		this.panel7.Controls.Add(this.SkipZero);
		this.panel7.Controls.Add(this.panel4);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerGroup);
		this.panel7.Controls.Add(this.label11);
		this.panel7.Controls.Add(this.ChkShowOnlyTrade);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.txttest);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccount);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdBillDetail);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCompany);
		this.panel7.Controls.Add(this.label29);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.txtBalanceTo);
		this.panel7.Controls.Add(this.txtBalanceFrom);
		this.panel7.Controls.Add(this.btnshow);
		this.panel7.Controls.Add(this.todate);
		this.panel7.Controls.Add(this.fromdate);
		this.panel7.Controls.Add(this.label27);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 29);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1098, 119);
		this.panel7.TabIndex = 112;
		this.groupBox1.Controls.Add(this.rdAll);
		this.groupBox1.Controls.Add(this.RdAssests);
		this.groupBox1.Controls.Add(this.RdLiability);
		this.groupBox1.Location = new System.Drawing.Point(891, 33);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(76, 78);
		this.groupBox1.TabIndex = 11;
		this.groupBox1.TabStop = false;
		this.rdAll.AutoSize = true;
		this.rdAll.Checked = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.ForeColor = System.Drawing.Color.Black;
		this.rdAll.Location = new System.Drawing.Point(6, 8);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(45, 19);
		this.rdAll.TabIndex = 0;
		this.rdAll.TabStop = true;
		this.rdAll.Text = "ALL";
		this.rdAll.UseVisualStyleBackColor = true;
		this.RdAssests.AutoSize = true;
		this.RdAssests.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdAssests.ForeColor = System.Drawing.Color.Black;
		this.RdAssests.Location = new System.Drawing.Point(6, 28);
		this.RdAssests.Name = "RdAssests";
		this.RdAssests.Size = new System.Drawing.Size(58, 19);
		this.RdAssests.TabIndex = 1;
		this.RdAssests.Text = "Assets";
		this.RdAssests.UseVisualStyleBackColor = true;
		this.RdLiability.AutoSize = true;
		this.RdLiability.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdLiability.ForeColor = System.Drawing.Color.Black;
		this.RdLiability.Location = new System.Drawing.Point(6, 51);
		this.RdLiability.Name = "RdLiability";
		this.RdLiability.Size = new System.Drawing.Size(66, 19);
		this.RdLiability.TabIndex = 2;
		this.RdLiability.Text = "Liability";
		this.RdLiability.UseVisualStyleBackColor = true;
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
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
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
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(402, 88);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 6;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(306, 94);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 165;
		this.Branches.Text = "Branches";
		this.SkipZero.AutoSize = true;
		this.SkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SkipZero.Location = new System.Drawing.Point(750, 92);
		this.SkipZero.Name = "SkipZero";
		this.SkipZero.Size = new System.Drawing.Size(73, 19);
		this.SkipZero.TabIndex = 9;
		this.SkipZero.Text = "SkipZero";
		this.SkipZero.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1098, 28);
		this.panel4.TabIndex = 127;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1057, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 112;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(393, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Receivables Report Balance Classification Wise";
		this.cmbCustomerGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Location = new System.Drawing.Point(402, 61);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Name = "cmbCustomerGroup";
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(306, 67);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(94, 15);
		this.label11.TabIndex = 126;
		this.label11.Text = "Inventory Group";
		this.ChkShowOnlyTrade.AutoSize = true;
		this.ChkShowOnlyTrade.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkShowOnlyTrade.Location = new System.Drawing.Point(627, 92);
		this.ChkShowOnlyTrade.Name = "ChkShowOnlyTrade";
		this.ChkShowOnlyTrade.Size = new System.Drawing.Size(121, 19);
		this.ChkShowOnlyTrade.TabIndex = 9;
		this.ChkShowOnlyTrade.Text = "Only Trade Parties";
		this.ChkShowOnlyTrade.UseVisualStyleBackColor = true;
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(402, 34);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 4;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(306, 40);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(84, 15);
		this.label10.TabIndex = 124;
		this.label10.Text = "Custom Group";
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(78, 61);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 67);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(62, 15);
		this.label9.TabIndex = 122;
		this.label9.Text = "City Name";
		this.txttest.Location = new System.Drawing.Point(1265, 34);
		this.txttest.Name = "txttest";
		this.txttest.Size = new System.Drawing.Size(56, 20);
		this.txttest.TabIndex = 120;
		this.txttest.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 86);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(52, 30);
		this.label8.TabIndex = 119;
		this.label8.Text = "Control \r\nAccount";
		this.cmbAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Location = new System.Drawing.Point(78, 88);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Name = "cmbAccount";
		((System.Windows.Forms.Control)(object)this.cmbAccount).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccount).TabIndex = 3;
		this.grdBillDetail.AllowDelete = (InheritableBoolean)1;
		grdBillDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdBillDetail.DesignTimeLayout = grdBillDetail_DesignTimeLayout;
		this.grdBillDetail.DynamicFiltering = true;
		this.grdBillDetail.FilterMode = (FilterMode)2;
		this.grdBillDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBillDetail.GroupByBoxVisible = false;
		this.grdBillDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Location = new System.Drawing.Point(1161, 8);
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Name = "grdBillDetail";
		((ScrollableBase)this.grdBillDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Size = new System.Drawing.Size(98, 45);
		((System.Windows.Forms.Control)(object)this.grdBillDetail).TabIndex = 45;
		this.grdBillDetail.TotalRow = (InheritableBoolean)1;
		this.grdBillDetail.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Visible = false;
		((ScrollableBase)this.grdBillDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Leave += new System.EventHandler(txtNextFollowupDays_TextChanged);
		this.cmbCompany.AllowNull = (DefaultableBoolean)1;
		this.cmbCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.cmbCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCompany).Location = new System.Drawing.Point(1265, 9);
		((System.Windows.Forms.Control)(object)this.cmbCompany).Name = "cmbCompany";
		((System.Windows.Forms.Control)(object)this.cmbCompany).Size = new System.Drawing.Size(56, 22);
		((System.Windows.Forms.Control)(object)this.cmbCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbCompany).Visible = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(1158, 8);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(59, 15);
		this.label29.TabIndex = 106;
		this.label29.Text = "Company";
		this.label29.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(623, 67);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 15);
		this.label5.TabIndex = 115;
		this.label5.Text = "Closing To";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(623, 40);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(77, 15);
		this.label4.TabIndex = 114;
		this.label4.Text = "Closing From";
		this.txtBalanceTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceTo.Location = new System.Drawing.Point(705, 63);
		this.txtBalanceTo.Name = "txtBalanceTo";
		this.txtBalanceTo.Size = new System.Drawing.Size(180, 23);
		this.txtBalanceTo.TabIndex = 8;
		this.txtBalanceTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceTo_KeyPress);
		this.txtBalanceFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceFrom.Location = new System.Drawing.Point(705, 36);
		this.txtBalanceFrom.Name = "txtBalanceFrom";
		this.txtBalanceFrom.Size = new System.Drawing.Size(180, 23);
		this.txtBalanceFrom.TabIndex = 7;
		this.txtBalanceFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceFrom_KeyPress);
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.Location = new System.Drawing.Point(824, 88);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 26);
		this.btnshow.TabIndex = 10;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(190, 36);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(111, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(78, 36);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(111, 23);
		this.fromdate.TabIndex = 0;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 31);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 32);
		this.label27.TabIndex = 103;
		this.label27.Text = "From && To Date";
		this.panel3.Controls.Add(this.toolStrip3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1098, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.print, this.btnSMS, this.btnSmSSave });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1098, 27);
		this.toolStrip3.TabIndex = 111;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStrip3.Click += new System.EventHandler(btnNew_Click);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click_1);
		this.print.Image = (System.Drawing.Image)resources.GetObject("print.Image");
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(79, 24);
		this.print.Text = "122-Print";
		this.print.Click += new System.EventHandler(print_Click);
		this.btnSMS.Image = (System.Drawing.Image)resources.GetObject("btnSMS.Image");
		this.btnSMS.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSMS.Name = "btnSMS";
		this.btnSMS.Size = new System.Drawing.Size(102, 24);
		this.btnSMS.Text = "SMSTemplate";
		this.btnSMS.Visible = false;
		this.btnSMS.Click += new System.EventHandler(btnSMS_Click);
		this.btnSmSSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSmSSave.Name = "btnSmSSave";
		this.btnSmSSave.Size = new System.Drawing.Size(34, 24);
		this.btnSmSSave.Text = "SMS";
		this.btnSmSSave.Visible = false;
		this.btnSmSSave.Click += new System.EventHandler(btnSaveSMS_Click);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1104, 567);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Follow Up";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.panel9);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1098, 561);
		this.panel5.TabIndex = 0;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdReceiveable);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 96);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1098, 465);
		this.panel10.TabIndex = 2;
		this.grdReceiveable.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdReceiveable).BackColor = System.Drawing.Color.White;
		grdReceiveable_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdReceiveable.DesignTimeLayout = grdReceiveable_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdReceiveable.FilterMode = (FilterMode)2;
		this.grdReceiveable.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdReceiveable.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Name = "grdReceiveable";
		((ScrollableBase)this.grdReceiveable).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Size = new System.Drawing.Size(1098, 465);
		((System.Windows.Forms.Control)(object)this.grdReceiveable).TabIndex = 7;
		this.grdReceiveable.TotalRow = (InheritableBoolean)1;
		this.grdReceiveable.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdReceiveable).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).DoubleClick += new System.EventHandler(grdReceiverList_DoubleClick);
		this.panel9.Controls.Add(this.txtNextFollowupDays);
		this.panel9.Controls.Add(this.txtPromiseDate);
		this.panel9.Controls.Add(this.label1);
		this.panel9.Controls.Add(this.label3);
		this.panel9.Controls.Add(this.label12);
		this.panel9.Controls.Add(this.txtCommentDetail);
		this.panel9.Controls.Add(this.txtFollowupDate);
		this.panel9.Controls.Add(this.label2);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCOA);
		this.panel9.Controls.Add(this.label7);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 29);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1098, 67);
		this.panel9.TabIndex = 1;
		this.txtNextFollowupDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNextFollowupDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNextFollowupDays.Location = new System.Drawing.Point(439, 35);
		this.txtNextFollowupDays.Name = "txtNextFollowupDays";
		this.txtNextFollowupDays.Size = new System.Drawing.Size(130, 23);
		this.txtNextFollowupDays.TabIndex = 2;
		this.txtNextFollowupDays.TextChanged += new System.EventHandler(txtNextFollowupDays_TextChanged);
		this.txtPromiseDate.CustomFormat = "dd-MMM-yyyy";
		this.txtPromiseDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPromiseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPromiseDate.Location = new System.Drawing.Point(573, 35);
		this.txtPromiseDate.Name = "txtPromiseDate";
		this.txtPromiseDate.Size = new System.Drawing.Size(207, 23);
		this.txtPromiseDate.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(439, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(134, 17);
		this.label1.TabIndex = 124;
		this.label1.Text = "Next Follow Up Days";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(573, 10);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(89, 17);
		this.label3.TabIndex = 123;
		this.label3.Text = "Promise Date";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(786, 10);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(106, 17);
		this.label12.TabIndex = 122;
		this.label12.Text = "Comment Detail";
		this.txtCommentDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommentDetail.Location = new System.Drawing.Point(786, 35);
		this.txtCommentDetail.Name = "txtCommentDetail";
		this.txtCommentDetail.Size = new System.Drawing.Size(291, 23);
		this.txtCommentDetail.TabIndex = 4;
		this.txtFollowupDate.CustomFormat = "dd-MMM-yyyy";
		this.txtFollowupDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFollowupDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFollowupDate.Location = new System.Drawing.Point(288, 35);
		this.txtFollowupDate.Name = "txtFollowupDate";
		this.txtFollowupDate.Size = new System.Drawing.Size(146, 23);
		this.txtFollowupDate.TabIndex = 1;
		this.txtFollowupDate.Value = new System.DateTime(2020, 11, 11, 22, 22, 0, 0);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(288, 10);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(100, 17);
		this.label2.TabIndex = 121;
		this.label2.Text = "Follow Up Date";
		this.cmbCOA.AllowNull = (DefaultableBoolean)1;
		this.cmbCOA.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCOA.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCOA.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbCOA).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCOA).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCOA).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCOA).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbCOA).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCOA).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbCOA).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCOA).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbCOA).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCOA).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCOA).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCOA).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCOA.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCOA).Location = new System.Drawing.Point(7, 34);
		((System.Windows.Forms.Control)(object)this.cmbCOA).Name = "cmbCOA";
		((System.Windows.Forms.Control)(object)this.cmbCOA).Size = new System.Drawing.Size(276, 26);
		((System.Windows.Forms.Control)(object)this.cmbCOA).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbCOA).TextChanged += new System.EventHandler(cmbCOA_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 10);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(87, 17);
		this.label7.TabIndex = 120;
		this.label7.Text = "Account Title";
		this.panel8.Controls.Add(this.toolStrip1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1098, 29);
		this.panel8.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripButton2, this.btnFollowUpRefresh, this.btnsave, this.toolStripSeparator, this.btnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1098, 27);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton2.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(166, 24);
		this.toolStripButton2.Text = "Account FollowUp       ";
		this.btnFollowUpRefresh.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnFollowUpRefresh.Image = (System.Drawing.Image)resources.GetObject("btnFollowUpRefresh.Image");
		this.btnFollowUpRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFollowUpRefresh.Name = "btnFollowUpRefresh";
		this.btnFollowUpRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnFollowUpRefresh.Text = "&Refresh";
		this.btnFollowUpRefresh.Click += new System.EventHandler(btnFollowUpRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1126, 625);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "Receivables";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Receivables_FormClosing);
		base.Load += new System.EventHandler(Receivables_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Receivables_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdBillDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCompany).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdReceiveable).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCOA).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
