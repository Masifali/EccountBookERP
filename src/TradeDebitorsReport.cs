// using Architecture.Reports;
// using Architecture.WinApp.Configurations;
// using Architecture.WinApp.SMSSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.SMS;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.SMS;
using Architecture.WinApp.Common;
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

public class TradeDebitorsReport : Form
{
	private int RecId;

	private bool UpdateMode;

	public DateTime FromDate;

	public DateTime ToDate;

	public int AccountClass;

	public int CompanyIdFromBreakUp;

	private DataTable dtGroups = new DataTable();

	private DataTable dtAccount = new DataTable();

	private DataTable dtreceive = new DataTable();

	private DataTable dtfollowUp = new DataTable();

	private DataTable dtBillDetail = new DataTable();

	private DataTable GrdTableForLayout = new DataTable();

	private bool FormInitialized;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	public string BranchesIds = "";

	private List<string> SortingList = new List<string>();

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

	internal GridEX grdBillDetail;

	private Panel panel5;

	private Panel panel10;

	private Panel panel9;

	private Panel panel8;

	private ToolStrip toolStrip1;

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

	private ToolStripButton btnSMS;

	private TextBox txtBalanceTo;

	private TextBox txtBalanceFrom;

	private Label label4;

	private Label label8;

	internal UltraCombo cmbAccount;

	private TextBox txttest;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnSmSSave;

	internal UltraCombo CmbCityName;

	private Label label9;

	private ToolStripButton btn122CityWise;

	internal UltraCombo CmbCustomGroup;

	internal UltraCombo cmbCustomerGroup;

	private Label label6;

	private Label label10;

	private CheckBox ChkPostedTransaction;

	private CheckBox ChkTradeParties;

	private Panel panel4;

	private Label lblHeading;

	internal UltraCombo CmbSubsidiaryAccount;

	private Label lblSubsidiaryaccount;

	internal UltraCombo CmbParentAccount;

	private Label lblParentAccount;

	internal GridEX grd;

	private Panel panel11;

	private Label label11;

	internal GridEX grdReceiveable;

	private Label label14;

	private Label label13;

	private TabPage tabReceivablesByDueDate;

	public CheckBox ChkCreditAmount;

	public CheckBox ChkDebitAmount;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	internal UltraCombo CmbBranches;

	private Label Branches;

	internal UltraCombo CmbCostCenter;

	private Label lblCostCenter;

	private RadioButton radBalanceClassification;

	private RadioButton radAccountClassification;

	private GroupBox groupBox1;

	private RadioButton rdAll;

	private RadioButton RdAssests;

	private RadioButton RdLiability;

	private GroupBox groupBox2;

	private RadioButton radPayablesReport;

	private RadioButton radReceivablesReport;

	private ToolStripButton btnPayablesAgingReport;

	private ToolStripButton btnReceivablesAgingReport;

	private CheckBox chkClosingSorting;

	private RadioButton rdDescending;

	private RadioButton rdAscending;

	private Button btnApplySorting;

	private CheckBox chkAccountTitleSorting;

	private CheckBox chkAccountCodeSorting;

	private Label lblSortingCriteria;

