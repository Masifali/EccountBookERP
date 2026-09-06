using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using Newtonsoft.Json;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class GeneralLedger : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool FilterRecordsByDocType;

	public static int GeneralLedgerId = 0;

	public static int ItemLedgerId = 0;

	public static int ItemLedgerItemId = 0;

	public static int SupplierCustomerId = 0;

	private int PointsRate = 2;

	private int PointsAmount = 2;

	private bool DefaulLedgerDetail;

	private bool DefaulLedgerSummaryOne;

	private bool DefaulLedgerSummaryTwo;

	private bool AllowEditOnVoucherNoinReports;

	private bool IsStockReservedPerParty;

	public static DateTime FromDate = DateTime.Now;

	public static DateTime ToDate = DateTime.Now;

	private DataTable dtCommissionAgent = new DataTable();

	private DataTable dtItemLedger = new DataTable();

	private DataTable dtCustomerledger = new DataTable();

	private DataTable dtFcyLedger = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtSummary1 = new DataTable();

	private DataTable dtSummary2 = new DataTable();

	private DataTable dtBankSummLedger = new DataTable();

	private DataTable dtAccountTypes = new DataTable();

	private DataTable dtForDetailGrid = new DataTable();

	private DataTable dtForSummary1 = new DataTable();

	private DataTable dtForSummary2 = new DataTable();

	private DataTable dtMultiDetail = new DataTable();

	private DataTable dtMultiSummary1 = new DataTable();

	private DataTable dtMultiSummary2 = new DataTable();

	private DataTable dtGroupAccounts = new DataTable();

	private DataTable dtCustomGroup = new DataTable();

	private DataTable dtDetailByDocTypes = new DataTable();

	private DataTable dtSummary1ByDocTypes = new DataTable();

	private DataTable dtSummary2ByDocTypes = new DataTable();

	private DataTable dtForDocumentTypeList = new DataTable();

	private bool FormInitializedForDetail;

	private bool FormInitializedForSummary1;

	private bool FormInitializedForSummary2;

	private bool BranchFeature;

	private bool AfterCloseUp;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPageGeneralLedger;

	private Panel panel2;

	internal GridEX grd;

	private CheckBox ChkBoxPost;

	private Button btnshow;

	internal UltraCombo cmbproject;

	internal UltraCombo cmbAccountTitle;

	private Label label2;

	private Label label30;

	private Label label33;

	private Label label27;

	private Label label1;

	private Label label23;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print105;

	private ToolStripButton btnPrint106;

	private ToolStripButton btnQuantativeLedger;

	private ToolStripButton btnSummeryLedger;

	private TabPage tabPageCommissionAgent;

	private TabPage tabPageCustomerLedger;

	internal UltraCombo comSupplier;

	private TabPage tabPageFcyGeneralLedger;

	private TabPage tabPageItemLedger;

	private Panel panel30;

	private Panel panel28;

	internal GridEX grdItemLedger;

	private Panel panel29;

	private DateTimePicker txtToDateItemLedger;

	private DateTimePicker txtFromDateItemLedger;

	private Button btnShowItemLedger;

	internal UltraCombo cmbStockAccountItemLedger;

	private Label label16;

	private Label label18;

	private Label label19;

	private Panel panel27;

	private Label label17;

	private Panel panel26;

	private CtrlGrdBar ctrlGrdBar5;

	private ToolStrip toolStrip5;

	private ToolStripButton btnNewItemLedger;

	private ToolStripButton btnRegisterItemLedger;

	private ToolStripButton btnItemLedger_164;

	private ToolStripButton btnSummary2;

	internal UltraCombo CmbItemNameItmLedger;

	private Label label11;

	private GroupBox gbAccountDetail;

	private Label lblAccountType;

	private Label lblAccountClass;

	private Label lblClosingBal;

	private Label lblCurrCredit;

	private Label lblCurrDebit;

	private Label lblOpening;

	private Label label25;

	private Label label24;

	private Label label22;

	private Label label21;

	private Label label28;

	private Label label26;

	private Label label32;

	private Label lblAccountCode;

	internal UltraCombo CmbSubsidiaryAccount;

	private Label lblSubsidiaryAccount;

	private ToolStripButton btnSaveCheckedRows;

	private ToolStripButton rBForCheckAllPreviousRows;

	private RadioButton ChkSummery2;

	private RadioButton ChkSummery1;

	private RadioButton ChkDetail;

	private TabControl tabControl2;

	private TabPage tabSingleAccountLedger;

	private Panel panel14;

	private Panel panel3;

	private TabPage tabMultiAccountsLedger;

	private Panel panel15;

	private GroupBox groupBox1;

	private ToolStrip toolStrip2;

	private ToolStripButton btnRefresh1;

	private ToolStripButton btn105MultiPrint;

	private ToolStripButton btn106MultiPrint;

	private ToolStripButton btn107MultiPrint;

	private ToolStripButton btn108MultiPrint;

	private ToolStripButton btn105AMultiPrint;

	private Panel panel4;

	private Label label4;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel5;

	private GroupBox groupBox2;

	private RadioButton rdSummary2Print;

	private RadioButton rdSummary1Print;

	private RadioButton rdDetailPrint;

	internal UltraCombo cmbCustomGroup;

	private Label label7;

	private CheckBox chkIncludeUnpostedVouchers;

	private Label label29;

	internal UltraCombo CmbGroupAccount;

	private Button btnGetAccounts;

	private Panel panel6;

	private Label label5;

	private Label label31;

	private DateTimePicker ToDateForMulti;

	private Label label10;

	private DateTimePicker FromDateForMulti;

	private Panel panel16;

	private Label label34;

	private GroupBox groupBox3;

	private Button btnShowMultiLedger;

	private Label label35;

	private DateTimePicker datToDate;

	internal GridEX grdAccounts;

	internal GridEX grdMultiAccountsLedger;

	private Label label36;

	internal UltraCombo CmbDateTypeMulti;

	private Label label37;

	internal UltraCombo CmbDateType;

	private ToolStripButton btnReset;

	private ToolStripButton btnShortcutKey1;

	private ToolStripButton btnShortCutKey;

	public DateTimePicker txtToDate;

	public DateTimePicker txtFromDate;

	private GroupBox SubsidiaryBalanceBox;

	private Label SubsidiaryAccountName;

	private Label lblSubsidiaryBalance;

	private TabPage tabBankSummaryLedger;

	private Panel panel17;

	private GroupBox groupBox4;

	private Label label38;

	internal UltraCombo CmbDateTypeBankSummLedger;

	public DateTimePicker txttoDateBankSummLedger;

	private Label label39;

	public DateTimePicker txtFromDateBankSummLedger;

	private Label label41;

	private Button BtnShowBankSummLedger;

	private Panel panel18;

	internal GridEX grdDetailBankSummLedger;

	private CtrlGrdBar ctrlGrdBar6;

	private Label label40;

	private Panel panel19;

	internal GridEX grdAccountsBankSummLedger;

	private Label label42;

	private Panel panel31;

	private Label label43;

	private ToolStrip toolStrip6;

	private ToolStripButton BtnNewBanksummLegder;

	private ToolStripButton BtnPrintBanksummLegder;

	private ToolStripButton BtnShortCutKeysBanksummLegder;

	private Panel panel32;

	internal GridEX grdSummaryValuesBanksummLegder;

	private Label label44;

	private ToolStripButton BtnBankSummaryPrint02;

	private Label label45;

	internal UltraCombo CmbAccountTypesForBankSummaryLedger;

	private Label ValuePostDatedCheqAmount;

	private Label lblPostDatedCheqAmount;

	private Label ValuePostDatedNoOfCheqs;

	private Label lblPostedNoOfCheqs;

	private Panel PanelAccountDetail;

	private CtrlGrdBar ctrlGrdBar7;

	private Button BtnShowFilteredRecords;

	internal GridEX GrdDocType;

	private ToolStripButton BtnPrintSelectedDocSummary2;

	private ToolStripButton BtnPrintSelectedDocSumm1;

	private ToolStripButton BtnPrintSelectedDocDetail;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	private Panel rdPanel;

	private GroupBox gbAccountOtherInfo;

	private Label lblPdcReceipts;

	private Label label48;

	private Label txtPendingPrematureReceipts;

	private Label label47;

	private Label label51;

	private Label lblPdcPayments;

	private Label txtPdcPayments;

	private Label txtPdcReceipts;

	private Label txtBalAfterPreReceiptsClearance;

	private Label txtBalAfterCheqClearance;

	private Panel panel33;

	private Label txtPendingPrePayments;

	private Label label52;

	private Label lblBranch;

	private Label lblBranchBalance;

	private Label label46;

	public UltraCombo cmbbranch;

	internal UltraCombo CmbCostCenter;

	private Label lblCostCenter;

	private RadioButton RdAcByCode;

	private RadioButton RdAcByName;

	private GroupBox gbBranchesWithBalance;

	internal GridEX grdBranchesWithBalance;

	public GeneralLedger(UserAccount user)
	{
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			FormInitializedForDetail = true;
			FormInitializedForSummary1 = true;
			FormInitializedForSummary2 = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1350, 550);
			MultiLanguagesGetAll();
			gbAccountDetail.Visible = false;
			gbAccountOtherInfo.Visible = false;
			print105.Visible = false;
			btnPrint106.Visible = false;
			btnQuantativeLedger.Visible = false;
			btnSummeryLedger.Visible = false;
			btnSummary2.Visible = false;
			btn105MultiPrint.Visible = false;
			btn105AMultiPrint.Visible = false;
			btn106MultiPrint.Visible = false;
			btn107MultiPrint.Visible = false;
			btn108MultiPrint.Visible = false;
			ChkDetail.Checked = true;
			List<ConfigrationsAllocation> configList = clsGlobalVariables.configrationsAllocation;
			ConfigrationsAllocation obj = configList.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "IsStockReservedPerParty");
			IsStockReservedPerParty = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			DataTable dtConfiq = CommonServices.GetMultipleConfigurationsByConfigDescriptions("Default NoofDecimal Points For Rate,Default NoofDecimal Points For Amount,DefaulLedgerDetail,DefaulLedgerSummaryOne,DefaulLedgerSummaryTwo,AllowEditOnVoucherNoinReports,FilterRecordsByDocumentTypeOnGeneralLedger");
			if (dtConfiq.Rows.Count > 0)
			{
				for (int i = 0; i < dtConfiq.Rows.Count; i++)
				{
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "Default NoofDecimal Points For Rate")
					{
						PointsRate = Conversion.ToInt(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "Default NoofDecimal Points For Amount")
					{
						PointsAmount = Conversion.ToInt(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "DefaulLedgerDetail")
					{
						DefaulLedgerDetail = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "DefaulLedgerSummaryOne")
					{
						DefaulLedgerSummaryOne = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "DefaulLedgerSummaryTwo")
					{
						DefaulLedgerSummaryTwo = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "AllowEditOnVoucherNoinReports")
					{
						AllowEditOnVoucherNoinReports = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "FilterRecordsByDocumentTypeOnGeneralLedger")
					{
						FilterRecordsByDocType = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
				}
			}
			if (FilterRecordsByDocType)
			{
				((Control)(object)GrdDocType).Visible = true;
				BtnShowFilteredRecords.Visible = true;
			}
			else
			{
				((Control)(object)GrdDocType).Visible = false;
				BtnShowFilteredRecords.Visible = false;
			}
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			BranchFeature = CommonServices.GetERPFeatureById(17);
			List<tblUserRights> lst = clsGlobalVariables.ScreenViewReights;
			List<tblUserRights> newList = lst.Where((tblUserRights x) => x.ModuleDescription == "Account Reports" && x.Value).ToList();
			if (newList.Count > 0)
			{
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "GeneralLedger").ToList();
				if (newList.Count > 0)
				{
					if (!bool.Parse(newList[0].Value.ToString()))
					{
						tabControl1.TabPages.Remove(tabPageGeneralLedger);
					}
				}
				else
				{
					tabControl1.TabPages.Remove(tabPageGeneralLedger);
				}
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "CustomerLedger").ToList();
				if (newList.Count > 0)
				{
					if (!bool.Parse(newList[0].Value.ToString()))
					{
						tabControl1.TabPages.Remove(tabPageCustomerLedger);
					}
					else
					{
						tabControl1.TabPages.Remove(tabPageCustomerLedger);
						tabControl1.TabPages.Add("Party_Ledger");
						CustomerLedger frm3 = new CustomerLedger(UserAccount);
						frm3.TopLevel = false;
						frm3.Visible = true;
						frm3.FormBorderStyle = FormBorderStyle.None;
						frm3.Dock = DockStyle.Fill;
						tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(frm3);
					}
				}
				else
				{
					tabControl1.TabPages.Remove(tabPageCustomerLedger);
				}
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "FcyGeneralLedgerRpt").ToList();
				if (newList.Count > 0)
				{
					if (!bool.Parse(newList[0].Value.ToString()))
					{
						tabControl1.TabPages.Remove(tabPageFcyGeneralLedger);
					}
					else
					{
						tabControl1.TabPages.Remove(tabPageFcyGeneralLedger);
						tabControl1.TabPages.Add("Fcy_Ledger");
						FcyGeneralLedgerRpt frm2 = new FcyGeneralLedgerRpt(UserAccount);
						frm2.TopLevel = false;
						frm2.Visible = true;
						frm2.FormBorderStyle = FormBorderStyle.None;
						frm2.Dock = DockStyle.Fill;
						tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(frm2);
					}
				}
				else
				{
					tabControl1.TabPages.Remove(tabPageFcyGeneralLedger);
				}
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "ItemLedger").ToList();
				if (newList.Count > 0)
				{
					StockAccountFill();
					if (!bool.Parse(newList[0].Value.ToString()))
					{
						tabControl1.TabPages.Remove(tabPageItemLedger);
					}
					if (ItemLedgerId > 0)
					{
						cmbStockAccountItemLedger.Focus();
						cmbStockAccountItemLedger.Value = ItemLedgerId;
						if (ItemLedgerItemId > 0)
						{
							GetItemByStockAccount();
						}
						tabControl1.SelectedIndex = tabControl1.TabPages.IndexOf(tabPageItemLedger);
						CmbItemNameItmLedger.Value = ItemLedgerItemId;
						txtFromDateItemLedger.Value = FromDate;
						txtToDateItemLedger.Value = ToDate;
						ItemLedgerFromAccounts();
					}
				}
				else
				{
					tabControl1.TabPages.Remove(tabPageItemLedger);
				}
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "CommissionAgentRpt").ToList();
				if (newList.Count > 0)
				{
					if (!bool.Parse(newList[0].Value.ToString()))
					{
						tabControl1.TabPages.Remove(tabPageCommissionAgent);
					}
					else
					{
						tabControl1.TabPages.Remove(tabPageCommissionAgent);
						tabControl1.TabPages.Add("CommissionAgent_Ledger");
						CommissionAgentLedger frm = new CommissionAgentLedger(UserAccount);
						frm.TopLevel = false;
						frm.Visible = true;
						frm.FormBorderStyle = FormBorderStyle.None;
						frm.Dock = DockStyle.Fill;
						tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(frm);
					}
				}
				else
				{
					tabControl1.TabPages.Remove(tabPageCommissionAgent);
				}
			}
			CostCenterFill();
			if (UserAccount.AppId == 5)
			{
				if (((DataTable)((UltraGridBase)CmbCostCenter).DataSource).Rows.Count > 0)
				{
					((UltraGridBase)CmbCostCenter).Rows[0].Activate();
					((Control)(object)CmbCostCenter).Enabled = false;
				}
				tabControl2.TabPages.Remove(tabMultiAccountsLedger);
				tabControl2.TabPages.Remove(tabBankSummaryLedger);
				tabControl1.TabPages.Remove(tabPageCustomerLedger);
				tabControl1.TabPages.Remove(tabPageItemLedger);
				tabControl1.TabPages.Remove(tabPageCommissionAgent);
				tabControl1.TabPages.Remove(tabPageFcyGeneralLedger);
			}
			CompanyFill();
			AccountTitleFill(Conversion.ToInt(CmbCostCenter.Value));
			DateTypeFill();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				CmbSubsidiaryAccountFill(Conversion.ToInt(CmbCostCenter.Value));
				lblSubsidiaryAccount.Visible = true;
				((Control)(object)CmbSubsidiaryAccount).Visible = true;
				rdPanel.Location = new Point(82, 126);
			}
			else
			{
				lblSubsidiaryAccount.Visible = false;
				((Control)(object)CmbSubsidiaryAccount).Visible = false;
				rdPanel.Location = new Point(82, 99);
			}
			if (BranchFeature)
			{
				lblBranch.Visible = true;
				((Control)(object)cmbbranch).Visible = true;
				((Control)(object)CmbDateType).Size = new Size(120, 26);
				gbBranchesWithBalance.Visible = true;
				base.AutoScrollMinSize = new Size(1670, 550);
			}
			else
			{
				lblBranch.Visible = false;
				((Control)(object)cmbbranch).Visible = false;
				((Control)(object)CmbDateType).Size = new Size(315, 26);
				gbBranchesWithBalance.Visible = false;
			}
			BranchFill();
			ProjectFill();
			if (!DefaulLedgerDetail && !DefaulLedgerSummaryOne && !DefaulLedgerSummaryTwo)
			{
				ChkDetail.Checked = true;
				rdDetailPrint.Checked = true;
			}
			else if (DefaulLedgerDetail)
			{
				ChkDetail.Checked = DefaulLedgerDetail;
				rdDetailPrint.Checked = DefaulLedgerDetail;
			}
			else if (DefaulLedgerSummaryOne)
			{
				ChkSummery1.Checked = DefaulLedgerSummaryOne;
				rdSummary1Print.Checked = DefaulLedgerSummaryOne;
			}
			else if (DefaulLedgerSummaryTwo)
			{
				ChkSummery2.Checked = DefaulLedgerSummaryTwo;
				rdSummary2Print.Checked = DefaulLedgerSummaryTwo;
			}
			ChkBoxPost.Checked = true;
			chkIncludeUnpostedVouchers.Checked = true;
			((UltraGridBase)CmbDateType).Rows[4].Activate();
			if (dtForDocumentTypeList.Columns.Count == 0)
			{
				dtForDocumentTypeList.Columns.Add("DocumentTypeId", typeof(int));
				dtForDocumentTypeList.Columns.Add("D.Type", typeof(string));
				dtForDocumentTypeList.Columns.Add("Balance", typeof(double));
			}
			if (GeneralLedgerId == 0)
			{
				txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			if (GeneralLedgerId > 0)
			{
				cmbAccountTitle.Value = GeneralLedgerId;
				txtFromDate.Value = FromDate;
				txtToDate.Value = ToDate;
				btnshow_Click(null, null);
				tabControl2.SelectedIndex = 0;
			}
			cmbAccountTitle.Focus();
			datToDate.Value = DateTime.Now;
			GroupAccountFill();
			CustomGroupFill();
			AllAccountsBind();
			((UltraGridBase)CmbDateTypeMulti).Rows[4].Activate();
			grdAccountFillBankSummary();
			((UltraGridBase)CmbDateTypeBankSummLedger).Rows[1].Activate();
			AccountTypesBindForBankLedgerSummary();
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
				DDL.BindDDLNew(dt, CmbLanguage, "Id", "LanguageDescription", "Language Name", false);
				((UltraGridBase)CmbLanguage).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CostCenterFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCostCenter.Value);
			DataTable dt = Projects.GetSubCostCenters(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, UserAccount.AppId, 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCostCenter, "Id", "CostCenterName", "Cost Center", ZeroIndex: false);
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCostCenter.Value = Id;
					}
					else
					{
						((Control)(object)CmbCostCenter).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbCostCenter).DataSource = null;
				((Control)(object)CmbCostCenter).Text = "";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountTypesBindForBankLedgerSummary()
	{
		try
		{
			dtAccountTypes.Rows.Clear();
			dtAccountTypes = VoucherHead.GetAccountTypesForBankSummary(UserAccount.OrganizationId, UserAccount.CompanyId, 15);
			if (dtAccountTypes.Rows.Count > 0)
			{
				DDL.BindDDL(dtAccountTypes, CmbAccountTypesForBankSummaryLedger, "Id", "AccountType", "Account Type", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbAccountTypesForBankSummaryLedger).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbAccountTypesForBankSummaryLedger.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbAccountTypesForBankSummaryLedger.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbAccountTypesForBankSummaryLedger.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbAccountTypesForBankSummaryLedger.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill(int CostCenterId = 0)
	{
		try
		{
			DataTable dt = CommonServices.GetAllDetailAccount(CostCenterId);
			if (dt.Rows.Count > 0 && dt != null)
			{
				if (RdAcByName.Checked)
				{
					DDL.BindDDL(dt, cmbAccountTitle, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				}
				else
				{
					DDL.BindDDL(dt, cmbAccountTitle, "Id", "AccountCode", "Account Code", ZeroIndex: false);
				}
				((UltraGridBase)cmbAccountTitle).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccountFill(int CostCenterId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherHead.GetSubsidiaryAccountFromVouchers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				CostCenterId = CostCenterId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSubsidiaryAccount, "SubsidiaryAccountId", "SubsidaryAccount", "Subsidiary Account", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
				((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SubsidiaryAccountBindByParentAccount(int CostCenterId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherHead.GetSubsidiaryAccountFromVouchers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				CostCenterId = CostCenterId,
				AccountId = Conversion.ToInt(cmbAccountTitle.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSubsidiaryAccount, "SubsidiaryAccountId", "SubsidaryAccount", "Subsidiary Account", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
				((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyFill()
	{
	}

	private void BranchFill()
	{
		try
		{
			DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, "", 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbbranch, "Id", "BranchName", "Branch Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbproject, "Id", "ProjectName", "Project Name", false);
			}
			((UltraGridBase)cmbproject).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateTypeFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, CmbDateType, "Id", "Parameters", "Date Type", ZeroIndex: false);
			DDL.BindDDL(dt, CmbDateTypeMulti, "Id", "Parameters", "Date Type", ZeroIndex: false);
			DDL.BindDDL(dt, CmbDateTypeBankSummLedger, "Id", "Parameters", "Date Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllAccountsBind()
	{
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		try
		{
			string GroupAccountIds = "";
			string CustomGroupIds = "";
			if (((Control)(object)CmbGroupAccount).Text != null && ((Control)(object)CmbGroupAccount).Text != string.Empty)
			{
				string[] array = Conversion.ToString((object)((Control)(object)CmbGroupAccount).Text).ToString().Split(',');
				foreach (string author2 in array)
				{
					DataRow[] dr2 = dtGroupAccounts.Select("AccountTitle='" + Conversion.ToString((object)author2) + "'");
					if (dr2.Length != 0)
					{
						GroupAccountIds = GroupAccountIds + Conversion.ToString(dr2[0].ItemArray[0]) + ",";
					}
				}
			}
			if (((Control)(object)cmbCustomGroup).Text != null && ((Control)(object)cmbCustomGroup).Text != string.Empty)
			{
				string[] array = Conversion.ToString((object)((Control)(object)cmbCustomGroup).Text).ToString().Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtCustomGroup.Select("AcLookUpsDescription='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						CustomGroupIds = CustomGroupIds + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
			}
			DataTable dt = ChartofAccount.GetAccountsWithBalanceByGroupIdsOrCustomGroups(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ToDate = datToDate.Value,
				AccountIds = GroupAccountIds,
				Ids = CustomGroupIds,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtAccounts = new DataTable();
				dtAccounts.Columns.Add("Id");
				dtAccounts.Columns.Add("AccountCode");
				dtAccounts.Columns.Add("AccountTitle");
				dtAccounts.Columns.Add("Balance", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtAccounts.Rows.Add(dt.Rows[i]["AccountId"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["Balance"]);
				}
				grdAccounts.DataSource = dtAccounts;
				grdAccounts.RetrieveStructure();
				grdAccounts.ColumnAutoResize = true;
				grdAccounts.RootTable.Columns["Id"].Visible = false;
				grdAccounts.RootTable.Columns["AccountTitle"].Width = 250;
				grdAccounts.RootTable.Columns["Balance"].AggregateFunction = (AggregateFunction)2;
				grdAccounts.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
				grdAccounts.RootTable.Columns["Balance"].FormatString = clsGlobalVariables.stringFormatboth;
				grdAccounts.RootTable.Columns["Balance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
				grdAccounts.RootTable.Columns.Add("Select");
				grdAccounts.RootTable.Columns["Select"].ActAsSelector = true;
				grdAccounts.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdAccounts.RootTable.Columns["Select"].Position = 0;
				grdAccounts.RootTable.Columns["Select"].Width = 25;
				grdAccounts.FrozenColumns = 1;
				ctrlGrdBar7_Load(null, null);
			}
			else
			{
				grdAccounts.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GroupAccountFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtGroupAccounts.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Account_Level = 3;
			dtGroupAccounts = ChartofAccount.ReadAllAccountgroup(obj);
			DDL.BindDDLNew(dtGroupAccounts, CmbGroupAccount, "Id", "AccountTitle", "Account Title", false);
			UltraGridColumn obj2 = ((UltraGridBase)CmbGroupAccount).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj2).Key = "Selected";
			((HeaderBase)obj2.Header).Caption = string.Empty;
			obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj2.DataType = typeof(bool);
			((HeaderBase)obj2.Header).VisiblePosition = 0;
			CmbGroupAccount.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbGroupAccount.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbGroupAccount.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbGroupAccount.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomGroupFill()
	{
		try
		{
			dtCustomGroup.Rows.Clear();
			dtCustomGroup = CommonServices.CustomeGroupsDefine(1);
			DDL.BindDDLNew(dtCustomGroup, cmbCustomGroup, "Id", "AcLookUpsDescription", "Custome Group", false);
			((UltraGridBase)cmbCustomGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			UltraGridColumn obj = ((UltraGridBase)cmbCustomGroup).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			cmbCustomGroup.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbCustomGroup.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbCustomGroup.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbCustomGroup.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateType.Value) == 1)
			{
				txtFromDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 2)
			{
				txtFromDate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDate.Value = first;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromDate.Value = first2;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 5)
			{
				txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloseUp = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[2].Value);
				if (GLOfSubSidory != 0)
				{
					cmbAccountTitle.Value = GLOfSubSidory;
					cmbAccountTitle_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!AfterCloseUp && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && ((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
			{
				cmbAccountTitle.Value = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value);
				cmbAccountTitle_Leave(null, null);
			}
			AfterCloseUp = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDateTypeBankSummLedger_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateTypeBankSummLedger.Value) == 1)
			{
				txtFromDateBankSummLedger.Value = DateTime.Today;
				txttoDateBankSummLedger.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeBankSummLedger.Value) == 2)
			{
				txtFromDateBankSummLedger.Value = DateTime.Today.AddDays(-7.0);
				txttoDateBankSummLedger.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeBankSummLedger.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDateBankSummLedger.Value = first;
				txttoDateBankSummLedger.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeBankSummLedger.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromDateBankSummLedger.Value = first2;
				txttoDateBankSummLedger.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeBankSummLedger.Value) == 5)
			{
				txtFromDateBankSummLedger.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				txttoDateBankSummLedger.Value = DateTime.Today;
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
			grd.DataSource = null;
			ChkBoxPost.Checked = true;
			((Control)(object)cmbAccountTitle).Text = string.Empty;
			cmbAccountTitle.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneralLedger_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
			GeneralLedgerId = 0;
			ItemLedgerId = 0;
			ItemLedgerItemId = 0;
			SupplierCustomerId = 0;
			FromDate = DateTime.Now;
			ToDate = DateTime.Now;
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

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.KeyData == Keys.Return)
				{
					SendKeys.Send("{TAB}");
				}
				if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
				{
					Close();
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						btnNew_Click(null, null);
					}
					else
					{
						btnReset_Click(null, null);
					}
				}
				if (e.Control && e.KeyCode == Keys.T)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						datToDate.Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						CmbDateType.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						CmbDateType.Focus();
					}
					else
					{
						datToDate.Focus();
					}
				}
				if ((e.Alt && e.KeyCode == Keys.D1) || (e.Alt && e.KeyCode == Keys.NumPad1))
				{
					if (tabControl2.SelectedIndex == 0)
					{
						if (ChkDetail.Checked)
						{
							print_Click_1(null, null);
						}
						else if (ChkSummery1.Checked)
						{
							btnSummeryLedger_Click(null, null);
						}
						else if (ChkSummery2.Checked)
						{
							btnSummary2_Click(null, null);
						}
					}
					else if (rdDetailPrint.Checked)
					{
						btn105MultiPrint_Click(null, null);
					}
					else if (rdSummary1Print.Checked)
					{
						btn108MultiPrint_Click(null, null);
					}
					else if (rdSummary2Print.Checked)
					{
						btn105AMultiPrint_Click(null, null);
					}
				}
				if ((e.Alt && e.KeyCode == Keys.D2) || (e.Alt && e.KeyCode == Keys.NumPad2))
				{
					if (tabControl2.SelectedIndex == 0)
					{
						if (ChkDetail.Checked)
						{
							toolStripButton1_Click(null, null);
						}
					}
					else if (rdDetailPrint.Checked)
					{
						btn106MultiPrint_Click(null, null);
					}
				}
				if ((e.Alt && e.KeyCode == Keys.D3) || (e.Alt && e.KeyCode == Keys.NumPad3))
				{
					if (tabControl2.SelectedIndex == 0)
					{
						if (ChkDetail.Checked)
						{
							btnQuantativeLedger_Click(null, null);
						}
					}
					else if (rdDetailPrint.Checked)
					{
						btn107MultiPrint_Click(null, null);
					}
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						btnshow_Click(null, null);
					}
					else
					{
						btnShowMultiLedger_Click(null, null);
					}
				}
				if (e.Control && e.Alt)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						MakeShortCutKeys1();
					}
					else
					{
						MakeShortCutKeys();
					}
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grd).Focus();
					}
					else if (((Control)(object)grdAccounts).Focused)
					{
						((Control)(object)grdMultiAccountsLedger).Focus();
					}
					else if (((Control)(object)grdMultiAccountsLedger).Focused)
					{
						((Control)(object)grdAccounts).Focus();
					}
					else
					{
						((Control)(object)grdAccounts).Focus();
					}
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.S && tabControl2.SelectedIndex == 0)
				{
					btnSaveCheckedRows_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					lblLanguage.Visible = true;
					((Control)(object)CmbLanguage).Visible = true;
				}
			}
			if (tabControl1.SelectedIndex == 1)
			{
				if (e.KeyData == Keys.Return)
				{
					SendKeys.Send("{TAB}");
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnRegisterItemLedger_Click(null, null);
				}
				if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
				{
					Close();
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewItemLedger_Click(null, null);
				}
			}
			if (tabControl1.SelectedIndex == 2 && e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (tabControl2.SelectedTab == tabBankSummaryLedger)
			{
				if (e.KeyData == Keys.Return)
				{
					SendKeys.Send("{TAB}");
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 80;
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					BtnNewBanksummLegder_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnShowBankSummLedger_Click(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtDetail == null || dtDetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtDetail, "105-AcRptGeneralLedger.rpt");
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		try
		{
			dtDetail.Rows.Clear();
			dtSummary1.Rows.Clear();
			dtSummary2.Rows.Clear();
			dtForDocumentTypeList.Rows.Clear();
			grd.ClearStructure();
			LedgerDetailByAccountId();
			if (Conversion.ToInt(cmbAccountTitle.Value) == 0)
			{
				throw new Exception("Account Title Field is Required");
			}
			DataTable dtBranchWiseBalance = new DataTable();
			if (BranchFeature)
			{
				dtBranchWiseBalance = ChartofAccount.getGLBalanceBranchesWiseByAccountId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					FromDate = txtFromDate.Value,
					ToDate = txtToDate.Value,
					AccountId = Conversion.ToInt(cmbAccountTitle.Value)
				});
			}
			ReportsParameters obj = new ReportsParameters();
			if (ChkDetail.Checked)
			{
				((Control)(object)grd).Name = "Detailgrd";
				print105.Visible = true;
				btnPrint106.Visible = true;
				btnQuantativeLedger.Visible = true;
				btnSaveCheckedRows.Visible = true;
				rBForCheckAllPreviousRows.Visible = true;
				btnSummeryLedger.Visible = false;
				btnSummary2.Visible = false;
				BtnPrintSelectedDocDetail.Visible = false;
				BtnPrintSelectedDocSummary2.Visible = false;
				BtnPrintSelectedDocSumm1.Visible = false;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
				obj.FromDate = txtFromDate.Value;
				obj.ToDate = txtToDate.Value;
				obj.EntryUser = UserAccount.ID;
				obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
				if (!ChkBoxPost.Checked)
				{
					obj.IsApproved = true;
				}
				else
				{
					obj.ApprovedFilter = "All";
				}
				if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
				{
					obj.SubsidiaryAccountId = Conversion.ToInt(CmbSubsidiaryAccount.Value);
					DataRow r = ((DataTable)((UltraGridBase)CmbSubsidiaryAccount).DataSource).AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("SubsidiaryAccountId") == obj.SubsidiaryAccountId);
					if (r != null)
					{
						obj.SubsidiaryTypeId = Conversion.ToInt(r["SubsidiaryTypeId"]);
					}
				}
				obj.BranchesId = (BranchFeature ? Conversion.ToInt(cmbbranch.Value) : 0);
				if (UserAccount.AppId == 5 && Conversion.ToInt(CmbCostCenter.Value) == 0)
				{
					throw new Exception("Cost Center Not Found");
				}
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				dtDetail = VoucherReports.GeneralLedgerWithOffsetAccount(obj);
				if (dtDetail.Rows.Count <= 0 || dtDetail == null)
				{
					grd.ClearStructure();
					throw new Exception("Record Not Found For Display");
				}
				dtForDetailGrid.BeginLoadData();
				dtForDetailGrid.Clear();
				if (dtForDetailGrid.Columns.Count == 0)
				{
					dtForDetailGrid.Columns.Add("DetailId", typeof(int));
					dtForDetailGrid.Columns.Add("Checked", typeof(bool));
					dtForDetailGrid.Columns.Add("BookmarkRemarks", typeof(string));
					dtForDetailGrid.Columns.Add("BranchName", typeof(string));
					dtForDetailGrid.Columns.Add("VoucherDate", typeof(DateTime));
					dtForDetailGrid.Columns.Add("DocType");
					dtForDetailGrid.Columns.Add("VoucherNo");
					dtForDetailGrid.Columns.Add("AccountCode");
					dtForDetailGrid.Columns.Add("OffSetTitle");
					dtForDetailGrid.Columns.Add("SubsidiaryAccount");
					dtForDetailGrid.Columns.Add("Debit", typeof(double));
					dtForDetailGrid.Columns.Add("Credit", typeof(double));
					dtForDetailGrid.Columns.Add("Balance", typeof(double));
					dtForDetailGrid.Columns.Add("BalType");
					dtForDetailGrid.Columns.Add("BillAmount", typeof(double));
					dtForDetailGrid.Columns.Add("DueDays", typeof(int));
					dtForDetailGrid.Columns.Add("DueDate", typeof(DateTime));
					dtForDetailGrid.Columns.Add("Comments");
					dtForDetailGrid.Columns.Add("ChequeNo");
					dtForDetailGrid.Columns.Add("ChequeDate", typeof(string));
					dtForDetailGrid.Columns.Add("PayeeTitle");
					dtForDetailGrid.Columns.Add("RefParty");
					dtForDetailGrid.Columns.Add("JobLot");
					dtForDetailGrid.Columns.Add("MannualNo");
					dtForDetailGrid.Columns.Add("RefNo");
					dtForDetailGrid.Columns.Add("StockStatus", typeof(string));
					dtForDetailGrid.Columns.Add("QtyIn", typeof(double));
					dtForDetailGrid.Columns.Add("QtyOut", typeof(double));
					dtForDetailGrid.Columns.Add("ItemRate", typeof(double));
					dtForDetailGrid.Columns.Add("VehicleNo", typeof(string));
					dtForDetailGrid.Columns.Add("NoOfAttachments", typeof(double));
					dtForDetailGrid.Columns.Add("VoucherHeadId");
					dtForDetailGrid.Columns.Add("DocumentTypeId");
					dtForDetailGrid.Columns.Add("DocumentTypeSrNo");
					dtForDetailGrid.Columns.Add("IsApproved");
					dtForDetailGrid.Columns.Add("BaseDocumentTypeId", typeof(int));
					dtForDetailGrid.Columns.Add("AccountId", typeof(int));
				}
				foreach (DataRow row2 in dtDetail.Rows)
				{
					string type = "Nill";
					if (Conversion.ToDouble(row2["Balance"]) > 0.0)
					{
						type = "Dr";
					}
					else if (Conversion.ToDouble(row2["Balance"]) < 0.0)
					{
						type = "Cr";
					}
					dtForDetailGrid.Rows.Add(row2["DetailId"], Conversion.ToBool(row2["BookmarkStatus"]), row2["BookmarkRemarks"], row2["BranchName"], Conversion.ToDateTime(row2["VoucherDate"]).ToShortDateString(), row2["DocTypeCode"], row2["VoucherCode"], row2["AccountCode"], row2["OffsetAccountTitle"], row2["SubsidiaryAccount"], row2["DebitAmount"], row2["CreditAmount"], row2["Balance"], type, row2["BillAmount"], row2["DueDays"], row2["DueDate"], row2["Comments"], row2["CheqNoDetail"], (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row2["CheqDate"]))) ? Conversion.ToDateTime(row2["CheqDate"]).ToShortDateString() : "", row2["PayeeTitle"], row2["RefParty"], row2["JobLot"], row2["MannualNo"], row2["RefInvoiceNo"], row2["StockReservedStatus"], row2["QtyIn"], row2["QtyOut"], row2["ItemRate"], row2["VehcileNo"], row2["NoOfAttachments"], row2["VoucherHeadId"], row2["DocumentTypeId"], row2["DocumentTypeSrNo"], row2["IsApproved"], row2["BaseDocumentTypeId"], row2["AccountId"]);
				}
				dtForDetailGrid.EndLoadData();
				foreach (var item in (from row in dtDetail.AsEnumerable()
					group row by new
					{
						DocumentTypeId = Conversion.ToInt(row["DocumentTypeId"]),
						DocTypeCode = row["DocTypeCode"].ToString()
					} into g
					select new
					{
						DocumentTypeId = g.Key.DocumentTypeId,
						DocTypeCode = g.Key.DocTypeCode,
						Balance = g.Sum((DataRow row) => Conversion.ToDouble(row["DebitAmount"]) - Conversion.ToDouble(row["CreditAmount"]))
					}).ToList())
				{
					dtForDocumentTypeList.Rows.Add(item.DocumentTypeId, item.DocTypeCode, item.Balance);
				}
				if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
				{
					double balance2 = Conversion.ToDouble(dtForDetailGrid.Rows[dtForDetailGrid.Rows.Count - 1]["Balance"]);
					string lbl2 = "";
					if (balance2 > 0.0)
					{
						lbl2 = "Dr";
					}
					else if (balance2 < 0.0)
					{
						lbl2 = "Cr";
					}
					lblSubsidiaryBalance.Text = balance2.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl2;
				}
				if (Conversion.ToInt(cmbbranch.Value) > 0)
				{
					label46.Visible = true;
					lblBranchBalance.Visible = true;
					double balance = Conversion.ToDouble(dtForDetailGrid.Rows[dtForDetailGrid.Rows.Count - 1]["Balance"]);
					string lbl = "";
					if (balance > 0.0)
					{
						lbl = "Dr";
					}
					else if (balance < 0.0)
					{
						lbl = "Cr";
					}
					label46.Text = ((Control)(object)cmbbranch).Text;
					lblBranchBalance.Text = balance.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl;
				}
				else
				{
					label46.Visible = false;
					lblBranchBalance.Visible = false;
				}
				if (Conversion.ToDouble(dtDetail.Rows[0]["PostDatedCheqAmount"]) != 0.0)
				{
					lblPostedNoOfCheqs.Visible = true;
					ValuePostDatedNoOfCheqs.Visible = true;
					lblPostDatedCheqAmount.Visible = true;
					ValuePostDatedCheqAmount.Visible = true;
					ValuePostDatedNoOfCheqs.Text = Conversion.ToDouble(dtDetail.Rows[0]["PostDatedNoOfCheqs"]).ToString();
					ValuePostDatedCheqAmount.Text = Conversion.ToDouble(dtDetail.Rows[0]["PostDatedCheqAmount"]).ToString(clsGlobalVariables.stringFormatboth);
					PanelAccountDetail.Location = new Point(4, 5);
				}
				else
				{
					lblPostedNoOfCheqs.Visible = false;
					ValuePostDatedNoOfCheqs.Visible = false;
					lblPostDatedCheqAmount.Visible = false;
					ValuePostDatedCheqAmount.Visible = false;
					PanelAccountDetail.Location = new Point(4, 22);
				}
				grd.DataSource = dtForDetailGrid;
				grd.RetrieveStructure();
				GridSettings();
				GeneralLedgerId = 0;
			}
			else if (ChkSummery1.Checked)
			{
				((Control)(object)grd).Name = "SummaryGrd";
				btnSummeryLedger.Visible = true;
				print105.Visible = false;
				btnPrint106.Visible = false;
				btnQuantativeLedger.Visible = false;
				btnSummary2.Visible = false;
				btnSaveCheckedRows.Visible = false;
				rBForCheckAllPreviousRows.Visible = false;
				BtnPrintSelectedDocDetail.Visible = false;
				BtnPrintSelectedDocSumm1.Visible = false;
				BtnPrintSelectedDocSummary2.Visible = false;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
				obj.FromDate = txtFromDate.Value;
				obj.ToDate = txtToDate.Value;
				obj.EntryUser = UserAccount.ID;
				obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
				if (!ChkBoxPost.Checked)
				{
					obj.IsApproved = true;
				}
				else
				{
					obj.ApprovedFilter = "All";
				}
				if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
				{
					obj.SubsidiaryAccountId = Conversion.ToInt(CmbSubsidiaryAccount.Value);
					DataRow r2 = ((DataTable)((UltraGridBase)CmbSubsidiaryAccount).DataSource).AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("SubsidiaryAccountId") == obj.SubsidiaryAccountId);
					if (r2 != null)
					{
						obj.SubsidiaryTypeId = Conversion.ToInt(r2["SubsidiaryTypeId"]);
					}
				}
				obj.BranchesId = (BranchFeature ? Conversion.ToInt(cmbbranch.Value) : 0);
				if (UserAccount.AppId == 5 && Conversion.ToInt(CmbCostCenter.Value) == 0)
				{
					throw new Exception("Cost Center Not Found");
				}
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				dtSummary1 = VoucherReports.GeneralLedgerSummery(obj);
				if (dtSummary1.Rows.Count > 0)
				{
					dtForSummary1.BeginLoadData();
					dtForSummary1.Clear();
					if (dtForSummary1.Columns.Count == 0)
					{
						dtForSummary1.Columns.Add("BranchName", typeof(string));
						dtForSummary1.Columns.Add("VoucherDate", typeof(DateTime));
						dtForSummary1.Columns.Add("DocType", typeof(string));
						dtForSummary1.Columns.Add("VoucherCode", typeof(int));
						dtForSummary1.Columns.Add("ManualBillNo", typeof(string));
						dtForSummary1.Columns.Add("AccountId", typeof(int));
						dtForSummary1.Columns.Add("AccountTitle", typeof(string));
						dtForSummary1.Columns.Add("Remarks", typeof(string));
						dtForSummary1.Columns.Add("DebitAmount", typeof(double));
						dtForSummary1.Columns.Add("CreditAmount", typeof(double));
						dtForSummary1.Columns.Add("RunningBalance", typeof(double));
						dtForSummary1.Columns.Add("DueDate", typeof(DateTime));
						dtForSummary1.Columns.Add("DueDays", typeof(int));
						dtForSummary1.Columns.Add("DecimlePointsRate", typeof(int));
						dtForSummary1.Columns.Add("DecimlePointsAmount", typeof(int));
						dtForSummary1.Columns.Add("IsApproved", typeof(bool));
						dtForSummary1.Columns.Add("CurrentDR/CrDiff", typeof(double));
						dtForSummary1.Columns.Add("VehicleNo", typeof(string));
						dtForSummary1.Columns.Add("VoucherHeadId");
						dtForSummary1.Columns.Add("DocumentTypeId");
						dtForSummary1.Columns.Add("BaseDocumentTypeId");
						dtForSummary1.Columns.Add("DocumentTypeSrNo");
					}
					foreach (DataRow row3 in dtSummary1.Rows)
					{
						dtForSummary1.Rows.Add(row3["BranchName"], row3["VoucherDate"], row3["DocumentTypeDescription"], row3["VoucherCode"], row3["ManualBillNo"], row3["AccountId"], row3["AccountTitle"], row3["Remarks"], row3["DebitAmount"], row3["CreditAmount"], row3["RunningBalance"], row3["DueDate"], row3["DueDays"], row3["DecimlePointsRate"], row3["DecimlePointsAmount"], row3["IsApproved"], Conversion.ToDouble(row3["DebitAmount"]) - Conversion.ToDouble(row3["CreditAmount"]), row3["VehicleNo"], row3["Id"], row3["DocumentTypeId"], row3["BaseDocumentTypeId"], row3["DocumentTypeSrNo"]);
					}
					dtForSummary1.EndLoadData();
					foreach (var item2 in (from row in dtSummary1.AsEnumerable()
						group row by new
						{
							DocumentTypeId = Conversion.ToInt(row["DocumentTypeId"]),
							DocTypeCode = row["DocumentTypeDescription"].ToString()
						} into g
						select new
						{
							DocumentTypeId = g.Key.DocumentTypeId,
							DocTypeCode = g.Key.DocTypeCode,
							Balance = g.Sum((DataRow row) => Conversion.ToDouble(row["DebitAmount"]) - Conversion.ToDouble(row["CreditAmount"]))
						}).ToList())
					{
						dtForDocumentTypeList.Rows.Add(item2.DocumentTypeId, item2.DocTypeCode, item2.Balance);
					}
					if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
					{
						double balance4 = Conversion.ToDouble(dtForSummary1.Rows[dtForSummary1.Rows.Count - 1]["RunningBalance"]);
						string lbl4 = "";
						if (balance4 > 0.0)
						{
							lbl4 = "Dr";
						}
						else if (balance4 < 0.0)
						{
							lbl4 = "Cr";
						}
						lblSubsidiaryBalance.Text = balance4.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl4;
					}
					if (Conversion.ToInt(cmbbranch.Value) > 0)
					{
						label46.Visible = true;
						lblBranchBalance.Visible = true;
						double balance3 = Conversion.ToDouble(dtForSummary1.Rows[dtForSummary1.Rows.Count - 1]["RunningBalance"]);
						string lbl3 = "";
						if (balance3 > 0.0)
						{
							lbl3 = "Dr";
						}
						else if (balance3 < 0.0)
						{
							lbl3 = "Cr";
						}
						label46.Text = ((Control)(object)cmbbranch).Text;
						lblBranchBalance.Text = balance3.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl3;
					}
					else
					{
						label46.Visible = false;
						lblBranchBalance.Visible = false;
					}
					if (Conversion.ToDouble(dtSummary1.Rows[0]["PostDatedCheqAmount"]) != 0.0)
					{
						lblPostedNoOfCheqs.Visible = true;
						ValuePostDatedNoOfCheqs.Visible = true;
						lblPostDatedCheqAmount.Visible = true;
						ValuePostDatedCheqAmount.Visible = true;
						ValuePostDatedNoOfCheqs.Text = Conversion.ToDouble(dtSummary1.Rows[0]["PostDatedNoOfCheqs"]).ToString();
						ValuePostDatedCheqAmount.Text = Conversion.ToDouble(dtSummary1.Rows[0]["PostDatedCheqAmount"]).ToString(clsGlobalVariables.stringFormatboth);
						PanelAccountDetail.Location = new Point(4, 5);
					}
					else
					{
						lblPostedNoOfCheqs.Visible = false;
						ValuePostDatedNoOfCheqs.Visible = false;
						lblPostDatedCheqAmount.Visible = false;
						ValuePostDatedCheqAmount.Visible = false;
						PanelAccountDetail.Location = new Point(4, 22);
					}
					grd.DataSource = dtForSummary1;
					grd.RetrieveStructure();
					GridSettingsForSummery();
				}
				else
				{
					grd.ClearStructure();
				}
			}
			else if (ChkSummery2.Checked)
			{
				((Control)(object)grd).Name = "Summary2Grd";
				btnSummary2.Visible = true;
				print105.Visible = false;
				btnPrint106.Visible = false;
				btnQuantativeLedger.Visible = false;
				btnSummeryLedger.Visible = false;
				btnSaveCheckedRows.Visible = false;
				rBForCheckAllPreviousRows.Visible = false;
				BtnPrintSelectedDocDetail.Visible = false;
				BtnPrintSelectedDocSumm1.Visible = false;
				BtnPrintSelectedDocSummary2.Visible = false;
				obj.FromDate = txtFromDate.Value;
				obj.ToDate = txtToDate.Value;
				obj.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.EntryUser = UserAccount.ID;
				obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
				if (!ChkBoxPost.Checked)
				{
					obj.IsApproved = true;
				}
				else
				{
					obj.ApprovedFilter = "All";
				}
				if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
				{
					obj.SubsidiaryAccountId = Conversion.ToInt(CmbSubsidiaryAccount.Value);
					DataRow r3 = ((DataTable)((UltraGridBase)CmbSubsidiaryAccount).DataSource).AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("SubsidiaryAccountId") == obj.SubsidiaryAccountId);
					if (r3 != null)
					{
						obj.SubsidiaryTypeId = Conversion.ToInt(r3["SubsidiaryTypeId"]);
					}
				}
				obj.BranchesId = (BranchFeature ? Conversion.ToInt(cmbbranch.Value) : 0);
				if (UserAccount.AppId == 5 && Conversion.ToInt(CmbCostCenter.Value) == 0)
				{
					throw new Exception("Cost Center Not Found");
				}
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				grd.ClearStructure();
				dtSummary2 = VoucherReports.GeneralLedger2Format_Rpt(obj);
				if (dtSummary2.Rows.Count > 0)
				{
					dtForSummary2.BeginLoadData();
					dtForSummary2.Clear();
					if (dtForSummary2.Columns.Count == 0)
					{
						dtForSummary2.Columns.Add("BranchName", typeof(string));
						dtForSummary2.Columns.Add("VoucherDate", typeof(DateTime));
						dtForSummary2.Columns.Add("VoucherType");
						dtForSummary2.Columns.Add("VoucherNo");
						dtForSummary2.Columns.Add("AccountTitle");
						dtForSummary2.Columns.Add("OffSetAccountTitle");
						dtForSummary2.Columns.Add("Remarks");
						dtForSummary2.Columns.Add("DebitAmount", typeof(double));
						dtForSummary2.Columns.Add("CreditAmount", typeof(double));
						dtForSummary2.Columns.Add("Balance", typeof(double));
						dtForSummary2.Columns.Add("RunningBalance", typeof(double));
						dtForSummary2.Columns.Add("ChequeDate", typeof(string));
						dtForSummary2.Columns.Add("ChequeNo");
						dtForSummary2.Columns.Add("PayeeTitle");
						dtForSummary2.Columns.Add("DueDate", typeof(DateTime));
						dtForSummary2.Columns.Add("DueDays", typeof(int));
						dtForSummary2.Columns.Add("VoucherHeadId");
						dtForSummary2.Columns.Add("DocumentTypeId");
						dtForSummary2.Columns.Add("BaseDocumentTypeId");
						dtForSummary2.Columns.Add("DocumentTypeSrNo");
						dtForSummary2.Columns.Add("NoOfAttachments");
						dtForSummary2.Columns.Add("AccountId");
					}
					double RunningOpening = 0.0;
					for (int i = 0; i < dtSummary2.Rows.Count; i++)
					{
						if (i == 0)
						{
							RunningOpening = Conversion.ToDouble(dtSummary2.Rows[i]["RunningBalance"]);
						}
						dtForSummary2.Rows.Add(dtSummary2.Rows[i]["BranchName"], Conversion.ToDateTime(dtSummary2.Rows[i]["VoucherDate"]), dtSummary2.Rows[i]["VoucherType"], dtSummary2.Rows[i]["VoucherNo"], dtSummary2.Rows[i]["AccountTitle"], dtSummary2.Rows[i]["OffSetAccountTitle"], dtSummary2.Rows[i]["Remarks"], dtSummary2.Rows[i]["DebitAmount"], dtSummary2.Rows[i]["CreditAmount"], dtSummary2.Rows[i]["RunningBalance"], RunningOpening + Conversion.ToDouble(dtSummary2.Rows[i]["DebitAmount"]) - Conversion.ToDouble(dtSummary2.Rows[i]["CreditAmount"]), (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dtSummary2.Rows[i]["ChequeDate"]))) ? Conversion.ToDateTime(dtSummary2.Rows[i]["ChequeDate"]).ToShortDateString() : "", dtSummary2.Rows[i]["ChequeNo"], dtSummary2.Rows[i]["PayeeTitle"], dtSummary2.Rows[i]["DueDate"], dtSummary2.Rows[i]["DueDays"], dtSummary2.Rows[i]["VoucherHeadId"], dtSummary2.Rows[i]["DocumentTypeId"], dtSummary2.Rows[i]["BaseDocumentTypeId"], dtSummary2.Rows[i]["DocumentTypeSrNo"], dtSummary2.Rows[i]["NoOfAttachments"], dtSummary2.Rows[i]["AccountId"]);
						if (i != 0)
						{
							RunningOpening = RunningOpening + Conversion.ToDouble(dtSummary2.Rows[i]["DebitAmount"]) - Conversion.ToDouble(dtSummary2.Rows[i]["CreditAmount"]);
						}
					}
					dtForSummary2.EndLoadData();
					foreach (var item3 in (from row in dtForSummary2.AsEnumerable()
						group row by new
						{
							DocumentTypeId = Conversion.ToInt(row["DocumentTypeId"]),
							DocTypeCode = row["VoucherType"].ToString()
						} into g
						select new
						{
							DocumentTypeId = g.Key.DocumentTypeId,
							DocTypeCode = g.Key.DocTypeCode,
							Balance = g.Sum((DataRow row) => Conversion.ToDouble(row["DebitAmount"]) - Conversion.ToDouble(row["CreditAmount"]))
						}).ToList())
					{
						dtForDocumentTypeList.Rows.Add(item3.DocumentTypeId, item3.DocTypeCode, item3.Balance);
					}
					if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
					{
						double balance6 = Conversion.ToDouble(dtForSummary2.Rows[dtForSummary2.Rows.Count - 1]["RunningBalance"]);
						string lbl6 = "";
						if (balance6 > 0.0)
						{
							lbl6 = "Dr";
						}
						else if (balance6 < 0.0)
						{
							lbl6 = "Cr";
						}
						lblSubsidiaryBalance.Text = balance6.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl6;
					}
					if (Conversion.ToInt(cmbbranch.Value) > 0)
					{
						label46.Visible = true;
						lblBranchBalance.Visible = true;
						double balance5 = Conversion.ToDouble(dtForSummary2.Rows[dtForSummary2.Rows.Count - 1]["RunningBalance"]);
						string lbl5 = "";
						if (balance5 > 0.0)
						{
							lbl5 = "Dr";
						}
						else if (balance5 < 0.0)
						{
							lbl5 = "Cr";
						}
						label46.Text = ((Control)(object)cmbbranch).Text;
						lblBranchBalance.Text = balance5.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl5;
					}
					else
					{
						label46.Visible = false;
						lblBranchBalance.Visible = false;
					}
					if (Conversion.ToDouble(dtSummary2.Rows[0]["PostDatedCheqAmount"]) != 0.0)
					{
						lblPostedNoOfCheqs.Visible = true;
						ValuePostDatedNoOfCheqs.Visible = true;
						lblPostDatedCheqAmount.Visible = true;
						ValuePostDatedCheqAmount.Visible = true;
						ValuePostDatedNoOfCheqs.Text = Conversion.ToDouble(dtSummary2.Rows[0]["PostDatedNoOfCheqs"]).ToString();
						ValuePostDatedCheqAmount.Text = Conversion.ToDouble(dtSummary2.Rows[0]["PostDatedCheqAmount"]).ToString(clsGlobalVariables.stringFormatboth);
						PanelAccountDetail.Location = new Point(4, 5);
					}
					else
					{
						lblPostedNoOfCheqs.Visible = false;
						ValuePostDatedNoOfCheqs.Visible = false;
						lblPostDatedCheqAmount.Visible = false;
						ValuePostDatedCheqAmount.Visible = false;
						PanelAccountDetail.Location = new Point(4, 22);
					}
					grd.DataSource = dtForSummary2;
					grd.RetrieveStructure();
					GridSettingsForSummeryFormatII();
				}
				else
				{
					grd.ClearStructure();
				}
			}
			FillDocTypeList(dtForDocumentTypeList);
			FillBranchWiseBalance(dtBranchWiseBalance);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.AllowEdit = (InheritableBoolean)1;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["DetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].EditType = (EditType)0;
			grd.RootTable.Columns["VoucherDate"].EditType = (EditType)0;
			grd.RootTable.Columns["DocType"].EditType = (EditType)0;
			grd.RootTable.Columns["VoucherNo"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["OffSetTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["Debit"].EditType = (EditType)0;
			grd.RootTable.Columns["Credit"].EditType = (EditType)0;
			grd.RootTable.Columns["Balance"].EditType = (EditType)0;
			grd.RootTable.Columns["BalType"].EditType = (EditType)0;
			grd.RootTable.Columns["Comments"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeDate"].EditType = (EditType)0;
			grd.RootTable.Columns["DueDays"].EditType = (EditType)0;
			grd.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grd.RootTable.Columns["MannualNo"].EditType = (EditType)0;
			grd.RootTable.Columns["RefNo"].EditType = (EditType)0;
			grd.RootTable.Columns["QtyIn"].EditType = (EditType)0;
			grd.RootTable.Columns["QtyOut"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grd.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			grd.RootTable.Columns["VoucherHeadId"].EditType = (EditType)0;
			grd.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["BaseDocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["DocumentTypeSrNo"].EditType = (EditType)0;
			grd.RootTable.Columns["IsApproved"].EditType = (EditType)0;
			grd.RootTable.Columns["BillAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			grd.RootTable.Columns["PayeeTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["RefParty"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["StockStatus"].EditType = (EditType)0;
			grd.RootTable.Columns["VoucherDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["ChequeDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["Checked"].ColumnType = (ColumnType)3;
			grd.RootTable.Columns["VoucherDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["DocType"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["VoucherNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["OffSetTitle"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["SubsidiaryAccount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Debit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Credit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Balance"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BalType"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Comments"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["DueDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["DueDays"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ChequeNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ChequeDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["MannualNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RefNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["QtyIn"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["QtyOut"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ItemRate"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["NoOfAttachments"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BillAmount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PayeeTitle"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["VehicleNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RefParty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["StockStatus"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["DetailId"].Visible = false;
			grd.RootTable.Columns["VoucherHeadId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["BaseDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grd.RootTable.Columns["IsApproved"].Visible = false;
			grd.RootTable.Columns["RefNo"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grd.RootTable.Columns["StockStatus"].Visible = IsStockReservedPerParty;
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				grd.RootTable.Columns["SubsidiaryAccount"].Visible = false;
			}
			grd.RootTable.Columns["SubsidiaryAccount"].Caption = "Subsidiary Ac";
			grd.RootTable.Columns["Checked"].Width = 50;
			grd.RootTable.Columns["BookmarkRemarks"].Width = 100;
			grd.RootTable.Columns["BranchName"].Width = 120;
			grd.RootTable.Columns["VoucherDate"].Width = 90;
			grd.RootTable.Columns["DocType"].Width = 65;
			grd.RootTable.Columns["VoucherNo"].Width = 65;
			grd.RootTable.Columns["AccountCode"].Width = 85;
			grd.RootTable.Columns["OffSetTitle"].Width = 150;
			grd.RootTable.Columns["SubsidiaryAccount"].Width = 120;
			grd.RootTable.Columns["Debit"].Width = 120;
			grd.RootTable.Columns["Credit"].Width = 120;
			grd.RootTable.Columns["Balance"].Width = 130;
			grd.RootTable.Columns["BalType"].Width = 55;
			grd.RootTable.Columns["BillAmount"].Width = 100;
			grd.RootTable.Columns["DueDays"].Width = 60;
			grd.RootTable.Columns["DueDate"].Width = 90;
			grd.RootTable.Columns["Comments"].Width = 250;
			grd.RootTable.Columns["ChequeNo"].Width = 75;
			grd.RootTable.Columns["ChequeDate"].Width = 75;
			grd.RootTable.Columns["PayeeTitle"].Width = 80;
			grd.RootTable.Columns["JobLot"].Width = 110;
			grd.RootTable.Columns["MannualNo"].Width = 80;
			grd.RootTable.Columns["RefNo"].Width = 60;
			grd.RootTable.Columns["StockStatus"].Width = 80;
			grd.RootTable.Columns["QtyIn"].Width = 65;
			grd.RootTable.Columns["QtyOut"].Width = 65;
			grd.RootTable.Columns["ItemRate"].Width = 70;
			grd.RootTable.Columns["VehicleNo"].Width = 80;
			grd.RootTable.Columns["NoOfAttachments"].Width = 100;
			grd.RootTable.Columns["DocumentTypeSrNo"].Width = 100;
			grd.RootTable.Columns["IsApproved"].Width = 80;
			grd.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Balance"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["QtyIn"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["QtyOut"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["QtyIn"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["QtyOut"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QtyIn"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QtyOut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QtyIn"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QtyOut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VoucherNo"].ColumnType = (ColumnType)((UserAccount.RoleName == "Admin") ? 5 : (AllowEditOnVoucherNoinReports ? 5 : 0));
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DocType"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingsForSummery()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.AllowEdit = (InheritableBoolean)2;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["DecimlePointsRate"].Visible = false;
			grd.RootTable.Columns["DecimlePointsAmount"].Visible = false;
			grd.RootTable.Columns["AccountTitle"].Visible = false;
			grd.RootTable.Columns["CurrentDR/CrDiff"].Visible = false;
			grd.RootTable.Columns["VoucherHeadId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["BaseDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && Conversion.ToInt(cmbbranch.Value) > 0;
			grd.RootTable.Columns["BranchName"].Width = 120;
			grd.RootTable.Columns["VoucherDate"].Width = 90;
			grd.RootTable.Columns["DueDate"].Width = 90;
			grd.RootTable.Columns["DocType"].Width = 70;
			grd.RootTable.Columns["VoucherCode"].Width = 90;
			grd.RootTable.Columns["ManualBillNo"].Width = 100;
			grd.RootTable.Columns["AccountTitle"].Width = 300;
			grd.RootTable.Columns["Remarks"].Width = 200;
			grd.RootTable.Columns["DebitAmount"].Width = 110;
			grd.RootTable.Columns["CreditAmount"].Width = 110;
			grd.RootTable.Columns["RunningBalance"].Width = 110;
			grd.RootTable.Columns["DueDays"].Width = 60;
			grd.RootTable.Columns["IsApproved"].Width = 80;
			grd.RootTable.Columns["CurrentDR/CrDiff"].Width = 120;
			grd.RootTable.Columns["VehicleNo"].Width = 90;
			grd.RootTable.Columns["VoucherDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["DueDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["RunningBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrentDR/CrDiff"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CurrentDR/CrDiff"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CurrentDR/CrDiff"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RunningBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CurrentDR/CrDiff"].AggregateFunction = (AggregateFunction)2;
			if (AllowEditOnVoucherNoinReports)
			{
				grd.RootTable.Columns["VoucherCode"].ColumnType = (ColumnType)5;
			}
			grd.RootTable.Columns["DocType"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingsForSummeryFormatII()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.AllowEdit = (InheritableBoolean)2;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["VoucherHeadId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["BaseDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grd.RootTable.Columns["Balance"].Visible = false;
			grd.RootTable.Columns["AccountTitle"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && Conversion.ToInt(cmbbranch.Value) > 0;
			grd.RootTable.Columns["VoucherDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["ChequeDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["BranchName"].Width = 120;
			grd.RootTable.Columns["VoucherDate"].Width = 90;
			grd.RootTable.Columns["DueDate"].Width = 90;
			grd.RootTable.Columns["VoucherType"].Width = 75;
			grd.RootTable.Columns["VoucherNo"].Width = 65;
			grd.RootTable.Columns["AccountTitle"].Width = 200;
			grd.RootTable.Columns["OffSetAccountTitle"].Width = 200;
			grd.RootTable.Columns["Remarks"].Width = 400;
			grd.RootTable.Columns["DebitAmount"].Width = 110;
			grd.RootTable.Columns["CreditAmount"].Width = 110;
			grd.RootTable.Columns["Balance"].Width = 110;
			grd.RootTable.Columns["RunningBalance"].Width = 120;
			grd.RootTable.Columns["ChequeDate"].Width = 80;
			grd.RootTable.Columns["ChequeNo"].Width = 80;
			grd.RootTable.Columns["PayeeTitle"].Width = 120;
			grd.RootTable.Columns["DueDays"].Width = 60;
			grd.RootTable.Columns["DocumentTypeSrNo"].Width = 100;
			grd.RootTable.Columns["NoOfAttachments"].Width = 100;
			grd.RootTable.Columns["VoucherDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["DueDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["ChequeDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["RunningBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RunningBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			if (AllowEditOnVoucherNoinReports)
			{
				grd.RootTable.Columns["VoucherNo"].ColumnType = (ColumnType)5;
			}
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["VoucherType"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillDocTypeList(DataTable dt)
	{
		try
		{
			if (FilterRecordsByDocType)
			{
				if (dt.Rows.Count > 0)
				{
					BtnShowFilteredRecords.Visible = true;
					BtnShowFilteredRecords.Enabled = true;
					((Control)(object)GrdDocType).Visible = true;
					GrdDocType.DataSource = dt;
					GrdDocType.RetrieveStructure();
					GrdDocType.ColumnAutoResize = true;
					GrdDocType.TotalRow = (InheritableBoolean)2;
					GrdDocType.RootTable.Columns["DocumentTypeId"].Visible = false;
					GrdDocType.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
					GrdDocType.RootTable.Columns["Balance"].FormatString = clsGlobalVariables.stringFormatboth;
					GrdDocType.RootTable.Columns.Add("Select");
					GrdDocType.RootTable.Columns["Select"].ActAsSelector = true;
					GrdDocType.RootTable.Columns["Select"].UseHeaderSelector = true;
					GrdDocType.RootTable.Columns["Select"].Position = 0;
					GrdDocType.RootTable.Columns["Select"].Width = 25;
					GrdDocType.FrozenColumns = 1;
					GrdDocType.RootTable.Columns["D.Type"].Width = 50;
					((JanusCollectionBase)GrdDocType.RootTable.SortKeys).Clear();
					GrdDocType.RootTable.SortKeys.Add(GrdDocType.RootTable.Columns["DocumentTypeId"], (SortOrder)1);
				}
				else
				{
					GrdDocType.ClearStructure();
					((Control)(object)GrdDocType).Visible = false;
					BtnShowFilteredRecords.Visible = false;
				}
			}
			else
			{
				((Control)(object)GrdDocType).Visible = false;
				BtnShowFilteredRecords.Visible = false;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillBranchWiseBalance(DataTable dt)
	{
		try
		{
			if (BranchFeature)
			{
				if (dt.Rows.Count > 0)
				{
					DataTable dtNew = new DataTable();
					dtNew.Columns.Add("BranchesId", typeof(int));
					dtNew.Columns.Add("BranchName", typeof(string));
					dtNew.Columns.Add("Balance", typeof(double));
					foreach (DataRow item in dt.Rows)
					{
						DataRow dr = dtNew.NewRow();
						dr["BranchesId"] = Conversion.ToInt(item["BranchesId"]);
						dr["BranchName"] = Conversion.ToString(item["BranchName"]);
						dr["Balance"] = Conversion.ToString(item["Closing"]);
						dtNew.Rows.Add(dr);
					}
					grdBranchesWithBalance.DataSource = dtNew;
					grdBranchesWithBalance.RetrieveStructure();
					grdBranchesWithBalance.ColumnAutoResize = true;
					grdBranchesWithBalance.TotalRow = (InheritableBoolean)2;
					grdBranchesWithBalance.RootTable.Columns["BranchesId"].Visible = false;
					grdBranchesWithBalance.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
					grdBranchesWithBalance.RootTable.Columns["Balance"].FormatString = clsGlobalVariables.stringFormatboth;
					grdBranchesWithBalance.RootTable.Columns["Balance"].Width = 100;
					((JanusCollectionBase)grdBranchesWithBalance.RootTable.SortKeys).Clear();
					grdBranchesWithBalance.RootTable.SortKeys.Add(grdBranchesWithBalance.RootTable.Columns["BranchesId"], (SortOrder)1);
				}
				else
				{
					grdBranchesWithBalance.ClearStructure();
				}
			}
			else
			{
				gbBranchesWithBalance.Visible = false;
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grd.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			if (ChkDetail.Checked && FormInitializedForDetail)
			{
				int count = ((JanusCollectionBase)grd.RootTable.Columns).Count;
				int TableColumnsCount3 = dtForDetailGrid.Columns.Count;
				if (count != TableColumnsCount3)
				{
					foreach (string column3 in CommonServices.GetMissingColumns(dtForDetailGrid, xmlDoc))
					{
						string d3 = CommonServices.GetDataTypeString(dtForDetailGrid.Columns[column3].DataType);
						CommonServices.AddNewColumn(xmlDoc, fileName, column3, d3);
					}
				}
				FileStream fs4 = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
				if (fs4 != null)
				{
					grd.LoadLayoutFile((Stream)fs4);
					fs4.Close();
					fs4.Dispose();
				}
			}
			if (ChkSummery1.Checked && FormInitializedForSummary1)
			{
				int count2 = ((JanusCollectionBase)grd.RootTable.Columns).Count;
				int TableColumnsCount2 = dtForSummary1.Columns.Count;
				if (count2 != TableColumnsCount2)
				{
					foreach (string column2 in CommonServices.GetMissingColumns(dtForSummary1, xmlDoc))
					{
						string d2 = CommonServices.GetDataTypeString(dtForSummary1.Columns[column2].DataType);
						CommonServices.AddNewColumn(xmlDoc, fileName, column2, d2);
					}
				}
				FileStream fs3 = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
				if (fs3 != null)
				{
					grd.LoadLayoutFile((Stream)fs3);
					fs3.Close();
					fs3.Dispose();
				}
			}
			if (!ChkSummery2.Checked || !FormInitializedForSummary2)
			{
				return;
			}
			int count3 = ((JanusCollectionBase)grd.RootTable.Columns).Count;
			int TableColumnsCount = dtForSummary2.Columns.Count;
			if (count3 != TableColumnsCount)
			{
				foreach (string column in CommonServices.GetMissingColumns(dtForSummary2, xmlDoc))
				{
					string d = CommonServices.GetDataTypeString(dtForSummary2.Columns[column].DataType);
					CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
				}
			}
			FileStream fs2 = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs2 != null)
			{
				grd.LoadLayoutFile((Stream)fs2);
				fs2.Close();
				fs2.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int BaseDocumentTypeId = 0;
			int Id = Conversion.ToInt(currentRow.Cells["VoucherHeadId"].Value);
			int AccountId = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			DateTime VoucherDate = Conversion.ToDateTime(currentRow.Cells["VoucherDate"].Value);
			int DocumentTypeId = Conversion.ToInt((object)currentRow.Cells["DocumentTypeId"].Value.ToString());
			BaseDocumentTypeId = Conversion.ToInt(currentRow.Cells["BaseDocumentTypeId"].Value);
			int DocumentTypeIdSrNo = int.Parse(currentRow.Cells["DocumentTypeSrNo"].Value.ToString());
			if (e.Column.Key == "VoucherNo" || e.Column.Key == "VoucherCode")
			{
				if (DocumentTypeId == 80 || DocumentTypeId == 110 || DocumentTypeId == 111 || DocumentTypeId == 112 || DocumentTypeId == 181)
				{
					BaseDocumentTypeId = InvFoodProduction.InvFoodProduction_GetJobOrderIdByProduction(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId, DocumentTypeIdSrNo);
				}
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo, BaseDocumentTypeId);
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(DocumentTypeIdSrNo, DocumentTypeId);
			}
			if (e.Column.Key == "DocType" || e.Column.Key == "VoucherType")
			{
				CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, Id, AccountId, VoucherDate, VoucherDate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtDetail == null || dtDetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtDetail, "106-AcRptGeneralLedgerB.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSummeryLedger_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSummary1 == null || dtSummary1.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSummary1, "108-AcRptGeneralLedgerSummery.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void btnQuantativeLedger_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtDetail == null || dtDetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtDetail, "107-AcRptQuantativeLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheckBoxValidations()
	{
		try
		{
			if (ChkDetail.Checked)
			{
				ChkSummery1.Checked = false;
				ChkSummery2.Checked = false;
			}
			if (ChkSummery1.Checked)
			{
				ChkDetail.Checked = false;
				ChkSummery2.Checked = false;
			}
			if (ChkSummery2.Checked)
			{
				ChkDetail.Checked = false;
				ChkSummery1.Checked = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkDetail_Click(object sender, EventArgs e)
	{
		CheckBoxValidations();
	}

	private void ChkSummery1_Click(object sender, EventArgs e)
	{
		CheckBoxValidations();
	}

	private void ChkSummery2_Click(object sender, EventArgs e)
	{
		CheckBoxValidations();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			int IndexOfItemLedger = tabControl1.TabPages.IndexOf(tabPageItemLedger);
			tabControl1.TabPages.IndexOf(tabPageFcyGeneralLedger);
			if (tabControl1.SelectedIndex == IndexOfItemLedger)
			{
				txtFromDateItemLedger.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				txtToDateItemLedger.Value = DateTime.Today;
				txtFromDateItemLedger.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbAccountTitle_Leave(object sender, EventArgs e)
	{
		try
		{
			LedgerDetailByAccountId();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (((UltraGridBase)cmbAccountTitle).ActiveRow != null && ((Control)(object)cmbAccountTitle).Text != string.Empty)
				{
					SubsidiaryAccountBindByParentAccount(Conversion.ToInt(CmbCostCenter.Value));
				}
				else
				{
					CmbSubsidiaryAccountFill(Conversion.ToInt(CmbCostCenter.Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LedgerDetailByAccountId()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		try
		{
			PanelAccountDetail.Location = new Point(4, 22);
			lblPostedNoOfCheqs.Visible = false;
			ValuePostDatedNoOfCheqs.Visible = false;
			lblPostDatedCheqAmount.Visible = false;
			ValuePostDatedCheqAmount.Visible = false;
			DataTable dt = ChartofAccount.GetCoaDetailByAccountId(new ChartofAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbAccountTitle.Value)
			});
			if (dt.Rows.Count > 0)
			{
				gbAccountDetail.Visible = true;
				gbAccountOtherInfo.Visible = true;
				lblAccountCode.Text = Conversion.ToString(dt.Rows[0]["AccountCode"]);
				lblAccountClass.Text = Conversion.ToString(dt.Rows[0]["ClassName"]);
				lblAccountType.Text = Conversion.ToString(dt.Rows[0]["AccountType"]);
				if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
				{
					SubsidiaryBalanceBox.Visible = true;
					DataRow r = ((DataTable)((UltraGridBase)CmbSubsidiaryAccount).DataSource).AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("SubsidiaryAccountId") == Conversion.ToInt(CmbSubsidiaryAccount.Value));
					if (r != null)
					{
						SubsidiaryAccountName.Text = Conversion.ToString(r["SubsidaryAccount"]);
					}
				}
				else
				{
					SubsidiaryBalanceBox.Visible = false;
				}
			}
			else
			{
				gbAccountDetail.Visible = false;
				gbAccountOtherInfo.Visible = false;
				lblAccountClass.Text = "";
				lblAccountType.Text = "";
				lblAccountCode.Text = "";
			}
			DataTable Baldt = CommonServices.GetAccountsBalancesOpeningCurrentAndClosing(Conversion.ToInt(cmbAccountTitle.Value), txtFromDate.Value, txtToDate.Value, Conversion.ToInt(CmbCostCenter.Value));
			if (Baldt.Rows.Count > 0)
			{
				double Opening = Conversion.ToDouble(Baldt.Rows[0]["OpeningBalance"]);
				double CurrDebit = Conversion.ToDouble(Baldt.Rows[0]["DebitAmount"]);
				double CurrCredit = Conversion.ToDouble(Baldt.Rows[0]["CreditAmount"]);
				double Closing = Conversion.ToDouble(Baldt.Rows[0]["ClosingBalance"]);
				if (Opening > 0.0 || CurrDebit > 0.0 || CurrCredit > 0.0 || Closing > 0.0)
				{
					gbAccountDetail.Visible = true;
					gbAccountOtherInfo.Visible = true;
				}
				lblOpening.Text = Opening.ToString("#,#;(#,#);0") + ((Opening > 0.0) ? " Dr" : ((Opening < -0.99) ? " Cr" : ""));
				lblCurrDebit.Text = CurrDebit.ToString("#,#;(#,#);0");
				lblCurrCredit.Text = CurrCredit.ToString("#,#;(#,#);0");
				lblClosingBal.Text = Closing.ToString("#,#;(#,#);0") + ((Closing > 0.0) ? " Dr" : ((Closing < -0.99) ? " Cr" : ""));
				double PendingPreReceipts = Conversion.ToDouble(Baldt.Rows[0]["PendingPrematureReceipts"]);
				double PendingPrePayments = Conversion.ToDouble(Baldt.Rows[0]["PendingPrematureDebit"]);
				double BalAfterPreClearance = Conversion.ToDouble(Baldt.Rows[0]["BalanceAfterPrematureReceiptsClearance"]);
				double PdcReceipts = Conversion.ToDouble(Baldt.Rows[0]["PdcReceipts"]);
				double PdcPayments = Conversion.ToDouble(Baldt.Rows[0]["PdcPayments"]);
				double BalAfterPdcClearance = Conversion.ToDouble(Baldt.Rows[0]["BalanceAfterCheqClearance"]);
				txtPendingPrematureReceipts.Text = PendingPreReceipts.ToString("#,#;(#,#);0");
				txtPendingPrePayments.Text = PendingPrePayments.ToString("#,#;(#,#);0");
				txtBalAfterPreReceiptsClearance.Text = BalAfterPreClearance.ToString("#,#;(#,#);0") + ((BalAfterPreClearance > 0.0) ? " Dr" : ((BalAfterPreClearance < -0.99) ? " Cr" : ""));
				txtPdcReceipts.Text = PdcReceipts.ToString("#,#;(#,#);0");
				txtPdcPayments.Text = PdcPayments.ToString("#,#;(#,#);0");
				txtBalAfterCheqClearance.Text = BalAfterPdcClearance.ToString("#,#;(#,#);0") + ((BalAfterPdcClearance > 0.0) ? " Dr" : ((BalAfterPdcClearance < -0.99) ? " Cr" : ""));
				txtBalAfterCheqClearance.ForeColor = ((BalAfterPdcClearance > 0.0) ? Color.Red : ((BalAfterPdcClearance < 0.0) ? Color.DarkGreen : Color.Black));
				txtBalAfterPreReceiptsClearance.ForeColor = ((BalAfterPreClearance > 0.0) ? Color.Red : ((BalAfterPreClearance < 0.0) ? Color.DarkGreen : Color.Black));
				lblClosingBal.ForeColor = ((Closing > 0.0) ? Color.Red : ((Closing < 0.0) ? Color.DarkGreen : Color.Black));
			}
			else
			{
				gbAccountDetail.Visible = false;
				gbAccountOtherInfo.Visible = false;
				lblOpening.Text = "0";
				lblCurrDebit.Text = "0";
				lblCurrCredit.Text = "0";
				lblClosingBal.Text = "0";
				txtPendingPrematureReceipts.Text = "0";
				txtBalAfterPreReceiptsClearance.Text = "0";
				txtPdcReceipts.Text = "0";
				txtPdcPayments.Text = "0";
				txtBalAfterCheqClearance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSummary2_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSummary2 == null || dtSummary2.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSummary2, "105A-GeneralLedgerSummary2.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void rBForCheckAllPreviousRows_Click(object sender, EventArgs e)
	{
		try
		{
			CheckRowsWhichAreBookmard();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CheckRowsWhichAreBookmard()
	{
		try
		{
			if (dtDetail.Rows.Count <= 0)
			{
				return;
			}
			int LastIndex = GetIndexOfLastCheckedRow();
			if (LastIndex == -1)
			{
				MessageBox.Show("No Row Checked");
				return;
			}
			int counter = 0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (counter < LastIndex)
				{
					item.BeginEdit();
					if (Conversion.ToInt(item.Cells["DetailId"].Value) > 0)
					{
						item.Cells["Checked"].Value = true;
					}
					item.EndEdit();
					counter++;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private int GetIndexOfLastCheckedRow()
	{
		try
		{
			int LastIndex = -1;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToBool(item.Cells["Checked"].Value))
				{
					LastIndex = item.RowIndex;
				}
			}
			return LastIndex;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnSaveCheckedRows_Click(object sender, EventArgs e)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		try
		{
			int CheckedRowCount = 0;
			GridEXRow[] rows = grd.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (Conversion.ToBool(rows[i].Cells["Checked"].Value))
				{
					CheckedRowCount++;
				}
			}
			if (CheckedRowCount > 0)
			{
				if (MessageBox.Show("Are you sure to Save Checked Rows?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToBool(item.Cells["Checked"].Value) && Conversion.ToInt(item.Cells["DetailId"].Value) > 0)
					{
						ApprovalList detail = new ApprovalList();
						detail.Id = Conversion.ToInt(item.Cells["DetailId"].Value);
						detail.ActionTypeId = Conversion.ToBool(item.Cells["Checked"].Value);
						detail.Comments = Conversion.ToString(item.Cells["BookmarkRemarks"].Value);
						obj.AllApprovalLists.Add(detail);
					}
					else if (!Conversion.ToBool(item.Cells["Checked"].Value) && Conversion.ToInt(item.Cells["DetailId"].Value) > 0)
					{
						ApprovalList detail2 = new ApprovalList();
						detail2.Id = Conversion.ToInt(item.Cells["DetailId"].Value);
						detail2.ActionTypeId = Conversion.ToBool(item.Cells["Checked"].Value);
						detail2.Comments = Conversion.ToString(item.Cells["BookmarkRemarks"].Value);
						obj.AllApprovalLists.Add(detail2);
					}
				}
				VoucherHead.InsertBookmarkStatus(obj);
				MessageBox.Show("Bookmark Status Saved Successfully");
				btnshow_Click(null, null);
				return;
			}
			throw new Exception("No Rows Checked");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void StockAccountFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4,10");
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDL(dt, cmbStockAccountItemLedger, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
				((UltraGridBase)cmbStockAccountItemLedger).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetItemLedger()
	{
		try
		{
			((Control)(object)cmbStockAccountItemLedger).Text = string.Empty;
			txtFromDateItemLedger.Focus();
			txtFromDateItemLedger.Value = clsGlobalVariables.ActiveYr.Start_Period;
			grdItemLedger.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemByStockAccount()
	{
		try
		{
			DataTable dt = VoucherHead.GetItemByStockAccount(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbStockAccountItemLedger.Value));
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDL(dt, CmbItemNameItmLedger, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbItemNameItmLedger).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((Control)(object)CmbItemNameItmLedger).Text = string.Empty;
				((UltraGridBase)CmbItemNameItmLedger).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemLedgerFromAccounts()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = txtFromDateItemLedger.Value;
			obj.ToDate = txtToDateItemLedger.Value;
			obj.AccountId = Conversion.ToInt(cmbStockAccountItemLedger.Value);
			obj.ItemId = Conversion.ToInt(CmbItemNameItmLedger.Value);
			dtItemLedger = StocksReport.itemLedgerTradePro(obj);
			if (dtItemLedger.Rows.Count > 0)
			{
				grdItemLedger.DataSource = dtItemLedger;
				grdItemLedger.RetrieveStructure();
				ItemLedgerSetting();
			}
			else
			{
				grdItemLedger.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemLedgerSetting()
	{
		try
		{
			grdItemLedger.RootTable.Columns["RecNo"].Visible = false;
			grdItemLedger.RootTable.Columns["Id"].Visible = false;
			grdItemLedger.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdItemLedger.RootTable.Columns["AccountId"].Visible = false;
			grdItemLedger.RootTable.Columns["DecimalPointsRate"].Visible = false;
			grdItemLedger.RootTable.Columns["DecimalPointsAmount"].Visible = false;
			grdItemLedger.RootTable.Columns["GLAccountTitle"].Visible = false;
			grdItemLedger.RootTable.Columns["Comments"].Visible = false;
			grdItemLedger.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grdItemLedger.GroupByBoxVisible = true;
			grdItemLedger.GroupTotals = (GroupTotals)2;
			grdItemLedger.RootTable.Columns["QtyIn"].AggregateFunction = (AggregateFunction)2;
			grdItemLedger.RootTable.Columns["QtyOut"].AggregateFunction = (AggregateFunction)2;
			grdItemLedger.RootTable.Columns["WeightIn"].AggregateFunction = (AggregateFunction)2;
			grdItemLedger.RootTable.Columns["WeightOut"].AggregateFunction = (AggregateFunction)2;
			grdItemLedger.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdItemLedger.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdItemLedger.RootTable.Columns["QtyIn"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["QtyOut"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["QtyBal"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["WeightIn"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["WeightOut"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["WeightBal"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["BalanceAmount"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdItemLedger.RootTable.Columns["QtyIn"].FormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["QtyOut"].FormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["QtyBal"].FormatString = "#,#;(#,#);0";
			grdItemLedger.RootTable.Columns["WeightIn"].FormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["WeightOut"].FormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["WeightBal"].FormatString = "#,#;(#,#);0";
			grdItemLedger.RootTable.Columns["ItemRate"].FormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["DebitAmount"].FormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["CreditAmount"].FormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["BalanceAmount"].FormatString = "#,#;(#,#);0";
			grdItemLedger.RootTable.Columns["ItemRate"].FormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["QtyIn"].TotalFormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["QtyOut"].TotalFormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["QtyBal"].TotalFormatString = "#,#;(#,#);0";
			grdItemLedger.RootTable.Columns["WeightIn"].TotalFormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["WeightOut"].TotalFormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["WeightBal"].TotalFormatString = "#,#;(#,#);0";
			grdItemLedger.RootTable.Columns["DebitAmount"].TotalFormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["CreditAmount"].TotalFormatString = "#,#.##";
			grdItemLedger.RootTable.Columns["BalanceAmount"].TotalFormatString = "#,#;(#,#);0";
			grdItemLedger.RootTable.Columns["DocumentTypeCode"].ColumnType = (ColumnType)5;
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnRegisterItemLedger_Click(object sender, EventArgs e)
	{
	}

	private void btnNewItemLedger_Click(object sender, EventArgs e)
	{
		try
		{
			ResetItemLedger();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowItemLedger_Click(object sender, EventArgs e)
	{
		try
		{
			ItemLedgerFromAccounts();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdItemLedger).Name);
			if (fs != null)
			{
				grdItemLedger.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnItemLedger_164_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtItemLedger == null || dtItemLedger.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtItemLedger, "164-AcRptQuantativeLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemLedger_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdItemLedger.CurrentRow;
			int Id = int.Parse(currentRow.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)currentRow.Cells["DocumentTypeId"].Value.ToString());
			int DocumentTypeIdSrNo = int.Parse(currentRow.Cells["DocumentTypeSrNo"].Value.ToString());
			if (e.Column.Key == "DocumentTypeCode")
			{
				CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbStockAccountItemLedger_Leave(object sender, EventArgs e)
	{
		try
		{
			GetItemByStockAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGetAccounts_Click(object sender, EventArgs e)
	{
		try
		{
			AllAccountsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private async void btnShowMultiLedger_Click1(object sender, EventArgs e)
	{
		try
		{
			grdMultiAccountsLedger.ClearStructure();
			string AccountIds = "";
			if (grdAccounts.GetCheckedRows().Length <= 0)
			{
				MessageBox.Show("No Account Selected in Details Accounts Side. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			GridEXRow[] checkedRows = grdAccounts.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				AccountIds = AccountIds + item.Cells["Id"].Value?.ToString() + ",";
			}
			dtMultiDetail.Rows.Clear();
			dtMultiSummary1.Rows.Clear();
			dtMultiSummary2.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = FromDateForMulti.Value;
			obj.ToDate = ToDateForMulti.Value;
			obj.EntryUser = UserAccount.ID;
			obj.AccountIds = AccountIds;
			if (!chkIncludeUnpostedVouchers.Checked)
			{
				obj.IsApproved = true;
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			ApiResponse<object> response = await ApiService.PostAsync<ApiResponse<object>>("api/AccountsReports/GeneralLedgerWithOffsetAccount", obj);
			if (!response.Status)
			{
				throw new Exception(response.Message);
			}
			dynamic jsonTable = ((dynamic)response.Data).Result.ToString();
			dtMultiDetail = JsonConvert.DeserializeObject<DataTable>(jsonTable);
			if (dtMultiDetail.Rows.Count > 0 && dtMultiDetail != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("DetailId", typeof(int));
				dt.Columns.Add("VoucherDate", typeof(DateTime));
				dt.Columns.Add("DocType");
				dt.Columns.Add("VoucherNo", typeof(int));
				dt.Columns.Add("AccountTitle");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("OffSetTitle");
				dt.Columns.Add("SubsidiaryAccount");
				dt.Columns.Add("Debit", typeof(double));
				dt.Columns.Add("Credit", typeof(double));
				dt.Columns.Add("Balance", typeof(double));
				dt.Columns.Add("BalType");
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("Comments");
				dt.Columns.Add("ChequeNo");
				dt.Columns.Add("ChequeDate", typeof(DateTime));
				dt.Columns.Add("JobLot");
				dt.Columns.Add("MannualNo");
				dt.Columns.Add("RefNo");
				dt.Columns.Add("QtyIn", typeof(double));
				dt.Columns.Add("QtyOut", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("NoOfAttachments", typeof(double));
				dt.Columns.Add("VoucherHeadId");
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("BaseDocumentTypeId");
				dt.Columns.Add("DocumentTypeSrNo");
				dt.Columns.Add("IsApproved");
				for (int i = 0; i < dtMultiDetail.Rows.Count; i++)
				{
					string type = "Nill";
					if (Conversion.ToDouble(dtMultiDetail.Rows[i]["Balance"]) > 0.0)
					{
						type = "Dr";
					}
					else if (Conversion.ToDouble(dtMultiDetail.Rows[i]["Balance"]) < 0.0)
					{
						type = "Cr";
					}
					dt.Rows.Add(dtMultiDetail.Rows[i]["DetailId"], Conversion.ToDateTime(dtMultiDetail.Rows[i]["VoucherDate"]).ToShortDateString(), dtMultiDetail.Rows[i]["DocTypeCode"], dtMultiDetail.Rows[i]["VoucherCode"], dtMultiDetail.Rows[i]["AccountTitle"], dtMultiDetail.Rows[i]["AccountCode"], dtMultiDetail.Rows[i]["OffsetAccountTitle"], dtMultiDetail.Rows[i]["SubsidiaryAccount"], dtMultiDetail.Rows[i]["DebitAmount"], dtMultiDetail.Rows[i]["CreditAmount"], dtMultiDetail.Rows[i]["Balance"], type, dtMultiDetail.Rows[i]["BillAmount"], dtMultiDetail.Rows[i]["Comments"], dtMultiDetail.Rows[i]["CheqNoDetail"], Conversion.ToDateTime(dtMultiDetail.Rows[i]["CheqDate"]).ToShortDateString(), dtMultiDetail.Rows[i]["JobLot"], dtMultiDetail.Rows[i]["MannualNo"], dtMultiDetail.Rows[i]["RefInvoiceNo"], dtMultiDetail.Rows[i]["QtyIn"], dtMultiDetail.Rows[i]["QtyOut"], dtMultiDetail.Rows[i]["ItemRate"], dtMultiDetail.Rows[i]["NoOfAttachments"], dtMultiDetail.Rows[i]["VoucherHeadId"], dtMultiDetail.Rows[i]["DocumentTypeId"], dtMultiDetail.Rows[i]["BaseDocumentTypeId"], dtMultiDetail.Rows[i]["DocumentTypeSrNo"], dtMultiDetail.Rows[i]["IsApproved"]);
				}
				grdMultiAccountsLedger.DataSource = dt;
				grdMultiAccountsLedger.RetrieveStructure();
				GridDetailMultiSettings();
				return;
			}
			grdMultiAccountsLedger.ClearStructure();
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	private void btnShowMultiLedger_Click(object sender, EventArgs e)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		try
		{
			grdMultiAccountsLedger.DataSource = null;
			grdMultiAccountsLedger.ClearStructure();
			GridEXRow[] checkedRows = grdAccounts.GetCheckedRows();
			if (checkedRows.Length == 0)
			{
				MessageBox.Show("No Account Selected in Details Accounts Side. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			string AccountIds = string.Join(",", checkedRows.Select((GridEXRow r) => r.Cells["Id"].Value.ToString()));
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = FromDateForMulti.Value,
				ToDate = ToDateForMulti.Value,
				EntryUser = UserAccount.ID,
				AccountIds = AccountIds
			};
			if (!chkIncludeUnpostedVouchers.Checked)
			{
				obj.IsApproved = true;
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			Cursor.Current = Cursors.WaitCursor;
			if (rdDetailPrint.Checked)
			{
				((Control)(object)grdMultiAccountsLedger).Name = "Detailgrd";
				btn105MultiPrint.Visible = true;
				btn106MultiPrint.Visible = true;
				btn107MultiPrint.Visible = true;
				btn108MultiPrint.Visible = false;
				btn105AMultiPrint.Visible = false;
				dtMultiDetail = VoucherReports.GeneralLedgerWithOffsetAccount(obj);
				if (dtMultiDetail == null || dtMultiDetail.Rows.Count == 0)
				{
					grdMultiAccountsLedger.ClearStructure();
					throw new Exception("Record Not Found For Display");
				}
				DataTable dt = CreateDetailTable();
				dt.BeginLoadData();
				foreach (DataRow row in dtMultiDetail.Rows)
				{
					double balance = ((row["Balance"] == DBNull.Value) ? 0.0 : Convert.ToDouble(row["Balance"]));
					string type = ((balance > 0.0) ? "Dr" : ((balance < 0.0) ? "Cr" : "Nill"));
					dt.Rows.Add(row["DetailId"], row["VoucherDate"], row["DocTypeCode"], row["VoucherCode"], row["AccountTitle"], row["AccountCode"], row["OffsetAccountTitle"], row["SubsidiaryAccount"], row["DebitAmount"], row["CreditAmount"], row["Balance"], type, row["BillAmount"], row["Comments"], row["CheqNoDetail"], (row["CheqDate"] == DBNull.Value) ? DBNull.Value : row["CheqDate"], row["JobLot"], row["MannualNo"], row["RefInvoiceNo"], row["QtyIn"], row["QtyOut"], row["ItemRate"], row["NoOfAttachments"], row["VoucherHeadId"], row["DocumentTypeId"], row["BaseDocumentTypeId"], row["DocumentTypeSrNo"], row["IsApproved"]);
				}
				dt.EndLoadData();
				grdMultiAccountsLedger.DataSource = dt;
				grdMultiAccountsLedger.RetrieveStructure();
				GridDetailMultiSettings();
			}
			else if (rdSummary1Print.Checked)
			{
				((Control)(object)grdMultiAccountsLedger).Name = "SummaryGrd";
				btn108MultiPrint.Visible = true;
				btn105MultiPrint.Visible = false;
				btn106MultiPrint.Visible = false;
				btn107MultiPrint.Visible = false;
				btn105AMultiPrint.Visible = false;
				btnSaveCheckedRows.Visible = false;
				dtMultiSummary1 = VoucherReports.GeneralLedgerSummery(obj);
				if (dtMultiSummary1 == null || dtMultiSummary1.Rows.Count == 0)
				{
					grdMultiAccountsLedger.ClearStructure();
					return;
				}
				DataTable dt2 = CreateSummary1Table();
				dt2.BeginLoadData();
				foreach (DataRow row2 in dtMultiSummary1.Rows)
				{
					dt2.Rows.Add(row2["VoucherCode"], row2["VoucherDate"], row2["DocumentTypeDescription"], row2["ManualBillNo"], row2["ParentAccountCode"], row2["AccountId"], row2["AccountTitle"], row2["DebitAmount"], row2["CreditAmount"], row2["balance"], row2["RunningBalance"]);
				}
				dt2.EndLoadData();
				grdMultiAccountsLedger.DataSource = dt2;
				grdMultiAccountsLedger.RetrieveStructure();
				GridSettingsForSummeryMulty();
			}
			else
			{
				if (!rdSummary2Print.Checked)
				{
					return;
				}
				((Control)(object)grdMultiAccountsLedger).Name = "Summary2Grd";
				btn105AMultiPrint.Visible = true;
				btn105MultiPrint.Visible = false;
				btn106MultiPrint.Visible = false;
				btn107MultiPrint.Visible = false;
				btn108MultiPrint.Visible = false;
				dtMultiSummary2 = VoucherReports.GeneralLedger2Format_Rpt(obj);
				if (dtMultiSummary2 == null || dtMultiSummary2.Rows.Count == 0)
				{
					grdMultiAccountsLedger.ClearStructure();
					return;
				}
				DataTable dt3 = CreateSummary2Table();
				dt3.BeginLoadData();
				double runningOpening = 0.0;
				for (int i = 0; i < dtMultiSummary2.Rows.Count; i++)
				{
					DataRow row3 = dtMultiSummary2.Rows[i];
					double debit = ((row3["DebitAmount"] == DBNull.Value) ? 0.0 : Convert.ToDouble(row3["DebitAmount"]));
					double credit = ((row3["CreditAmount"] == DBNull.Value) ? 0.0 : Convert.ToDouble(row3["CreditAmount"]));
					if (i == 0)
					{
						runningOpening = Convert.ToDouble(row3["RunningBalance"]);
					}
					double runningBalance = runningOpening + debit - credit;
					dt3.Rows.Add(row3["VoucherDate"], row3["VoucherType"], row3["VoucherNo"], row3["AccountTitle"], row3["OffSetAccountTitle"], row3["Remarks"], debit, credit, row3["RunningBalance"], runningBalance, (row3["ChequeDate"] == DBNull.Value) ? DBNull.Value : row3["ChequeDate"], row3["ChequeNo"], row3["VoucherHeadId"], row3["DocumentTypeId"], row3["DocumentTypeSrNo"], row3["NoOfAttachments"]);
					runningOpening = runningBalance;
				}
				dt3.EndLoadData();
				grdMultiAccountsLedger.DataSource = dt3;
				grdMultiAccountsLedger.RetrieveStructure();
				GridSettingsForSummeryFormatIIMulty();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	private DataTable CreateDetailTable()
	{
		return new DataTable
		{
			Columns = 
			{
				{
					"DetailId",
					typeof(int)
				},
				{
					"VoucherDate",
					typeof(DateTime)
				},
				"DocType",
				{
					"VoucherNo",
					typeof(int)
				},
				"AccountTitle",
				"AccountCode",
				"OffSetTitle",
				"SubsidiaryAccount",
				{
					"Debit",
					typeof(double)
				},
				{
					"Credit",
					typeof(double)
				},
				{
					"Balance",
					typeof(double)
				},
				"BalType",
				{
					"BillAmount",
					typeof(double)
				},
				"Comments",
				"ChequeNo",
				{
					"ChequeDate",
					typeof(DateTime)
				},
				"JobLot",
				"MannualNo",
				"RefNo",
				{
					"QtyIn",
					typeof(double)
				},
				{
					"QtyOut",
					typeof(double)
				},
				{
					"ItemRate",
					typeof(double)
				},
				{
					"NoOfAttachments",
					typeof(double)
				},
				"VoucherHeadId",
				"DocumentTypeId",
				"BaseDocumentTypeId",
				"DocumentTypeSrNo",
				"IsApproved"
			}
		};
	}

	private DataTable CreateSummary1Table()
	{
		return new DataTable
		{
			Columns = 
			{
				{
					"VoucherCode",
					typeof(int)
				},
				{
					"VoucherDate",
					typeof(DateTime)
				},
				"DocumentType",
				"ManualBillNo",
				"ParentAccountCode",
				{
					"AccountId",
					typeof(int)
				},
				"AccountTitle",
				{
					"DebitAmount",
					typeof(double)
				},
				{
					"CreditAmount",
					typeof(double)
				},
				{
					"balance",
					typeof(double)
				},
				{
					"RunningBalance",
					typeof(double)
				}
			}
		};
	}

	private DataTable CreateSummary2Table()
	{
		return new DataTable
		{
			Columns = 
			{
				{
					"VoucherDate",
					typeof(DateTime)
				},
				"VoucherType",
				{
					"VoucherNo",
					typeof(int)
				},
				"AccountTitle",
				"OffSetAccountTitle",
				"Remarks",
				{
					"DebitAmount",
					typeof(double)
				},
				{
					"CreditAmount",
					typeof(double)
				},
				{
					"Balance",
					typeof(double)
				},
				{
					"RunningBalance",
					typeof(double)
				},
				{
					"ChequeDate",
					typeof(DateTime)
				},
				"ChequeNo",
				"VoucherHeadId",
				"DocumentTypeId",
				"DocumentTypeSrNo",
				"NoOfAttachments"
			}
		};
	}

	private void GridDetailMultiSettings()
	{
		try
		{
			grdMultiAccountsLedger.RootTable.Groups.Add("AccountTitle");
			grdMultiAccountsLedger.RootTable.GroupTotals = (GroupTotals)2;
			grdMultiAccountsLedger.RootTable.Columns["Balance"].Caption = "RunningBalance";
			grdMultiAccountsLedger.RootTable.Columns["SubsidiaryAccount"].Caption = "Subsidiary Ac";
			List<string> hiddenColumns = new List<string> { "VoucherHeadId", "DetailId", "BaseDocumentTypeId", "DocumentTypeSrNo", "DocumentTypeId", "AccountTitle", "IsApproved", "RefNo", "SubsidiaryAccount" };
			List<string> excludetotals = new List<string> { "Balance" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdMultiAccountsLedger, 2, 0, hiddenColumns, null, null, ColumnSetting: true, BracketsForamt: false, excludetotals);
			grdMultiAccountsLedger.RootTable.Columns["VoucherDate"].FormatString = "dd-MMM-yyyy";
			grdMultiAccountsLedger.RootTable.Columns["ChequeDate"].FormatString = "dd-MMM-yyyy";
			grdMultiAccountsLedger.RootTable.Columns["VoucherDate"].Width = 90;
			grdMultiAccountsLedger.RootTable.Columns["DocType"].Width = 65;
			grdMultiAccountsLedger.RootTable.Columns["VoucherNo"].Width = 70;
			grdMultiAccountsLedger.RootTable.Columns["AccountTitle"].Width = 180;
			grdMultiAccountsLedger.RootTable.Columns["AccountCode"].Width = 80;
			grdMultiAccountsLedger.RootTable.Columns["OffSetTitle"].Width = 180;
			grdMultiAccountsLedger.RootTable.Columns["SubsidiaryAccount"].Width = 180;
			grdMultiAccountsLedger.RootTable.Columns["Debit"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["Credit"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["Balance"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["BalType"].Width = 55;
			grdMultiAccountsLedger.RootTable.Columns["BillAmount"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["Comments"].Width = 180;
			grdMultiAccountsLedger.RootTable.Columns["ChequeNo"].Width = 85;
			grdMultiAccountsLedger.RootTable.Columns["ChequeDate"].Width = 90;
			grdMultiAccountsLedger.RootTable.Columns["JobLot"].Width = 100;
			grdMultiAccountsLedger.RootTable.Columns["MannualNo"].Width = 85;
			grdMultiAccountsLedger.RootTable.Columns["RefNo"].Width = 65;
			grdMultiAccountsLedger.RootTable.Columns["QtyIn"].Width = 90;
			grdMultiAccountsLedger.RootTable.Columns["QtyOut"].Width = 90;
			grdMultiAccountsLedger.RootTable.Columns["ItemRate"].Width = 90;
			grdMultiAccountsLedger.RootTable.Columns["NoOfAttachments"].Width = 100;
			grdMultiAccountsLedger.RootTable.Columns["DocumentTypeSrNo"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["IsApproved"].Width = 80;
			grdMultiAccountsLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdMultiAccountsLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["Debit"].FormatString = "#,##0.####";
			grdMultiAccountsLedger.RootTable.Columns["Debit"].TotalFormatString = "#,##0.####";
			grdMultiAccountsLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdMultiAccountsLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["Credit"].FormatString = "#,#;(#,#);0";
			grdMultiAccountsLedger.RootTable.Columns["Credit"].TotalFormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["QtyIn"].AggregateFunction = (AggregateFunction)2;
			grdMultiAccountsLedger.RootTable.Columns["QtyIn"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["QtyIn"].FormatString = "#,#;(#,#);0";
			grdMultiAccountsLedger.RootTable.Columns["QtyIn"].TotalFormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["QtyOut"].AggregateFunction = (AggregateFunction)2;
			grdMultiAccountsLedger.RootTable.Columns["QtyOut"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["QtyOut"].FormatString = "#,#;(#,#);0";
			grdMultiAccountsLedger.RootTable.Columns["QtyOut"].TotalFormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["ItemRate"].FormatString = "#,#;(#,#.##);0";
			grdMultiAccountsLedger.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["BillAmount"].FormatString = "#,#;(#,#.##);0";
			grdMultiAccountsLedger.RootTable.Columns["BillAmount"].TotalFormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["Balance"].FormatString = "#,#;(#,#);0";
			if (AllowEditOnVoucherNoinReports)
			{
				grdMultiAccountsLedger.RootTable.Columns["VoucherNo"].ColumnType = (ColumnType)5;
			}
			grdMultiAccountsLedger.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdMultiAccountsLedger.RootTable.Columns["DocType"].ColumnType = (ColumnType)5;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingsForSummeryMulty()
	{
		try
		{
			grdMultiAccountsLedger.RootTable.Groups.Add("AccountTitle");
			grdMultiAccountsLedger.RootTable.GroupTotals = (GroupTotals)2;
			grdMultiAccountsLedger.RootTable.Columns["AccountTitle"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["ParentAccountCode"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["AccountId"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["balance"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["VoucherCode"].Width = 95;
			grdMultiAccountsLedger.RootTable.Columns["VoucherDate"].Width = 100;
			grdMultiAccountsLedger.RootTable.Columns["DocumentType"].Width = 120;
			grdMultiAccountsLedger.RootTable.Columns["ManualBillNo"].Width = 100;
			grdMultiAccountsLedger.RootTable.Columns["ParentAccountCode"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["AccountTitle"].Width = 180;
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["balance"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["RunningBalance"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].FormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].TotalFormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].FormatString = "#,#;(#,#);0";
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].TotalFormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["RunningBalance"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["RunningBalance"].FormatString = "#,#;(#,#);0";
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingsForSummeryFormatIIMulty()
	{
		try
		{
			grdMultiAccountsLedger.RootTable.Groups.Add("AccountTitle");
			grdMultiAccountsLedger.RootTable.GroupTotals = (GroupTotals)2;
			grdMultiAccountsLedger.RootTable.Columns["AccountTitle"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["Balance"].Visible = false;
			grdMultiAccountsLedger.RootTable.Columns["VoucherDate"].Width = 95;
			grdMultiAccountsLedger.RootTable.Columns["VoucherType"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["VoucherNo"].Width = 80;
			grdMultiAccountsLedger.RootTable.Columns["AccountTitle"].Width = 180;
			grdMultiAccountsLedger.RootTable.Columns["OffSetAccountTitle"].Width = 180;
			grdMultiAccountsLedger.RootTable.Columns["Remarks"].Width = 180;
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["Balance"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["RunningBalance"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["ChequeDate"].Width = 90;
			grdMultiAccountsLedger.RootTable.Columns["ChequeNo"].Width = 80;
			grdMultiAccountsLedger.RootTable.Columns["DocumentTypeSrNo"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["NoOfAttachments"].Width = 110;
			grdMultiAccountsLedger.RootTable.Columns["VoucherDate"].FormatString = "dd-MMM-yyyy";
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].FormatString = "#,#;#,#;0";
			grdMultiAccountsLedger.RootTable.Columns["DebitAmount"].TotalFormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].FormatString = "#,#;(#,#);0";
			grdMultiAccountsLedger.RootTable.Columns["CreditAmount"].TotalFormatString = "#,#";
			grdMultiAccountsLedger.RootTable.Columns["RunningBalance"].TextAlignment = (TextAlignment)3;
			grdMultiAccountsLedger.RootTable.Columns["RunningBalance"].FormatString = "#,#;(#,#);0";
			if (AllowEditOnVoucherNoinReports)
			{
				grdMultiAccountsLedger.RootTable.Columns["VoucherNo"].ColumnType = (ColumnType)5;
			}
			grdMultiAccountsLedger.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdMultiAccountsLedger.RootTable.Columns["VoucherType"].ColumnType = (ColumnType)5;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdMultiAccountsLedger).Name);
			if (fs != null)
			{
				grdMultiAccountsLedger.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void CmbDateTypeMulti_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateTypeMulti.Value) == 1)
			{
				FromDateForMulti.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeMulti.Value) == 2)
			{
				FromDateForMulti.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(CmbDateTypeMulti.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDateForMulti.Value = first;
				ToDateForMulti.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeMulti.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				FromDateForMulti.Value = first2;
				ToDateForMulti.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeMulti.Value) == 5)
			{
				FromDateForMulti.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn105MultiPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtMultiDetail == null || dtMultiDetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtMultiDetail, "105-AcRptGeneralLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn105AMultiPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtMultiSummary2 == null || dtMultiSummary2.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtMultiSummary2, "105A-GeneralLedgerSummary2.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void btn106MultiPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtMultiDetail == null || dtMultiDetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtMultiDetail, "106-AcRptGeneralLedgerB.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn107MultiPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtMultiDetail == null || dtMultiDetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtMultiDetail, "107-AcRptQuantativeLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn108MultiPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtMultiSummary1 == null || dtMultiSummary1.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtMultiSummary1, "108-AcRptGeneralLedgerSummery.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void grdMultiAccountsLedger_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdMultiAccountsLedger.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["VoucherHeadId"].Value);
			int DocumentTypeId = Conversion.ToInt(currentRow.Cells["DocumentTypeId"].Value);
			int BaseDocumentTypeId = Conversion.ToInt(currentRow.Cells["BaseDocumentTypeId"].Value);
			int DocumentTypeIdSrNo = Conversion.ToInt(currentRow.Cells["DocumentTypeSrNo"].Value);
			if (e.Column.Key == "VoucherNo" || e.Column.Key == "VoucherCode")
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo, BaseDocumentTypeId);
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(DocumentTypeIdSrNo, DocumentTypeId);
			}
			if (e.Column.Key == "DocType" || e.Column.Key == "VoucherType")
			{
				CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh1_Click(object sender, EventArgs e)
	{
		try
		{
			CustomGroupFill();
			GroupAccountFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			datToDate.Value = DateTime.Now;
			((Control)(object)CmbGroupAccount).Text = string.Empty;
			((Control)(object)cmbCustomGroup).Text = string.Empty;
			((UltraGridBase)CmbDateTypeMulti).Rows[5].Activate();
			grdAccounts.ClearStructure();
			grdMultiAccountsLedger.ClearStructure();
			datToDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowBankSummLedger_Click(object sender, EventArgs e)
	{
		try
		{
			GetClosingByToDateAndBankType();
			FillDetailGrdBankSummLedger();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetClosingByToDateAndBankType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			DataTable dt = ChartofAccount.GetAccountsWithBalanceByGroupIdsOrCustomGroups(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ToDate = txttoDateBankSummLedger.Value,
				AccountTypeId = 15,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				GridEXRow[] rows = grdAccountsBankSummLedger.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dt.Rows[i]["AccountId"]) == Conversion.ToInt(item.Cells["Id"].Value))
					{
						item.BeginEdit();
						item.Cells["Closing"].Value = Conversion.ToDouble(dt.Rows[i]["Balance"]);
						item.EndEdit();
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdAccountsBankSummLedger_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdAccountsBankSummLedger.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "AccountCode")
			{
				tabControl2.SelectedIndex = 0;
				cmbAccountTitle.Value = Id;
				if (SubsidiaryAccountAllownOnVouchers)
				{
					((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
				}
				cmbAccountTitle_Leave(null, null);
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAccountFillBankSummary()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("15");
			if (dt.Rows.Count > 0)
			{
				DataTable dtAccounts = new DataTable();
				dtAccounts.Columns.Add("Id", typeof(int));
				dtAccounts.Columns.Add("AccountCode");
				dtAccounts.Columns.Add("AccountTitle");
				dtAccounts.Columns.Add("Closing", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtAccounts.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTitle"], 0);
				}
				grdAccountsBankSummLedger.DataSource = dtAccounts;
				grdAccountsBankSummLedger.RetrieveStructure();
				grdAccountsBankSummLedger.ColumnAutoResize = true;
				grdAccountsBankSummLedger.RootTable.Columns["Id"].Visible = false;
				grdAccountsBankSummLedger.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
				grdAccountsBankSummLedger.RootTable.Columns.Add("Select");
				grdAccountsBankSummLedger.RootTable.Columns["Select"].ActAsSelector = true;
				grdAccountsBankSummLedger.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdAccountsBankSummLedger.RootTable.Columns["Select"].Position = 0;
				grdAccountsBankSummLedger.RootTable.Columns["Select"].Width = 25;
				grdAccountsBankSummLedger.FrozenColumns = 1;
				grdAccountsBankSummLedger.RootTable.Columns["AccountCode"].Width = 90;
				grdAccountsBankSummLedger.RootTable.Columns["AccountTitle"].Width = 250;
				grdAccountsBankSummLedger.RootTable.Columns["Closing"].Width = 120;
				grdAccountsBankSummLedger.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
				grdAccountsBankSummLedger.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
				grdAccountsBankSummLedger.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
				grdAccountsBankSummLedger.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			}
			else
			{
				grdAccountsBankSummLedger.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailGrdBankSummLedger()
	{
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		try
		{
			grdDetailBankSummLedger.ClearStructure();
			string AccountIds = "";
			if (grdAccountsBankSummLedger.GetCheckedRows().Length <= 0)
			{
				MessageBox.Show("No Account Selected in Details Accounts Side. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			GridEXRow[] checkedRows = grdAccountsBankSummLedger.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				AccountIds = AccountIds + item.Cells["Id"].Value?.ToString() + ",";
			}
			string AccountTypeIds = "";
			if (((Control)(object)CmbAccountTypesForBankSummaryLedger).Text != null && ((Control)(object)CmbAccountTypesForBankSummaryLedger).Text != string.Empty)
			{
				string[] array = Conversion.ToString((object)((Control)(object)CmbAccountTypesForBankSummaryLedger).Text).ToString().Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtAccountTypes.Select("AccountType='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						AccountTypeIds = AccountTypeIds + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
			}
			dtBankSummLedger.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = txtFromDateBankSummLedger.Value;
			obj.ToDate = txttoDateBankSummLedger.Value;
			obj.AccountIds = AccountIds;
			obj.AccountTypeIds = AccountTypeIds;
			obj.AccountTypeId = 15;
			dtBankSummLedger = VoucherReports.BankSummaryLedgersWithAgainstsAccounts(obj);
			if (dtBankSummLedger != null && dtBankSummLedger.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("BankAccountId", typeof(int));
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("BankAccount", typeof(string));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("SubsidiaryAccountId", typeof(int));
				dt.Columns.Add("SubsidiaryTitle", typeof(string));
				dt.Columns.Add("BankOpening", typeof(decimal));
				dt.Columns.Add("DebitAmount", typeof(decimal));
				dt.Columns.Add("CreditAmount", typeof(decimal));
				dt.Columns.Add("BankClosing", typeof(decimal));
				DataTable dtSumm = new DataTable();
				dtSumm.Columns.Add("BankAccountId", typeof(int));
				dtSumm.Columns.Add("BankAccount", typeof(string));
				dtSumm.Columns.Add("Opening", typeof(decimal));
				dtSumm.Columns.Add("Debit", typeof(decimal));
				dtSumm.Columns.Add("Credit", typeof(decimal));
				dtSumm.Columns.Add("Closing", typeof(decimal));
				foreach (DataRow row in dtBankSummLedger.Rows)
				{
					dt.Rows.Add(row["BankAccountId"], row["AccountId"], row["BankAccount"], row["AccountTitle"], row["SubsidiaryAccountId"], row["SubsidiaryTitle"], row["BankOpening"], row["DebitAmount"], row["CrediAmount"], row["BankClosing"]);
					if (!dtSumm.AsEnumerable().Any((DataRow rr) => rr.Field<int>("BankAccountId") == Conversion.ToInt(row["BankAccountId"])))
					{
						dtSumm.Rows.Add(row["BankAccountId"], row["BankAccount"], row["BankOpening"], row["TotalDebitAmount"], row["TotalCreditAmount"], row["BankClosing"]);
					}
				}
				grdDetailBankSummLedger.DataSource = dt;
				grdDetailBankSummLedger.RetrieveStructure();
				GrdDetailBankSummLedgerSettings();
				grdSummaryValuesBanksummLegder.DataSource = dtSumm;
				grdSummaryValuesBanksummLegder.RetrieveStructure();
				GrdSummaryValuesBankSummLedgerSettings();
				return;
			}
			grdDetailBankSummLedger.ClearStructure();
			grdSummaryValuesBanksummLegder.ClearStructure();
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdDetailBankSummLedgerSettings()
	{
		try
		{
			if (grdAccountsBankSummLedger.GetCheckedRows().Length == 1)
			{
				grdDetailBankSummLedger.RootTable.Groups.Add("BankAccount");
				grdDetailBankSummLedger.RootTable.GroupTotals = (GroupTotals)2;
				grdDetailBankSummLedger.RootTable.Columns["BankAccount"].Visible = false;
				BtnBankSummaryPrint02.Visible = false;
				BtnPrintBanksummLegder.Visible = true;
			}
			grdDetailBankSummLedger.RootTable.Columns["BankAccountId"].Visible = false;
			grdDetailBankSummLedger.RootTable.Columns["AccountId"].Visible = false;
			grdDetailBankSummLedger.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grdDetailBankSummLedger.RootTable.Columns["BankOpening"].Visible = false;
			grdDetailBankSummLedger.RootTable.Columns["BankClosing"].Visible = false;
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (grdAccountsBankSummLedger.GetCheckedRows().Length > 1)
				{
					grdDetailBankSummLedger.RootTable.Groups.Add("SubsidiaryTitle");
					grdDetailBankSummLedger.RootTable.GroupTotals = (GroupTotals)2;
					grdDetailBankSummLedger.RootTable.Columns["SubsidiaryTitle"].Visible = false;
					grdDetailBankSummLedger.RootTable.Columns["AccountTitle"].Visible = false;
					BtnBankSummaryPrint02.Visible = true;
					BtnPrintBanksummLegder.Visible = false;
				}
				else
				{
					grdDetailBankSummLedger.RootTable.Columns["AccountTitle"].Visible = false;
					grdDetailBankSummLedger.RootTable.Columns["SubsidiaryTitle"].Visible = true;
				}
			}
			else
			{
				if (grdAccountsBankSummLedger.GetCheckedRows().Length > 1)
				{
					grdDetailBankSummLedger.RootTable.Groups.Add("AccountTitle");
					grdDetailBankSummLedger.RootTable.GroupTotals = (GroupTotals)2;
					grdDetailBankSummLedger.RootTable.Columns["AccountTitle"].Visible = false;
					grdDetailBankSummLedger.RootTable.Columns["SubsidiaryTitle"].Visible = false;
					BtnBankSummaryPrint02.Visible = true;
					BtnPrintBanksummLegder.Visible = false;
				}
				else
				{
					grdDetailBankSummLedger.RootTable.Columns["AccountTitle"].Visible = true;
					grdDetailBankSummLedger.RootTable.Columns["SubsidiaryTitle"].Visible = false;
				}
				grdDetailBankSummLedger.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			}
			grdDetailBankSummLedger.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailBankSummLedger.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdDetailBankSummLedger.RootTable.Columns["DebitAmount"].FormatString = "#,###";
			grdDetailBankSummLedger.RootTable.Columns["DebitAmount"].TotalFormatString = "#,###";
			grdDetailBankSummLedger.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailBankSummLedger.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdDetailBankSummLedger.RootTable.Columns["CreditAmount"].FormatString = "#,#;(#,#);0";
			grdDetailBankSummLedger.RootTable.Columns["CreditAmount"].TotalFormatString = "#,#;(#,##);0";
			grdDetailBankSummLedger.RootTable.Columns["BankOpening"].TextAlignment = (TextAlignment)3;
			grdDetailBankSummLedger.RootTable.Columns["BankOpening"].FormatString = "#,#;(#,##);0";
			grdDetailBankSummLedger.RootTable.Columns["BankOpening"].TotalFormatString = "#,#;(#,##);0";
			grdDetailBankSummLedger.RootTable.Columns["BankClosing"].TextAlignment = (TextAlignment)3;
			grdDetailBankSummLedger.RootTable.Columns["BankClosing"].FormatString = "#,#;(#,##);0";
			grdDetailBankSummLedger.RootTable.Columns["BankClosing"].TotalFormatString = "#,#;(#,##);0";
			grdDetailBankSummLedger.RootTable.Columns["BankAccount"].Width = 225;
			grdDetailBankSummLedger.RootTable.Columns["AccountTitle"].Width = 225;
			grdDetailBankSummLedger.RootTable.Columns["SubsidiaryTitle"].Width = 225;
			grdDetailBankSummLedger.RootTable.Columns["DebitAmount"].Width = 150;
			grdDetailBankSummLedger.RootTable.Columns["CreditAmount"].Width = 150;
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdSummaryValuesBankSummLedgerSettings()
	{
		try
		{
			grdSummaryValuesBanksummLegder.RootTable.Columns["BankAccountId"].Visible = false;
			grdSummaryValuesBanksummLegder.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdSummaryValuesBanksummLegder.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdSummaryValuesBanksummLegder.RootTable.Columns["Debit"].FormatString = "#,###";
			grdSummaryValuesBanksummLegder.RootTable.Columns["Debit"].TotalFormatString = "#,###";
			grdSummaryValuesBanksummLegder.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdSummaryValuesBanksummLegder.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdSummaryValuesBanksummLegder.RootTable.Columns["Credit"].FormatString = "#,#;(#,#);0";
			grdSummaryValuesBanksummLegder.RootTable.Columns["Credit"].TotalFormatString = "#,#;(#,#.##);0";
			grdSummaryValuesBanksummLegder.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grdSummaryValuesBanksummLegder.RootTable.Columns["Opening"].FormatString = "#,#;(#,#.##);0";
			grdSummaryValuesBanksummLegder.RootTable.Columns["Opening"].TotalFormatString = "#,#";
			grdSummaryValuesBanksummLegder.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grdSummaryValuesBanksummLegder.RootTable.Columns["Closing"].FormatString = "#,#;(#,#.##);0";
			grdSummaryValuesBanksummLegder.RootTable.Columns["Closing"].TotalFormatString = "#,#;(#,#.##);0";
			grdSummaryValuesBanksummLegder.RootTable.Columns["BankAccount"].Width = 180;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdDetailBankSummLedger_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetailBankSummLedger.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				Conversion.ToInt(r.Cells["BankAccountId"].Value);
				int AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
				Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
				if ((!(e.Column.Key == "SubsidiaryTitle") || !SubsidiaryAccountAllownOnVouchers) && e.Column.Key == "AccountTitle" && !SubsidiaryAccountAllownOnVouchers)
				{
					tabControl2.SelectedIndex = 0;
					cmbAccountTitle.Value = AccountId;
					cmbAccountTitle_Leave(null, null);
					btnshow_Click(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetailBankSummLedger).Name);
			if (fs != null)
			{
				grdDetailBankSummLedger.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnNewBanksummLegder_Click(object sender, EventArgs e)
	{
		try
		{
			grdAccountFillBankSummary();
			CmbDateTypeBankSummLedger.Focus();
			grdSummaryValuesBanksummLegder.ClearStructure();
			grdDetailBankSummLedger.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintBanksummLegder_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtBankSummLedger == null || dtBankSummLedger.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtBankSummLedger, "105_01_BankSummaryLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnBankSummaryPrint02_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtBankSummLedger == null || dtBankSummLedger.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtBankSummLedger, "105_02_BankSummaryLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKey1_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys1();
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

	private void MakeShortCutKeys1()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For Reset");
			dt.Rows.Add("alt+1", "For Print 1");
			dt.Rows.Add("alt+2", "For Print 2");
			dt.Rows.Add("alt+3", "For Print 3");
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+Shift+S", "For Save Bookmark");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grids");
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For Reset");
			dt.Rows.Add("alt+1", "For Print 1");
			dt.Rows.Add("alt+2", "For Print 2");
			dt.Rows.Add("alt+3", "For Print 3");
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+F5", "For Focus on To Date");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grids");
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(item.Cells["VoucherHeadId"].Value);
				int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				int DocumentTypeIdSrNo = Conversion.ToInt(item.Cells["DocumentTypeSrNo"].Value);
				if (grd.CurrentColumn.Key == "VoucherNo" || grd.CurrentColumn.Key == "VoucherNo")
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo);
				}
				else if (grd.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(DocumentTypeIdSrNo, DocumentTypeId);
				}
				else if (grd.CurrentColumn.Key == "DocType" || grd.CurrentColumn.Key == "DocType")
				{
					CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMultiAccountsLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdMultiAccountsLedger.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Space && grdMultiAccountsLedger.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(item.Cells["VoucherHeadId"].Value);
				int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				int DocumentTypeIdSrNo = Conversion.ToInt(item.Cells["DocumentTypeSrNo"].Value);
				if (grdMultiAccountsLedger.CurrentColumn.Key == "VoucherNo" || grdMultiAccountsLedger.CurrentColumn.Key == "VoucherCode")
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo);
				}
				else if (grdMultiAccountsLedger.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(DocumentTypeIdSrNo, DocumentTypeId);
				}
				else if (grdMultiAccountsLedger.CurrentColumn.Key == "DocType" || grdMultiAccountsLedger.CurrentColumn.Key == "VoucherType")
				{
					CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedTab == tabSingleAccountLedger)
			{
				CmbDateType.Focus();
			}
			if (tabControl2.SelectedTab == tabMultiAccountsLedger)
			{
				datToDate.Focus();
			}
			else if (tabControl2.SelectedTab == tabBankSummaryLedger)
			{
				CmbDateTypeBankSummLedger.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ValuePostDatedNoOfCheqs_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)ValuePostDatedNoOfCheqs.Text) > 0)
			{
				PostDatedCheqInformation postDatedCheqInformation = new PostDatedCheqInformation(UserAccount);
				postDatedCheqInformation.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
				postDatedCheqInformation.ToDate = txtToDate.Value;
				postDatedCheqInformation.StartPosition = FormStartPosition.CenterScreen;
				postDatedCheqInformation.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdAccounts).Name);
			if (fs != null)
			{
				grdAccounts.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnShowFilteredRecords_Click(object sender, EventArgs e)
	{
		try
		{
			if (dtForDocumentTypeList.Rows.Count > 0)
			{
				dtDetailByDocTypes.Clear();
				dtSummary1ByDocTypes.Clear();
				dtSummary2ByDocTypes.Clear();
				string Ids = "";
				GridEXRow[] checkedRows = GrdDocType.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					Ids = Ids + "," + r.Cells["DocumentTypeId"].Value.ToString();
				}
				if (string.IsNullOrEmpty(Ids))
				{
					if (MessageBox.Show("No Document Type Selected. Are You Sure to Show?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						grd.ClearStructure();
					}
					return;
				}
				((Control)(object)grd).Name = "Detailgrd";
				print105.Visible = false;
				btnPrint106.Visible = false;
				btnQuantativeLedger.Visible = false;
				btnSummeryLedger.Visible = false;
				btnSummary2.Visible = false;
				BtnPrintSelectedDocDetail.Visible = false;
				BtnPrintSelectedDocSummary2.Visible = false;
				BtnPrintSelectedDocSumm1.Visible = false;
				btnSaveCheckedRows.Visible = true;
				rBForCheckAllPreviousRows.Visible = true;
				if (ChkDetail.Checked)
				{
					BtnPrintSelectedDocDetail.Visible = true;
					List<int> idList3 = (from id in Ids.Split(',')
						select Conversion.ToInt((object)id)).ToList();
					dtDetailByDocTypes = (from row in dtDetail.AsEnumerable()
						where idList3.Contains(row.Field<int>("DocumentTypeId"))
						select row).CopyToDataTable();
					if (dtDetailByDocTypes.Rows.Count <= 0)
					{
						return;
					}
					DataTable dt = new DataTable();
					dt.Columns.Add("DetailId", typeof(int));
					dt.Columns.Add("Checked", typeof(bool));
					dt.Columns.Add("BookmarkRemarks", typeof(string));
					dt.Columns.Add("VoucherDate", typeof(DateTime));
					dt.Columns.Add("DocType");
					dt.Columns.Add("VoucherNo");
					dt.Columns.Add("AccountCode");
					dt.Columns.Add("OffSetTitle");
					dt.Columns.Add("SubsidiaryAccount");
					dt.Columns.Add("Debit", typeof(double));
					dt.Columns.Add("Credit", typeof(double));
					dt.Columns.Add("Balance", typeof(double));
					dt.Columns.Add("BalType");
					dt.Columns.Add("BillAmount", typeof(double));
					dt.Columns.Add("DueDays", typeof(int));
					dt.Columns.Add("DueDate", typeof(DateTime));
					dt.Columns.Add("Comments");
					dt.Columns.Add("ChequeNo");
					dt.Columns.Add("ChequeDate", typeof(DateTime));
					dt.Columns.Add("PayeeTitle");
					dt.Columns.Add("RefParty");
					dt.Columns.Add("JobLot");
					dt.Columns.Add("MannualNo");
					dt.Columns.Add("RefNo");
					dt.Columns.Add("StockStatus");
					dt.Columns.Add("QtyIn", typeof(double));
					dt.Columns.Add("QtyOut", typeof(double));
					dt.Columns.Add("ItemRate", typeof(double));
					dt.Columns.Add("VehicleNo", typeof(string));
					dt.Columns.Add("NoOfAttachments", typeof(double));
					dt.Columns.Add("VoucherHeadId");
					dt.Columns.Add("DocumentTypeId");
					dt.Columns.Add("BaseDocumentTypeId");
					dt.Columns.Add("DocumentTypeSrNo");
					dt.Columns.Add("IsApproved");
					dt.Columns.Add("BranchName");
					dt.Columns.Add("AccountId");
					for (int i = 0; i < dtDetailByDocTypes.Rows.Count; i++)
					{
						string type = "Nill";
						if (Conversion.ToDouble(dtDetailByDocTypes.Rows[i]["Balance"]) > 0.0)
						{
							type = "Dr";
						}
						else if (Conversion.ToDouble(dtDetailByDocTypes.Rows[i]["Balance"]) < 0.0)
						{
							type = "Cr";
						}
						dt.Rows.Add(dtDetailByDocTypes.Rows[i]["DetailId"], Conversion.ToBool(dtDetailByDocTypes.Rows[i]["BookmarkStatus"]), dtDetailByDocTypes.Rows[i]["BookmarkRemarks"], Conversion.ToDateTime(dtDetailByDocTypes.Rows[i]["VoucherDate"]).ToShortDateString(), dtDetailByDocTypes.Rows[i]["DocTypeCode"], dtDetailByDocTypes.Rows[i]["VoucherCode"], dtDetailByDocTypes.Rows[i]["AccountCode"], dtDetailByDocTypes.Rows[i]["OffsetAccountTitle"], dtDetailByDocTypes.Rows[i]["SubsidiaryAccount"], dtDetailByDocTypes.Rows[i]["DebitAmount"], dtDetailByDocTypes.Rows[i]["CreditAmount"], dtDetailByDocTypes.Rows[i]["Balance"], type, dtDetailByDocTypes.Rows[i]["BillAmount"], dtDetailByDocTypes.Rows[i]["DueDays"], dtDetailByDocTypes.Rows[i]["DueDate"], dtDetailByDocTypes.Rows[i]["Comments"], dtDetailByDocTypes.Rows[i]["CheqNoDetail"], Conversion.ToDateTime(dtDetailByDocTypes.Rows[i]["CheqDate"]).ToShortDateString(), dtDetailByDocTypes.Rows[i]["PayeeTitle"], dtDetailByDocTypes.Rows[i]["RefParty"], dtDetailByDocTypes.Rows[i]["JobLot"], dtDetailByDocTypes.Rows[i]["MannualNo"], dtDetailByDocTypes.Rows[i]["RefInvoiceNo"], dtDetailByDocTypes.Rows[i]["StockReservedStatus"], dtDetailByDocTypes.Rows[i]["QtyIn"], dtDetailByDocTypes.Rows[i]["QtyOut"], dtDetailByDocTypes.Rows[i]["ItemRate"], dtDetailByDocTypes.Rows[i]["VehcileNo"], dtDetailByDocTypes.Rows[i]["NoOfAttachments"], dtDetailByDocTypes.Rows[i]["VoucherHeadId"], dtDetailByDocTypes.Rows[i]["DocumentTypeId"], dtDetailByDocTypes.Rows[i]["BaseDocumentTypeId"], dtDetailByDocTypes.Rows[i]["DocumentTypeSrNo"], dtDetailByDocTypes.Rows[i]["IsApproved"], dtDetailByDocTypes.Rows[i]["BranchName"], dtDetailByDocTypes.Rows[i]["AccountId"]);
					}
					if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
					{
						double balance = Conversion.ToDouble(dtDetailByDocTypes.Rows[dtDetailByDocTypes.Rows.Count - 1]["Balance"]);
						string lbl = "";
						if (balance > 0.0)
						{
							lbl = "Dr";
						}
						else if (balance < 0.0)
						{
							lbl = "Cr";
						}
						lblSubsidiaryBalance.Text = balance.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl;
					}
					if (Conversion.ToDouble(dtDetailByDocTypes.Rows[0]["PostDatedCheqAmount"]) != 0.0)
					{
						lblPostedNoOfCheqs.Visible = true;
						ValuePostDatedNoOfCheqs.Visible = true;
						lblPostDatedCheqAmount.Visible = true;
						ValuePostDatedCheqAmount.Visible = true;
						ValuePostDatedNoOfCheqs.Text = Conversion.ToDouble(dtDetailByDocTypes.Rows[0]["PostDatedNoOfCheqs"]).ToString();
						ValuePostDatedCheqAmount.Text = Conversion.ToDouble(dtDetailByDocTypes.Rows[0]["PostDatedCheqAmount"]).ToString(clsGlobalVariables.stringFormatboth);
						PanelAccountDetail.Location = new Point(4, 5);
					}
					else
					{
						lblPostedNoOfCheqs.Visible = false;
						ValuePostDatedNoOfCheqs.Visible = false;
						lblPostDatedCheqAmount.Visible = false;
						ValuePostDatedCheqAmount.Visible = false;
						PanelAccountDetail.Location = new Point(4, 22);
					}
					grd.DataSource = dt;
					grd.RetrieveStructure();
					GridSettings();
				}
				else if (ChkSummery1.Checked)
				{
					BtnPrintSelectedDocSumm1.Visible = true;
					List<int> idList2 = (from id in Ids.Split(',')
						select Conversion.ToInt((object)id)).ToList();
					dtSummary1ByDocTypes = (from row in dtSummary1.AsEnumerable()
						where idList2.Contains(row.Field<int>("DocumentTypeId"))
						select row).CopyToDataTable();
					if (dtSummary1ByDocTypes.Rows.Count <= 0)
					{
						return;
					}
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("BranchName", typeof(string));
					dt2.Columns.Add("VoucherDate", typeof(DateTime));
					dt2.Columns.Add("DocType", typeof(string));
					dt2.Columns.Add("VoucherCode", typeof(int));
					dt2.Columns.Add("ManualBillNo", typeof(string));
					dt2.Columns.Add("AccountId", typeof(int));
					dt2.Columns.Add("AccountTitle", typeof(string));
					dt2.Columns.Add("Remarks", typeof(string));
					dt2.Columns.Add("DebitAmount", typeof(double));
					dt2.Columns.Add("CreditAmount", typeof(double));
					dt2.Columns.Add("RunningBalance", typeof(double));
					dt2.Columns.Add("DueDate", typeof(DateTime));
					dt2.Columns.Add("DueDays", typeof(int));
					dt2.Columns.Add("DecimlePointsRate", typeof(int));
					dt2.Columns.Add("DecimlePointsAmount", typeof(int));
					dt2.Columns.Add("IsApproved", typeof(bool));
					dt2.Columns.Add("CurrentDR/CrDiff", typeof(double));
					dt2.Columns.Add("VehicleNo", typeof(string));
					dt2.Columns.Add("VoucherHeadId");
					dt2.Columns.Add("DocumentTypeId");
					dt2.Columns.Add("BaseDocumentTypeId");
					dt2.Columns.Add("DocumentTypeSrNo");
					for (int j = 0; j < dtSummary1ByDocTypes.Rows.Count; j++)
					{
						dt2.Rows.Add(dtSummary1ByDocTypes.Rows[j]["BranchName"], dtSummary1ByDocTypes.Rows[j]["VoucherDate"], dtSummary1ByDocTypes.Rows[j]["DocumentTypeDescription"], dtSummary1ByDocTypes.Rows[j]["VoucherCode"], dtSummary1ByDocTypes.Rows[j]["ManualBillNo"], dtSummary1ByDocTypes.Rows[j]["AccountId"], dtSummary1ByDocTypes.Rows[j]["AccountTitle"], dtSummary1ByDocTypes.Rows[j]["Remarks"], dtSummary1ByDocTypes.Rows[j]["DebitAmount"], dtSummary1ByDocTypes.Rows[j]["CreditAmount"], dtSummary1ByDocTypes.Rows[j]["RunningBalance"], dtSummary1ByDocTypes.Rows[j]["DueDate"], dtSummary1ByDocTypes.Rows[j]["DueDays"], dtSummary1ByDocTypes.Rows[j]["DecimlePointsRate"], dtSummary1ByDocTypes.Rows[j]["DecimlePointsAmount"], dtSummary1ByDocTypes.Rows[j]["IsApproved"], Conversion.ToDouble(dtSummary1ByDocTypes.Rows[j]["DebitAmount"]) - Conversion.ToDouble(dtSummary1ByDocTypes.Rows[j]["CreditAmount"]), dtSummary1ByDocTypes.Rows[j]["VehicleNo"], dtSummary1ByDocTypes.Rows[j]["Id"], dtSummary1ByDocTypes.Rows[j]["DocumentTypeId"], dtSummary1ByDocTypes.Rows[j]["BaseDocumentTypeId"], dtSummary1ByDocTypes.Rows[j]["DocumentTypeSrNo"]);
					}
					if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
					{
						double balance2 = Conversion.ToDouble(dtSummary1ByDocTypes.Rows[dtSummary1ByDocTypes.Rows.Count - 1]["Balance"]);
						string lbl2 = "";
						if (balance2 > 0.0)
						{
							lbl2 = "Dr";
						}
						else if (balance2 < 0.0)
						{
							lbl2 = "Cr";
						}
						lblSubsidiaryBalance.Text = balance2.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl2;
					}
					if (Conversion.ToDouble(dtSummary1ByDocTypes.Rows[0]["PostDatedCheqAmount"]) != 0.0)
					{
						lblPostedNoOfCheqs.Visible = true;
						ValuePostDatedNoOfCheqs.Visible = true;
						lblPostDatedCheqAmount.Visible = true;
						ValuePostDatedCheqAmount.Visible = true;
						ValuePostDatedNoOfCheqs.Text = Conversion.ToDouble(dtSummary1ByDocTypes.Rows[0]["PostDatedNoOfCheqs"]).ToString();
						ValuePostDatedCheqAmount.Text = Conversion.ToDouble(dtSummary1ByDocTypes.Rows[0]["PostDatedCheqAmount"]).ToString(clsGlobalVariables.stringFormatboth);
						PanelAccountDetail.Location = new Point(4, 5);
					}
					else
					{
						lblPostedNoOfCheqs.Visible = false;
						ValuePostDatedNoOfCheqs.Visible = false;
						lblPostDatedCheqAmount.Visible = false;
						ValuePostDatedCheqAmount.Visible = false;
						PanelAccountDetail.Location = new Point(4, 22);
					}
					grd.DataSource = dt2;
					grd.RetrieveStructure();
					GridSettingsForSummery();
				}
				else
				{
					if (!ChkSummery2.Checked)
					{
						return;
					}
					BtnPrintSelectedDocSummary2.Visible = true;
					List<int> idList = (from id in Ids.Split(',')
						select Conversion.ToInt((object)id)).ToList();
					dtSummary2ByDocTypes = (from row in dtSummary2.AsEnumerable()
						where idList.Contains(row.Field<int>("DocumentTypeId"))
						select row).CopyToDataTable();
					if (dtSummary2ByDocTypes.Rows.Count <= 0)
					{
						return;
					}
					DataTable dt3 = new DataTable();
					dt3.Columns.Add("BranchName", typeof(string));
					dt3.Columns.Add("VoucherDate", typeof(DateTime));
					dt3.Columns.Add("VoucherType");
					dt3.Columns.Add("VoucherNo");
					dt3.Columns.Add("AccountTitle");
					dt3.Columns.Add("OffSetAccountTitle");
					dt3.Columns.Add("Remarks");
					dt3.Columns.Add("DebitAmount", typeof(double));
					dt3.Columns.Add("CreditAmount", typeof(double));
					dt3.Columns.Add("Balance", typeof(double));
					dt3.Columns.Add("RunningBalance", typeof(double));
					dt3.Columns.Add("ChequeDate", typeof(DateTime));
					dt3.Columns.Add("ChequeNo");
					dt3.Columns.Add("PayeeTitle");
					dt3.Columns.Add("DueDate", typeof(DateTime));
					dt3.Columns.Add("DueDays", typeof(int));
					dt3.Columns.Add("VoucherHeadId");
					dt3.Columns.Add("DocumentTypeId");
					dt3.Columns.Add("BaseDocumentTypeId");
					dt3.Columns.Add("DocumentTypeSrNo");
					dt3.Columns.Add("NoOfAttachments");
					dt3.Columns.Add("AccountId", typeof(int));
					double RunningOpening = 0.0;
					for (int k = 0; k < dtSummary2ByDocTypes.Rows.Count; k++)
					{
						if (k == 0)
						{
							RunningOpening = Conversion.ToDouble(dtSummary2ByDocTypes.Rows[k]["RunningBalance"]);
						}
						dt3.Rows.Add(dtSummary2ByDocTypes.Rows[k]["BranchName"], Conversion.ToDateTime(dtSummary2ByDocTypes.Rows[k]["VoucherDate"]), dtSummary2ByDocTypes.Rows[k]["VoucherType"], dtSummary2ByDocTypes.Rows[k]["VoucherNo"], dtSummary2ByDocTypes.Rows[k]["AccountTitle"], dtSummary2ByDocTypes.Rows[k]["OffSetAccountTitle"], dtSummary2ByDocTypes.Rows[k]["Remarks"], dtSummary2ByDocTypes.Rows[k]["DebitAmount"], dtSummary2ByDocTypes.Rows[k]["CreditAmount"], dtSummary2ByDocTypes.Rows[k]["RunningBalance"], RunningOpening + Conversion.ToDouble(dtSummary2ByDocTypes.Rows[k]["DebitAmount"]) - Conversion.ToDouble(dtSummary2ByDocTypes.Rows[k]["CreditAmount"]), Conversion.ToDateTime(dtSummary2ByDocTypes.Rows[k]["ChequeDate"]).ToString("dd-MMM-yyyy"), dtSummary2ByDocTypes.Rows[k]["ChequeNo"], dtSummary2ByDocTypes.Rows[k]["PayeeTitle"], dtSummary2ByDocTypes.Rows[k]["DueDate"], dtSummary2ByDocTypes.Rows[k]["DueDays"], dtSummary2ByDocTypes.Rows[k]["VoucherHeadId"], dtSummary2ByDocTypes.Rows[k]["DocumentTypeId"], dtSummary2ByDocTypes.Rows[k]["BaseDocumentTypeId"], dtSummary2ByDocTypes.Rows[k]["DocumentTypeSrNo"], dtSummary2ByDocTypes.Rows[k]["NoOfAttachments"], dtSummary2ByDocTypes.Rows[k]["AccountId"]);
						if (k != 0)
						{
							RunningOpening = RunningOpening + Conversion.ToDouble(dtSummary2ByDocTypes.Rows[k]["DebitAmount"]) - Conversion.ToDouble(dtSummary2ByDocTypes.Rows[k]["CreditAmount"]);
						}
					}
					if (Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
					{
						double balance3 = Conversion.ToDouble(dtSummary2ByDocTypes.Rows[dtSummary2ByDocTypes.Rows.Count - 1]["RunningBalance"]);
						string lbl3 = "";
						if (balance3 > 0.0)
						{
							lbl3 = "Dr";
						}
						else if (balance3 < 0.0)
						{
							lbl3 = "Cr";
						}
						lblSubsidiaryBalance.Text = balance3.ToString(clsGlobalVariables.stringFormatboth) + " " + lbl3;
					}
					if (Conversion.ToDouble(dtSummary2ByDocTypes.Rows[0]["PostDatedCheqAmount"]) != 0.0)
					{
						lblPostedNoOfCheqs.Visible = true;
						ValuePostDatedNoOfCheqs.Visible = true;
						lblPostDatedCheqAmount.Visible = true;
						ValuePostDatedCheqAmount.Visible = true;
						ValuePostDatedNoOfCheqs.Text = Conversion.ToDouble(dtSummary2ByDocTypes.Rows[0]["PostDatedNoOfCheqs"]).ToString();
						ValuePostDatedCheqAmount.Text = Conversion.ToDouble(dtSummary2ByDocTypes.Rows[0]["PostDatedCheqAmount"]).ToString(clsGlobalVariables.stringFormatboth);
						PanelAccountDetail.Location = new Point(4, 5);
					}
					else
					{
						lblPostedNoOfCheqs.Visible = false;
						ValuePostDatedNoOfCheqs.Visible = false;
						lblPostDatedCheqAmount.Visible = false;
						ValuePostDatedCheqAmount.Visible = false;
						PanelAccountDetail.Location = new Point(4, 22);
					}
					grd.DataSource = dt3;
					grd.RetrieveStructure();
					GridSettingsForSummeryFormatII();
				}
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintSelectedDocDetail_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtDetailByDocTypes.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtDetailByDocTypes, "107A_GeneralLedger.rpt");
			}
			else
			{
				MessageBox.Show("No Records Found in Document Type Selection grid");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintSelectedDocSumm1_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSummary1ByDocTypes.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummary1ByDocTypes, "108A_GeneralLedgerSummery.rpt");
			}
			else
			{
				MessageBox.Show("No Records Found in Document Type Selection grid");
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void BtnPrintSelectedDocSummary2_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSummary2ByDocTypes.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummary2ByDocTypes, "106A_GeneralLedgerSummary2.rpt");
			}
			else
			{
				MessageBox.Show("No Records Found in Document Type Selection grid");
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void lblPdcReceipts_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbAccountTitle.Value) > 0 && Conversion.ToDouble((object)txtPdcReceipts.Text) != 0.0)
			{
				CommonServices.OpenPdcTransactionPayment(txtFromDate.Value, txtToDate.Value, Conversion.ToInt(cmbAccountTitle.Value), CreditAccount: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblPdcPayments_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbAccountTitle.Value) > 0 && Conversion.ToDouble((object)txtPdcPayments.Text) != 0.0)
			{
				CommonServices.OpenPdcTransactionPayment(txtFromDate.Value, txtToDate.Value, Conversion.ToInt(cmbAccountTitle.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPdcReceipts_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbAccountTitle.Value) > 0 && Conversion.ToDouble((object)txtPdcReceipts.Text) != 0.0)
			{
				CommonServices.OpenPdcTransactionPayment(txtFromDate.Value, txtToDate.Value, Conversion.ToInt(cmbAccountTitle.Value), CreditAccount: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPdcPayments_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbAccountTitle.Value) > 0 && Conversion.ToDouble((object)txtPdcPayments.Text) != 0.0)
			{
				CommonServices.OpenPdcTransactionPayment(txtFromDate.Value, txtToDate.Value, Conversion.ToInt(cmbAccountTitle.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdAcByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable dtAc = (DataTable)((UltraGridBase)cmbAccountTitle).DataSource;
			if (dtAc.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(cmbAccountTitle.Value) > 0)
				{
					Id = Conversion.ToInt(cmbAccountTitle.Value);
				}
				if (RdAcByName.Checked)
				{
					DDL.BindDDL(dtAc, cmbAccountTitle, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				}
				else
				{
					DDL.BindDDL(dtAc, cmbAccountTitle, "Id", "AccountCode", "Account Code", ZeroIndex: false);
				}
				if (Id > 0)
				{
					cmbAccountTitle.Value = Id;
				}
				cmbAccountTitle.Focus();
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
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Expected O, but got Unknown
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Expected O, but got Unknown
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Expected O, but got Unknown
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Expected O, but got Unknown
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Expected O, but got Unknown
		//IL_0867: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Expected O, but got Unknown
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Expected O, but got Unknown
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Expected O, but got Unknown
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Expected O, but got Unknown
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a76: Expected O, but got Unknown
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a97: Expected O, but got Unknown
		//IL_0b27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b31: Expected O, but got Unknown
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Expected O, but got Unknown
		//IL_0b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Expected O, but got Unknown
		//IL_0bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c02: Expected O, but got Unknown
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c18: Expected O, but got Unknown
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Expected O, but got Unknown
		//IL_0c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9c: Expected O, but got Unknown
		//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd3: Expected O, but got Unknown
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6d: Expected O, but got Unknown
		//IL_0d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d83: Expected O, but got Unknown
		//IL_15d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e1: Expected O, but got Unknown
		//IL_67e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_67ea: Expected O, but got Unknown
		//IL_8d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d11: Expected O, but got Unknown
		//IL_8ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_9006: Expected O, but got Unknown
		//IL_a81a: Unknown result type (might be due to invalid IL or missing references)
		//IL_a824: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDocType_DesignTimeLayout = new GridEXLayout();
		Appearance appearance128 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.GeneralLedger));
		GridEXLayout grdMultiAccountsLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdAccounts_DesignTimeLayout = new GridEXLayout();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance49 = new Appearance();
		GridEXLayout grdDetailBankSummLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdAccountsBankSummLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummaryValuesBanksummLegder_DesignTimeLayout = new GridEXLayout();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		GridEXLayout grdItemLedger_DesignTimeLayout = new GridEXLayout();
		Appearance appearance156 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		GridEXLayout grdBranchesWithBalance_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPageGeneralLedger = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabSingleAccountLedger = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel33 = new System.Windows.Forms.Panel();
		this.GrdDocType = new GridEX();
		this.BtnShowFilteredRecords = new System.Windows.Forms.Button();
		this.gbAccountOtherInfo = new System.Windows.Forms.GroupBox();
		this.txtPendingPrePayments = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtBalAfterCheqClearance = new System.Windows.Forms.Label();
		this.txtPdcPayments = new System.Windows.Forms.Label();
		this.txtPdcReceipts = new System.Windows.Forms.Label();
		this.txtBalAfterPreReceiptsClearance = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.lblPdcPayments = new System.Windows.Forms.Label();
		this.lblPdcReceipts = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.txtPendingPrematureReceipts = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.gbAccountDetail = new System.Windows.Forms.GroupBox();
		this.lblBranchBalance = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.PanelAccountDetail = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.lblAccountClass = new System.Windows.Forms.Label();
		this.lblAccountCode = new System.Windows.Forms.Label();
		this.lblAccountType = new System.Windows.Forms.Label();
		this.lblOpening = new System.Windows.Forms.Label();
		this.lblCurrDebit = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.lblCurrCredit = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.lblClosingBal = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.ValuePostDatedCheqAmount = new System.Windows.Forms.Label();
		this.SubsidiaryBalanceBox = new System.Windows.Forms.GroupBox();
		this.lblSubsidiaryBalance = new System.Windows.Forms.Label();
		this.SubsidiaryAccountName = new System.Windows.Forms.Label();
		this.ValuePostDatedNoOfCheqs = new System.Windows.Forms.Label();
		this.lblPostedNoOfCheqs = new System.Windows.Forms.Label();
		this.lblPostDatedCheqAmount = new System.Windows.Forms.Label();
		this.cmbproject = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RdAcByCode = new System.Windows.Forms.RadioButton();
		this.RdAcByName = new System.Windows.Forms.RadioButton();
		this.lblBranch = new System.Windows.Forms.Label();
		this.rdPanel = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.ChkDetail = new System.Windows.Forms.RadioButton();
		this.ChkSummery1 = new System.Windows.Forms.RadioButton();
		this.ChkSummery2 = new System.Windows.Forms.RadioButton();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbDateType = new UltraCombo();
		this.cmbbranch = new UltraCombo();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.CmbSubsidiaryAccount = new UltraCombo();
		this.lblSubsidiaryAccount = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbAccountTitle = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.lblCostCenter = new System.Windows.Forms.Label();
		this.CmbLanguage = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ChkBoxPost = new System.Windows.Forms.CheckBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print105 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint106 = new System.Windows.Forms.ToolStripButton();
		this.btnQuantativeLedger = new System.Windows.Forms.ToolStripButton();
		this.btnSummeryLedger = new System.Windows.Forms.ToolStripButton();
		this.btnSummary2 = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintSelectedDocSummary2 = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintSelectedDocDetail = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintSelectedDocSumm1 = new System.Windows.Forms.ToolStripButton();
		this.btnSaveCheckedRows = new System.Windows.Forms.ToolStripButton();
		this.rBForCheckAllPreviousRows = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKey1 = new System.Windows.Forms.ToolStripButton();
		this.tabMultiAccountsLedger = new System.Windows.Forms.TabPage();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdMultiAccountsLedger = new GridEX();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label34 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdAccounts = new GridEX();
		this.label5 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbDateTypeMulti = new UltraCombo();
		this.btnShowMultiLedger = new System.Windows.Forms.Button();
		this.rdSummary2Print = new System.Windows.Forms.RadioButton();
		this.label31 = new System.Windows.Forms.Label();
		this.rdSummary1Print = new System.Windows.Forms.RadioButton();
		this.ToDateForMulti = new System.Windows.Forms.DateTimePicker();
		this.rdDetailPrint = new System.Windows.Forms.RadioButton();
		this.label10 = new System.Windows.Forms.Label();
		this.FromDateForMulti = new System.Windows.Forms.DateTimePicker();
		this.chkIncludeUnpostedVouchers = new System.Windows.Forms.CheckBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label35 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.btnGetAccounts = new System.Windows.Forms.Button();
		this.cmbCustomGroup = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbGroupAccount = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnReset = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh1 = new System.Windows.Forms.ToolStripButton();
		this.btn105MultiPrint = new System.Windows.Forms.ToolStripButton();
		this.btn105AMultiPrint = new System.Windows.Forms.ToolStripButton();
		this.btn106MultiPrint = new System.Windows.Forms.ToolStripButton();
		this.btn107MultiPrint = new System.Windows.Forms.ToolStripButton();
		this.btn108MultiPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabBankSummaryLedger = new System.Windows.Forms.TabPage();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdDetailBankSummLedger = new GridEX();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label40 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdAccountsBankSummLedger = new GridEX();
		this.label42 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel32 = new System.Windows.Forms.Panel();
		this.grdSummaryValuesBanksummLegder = new GridEX();
		this.label44 = new System.Windows.Forms.Label();
		this.panel31 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.toolStrip6 = new System.Windows.Forms.ToolStrip();
		this.BtnNewBanksummLegder = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintBanksummLegder = new System.Windows.Forms.ToolStripButton();
		this.BtnBankSummaryPrint02 = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutKeysBanksummLegder = new System.Windows.Forms.ToolStripButton();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label45 = new System.Windows.Forms.Label();
		this.CmbAccountTypesForBankSummaryLedger = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbDateTypeBankSummLedger = new UltraCombo();
		this.txttoDateBankSummLedger = new System.Windows.Forms.DateTimePicker();
		this.label39 = new System.Windows.Forms.Label();
		this.txtFromDateBankSummLedger = new System.Windows.Forms.DateTimePicker();
		this.label41 = new System.Windows.Forms.Label();
		this.BtnShowBankSummLedger = new System.Windows.Forms.Button();
		this.tabPageCustomerLedger = new System.Windows.Forms.TabPage();
		this.tabPageItemLedger = new System.Windows.Forms.TabPage();
		this.panel30 = new System.Windows.Forms.Panel();
		this.panel28 = new System.Windows.Forms.Panel();
		this.grdItemLedger = new GridEX();
		this.panel29 = new System.Windows.Forms.Panel();
		this.CmbItemNameItmLedger = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtToDateItemLedger = new System.Windows.Forms.DateTimePicker();
		this.txtFromDateItemLedger = new System.Windows.Forms.DateTimePicker();
		this.btnShowItemLedger = new System.Windows.Forms.Button();
		this.cmbStockAccountItemLedger = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.panel27 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.panel26 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.btnNewItemLedger = new System.Windows.Forms.ToolStripButton();
		this.btnRegisterItemLedger = new System.Windows.Forms.ToolStripButton();
		this.btnItemLedger_164 = new System.Windows.Forms.ToolStripButton();
		this.tabPageCommissionAgent = new System.Windows.Forms.TabPage();
		this.tabPageFcyGeneralLedger = new System.Windows.Forms.TabPage();
		this.comSupplier = new UltraCombo();
		this.gbBranchesWithBalance = new System.Windows.Forms.GroupBox();
		this.grdBranchesWithBalance = new GridEX();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPageGeneralLedger.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabSingleAccountLedger.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel15.SuspendLayout();
		this.panel33.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDocType).BeginInit();
		this.gbAccountOtherInfo.SuspendLayout();
		this.gbAccountDetail.SuspendLayout();
		this.PanelAccountDetail.SuspendLayout();
		this.SubsidiaryBalanceBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		this.groupBox1.SuspendLayout();
		this.rdPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabMultiAccountsLedger.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiAccountsLedger).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAccounts).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeMulti).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroupAccount).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabBankSummaryLedger.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailBankSummLedger).BeginInit();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAccountsBankSummLedger).BeginInit();
		this.panel17.SuspendLayout();
		this.panel32.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummaryValuesBanksummLegder).BeginInit();
		this.panel31.SuspendLayout();
		this.toolStrip6.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypesForBankSummaryLedger).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeBankSummLedger).BeginInit();
		this.tabPageItemLedger.SuspendLayout();
		this.panel30.SuspendLayout();
		this.panel28.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemLedger).BeginInit();
		this.panel29.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameItmLedger).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStockAccountItemLedger).BeginInit();
		this.panel27.SuspendLayout();
		this.panel26.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comSupplier).BeginInit();
		this.gbBranchesWithBalance.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdBranchesWithBalance).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1656, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPageGeneralLedger);
		this.tabControl1.Controls.Add(this.tabPageCustomerLedger);
		this.tabControl1.Controls.Add(this.tabPageItemLedger);
		this.tabControl1.Controls.Add(this.tabPageCommissionAgent);
		this.tabControl1.Controls.Add(this.tabPageFcyGeneralLedger);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1656, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPageGeneralLedger.Controls.Add(this.panel2);
		this.tabPageGeneralLedger.Location = new System.Drawing.Point(4, 4);
		this.tabPageGeneralLedger.Name = "tabPageGeneralLedger";
		this.tabPageGeneralLedger.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageGeneralLedger.Size = new System.Drawing.Size(1648, 723);
		this.tabPageGeneralLedger.TabIndex = 0;
		this.tabPageGeneralLedger.Text = "General Leder";
		this.tabPageGeneralLedger.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.tabControl2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1642, 717);
		this.panel2.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabSingleAccountLedger);
		this.tabControl2.Controls.Add(this.tabMultiAccountsLedger);
		this.tabControl2.Controls.Add(this.tabBankSummaryLedger);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1642, 717);
		this.tabControl2.TabIndex = 6;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabSingleAccountLedger.Controls.Add(this.panel14);
		this.tabSingleAccountLedger.Location = new System.Drawing.Point(4, 22);
		this.tabSingleAccountLedger.Name = "tabSingleAccountLedger";
		this.tabSingleAccountLedger.Padding = new System.Windows.Forms.Padding(3);
		this.tabSingleAccountLedger.Size = new System.Drawing.Size(1634, 691);
		this.tabSingleAccountLedger.TabIndex = 0;
		this.tabSingleAccountLedger.Text = "Single Account Ledger";
		this.tabSingleAccountLedger.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel14.Controls.Add(this.panel15);
		this.panel14.Controls.Add(this.panel3);
		this.panel14.Controls.Add(this.panel7);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 3);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1628, 685);
		this.panel14.TabIndex = 0;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		this.grd.AutomaticSort = false;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 220);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1628, 465);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel15.Controls.Add(this.gbBranchesWithBalance);
		this.panel15.Controls.Add(this.panel33);
		this.panel15.Controls.Add(this.gbAccountOtherInfo);
		this.panel15.Controls.Add(this.gbAccountDetail);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.panel15.Controls.Add(this.groupBox1);
		this.panel15.Controls.Add(this.label1);
		this.panel15.Controls.Add(this.label30);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 62);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1628, 158);
		this.panel15.TabIndex = 22;
		this.panel33.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDocType);
		this.panel33.Controls.Add(this.BtnShowFilteredRecords);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel33.Location = new System.Drawing.Point(1125, 0);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(254, 158);
		this.panel33.TabIndex = 118;
		this.GrdDocType.AllowEdit = (InheritableBoolean)2;
		this.GrdDocType.AlternatingColors = true;
		this.GrdDocType.AutomaticSort = false;
		GrdDocType_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDocType.DesignTimeLayout = GrdDocType_DesignTimeLayout;
		this.GrdDocType.DynamicFiltering = true;
		this.GrdDocType.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDocType).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDocType.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdDocType).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdDocType).Name = "GrdDocType";
		((System.Windows.Forms.Control)(object)this.GrdDocType).Size = new System.Drawing.Size(200, 156);
		((System.Windows.Forms.Control)(object)this.GrdDocType).TabIndex = 115;
		this.GrdDocType.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDocType).VisualStyle = (VisualStyle)4;
		this.BtnShowFilteredRecords.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowFilteredRecords.FlatAppearance.BorderSize = 0;
		this.BtnShowFilteredRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowFilteredRecords.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowFilteredRecords.ForeColor = System.Drawing.Color.White;
		this.BtnShowFilteredRecords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowFilteredRecords.Location = new System.Drawing.Point(200, 130);
		this.BtnShowFilteredRecords.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowFilteredRecords.Name = "BtnShowFilteredRecords";
		this.BtnShowFilteredRecords.Size = new System.Drawing.Size(54, 26);
		this.BtnShowFilteredRecords.TabIndex = 116;
		this.BtnShowFilteredRecords.Text = "Show";
		this.BtnShowFilteredRecords.UseVisualStyleBackColor = false;
		this.BtnShowFilteredRecords.Click += new System.EventHandler(BtnShowFilteredRecords_Click);
		this.gbAccountOtherInfo.Controls.Add(this.txtPendingPrePayments);
		this.gbAccountOtherInfo.Controls.Add(this.label52);
		this.gbAccountOtherInfo.Controls.Add(this.txtBalAfterCheqClearance);
		this.gbAccountOtherInfo.Controls.Add(this.txtPdcPayments);
		this.gbAccountOtherInfo.Controls.Add(this.txtPdcReceipts);
		this.gbAccountOtherInfo.Controls.Add(this.txtBalAfterPreReceiptsClearance);
		this.gbAccountOtherInfo.Controls.Add(this.label51);
		this.gbAccountOtherInfo.Controls.Add(this.lblPdcPayments);
		this.gbAccountOtherInfo.Controls.Add(this.lblPdcReceipts);
		this.gbAccountOtherInfo.Controls.Add(this.label48);
		this.gbAccountOtherInfo.Controls.Add(this.txtPendingPrematureReceipts);
		this.gbAccountOtherInfo.Controls.Add(this.label47);
		this.gbAccountOtherInfo.Dock = System.Windows.Forms.DockStyle.Left;
		this.gbAccountOtherInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.gbAccountOtherInfo.Location = new System.Drawing.Point(844, 0);
		this.gbAccountOtherInfo.Name = "gbAccountOtherInfo";
		this.gbAccountOtherInfo.Size = new System.Drawing.Size(281, 158);
		this.gbAccountOtherInfo.TabIndex = 117;
		this.gbAccountOtherInfo.TabStop = false;
		this.gbAccountOtherInfo.Text = "Account Other Info";
		this.txtPendingPrePayments.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPendingPrePayments.Location = new System.Drawing.Point(179, 34);
		this.txtPendingPrePayments.Name = "txtPendingPrePayments";
		this.txtPendingPrePayments.Size = new System.Drawing.Size(96, 13);
		this.txtPendingPrePayments.TabIndex = 22;
		this.txtPendingPrePayments.Text = "0,000,000";
		this.txtPendingPrePayments.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(3, 33);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(164, 15);
		this.label52.TabIndex = 23;
		this.label52.Text = "Pending Premature Payments";
		this.txtBalAfterCheqClearance.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalAfterCheqClearance.ForeColor = System.Drawing.Color.Red;
		this.txtBalAfterCheqClearance.Location = new System.Drawing.Point(179, 128);
		this.txtBalAfterCheqClearance.Name = "txtBalAfterCheqClearance";
		this.txtBalAfterCheqClearance.Size = new System.Drawing.Size(96, 13);
		this.txtBalAfterCheqClearance.TabIndex = 21;
		this.txtBalAfterCheqClearance.Text = "0,000,000";
		this.txtBalAfterCheqClearance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtPdcPayments.Cursor = System.Windows.Forms.Cursors.Hand;
		this.txtPdcPayments.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPdcPayments.Location = new System.Drawing.Point(179, 109);
		this.txtPdcPayments.Name = "txtPdcPayments";
		this.txtPdcPayments.Size = new System.Drawing.Size(96, 13);
		this.txtPdcPayments.TabIndex = 20;
		this.txtPdcPayments.Text = "0,000,000";
		this.txtPdcPayments.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtPdcPayments.Click += new System.EventHandler(txtPdcPayments_Click);
		this.txtPdcReceipts.Cursor = System.Windows.Forms.Cursors.Hand;
		this.txtPdcReceipts.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPdcReceipts.Location = new System.Drawing.Point(179, 88);
		this.txtPdcReceipts.Name = "txtPdcReceipts";
		this.txtPdcReceipts.Size = new System.Drawing.Size(96, 13);
		this.txtPdcReceipts.TabIndex = 19;
		this.txtPdcReceipts.Text = "0,000,000";
		this.txtPdcReceipts.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtPdcReceipts.Click += new System.EventHandler(txtPdcReceipts_Click);
		this.txtBalAfterPreReceiptsClearance.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalAfterPreReceiptsClearance.ForeColor = System.Drawing.Color.Red;
		this.txtBalAfterPreReceiptsClearance.Location = new System.Drawing.Point(179, 61);
		this.txtBalAfterPreReceiptsClearance.Name = "txtBalAfterPreReceiptsClearance";
		this.txtBalAfterPreReceiptsClearance.Size = new System.Drawing.Size(96, 13);
		this.txtBalAfterPreReceiptsClearance.TabIndex = 18;
		this.txtBalAfterPreReceiptsClearance.Text = "0,000,000";
		this.txtBalAfterPreReceiptsClearance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(3, 127);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(166, 15);
		this.label51.TabIndex = 17;
		this.label51.Text = "Balance After Cheqs Clearance";
		this.lblPdcPayments.AutoSize = true;
		this.lblPdcPayments.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblPdcPayments.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPdcPayments.Location = new System.Drawing.Point(3, 108);
		this.lblPdcPayments.Name = "lblPdcPayments";
		this.lblPdcPayments.Size = new System.Drawing.Size(155, 15);
		this.lblPdcPayments.TabIndex = 16;
		this.lblPdcPayments.Text = "Post Dated Cheqs Payments";
		this.lblPdcPayments.Click += new System.EventHandler(lblPdcPayments_Click);
		this.lblPdcReceipts.AutoSize = true;
		this.lblPdcReceipts.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblPdcReceipts.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPdcReceipts.Location = new System.Drawing.Point(3, 87);
		this.lblPdcReceipts.Name = "lblPdcReceipts";
		this.lblPdcReceipts.Size = new System.Drawing.Size(147, 15);
		this.lblPdcReceipts.TabIndex = 15;
		this.lblPdcReceipts.Text = "Post Dated Cheqs Receipts";
		this.lblPdcReceipts.Click += new System.EventHandler(lblPdcReceipts_Click);
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(3, 52);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(176, 30);
		this.label48.TabIndex = 14;
		this.label48.Text = "Balance After Premature Receipts Clearance";
		this.txtPendingPrematureReceipts.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPendingPrematureReceipts.Location = new System.Drawing.Point(179, 16);
		this.txtPendingPrematureReceipts.Name = "txtPendingPrematureReceipts";
		this.txtPendingPrematureReceipts.Size = new System.Drawing.Size(96, 13);
		this.txtPendingPrematureReceipts.TabIndex = 12;
		this.txtPendingPrematureReceipts.Text = "0,000,000";
		this.txtPendingPrematureReceipts.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(3, 15);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(156, 15);
		this.label47.TabIndex = 13;
		this.label47.Text = "Pending Premature Receipts";
		this.gbAccountDetail.Controls.Add(this.lblBranchBalance);
		this.gbAccountDetail.Controls.Add(this.label46);
		this.gbAccountDetail.Controls.Add(this.PanelAccountDetail);
		this.gbAccountDetail.Controls.Add(this.ValuePostDatedCheqAmount);
		this.gbAccountDetail.Controls.Add(this.SubsidiaryBalanceBox);
		this.gbAccountDetail.Controls.Add(this.ValuePostDatedNoOfCheqs);
		this.gbAccountDetail.Controls.Add(this.lblPostedNoOfCheqs);
		this.gbAccountDetail.Controls.Add(this.lblPostDatedCheqAmount);
		this.gbAccountDetail.Dock = System.Windows.Forms.DockStyle.Left;
		this.gbAccountDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.gbAccountDetail.Location = new System.Drawing.Point(403, 0);
		this.gbAccountDetail.Name = "gbAccountDetail";
		this.gbAccountDetail.Size = new System.Drawing.Size(441, 158);
		this.gbAccountDetail.TabIndex = 107;
		this.gbAccountDetail.TabStop = false;
		this.gbAccountDetail.Text = "Account Detail";
		this.lblBranchBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranchBalance.Location = new System.Drawing.Point(219, 138);
		this.lblBranchBalance.Name = "lblBranchBalance";
		this.lblBranchBalance.Size = new System.Drawing.Size(211, 16);
		this.lblBranchBalance.TabIndex = 14;
		this.lblBranchBalance.Text = "0";
		this.lblBranchBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblBranchBalance.Visible = false;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(8, 139);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(79, 15);
		this.label46.TabIndex = 14;
		this.label46.Text = "Branch Name";
		this.label46.Visible = false;
		this.PanelAccountDetail.Controls.Add(this.label24);
		this.PanelAccountDetail.Controls.Add(this.lblAccountClass);
		this.PanelAccountDetail.Controls.Add(this.lblAccountCode);
		this.PanelAccountDetail.Controls.Add(this.lblAccountType);
		this.PanelAccountDetail.Controls.Add(this.lblOpening);
		this.PanelAccountDetail.Controls.Add(this.lblCurrDebit);
		this.PanelAccountDetail.Controls.Add(this.label28);
		this.PanelAccountDetail.Controls.Add(this.lblCurrCredit);
		this.PanelAccountDetail.Controls.Add(this.label32);
		this.PanelAccountDetail.Controls.Add(this.lblClosingBal);
		this.PanelAccountDetail.Controls.Add(this.label26);
		this.PanelAccountDetail.Controls.Add(this.label21);
		this.PanelAccountDetail.Controls.Add(this.label22);
		this.PanelAccountDetail.Controls.Add(this.label25);
		this.PanelAccountDetail.Location = new System.Drawing.Point(4, 5);
		this.PanelAccountDetail.Name = "PanelAccountDetail";
		this.PanelAccountDetail.Size = new System.Drawing.Size(431, 70);
		this.PanelAccountDetail.TabIndex = 108;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(232, 37);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(71, 15);
		this.label24.TabIndex = 6;
		this.label24.Text = "Total Credit";
		this.lblAccountClass.AutoSize = true;
		this.lblAccountClass.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccountClass.Location = new System.Drawing.Point(92, 29);
		this.lblAccountClass.Name = "lblAccountClass";
		this.lblAccountClass.Size = new System.Drawing.Size(82, 15);
		this.lblAccountClass.TabIndex = 2;
		this.lblAccountClass.Text = "Account Class";
		this.lblAccountCode.AutoSize = true;
		this.lblAccountCode.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccountCode.Location = new System.Drawing.Point(92, 11);
		this.lblAccountCode.Name = "lblAccountCode";
		this.lblAccountCode.Size = new System.Drawing.Size(83, 15);
		this.lblAccountCode.TabIndex = 2;
		this.lblAccountCode.Text = "Account Code";
		this.lblAccountType.AutoSize = true;
		this.lblAccountType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccountType.Location = new System.Drawing.Point(92, 47);
		this.lblAccountType.Name = "lblAccountType";
		this.lblAccountType.Size = new System.Drawing.Size(79, 15);
		this.lblAccountType.TabIndex = 3;
		this.lblAccountType.Text = "Account Type";
		this.lblOpening.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOpening.Location = new System.Drawing.Point(319, 1);
		this.lblOpening.Name = "lblOpening";
		this.lblOpening.Size = new System.Drawing.Size(111, 15);
		this.lblOpening.TabIndex = 4;
		this.lblOpening.Text = "Account Class";
		this.lblOpening.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCurrDebit.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCurrDebit.Location = new System.Drawing.Point(319, 19);
		this.lblCurrDebit.Name = "lblCurrDebit";
		this.lblCurrDebit.Size = new System.Drawing.Size(111, 15);
		this.lblCurrDebit.TabIndex = 5;
		this.lblCurrDebit.Text = "Account Class";
		this.lblCurrDebit.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(3, 47);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(82, 15);
		this.label28.TabIndex = 8;
		this.label28.Text = "Account Type";
		this.lblCurrCredit.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCurrCredit.Location = new System.Drawing.Point(319, 37);
		this.lblCurrCredit.Name = "lblCurrCredit";
		this.lblCurrCredit.Size = new System.Drawing.Size(111, 15);
		this.lblCurrCredit.TabIndex = 5;
		this.lblCurrCredit.Text = "Account Class";
		this.lblCurrCredit.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(3, 11);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(84, 15);
		this.label32.TabIndex = 7;
		this.label32.Text = "Account Code";
		this.lblClosingBal.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblClosingBal.Location = new System.Drawing.Point(319, 54);
		this.lblClosingBal.Name = "lblClosingBal";
		this.lblClosingBal.Size = new System.Drawing.Size(111, 15);
		this.lblClosingBal.TabIndex = 5;
		this.lblClosingBal.Text = "Account Class";
		this.lblClosingBal.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 29);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(82, 15);
		this.label26.TabIndex = 7;
		this.label26.Text = "Account Class";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(232, 1);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(54, 15);
		this.label21.TabIndex = 6;
		this.label21.Text = "Opening";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(232, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(68, 15);
		this.label22.TabIndex = 6;
		this.label22.Text = "Total Debit";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(232, 54);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(46, 15);
		this.label25.TabIndex = 6;
		this.label25.Text = "Closing";
		this.ValuePostDatedCheqAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ValuePostDatedCheqAmount.Location = new System.Drawing.Point(324, 81);
		this.ValuePostDatedCheqAmount.Name = "ValuePostDatedCheqAmount";
		this.ValuePostDatedCheqAmount.Size = new System.Drawing.Size(111, 15);
		this.ValuePostDatedCheqAmount.TabIndex = 16;
		this.ValuePostDatedCheqAmount.Text = "Account Class";
		this.ValuePostDatedCheqAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.ValuePostDatedCheqAmount.Visible = false;
		this.SubsidiaryBalanceBox.Controls.Add(this.lblSubsidiaryBalance);
		this.SubsidiaryBalanceBox.Controls.Add(this.SubsidiaryAccountName);
		this.SubsidiaryBalanceBox.Location = new System.Drawing.Point(3, 101);
		this.SubsidiaryBalanceBox.Name = "SubsidiaryBalanceBox";
		this.SubsidiaryBalanceBox.Size = new System.Drawing.Size(432, 35);
		this.SubsidiaryBalanceBox.TabIndex = 12;
		this.SubsidiaryBalanceBox.TabStop = false;
		this.SubsidiaryBalanceBox.Text = "Subsidiary Information";
		this.lblSubsidiaryBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryBalance.Location = new System.Drawing.Point(215, 13);
		this.lblSubsidiaryBalance.Name = "lblSubsidiaryBalance";
		this.lblSubsidiaryBalance.Size = new System.Drawing.Size(211, 16);
		this.lblSubsidiaryBalance.TabIndex = 13;
		this.lblSubsidiaryBalance.Text = "0";
		this.lblSubsidiaryBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.SubsidiaryAccountName.AutoSize = true;
		this.SubsidiaryAccountName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.SubsidiaryAccountName.Location = new System.Drawing.Point(5, 14);
		this.SubsidiaryAccountName.Name = "SubsidiaryAccountName";
		this.SubsidiaryAccountName.Size = new System.Drawing.Size(144, 15);
		this.SubsidiaryAccountName.TabIndex = 12;
		this.SubsidiaryAccountName.Text = "Subsidiary Account Name";
		this.ValuePostDatedNoOfCheqs.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ValuePostDatedNoOfCheqs.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValuePostDatedNoOfCheqs.ForeColor = System.Drawing.Color.Blue;
		this.ValuePostDatedNoOfCheqs.Location = new System.Drawing.Point(99, 81);
		this.ValuePostDatedNoOfCheqs.Name = "ValuePostDatedNoOfCheqs";
		this.ValuePostDatedNoOfCheqs.Size = new System.Drawing.Size(62, 15);
		this.ValuePostDatedNoOfCheqs.TabIndex = 14;
		this.ValuePostDatedNoOfCheqs.Text = "0";
		this.ValuePostDatedNoOfCheqs.Visible = false;
		this.ValuePostDatedNoOfCheqs.Click += new System.EventHandler(ValuePostDatedNoOfCheqs_Click);
		this.lblPostedNoOfCheqs.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPostedNoOfCheqs.Location = new System.Drawing.Point(7, 74);
		this.lblPostedNoOfCheqs.Name = "lblPostedNoOfCheqs";
		this.lblPostedNoOfCheqs.Size = new System.Drawing.Size(82, 27);
		this.lblPostedNoOfCheqs.TabIndex = 13;
		this.lblPostedNoOfCheqs.Text = "Post Dated No Of Cheqs";
		this.lblPostedNoOfCheqs.Visible = false;
		this.lblPostDatedCheqAmount.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold);
		this.lblPostDatedCheqAmount.Location = new System.Drawing.Point(234, 74);
		this.lblPostDatedCheqAmount.Name = "lblPostDatedCheqAmount";
		this.lblPostDatedCheqAmount.Size = new System.Drawing.Size(89, 26);
		this.lblPostDatedCheqAmount.TabIndex = 15;
		this.lblPostDatedCheqAmount.Text = "Post Dated Cheqs Amount";
		this.lblPostDatedCheqAmount.Visible = false;
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproject).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(188, 237);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
		this.groupBox1.Controls.Add(this.RdAcByCode);
		this.groupBox1.Controls.Add(this.RdAcByName);
		this.groupBox1.Controls.Add(this.lblBranch);
		this.groupBox1.Controls.Add(this.rdPanel);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtFromDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount);
		this.groupBox1.Controls.Add(this.lblSubsidiaryAccount);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitle);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(403, 158);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters ";
		this.RdAcByCode.AutoSize = true;
		this.RdAcByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdAcByCode.Location = new System.Drawing.Point(31, 81);
		this.RdAcByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdAcByCode.Name = "RdAcByCode";
		this.RdAcByCode.Size = new System.Drawing.Size(52, 19);
		this.RdAcByCode.TabIndex = 587;
		this.RdAcByCode.Text = "Code";
		this.RdAcByCode.UseVisualStyleBackColor = true;
		this.RdAcByName.AutoSize = true;
		this.RdAcByName.Checked = true;
		this.RdAcByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdAcByName.Location = new System.Drawing.Point(31, 65);
		this.RdAcByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdAcByName.Name = "RdAcByName";
		this.RdAcByName.Size = new System.Drawing.Size(48, 19);
		this.RdAcByName.TabIndex = 586;
		this.RdAcByName.TabStop = true;
		this.RdAcByName.Text = "Title";
		this.RdAcByName.UseVisualStyleBackColor = true;
		this.RdAcByName.CheckedChanged += new System.EventHandler(RdAcByName_CheckedChanged);
		this.lblBranch.AutoSize = true;
		this.lblBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranch.Location = new System.Drawing.Point(206, 19);
		this.lblBranch.Name = "lblBranch";
		this.lblBranch.Size = new System.Drawing.Size(44, 15);
		this.lblBranch.TabIndex = 147;
		this.lblBranch.Text = "Branch";
		this.rdPanel.Controls.Add(this.btnshow);
		this.rdPanel.Controls.Add(this.ChkDetail);
		this.rdPanel.Controls.Add(this.ChkSummery1);
		this.rdPanel.Controls.Add(this.ChkSummery2);
		this.rdPanel.Location = new System.Drawing.Point(82, 126);
		this.rdPanel.Name = "rdPanel";
		this.rdPanel.Size = new System.Drawing.Size(320, 28);
		this.rdPanel.TabIndex = 6;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(260, 1);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.ChkDetail.AutoSize = true;
		this.ChkDetail.Checked = true;
		this.ChkDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkDetail.Location = new System.Drawing.Point(3, 5);
		this.ChkDetail.Name = "ChkDetail";
		this.ChkDetail.Size = new System.Drawing.Size(56, 19);
		this.ChkDetail.TabIndex = 0;
		this.ChkDetail.TabStop = true;
		this.ChkDetail.Text = "Detail";
		this.ChkDetail.UseVisualStyleBackColor = true;
		this.ChkSummery1.AutoSize = true;
		this.ChkSummery1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSummery1.Location = new System.Drawing.Point(69, 5);
		this.ChkSummery1.Name = "ChkSummery1";
		this.ChkSummery1.Size = new System.Drawing.Size(86, 19);
		this.ChkSummery1.TabIndex = 1;
		this.ChkSummery1.Text = "Summary-I";
		this.ChkSummery1.UseVisualStyleBackColor = true;
		this.ChkSummery2.AutoSize = true;
		this.ChkSummery2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSummery2.Location = new System.Drawing.Point(162, 5);
		this.ChkSummery2.Name = "ChkSummery2";
		this.ChkSummery2.Size = new System.Drawing.Size(90, 19);
		this.ChkSummery2.TabIndex = 2;
		this.ChkSummery2.Text = "Summary-II";
		this.ChkSummery2.UseVisualStyleBackColor = true;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(4, 19);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(60, 15);
		this.label37.TabIndex = 144;
		this.label37.Text = "Date Type";
		this.CmbDateType.AllowNull = (DefaultableBoolean)1;
		this.CmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Location = new System.Drawing.Point(83, 13);
		((System.Windows.Forms.Control)(object)this.CmbDateType).Name = "CmbDateType";
		((System.Windows.Forms.Control)(object)this.CmbDateType).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateType).TabIndex = 0;
		this.CmbDateType.ValueChanged += new System.EventHandler(CmbDateType_ValueChanged);
		this.cmbbranch.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbbranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(256, 13);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 1;
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(256, 41);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(141, 23);
		this.txtToDate.TabIndex = 3;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(4, 45);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDate.Location = new System.Drawing.Point(83, 41);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(120, 23);
		this.txtFromDate.TabIndex = 2;
		this.CmbSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Location = new System.Drawing.Point(83, 98);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Name = "CmbSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Size = new System.Drawing.Size(315, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).TabIndex = 5;
		this.CmbSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbSubsidiaryAccount_AfterCloseUp);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Leave += new System.EventHandler(CmbSubsidiaryAccount_Leave);
		this.lblSubsidiaryAccount.AutoSize = true;
		this.lblSubsidiaryAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryAccount.Location = new System.Drawing.Point(4, 102);
		this.lblSubsidiaryAccount.Name = "lblSubsidiaryAccount";
		this.lblSubsidiaryAccount.Size = new System.Drawing.Size(79, 15);
		this.lblSubsidiaryAccount.TabIndex = 109;
		this.lblSubsidiaryAccount.Text = "Subsidiary Ac";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(206, 45);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(2, 68);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(32, 30);
		this.label2.TabIndex = 105;
		this.label2.Text = "Acc\r\nount";
		this.cmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Location = new System.Drawing.Point(83, 69);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Name = "cmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).RightToLeft = System.Windows.Forms.RightToLeft.No;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Size = new System.Drawing.Size(315, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Leave += new System.EventHandler(cmbAccountTitle_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(142, 243);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(44, 15);
		this.label1.TabIndex = 104;
		this.label1.Text = "Project";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(142, 216);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(44, 15);
		this.label30.TabIndex = 105;
		this.label30.Text = "Branch";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.panel3.Controls.Add(this.lblCostCenter);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.panel3.Controls.Add(this.lblLanguage);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.ChkBoxPost);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 33);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1628, 29);
		this.panel3.TabIndex = 21;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance162).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(234, 1);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(260, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 135;
		this.lblCostCenter.AutoSize = true;
		this.lblCostCenter.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCostCenter.ForeColor = System.Drawing.Color.White;
		this.lblCostCenter.Location = new System.Drawing.Point(157, 7);
		this.lblCostCenter.Name = "lblCostCenter";
		this.lblCostCenter.Size = new System.Drawing.Size(72, 15);
		this.lblCostCenter.TabIndex = 136;
		this.lblCostCenter.Text = "Cost Center";
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(605, 1);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 133;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLanguage.ForeColor = System.Drawing.Color.White;
		this.lblLanguage.Location = new System.Drawing.Point(536, 6);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(68, 17);
		this.lblLanguage.TabIndex = 134;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(134, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "General Ledger";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1586, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ChkBoxPost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPost.AutoSize = true;
		this.ChkBoxPost.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPost.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPost.Location = new System.Drawing.Point(1390, 4);
		this.ChkBoxPost.Name = "ChkBoxPost";
		this.ChkBoxPost.Size = new System.Drawing.Size(193, 21);
		this.ChkBoxPost.TabIndex = 5;
		this.ChkBoxPost.Text = "Include unposted vouchers";
		this.ChkBoxPost.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.toolStrip3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1628, 33);
		this.panel7.TabIndex = 3;
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnNew, this.print105, this.btnPrint106, this.btnQuantativeLedger, this.btnSummeryLedger, this.btnSummary2, this.BtnPrintSelectedDocSummary2, this.BtnPrintSelectedDocDetail, this.BtnPrintSelectedDocSumm1, this.btnSaveCheckedRows,
			this.rBForCheckAllPreviousRows, this.btnShortcutKey1
		});
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(1628, 33);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 30);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print105.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print105.Image = (System.Drawing.Image)resources.GetObject("print105.Image");
		this.print105.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print105.Name = "print105";
		this.print105.Size = new System.Drawing.Size(92, 30);
		this.print105.Text = "&Print-105";
		this.print105.Click += new System.EventHandler(print_Click_1);
		this.btnPrint106.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint106.Image = (System.Drawing.Image)resources.GetObject("btnPrint106.Image");
		this.btnPrint106.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint106.Name = "btnPrint106";
		this.btnPrint106.Size = new System.Drawing.Size(92, 30);
		this.btnPrint106.Text = "&Print-106";
		this.btnPrint106.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnQuantativeLedger.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnQuantativeLedger.Image = (System.Drawing.Image)resources.GetObject("btnQuantativeLedger.Image");
		this.btnQuantativeLedger.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnQuantativeLedger.Name = "btnQuantativeLedger";
		this.btnQuantativeLedger.Size = new System.Drawing.Size(92, 30);
		this.btnQuantativeLedger.Text = "&Print-107";
		this.btnQuantativeLedger.Click += new System.EventHandler(btnQuantativeLedger_Click);
		this.btnSummeryLedger.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSummeryLedger.Image = (System.Drawing.Image)resources.GetObject("btnSummeryLedger.Image");
		this.btnSummeryLedger.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSummeryLedger.Name = "btnSummeryLedger";
		this.btnSummeryLedger.Size = new System.Drawing.Size(92, 30);
		this.btnSummeryLedger.Text = "Print-108";
		this.btnSummeryLedger.Click += new System.EventHandler(btnSummeryLedger_Click);
		this.btnSummary2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSummary2.Image = (System.Drawing.Image)resources.GetObject("btnSummary2.Image");
		this.btnSummary2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSummary2.Name = "btnSummary2";
		this.btnSummary2.Size = new System.Drawing.Size(101, 30);
		this.btnSummary2.Text = "Print-105A";
		this.btnSummary2.Click += new System.EventHandler(btnSummary2_Click);
		this.BtnPrintSelectedDocSummary2.Image = (System.Drawing.Image)resources.GetObject("BtnPrintSelectedDocSummary2.Image");
		this.BtnPrintSelectedDocSummary2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintSelectedDocSummary2.Name = "BtnPrintSelectedDocSummary2";
		this.BtnPrintSelectedDocSummary2.Size = new System.Drawing.Size(101, 30);
		this.BtnPrintSelectedDocSummary2.Text = "Print-106A";
		this.BtnPrintSelectedDocSummary2.Visible = false;
		this.BtnPrintSelectedDocSummary2.Click += new System.EventHandler(BtnPrintSelectedDocSummary2_Click);
		this.BtnPrintSelectedDocDetail.Image = (System.Drawing.Image)resources.GetObject("BtnPrintSelectedDocDetail.Image");
		this.BtnPrintSelectedDocDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintSelectedDocDetail.Name = "BtnPrintSelectedDocDetail";
		this.BtnPrintSelectedDocDetail.Size = new System.Drawing.Size(101, 30);
		this.BtnPrintSelectedDocDetail.Text = "&Print-107A";
		this.BtnPrintSelectedDocDetail.Visible = false;
		this.BtnPrintSelectedDocDetail.Click += new System.EventHandler(BtnPrintSelectedDocDetail_Click);
		this.BtnPrintSelectedDocSumm1.Image = (System.Drawing.Image)resources.GetObject("BtnPrintSelectedDocSumm1.Image");
		this.BtnPrintSelectedDocSumm1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintSelectedDocSumm1.Name = "BtnPrintSelectedDocSumm1";
		this.BtnPrintSelectedDocSumm1.Size = new System.Drawing.Size(101, 30);
		this.BtnPrintSelectedDocSumm1.Text = "Print-108A";
		this.BtnPrintSelectedDocSumm1.Visible = false;
		this.BtnPrintSelectedDocSumm1.Click += new System.EventHandler(BtnPrintSelectedDocSumm1_Click);
		this.btnSaveCheckedRows.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveCheckedRows.Image = (System.Drawing.Image)resources.GetObject("btnSaveCheckedRows.Image");
		this.btnSaveCheckedRows.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveCheckedRows.Name = "btnSaveCheckedRows";
		this.btnSaveCheckedRows.Size = new System.Drawing.Size(154, 30);
		this.btnSaveCheckedRows.Text = "SaveCheckedRows";
		this.btnSaveCheckedRows.Visible = false;
		this.btnSaveCheckedRows.Click += new System.EventHandler(btnSaveCheckedRows_Click);
		this.rBForCheckAllPreviousRows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.rBForCheckAllPreviousRows.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.rBForCheckAllPreviousRows.Image = (System.Drawing.Image)resources.GetObject("rBForCheckAllPreviousRows.Image");
		this.rBForCheckAllPreviousRows.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.rBForCheckAllPreviousRows.Name = "rBForCheckAllPreviousRows";
		this.rBForCheckAllPreviousRows.Size = new System.Drawing.Size(356, 30);
		this.rBForCheckAllPreviousRows.Text = "Bookmark All Previous Rows After Last Checked Row";
		this.rBForCheckAllPreviousRows.Visible = false;
		this.rBForCheckAllPreviousRows.Click += new System.EventHandler(rBForCheckAllPreviousRows_Click);
		this.btnShortcutKey1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortcutKey1.Image = (System.Drawing.Image)resources.GetObject("btnShortcutKey1.Image");
		this.btnShortcutKey1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKey1.Name = "btnShortcutKey1";
		this.btnShortcutKey1.Size = new System.Drawing.Size(106, 30);
		this.btnShortcutKey1.Text = "ShortCut Keys";
		this.btnShortcutKey1.Click += new System.EventHandler(btnShortcutKey1_Click);
		this.tabMultiAccountsLedger.Controls.Add(this.panel16);
		this.tabMultiAccountsLedger.Controls.Add(this.panel6);
		this.tabMultiAccountsLedger.Controls.Add(this.panel5);
		this.tabMultiAccountsLedger.Controls.Add(this.panel4);
		this.tabMultiAccountsLedger.Controls.Add(this.toolStrip2);
		this.tabMultiAccountsLedger.Location = new System.Drawing.Point(4, 22);
		this.tabMultiAccountsLedger.Name = "tabMultiAccountsLedger";
		this.tabMultiAccountsLedger.Padding = new System.Windows.Forms.Padding(3);
		this.tabMultiAccountsLedger.Size = new System.Drawing.Size(1634, 691);
		this.tabMultiAccountsLedger.TabIndex = 1;
		this.tabMultiAccountsLedger.Text = "Multi Accounts Ledger";
		this.tabMultiAccountsLedger.UseVisualStyleBackColor = true;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdMultiAccountsLedger);
		this.panel16.Controls.Add(this.ctrlGrdBar3);
		this.panel16.Controls.Add(this.label34);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(467, 178);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1164, 510);
		this.panel16.TabIndex = 25;
		this.grdMultiAccountsLedger.AllowEdit = (InheritableBoolean)2;
		this.grdMultiAccountsLedger.AlternatingColors = true;
		this.grdMultiAccountsLedger.AutomaticSort = false;
		this.grdMultiAccountsLedger.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdMultiAccountsLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMultiAccountsLedger.DesignTimeLayout = grdMultiAccountsLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMultiAccountsLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdMultiAccountsLedger.DynamicFiltering = true;
		this.grdMultiAccountsLedger.FilterMode = (FilterMode)2;
		this.grdMultiAccountsLedger.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMultiAccountsLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMultiAccountsLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMultiAccountsLedger).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdMultiAccountsLedger).Name = "grdMultiAccountsLedger";
		((ScrollableBase)this.grdMultiAccountsLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMultiAccountsLedger).Size = new System.Drawing.Size(1164, 485);
		((System.Windows.Forms.Control)(object)this.grdMultiAccountsLedger).TabIndex = 2;
		this.grdMultiAccountsLedger.TotalRow = (InheritableBoolean)1;
		this.grdMultiAccountsLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMultiAccountsLedger).VisualStyle = (VisualStyle)4;
		this.grdMultiAccountsLedger.LinkClicked += new ColumnActionEventHandler(grdMultiAccountsLedger_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdMultiAccountsLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdMultiAccountsLedger_KeyDown);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1123, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdMultiAccountsLedger;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label34.BackColor = System.Drawing.Color.Teal;
		this.label34.Dock = System.Windows.Forms.DockStyle.Top;
		this.label34.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(0, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(1164, 25);
		this.label34.TabIndex = 1;
		this.label34.Text = "Detail Accounts Ledger";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel6.Controls.Add(this.ctrlGrdBar7);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdAccounts);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(3, 178);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(464, 510);
		this.panel6.TabIndex = 24;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(420, 2);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdAccounts;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 21;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.grdAccounts.AllowEdit = (InheritableBoolean)2;
		this.grdAccounts.AlternatingColors = true;
		this.grdAccounts.AutomaticSort = false;
		this.grdAccounts.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdAccounts_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAccounts.DesignTimeLayout = grdAccounts_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAccounts).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAccounts.DynamicFiltering = true;
		this.grdAccounts.FilterMode = (FilterMode)2;
		this.grdAccounts.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAccounts).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAccounts.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAccounts).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdAccounts).Name = "grdAccounts";
		((ScrollableBase)this.grdAccounts).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAccounts).Size = new System.Drawing.Size(464, 485);
		((System.Windows.Forms.Control)(object)this.grdAccounts).TabIndex = 1;
		this.grdAccounts.TotalRow = (InheritableBoolean)1;
		this.grdAccounts.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAccounts).VisualStyle = (VisualStyle)4;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(464, 25);
		this.label5.TabIndex = 0;
		this.label5.Text = "Detail Accounts";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 59);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1628, 119);
		this.panel5.TabIndex = 23;
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateTypeMulti);
		this.groupBox3.Controls.Add(this.btnShowMultiLedger);
		this.groupBox3.Controls.Add(this.rdSummary2Print);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.rdSummary1Print);
		this.groupBox3.Controls.Add(this.ToDateForMulti);
		this.groupBox3.Controls.Add(this.rdDetailPrint);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.FromDateForMulti);
		this.groupBox3.Controls.Add(this.chkIncludeUnpostedVouchers);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(469, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(490, 110);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters To Get Ledger";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(14, 27);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(60, 15);
		this.label36.TabIndex = 144;
		this.label36.Text = "Date Type";
		this.CmbDateTypeMulti.AllowNull = (DefaultableBoolean)1;
		this.CmbDateTypeMulti.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateTypeMulti.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateTypeMulti.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateTypeMulti).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeMulti).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateTypeMulti.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeMulti).Location = new System.Drawing.Point(86, 21);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeMulti).Name = "CmbDateTypeMulti";
		((System.Windows.Forms.Control)(object)this.CmbDateTypeMulti).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeMulti).TabIndex = 0;
		this.CmbDateTypeMulti.ValueChanged += new System.EventHandler(CmbDateTypeMulti_ValueChanged);
		this.btnShowMultiLedger.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowMultiLedger.FlatAppearance.BorderSize = 0;
		this.btnShowMultiLedger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowMultiLedger.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowMultiLedger.ForeColor = System.Drawing.Color.White;
		this.btnShowMultiLedger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowMultiLedger.Location = new System.Drawing.Point(216, 75);
		this.btnShowMultiLedger.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowMultiLedger.Name = "btnShowMultiLedger";
		this.btnShowMultiLedger.Size = new System.Drawing.Size(56, 24);
		this.btnShowMultiLedger.TabIndex = 7;
		this.btnShowMultiLedger.Text = "Show";
		this.btnShowMultiLedger.UseVisualStyleBackColor = false;
		this.btnShowMultiLedger.Click += new System.EventHandler(btnShowMultiLedger_Click);
		this.rdSummary2Print.AutoSize = true;
		this.rdSummary2Print.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSummary2Print.Location = new System.Drawing.Point(370, 52);
		this.rdSummary2Print.Name = "rdSummary2Print";
		this.rdSummary2Print.Size = new System.Drawing.Size(90, 19);
		this.rdSummary2Print.TabIndex = 6;
		this.rdSummary2Print.Text = "Summary-II";
		this.rdSummary2Print.UseVisualStyleBackColor = true;
		this.rdSummary2Print.Visible = false;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(14, 80);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(48, 15);
		this.label31.TabIndex = 113;
		this.label31.Text = "To Date";
		this.rdSummary1Print.AutoSize = true;
		this.rdSummary1Print.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSummary1Print.Location = new System.Drawing.Point(278, 52);
		this.rdSummary1Print.Name = "rdSummary1Print";
		this.rdSummary1Print.Size = new System.Drawing.Size(86, 19);
		this.rdSummary1Print.TabIndex = 5;
		this.rdSummary1Print.Text = "Summary-I";
		this.rdSummary1Print.UseVisualStyleBackColor = true;
		this.rdSummary1Print.Visible = false;
		this.ToDateForMulti.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateForMulti.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.ToDateForMulti.Location = new System.Drawing.Point(86, 76);
		this.ToDateForMulti.Name = "ToDateForMulti";
		this.ToDateForMulti.Size = new System.Drawing.Size(124, 23);
		this.ToDateForMulti.TabIndex = 2;
		this.rdDetailPrint.AutoSize = true;
		this.rdDetailPrint.Checked = true;
		this.rdDetailPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDetailPrint.Location = new System.Drawing.Point(216, 52);
		this.rdDetailPrint.Name = "rdDetailPrint";
		this.rdDetailPrint.Size = new System.Drawing.Size(56, 19);
		this.rdDetailPrint.TabIndex = 4;
		this.rdDetailPrint.TabStop = true;
		this.rdDetailPrint.Text = "Detail";
		this.rdDetailPrint.UseVisualStyleBackColor = true;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(14, 54);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(63, 15);
		this.label10.TabIndex = 111;
		this.label10.Text = "From Date";
		this.FromDateForMulti.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateForMulti.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.FromDateForMulti.Location = new System.Drawing.Point(86, 50);
		this.FromDateForMulti.Name = "FromDateForMulti";
		this.FromDateForMulti.Size = new System.Drawing.Size(124, 23);
		this.FromDateForMulti.TabIndex = 1;
		this.chkIncludeUnpostedVouchers.AutoSize = true;
		this.chkIncludeUnpostedVouchers.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIncludeUnpostedVouchers.Location = new System.Drawing.Point(216, 25);
		this.chkIncludeUnpostedVouchers.Name = "chkIncludeUnpostedVouchers";
		this.chkIncludeUnpostedVouchers.Size = new System.Drawing.Size(170, 19);
		this.chkIncludeUnpostedVouchers.TabIndex = 3;
		this.chkIncludeUnpostedVouchers.Text = "Include unposted vouchers";
		this.chkIncludeUnpostedVouchers.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.datToDate);
		this.groupBox2.Controls.Add(this.btnGetAccounts);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomGroup);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGroupAccount);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(459, 110);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters To Get Accounts";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(8, 25);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(48, 15);
		this.label35.TabIndex = 113;
		this.label35.Text = "To Date";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datToDate.Location = new System.Drawing.Point(99, 21);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(249, 23);
		this.datToDate.TabIndex = 0;
		this.btnGetAccounts.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnGetAccounts.FlatAppearance.BorderSize = 0;
		this.btnGetAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGetAccounts.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGetAccounts.ForeColor = System.Drawing.Color.White;
		this.btnGetAccounts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnGetAccounts.Location = new System.Drawing.Point(353, 53);
		this.btnGetAccounts.Margin = new System.Windows.Forms.Padding(2);
		this.btnGetAccounts.Name = "btnGetAccounts";
		this.btnGetAccounts.Size = new System.Drawing.Size(94, 46);
		this.btnGetAccounts.TabIndex = 3;
		this.btnGetAccounts.Text = "Get Accounts";
		this.btnGetAccounts.UseVisualStyleBackColor = false;
		this.btnGetAccounts.Click += new System.EventHandler(btnGetAccounts_Click);
		this.cmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance160;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).Location = new System.Drawing.Point(99, 75);
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).Name = "cmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(8, 80);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(84, 15);
		this.label7.TabIndex = 109;
		this.label7.Text = "Custom Group";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(8, 53);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(88, 15);
		this.label29.TabIndex = 105;
		this.label29.Text = "Group Account";
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
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGroupAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).Location = new System.Drawing.Point(99, 47);
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).Name = "CmbGroupAccount";
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbGroupAccount).TabIndex = 1;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1628, 29);
		this.panel4.TabIndex = 22;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(6, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(189, 19);
		this.label4.TabIndex = 1;
		this.label4.Text = "Multi Accounts Ledger";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnReset, this.btnRefresh1, this.btn105MultiPrint, this.btn105AMultiPrint, this.btn106MultiPrint, this.btn107MultiPrint, this.btn108MultiPrint, this.btnShortCutKey });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip2.Size = new System.Drawing.Size(1628, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.Image = (System.Drawing.Image)resources.GetObject("btnReset.Image");
		this.btnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(68, 24);
		this.btnReset.Text = "&Reset";
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnRefresh1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh1.Image = (System.Drawing.Image)resources.GetObject("btnRefresh1.Image");
		this.btnRefresh1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh1.Name = "btnRefresh1";
		this.btnRefresh1.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh1.Text = "&Refresh";
		this.btnRefresh1.Click += new System.EventHandler(btnRefresh1_Click);
		this.btn105MultiPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn105MultiPrint.Image = (System.Drawing.Image)resources.GetObject("btn105MultiPrint.Image");
		this.btn105MultiPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn105MultiPrint.Name = "btn105MultiPrint";
		this.btn105MultiPrint.Size = new System.Drawing.Size(92, 24);
		this.btn105MultiPrint.Text = "&Print-105";
		this.btn105MultiPrint.Click += new System.EventHandler(btn105MultiPrint_Click);
		this.btn105AMultiPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn105AMultiPrint.Image = (System.Drawing.Image)resources.GetObject("btn105AMultiPrint.Image");
		this.btn105AMultiPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn105AMultiPrint.Name = "btn105AMultiPrint";
		this.btn105AMultiPrint.Size = new System.Drawing.Size(101, 24);
		this.btn105AMultiPrint.Text = "Print-105A";
		this.btn105AMultiPrint.Click += new System.EventHandler(btn105AMultiPrint_Click);
		this.btn106MultiPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn106MultiPrint.Image = (System.Drawing.Image)resources.GetObject("btn106MultiPrint.Image");
		this.btn106MultiPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn106MultiPrint.Name = "btn106MultiPrint";
		this.btn106MultiPrint.Size = new System.Drawing.Size(92, 24);
		this.btn106MultiPrint.Text = "&Print-106";
		this.btn106MultiPrint.Click += new System.EventHandler(btn106MultiPrint_Click);
		this.btn107MultiPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn107MultiPrint.Image = (System.Drawing.Image)resources.GetObject("btn107MultiPrint.Image");
		this.btn107MultiPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn107MultiPrint.Name = "btn107MultiPrint";
		this.btn107MultiPrint.Size = new System.Drawing.Size(92, 24);
		this.btn107MultiPrint.Text = "&Print-107";
		this.btn107MultiPrint.Click += new System.EventHandler(btn107MultiPrint_Click);
		this.btn108MultiPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn108MultiPrint.Image = (System.Drawing.Image)resources.GetObject("btn108MultiPrint.Image");
		this.btn108MultiPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn108MultiPrint.Name = "btn108MultiPrint";
		this.btn108MultiPrint.Size = new System.Drawing.Size(92, 24);
		this.btn108MultiPrint.Text = "Print-108";
		this.btn108MultiPrint.Click += new System.EventHandler(btn108MultiPrint_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabBankSummaryLedger.Controls.Add(this.panel18);
		this.tabBankSummaryLedger.Controls.Add(this.panel19);
		this.tabBankSummaryLedger.Controls.Add(this.panel17);
		this.tabBankSummaryLedger.Location = new System.Drawing.Point(4, 22);
		this.tabBankSummaryLedger.Name = "tabBankSummaryLedger";
		this.tabBankSummaryLedger.Size = new System.Drawing.Size(1634, 691);
		this.tabBankSummaryLedger.TabIndex = 2;
		this.tabBankSummaryLedger.Text = "Bank Summary Ledger";
		this.tabBankSummaryLedger.UseVisualStyleBackColor = true;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailBankSummLedger);
		this.panel18.Controls.Add(this.ctrlGrdBar6);
		this.panel18.Controls.Add(this.label40);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel18.Location = new System.Drawing.Point(519, 106);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(567, 585);
		this.panel18.TabIndex = 27;
		this.grdDetailBankSummLedger.AllowEdit = (InheritableBoolean)2;
		this.grdDetailBankSummLedger.AlternatingColors = true;
		this.grdDetailBankSummLedger.AutomaticSort = false;
		this.grdDetailBankSummLedger.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdDetailBankSummLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailBankSummLedger.DesignTimeLayout = grdDetailBankSummLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailBankSummLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailBankSummLedger.DynamicFiltering = true;
		this.grdDetailBankSummLedger.FilterMode = (FilterMode)2;
		this.grdDetailBankSummLedger.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailBankSummLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailBankSummLedger.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetailBankSummLedger).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdDetailBankSummLedger).Name = "grdDetailBankSummLedger";
		((ScrollableBase)this.grdDetailBankSummLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailBankSummLedger).Size = new System.Drawing.Size(567, 560);
		((System.Windows.Forms.Control)(object)this.grdDetailBankSummLedger).TabIndex = 0;
		this.grdDetailBankSummLedger.TotalRow = (InheritableBoolean)1;
		this.grdDetailBankSummLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailBankSummLedger).VisualStyle = (VisualStyle)4;
		this.grdDetailBankSummLedger.LinkClicked += new ColumnActionEventHandler(grdDetailBankSummLedger_LinkClicked);
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(526, 2);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar6.MyGrid = this.grdDetailBankSummLedger;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar6.TabIndex = 20;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.label40.BackColor = System.Drawing.Color.Teal;
		this.label40.Dock = System.Windows.Forms.DockStyle.Top;
		this.label40.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(0, 0);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(567, 25);
		this.label40.TabIndex = 1;
		this.label40.Text = "Selected Bank Accounts Ledger";
		this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdAccountsBankSummLedger);
		this.panel19.Controls.Add(this.label42);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 106);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(519, 585);
		this.panel19.TabIndex = 26;
		this.grdAccountsBankSummLedger.AllowEdit = (InheritableBoolean)2;
		this.grdAccountsBankSummLedger.AlternatingColors = true;
		this.grdAccountsBankSummLedger.AutomaticSort = false;
		this.grdAccountsBankSummLedger.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdAccountsBankSummLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAccountsBankSummLedger.DesignTimeLayout = grdAccountsBankSummLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAccountsBankSummLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAccountsBankSummLedger.DynamicFiltering = true;
		this.grdAccountsBankSummLedger.FilterMode = (FilterMode)2;
		this.grdAccountsBankSummLedger.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAccountsBankSummLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAccountsBankSummLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAccountsBankSummLedger).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdAccountsBankSummLedger).Name = "grdAccountsBankSummLedger";
		((ScrollableBase)this.grdAccountsBankSummLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAccountsBankSummLedger).Size = new System.Drawing.Size(519, 560);
		((System.Windows.Forms.Control)(object)this.grdAccountsBankSummLedger).TabIndex = 0;
		this.grdAccountsBankSummLedger.TotalRow = (InheritableBoolean)1;
		this.grdAccountsBankSummLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAccountsBankSummLedger).VisualStyle = (VisualStyle)4;
		this.grdAccountsBankSummLedger.LinkClicked += new ColumnActionEventHandler(grdAccountsBankSummLedger_LinkClicked);
		this.label42.BackColor = System.Drawing.Color.Teal;
		this.label42.Dock = System.Windows.Forms.DockStyle.Top;
		this.label42.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.White;
		this.label42.Location = new System.Drawing.Point(0, 0);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(519, 25);
		this.label42.TabIndex = 0;
		this.label42.Text = "Bank Accounts";
		this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel17.Controls.Add(this.panel32);
		this.panel17.Controls.Add(this.panel31);
		this.panel17.Controls.Add(this.toolStrip6);
		this.panel17.Controls.Add(this.groupBox4);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1634, 106);
		this.panel17.TabIndex = 0;
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummaryValuesBanksummLegder);
		this.panel32.Controls.Add(this.label44);
		this.panel32.Location = new System.Drawing.Point(1078, 185);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(322, 140);
		this.panel32.TabIndex = 28;
		this.grdSummaryValuesBanksummLegder.AllowEdit = (InheritableBoolean)2;
		this.grdSummaryValuesBanksummLegder.AlternatingColors = true;
		this.grdSummaryValuesBanksummLegder.AutomaticSort = false;
		this.grdSummaryValuesBanksummLegder.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdSummaryValuesBanksummLegder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummaryValuesBanksummLegder.DesignTimeLayout = grdSummaryValuesBanksummLegder_DesignTimeLayout;
		this.grdSummaryValuesBanksummLegder.DynamicFiltering = true;
		this.grdSummaryValuesBanksummLegder.FilterMode = (FilterMode)2;
		this.grdSummaryValuesBanksummLegder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummaryValuesBanksummLegder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummaryValuesBanksummLegder.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummaryValuesBanksummLegder).Location = new System.Drawing.Point(6, 28);
		((System.Windows.Forms.Control)(object)this.grdSummaryValuesBanksummLegder).Name = "grdSummaryValuesBanksummLegder";
		((ScrollableBase)this.grdSummaryValuesBanksummLegder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummaryValuesBanksummLegder).Size = new System.Drawing.Size(309, 104);
		((System.Windows.Forms.Control)(object)this.grdSummaryValuesBanksummLegder).TabIndex = 0;
		this.grdSummaryValuesBanksummLegder.TotalRow = (InheritableBoolean)1;
		this.grdSummaryValuesBanksummLegder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummaryValuesBanksummLegder).VisualStyle = (VisualStyle)4;
		this.label44.BackColor = System.Drawing.Color.Teal;
		this.label44.Dock = System.Windows.Forms.DockStyle.Top;
		this.label44.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(0, 0);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(322, 25);
		this.label44.TabIndex = 1;
		this.label44.Text = "Summary";
		this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel31.BackColor = System.Drawing.Color.Teal;
		this.panel31.Controls.Add(this.label43);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(0, 27);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1634, 29);
		this.panel31.TabIndex = 24;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.Teal;
		this.label43.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(6, 5);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(192, 19);
		this.label43.TabIndex = 1;
		this.label43.Text = "Bank Summary Ledger";
		this.toolStrip6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNewBanksummLegder, this.BtnPrintBanksummLegder, this.BtnBankSummaryPrint02, this.BtnShortCutKeysBanksummLegder });
		this.toolStrip6.Location = new System.Drawing.Point(0, 0);
		this.toolStrip6.Name = "toolStrip6";
		this.toolStrip6.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip6.Size = new System.Drawing.Size(1634, 27);
		this.toolStrip6.TabIndex = 23;
		this.toolStrip6.Text = "toolStrip6";
		this.BtnNewBanksummLegder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewBanksummLegder.Image = (System.Drawing.Image)resources.GetObject("BtnNewBanksummLegder.Image");
		this.BtnNewBanksummLegder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewBanksummLegder.Name = "BtnNewBanksummLegder";
		this.BtnNewBanksummLegder.Size = new System.Drawing.Size(68, 24);
		this.BtnNewBanksummLegder.Text = "&Reset";
		this.BtnNewBanksummLegder.Click += new System.EventHandler(BtnNewBanksummLegder_Click);
		this.BtnPrintBanksummLegder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintBanksummLegder.Image = (System.Drawing.Image)resources.GetObject("BtnPrintBanksummLegder.Image");
		this.BtnPrintBanksummLegder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintBanksummLegder.Name = "BtnPrintBanksummLegder";
		this.BtnPrintBanksummLegder.Size = new System.Drawing.Size(117, 24);
		this.BtnPrintBanksummLegder.Text = "&Print_105_01";
		this.BtnPrintBanksummLegder.Click += new System.EventHandler(BtnPrintBanksummLegder_Click);
		this.BtnBankSummaryPrint02.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnBankSummaryPrint02.Image = (System.Drawing.Image)resources.GetObject("BtnBankSummaryPrint02.Image");
		this.BtnBankSummaryPrint02.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnBankSummaryPrint02.Name = "BtnBankSummaryPrint02";
		this.BtnBankSummaryPrint02.Size = new System.Drawing.Size(117, 24);
		this.BtnBankSummaryPrint02.Text = "&Print_105_02";
		this.BtnBankSummaryPrint02.Click += new System.EventHandler(BtnBankSummaryPrint02_Click);
		this.BtnShortCutKeysBanksummLegder.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutKeysBanksummLegder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutKeysBanksummLegder.Name = "BtnShortCutKeysBanksummLegder";
		this.BtnShortCutKeysBanksummLegder.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutKeysBanksummLegder.Text = "ShortCut Keys";
		this.BtnShortCutKeysBanksummLegder.Click += new System.EventHandler(btnShortCutKey_Click);
		this.groupBox4.Controls.Add(this.label45);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTypesForBankSummaryLedger);
		this.groupBox4.Controls.Add(this.label38);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateTypeBankSummLedger);
		this.groupBox4.Controls.Add(this.txttoDateBankSummLedger);
		this.groupBox4.Controls.Add(this.label39);
		this.groupBox4.Controls.Add(this.txtFromDateBankSummLedger);
		this.groupBox4.Controls.Add(this.label41);
		this.groupBox4.Controls.Add(this.BtnShowBankSummLedger);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(3, 56);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1083, 46);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters ";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(674, 20);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(85, 15);
		this.label45.TabIndex = 146;
		this.label45.Text = "Account Types";
		this.CmbAccountTypesForBankSummaryLedger.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTypesForBankSummaryLedger.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTypesForBankSummaryLedger.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTypesForBankSummaryLedger.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTypesForBankSummaryLedger).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypesForBankSummaryLedger).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypesForBankSummaryLedger).Location = new System.Drawing.Point(760, 14);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypesForBankSummaryLedger).Name = "CmbAccountTypesForBankSummaryLedger";
		((System.Windows.Forms.Control)(object)this.CmbAccountTypesForBankSummaryLedger).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypesForBankSummaryLedger).TabIndex = 3;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(9, 20);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(60, 15);
		this.label38.TabIndex = 144;
		this.label38.Text = "Date Type";
		this.CmbDateTypeBankSummLedger.AllowNull = (DefaultableBoolean)1;
		this.CmbDateTypeBankSummLedger.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateTypeBankSummLedger.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateTypeBankSummLedger.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateTypeBankSummLedger).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeBankSummLedger).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateTypeBankSummLedger.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeBankSummLedger).Location = new System.Drawing.Point(76, 14);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeBankSummLedger).Name = "CmbDateTypeBankSummLedger";
		((System.Windows.Forms.Control)(object)this.CmbDateTypeBankSummLedger).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeBankSummLedger).TabIndex = 0;
		this.CmbDateTypeBankSummLedger.ValueChanged += new System.EventHandler(CmbDateTypeBankSummLedger_ValueChanged);
		this.txttoDateBankSummLedger.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttoDateBankSummLedger.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txttoDateBankSummLedger.Location = new System.Drawing.Point(514, 16);
		this.txttoDateBankSummLedger.Name = "txttoDateBankSummLedger";
		this.txttoDateBankSummLedger.Size = new System.Drawing.Size(153, 23);
		this.txttoDateBankSummLedger.TabIndex = 2;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(237, 20);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(63, 15);
		this.label39.TabIndex = 103;
		this.label39.Text = "From Date";
		this.txtFromDateBankSummLedger.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateBankSummLedger.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDateBankSummLedger.Location = new System.Drawing.Point(304, 16);
		this.txtFromDateBankSummLedger.Name = "txtFromDateBankSummLedger";
		this.txtFromDateBankSummLedger.Size = new System.Drawing.Size(153, 23);
		this.txtFromDateBankSummLedger.TabIndex = 1;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(460, 20);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(48, 15);
		this.label41.TabIndex = 102;
		this.label41.Text = "To Date";
		this.BtnShowBankSummLedger.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowBankSummLedger.FlatAppearance.BorderSize = 0;
		this.BtnShowBankSummLedger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowBankSummLedger.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowBankSummLedger.ForeColor = System.Drawing.Color.White;
		this.BtnShowBankSummLedger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowBankSummLedger.Location = new System.Drawing.Point(1009, 14);
		this.BtnShowBankSummLedger.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowBankSummLedger.Name = "BtnShowBankSummLedger";
		this.BtnShowBankSummLedger.Size = new System.Drawing.Size(58, 26);
		this.BtnShowBankSummLedger.TabIndex = 4;
		this.BtnShowBankSummLedger.Text = "Show";
		this.BtnShowBankSummLedger.UseVisualStyleBackColor = false;
		this.BtnShowBankSummLedger.Click += new System.EventHandler(BtnShowBankSummLedger_Click);
		this.tabPageCustomerLedger.Location = new System.Drawing.Point(4, 4);
		this.tabPageCustomerLedger.Name = "tabPageCustomerLedger";
		this.tabPageCustomerLedger.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageCustomerLedger.Size = new System.Drawing.Size(1362, 723);
		this.tabPageCustomerLedger.TabIndex = 2;
		this.tabPageCustomerLedger.Text = "Party Ledger";
		this.tabPageCustomerLedger.UseVisualStyleBackColor = true;
		this.tabPageItemLedger.Controls.Add(this.panel30);
		this.tabPageItemLedger.Location = new System.Drawing.Point(4, 4);
		this.tabPageItemLedger.Name = "tabPageItemLedger";
		this.tabPageItemLedger.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageItemLedger.Size = new System.Drawing.Size(1362, 723);
		this.tabPageItemLedger.TabIndex = 4;
		this.tabPageItemLedger.Text = "Item Ledger";
		this.tabPageItemLedger.UseVisualStyleBackColor = true;
		this.panel30.Controls.Add(this.panel28);
		this.panel30.Controls.Add(this.panel29);
		this.panel30.Controls.Add(this.panel27);
		this.panel30.Controls.Add(this.panel26);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel30.Location = new System.Drawing.Point(3, 3);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1356, 717);
		this.panel30.TabIndex = 1;
		this.panel28.Controls.Add((System.Windows.Forms.Control)(object)this.grdItemLedger);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel28.Location = new System.Drawing.Point(0, 128);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1356, 589);
		this.panel28.TabIndex = 3;
		this.grdItemLedger.AllowEdit = (InheritableBoolean)2;
		this.grdItemLedger.AlternatingColors = true;
		grdItemLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItemLedger.DesignTimeLayout = grdItemLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItemLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdItemLedger.DynamicFiltering = true;
		this.grdItemLedger.FilterMode = (FilterMode)2;
		this.grdItemLedger.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdItemLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItemLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdItemLedger).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdItemLedger).Name = "grdItemLedger";
		((ScrollableBase)this.grdItemLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemLedger).Size = new System.Drawing.Size(1356, 589);
		((System.Windows.Forms.Control)(object)this.grdItemLedger).TabIndex = 0;
		this.grdItemLedger.TotalRow = (InheritableBoolean)1;
		this.grdItemLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItemLedger).VisualStyle = (VisualStyle)4;
		this.grdItemLedger.LinkClicked += new ColumnActionEventHandler(grdItemLedger_LinkClicked);
		this.panel29.BackColor = System.Drawing.Color.White;
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameItmLedger);
		this.panel29.Controls.Add(this.label11);
		this.panel29.Controls.Add(this.txtToDateItemLedger);
		this.panel29.Controls.Add(this.txtFromDateItemLedger);
		this.panel29.Controls.Add(this.btnShowItemLedger);
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStockAccountItemLedger);
		this.panel29.Controls.Add(this.label16);
		this.panel29.Controls.Add(this.label18);
		this.panel29.Controls.Add(this.label19);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(0, 67);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1356, 61);
		this.panel29.TabIndex = 0;
		this.CmbItemNameItmLedger.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameItmLedger.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameItmLedger.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameItmLedger.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Appearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance3).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameItmLedger).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameItmLedger).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameItmLedger.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameItmLedger).Location = new System.Drawing.Point(529, 29);
		((System.Windows.Forms.Control)(object)this.CmbItemNameItmLedger).Name = "CmbItemNameItmLedger";
		((System.Windows.Forms.Control)(object)this.CmbItemNameItmLedger).Size = new System.Drawing.Size(274, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameItmLedger).TabIndex = 3;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(526, 9);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(65, 15);
		this.label11.TabIndex = 149;
		this.label11.Text = "Item Name";
		this.txtToDateItemLedger.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateItemLedger.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDateItemLedger.Location = new System.Drawing.Point(135, 30);
		this.txtToDateItemLedger.Name = "txtToDateItemLedger";
		this.txtToDateItemLedger.Size = new System.Drawing.Size(121, 25);
		this.txtToDateItemLedger.TabIndex = 1;
		this.txtFromDateItemLedger.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateItemLedger.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDateItemLedger.Location = new System.Drawing.Point(17, 30);
		this.txtFromDateItemLedger.Name = "txtFromDateItemLedger";
		this.txtFromDateItemLedger.Size = new System.Drawing.Size(115, 25);
		this.txtFromDateItemLedger.TabIndex = 0;
		this.btnShowItemLedger.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.btnShowItemLedger.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowItemLedger.Location = new System.Drawing.Point(808, 29);
		this.btnShowItemLedger.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowItemLedger.Name = "btnShowItemLedger";
		this.btnShowItemLedger.Size = new System.Drawing.Size(46, 26);
		this.btnShowItemLedger.TabIndex = 4;
		this.btnShowItemLedger.Text = "Show";
		this.btnShowItemLedger.UseVisualStyleBackColor = false;
		this.btnShowItemLedger.Click += new System.EventHandler(btnShowItemLedger_Click);
		this.cmbStockAccountItemLedger.AllowNull = (DefaultableBoolean)1;
		this.cmbStockAccountItemLedger.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStockAccountItemLedger.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStockAccountItemLedger.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStockAccountItemLedger).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStockAccountItemLedger).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStockAccountItemLedger.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStockAccountItemLedger).Location = new System.Drawing.Point(258, 29);
		((System.Windows.Forms.Control)(object)this.cmbStockAccountItemLedger).Name = "cmbStockAccountItemLedger";
		((System.Windows.Forms.Control)(object)this.cmbStockAccountItemLedger).Size = new System.Drawing.Size(270, 26);
		((System.Windows.Forms.Control)(object)this.cmbStockAccountItemLedger).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbStockAccountItemLedger).Leave += new System.EventHandler(cmbStockAccountItemLedger_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(255, 9);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(83, 15);
		this.label16.TabIndex = 147;
		this.label16.Text = "Stock Account";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(14, 9);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(63, 15);
		this.label18.TabIndex = 139;
		this.label18.Text = "From Date";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(132, 9);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(47, 15);
		this.label19.TabIndex = 138;
		this.label19.Text = "To Date";
		this.panel27.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel27.Controls.Add(this.label17);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(0, 26);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1356, 41);
		this.panel27.TabIndex = 1;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(7, 8);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(225, 24);
		this.label17.TabIndex = 4;
		this.label17.Text = "Item Ledger (@Accounts)";
		this.panel26.Controls.Add(this.ctrlGrdBar5);
		this.panel26.Controls.Add(this.toolStrip5);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1356, 26);
		this.panel26.TabIndex = 0;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1303, -1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.grdItemLedger;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar5.TabIndex = 8;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewItemLedger, this.btnRegisterItemLedger, this.btnItemLedger_164 });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1356, 27);
		this.toolStrip5.TabIndex = 7;
		this.toolStrip5.Text = "toolStrip5";
		this.btnNewItemLedger.Image = (System.Drawing.Image)resources.GetObject("btnNewItemLedger.Image");
		this.btnNewItemLedger.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewItemLedger.Name = "btnNewItemLedger";
		this.btnNewItemLedger.Size = new System.Drawing.Size(55, 24);
		this.btnNewItemLedger.Text = "&New";
		this.btnNewItemLedger.Click += new System.EventHandler(btnNewItemLedger_Click);
		this.btnRegisterItemLedger.Image = (System.Drawing.Image)resources.GetObject("btnRegisterItemLedger.Image");
		this.btnRegisterItemLedger.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegisterItemLedger.Name = "btnRegisterItemLedger";
		this.btnRegisterItemLedger.Size = new System.Drawing.Size(56, 24);
		this.btnRegisterItemLedger.Text = "Print";
		this.btnRegisterItemLedger.Visible = false;
		this.btnRegisterItemLedger.Click += new System.EventHandler(btnRegisterItemLedger_Click);
		this.btnItemLedger_164.Image = (System.Drawing.Image)resources.GetObject("btnItemLedger_164.Image");
		this.btnItemLedger_164.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnItemLedger_164.Name = "btnItemLedger_164";
		this.btnItemLedger_164.Size = new System.Drawing.Size(79, 24);
		this.btnItemLedger_164.Text = "164-Print";
		this.btnItemLedger_164.Click += new System.EventHandler(btnItemLedger_164_Click);
		this.tabPageCommissionAgent.Location = new System.Drawing.Point(4, 4);
		this.tabPageCommissionAgent.Name = "tabPageCommissionAgent";
		this.tabPageCommissionAgent.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageCommissionAgent.Size = new System.Drawing.Size(1362, 723);
		this.tabPageCommissionAgent.TabIndex = 1;
		this.tabPageCommissionAgent.Text = "Commission Agent";
		this.tabPageCommissionAgent.UseVisualStyleBackColor = true;
		this.tabPageFcyGeneralLedger.Location = new System.Drawing.Point(4, 4);
		this.tabPageFcyGeneralLedger.Name = "tabPageFcyGeneralLedger";
		this.tabPageFcyGeneralLedger.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageFcyGeneralLedger.Size = new System.Drawing.Size(1362, 723);
		this.tabPageFcyGeneralLedger.TabIndex = 3;
		this.tabPageFcyGeneralLedger.Text = "Fcy General Ledger";
		this.tabPageFcyGeneralLedger.UseVisualStyleBackColor = true;
		this.comSupplier.AllowNull = (DefaultableBoolean)1;
		this.comSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.comSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.comSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.comSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comSupplier).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comSupplier).Location = new System.Drawing.Point(318, 30);
		((System.Windows.Forms.Control)(object)this.comSupplier).Name = "comSupplier";
		((System.Windows.Forms.Control)(object)this.comSupplier).Size = new System.Drawing.Size(359, 27);
		((System.Windows.Forms.Control)(object)this.comSupplier).TabIndex = 2;
		this.gbBranchesWithBalance.Controls.Add((System.Windows.Forms.Control)(object)this.grdBranchesWithBalance);
		this.gbBranchesWithBalance.Dock = System.Windows.Forms.DockStyle.Left;
		this.gbBranchesWithBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.gbBranchesWithBalance.Location = new System.Drawing.Point(1379, 0);
		this.gbBranchesWithBalance.Name = "gbBranchesWithBalance";
		this.gbBranchesWithBalance.Size = new System.Drawing.Size(245, 158);
		this.gbBranchesWithBalance.TabIndex = 119;
		this.gbBranchesWithBalance.TabStop = false;
		this.gbBranchesWithBalance.Text = "Branches With Balance";
		this.grdBranchesWithBalance.AllowEdit = (InheritableBoolean)2;
		this.grdBranchesWithBalance.AlternatingColors = true;
		this.grdBranchesWithBalance.AutomaticSort = false;
		grdBranchesWithBalance_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdBranchesWithBalance.DesignTimeLayout = grdBranchesWithBalance_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdBranchesWithBalance).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdBranchesWithBalance.DynamicFiltering = true;
		this.grdBranchesWithBalance.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdBranchesWithBalance).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBranchesWithBalance.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdBranchesWithBalance).Location = new System.Drawing.Point(3, 16);
		((System.Windows.Forms.Control)(object)this.grdBranchesWithBalance).Name = "grdBranchesWithBalance";
		((System.Windows.Forms.Control)(object)this.grdBranchesWithBalance).Size = new System.Drawing.Size(239, 139);
		((System.Windows.Forms.Control)(object)this.grdBranchesWithBalance).TabIndex = 116;
		this.grdBranchesWithBalance.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdBranchesWithBalance).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1656, 749);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "GeneralLedger";
		this.Text = "Ledgers";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GeneralLedger_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPageGeneralLedger.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabSingleAccountLedger.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel33.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDocType).EndInit();
		this.gbAccountOtherInfo.ResumeLayout(false);
		this.gbAccountOtherInfo.PerformLayout();
		this.gbAccountDetail.ResumeLayout(false);
		this.gbAccountDetail.PerformLayout();
		this.PanelAccountDetail.ResumeLayout(false);
		this.PanelAccountDetail.PerformLayout();
		this.SubsidiaryBalanceBox.ResumeLayout(false);
		this.SubsidiaryBalanceBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.rdPanel.ResumeLayout(false);
		this.rdPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabMultiAccountsLedger.ResumeLayout(false);
		this.tabMultiAccountsLedger.PerformLayout();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMultiAccountsLedger).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAccounts).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeMulti).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroupAccount).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabBankSummaryLedger.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailBankSummLedger).EndInit();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAccountsBankSummLedger).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel32.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummaryValuesBanksummLegder).EndInit();
		this.panel31.ResumeLayout(false);
		this.panel31.PerformLayout();
		this.toolStrip6.ResumeLayout(false);
		this.toolStrip6.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypesForBankSummaryLedger).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeBankSummLedger).EndInit();
		this.tabPageItemLedger.ResumeLayout(false);
		this.panel30.ResumeLayout(false);
		this.panel28.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdItemLedger).EndInit();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameItmLedger).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStockAccountItemLedger).EndInit();
		this.panel27.ResumeLayout(false);
		this.panel27.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comSupplier).EndInit();
		this.gbBranchesWithBalance.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdBranchesWithBalance).EndInit();
		base.ResumeLayout(false);
	}
}
