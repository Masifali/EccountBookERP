using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.SmartApp;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SmartApp;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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

public class AcFrmDashboard : Form
{
	public UserAccount UserAccount = new UserAccount();

	public ReportsParameters dashboard = new ReportsParameters();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtCompany = new DataTable();

	private bool IsHeadOffice;

	private string Companies = "";

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private WaitForm wf = new WaitForm();

	private IContainer components;

	private Panel panel1;

	private SaveFileDialog saveFileDialog1;

	private Panel panel2;

	private DateTimePicker txtdateto;

	private DateTimePicker txtdatefrom;

	private Label label18;

	private Label label17;

	private Button btnsearch;

	private Panel panel7;

	private PictureBox pictureBox5;

	private Label lbloutstandingpoclosing;

	private Label lbloutstandingpocurrout;

	private Label lbloutstandingpocurrin;

	private Label lbloutstandingpoop;

	private Label label12;

	private Label label13;

	private Label label14;

	private Label label15;

	private Label label16;

	private Panel panel9;

	private PictureBox pictureBox7;

	private Label lblOutstandingClosing;

	private Label lblOutstandingOut;

	private Label lblOutstandingCurrIn;

	private Label lbloutsaleorderop;

	private Label label37;

	private Label label38;

	private Label label43;

	private Label label44;

	private Label label45;

	private Panel panel8;

	private PictureBox pictureBox6;

	private Label lbloutexportorderclosing;

	private Label lbloutexportordercurrout;

	private Label lbloutexportordercurrin;

	private Label lbloutexportorderop;

	private Label label20;

	private Label label21;

	private Label label22;

	private Label label27;

	private Label label28;

	private Panel panel10;

	private PictureBox pictureBox8;

	private Label lblPhookerClosing;

	private Label lblPhookerOut;

	private Label lblPhookerIn;

	private Label lblPhookerOpening;

	private Label label29;

	private Label label30;

	private Label label35;

	private Label label36;

	private Label label46;

	private Label label8;

	internal UltraCombo cmbperemeter;

	private Panel panel15;

	private Panel panel14;

	private FlowLayoutPanel flowLayoutPanel;

	private Panel panel3;

	private Panel panel4;

	private Panel panel5;

	private Label label1;

	private FlowLayoutPanel FcyPayablesRcvablesFlowLayout;

	internal UltraCombo CmbCompany;

	private Label label2;

	internal UltraCombo CmbBranches;

	private Label Branches;