	public TradeDebitorsReport(UserAccount user)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Receivables_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1060, 600);
			FormInitialized = true;
			UserAccount.CompanyId = ((!UserAccount.IsHeadOffice) ? UserAccount.CompanyId : ((CompanyIdFromBreakUp > 0) ? CompanyIdFromBreakUp : UserAccount.CompanyId));
			fromdate.Focus();
			if (AccountClass <= 0)
			{
				if (Conversion.ToString(base.Tag) == "TradeCreditorsReport")
				{
					AccountClass = 3;
				}
				else
				{
					AccountClass = 2;
				}
			}
			tabControl1.TabPages.Remove(tabReceivablesByDueDate);
			if (AccountClass == 2)
			{
				((Control)(object)grd).Name = "Receivablegrd";
				ChkDebitAmount.Checked = true;
				lblHeading.Text = "Receivables Report Accounts Classification Wise";
				if (tabControl1.TabPages.Count == 2)
				{
					tabControl1.TabPages.Add("Receivables By Due Date");
					ReceivablesByDueDatesNew frm = new ReceivablesByDueDatesNew(UserAccount);
					frm.TopLevel = false;
					frm.Visible = true;
					frm.FormBorderStyle = FormBorderStyle.None;
					frm.Dock = DockStyle.Fill;
					tabControl1.TabPages[2].Controls.Add(frm);
				}
				CostCenterFill();
				if (UserAccount.AppId == 5 && ((DataTable)((UltraGridBase)CmbCostCenter).DataSource).Rows.Count > 0)
				{
					((UltraGridBase)CmbCostCenter).Rows[0].Activate();
					((Control)(object)CmbCostCenter).Enabled = false;
				}
				radReceivablesReport.Checked = true;
			}
			else
			{
				((Control)(object)grd).Name = "Payablesgrd";
				ChkCreditAmount.Checked = true;
				lblHeading.Text = "Payables Report Accounts Classification Wise";
				lblCostCenter.Visible = false;
				((Control)(object)CmbCostCenter).Visible = false;
				radPayablesReport.Checked = true;
			}
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			AccountTitleFill();
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
			CustomerGroupBind();
			CustomeGroupsDefine();
			MultiLanguagesGetAll();
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				fromdate.Value = Conversion.ToDateTime((object)FromDate);
				todate.Value = Conversion.ToDateTime((object)ToDate);
			}
			else
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			bindBillHistoryDetail();
			AccountFill();
			GridFill();
			txtFollowupDate.Value = DateTime.Now;
			txtBalanceFrom.Text = "0";
			chkAccountTitleSorting.Checked = true;
			BindContolAccount();
			btnshow_Click(null, null);
			groupBox1.Enabled = false;
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

	private void AccountTitleFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,16,17,18", null, Conversion.ToInt(CmbCostCenter.Value), "2,3");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbParentAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CityNameFill()
	{
		DataTable dt = CommonServices.CityGetAllService();
		if (dt.Rows.Count > 0)
		{
			DDL.BindDDLNew(dt, CmbCityName, "Id", "CityName", "City Name", false);
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
			MessageBox.Show(ex.Message);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindContolAccount()
	{
		try
		{
			((UltraGridBase)cmbAccount).DataSource = null;
			int AccountClassId = (radPayablesReport.Checked ? 3 : (radReceivablesReport.Checked ? 2 : 0));
			EnumerableRowCollection<DataRow> rows = from r in dtAccount.AsEnumerable()
				where Conversion.ToInt(r["AccountClass"]) == AccountClassId
				select r;
			DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtAccount.Clone());
			if (dt.Rows.Count > 0)
			{
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("AccountTitle");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dt2.Rows.Add(dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTitle"]);
				}
				DDL.BindDDL(dt2, cmbAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbAccount).DisplayLayout.Bands[0].Columns[1].Width = 336;
				UltraGridColumn obj = ((UltraGridBase)cmbAccount).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbAccount.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbAccount.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbAccount.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbAccount.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
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
			DataTable dtCustome = CommonServices.CustomeGroupsDefine(1);
			if (dtCustome.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustome, CmbCustomGroup, "Id", "AcLookUpsDescription", "Custom Group", false);
				((UltraGridBase)CmbCustomGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindAllSubsidiaryaccounts()
	{
		try
		{
			DataTable dt = VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, 0, (string)null);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSubsidiaryAccount()
	{
		try
		{
			if (((UltraGridBase)CmbParentAccount).ActiveRow != null && ((Control)(object)CmbParentAccount).Text != string.Empty)
			{
				DataTable dt = VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, Conversion.ToInt(CmbParentAccount.Value), (string)null);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					if (dt.Rows.Count == 1)
					{
						((UltraGridBase)CmbSubsidiaryAccount).Rows[0].Activate();
					}
				}
				else
				{
					((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
					((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
				((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindParentAccount()
	{
		try
		{
			if (((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null)
			{
				CmbParentAccount.Value = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[4].Value);
				CmbParentAccount_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			BindParentAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbParentAccount_Leave(object sender, EventArgs e)
	{
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void reset()
	{
		GridSetting();
		bindBillHistoryDetail();
	}

	private void btnNew_Click_1(object sender, EventArgs e)
	{
		try
		{
			fromdate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			todate.Value = Conversion.ToDateTime((object)DateTime.Now);
			((Control)(object)CmbCostCenter).Text = string.Empty;
			txtBalanceFrom.Text = string.Empty;
			txtBalanceTo.Text = string.Empty;
			((Control)(object)cmbAccount).Text = string.Empty;
			((UltraGridBase)cmbAccount).DataSource = null;
			grd.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		try
		{
			wf.Show();
			dtreceive.Rows.Clear();
			string account = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbAccount).Text, dtAccount, "AccountTitle");
			string GroupIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbCustomerGroup).Text, dtGroups, "Description");
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			if (UserAccount.AppId == 5 && Conversion.ToInt(CmbCostCenter.Value) == 0)
			{
				throw new Exception("Cost Center Not Found");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.CwCoaId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			if (radAccountClassification.Checked)
			{
				obj.ReportTypeId = 0;
				if (radReceivablesReport.Checked)
				{
					obj.AccouuntClassId = 2;
				}
				else if (radPayablesReport.Checked)
				{
					obj.AccouuntClassId = 3;
				}
			}
			else if (radBalanceClassification.Checked)
			{
				obj.ReportTypeId = 1;
				if (radReceivablesReport.Checked)
				{
					obj.ReportType = "RECEIVABLES";
				}
				else if (radPayablesReport.Checked)
				{
					obj.ReportType = "PAYABLES";
				}
				if (rdAll.Checked)
				{
					obj.AccouuntClassId = 0;
				}
				else if (RdAssests.Checked)
				{
					obj.AccouuntClassId = 2;
				}
				else if (RdLiability.Checked)
				{
					obj.AccouuntClassId = 3;
				}
			}
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToDouble((object)txtBalanceFrom.Text);
			obj.ToDocNo = Conversion.ToDouble((object)txtBalanceTo.Text);
			obj.Status = account;
			obj.AccountTypeIds = GroupIds;
			obj.CityId = Conversion.ToInt(CmbCityName.Value);
			obj.CustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			if (ChkCreditAmount.Checked)
			{
				obj.ActionId = 1;
			}
			if (ChkDebitAmount.Checked)
			{
				obj.ActionId = 2;
			}
			if (ChkDebitAmount.Checked && ChkCreditAmount.Checked)
			{
				obj.ActionId = 0;
			}
			if (ChkTradeParties.Checked)
			{
				if (radReceivablesReport.Checked)
				{
					obj.TradeTypeId = 2;
				}
				else if (radPayablesReport.Checked)
				{
					obj.TradeTypeId = 1;
				}
			}
			if (ChkPostedTransaction.Checked)
			{
				obj.IsApproved = true;
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			obj.AccountId = Conversion.ToInt(CmbParentAccount.Value);
			obj.SubsidiaryAccountId = Conversion.ToInt(CmbSubsidiaryAccount.Value);
			obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
			obj.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			DataSet ds = VoucherReports.TradeDebtorsAndCreditors_Report(obj);
			dtreceive = ds.Tables[0].Copy();
			if (dtreceive.Rows.Count > 0)
			{
				if (dtreceive.Columns.Contains("CompLogoImage"))
				{
					dtreceive.Columns["CompLogoImage"].ColumnName = "CompLogoImage2";
				}
				dtreceive.Columns.Add("CompLogoImage", typeof(byte[]));
				foreach (DataRow row2 in dtreceive.Rows)
				{
					row2["CompLogoImage"] = ((ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows[0][0] : "");
				}
				string sortExpression = string.Join(", ", SortingList.Select((string col) => col + (rdAscending.Checked ? " ASC" : " DESC")));
				DataView view = dtreceive.DefaultView;
				view.Sort = sortExpression;
				dtreceive = view.ToTable();
				DataTable dt = new DataTable();
				dt.BeginLoadData();
				if (dt.Columns.Count == 0)
				{
					dt.Columns.Add("AccountId");
					dt.Columns.Add("AccountClassId");
					dt.Columns.Add("BranchesId", typeof(int));
					dt.Columns.Add("BranchName");
					dt.Columns.Add("AccountTitle3rdLevel");
					dt.Columns.Add("AccountClass");
					dt.Columns.Add("AccountCode");
					dt.Columns.Add("PartyCode");
					dt.Columns.Add("AccountTitle");
					dt.Columns.Add("SubsidiaryAccountId", typeof(int));
					dt.Columns.Add("AccountTitleEnglish");
					dt.Columns.Add("AccountType");
					dt.Columns.Add("Opening", typeof(double));
					dt.Columns.Add("Debit", typeof(double));
					dt.Columns.Add("Credit", typeof(double));
					dt.Columns.Add("Closing", typeof(double));
					dt.Columns.Add("Increase/Decrease", typeof(double));
					dt.Columns.Add("LastBillDate", typeof(string));
					dt.Columns.Add("LastBillAmount", typeof(double));
					dt.Columns.Add("BillDays");
					dt.Columns.Add("LastRcvdDate", typeof(string));
					dt.Columns.Add("LastRcvdAmount", typeof(double));
					dt.Columns.Add("RcvdDays");
					dt.Columns.Add("CityName");
					dt.Columns.Add("Mobile#");
					dt.Columns.Add("Title");
				}
				GrdTableForLayout = dt.Clone();
				foreach (DataRow row in dtreceive.Rows)
				{
					string lstbilldate = "";
					string lstrecdate = "";
					if (Conversion.ToDouble(row["LastBillsAmount"]) > 0.0)
					{
						lstbilldate = Conversion.ToString(row["LastBillDate"]);
					}
					if (Conversion.ToDouble(row["LastRcvdAmount"]) > 0.0)
					{
						lstrecdate = Conversion.ToString(row["LastRcvdDate"]);
					}
					DataRow newRow = dt.Rows.Add(row["AccountId"], row["AccountClassId"], row["BranchesId"], row["BranchName"], row["lvl03_Title"], row["ClassName"], row["AccountCode"], row["PartyCode"], row["AccountTitle"], row["SubsidiaryAccountId"], row["AccountTitleEnglish"], row["AccountType"], row["Opening"], row["CurrDebit"], row["CurrCredit"], Conversion.ToDouble(row["Closing"]), Conversion.ToDouble(row["Increase/Decrease"]), lstbilldate, row["LastBillsAmount"], row["BillDays"], lstrecdate, row["LastRcvdAmount"], row["RcvdDays"], row["CityName"], row["MobilePersonal"], row["Title"]);
					GrdTableForLayout.ImportRow(newRow);
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["LastBillDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LastRcvdDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["BranchesId"].Visible = false;
			grd.RootTable.Columns["AccountClassId"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grd.RootTable.Columns["AccountTitleEnglish"].Visible = false;
			grd.RootTable.Columns["PartyCode"].Visible = false;
			grd.RootTable.Columns["Title"].Visible = SubsidiaryAccountAllownOnVouchers;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grd.RootTable.Columns["AccountTitle3rdLevel"].Caption = "Parent Account";
			grd.RootTable.GroupTotals = (GroupTotals)2;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["AccountTitle3rdLevel"].Width = 145;
			grd.RootTable.Columns["AccountCode"].Width = 80;
			grd.RootTable.Columns["AccountType"].Width = 65;
			grd.RootTable.Columns["AccountClass"].Width = 75;
			grd.RootTable.Columns["PartyCode"].Width = 100;
			grd.RootTable.Columns["AccountTitle"].Width = 180;
			grd.RootTable.Columns["AccountTitleEnglish"].Width = 180;
			grd.RootTable.Columns["Opening"].Width = 102;
			grd.RootTable.Columns["Debit"].Width = 102;
			grd.RootTable.Columns["Credit"].Width = 102;
			grd.RootTable.Columns["Closing"].Width = 102;
			grd.RootTable.Columns["Increase/Decrease"].Width = 102;
			grd.RootTable.Columns["LastBillDate"].Width = 80;
			grd.RootTable.Columns["LastBillAmount"].Width = 90;
			grd.RootTable.Columns["BillDays"].Width = 50;
			grd.RootTable.Columns["LastRcvdDate"].Width = 80;
			grd.RootTable.Columns["LastRcvdAmount"].Width = 90;
			grd.RootTable.Columns["RcvdDays"].Width = 75;
			grd.RootTable.Columns["CityName"].Width = 120;
			grd.RootTable.Columns["Mobile#"].Width = 110;
			grd.RootTable.Columns["Title"].Width = 130;
			grd.RootTable.Columns["BillDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RcvdDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Increase/Decrease"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Increase/Decrease"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Increase/Decrease"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Increase/Decrease"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LastBillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LastBillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastBillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["LastBillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["LastRcvdAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LastRcvdAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LastRcvdAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["LastRcvdAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			if (AccountClass == 3)
			{
				grd.RootTable.Columns["LastRcvdDate"].Caption = "Last Paid Date";
				grd.RootTable.Columns["LastRcvdAmount"].Caption = "Last Paid Amount";
				grd.RootTable.Columns["RcvdDays"].Caption = "Paid Days";
			}
			else
			{
				grd.RootTable.Columns["LastRcvdDate"].Caption = "Last Received Date";
				grd.RootTable.Columns["LastRcvdAmount"].Caption = "Last Received Amount";
				grd.RootTable.Columns["RcvdDays"].Caption = "Received Days";
			}
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grd.RootTable.Columns["PartyCode"].Visible = true;
				grd.RootTable.Columns.Add("BreakUp");
				grd.RootTable.Columns["BreakUp"].Caption = "SubsidiaryBreakUp";
				grd.RootTable.Columns["BreakUp"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["BreakUp"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["BreakUp"].ButtonText = "SubsidiaryBreakUp";
				grd.RootTable.Columns["BreakUp"].Width = 120;
				grd.RootTable.Columns["BreakUp"].Position = 0;
				grd.FrozenColumns = 1;
				grd.RootTable.Columns["BreakUp"].Visible = false;
			}
			((Control)(object)grd).Refresh();
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindBillHistoryDetail()
	{
		try
		{
			dtBillDetail.Columns.Clear();
			dtBillDetail.Rows.Clear();
			dtBillDetail.Columns.Add("BNo", typeof(int));
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int AccountId = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			int SubsidiaryAccountId = Conversion.ToInt(currentRow.Cells["SubsidiaryAccountId"].Value);
			int branchesId = Conversion.ToInt(currentRow.Cells["BranchesId"].Value);
			CommonServices.GoToGeneralLedgerFromLinkedEvent(AccountId, fromdate.Value, todate.Value, SubsidiaryAccountId, branchesId);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "BreakUp")
			{
				SubsidiaryPayableReceiveables subsidiaryPayableReceiveables = new SubsidiaryPayableReceiveables(UserAccount);
				subsidiaryPayableReceiveables.ControlAccountId = Conversion.ToInt(grd.CurrentRow.Cells["AccountId"].Value);
				subsidiaryPayableReceiveables.PartyTypeId = ((Conversion.ToInt(grd.CurrentRow.Cells["AccountClassId"].Value) == 2) ? 2 : ((Conversion.ToInt(grd.CurrentRow.Cells["AccountClassId"].Value) == 3) ? 1 : 0));
				subsidiaryPayableReceiveables.FromDate = fromdate.Value;
				subsidiaryPayableReceiveables.ToDate = todate.Value;
				subsidiaryPayableReceiveables.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		ShowReport();
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
			val.ShowReportWithDataTable(dtreceive, "122A-AcRptAccounts_ReceivablesWithStatus.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn122CityWise_Click(object sender, EventArgs e)
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
			val.ShowReportWithDataTable(dtreceive, "122A-CityWise-AcRptAccounts_ReceivablesWithStatus.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, GrdTableForLayout);
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
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

	private void txtBalanceFrom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtBalanceFrom.SelectionStart = txtBalanceFrom.Text.Length;
			double a = Conversion.ToDouble((object)txtBalanceFrom.Text);
			txtBalanceFrom.Text = a.ToString("#,#");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBalanceTo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtBalanceTo.SelectionStart = txtBalanceTo.Text.Length;
			double a = Conversion.ToDouble((object)txtBalanceTo.Text);
			txtBalanceTo.Text = a.ToString("#,#");
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

	private void AccountFill()
	{
		try
		{
			DataTable dtGlAcc = CommonServices.CoaAllocationGetForComboServiceBind();
			if (dtGlAcc.Rows.Count > 0)
			{
				DDL.BindDDL(dtGlAcc, cmbCOA, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbCOA).DisplayLayout.Bands[0].Columns[1].Width = 276;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
						break;
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

	private void btnFollowUpRefresh_Click(object sender, EventArgs e)
	{
		FormReset();
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

	private void GridFill()
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
			dt.Columns.Add("FollowUpDate", typeof(DateTime));
			dt.Columns.Add("NF-Days");
			dt.Columns.Add("PromiseDate", typeof(DateTime));
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
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
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
			MessageBox.Show(ex.Message);
		}
	}

	private void grdReceiverList_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt((object)grdReceiveable.CurrentRow.Cells[0].Value.ToString());
			if (dtfollowUp.Rows.Count > 0)
			{
				int i = 0;
				if (i < dtfollowUp.Rows.Count && Conversion.ToInt(dtfollowUp.Rows[i]["Id"]) == Conversion.ToInt((object)RecId))
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
				accountsPayRec.EntryUser = UserAccount.ID;
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

	private void txtNextFollowupDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void TradeDebitorsReport_FormClosing(object sender, FormClosingEventArgs e)
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

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
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
		if (e.KeyCode == Keys.Escape)
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
		if (tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.L)
		{
			lblLanguage.Visible = true;
			((Control)(object)CmbLanguage).Visible = true;
		}
	}

	private void grd_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			GridEXColumn Closing = grd.RootTable.Columns["Closing"];
			if (Closing != null)
			{
				GridEXCell CutCell = e.Row.Cells[((JanusCollectionElement)Closing).Index];
				if (Conversion.ToDouble(CutCell.Value) < 0.0 && AccountClass == 2)
				{
					CutCell.FormatStyle = new GridEXFormatStyle();
					CutCell.FormatStyle.ForeColor = Color.Red;
					CutCell.FormatStyle.FontBold = (TriState)2;
				}
				else if (Conversion.ToDouble(CutCell.Value) > 0.0 && AccountClass == 3)
				{
					CutCell.FormatStyle = new GridEXFormatStyle();
					CutCell.FormatStyle.ForeColor = Color.Red;
					CutCell.FormatStyle.FontBold = (TriState)2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void radAccountClassification_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (radAccountClassification.Checked)
			{
				ChkCreditAmount.Enabled = true;
				ChkDebitAmount.Enabled = true;
				groupBox1.Enabled = false;
				if (radPayablesReport.Checked)
				{
					ChkCreditAmount.Checked = true;
					lblHeading.Text = "Payables Report Accounts Classification Wise";
				}
				else if (radReceivablesReport.Checked)
				{
					ChkDebitAmount.Checked = true;
					lblHeading.Text = "Receivables Report Accounts Classification Wise";
				}
			}
			else
			{
				if (radPayablesReport.Checked)
				{
					lblHeading.Text = "Payables Report Balance Classification Wise";
				}
				else if (radReceivablesReport.Checked)
				{
					lblHeading.Text = "Receivables Report Balance Classification Wise";
				}
				ChkCreditAmount.Enabled = false;
				ChkDebitAmount.Enabled = false;
				ChkDebitAmount.Checked = false;
				ChkCreditAmount.Checked = false;
				groupBox1.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void radPayablesReport_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (radPayablesReport.Checked)
			{
				lblHeading.Text = (radAccountClassification.Checked ? "Payables Report Accounts Classification Wise" : "Payables Report Balance Classification Wise");
			}
			else if (radReceivablesReport.Checked)
			{
				lblHeading.Text = (radAccountClassification.Checked ? "Receivables Report Accounts Classification Wise" : "Receivables Report Balance Classification Wise");
			}
			BindContolAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReceivablesAgingReport_Click(object sender, EventArgs e)
	{
		try
		{
			new Receivables_New(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPayablesAgingReport_Click(object sender, EventArgs e)
	{
		try
		{
			new PayablesAging_New(UserAccount).Show();
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
			if (dtreceive.Rows.Count > 0)
			{
				string sortExpression = string.Join(", ", SortingList.Select((string col) => col + (rdAscending.Checked ? " ASC" : " DESC")));
				lblSortingCriteria.Text = "(" + sortExpression + ")";
				DataView view = dtreceive.DefaultView;
				view.Sort = sortExpression;
				dtreceive = view.ToTable();
				DataTable dt = new DataTable();
				dt.BeginLoadData();
				if (dt.Columns.Count == 0)
				{
					dt.Columns.Add("AccountId");
					dt.Columns.Add("AccountClassId");
					dt.Columns.Add("BranchesId", typeof(int));
					dt.Columns.Add("BranchName");
					dt.Columns.Add("AccountTitle3rdLevel");
					dt.Columns.Add("AccountClass");
					dt.Columns.Add("AccountCode");
					dt.Columns.Add("PartyCode");
					dt.Columns.Add("AccountTitle");
					dt.Columns.Add("SubsidiaryAccountId", typeof(int));
					dt.Columns.Add("AccountTitleEnglish");
					dt.Columns.Add("AccountType");
					dt.Columns.Add("Opening", typeof(double));
					dt.Columns.Add("Debit", typeof(double));
					dt.Columns.Add("Credit", typeof(double));
					dt.Columns.Add("Closing", typeof(double));
					dt.Columns.Add("Increase/Decrease", typeof(double));
					dt.Columns.Add("LastBillDate", typeof(string));
					dt.Columns.Add("LastBillAmount", typeof(double));
					dt.Columns.Add("BillDays");
					dt.Columns.Add("LastRcvdDate", typeof(string));
					dt.Columns.Add("LastRcvdAmount", typeof(double));
					dt.Columns.Add("RcvdDays");
					dt.Columns.Add("CityName");
					dt.Columns.Add("Mobile#");
					dt.Columns.Add("Title");
				}
				GrdTableForLayout = dt.Clone();
				foreach (DataRow row in dtreceive.Rows)
				{
					string lstbilldate = "";
					string lstrecdate = "";
					if (Conversion.ToDouble(row["LastBillsAmount"]) > 0.0)
					{
						lstbilldate = Conversion.ToString(row["LastBillDate"]);
					}
					if (Conversion.ToDouble(row["LastRcvdAmount"]) > 0.0)
					{
						lstrecdate = Conversion.ToString(row["LastRcvdDate"]);
					}
					DataRow newRow = dt.Rows.Add(row["AccountId"], row["AccountClassId"], row["BranchesId"], row["BranchName"], row["lvl03_Title"], row["ClassName"], row["AccountCode"], row["PartyCode"], row["AccountTitle"], row["SubsidiaryAccountId"], row["AccountTitleEnglish"], row["AccountType"], row["Opening"], row["CurrDebit"], row["CurrCredit"], Conversion.ToDouble(row["Closing"]), Conversion.ToDouble(row["Increase/Decrease"]), lstbilldate, row["LastBillsAmount"], row["BillDays"], lstrecdate, row["LastRcvdAmount"], row["RcvdDays"], row["CityName"], row["MobilePersonal"], row["Title"]);
					GrdTableForLayout.ImportRow(newRow);
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
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Expected O, but got Unknown
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Expected O, but got Unknown
		//IL_0c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.TradeDebitorsReport));
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
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
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
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		GridEXLayout grdBillDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdReceiveable_DesignTimeLayout = new GridEXLayout();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.lblSortingCriteria = new System.Windows.Forms.Label();
		this.chkClosingSorting = new System.Windows.Forms.CheckBox();
		this.rdDescending = new System.Windows.Forms.RadioButton();
		this.rdAscending = new System.Windows.Forms.RadioButton();
		this.btnApplySorting = new System.Windows.Forms.Button();
		this.chkAccountTitleSorting = new System.Windows.Forms.CheckBox();
		this.chkAccountCodeSorting = new System.Windows.Forms.CheckBox();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label11 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.radPayablesReport = new System.Windows.Forms.RadioButton();
		this.radReceivablesReport = new System.Windows.Forms.RadioButton();
		this.radBalanceClassification = new System.Windows.Forms.RadioButton();
		this.radAccountClassification = new System.Windows.Forms.RadioButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.RdAssests = new System.Windows.Forms.RadioButton();
		this.RdLiability = new System.Windows.Forms.RadioButton();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.CmbSubsidiaryAccount = new UltraCombo();
		this.lblSubsidiaryaccount = new System.Windows.Forms.Label();
		this.CmbParentAccount = new UltraCombo();
		this.lblParentAccount = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.lblCostCenter = new System.Windows.Forms.Label();
		this.lblHeading = new System.Windows.Forms.Label();
		this.CmbLanguage = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.ChkPostedTransaction = new System.Windows.Forms.CheckBox();
		this.ChkTradeParties = new System.Windows.Forms.CheckBox();
		this.ChkCreditAmount = new System.Windows.Forms.CheckBox();
		this.ChkDebitAmount = new System.Windows.Forms.CheckBox();
		this.CmbCustomGroup = new UltraCombo();
		this.cmbCustomerGroup = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbCityName = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txttest = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbAccount = new UltraCombo();
		this.grdBillDetail = new GridEX();
		this.label29 = new System.Windows.Forms.Label();
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
		this.btn122CityWise = new System.Windows.Forms.ToolStripButton();
		this.btnSMS = new System.Windows.Forms.ToolStripButton();
		this.btnSmSSave = new System.Windows.Forms.ToolStripButton();
		this.btnReceivablesAgingReport = new System.Windows.Forms.ToolStripButton();
		this.btnPayablesAgingReport = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdReceiveable = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
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
		this.label13 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnFollowUpRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.tabReceivablesByDueDate = new System.Windows.Forms.TabPage();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdBillDetail).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdReceiveable).BeginInit();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCOA).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1388, 756);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabReceivablesByDueDate);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1388, 756);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1380, 727);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel2.Controls.Add(this.panel11);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1374, 721);
		this.panel2.TabIndex = 0;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 202);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1374, 519);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 129;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.FormattingRow += new RowLoadEventHandler(grd_FormattingRow);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		this.panel11.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel11.Controls.Add(this.lblSortingCriteria);
		this.panel11.Controls.Add(this.chkClosingSorting);
		this.panel11.Controls.Add(this.rdDescending);
		this.panel11.Controls.Add(this.rdAscending);
		this.panel11.Controls.Add(this.btnApplySorting);
		this.panel11.Controls.Add(this.chkAccountTitleSorting);
		this.panel11.Controls.Add(this.chkAccountCodeSorting);
		this.panel11.Controls.Add(this.ctrlGrdBar1);
		this.panel11.Controls.Add(this.label11);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 174);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1374, 28);
		this.panel11.TabIndex = 128;
		this.lblSortingCriteria.AutoSize = true;
		this.lblSortingCriteria.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSortingCriteria.ForeColor = System.Drawing.Color.White;
		this.lblSortingCriteria.Location = new System.Drawing.Point(775, 7);
		this.lblSortingCriteria.Name = "lblSortingCriteria";
		this.lblSortingCriteria.Size = new System.Drawing.Size(86, 15);
		this.lblSortingCriteria.TabIndex = 184;
		this.lblSortingCriteria.Text = "SortingCritera";
		this.chkClosingSorting.AutoSize = true;
		this.chkClosingSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkClosingSorting.ForeColor = System.Drawing.Color.White;
		this.chkClosingSorting.Location = new System.Drawing.Point(425, 5);
		this.chkClosingSorting.Name = "chkClosingSorting";
		this.chkClosingSorting.Size = new System.Drawing.Size(65, 19);
		this.chkClosingSorting.TabIndex = 183;
		this.chkClosingSorting.Text = "Closing";
		this.chkClosingSorting.UseVisualStyleBackColor = true;
		this.chkClosingSorting.CheckedChanged += new System.EventHandler(chkClosingSorting_CheckedChanged);
		this.rdDescending.AutoSize = true;
		this.rdDescending.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDescending.ForeColor = System.Drawing.Color.White;
		this.rdDescending.Location = new System.Drawing.Point(677, 5);
		this.rdDescending.Name = "rdDescending";
		this.rdDescending.Size = new System.Drawing.Size(90, 19);
		this.rdDescending.TabIndex = 182;
		this.rdDescending.Text = "Descending";
		this.rdDescending.UseVisualStyleBackColor = true;
		this.rdAscending.AutoSize = true;
		this.rdAscending.Checked = true;
		this.rdAscending.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAscending.ForeColor = System.Drawing.Color.White;
		this.rdAscending.Location = new System.Drawing.Point(593, 5);
		this.rdAscending.Name = "rdAscending";
		this.rdAscending.Size = new System.Drawing.Size(82, 19);
		this.rdAscending.TabIndex = 181;
		this.rdAscending.TabStop = true;
		this.rdAscending.Text = "Ascending";
		this.rdAscending.UseVisualStyleBackColor = true;
		this.btnApplySorting.BackColor = System.Drawing.Color.White;
		this.btnApplySorting.FlatAppearance.BorderSize = 0;
		this.btnApplySorting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApplySorting.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApplySorting.ForeColor = System.Drawing.Color.Teal;
		this.btnApplySorting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApplySorting.Location = new System.Drawing.Point(493, 3);
		this.btnApplySorting.Margin = new System.Windows.Forms.Padding(2);
		this.btnApplySorting.Name = "btnApplySorting";
		this.btnApplySorting.Size = new System.Drawing.Size(93, 23);
		this.btnApplySorting.TabIndex = 180;
		this.btnApplySorting.Text = "Apply Sorting";
		this.btnApplySorting.UseVisualStyleBackColor = false;
		this.btnApplySorting.Click += new System.EventHandler(btnApplySorting_Click);
		this.chkAccountTitleSorting.AutoSize = true;
		this.chkAccountTitleSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAccountTitleSorting.ForeColor = System.Drawing.Color.White;
		this.chkAccountTitleSorting.Location = new System.Drawing.Point(324, 5);
		this.chkAccountTitleSorting.Name = "chkAccountTitleSorting";
		this.chkAccountTitleSorting.Size = new System.Drawing.Size(100, 19);
		this.chkAccountTitleSorting.TabIndex = 179;
		this.chkAccountTitleSorting.Text = "Account Title";
		this.chkAccountTitleSorting.UseVisualStyleBackColor = true;
		this.chkAccountTitleSorting.CheckedChanged += new System.EventHandler(chkAccountTitleSorting_CheckedChanged);
		this.chkAccountCodeSorting.AutoSize = true;
		this.chkAccountCodeSorting.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAccountCodeSorting.ForeColor = System.Drawing.Color.White;
		this.chkAccountCodeSorting.Location = new System.Drawing.Point(215, 5);
		this.chkAccountCodeSorting.Name = "chkAccountCodeSorting";
		this.chkAccountCodeSorting.Size = new System.Drawing.Size(103, 19);
		this.chkAccountCodeSorting.TabIndex = 178;
		this.chkAccountCodeSorting.Text = "Account Code";
		this.chkAccountCodeSorting.UseVisualStyleBackColor = true;
		this.chkAccountCodeSorting.CheckedChanged += new System.EventHandler(chkAccountCodeSorting_CheckedChanged);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1334, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 112;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(7, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(142, 19);
		this.label11.TabIndex = 1;
		this.label11.Text = "Filtered Records";
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Controls.Add(this.radBalanceClassification);
		this.panel7.Controls.Add(this.radAccountClassification);
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel7.Controls.Add(this.Branches);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount);
		this.panel7.Controls.Add(this.lblSubsidiaryaccount);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAccount);
		this.panel7.Controls.Add(this.lblParentAccount);
		this.panel7.Controls.Add(this.panel4);
		this.panel7.Controls.Add(this.ChkPostedTransaction);
		this.panel7.Controls.Add(this.ChkTradeParties);
		this.panel7.Controls.Add(this.ChkCreditAmount);
		this.panel7.Controls.Add(this.ChkDebitAmount);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerGroup);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.txttest);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccount);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdBillDetail);
		this.panel7.Controls.Add(this.label29);
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
		this.panel7.Size = new System.Drawing.Size(1374, 145);
		this.panel7.TabIndex = 112;
		this.groupBox2.Controls.Add(this.radPayablesReport);
		this.groupBox2.Controls.Add(this.radReceivablesReport);
		this.groupBox2.Location = new System.Drawing.Point(808, 30);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(0);
		this.groupBox2.Size = new System.Drawing.Size(97, 50);
		this.groupBox2.TabIndex = 16;
		this.groupBox2.TabStop = false;
		this.radPayablesReport.AutoSize = true;
		this.radPayablesReport.Checked = true;
		this.radPayablesReport.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radPayablesReport.ForeColor = System.Drawing.Color.Black;
		this.radPayablesReport.Location = new System.Drawing.Point(4, 11);
		this.radPayablesReport.Name = "radPayablesReport";
		this.radPayablesReport.Size = new System.Drawing.Size(71, 19);
		this.radPayablesReport.TabIndex = 0;
		this.radPayablesReport.TabStop = true;
		this.radPayablesReport.Text = "Payables";
		this.radPayablesReport.UseVisualStyleBackColor = true;
		this.radPayablesReport.CheckedChanged += new System.EventHandler(radPayablesReport_CheckedChanged);
		this.radReceivablesReport.AutoSize = true;
		this.radReceivablesReport.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radReceivablesReport.ForeColor = System.Drawing.Color.Black;
		this.radReceivablesReport.Location = new System.Drawing.Point(4, 28);
		this.radReceivablesReport.Name = "radReceivablesReport";
		this.radReceivablesReport.Size = new System.Drawing.Size(86, 19);
		this.radReceivablesReport.TabIndex = 1;
		this.radReceivablesReport.Text = "Receivables";
		this.radReceivablesReport.UseVisualStyleBackColor = true;
		this.radBalanceClassification.AutoSize = true;
		this.radBalanceClassification.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radBalanceClassification.Location = new System.Drawing.Point(664, 120);
		this.radBalanceClassification.Name = "radBalanceClassification";
		this.radBalanceClassification.Size = new System.Drawing.Size(138, 19);
		this.radBalanceClassification.TabIndex = 15;
		this.radBalanceClassification.Text = "Balance Classification";
		this.radBalanceClassification.UseVisualStyleBackColor = true;
		this.radAccountClassification.AutoSize = true;
		this.radAccountClassification.Checked = true;
		this.radAccountClassification.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radAccountClassification.Location = new System.Drawing.Point(664, 101);
		this.radAccountClassification.Name = "radAccountClassification";
		this.radAccountClassification.Size = new System.Drawing.Size(142, 19);
		this.radAccountClassification.TabIndex = 14;
		this.radAccountClassification.TabStop = true;
		this.radAccountClassification.Text = "Account Classification";
		this.radAccountClassification.UseVisualStyleBackColor = true;
		this.radAccountClassification.CheckedChanged += new System.EventHandler(radAccountClassification_CheckedChanged);
		this.groupBox1.Controls.Add(this.rdAll);
		this.groupBox1.Controls.Add(this.RdAssests);
		this.groupBox1.Controls.Add(this.RdLiability);
		this.groupBox1.Location = new System.Drawing.Point(808, 72);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(97, 64);
		this.groupBox1.TabIndex = 17;
		this.groupBox1.TabStop = false;
		this.rdAll.AutoSize = true;
		this.rdAll.Checked = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.ForeColor = System.Drawing.Color.Black;
		this.rdAll.Location = new System.Drawing.Point(6, 10);
		this.rdAll.Margin = new System.Windows.Forms.Padding(0);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(45, 19);
		this.rdAll.TabIndex = 0;
		this.rdAll.TabStop = true;
		this.rdAll.Text = "ALL";
		this.rdAll.UseVisualStyleBackColor = true;
		this.RdAssests.AutoSize = true;
		this.RdAssests.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdAssests.ForeColor = System.Drawing.Color.Black;
		this.RdAssests.Location = new System.Drawing.Point(6, 26);
		this.RdAssests.Margin = new System.Windows.Forms.Padding(0);
		this.RdAssests.Name = "RdAssests";
		this.RdAssests.Size = new System.Drawing.Size(58, 19);
		this.RdAssests.TabIndex = 1;
		this.RdAssests.Text = "Assets";
		this.RdAssests.UseVisualStyleBackColor = true;
		this.RdLiability.AutoSize = true;
		this.RdLiability.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdLiability.ForeColor = System.Drawing.Color.Black;
		this.RdLiability.Location = new System.Drawing.Point(6, 42);
		this.RdLiability.Margin = new System.Windows.Forms.Padding(0);
		this.RdLiability.Name = "RdLiability";
		this.RdLiability.Size = new System.Drawing.Size(66, 19);
		this.RdLiability.TabIndex = 2;
		this.RdLiability.Text = "Liability";
		this.RdLiability.UseVisualStyleBackColor = true;
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(424, 113);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 9;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(329, 119);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 134;
		this.Branches.Text = "Branches";
		this.CmbSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Location = new System.Drawing.Point(436, 183);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Name = "CmbSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Visible = false;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Leave += new System.EventHandler(CmbSubsidiaryAccount_TextChanged);
		this.lblSubsidiaryaccount.AutoSize = true;
		this.lblSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccount.Location = new System.Drawing.Point(325, 189);
		this.lblSubsidiaryaccount.Name = "lblSubsidiaryaccount";
		this.lblSubsidiaryaccount.Size = new System.Drawing.Size(110, 15);
		this.lblSubsidiaryaccount.TabIndex = 131;
		this.lblSubsidiaryaccount.Text = "Subsidiary Account";
		this.lblSubsidiaryaccount.Visible = false;
		this.CmbParentAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbParentAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Location = new System.Drawing.Point(100, 86);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Name = "CmbParentAccount";
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Leave += new System.EventHandler(CmbParentAccount_Leave);
		this.lblParentAccount.AutoSize = true;
		this.lblParentAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblParentAccount.Location = new System.Drawing.Point(3, 92);
		this.lblParentAccount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblParentAccount.Name = "lblParentAccount";
		this.lblParentAccount.Size = new System.Drawing.Size(78, 15);
		this.lblParentAccount.TabIndex = 130;
		this.lblParentAccount.Text = "Account Title";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.panel4.Controls.Add(this.lblCostCenter);
		this.panel4.Controls.Add(this.lblHeading);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.panel4.Controls.Add(this.lblLanguage);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1374, 28);
		this.panel4.TabIndex = 127;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(491, 0);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 30;
		this.lblCostCenter.AutoSize = true;
		this.lblCostCenter.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCostCenter.ForeColor = System.Drawing.Color.White;
		this.lblCostCenter.Location = new System.Drawing.Point(414, 6);
		this.lblCostCenter.Name = "lblCostCenter";
		this.lblCostCenter.Size = new System.Drawing.Size(72, 15);
		this.lblCostCenter.TabIndex = 31;
		this.lblCostCenter.Text = "Cost Center";
		this.lblHeading.AutoSize = true;
		this.lblHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeading.ForeColor = System.Drawing.Color.White;
		this.lblHeading.Location = new System.Drawing.Point(5, 4);
		this.lblHeading.Name = "lblHeading";
		this.lblHeading.Size = new System.Drawing.Size(402, 19);
		this.lblHeading.TabIndex = 1;
		this.lblHeading.Text = "Receivables Report Accounts Classification Wise";
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(1252, 1);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(121, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 131;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.lblLanguage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLanguage.ForeColor = System.Drawing.Color.White;
		this.lblLanguage.Location = new System.Drawing.Point(1190, 7);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(60, 15);
		this.lblLanguage.TabIndex = 132;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		this.ChkPostedTransaction.AutoSize = true;
		this.ChkPostedTransaction.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPostedTransaction.Location = new System.Drawing.Point(664, 83);
		this.ChkPostedTransaction.Name = "ChkPostedTransaction";
		this.ChkPostedTransaction.Size = new System.Drawing.Size(147, 19);
		this.ChkPostedTransaction.TabIndex = 13;
		this.ChkPostedTransaction.Text = "Approved Transactions";
		this.ChkPostedTransaction.UseVisualStyleBackColor = true;
		this.ChkTradeParties.AutoSize = true;
		this.ChkTradeParties.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkTradeParties.Location = new System.Drawing.Point(664, 66);
		this.ChkTradeParties.Name = "ChkTradeParties";
		this.ChkTradeParties.Size = new System.Drawing.Size(93, 19);
		this.ChkTradeParties.TabIndex = 12;
		this.ChkTradeParties.Text = "Trade Parties";
		this.ChkTradeParties.UseVisualStyleBackColor = true;
		this.ChkCreditAmount.AutoSize = true;
		this.ChkCreditAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkCreditAmount.Location = new System.Drawing.Point(664, 31);
		this.ChkCreditAmount.Name = "ChkCreditAmount";
		this.ChkCreditAmount.Size = new System.Drawing.Size(129, 19);
		this.ChkCreditAmount.TabIndex = 10;
		this.ChkCreditAmount.Text = "Only Credit Balance";
		this.ChkCreditAmount.UseVisualStyleBackColor = true;
		this.ChkDebitAmount.AutoSize = true;
		this.ChkDebitAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkDebitAmount.Location = new System.Drawing.Point(664, 48);
		this.ChkDebitAmount.Name = "ChkDebitAmount";
		this.ChkDebitAmount.Size = new System.Drawing.Size(127, 19);
		this.ChkDebitAmount.TabIndex = 11;
		this.ChkDebitAmount.Text = "Only Debit Balance";
		this.ChkDebitAmount.UseVisualStyleBackColor = true;
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(100, 113);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 4;
		this.cmbCustomerGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Location = new System.Drawing.Point(424, 32);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Name = "cmbCustomerGroup";
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(329, 38);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(94, 15);
		this.label6.TabIndex = 126;
		this.label6.Text = "Inventory Group";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 119);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(84, 15);
		this.label10.TabIndex = 125;
		this.label10.Text = "Custom Group";
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(424, 59);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 6;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(329, 65);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(62, 15);
		this.label9.TabIndex = 122;
		this.label9.Text = "City Name";
		this.txttest.Location = new System.Drawing.Point(436, 215);
		this.txttest.Name = "txttest";
		this.txttest.Size = new System.Drawing.Size(563, 23);
		this.txttest.TabIndex = 120;
		this.txttest.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(3, 65);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(94, 15);
		this.label8.TabIndex = 119;
		this.label8.Text = "Control Account";
		this.cmbAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Location = new System.Drawing.Point(100, 59);
		((System.Windows.Forms.Control)(object)this.cmbAccount).Name = "cmbAccount";
		((System.Windows.Forms.Control)(object)this.cmbAccount).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccount).TabIndex = 2;
		this.grdBillDetail.AllowDelete = (InheritableBoolean)1;
		grdBillDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdBillDetail.DesignTimeLayout = grdBillDetail_DesignTimeLayout;
		this.grdBillDetail.DynamicFiltering = true;
		this.grdBillDetail.FilterMode = (FilterMode)2;
		this.grdBillDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBillDetail.GroupByBoxVisible = false;
		this.grdBillDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Location = new System.Drawing.Point(101, 268);
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Name = "grdBillDetail";
		((ScrollableBase)this.grdBillDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Size = new System.Drawing.Size(330, 53);
		((System.Windows.Forms.Control)(object)this.grdBillDetail).TabIndex = 45;
		this.grdBillDetail.TotalRow = (InheritableBoolean)1;
		this.grdBillDetail.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Visible = false;
		((ScrollableBase)this.grdBillDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdBillDetail).Leave += new System.EventHandler(txtNextFollowupDays_TextChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(1158, 8);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(59, 15);
		this.label29.TabIndex = 106;
		this.label29.Text = "Company";
		this.label29.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(329, 84);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 30);
		this.label4.TabIndex = 114;
		this.label4.Text = "Closing From &&\r\n To";
		this.txtBalanceTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceTo.Location = new System.Drawing.Point(541, 88);
		this.txtBalanceTo.Name = "txtBalanceTo";
		this.txtBalanceTo.Size = new System.Drawing.Size(117, 23);
		this.txtBalanceTo.TabIndex = 8;
		this.txtBalanceTo.TextChanged += new System.EventHandler(txtBalanceTo_TextChanged);
		this.txtBalanceTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceTo_KeyPress);
		this.txtBalanceFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceFrom.Location = new System.Drawing.Point(424, 88);
		this.txtBalanceFrom.Name = "txtBalanceFrom";
		this.txtBalanceFrom.Size = new System.Drawing.Size(115, 23);
		this.txtBalanceFrom.TabIndex = 7;
		this.txtBalanceFrom.TextChanged += new System.EventHandler(txtBalanceFrom_TextChanged);
		this.txtBalanceFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalanceFrom_KeyPress);
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.Location = new System.Drawing.Point(906, 107);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 28);
		this.btnshow.TabIndex = 18;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(213, 34);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(112, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(100, 34);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(112, 23);
		this.fromdate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(3, 38);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From && To Date";
		this.panel3.Controls.Add(this.toolStrip3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1374, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.print, this.btn122CityWise, this.btnSMS, this.btnSmSSave, this.btnReceivablesAgingReport, this.btnPayablesAgingReport });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1374, 27);
		this.toolStrip3.TabIndex = 111;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStrip3.Click += new System.EventHandler(btnNew_Click);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click_1);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "122-Print";
		this.print.Click += new System.EventHandler(print_Click);
		this.btn122CityWise.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn122CityWise.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn122CityWise.Name = "btn122CityWise";
		this.btn122CityWise.Size = new System.Drawing.Size(119, 24);
		this.btn122CityWise.Text = "122-CityWise";
		this.btn122CityWise.Click += new System.EventHandler(btn122CityWise_Click);
		this.btnSMS.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.btnSMS.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSMS.Name = "btnSMS";
		this.btnSMS.Size = new System.Drawing.Size(120, 24);
		this.btnSMS.Text = "SMSTemplate";
		this.btnSMS.Visible = false;
		this.btnSMS.Click += new System.EventHandler(btnSMS_Click);
		this.btnSmSSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSmSSave.Name = "btnSmSSave";
		this.btnSmSSave.Size = new System.Drawing.Size(40, 24);
		this.btnSmSSave.Text = "SMS";
		this.btnSmSSave.Visible = false;
		this.btnSmSSave.Click += new System.EventHandler(btnSaveSMS_Click);
		this.btnReceivablesAgingReport.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.btnReceivablesAgingReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReceivablesAgingReport.Name = "btnReceivablesAgingReport";
		this.btnReceivablesAgingReport.Size = new System.Drawing.Size(197, 24);
		this.btnReceivablesAgingReport.Text = "Receivables Aging Report";
		this.btnReceivablesAgingReport.Click += new System.EventHandler(btnReceivablesAgingReport_Click);
		this.btnPayablesAgingReport.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.btnPayablesAgingReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPayablesAgingReport.Name = "btnPayablesAgingReport";
		this.btnPayablesAgingReport.Size = new System.Drawing.Size(178, 24);
		this.btnPayablesAgingReport.Text = "Payables Aging Report";
		this.btnPayablesAgingReport.Click += new System.EventHandler(btnPayablesAgingReport_Click);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1380, 727);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Follow Up";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdReceiveable);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.panel9);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1374, 721);
		this.panel5.TabIndex = 0;
		this.grdReceiveable.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdReceiveable).BackColor = System.Drawing.Color.White;
		grdReceiveable_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdReceiveable.DesignTimeLayout = grdReceiveable_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdReceiveable.FilterMode = (FilterMode)2;
		this.grdReceiveable.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdReceiveable.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Location = new System.Drawing.Point(0, 185);
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Name = "grdReceiveable";
		((ScrollableBase)this.grdReceiveable).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).Size = new System.Drawing.Size(1374, 536);
		((System.Windows.Forms.Control)(object)this.grdReceiveable).TabIndex = 1;
		this.grdReceiveable.TotalRow = (InheritableBoolean)1;
		this.grdReceiveable.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdReceiveable).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdReceiveable).DoubleClick += new System.EventHandler(grdReceiverList_DoubleClick);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label14);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 161);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1374, 24);
		this.panel10.TabIndex = 2;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(8, 2);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(67, 19);
		this.label14.TabIndex = 122;
		this.label14.Text = "History";
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
		this.panel9.Location = new System.Drawing.Point(0, 55);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1374, 106);
		this.panel9.TabIndex = 0;
		this.txtNextFollowupDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNextFollowupDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNextFollowupDays.Location = new System.Drawing.Point(565, 31);
		this.txtNextFollowupDays.Name = "txtNextFollowupDays";
		this.txtNextFollowupDays.Size = new System.Drawing.Size(173, 23);
		this.txtNextFollowupDays.TabIndex = 2;
		this.txtNextFollowupDays.TextChanged += new System.EventHandler(txtNextFollowupDays_TextChanged);
		this.txtNextFollowupDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNextFollowupDays_KeyPress);
		this.txtPromiseDate.CustomFormat = "dd-MMM-yyyy";
		this.txtPromiseDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPromiseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPromiseDate.Location = new System.Drawing.Point(741, 31);
		this.txtPromiseDate.Name = "txtPromiseDate";
		this.txtPromiseDate.Size = new System.Drawing.Size(173, 23);
		this.txtPromiseDate.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(565, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(117, 15);
		this.label1.TabIndex = 124;
		this.label1.Text = "Next Follow Up Days";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(741, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 123;
		this.label3.Text = "Promise Date";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(12, 60);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(94, 15);
		this.label12.TabIndex = 122;
		this.label12.Text = "Comment Detail";
		this.txtCommentDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommentDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommentDetail.Location = new System.Drawing.Point(12, 77);
		this.txtCommentDetail.Name = "txtCommentDetail";
		this.txtCommentDetail.Size = new System.Drawing.Size(902, 23);
		this.txtCommentDetail.TabIndex = 4;
		this.txtFollowupDate.CustomFormat = "dd-MMM-yyyy";
		this.txtFollowupDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFollowupDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFollowupDate.Location = new System.Drawing.Point(389, 31);
		this.txtFollowupDate.Name = "txtFollowupDate";
		this.txtFollowupDate.Size = new System.Drawing.Size(173, 23);
		this.txtFollowupDate.TabIndex = 1;
		this.txtFollowupDate.Value = new System.DateTime(2020, 11, 11, 22, 22, 0, 0);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(389, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(88, 15);
		this.label2.TabIndex = 121;
		this.label2.Text = "Follow Up Date";
		this.cmbCOA.AllowNull = (DefaultableBoolean)1;
		this.cmbCOA.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCOA.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCOA.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbCOA).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCOA).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCOA).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCOA).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbCOA).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCOA).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbCOA).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCOA).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCOA).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbCOA).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCOA).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCOA).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCOA).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCOA.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCOA).Location = new System.Drawing.Point(12, 30);
		((System.Windows.Forms.Control)(object)this.cmbCOA).Name = "cmbCOA";
		((System.Windows.Forms.Control)(object)this.cmbCOA).Size = new System.Drawing.Size(374, 26);
		((System.Windows.Forms.Control)(object)this.cmbCOA).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbCOA).TextChanged += new System.EventHandler(cmbCOA_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(12, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 15);
		this.label7.TabIndex = 120;
		this.label7.Text = "Account Title";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label13);
		this.panel8.Controls.Add(this.toolStrip1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1374, 55);
		this.panel8.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 31);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(189, 19);
		this.label13.TabIndex = 121;
		this.label13.Text = "Account FollowUp       ";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnFollowUpRefresh, this.btnsave, this.toolStripSeparator, this.btnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1374, 27);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.btnFollowUpRefresh.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnFollowUpRefresh.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnFollowUpRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFollowUpRefresh.Name = "btnFollowUpRefresh";
		this.btnFollowUpRefresh.Size = new System.Drawing.Size(59, 24);
		this.btnFollowUpRefresh.Text = "&New";
		this.btnFollowUpRefresh.Click += new System.EventHandler(btnFollowUpRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.tabReceivablesByDueDate.Location = new System.Drawing.Point(4, 4);
		this.tabReceivablesByDueDate.Name = "tabReceivablesByDueDate";
		this.tabReceivablesByDueDate.Size = new System.Drawing.Size(1380, 727);
		this.tabReceivablesByDueDate.TabIndex = 2;
		this.tabReceivablesByDueDate.Text = "Receivables By Due Date";
		this.tabReceivablesByDueDate.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1388, 756);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "TradeDebitorsReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(TradeDebitorsReport_FormClosing);
		base.Load += new System.EventHandler(Receivables_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Receivables_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdBillDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdReceiveable).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
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

}