	public AcFrmDashboard(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void GenerateFcyPayablesReceivablesCards(string BranchIds)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		try
		{
			FcyPayablesRcvablesFlowLayout.Controls.Clear();
			DataTable dtdata = VoucherReports.FCYPayablesAndReceivables_Rpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = txtdatefrom.Value,
				ToDate = txtdateto.Value,
				CompanyIds = Companies,
				BranchIds = ((BranchIds != "") ? BranchIds : ""),
				ActionId = 1
			});
			if (dtdata != null && dtdata.Rows.Count > 0)
			{
				label1.Visible = true;
				List<int> distinctCards = (from row in dtdata.AsEnumerable()
					select Conversion.ToInt(row["TypeId"])).Distinct().ToList();
				FcyPayablesReceivablesHeaderCard[] Cards = new FcyPayablesReceivablesHeaderCard[distinctCards.Count()];
				int i;
				for (i = 0; i < Cards.Length; i++)
				{
					DataTable distinctData = (from row in dtdata.AsEnumerable()
						where row.Field<int>("TypeId") == Conversion.ToInt((object)distinctCards[i])
						select row).CopyToDataTable();
					Cards[i] = new FcyPayablesReceivablesHeaderCard();
					Cards[i].CardTypeId = Conversion.ToInt(distinctData.Rows[0]["TypeId"]);
					Cards[i].CardDescription = Conversion.ToString(distinctData.Rows[0]["TypeName"]);
					Cards[i].flowLayoutPanel1.Controls.Clear();
					double FcyAmount = 0.0;
					double pkrAmount = 0.0;
					double Rate = 0.0;
					FcyPayablesReceivablesInfoCard[] CardsValue = new FcyPayablesReceivablesInfoCard[distinctData.Rows.Count];
					for (int j = 0; j < distinctData.Rows.Count; j++)
					{
						CardsValue[j] = new FcyPayablesReceivablesInfoCard();
						CardsValue[j].FcyCode = Conversion.ToString(distinctData.Rows[j]["CurrencyCode"]);
						FcyAmount += Conversion.ToDouble(distinctData.Rows[j]["FcyAmount"]);
						CardsValue[j].FcyAmount = Conversion.ToDouble(distinctData.Rows[j]["FcyAmount"]).ToString(clsGlobalVariables.stringFormatbothForFcy);
						pkrAmount += Conversion.ToDouble(distinctData.Rows[j]["LcyAmount"]);
						CardsValue[j].PkrAmount = Conversion.ToDouble(distinctData.Rows[j]["LcyAmount"]).ToString(clsGlobalVariables.stringFormatboth);
						Rate += Conversion.ToDouble(distinctData.Rows[j]["LastExRate"]);
						CardsValue[j].FcyRate = Conversion.ToDouble(distinctData.Rows[j]["LastExRate"]).ToString("#,##0.####");
						Cards[i].flowLayoutPanel1.Controls.Add(CardsValue[j]);
					}
					Cards[i].TotalFcyAmount = FcyAmount.ToString(clsGlobalVariables.stringFormatbothForFcy);
					Cards[i].TotalLcyAmount = pkrAmount.ToString(clsGlobalVariables.stringFormatboth);
					Cards[i].UserControlClicked += FcyPayablesRcvablesControl_Click;
					FcyPayablesRcvablesFlowLayout.Controls.Add(Cards[i]);
				}
			}
			else
			{
				label1.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FcyPayablesRcvablesControl_Click(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		try
		{
			int TransId = ((FcyPayablesReceivablesHeaderCard)sender).CardTypeId;
			if (IsHeadOffice)
			{
				DataTable dtdata = VoucherReports.FCYPayablesAndReceivables_Rpt(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					FromDate = txtdatefrom.Value,
					ToDate = txtdateto.Value,
					CompanyIds = Companies,
					ActionId = 1,
					ActivityId = TransId
				});
				CompanyWiseAccountsDetail companyWiseAccountsDetail = new CompanyWiseAccountsDetail(UserAccount, dtdata);
				companyWiseAccountsDetail.Descrition = ((TransId == 1) ? "Export Receivables" : "Export Payables");
				companyWiseAccountsDetail.SortId = 10;
				companyWiseAccountsDetail.FromDate = txtdatefrom.Value;
				companyWiseAccountsDetail.ToDate = txtdateto.Value;
				companyWiseAccountsDetail.Show();
			}
			else
			{
				FCYPayablesAndReceivablesRpt fCYPayablesAndReceivablesRpt = new FCYPayablesAndReceivablesRpt(UserAccount);
				fCYPayablesAndReceivablesRpt.RequestedByOtherDocument = true;
				fCYPayablesAndReceivablesRpt.fromdate.Value = txtdatefrom.Value;
				fCYPayablesAndReceivablesRpt.todate.Value = txtdateto.Value;
				fCYPayablesAndReceivablesRpt.ShowDialog();
				fCYPayablesAndReceivablesRpt.BringToFront();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCards(string BranchIds)
	{
		try
		{
			flowLayoutPanel.Controls.Clear();
			dashboard.DashboardObjectName = "ExectiveAccountsDashboard";
			dashboard.OrganizationId = UserAccount.OrganizationId;
			dashboard.CompanyId = UserAccount.CompanyId;
			dashboard.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			dashboard.FromDate = txtdatefrom.Value;
			dashboard.ToDate = txtdateto.Value;
			dashboard.CompanyIds = Companies;
			dashboard.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			dashboard.ActivityId = 1;
			List<Dashboard> lst = new List<Dashboard>();
			lst = Dashboard.ExectiveDashboard(dashboard);
			if (lst.Count > 0)
			{
				AccountsDashboardCard[] AccountCardsList = new AccountsDashboardCard[lst.Count];
				for (int i = 0; i < AccountCardsList.Length; i++)
				{
					AccountCardsList[i] = new AccountsDashboardCard();
					AccountCardsList[i].Id = lst[i].SortingNo;
					AccountCardsList[i].Title = lst[i].AccountDescription;
					AccountCardsList[i].OpeningBalance = lst[i].Opening.ToString("#,#;(#,#);0");
					AccountCardsList[i].CurrentDrBalance = lst[i].CurrDr.ToString("#,#;(#,#);0");
					AccountCardsList[i].CurrentCrBalance = lst[i].CurrCr.ToString("#,#;(#,#);0");
					AccountCardsList[i].DiffValue = lst[i].DiffAmount.ToString("#,#;(#,#);0");
					AccountCardsList[i].Status = lst[i].DiffStatus.ToString();
					AccountCardsList[i].ClosingBalance = lst[i].Closing.ToString("#,#;(#,#);0");
					flowLayoutPanel.Controls.Add(AccountCardsList[i]);
					AccountCardsList[i].UserControlClicked += UserControl_Click;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		AccountsDashboardCard obj1 = (AccountsDashboardCard)sender;
		int id = obj1.Id;
		if (IsHeadOffice)
		{
			List<Dashboard> lst = Dashboard.ExectiveDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				FromDate = txtdatefrom.Value,
				ToDate = txtdateto.Value,
				ReqType = obj1.Title,
				CompanyIds = Companies,
				ActivityId = 2
			});
			CompanyWiseAccountsDetail companyWiseAccountsDetail = new CompanyWiseAccountsDetail(UserAccount, lst);
			companyWiseAccountsDetail.Descrition = obj1.Title;
			companyWiseAccountsDetail.SortId = obj1.Id;
			companyWiseAccountsDetail.FromDate = txtdatefrom.Value;
			companyWiseAccountsDetail.ToDate = txtdateto.Value;
			companyWiseAccountsDetail.Show();
			return;
		}
		string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
		string BranchIdsToSend = ((BranchIds != "") ? BranchIds : "");
		switch (id)
		{
		case 1:
			CommonServices.GoToActivitySummaryFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 0, BranchIdsToSend);
			break;
		case 2:
			CommonServices.GoToCashBalancesFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 0, BranchIdsToSend);
			break;
		case 3:
			CommonServices.GoToBankBalancesFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 0, BranchIdsToSend);
			break;
		case 4:
			CommonServices.GoToTradeCreditorsReportFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 0, chkCrAmount: true, chkDrAmount: true, BranchIdsToSend);
			break;
		case 5:
			CommonServices.GoToTradeDebitorsReportFromLinkedEvent(txtdatefrom.Value, txtdatefrom.Value, 0, chkCrAmount: true, chkDrAmount: true, BranchIdsToSend);
			break;
		case 6:
			CommonServices.GoToPayablesWithBillAmountFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 0, 0, BranchIdsToSend);
			break;
		case 7:
			CommonServices.GoToReceivablesFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 0, BranchIdsToSend);
			break;
		case 8:
			CommonServices.GoToInventoryPayablesandReceivablesFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 0, 0, BranchIdsToSend);
			break;
		case 9:
			CommonServices.GoToInventoryPayablesandReceivablesFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 1, 0, BranchIdsToSend);
			break;
		case 10:
		case 11:
			CommonServices.GoToCshAndBankBalancesReportFromLinkedEvent(txtdatefrom.Value, txtdateto.Value, 0, BranchIdsToSend);
			break;
		}
	}

	private void CompanyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Company obj = new Company();
			obj.OrgCompanyTypeId = UserAccount.OrganizationId;
			dtCompany = Company.GetAlldt(obj);
			DataTable dataTable = dtCompany;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCompany, CmbCompany, "Id", "CompName", "Company Name", false);
				if (IsHeadOffice)
				{
					UltraGridColumn obj2 = ((UltraGridBase)CmbCompany).DisplayLayout.Bands[0].Columns.Add();
					((KeyedSubObjectBase)obj2).Key = "Selected";
					((HeaderBase)obj2.Header).Caption = string.Empty;
					obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
					obj2.DataType = typeof(bool);
					((HeaderBase)obj2.Header).VisiblePosition = 0;
					CmbCompany.CheckedListSettings.CheckStateMember = "Selected";
					((EditorCheckedListSettings)CmbCompany.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
					((EditorCheckedListSettings)CmbCompany.CheckedListSettings).ListSeparator = ",";
					((EditorCheckedListSettings)CmbCompany.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				}
				else
				{
					List<Company> lst = Conversion.ConvertDataTableToList<Company>(dtCompany);
					CmbCompany.Value = Conversion.ToInt((object)lst.Find((Company item) => item.Id == UserAccount.CompanyId).Id);
					((Control)(object)CmbCompany).Enabled = false;
				}
			}
			else
			{
				((Control)(object)CmbCompany).Text = string.Empty;
				((UltraGridBase)CmbCompany).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Parameters");
			dt.Rows.Add(1, "This Day");
			dt.Rows.Add(2, "This Week");
			dt.Rows.Add(3, "This Month");
			dt.Rows.Add(4, "This Year");
			dt.Rows.Add(5, "Financial Year");
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
			((UltraGridBase)cmbperemeter).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcFrmDashboard_Load(object sender, EventArgs e)
	{
		try
		{
			IsHeadOffice = UserAccount.IsHeadOffice;
			cmbperemeter.Focus();
			label1.Visible = false;
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				Branches.Visible = false;
				((Control)(object)CmbBranches).Visible = false;
				btnsearch.Location = new Point(653, 24);
			}
			ParameterFill();
			CompanyFill();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			GenerateCards(BranchIds);
			GenerateFcyPayablesReceivablesCards(BranchIds);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsearch_Click(object sender, EventArgs e)
	{
		try
		{
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			GenerateCards(BranchIds);
			GenerateFcyPayablesReceivablesCards(BranchIds);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				txtdatefrom.Value = DateTime.Today;
			}
			if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				txtdatefrom.Value = DateTime.Today.AddDays(-7.0);
			}
			if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first2 = now.Date.AddDays(-(now.Date.Day - 1));
				txtdatefrom.Value = first2;
				txtdateto.Value = DateTime.Today;
			}
			if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first = new DateTime(year, 1, 1);
				txtdatefrom.Value = first;
				txtdateto.Value = DateTime.Today;
			}
			if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				txtdatefrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcFrmDashboard_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CmbCompany_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Companies = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)CmbCompany).Text, dtCompany, "CompName");
			if (BranchFeature)
			{
				DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, 0, Companies, 0);
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
				else
				{
					((Control)(object)CmbBranches).Text = string.Empty;
					((UltraGridBase)CmbBranches).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbBranches).Text = string.Empty;
				((UltraGridBase)CmbBranches).DataSource = null;
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
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
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
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.AcFrmDashboard));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.FcyPayablesRcvablesFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.CmbCompany = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.btnsearch = new System.Windows.Forms.Button();
		this.cmbperemeter = new UltraCombo();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.pictureBox8 = new System.Windows.Forms.PictureBox();
		this.lblPhookerClosing = new System.Windows.Forms.Label();
		this.lblPhookerOut = new System.Windows.Forms.Label();
		this.lblPhookerIn = new System.Windows.Forms.Label();
		this.lblPhookerOpening = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.pictureBox7 = new System.Windows.Forms.PictureBox();
		this.lblOutstandingClosing = new System.Windows.Forms.Label();
		this.lblOutstandingOut = new System.Windows.Forms.Label();
		this.lblOutstandingCurrIn = new System.Windows.Forms.Label();
		this.lbloutsaleorderop = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.pictureBox6 = new System.Windows.Forms.PictureBox();
		this.lbloutexportorderclosing = new System.Windows.Forms.Label();
		this.lbloutexportordercurrout = new System.Windows.Forms.Label();
		this.lbloutexportordercurrin = new System.Windows.Forms.Label();
		this.lbloutexportorderop = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.pictureBox5 = new System.Windows.Forms.PictureBox();
		this.lbloutstandingpoclosing = new System.Windows.Forms.Label();
		this.lbloutstandingpocurrout = new System.Windows.Forms.Label();
		this.lbloutstandingpocurrin = new System.Windows.Forms.Label();
		this.lbloutstandingpoop = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox8).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox7).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox6).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.FromArgb(245, 249, 252);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1011, 749);
		this.panel1.TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.panel15);
		this.panel2.Controls.Add(this.panel14);
		this.panel2.Controls.Add(this.panel10);
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel8);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1011, 749);
		this.panel2.TabIndex = 0;
		this.panel15.Controls.Add(this.panel4);
		this.panel15.Controls.Add(this.panel3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 54);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1009, 693);
		this.panel15.TabIndex = 42;
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 367);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1009, 326);
		this.panel4.TabIndex = 4;
		this.panel5.Controls.Add(this.FcyPayablesRcvablesFlowLayout);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1008, 326);
		this.panel5.TabIndex = 0;
		this.FcyPayablesRcvablesFlowLayout.BackColor = System.Drawing.SystemColors.Control;
		this.FcyPayablesRcvablesFlowLayout.Dock = System.Windows.Forms.DockStyle.Left;
		this.FcyPayablesRcvablesFlowLayout.Location = new System.Drawing.Point(0, 27);
		this.FcyPayablesRcvablesFlowLayout.Name = "FcyPayablesRcvablesFlowLayout";
		this.FcyPayablesRcvablesFlowLayout.Size = new System.Drawing.Size(1008, 299);
		this.FcyPayablesRcvablesFlowLayout.TabIndex = 1;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(1008, 27);
		this.label1.TabIndex = 0;
		this.label1.Text = "Fcy Receivables Payables";
		this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.panel3.Controls.Add(this.flowLayoutPanel);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1009, 367);
		this.panel3.TabIndex = 3;
		this.flowLayoutPanel.AutoScroll = true;
		this.flowLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
		this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
		this.flowLayoutPanel.Name = "flowLayoutPanel";
		this.flowLayoutPanel.Size = new System.Drawing.Size(1008, 367);
		this.flowLayoutPanel.TabIndex = 0;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel14.Controls.Add(this.Branches);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompany);
		this.panel14.Controls.Add(this.label2);
		this.panel14.Controls.Add(this.btnsearch);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel14.Controls.Add(this.txtdatefrom);
		this.panel14.Controls.Add(this.label8);
		this.panel14.Controls.Add(this.txtdateto);
		this.panel14.Controls.Add(this.label17);
		this.panel14.Controls.Add(this.label18);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1009, 54);
		this.panel14.TabIndex = 0;
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(653, 24);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 4;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(654, 5);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 594;
		this.Branches.Text = "Branches";
		this.CmbCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCompany).Location = new System.Drawing.Point(434, 24);
		((System.Windows.Forms.Control)(object)this.CmbCompany).Name = "CmbCompany";
		((System.Windows.Forms.Control)(object)this.CmbCompany).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompany).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCompany).TextChanged += new System.EventHandler(CmbCompany_TextChanged);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(437, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 45;
		this.label2.Text = "Company";
		this.btnsearch.BackColor = System.Drawing.Color.Teal;
		this.btnsearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnsearch.FlatAppearance.BorderSize = 0;
		this.btnsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnsearch.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsearch.ForeColor = System.Drawing.Color.White;
		this.btnsearch.Location = new System.Drawing.Point(871, 24);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(69, 26);
		this.btnsearch.TabIndex = 3;
		this.btnsearch.Text = "Search";
		this.btnsearch.UseVisualStyleBackColor = false;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(8, 24);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_SelectedIndexChanged);
		this.txtdatefrom.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.CustomFormat = "dd-MMM-yyy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(166, 26);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(134, 23);
		this.txtdatefrom.TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(11, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(76, 19);
		this.label8.TabIndex = 39;
		this.label8.Text = "Perameter";
		this.txtdateto.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.CustomFormat = "dd-MMM-yyy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(302, 26);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(131, 23);
		this.txtdateto.TabIndex = 2;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(169, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 19);
		this.label17.TabIndex = 12;
		this.label17.Text = "Date From";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(304, 3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(59, 19);
		this.label18.TabIndex = 13;
		this.label18.Text = "Date To";
		this.panel10.BackColor = System.Drawing.Color.GreenYellow;
		this.panel10.Controls.Add(this.pictureBox8);
		this.panel10.Controls.Add(this.lblPhookerClosing);
		this.panel10.Controls.Add(this.lblPhookerOut);
		this.panel10.Controls.Add(this.lblPhookerIn);
		this.panel10.Controls.Add(this.lblPhookerOpening);
		this.panel10.Controls.Add(this.label29);
		this.panel10.Controls.Add(this.label30);
		this.panel10.Controls.Add(this.label35);
		this.panel10.Controls.Add(this.label36);
		this.panel10.Controls.Add(this.label46);
		this.panel10.Location = new System.Drawing.Point(1567, 560);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(65, 55);
		this.panel10.TabIndex = 32;
		this.panel10.Visible = false;
		this.pictureBox8.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
		this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pictureBox8.Image = (System.Drawing.Image)resources.GetObject("pictureBox8.Image");
		this.pictureBox8.InitialImage = (System.Drawing.Image)resources.GetObject("pictureBox8.InitialImage");
		this.pictureBox8.Location = new System.Drawing.Point(11, 53);
		this.pictureBox8.Name = "pictureBox8";
		this.pictureBox8.Size = new System.Drawing.Size(57, 58);
		this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox8.TabIndex = 4;
		this.pictureBox8.TabStop = false;
		this.lblPhookerClosing.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPhookerClosing.Location = new System.Drawing.Point(180, 117);
		this.lblPhookerClosing.Name = "lblPhookerClosing";
		this.lblPhookerClosing.Size = new System.Drawing.Size(112, 22);
		this.lblPhookerClosing.TabIndex = 27;
		this.lblPhookerClosing.Text = " ";
		this.lblPhookerClosing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPhookerOut.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPhookerOut.Location = new System.Drawing.Point(180, 91);
		this.lblPhookerOut.Name = "lblPhookerOut";
		this.lblPhookerOut.Size = new System.Drawing.Size(112, 22);
		this.lblPhookerOut.TabIndex = 26;
		this.lblPhookerOut.Text = " ";
		this.lblPhookerOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPhookerIn.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPhookerIn.Location = new System.Drawing.Point(180, 65);
		this.lblPhookerIn.Name = "lblPhookerIn";
		this.lblPhookerIn.Size = new System.Drawing.Size(112, 22);
		this.lblPhookerIn.TabIndex = 25;
		this.lblPhookerIn.Text = " ";
		this.lblPhookerIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPhookerOpening.AutoEllipsis = true;
		this.lblPhookerOpening.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPhookerOpening.Location = new System.Drawing.Point(180, 39);
		this.lblPhookerOpening.Name = "lblPhookerOpening";
		this.lblPhookerOpening.Size = new System.Drawing.Size(112, 22);
		this.lblPhookerOpening.TabIndex = 24;
		this.lblPhookerOpening.Text = " ";
		this.lblPhookerOpening.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(81, 119);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(52, 17);
		this.label29.TabIndex = 23;
		this.label29.Text = "Balance";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(81, 93);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(82, 17);
		this.label30.TabIndex = 22;
		this.label30.Text = "Dispatch Qty";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(81, 67);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(72, 17);
		this.label35.TabIndex = 21;
		this.label35.Text = "Vehicle Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(81, 42);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(58, 17);
		this.label36.TabIndex = 20;
		this.label36.Text = "Opening";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(37, 7);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(188, 21);
		this.label46.TabIndex = 3;
		this.label46.Text = "Outstanding Sale Order";
		this.panel9.BackColor = System.Drawing.Color.MediumSeaGreen;
		this.panel9.Controls.Add(this.pictureBox7);
		this.panel9.Controls.Add(this.lblOutstandingClosing);
		this.panel9.Controls.Add(this.lblOutstandingOut);
		this.panel9.Controls.Add(this.lblOutstandingCurrIn);
		this.panel9.Controls.Add(this.lbloutsaleorderop);
		this.panel9.Controls.Add(this.label37);
		this.panel9.Controls.Add(this.label38);
		this.panel9.Controls.Add(this.label43);
		this.panel9.Controls.Add(this.label44);
		this.panel9.Controls.Add(this.label45);
		this.panel9.Location = new System.Drawing.Point(1499, 560);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(63, 55);
		this.panel9.TabIndex = 31;
		this.panel9.Visible = false;
		this.pictureBox7.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
		this.pictureBox7.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pictureBox7.Image = (System.Drawing.Image)resources.GetObject("pictureBox7.Image");
		this.pictureBox7.InitialImage = (System.Drawing.Image)resources.GetObject("pictureBox7.InitialImage");
		this.pictureBox7.Location = new System.Drawing.Point(11, 53);
		this.pictureBox7.Name = "pictureBox7";
		this.pictureBox7.Size = new System.Drawing.Size(57, 58);
		this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox7.TabIndex = 4;
		this.pictureBox7.TabStop = false;
		this.lblOutstandingClosing.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOutstandingClosing.Location = new System.Drawing.Point(180, 117);
		this.lblOutstandingClosing.Name = "lblOutstandingClosing";
		this.lblOutstandingClosing.Size = new System.Drawing.Size(112, 22);
		this.lblOutstandingClosing.TabIndex = 27;
		this.lblOutstandingClosing.Text = " ";
		this.lblOutstandingClosing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutstandingOut.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOutstandingOut.Location = new System.Drawing.Point(180, 91);
		this.lblOutstandingOut.Name = "lblOutstandingOut";
		this.lblOutstandingOut.Size = new System.Drawing.Size(112, 22);
		this.lblOutstandingOut.TabIndex = 26;
		this.lblOutstandingOut.Text = " ";
		this.lblOutstandingOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutstandingCurrIn.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOutstandingCurrIn.Location = new System.Drawing.Point(180, 65);
		this.lblOutstandingCurrIn.Name = "lblOutstandingCurrIn";
		this.lblOutstandingCurrIn.Size = new System.Drawing.Size(112, 22);
		this.lblOutstandingCurrIn.TabIndex = 25;
		this.lblOutstandingCurrIn.Text = " ";
		this.lblOutstandingCurrIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbloutsaleorderop.AutoEllipsis = true;
		this.lbloutsaleorderop.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutsaleorderop.Location = new System.Drawing.Point(180, 39);
		this.lbloutsaleorderop.Name = "lbloutsaleorderop";
		this.lbloutsaleorderop.Size = new System.Drawing.Size(112, 22);
		this.lbloutsaleorderop.TabIndex = 24;
		this.lbloutsaleorderop.Text = " ";
		this.lbloutsaleorderop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(81, 119);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(52, 17);
		this.label37.TabIndex = 23;
		this.label37.Text = "Balance";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(81, 93);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(82, 17);
		this.label38.TabIndex = 22;
		this.label38.Text = "Dispatch Qty";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(81, 67);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(67, 17);
		this.label43.TabIndex = 21;
		this.label43.Text = "Order Qty";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(81, 42);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(58, 17);
		this.label44.TabIndex = 20;
		this.label44.Text = "Opening";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(37, 7);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(188, 21);
		this.label45.TabIndex = 3;
		this.label45.Text = "Outstanding Sale Order";
		this.panel8.BackColor = System.Drawing.Color.DeepSkyBlue;
		this.panel8.Controls.Add(this.pictureBox6);
		this.panel8.Controls.Add(this.lbloutexportorderclosing);
		this.panel8.Controls.Add(this.lbloutexportordercurrout);
		this.panel8.Controls.Add(this.lbloutexportordercurrin);
		this.panel8.Controls.Add(this.lbloutexportorderop);
		this.panel8.Controls.Add(this.label20);
		this.panel8.Controls.Add(this.label21);
		this.panel8.Controls.Add(this.label22);
		this.panel8.Controls.Add(this.label27);
		this.panel8.Controls.Add(this.label28);
		this.panel8.Location = new System.Drawing.Point(1567, 505);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(70, 55);
		this.panel8.TabIndex = 31;
		this.panel8.Visible = false;
		this.pictureBox6.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
		this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pictureBox6.Image = (System.Drawing.Image)resources.GetObject("pictureBox6.Image");
		this.pictureBox6.InitialImage = (System.Drawing.Image)resources.GetObject("pictureBox6.InitialImage");
		this.pictureBox6.Location = new System.Drawing.Point(11, 53);
		this.pictureBox6.Name = "pictureBox6";
		this.pictureBox6.Size = new System.Drawing.Size(57, 58);
		this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox6.TabIndex = 4;
		this.pictureBox6.TabStop = false;
		this.lbloutexportorderclosing.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutexportorderclosing.Location = new System.Drawing.Point(174, 117);
		this.lbloutexportorderclosing.Name = "lbloutexportorderclosing";
		this.lbloutexportorderclosing.Size = new System.Drawing.Size(118, 22);
		this.lbloutexportorderclosing.TabIndex = 27;
		this.lbloutexportorderclosing.Text = " ";
		this.lbloutexportorderclosing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbloutexportordercurrout.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutexportordercurrout.Location = new System.Drawing.Point(174, 91);
		this.lbloutexportordercurrout.Name = "lbloutexportordercurrout";
		this.lbloutexportordercurrout.Size = new System.Drawing.Size(118, 22);
		this.lbloutexportordercurrout.TabIndex = 26;
		this.lbloutexportordercurrout.Text = " ";
		this.lbloutexportordercurrout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbloutexportordercurrin.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutexportordercurrin.Location = new System.Drawing.Point(174, 65);
		this.lbloutexportordercurrin.Name = "lbloutexportordercurrin";
		this.lbloutexportordercurrin.Size = new System.Drawing.Size(118, 22);
		this.lbloutexportordercurrin.TabIndex = 25;
		this.lbloutexportordercurrin.Text = " ";
		this.lbloutexportordercurrin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbloutexportorderop.AutoEllipsis = true;
		this.lbloutexportorderop.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutexportorderop.Location = new System.Drawing.Point(174, 39);
		this.lbloutexportorderop.Name = "lbloutexportorderop";
		this.lbloutexportorderop.Size = new System.Drawing.Size(118, 22);
		this.lbloutexportorderop.TabIndex = 24;
		this.lbloutexportorderop.Text = " ";
		this.lbloutexportorderop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(81, 119);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(52, 17);
		this.label20.TabIndex = 23;
		this.label20.Text = "Balance";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(81, 93);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(70, 17);
		this.label21.TabIndex = 22;
		this.label21.Text = "Export Qty";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(81, 67);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(67, 17);
		this.label22.TabIndex = 21;
		this.label22.Text = "Order Qty";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(81, 42);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(58, 17);
		this.label27.TabIndex = 20;
		this.label27.Text = "Opening";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(37, 7);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(206, 21);
		this.label28.TabIndex = 3;
		this.label28.Text = "Outstanding Export Order";
		this.panel7.BackColor = System.Drawing.Color.Thistle;
		this.panel7.Controls.Add(this.pictureBox5);
		this.panel7.Controls.Add(this.lbloutstandingpoclosing);
		this.panel7.Controls.Add(this.lbloutstandingpocurrout);
		this.panel7.Controls.Add(this.lbloutstandingpocurrin);
		this.panel7.Controls.Add(this.lbloutstandingpoop);
		this.panel7.Controls.Add(this.label12);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.label15);
		this.panel7.Controls.Add(this.label16);
		this.panel7.Location = new System.Drawing.Point(1494, 505);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(67, 55);
		this.panel7.TabIndex = 30;
		this.panel7.Visible = false;
		this.pictureBox5.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
		this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pictureBox5.Image = (System.Drawing.Image)resources.GetObject("pictureBox5.Image");
		this.pictureBox5.InitialImage = (System.Drawing.Image)resources.GetObject("pictureBox5.InitialImage");
		this.pictureBox5.Location = new System.Drawing.Point(11, 53);
		this.pictureBox5.Name = "pictureBox5";
		this.pictureBox5.Size = new System.Drawing.Size(57, 58);
		this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox5.TabIndex = 4;
		this.pictureBox5.TabStop = false;
		this.lbloutstandingpoclosing.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutstandingpoclosing.Location = new System.Drawing.Point(174, 117);
		this.lbloutstandingpoclosing.Name = "lbloutstandingpoclosing";
		this.lbloutstandingpoclosing.Size = new System.Drawing.Size(118, 22);
		this.lbloutstandingpoclosing.TabIndex = 27;
		this.lbloutstandingpoclosing.Text = " ";
		this.lbloutstandingpoclosing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbloutstandingpocurrout.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutstandingpocurrout.Location = new System.Drawing.Point(171, 91);
		this.lbloutstandingpocurrout.Name = "lbloutstandingpocurrout";
		this.lbloutstandingpocurrout.Size = new System.Drawing.Size(121, 22);
		this.lbloutstandingpocurrout.TabIndex = 26;
		this.lbloutstandingpocurrout.Text = " ";
		this.lbloutstandingpocurrout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbloutstandingpocurrin.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutstandingpocurrin.Location = new System.Drawing.Point(174, 65);
		this.lbloutstandingpocurrin.Name = "lbloutstandingpocurrin";
		this.lbloutstandingpocurrin.Size = new System.Drawing.Size(118, 22);
		this.lbloutstandingpocurrin.TabIndex = 25;
		this.lbloutstandingpocurrin.Text = " ";
		this.lbloutstandingpocurrin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbloutstandingpoop.AutoEllipsis = true;
		this.lbloutstandingpoop.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbloutstandingpoop.Location = new System.Drawing.Point(174, 39);
		this.lbloutstandingpoop.Name = "lbloutstandingpoop";
		this.lbloutstandingpoop.Size = new System.Drawing.Size(118, 22);
		this.lbloutstandingpoop.TabIndex = 24;
		this.lbloutstandingpoop.Text = " ";
		this.lbloutstandingpoop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(81, 119);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 17);
		this.label12.TabIndex = 23;
		this.label12.Text = "Balance";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(81, 93);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(84, 17);
		this.label13.TabIndex = 22;
		this.label13.Text = "Received Qty";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(81, 67);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(67, 17);
		this.label14.TabIndex = 21;
		this.label14.Text = "Order Qty";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(81, 42);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(58, 17);
		this.label15.TabIndex = 20;
		this.label15.Text = "Opening";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(37, 7);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(225, 21);
		this.label16.TabIndex = 3;
		this.label16.Text = "Outstanding Purchase Order";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1011, 749);
		base.Controls.Add(this.panel1);
		base.Name = "AcFrmDashboard";
		this.Text = "Accounts Dashboard";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AcFrmDashboard_FormClosing);
		base.Load += new System.EventHandler(AcFrmDashboard_Load);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox8).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox7).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox6).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
		base.ResumeLayout(false);
	}
}

}
